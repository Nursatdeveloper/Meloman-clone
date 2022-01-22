using Meloman_clone.Dtos;
using Meloman_clone.Models;
using Meloman_clone.Repository;
using Meloman_clone.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Meloman_clone.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IService _services;

        public UserController(IUserRepository userRepository, IService services)
        {
            _userRepository = userRepository;
            _services = services;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(AuthenticationSchemes ="MelomanAuthCookie", Policy = "OnlyUsers")]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/User/Profile");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto, string returnUrl = "")
        {
            if(ModelState.IsValid)
            {
                var user = _userRepository.FindByEmail(loginDto.Email);
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                    {
                        string role = "User";
                        if (user.Email.Contains("admin"))
                        {
                            role = "Admin";
                            await AuthenticateAdmin();
                        }
                        await AuthenticateUser(user.Name, user.UserId);
                        await MelomanAuthentication(user, role);

                        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return Redirect("/User/Profile");
                    }
                    return Redirect("/User/Login?credential=invalid");
                }
            }
            return Redirect("/User/Login?user=notfound");
        }
        [HttpPost]
        public async Task<JsonResult> Register(string name, string email, string password)
        {
            if (email.Contains("admin") || email.Contains("Admin"))
            {
                return new JsonResult("invalid login");
            }
            var user = new User();
            user.Name = name;
            user.Email = email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);

            if (_userRepository.RegisterUser(user))
            {
                await AuthenticateUser(user.Name, user.UserId);
                await MelomanAuthentication(user, "User");
                return new JsonResult("Successful Registration!");
            }
            return new JsonResult("Registration Failed!");
        }
        public async Task<IActionResult> Logout()
        {
            var adminCookie = HttpContext.Request.Cookies.FirstOrDefault(e => e.Key == "MelomanAdminCookie");
            if (adminCookie.Key == "MelomanAdminCookie")
            {
                await HttpContext.SignOutAsync("MelomanAdminCookie");
            }
            await HttpContext.SignOutAsync("MelomanAuthCookie");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        private async Task AuthenticateUser(string userName, int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimTypes.Name, userName),
                new Claim("Name", userName),
                new Claim("Role", "User"),
                new Claim("UserId", userId.ToString()),
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        private async Task AuthenticateAdmin()
        {
            var adminClaims = _userRepository.GetAdminClaims();
            var adminIdentity = new ClaimsIdentity(adminClaims, "MelomanAdminCookie");
            var adminClaimsPrincipal = new ClaimsPrincipal(adminIdentity);
            await HttpContext.SignInAsync("MelomanAdminCookie", adminClaimsPrincipal);
        }
        private async Task MelomanAuthentication(User user, string role)
        {
            var claims = _userRepository.GetClaims(user, role);
            var identity = new ClaimsIdentity(claims, "MelomanAuthCookie");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MelomanAuthCookie", claimsPrincipal);
        }

        [HttpPost]
        public JsonResult DownloadToExcel(string downloadItem)
        {
            if(_services.DownloadExcel(downloadItem))
            {
                return new JsonResult("success");
            }
            return new JsonResult("fail");
        }
    }
}
