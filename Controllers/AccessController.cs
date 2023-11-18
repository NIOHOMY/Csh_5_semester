using WebApplication1.Models;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using WebApplication1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class AccessController : Controller
    {
        private readonly DatabaseManager _databaseManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccessController(LibraryContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            //_context = context;
            _databaseManager = new DatabaseManager(context);
            _userManager = userManager;
            _signInManager = signInManager;
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
            /*
            string AhashedPassword = HashPassword("Qwerty123-");
            var adminUser = new UserModel
            {
                UserName = "admin@a.com",

                Email = "admin@a.com",
                PasswordHash = AhashedPassword,
                FirstName = "Alex",
                LastName = "Temdijw",
                Patronymic = "Rfb",
                Address = "st Prsefsf",
                PhoneNumber = "+75436735622"
            };

            var Aresult = await _userManager.CreateAsync(adminUser, "Qwerty123-");

            if (Aresult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                // Sign in the user after registration
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, adminUser.UserName),
                        // Add other claims as needed
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // You can set this based on your requirement
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            _databaseManager.AddUser(adminUser);
            _databaseManager.AddReader(new Reader
            {
                FirstName = "Alex",
                LastName = "Temdijw",
                Patronymic = "Rfb",
                Address = "st Prsefsf",
                PhoneNumber = "+75436735622"
            });
            */
            /////////////////////////////////
            /*
            string MhashedPassword = HashPassword("Qwerty123-");
            var adminUser = new UserModel
            {
                UserName = "employee@e.com",

                Email = "employee@e.com",
                PasswordHash = MhashedPassword,
                FirstName = "Elex",
                LastName = "Gemdijw",
                Patronymic = "Pfb",
                Address = "st Arsefsf",
                PhoneNumber = "+75468335622"
            };

            var Aresult = await _userManager.CreateAsync(adminUser, "Qwerty123-");

            if (Aresult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Manager");
                // Sign in the user after registration
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, adminUser.UserName),
                        // Add other claims as needed
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // You can set this based on your requirement
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            _databaseManager.AddUser(adminUser);
            _databaseManager.AddReader(new Reader
            {
                FirstName = "Elex",
                LastName = "Gemdijw",
                Patronymic = "Pfb",
                Address = "st Arsefsf",
                PhoneNumber = "+75468335622"
            });
            */
            /////////////////////////////////

            var user = _databaseManager.GetUserByEmail(modelLogin.Email); 

            if (user != null && BCrypt.Net.BCrypt.Verify(modelLogin.PassWord, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    // Add other claims as needed
                };

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // You can set this based on your requirement
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                if (roles.Contains("User"))
                {
                    return RedirectToAction("Index", "Books");
                }
                else if (roles.Contains("Manager"))
                {
                    return RedirectToAction("Index", "Issues");
                }
                else if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Books");
                }

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
                
                /////////////////////////////////////////////////////

                // Хэширование пароля перед сохранением в базу данных
                string hashedPassword = HashPassword(model.Password);
                var user = new UserModel
                {
                    UserName = model.Email,

                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Patronymic = model.Patronymic,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    // Sign in the user after registration
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        // Add other claims as needed
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // You can set this based on your requirement
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                }


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
