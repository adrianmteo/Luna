using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darky.Helpers
{
    class RegistryHandler
    {
        public static void AppsUseLightTheme(bool enabled)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

            if (key != null)
            {
                key.SetValue("AppsUseLightTheme", enabled ? 1 : 0, RegistryValueKind.DWord);
            }
        }

        public static void SystemUsesLightTheme(bool enabled)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

            if (key != null)
            {
                key.SetValue("SystemUsesLightTheme", enabled ? 1 : 0, RegistryValueKind.DWord);
            }
        }
    }
}
