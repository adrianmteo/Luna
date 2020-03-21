using Luna.Models;

namespace Luna.Utils.Handlers
{
    public class AppearanceHandler
    {
        public SettingsModel Model { get; private set; }

        public AppearanceHandler(SettingsModel model)
        {
            Model = model;
        }

        public void SwitchToLightTheme()
        {
            switch (Model.ChangeType)
            {
                case SettingsChangeType.Custom:
                    if (Model.ChangeAppTheme) RegistryHandler.SetAppsUseLightTheme(true);
                    if (Model.ChangeSystemTheme) RegistryHandler.SetSystemUsesLightTheme(true);
                    if (Model.ChangeWallpaper) WallpaperHandler.ChangeWallpaper(Model.LightWallpaperPath);
                    break;

                case SettingsChangeType.Theme:
                    ThemeHandler.ChangeTheme(Model.LightThemePath);
                    break;

                default:
                    break;
            }
        }

        public void SwitchToDarkTheme()
        {
            switch (Model.ChangeType)
            {
                case SettingsChangeType.Custom:
                    if (Model.ChangeAppTheme) RegistryHandler.SetAppsUseLightTheme(false);
                    if (Model.ChangeSystemTheme) RegistryHandler.SetSystemUsesLightTheme(false);
                    if (Model.ChangeWallpaper) WallpaperHandler.ChangeWallpaper(Model.DarkWallpaperPath);
                    break;

                case SettingsChangeType.Theme:
                    ThemeHandler.ChangeTheme(Model.DarkThemePath);
                    break;

                default:
                    break;
            }
        }
    }
}
