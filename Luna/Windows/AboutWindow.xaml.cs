using Luna.Utils;
using Luna.Utils.Logger;
using System;
using System.Diagnostics;
using System.Windows;

namespace Luna.Windows
{
    public partial class AboutWindow : Window
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        private readonly AutoUpdater _autoUpdater;

        public AboutWindow(AutoUpdater autoUpdater)
        {
            InitializeComponent();

            _autoUpdater = autoUpdater;

            VersionText.Text = autoUpdater.LocalVersion.ToString();
            LastCheckText.Text = autoUpdater.Model.LastCheck.ToString();
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            Close();

            _ = _autoUpdater.CheckForUpdates(true);

            Window window = new UpdateWindow(_autoUpdater) { Owner = Owner };
            window.ShowDialog();
        }

        private void OpenLink(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            OpenLink("https://github.com/adrianmteo");
        }

        private void Hyperlink1_Click(object sender, RoutedEventArgs e)
        {
            OpenLink("https://github.com/adrianmteo/Luna");
        }

        private void Hyperlink2_Click(object sender, RoutedEventArgs e)
        {
            OpenLink("https://github.com/adrianmteo/Luna/issues");
        }
    }
}
