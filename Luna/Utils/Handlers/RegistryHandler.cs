using Luna.Utils.Logger;
using Microsoft.Win32;
using System;

namespace Luna.Utils.Handlers
{
    public class RegistryHandler
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
    }
}
