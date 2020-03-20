using Luna.Models;
using Luna.Properties;
using Luna.Utils;
using Luna.Utils.Handlers;
using Luna.Utils.Logger;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace Luna.Windows
{
    public partial class SettingsWindow : Window
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        private readonly AutoFileSaver<SettingsModel> _autoFileSaver = new AutoFileSaver<SettingsModel>("settings.xml");

        private readonly AutoUpdater _autoUpdater = new AutoUpdater();

        public SettingsWindow()
        {
            InitializeComponent();

            MigrateSettingsModel();

            DataContext = _autoFileSaver.Model;
            Header.DataContext = _autoUpdater.Model;

            _ = _autoUpdater.CheckForUpdates(false);

            _autoFileSaver.Model.PropertyChanged += Model_PropertyChanged;
        }

        private void MigrateSettingsModel()
        {
            if (!_autoFileSaver.FoundOnDisk)
            {
                Logger.Warning("Running settings migration...");

                Settings.Default.Upgrade();
                Settings.Default.Save();

                Logger.Warning("Settings upgrade done");

                _autoFileSaver.Model.Enabled = Settings.Default.Enabled;
                _autoFileSaver.Model.ChangeSystemTheme = Settings.Default.ChangeSystemTheme;
                _autoFileSaver.Model.ChangeAppTheme = Settings.Default.ChangeAppTheme;
                _autoFileSaver.Model.ChangeWallpaper = Settings.Default.ChangeWallpaper;
                _autoFileSaver.Model.LightWallpaperPath = Settings.Default.LightWallpaperPath;
                _autoFileSaver.Model.DarkWallpaperPath = Settings.Default.DarkWallpaperPath;
                _autoFileSaver.Model.ChangeType = SettingsChangeType.Custom;

                _autoFileSaver.Model.LightThemeTime = new DateTime(1, 1, 1, Settings.Default.LightThemeHour, Settings.Default.LightThemeMinute, 0);
                _autoFileSaver.Model.DarkThemeTime = new DateTime(1, 1, 1, Settings.Default.DarkThemeHour + 12, Settings.Default.DarkThemeMinute, 0);

                Logger.Warning("Settings migration done");
            }
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SettingsModel model = (SettingsModel)sender;

            List<string> properties = new List<string>() { "Enabled", "LightThemeTime", "DarkThemeTime" };

            if (properties.Contains(e.PropertyName))
            {
                try
                {
                    if (!model.Enabled)
                    {
                        TaskSchedulerHandler.DeleteAllTasks();
                    }
                    else
                    {
                        TaskSchedulerHandler.UpdateAllTasks(model.LightThemeTime, model.DarkThemeTime);
                    }
                }
                catch
                {
                    new MessageWindow(this, "An error occurred", "There was an error while writing to TaskScheduler. Please check logs for more info.", null, "Close").ShowDialog();
                }
            }
        }

        private void WindowHeader_OnClickUpdate(object sender, RoutedEventArgs e)
        {
            OpenUpdateWindow();
        }

        private void OpenUpdateWindow()
        {
            UpdateWindow window = new UpdateWindow(_autoUpdater)
            {
                Owner = this
            };

            window.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_autoUpdater.Model.Status >= UpdateStatus.NewUpdate && new MessageWindow(this, "Update available", $"An update to version {_autoUpdater.Model.Version} is now available. Do you want to download and install it now?", "Update now").ShowDialog() == true)
            {
                e.Cancel = true;

                _autoUpdater.AutoUpdate = true;

                switch (_autoUpdater.Model.Status)
                {
                    case UpdateStatus.NewUpdate:
                        _ = _autoUpdater.DownloadUpdate();
                        break;

                    case UpdateStatus.Ready:
                        _autoUpdater.InstallUpdate();
                        break;

                    default:
                        break;
                }

                OpenUpdateWindow();
            }
        }

        private void BrowseThemeHyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;

            OpenFileDialog dialog = new OpenFileDialog();

            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string initialPath = Path.Combine(localAppDataPath, @"Microsoft\Windows\Themes");

            dialog.Filter = "Theme files|*.theme";
            dialog.Title = "Select a theme";
            dialog.InitialDirectory = initialPath;

            if (dialog.ShowDialog() == true)
            {
                PropertyInfo propertyInfo = _autoFileSaver.Model.GetType().GetProperty((string)hyperlink.Tag);
                propertyInfo.SetValue(_autoFileSaver.Model, dialog.FileName, null);
            }
        }

        private void BrowseWallpaperHyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;

            OpenFileDialog dialog = new OpenFileDialog();

            string initialPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            dialog.Filter = "Image files|*.jpg;*.jpeg;*.png|All files|*.*";
            dialog.Title = "Select a wallpaper";
            dialog.InitialDirectory = initialPath;

            if (dialog.ShowDialog() == true)
            {
                PropertyInfo propertyInfo = _autoFileSaver.Model.GetType().GetProperty((string)hyperlink.Tag);
                propertyInfo.SetValue(_autoFileSaver.Model, dialog.FileName, null);
            }
        }

        private void StartLightThemeButton_Click(object sender, RoutedEventArgs e)
        {
            AppearanceHandler handler = new AppearanceHandler(_autoFileSaver.Model);
            handler.SwitchToLightTheme();
        }

        private void StartDarkThemeButton_Click(object sender, RoutedEventArgs e)
        {
            AppearanceHandler handler = new AppearanceHandler(_autoFileSaver.Model);
            handler.SwitchToDarkTheme();
        }
    }
}
