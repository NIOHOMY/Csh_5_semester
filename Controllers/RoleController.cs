using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Security.Claims;

using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseManager _databaseManager;
        public RoleController(LibraryContext context, UserManager<IdentityUser> userManager)
        {
            _databaseManager = new DatabaseManager(context);
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUserRolePost(string userEmail, string newRole)
        {
            var user = _databaseManager.GetUserByEmail(userEmail);

            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var freshUser = await _userManager.FindByIdAsync(user.Id);
            foreach (var role in roles)
            {

                if (await _userManager.IsInRoleAsync(freshUser, role))
                {
                    await _userManager.RemoveFromRoleAsync(freshUser, role);
                }
            }

            await _userManager.AddToRoleAsync(freshUser, newRole);
            await _userManager.UpdateAsync(freshUser);

            return RedirectToAction("Index", "Readers");
        }
        public async Task<IActionResult> ChangeUserRoleAsync(string userEmail)
        {
            var user = _databaseManager.GetUserByEmail(userEmail);
            ViewBag.UserId = user.Id.ToString();
            ViewBag.Email = userEmail;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = roles.Any() ? roles.First() : "No Roles";

            // Здесь получите список ролей, например, из базы данных или статически
            ViewBag.Roles = new SelectList(new List<string> { "Admin", "Manager", "User" });

            return View("SelectRole");
        }
        public IActionResult ChangeUserRoleConfirmation()
        {
            return View();
        }
    }

}
