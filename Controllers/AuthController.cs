using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
                return RedirectToLocal(returnUrl);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("/auth/login")]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(string password, string? returnUrl = null)
        {
            var storedHash = _config["Admin:PasswordHash"];
            if (!string.IsNullOrEmpty(storedHash) && VerifyPassword(password, storedHash))
            {
                await SignInAsync();
                return RedirectToLocal(returnUrl);
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Error"] = "Incorrect password.";
            return View();
        }

        private IActionResult RedirectToLocal(string? returnUrl) =>
            !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : Redirect("/admin");

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
            return Redirect("/admin");
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

        private const int Pbkdf2Iterations = 210_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        // Format: pbkdf2.<iterations>.<saltBase64>.<hashBase64>
        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password), salt, Pbkdf2Iterations, HashAlgorithmName.SHA256, HashSize);

            return $"pbkdf2.{Pbkdf2Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 4 || parts[0] != "pbkdf2") return false;
            if (!int.TryParse(parts[1], out var iterations)) return false;

            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);
            var actual = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password), salt, iterations, HashAlgorithmName.SHA256, expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
    }
}
