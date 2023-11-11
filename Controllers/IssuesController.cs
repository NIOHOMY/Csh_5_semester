using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class IssuesController : Controller
    {
        //private readonly LibraryContext _context;
        private readonly DatabaseManager _databaseManager;

        public IssuesController(LibraryContext context)
        {
            //_context = context;
            _databaseManager = new DatabaseManager(context);
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            List<Issue>? libraryContext = _databaseManager.GetAllIssues();//_context.Issues.Include(i => i.Reader);
            return View(libraryContext);
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
                if(selectedBookIds != null)
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
            var books = availableBooks.Where(b => b.Title.Contains(query)).ToList();
            return Json(books);
        }


        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue issue = _databaseManager.GetIssueById(id.Value);
            if (issue == null)
            {
                return NotFound();
            }

            // Добавим список уже выбранных книг для данного выпуска
            var selectedBooks = _databaseManager.GetIssueBooksByIssueId(id.Value);
            ViewData["ReaderId"] = new SelectList(_databaseManager.GetAllReaders(), "ReaderId", "Address", issue.ReaderId);
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
        public async Task<IActionResult> Edit(int id, [Bind("IssueDate,ReturnDate,ReaderId")] Issue issue, string selectedBookIds)
        {
            if (issue != null)
            {
                try
                {
                    int[] selectedBooks = Array.Empty<int>();
                    if (!string.IsNullOrEmpty(selectedBookIds))
                    {
                        var bookIds = selectedBookIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        foreach (var bookId in bookIds)
                        {
                            if (int.TryParse(bookId, out int bid))
                            {
                                selectedBooks = selectedBooks.Append(bid).ToArray();
                            }
                        }

                        _databaseManager.UpdateIssue(id, issue, selectedBooks);
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
