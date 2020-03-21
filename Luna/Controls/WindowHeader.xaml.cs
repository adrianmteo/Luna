using System.Windows;
using System.Windows.Controls;

namespace Luna.Controls
{
    public partial class WindowHeader : UserControl
    {
        private static readonly DependencyProperty ShowAboutProperty = DependencyProperty.Register("ShowAbout", typeof(bool), typeof(WindowHeader), new PropertyMetadata(false));
        public bool ShowAbout
        {
            get { return (bool)GetValue(ShowAboutProperty); }
            set { SetValue(ShowAboutProperty, value); }
        }

        private static readonly RoutedEvent OnClickAboutEvent = EventManager.RegisterRoutedEvent("OnClickAbout", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WindowHeader));

        public event RoutedEventHandler OnClickAbout
        {
            add { AddHandler(OnClickAboutEvent, value); }
            remove { RemoveHandler(OnClickAboutEvent, value); }
        }

        private Window _window;

        public WindowHeader()
        {
            InitializeComponent();
        }

        private void WindowHeader_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DependencyObject dependencyObject)
            {
                _window = Window.GetWindow(dependencyObject);

                if (_window != null)
                {
                    ButtonMinimize.Visibility = !_window.ShowInTaskbar || _window.ResizeMode == ResizeMode.NoResize ? Visibility.Collapsed : Visibility.Visible;
                    ButtonMaximize.Visibility = _window.ResizeMode == ResizeMode.CanMinimize || _window.ResizeMode == ResizeMode.NoResize ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (_window != null)
            {
                _window.WindowState = WindowState.Minimized;
            }
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (_window != null)
            {
                _window.WindowState = _window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (_window != null)
            {
                _window.Close();
            }
        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnClickAboutEvent));
        }
    }
}
