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
    public class BooksController : Controller
    {
        //private readonly LibraryContext _context;
        private readonly DatabaseManager _databaseManager;

        public BooksController(LibraryContext context)
        {
            //_context = context;
            _databaseManager = new DatabaseManager(context);
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            List<Book>? libraryContext = _databaseManager.GetAllBooks();

            return View(libraryContext);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book =  _databaseManager.GetBookById(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["FirstAuthorId"] = new SelectList(_databaseManager.GetAllAuthors(), "AuthorId", "Name");
            ViewData["PublisherId"] = new SelectList(_databaseManager.GetAllPublishers(), "PublisherId", "City");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("BookId,Title,FirstAuthorId,YearOfPublication,Price,NumberOfExamples,PublisherId")] Book book)
        {
            
            if (book!=null)
            {
                _databaseManager.AddBook(book);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstAuthorId"] = new SelectList(_databaseManager.GetAllAuthors(), "AuthorId", "Name", book.FirstAuthorId);
            ViewData["PublisherId"] = new SelectList(_databaseManager.GetAllPublishers(), "PublisherId", "City", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _databaseManager.GetBookById(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            
            ViewData["FirstAuthorId"] = new SelectList(_databaseManager.GetAllAuthors(), "AuthorId", "Name", book.FirstAuthorId);
            ViewData["PublisherId"] = new SelectList(_databaseManager.GetAllPublishers(), "PublisherId", "City", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,FirstAuthorId,YearOfPublication,Price,NumberOfExamples,PublisherId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (book != null)//(ModelState.IsValid)
            {
                try
                {
                    _databaseManager.UpdateBook(book);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstAuthorId"] = new SelectList(_databaseManager.GetAllAuthors(), "AuthorId", "Name", book.FirstAuthorId);
            ViewData["PublisherId"] = new SelectList(_databaseManager.GetAllPublishers(), "PublisherId", "City", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _databaseManager.GetBookById(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_databaseManager.GetAllBooks().Count == 0)
            {
                return Problem("Entity set 'LibraryContext.Books'  is null.");
            }
            _databaseManager.DeleteBook(id);
            
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_databaseManager.GetBookById(id)!=null);
        }
    }
}
