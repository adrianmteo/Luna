using Luna.Helpers;
using Luna.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Luna
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Settings.Default.PropertyChanged += Settings_PropertyChanged;

            DataContext = Settings.Default;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Settings.Default.Save();

            try
            {
                if (e.PropertyName == "Enabled")
                {
                    if (Settings.Default.Enabled)
                    {
                        TaskHandler.UpdateTasks(Settings.Default.LightThemeHour, Settings.Default.LightThemeMinute, Settings.Default.DarkThemeHour, Settings.Default.DarkThemeMinute);
                    }
                    else
                    {
                        TaskHandler.DeleteTasks();
                    }
                }
                else if (e.PropertyName.EndsWith("Hour") || e.PropertyName.EndsWith("Minute"))
                {
                    TaskHandler.UpdateTasks(Settings.Default.LightThemeHour, Settings.Default.LightThemeMinute, Settings.Default.DarkThemeHour, Settings.Default.DarkThemeMinute);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string BrowseWallpaper(string title)
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                Title = title,
                Filter = "Images|*.jpg;*.jpeg;*.png"
            };

            if (open.ShowDialog() == true)
            {
                return open.FileName;
            }

            return null;
        }

        private void ButtonBrowseLightWallpaper_Click(object sender, RoutedEventArgs e)
        {
            string path = BrowseWallpaper("Browse light theme wallpaper");

            if (!string.IsNullOrEmpty(path))
            {
                Settings.Default.LightWallpaperPath = path;
            }
        }

        private void ButtonBrowseDarkWallpaper_Click(object sender, RoutedEventArgs e)
        {
            string path = BrowseWallpaper("Browse dark theme wallpaper");

            if (!string.IsNullOrEmpty(path))
            {
                Settings.Default.DarkWallpaperPath = path;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope((TextBox)sender), null);
                Keyboard.ClearFocus();
            }
        }

        private void ButtonGithub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adrianmteo/Luna");
        }

        private void ButtonIssues_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adrianmteo/Luna/issues");
        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adrianmteo");
        }
    }
}
