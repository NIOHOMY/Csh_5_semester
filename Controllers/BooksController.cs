using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace WebApplication1.Controllers
{
    //[Authorize(Roles = "User")]
    [Authorize(Roles = "Admin,Manager,User")]
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
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            List<Book>? libraryContext;

            if (!string.IsNullOrEmpty(searchString))
            {
                libraryContext = _databaseManager.SearchBooksByTitleOrFirstAuthor(searchString);
            }
            else
            {
                libraryContext = _databaseManager.GetAllBooks();
            }

            ViewBag.SearchString = searchString;

            return View(libraryContext);
        }

        // GET: Books/Details/5
        [Authorize(Roles = "Admin,Manager,User")]
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

        
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            ViewData["FirstAuthorId"] = new SelectList(_databaseManager.GetAllAuthors(), "AuthorId", "Name");
            ViewData["PublisherId"] = new SelectList(_databaseManager.GetAllPublishers(), "PublisherId", "City");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([Bind("BookId,Title,FirstAuthorId,YearOfPublication,Price,NumberOfExamples,PublisherId,ImageData")] Book book, IFormFile imageData)
        {

            if (book != null)
            {
                if (imageData != null && imageData.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageData.CopyTo(memoryStream);
                        book.ImageData = memoryStream.ToArray();
                    }
                }
                _databaseManager.AddBook(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstAuthorId"] = new SelectList(_databaseManager.GetAllAuthors(), "AuthorId", "Name", book.FirstAuthorId);
            ViewData["PublisherId"] = new SelectList(_databaseManager.GetAllPublishers(), "PublisherId", "City", book.PublisherId);
            return View(book);
        }


        // GET: Books/Edit/5
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,FirstAuthorId,YearOfPublication,Price,NumberOfExamples,PublisherId,ImageData")] Book book, IFormFile imageData)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (book != null)//(ModelState.IsValid)
            {
                try
                {
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageData.CopyTo(memoryStream);
                            book.ImageData = memoryStream.ToArray();
                        }
                    }
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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

        /*
        */
        public IActionResult GetImage(int id)
        {
            var book = _databaseManager.GetBookById(id);
            if (book != null)
            {
                byte[] imageData = book.ImageData;
                if (imageData != null)
                {
                    string imageUrl = Url.Action("GetImage", "Books", new { id = book.BookId, width = 100, height = 100, random = DateTime.Now.Ticks });

                    return File(book.ImageData, "image/jpeg");
                    //return File(imageData, "image/jpeg");
                }
            }
            return File("~/images/default-book-image.jpg", "image/jpeg");
            /*string imagePath = "~/images/default-book-image.jpg";

            string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('~').Trim('/'));

            if (System.IO.File.Exists(physicalPath))
            {
                // Загрузите изображение и верните его в ответе
                var imageBytes = System.IO.File.ReadAllBytes(physicalPath);
                return File(imageBytes, "image/jpeg"); // Изменьте MIME-тип по необходимости
            }*/
        }

        /*public IActionResult GetImage(int id, int width = 100, int height = 100)
        {
            Console.WriteLine($"GetImage called with id: {id}, width: {width}, height: {height}");
            var book = _databaseManager.GetBookById(id);
            if (book != null)
            {
                byte[] imageData = book.ImageData;
                if (imageData != null)
                {
                    using (var image = Image.Load(imageData))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(width, height),
                            Mode = ResizeMode.Max
                        }));

                        using (var outputStream = new MemoryStream())
                        {
                            image.Save(outputStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());

                            return File(outputStream.ToArray(), "image/jpeg");
                        }
                    }
                }
            }
            return File("~/images/default-book-image.jpg", "image/jpeg");
        }*/

    }

}
