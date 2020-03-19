using System.Windows;

namespace Luna.Windows
{
    public partial class MessageWindow : Window
    {
        private static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(MessageWindow), new PropertyMetadata("Caption"));
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        private static readonly DependencyProperty OKProperty = DependencyProperty.Register("OK", typeof(string), typeof(MessageWindow), new PropertyMetadata("OK"));
        public string OK
        {
            get { return (string)GetValue(OKProperty); }
            set { SetValue(OKProperty, value); }
        }

        private static readonly DependencyProperty CancelProperty = DependencyProperty.Register("Cancel", typeof(string), typeof(MessageWindow), new PropertyMetadata("Cancel"));
        public string Cancel
        {
            get { return (string)GetValue(CancelProperty); }
            set { SetValue(CancelProperty, value); }
        }

        public MessageWindow(Window owner, string title, string caption, string ok = "OK", string cancel = "Cancel")
        {
            InitializeComponent();

            Owner = owner;
            Title = title;
            Caption = caption;
            OK = ok;
            Cancel = cancel;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
