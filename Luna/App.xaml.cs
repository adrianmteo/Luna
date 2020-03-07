using Luna.Helpers;
using Luna.Properties;
using System;
using System.Windows;

namespace Luna
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "/light":
                            SwitchToLightTheme();
                            break;

                        case "/dark":
                            SwitchToDarkTheme();
                            break;

                        case "/clean":
                            TaskHandler.DeleteTasks();
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }
        public static void SwitchToLightTheme()
        {
            if (Settings.Default.ChangeAppTheme) RegistryHandler.AppsUseLightTheme(true);
            if (Settings.Default.ChangeSystemTheme) RegistryHandler.SystemUsesLightTheme(true);
            if (Settings.Default.ChangeWallpaper) WallpaperHandler.ChangeWallpaper(Settings.Default.LightWallpaperPath);
        }

        public static void SwitchToDarkTheme()
        {
            if (Settings.Default.ChangeAppTheme) RegistryHandler.AppsUseLightTheme(false);
            if (Settings.Default.ChangeSystemTheme) RegistryHandler.SystemUsesLightTheme(false);
            if (Settings.Default.ChangeWallpaper) WallpaperHandler.ChangeWallpaper(Settings.Default.DarkWallpaperPath);
        }
    }
}
