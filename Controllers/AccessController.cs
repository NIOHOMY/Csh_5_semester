using WebApplication1.Models;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class AccessController : Controller
    {
        private readonly DatabaseManager _databaseManager;

        public AccessController(LibraryContext context)
        {
            //_context = context;
            _databaseManager = new DatabaseManager(context);
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {

            var user = _databaseManager.GetUserByEmail(modelLogin.Email); 

            if (user != null && BCrypt.Net.BCrypt.Verify(modelLogin.PassWord, user.PasswordHash))
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("OtherProperties","Example Role")

                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }



            ViewData["ValidateMessage"] = "user not found";
            return View();
        }


        // AccessController.cs
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Register(VMRegistration model)
        {
            if (model.Email != null && model.Password != null)
            {
                // Хэширование пароля перед сохранением в базу данных
                string hashedPassword = HashPassword(model.Password);

                var user = new UserModel
                {
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Patronymic = model.Patronymic,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };

                _databaseManager.AddUser(user);
                _databaseManager.AddReader(new Reader
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Patronymic = model.Patronymic,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                });

                return RedirectToAction("Login");
            }

            return View("Register", model);
        }

        private string HashPassword(string password)
        {
            // В реальном приложении рекомендуется использовать библиотеку для хэширования паролей, например, BCrypt.Net
            // Пример с использованием BCrypt.Net: https://github.com/BcryptNet/bcrypt.net
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
