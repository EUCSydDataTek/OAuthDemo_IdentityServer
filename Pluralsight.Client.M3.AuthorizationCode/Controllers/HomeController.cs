using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.Client.M3.AuthorizationCode
{
    public class HomeController : Controller
    {
        private static string Message { get; set; } = "";
        private static string Code { get; set; }
        private static string Token { get; set; }
        private static string TokenType { get; set; }

        private static readonly string State = CryptoRandom.CreateUniqueId();

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
            var authUrl =
                $"http://localhost:5000/connect/authorize?client_id={ClientId}&scope=wiredbrain_api.rewards offline_access&redirect_uri={RedirectUri}&response_type=code&response_mode=query&state={State}";
            
            Message += $"Redirecting to authorization endpoint. State value of: {State}";
            Message += $"\n<b>URL:</b> {authUrl}";
            return Redirect(authUrl);
        }

        public IActionResult Callback([FromQuery] string code, [FromQuery] string state)
        {
            if (State != state)
            {
                Message += "\n\nState not recognised. Cannot trust response.";
                return RedirectToAction("Index");
            }

            Code = code;

            Message += "\n\nApplication Authorized!";
            Message += $"\n<b>code:</b> {code}";
            Message += $"\n<b>state:</b> {State}";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetToken()
        {
            if (Code == null)
            {
                Message += "\n\nNot ready! Authorize first.";
                return RedirectToAction("Index");
            }

            Message += "\n\nCalling token endpoint...";

            var tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = "http://localhost:5000/connect/token",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Code = Code,
                RedirectUri = RedirectUri,
            });

            if (tokenResponse.IsError)
            {
                Message += "\n\nToken request failed";
                return RedirectToAction("Index");
            }

            TokenType = tokenResponse.TokenType;
            Token = tokenResponse.AccessToken;
            Message += "\n\nToken Received!";
            Message += $"\n<b>access_token:</b> {tokenResponse.AccessToken}";
            Message += $"\n<b>refresh_token:</b> {tokenResponse.RefreshToken}";
            Message += $"\n<b>expires_in:</b> {tokenResponse.ExpiresIn}";
            Message += $"\n<b>token_type:</b> {tokenResponse.TokenType}";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CallApi()
        {
            if (Token != null)
            {
                Message += $"\n\nCalling API with Authorization header: {TokenType} {Token}";
                client.SetBearerToken(Token);
            }

            var response = await client.GetAsync("http://localhost:5002/api/rewards");

            if (response.IsSuccessStatusCode) Message += "\nAPI access authorized!";
            else if (response.StatusCode == HttpStatusCode.Unauthorized) Message += "\nUnable to contact API: Unauthorized!";
            else Message += $"\nUnable to contact API. Status code {response.StatusCode}";

            return RedirectToAction("Index");
        }
    }
}
