using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using WebApplication1.Data;
using WebApplication1.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication1.Controllers;

[Authorize(Roles = "Admin,Manager,User")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseManager _databaseManager;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        //var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [AllowAnonymous]
    public async Task<IActionResult> LogOut()
    {

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Access");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}