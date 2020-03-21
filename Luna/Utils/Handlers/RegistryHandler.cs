using Luna.Utils.Logger;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Management;
using System.Security.Principal;

namespace Luna.Utils.Handlers
{
    public enum WindowsTheme
    {
        Light,
        Dark
    }

    public static class RegistryHandler
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        private static readonly string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        public static void SetAppsUseLightTheme(bool on)
        {
            Logger.Debug("Set 'AppsUseLightTheme' to {0}", on);

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyPath, true);
                key.SetValue("AppsUseLightTheme", on ? 1 : 0, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        public static void SetSystemUsesLightTheme(bool on)
        {
            Logger.Debug("Set 'SystemUsesLightTheme' to {0}", on);

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyPath, true);
                key.SetValue("SystemUsesLightTheme", on ? 1 : 0, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        public static WindowsTheme GetAppTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyPath))
            {
                object registryValueObject = key?.GetValue("AppsUseLightTheme");

                if (registryValueObject == null)
                {
                    return WindowsTheme.Light;
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
            }
        }

        public delegate void WatchAppThemeCallback(WindowsTheme theme);

        public static void WatchAppTheme(WatchAppThemeCallback callback)
        {
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();

            string query = string.Format(CultureInfo.InvariantCulture, @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'", currentUser.User.Value, KeyPath.Replace(@"\", @"\\"), "AppsUseLightTheme");

            try
            {
                ManagementEventWatcher watcher = new ManagementEventWatcher(query);

                watcher.EventArrived += (sender, args) =>
                {
                    callback(GetAppTheme());
                };

                watcher.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            callback(GetAppTheme());
        }
    }
}
