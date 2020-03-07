using System.Windows;
using System.Windows.Controls;

namespace Luna
{
    public partial class AnimatedView : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(bool), typeof(AnimatedView), new PropertyMetadata(true));
        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public AnimatedView()
        {
            InitializeComponent();
        }
    }
}
