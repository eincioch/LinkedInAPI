using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LinkedInLogin.Servicios
{
    public class ServicioApiLinkedIn : IDisposable
    {
        // metodo de login
        static string clientId = "78n1bc0q97csnq";
        static string state = "fooobar";
        static string scope = "r_liteprofile%20r_emailaddress";
        static string responseType = "code";
        static string redirectUrl = "https://dotnetconf2020.azurewebsites.net/dotnetconf/v1/ReceiveLinkedInResponse";
        static string callbackUrl = "dotnetconf://";

        // metodo de access token
        static string accessUrl = "https://www.linkedin.com/oauth/v2/accessToken";
        static string secret = "saSCGhvokBQ5MWRn";
        static string grantType = "authorization_code";

        // autenticacion
        public async Task<WebAuthenticatorResult> Login()
        {
            // efectua el login de linkedin
            string LoginUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type={responseType}&client_id={clientId}&state={state}&scope={scope}&redirect_uri={redirectUrl}";

            return await WebAuthenticator.AuthenticateAsync(new Uri(LoginUrl), new Uri(callbackUrl));
        }

        // autorizacion
        // se hace un post
        public async Task<string> SolicitarAccessToken(string authCode)
        {
            HttpClient linClient = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("code", authCode),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", secret)
            });

            var response = await linClient.PostAsync(accessUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        // obtener el correo con el que se autentico para validarlo contra backend
        public async Task<string> SolicitarHandle(string accessToken)
        {
            HttpClient linClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.linkedin.com/v2/clientAwareMemberHandles?q=members&projection=(elements*(primary,type,handle~))");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            HttpResponseMessage response = await linClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        // obtener el perfil del usuario
        public async Task<string> ObtenerPerfil(string accessToken)
        {
            HttpClient linClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.linkedin.com/v2/me");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            HttpResponseMessage response = await linClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        // obtener la foto de perfil
        public async Task<string> ObtenerPerfilConFoto(string accessToken)
        {
            HttpClient linClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            HttpResponseMessage response = await linClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
        }
    }
}
