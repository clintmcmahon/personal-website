using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Website.Controllers
{
    public class AuthController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public AuthController(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsLoggedIn(HttpContext))
                return Redirect(returnUrl ?? "/admin");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("/auth/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string password, string? returnUrl = null)
        {
            var storedHash = _config["Admin:PasswordHash"];
            if (!string.IsNullOrEmpty(storedHash) && HashPassword(password) == storedHash)
            {
                await SignInAsync();
                return Redirect(returnUrl ?? "/admin");
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Error"] = "Incorrect password.";
            return View();
        }

        [HttpGet("/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
        public async Task<IActionResult> DevLogin()
        {
            if (!_env.IsDevelopment())
                return NotFound();

            await SignInAsync();
            return Redirect("/admin/photos/new");
        }

        public static bool IsLoggedIn(HttpContext context) =>
            context.User?.Identity?.IsAuthenticated == true;

        private Task SignInAsync()
        {
            var identity = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, "admin") },
                CookieAuthenticationDefaults.AuthenticationScheme);

            return HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = true });
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
