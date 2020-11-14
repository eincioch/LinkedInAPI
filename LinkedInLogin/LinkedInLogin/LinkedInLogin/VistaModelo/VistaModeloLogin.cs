using Acr.UserDialogs;
using LinkedInLogin.Modelo;
using LinkedInLogin.Servicios;
using LinkedInLogin.Vista;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinkedInLogin.VistaModelo
{
    public class VistaModeloLogin : VistaModeloBase
    {
        //string EmailLogged = 

        private Command _CmdLinkedInLogin;
        public Command CmdLinkedInLogin
        {
            get
            {
                if (_CmdLinkedInLogin == null)
                {
                    _CmdLinkedInLogin = new Command(async () =>
                    {
                        using (ServicioApiLinkedIn servicioLin = new ServicioApiLinkedIn())
                        {
                            WebAuthenticatorResult loginResult = await servicioLin.Login();

                            if (loginResult != null)
                            {
                                UserDialogs.Instance.ShowLoading("Descargando perfil...");

                                string jsonTokenResponse = await servicioLin.SolicitarAccessToken(loginResult.AccessToken);

                                if (jsonTokenResponse != null)
                                {
                                    string accessToken = JObject.Parse(jsonTokenResponse).GetValue("access_token").Value<string>();

                                    string jsonEmailResponse = await servicioLin.SolicitarHandle(accessToken);

                                    if (jsonEmailResponse != null)
                                    {
                                        EmailResponseModel emailResponse = JsonConvert.DeserializeObject<EmailResponseModel>(jsonEmailResponse);

                                        // obtener perfil
                                        string jsonPerfilResponse = await servicioLin.ObtenerPerfilConFoto(accessToken);

                                        if (jsonPerfilResponse != null)
                                        {
                                            UserDialogs.Instance.HideLoading();

                                            ModeloPerfilCompleto perfil = JsonConvert.DeserializeObject<ModeloPerfilCompleto>(jsonPerfilResponse);

                                            await ((NavigationPage)App.Current.MainPage).PushAsync(new VistaResultadoLogin(perfil));
                                        }
                                    }
                                    else
                                    {
                                        UserDialogs.Instance.HideLoading();

                                        await UserDialogs.Instance.AlertAsync(new AlertConfig { 
                                            Message = "Error al obtener el email",
                                            OkText = "Ok",
                                            Title = "Dotnet conf 2020"
                                        });
                                    }
                                }
                                else
                                {
                                    UserDialogs.Instance.HideLoading();

                                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                                    {
                                        Message = "Error al obtener el email",
                                        OkText = "Ok",
                                        Title = "Dotnet conf 2020"
                                    });
                                }
                            }
                        }
                    });
                }

                return _CmdLinkedInLogin;
            }
        }

    }
}
