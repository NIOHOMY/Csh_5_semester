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
using System.Data;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Admin,Manager,User")]
    public class AccessController : Controller
    {
        private readonly DatabaseManager _databaseManager;
        private readonly UserManager<IdentityUser> _userManager;
        HashPasswordHasher _passwordHasher;

        public AccessController(LibraryContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _databaseManager = new DatabaseManager(context);
            _passwordHasher = new HashPasswordHasher();
            /*
            var allUsers = _userManager.Users.ToList();
            foreach (var user in allUsers)
            {
                _userManager.DeleteAsync(user);
            }
            */
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            //var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            if (claimUser.Identity.IsAuthenticated)
            {
                if (User.IsInRole("User"))
                {
                    return RedirectToAction("Index", "Books");
                }
                else if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "Issues");
                }
                else if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Readers");
                }
            }    


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
            /*
            var adminUser = new IdentityUser
            {
                UserName = "admin@a.com",
                Email = "admin@a.com"
            };

            var Aresult = await _userManager.CreateAsync(adminUser, "Qwerty123-");

            if (Aresult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, adminUser.UserName),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, 
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            _databaseManager.AddReader(new Reader
            {
                Email = "admin@a.com",
                FirstName = "Alex",
                LastName = "Temdijw",
                Patronymic = "Rfb",
                Address = "st Prsefsf",
                PhoneNumber = "+75436735622"
            });

            */
            /////////////////////////////////
            /*
            var managerUser = new IdentityUser
            {
                UserName = "employee@e.com",

                Email = "employee@e.com"

            };

            var Mresult = await _userManager.CreateAsync(managerUser, "Qwerty123-");

            if (Mresult.Succeeded)
            {
                await _userManager.AddToRoleAsync(managerUser, "Manager");
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, managerUser.UserName),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, 
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            _databaseManager.AddReader(new Reader
            {
                Email = "employee@e.com",
                FirstName = "Elex",
                LastName = "Gemdijw",
                Patronymic = "Pfb",
                Address = "st Arsefsf",
                PhoneNumber = "+75468335622"
            });

            */
            /////////////////////////////////

            var user = _userManager.FindByNameAsync(modelLogin.Email).Result; 

            if (user != null && _passwordHasher.VerifyHashedPassword(user,  user.PasswordHash, modelLogin.PassWord) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    
                };

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = modelLogin.KeepLoggedIn, 
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
                    return RedirectToAction("Index", "Readers");
                }

            }

            ViewData["ValidateMessage"] = "user not found";
            return View();
        }


        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Register(VMRegistration model)
        {
            if (model.Email != null && model.Password != null)
            {

                var user = new IdentityUser 
                { 
                    Email = model.Email, 
                    UserName = model.Email 
                };

                //string hashedPassword = _passwordHasher.HashPassword(user, model.Password);

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //var u = _userManager.FindByNameAsync(model.Email);
                    await _userManager.AddToRoleAsync(user, "User");
                    
                    //_databaseManager.AddUser(user);
                    _databaseManager.AddReader(
                        new Reader
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Patronymic = model.Patronymic,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber
                    });
                    return RedirectToAction("Login");
                    
                    /*var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        
                    };
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);*/
                }


            }

            return View("Register", model);
        }

    }
}
