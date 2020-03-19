using Luna.Models;
using Luna.Utils;
using Luna.Utils.Handlers;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace Luna.Windows
{
    public partial class SettingsWindow : Window
    {
        private readonly AutoFileSaver<SettingsModel> _autoFileSaver = new AutoFileSaver<SettingsModel>("settings.xml");

        private readonly AutoUpdater _autoUpdater = new AutoUpdater();

        public SettingsWindow()
        {
            InitializeComponent();

            DataContext = _autoFileSaver.Model;
            Header.DataContext = _autoUpdater.Model;

            _autoUpdater.CheckForUpdates(false);
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
                        _autoUpdater.DownloadUpdate();
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
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

        private void Hyperlink2_Click(object sender, RoutedEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppearanceHandler handler = new AppearanceHandler(_autoFileSaver.Model);
            handler.SwitchToLightTheme();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AppearanceHandler handler = new AppearanceHandler(_autoFileSaver.Model);
            handler.SwitchToDarkTheme();
        }
    }
}
