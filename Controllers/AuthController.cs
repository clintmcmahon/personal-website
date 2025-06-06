using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Text.Json;

namespace Website.Controllers
{
    public class AuthController : Controller
    {
        private const string IndieAuthAuthorizeEndpoint = "https://indieauth.com/auth";
        private const string IndieAuthTokenEndpoint = "https://indieauth.com/token";
        private const string Me = "https://clintmcmahon.com/";
        private const string ClientId = "https://clintmcmahon.com/";
        private const string RedirectUri = "https://clintmcmahon.com/auth/callback";
        private const string SessionKey = "IndieAuth_AccessToken";

        [HttpGet("/auth/login")]
        public IActionResult Login()
        {
            var state = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("IndieAuth_State", state);
            var url = $"{IndieAuthAuthorizeEndpoint}?me={Uri.EscapeDataString(Me)}&client_id={Uri.EscapeDataString(ClientId)}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&state={state}&response_type=code";
            return Redirect(url);
        }

        [HttpGet("/auth/callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            var expectedState = HttpContext.Session.GetString("IndieAuth_State");
            if (state != expectedState)
            {
                return Unauthorized("Invalid state");
            }

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri),
            });
            var response = await client.PostAsync(IndieAuthTokenEndpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized("Token exchange failed: " + responseString);
            }

            // IndieAuth returns application/json or application/x-www-form-urlencoded
            string? accessToken = null;
            string? me = null;
            try
            {
                var json = JsonDocument.Parse(responseString);
                accessToken = json.RootElement.GetProperty("access_token").GetString();
                me = json.RootElement.GetProperty("me").GetString();
            }
            catch
            {
                var parsed = HttpUtility.ParseQueryString(responseString);
                accessToken = parsed["access_token"];
                me = parsed["me"];
            }

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(me))
            {
                return Unauthorized("Token or user missing");
            }

            if (me != Me)
            {
                return Unauthorized("Authenticated as wrong user");
            }

            HttpContext.Session.SetString(SessionKey, accessToken!);
            return Redirect("/");
        }

        [HttpGet("/auth/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKey);
            return Redirect("/");
        }

        public static bool IsLoggedIn(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Session.GetString(SessionKey));
        }
    }
}
