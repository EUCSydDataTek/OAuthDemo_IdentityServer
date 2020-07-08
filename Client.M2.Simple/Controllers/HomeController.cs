using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pluralsight.Client.M2.Simple.Controllers
{
    public class HomeController : Controller
    {
        private static string Message { get; set; } = "";
        private static string Code { get; set; }
        private static string Token { get; set; }

        HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            return View("Index", Message);
        }

        private const string ClientId = "simple_client";
        private const string ClientSecret = "secret";
        private const string RedirectUri = "http://localhost:5001/home/callback";

        public IActionResult Authorize()
        {
            Message += "\n\nRedirecting to authorization endpoint...";
            return Redirect($"http://localhost:5000/connect/authorize?client_id={ClientId}&scope=wiredbrain_api.rewards&redirect_uri={RedirectUri}&response_type=code&response_mode=query");
        }

        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            Code = code;
            Message += "\nApplication Authorized!";

            Message += "\n\nCalling token endpoint...";

            var tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = "http://localhost:5000/connect/token",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Code = code,
                RedirectUri = RedirectUri,
            });

            if (tokenResponse.IsError)
            {
                Message += "\nToken request failed";
                return RedirectToAction("Index");
            }

            Token = tokenResponse.AccessToken;
            Message += "\nToken Received!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CallApi()
        {
            if (Token != null) client.SetBearerToken(Token);

            var response = await client.GetAsync("http://localhost:5002/api/rewards");

            if (response.IsSuccessStatusCode) Message += "\n\nAPI access authorized!";
            else if (response.StatusCode == HttpStatusCode.Unauthorized) Message += "\nUnable to contact API: Unauthorized!";
            else Message += $"\n\nUnable to contact API. Status code {response.StatusCode}";

            return RedirectToAction("Index");
        }
    }
}
