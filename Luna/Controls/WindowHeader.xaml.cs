using System.Windows;
using System.Windows.Controls;

namespace Luna.Controls
{
    public partial class WindowHeader : UserControl
    {
        private static readonly DependencyProperty HasUpdateProperty = DependencyProperty.Register("HasUpdate", typeof(bool), typeof(WindowHeader), new PropertyMetadata(false));
        public bool HasUpdate
        {
            get { return (bool)GetValue(HasUpdateProperty); }
            set { SetValue(HasUpdateProperty, value); }
        }

        private static readonly RoutedEvent OnClickUpdateEvent = EventManager.RegisterRoutedEvent("OnClickUpdate", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WindowHeader));

        public event RoutedEventHandler OnClickUpdate
        {
            add { AddHandler(OnClickUpdateEvent, value); }
            remove { RemoveHandler(OnClickUpdateEvent, value); }
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

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnClickUpdateEvent));
        }
    }
}
