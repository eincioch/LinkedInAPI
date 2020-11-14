using Microsoft.AspNetCore.Mvc;

namespace LinkedInBackend.Controllers
{
    [ApiController]
    public class LinkedInController : ControllerBase
    {
        const string callbackAppUri = "dotnetconf";

        [HttpGet]
        [Route("dotnetconf/v1/ReceiveLinkedInResponse")]
        public IActionResult ReceiveLinkedInResponse(string code)
        {
            string callbackUrl = $"{callbackAppUri}://#access_token={code}";
            return Redirect(callbackUrl);
        }
    }
}