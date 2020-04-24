using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Luna.Utils
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public delegate bool ShouldChangePropertyEventHandler(object sender, PropertyChangedEventArgs e);
        public event ShouldChangePropertyEventHandler ShouldChangeProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        internal bool CanChangeProperty([CallerMemberName] string propertyName = "")
        {
            return ShouldChangeProperty == null || ShouldChangeProperty(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
