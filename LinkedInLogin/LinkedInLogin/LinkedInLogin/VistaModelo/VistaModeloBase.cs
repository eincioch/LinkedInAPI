using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LinkedInLogin.VistaModelo
{
    public class VistaModeloBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
