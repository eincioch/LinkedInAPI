using LinkedInLogin.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinkedInLogin.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaResultadoLogin : ContentPage
    {
        public VistaResultadoLogin()
        {
            InitializeComponent();
        }

        public VistaResultadoLogin(ModeloPerfilCompleto perfil)
        {
            InitializeComponent();

            lblNombreCompleto.Text = $"{perfil.firstName.localized.es_ES} {perfil.lastName.localized.es_ES}";

            imgFoto.Source = ImageSource.FromUri(new System.Uri(perfil.profilePicture.DisplayImage.elements[0].identifiers[0].identifier));
        }
    }
}