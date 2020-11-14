using LinkedInLogin.VistaModelo;
using Xamarin.Forms;

namespace LinkedInLogin.Vista
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new VistaModeloLogin();
        }
    }
}
