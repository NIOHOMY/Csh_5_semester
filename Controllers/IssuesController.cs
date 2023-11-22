using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using WebApplication1.Data;
using WebApplication1.Models;

using static System.Reflection.Metadata.BlobBuilder;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin,Manager,User")]
    public class IssuesController : Controller
    {
        private readonly DatabaseManager _databaseManager;
        private readonly UserManager<IdentityUser> _userManager;

        public IssuesController(LibraryContext context, UserManager<IdentityUser> userManager)
        {
            _databaseManager = new DatabaseManager(context);
            _userManager = userManager;
        }

        // GET: Issues
        public async Task<IActionResult> Index(string filter, [FromQuery(Name = "search")] string searchString)
        {
            List<Issue>? issues = new List<Issue>();
            var user = await _userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(filter))
            {

                switch (filter.ToLower())
                {
                    case "confirmed":
                        issues = _databaseManager.GetIssuesByStatus(true);
                        break;
                    case "pending":
                        issues = _databaseManager.GetIssuesByStatus(false);
                        break;
                    default:
                        issues = _databaseManager.GetAllIssues();
                        break;
                }

            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                issues = _databaseManager.SearchIssuesByIdOrReader(searchString);
            }
            else
            {
                issues = _databaseManager.GetAllIssues();
            }
            if (User.IsInRole("User"))
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Name).Value;
                issues = issues.Where(b => b.Reader.Email == userEmailClaim).ToList();
            }

            ViewBag.SearchString = searchString;

            return issues!=null ?
                         View(issues) :
                         Problem("Entity set 'LibraryContext.Issues'  is null.");
            //return View(issues);
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue? issue = _databaseManager.GetIssueById(id.Value);
            if (issue == null)
            {
                return NotFound();
            }


            return View(issue);
        }
        public IActionResult Create()
        {
            ViewData["ReaderId"] = new SelectList(_databaseManager.GetAllReaders(), "ReaderId", "PhoneNumber");
            ViewData["Books"] = _databaseManager.GetAllAvailableBooks(); // Получаем список всех книг и передаем его в представление
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueId,IssueDate,ReturnDate,ReaderId")] Issue issue, string selectedBookIds)
        {

            if (issue != null)
            {
                if (selectedBookIds != null)
                {
                    var bookIds = selectedBookIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                
                        foreach (var bookId in bookIds)
                        {
                            if (int.TryParse(bookId, out int id))
                            {
                                var book = _databaseManager.GetBookById(id);
                                if (book != null)
                                {
                                    issue.Books.Add(book);
                                }
                            }
                        }
                
                    _databaseManager.AddIssue(issue);
                    return RedirectToAction(nameof(Index));

                }
            }
            ViewData["ReaderId"] = new SelectList(_databaseManager.GetAllReaders(), "ReaderId", "PhoneNumber", issue.ReaderId);
            ViewData["Books"] = _databaseManager.GetAllAvailableBooks();
            return View(issue);
        }



        [HttpPost]
        public IActionResult SearchBooks(string query)
        {
            var availableBooks = _databaseManager.GetAllAvailableBooks();
            var books = availableBooks.Where(b => b.Title.ToLower().Contains(query.ToLower())).ToList();
            return Json(books);
        }
        [HttpPost]
        public IActionResult GetBookInfo(int bookId)
        {
            var book = _databaseManager.GetBookById(bookId);
            return Json(new { title = book.Title, price = book.Price });
        }

        [HttpPost]
        public IActionResult SearchReaders(string query)
        {
            var readers = _databaseManager.GetAllReaders();
            var q = readers
                .Where(r => (r.LastName.ToLower() + ' '+r.FirstName.ToLower() + ' ' + r.Patronymic.ToLower() + ' ' + r.PhoneNumber).Contains(query.ToLower()))
                .Select(r => new { 
                    ReaderId = r.ReaderId, 
                    DisplayText = $"{r.LastName} {r.FirstName} {r.Patronymic} {r.FormattedPhoneNumber}" 
                })
                .ToList();

            return Json(q);
        }


        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue issue = _databaseManager.GetIssueById(id.Value);
            if (issue == null || (User.IsInRole("User") && issue.isСonfirmed))
            {
                return NotFound();
            }
                // Добавим список уже выбранных книг для данного выпуска
                var selectedBooks = _databaseManager.GetIssueBooksByIssueId(id.Value);
            /*ViewBag.ReaderId = new SelectList(_databaseManager.GetAllReaders()
                        .Select(r => new {
                            Value = r.ReaderId,
                            Text = $"{r.LastName} {r.FirstName} {r.Patronymic} {r.PhoneNumber}"
                        }), "Value", "Text", issue.ReaderId);*/
            ViewBag.ReaderId = issue.Reader;
            ViewData["Books"] = _databaseManager.GetAllAvailableBooks();
            ViewBag.SelectedBooks = selectedBooks; // Передача выбранных книг в представление
            string ids = "";
            foreach (var book in selectedBooks)
            {
                ids += book.BookId.ToString() + ",";
            }
            ids = ids.TrimEnd(','); // Удаление последней запятой
            ViewBag.SelectedBooksIds = ids; // Передача выбранных книг в представление

            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueDate,ReturnDate,isСonfirmed,ReaderId")] Issue issue, string selectedBookIds, string removedBookIds)
        {
            if (issue != null)
            {
                try
                {
                    if (issue == null || (User.IsInRole("User") && issue.isСonfirmed))
                    {
                        return NotFound();
                    }
                    int[] selectedBooksToAdd = Array.Empty<int>();
                    int[] selectedBooksToDelete = Array.Empty<int>();
                    if (!string.IsNullOrEmpty(selectedBookIds) || !string.IsNullOrEmpty(removedBookIds))
                    {
                        if (!string.IsNullOrEmpty(selectedBookIds))
                        {
                            var bookIds = selectedBookIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var bookId in bookIds)
                            {
                                if (int.TryParse(bookId, out int bid))
                                {
                                    selectedBooksToAdd = selectedBooksToAdd.Append(bid).ToArray();
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(removedBookIds))
                        {
                            var removedIds = removedBookIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            
                            foreach (var removedId in removedIds)
                            {
                                if (int.TryParse(removedId, out int removedBid))
                                {
                                    selectedBooksToDelete = selectedBooksToDelete.Append(removedBid).ToArray();
                                }
                            }
                        }

                        _databaseManager.UpdateIssue(id, issue, selectedBooksToAdd, selectedBooksToDelete);
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction(nameof(Edit));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.IssueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["ReaderId"] = new SelectList(_databaseManager.GetAllReaders(), "ReaderId", "PhoneNumber", issue.ReaderId);
            ViewData["Books"] = _databaseManager.GetAllAvailableBooks(); // Предполагая, что у вас есть метод для получения всех книг
            ViewBag.SelectedBooks = _databaseManager.GetIssueBooksByIssueId(id);
            var selectedBookss = _databaseManager.GetIssueBooksByIssueId(id);
            string ids = "";
            foreach (var book in selectedBookss)
            {
                ids += book.BookId.ToString() + ",";
            }
            ids = ids.TrimEnd(','); // Удаление последней запятой
            ViewBag.SelectedBooksIds = ids; // Передача выбранных книг в представление
            return View(issue);
        }

        public IActionResult Status(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SaveStatus(int id, bool isСonfirmed)
        {
            if (ModelState.IsValid)
            {
                // Обновление статуса в базе данных
                _databaseManager.UpdateIssueStatus(id, isСonfirmed);
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue? issue = _databaseManager.GetIssueById(id.Value);
            if (issue == null)
            {
                return NotFound();
            }


            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            _databaseManager.DeleteIssue(id);

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
          return (_databaseManager.GetIssueById(id) != null);
        }
    }
}
