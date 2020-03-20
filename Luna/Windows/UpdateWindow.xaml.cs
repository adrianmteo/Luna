using Luna.Utils;
using System.Windows;

namespace Luna.Windows
{
    public partial class UpdateWindow : Window
    {
        public AutoUpdater AutoUpdater { get; private set; }

        public UpdateWindow(AutoUpdater autoUpdater)
        {
            InitializeComponent();

            AutoUpdater = autoUpdater;

            DataContext = autoUpdater.Model;

            if (autoUpdater.Model.Status == Models.UpdateStatus.None)
            {
                _ = autoUpdater.CheckForUpdates();
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            _ = AutoUpdater.CheckForUpdates(true);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            _ = AutoUpdater.DownloadUpdate();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            AutoUpdater.InstallUpdate();
        }
    }
}
