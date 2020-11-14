using Newtonsoft.Json;

namespace LinkedInLogin.Modelo
{
    public class EmailResponseModel
    {
        public EmailResponseElementModel[] elements { get; set; }
    }

    public class EmailResponseElementModel
    {
        public string handle { get; set; }
        public string type { get; set; }
        public bool primary { get; set; }

        [JsonProperty("handle~")]
        public EmailResponseAddressModel handle2 { get; set; }
    }

    public class EmailResponseAddressModel
    {
        public string emailAddress { get; set; }
    }

    public class ProfileModel
    {
        public string localizedFirstName { get; set; }

        public string localizedLastName { get; set; }
    }
}