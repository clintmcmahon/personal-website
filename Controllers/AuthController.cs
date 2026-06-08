using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Website.Controllers
{
    public class AuthController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private const string SessionKey = "Admin_Authenticated";

        public AuthController(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsLoggedIn(HttpContext))
                return Redirect(returnUrl ?? "/admin/photos/new");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("/auth/login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string password, string? returnUrl = null)
        {
            var storedHash = _config["Admin:PasswordHash"];
            if (!string.IsNullOrEmpty(storedHash) && HashPassword(password) == storedHash)
            {
                HttpContext.Session.SetString(SessionKey, "1");
                return Redirect(returnUrl ?? "/admin/photos/new");
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Error"] = "Incorrect password.";
            return View();
        }

        [HttpGet("/auth/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKey);
            return Redirect("/");
        }

        // Dev-only: visit /auth/gen-hash?password=yourpassword to get the hash to paste into appsettings.local.json
        [HttpGet("/auth/gen-hash")]
        public IActionResult GenHash(string password)
        {
            if (!_env.IsDevelopment())
                return NotFound();

            return Content(HashPassword(password));
        }

        [HttpGet("/auth/dev-login")]
        public IActionResult DevLogin()
        {
            if (!_env.IsDevelopment())
                return NotFound();

            HttpContext.Session.SetString(SessionKey, "1");
            return Redirect("/admin/photos/new");
        }

        public static bool IsLoggedIn(HttpContext context) =>
            context.Session.GetString(SessionKey) == "1";

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
