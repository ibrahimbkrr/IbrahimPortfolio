using IbrahimPortfolio.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var username =
                _configuration["AdminUser:Username"];

            var password =
                _configuration["AdminUser:Password"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || model.Username != username ||
                model.Password != password)
            {
                ModelState.AddModelError(
                    "",
                    "Kullanıcı adı veya şifre hatalı.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.Name,
                    model.Username),

                new Claim(
                    ClaimTypes.Role,
                    "Admin")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties =
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authenticationProperties);

            return RedirectToAction(
                "Index",
                "Dashboard",
                new { area = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(
                "Index",
                "Login",
                new { area = "Admin" });
        }
    }
}
