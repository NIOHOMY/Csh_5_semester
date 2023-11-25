using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin,Manager,User")]
    public class AuthorsController : Controller
    {
        private readonly DatabaseManager _databaseManager;

        public AuthorsController(LibraryContext context)
        {
            _databaseManager = new DatabaseManager(context);
        }

        // GET: 
        public async Task<IActionResult> Index()
        {
                List<Author>? authors = null;
            try
            {
                authors = _databaseManager.GetAllAuthors();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Console.WriteLine(ex.Message);

                Debug.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Debug.WriteLine(ex.Message);

            }
            return authors != null ?
                          View(authors) :
                          Problem("Entity set is null.");
        }

        // GET: 
        public async Task<IActionResult> Details(int? id)
        {
            Author? author = null;
            try
            {
            if (id == null )
            {
                return NotFound();
            }

            author  = _databaseManager.GetAuthorById(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Console.WriteLine(ex.Message);

                Debug.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Debug.WriteLine(ex.Message);

            }
            return author != null ?
                          View(author) :
                          Problem("Entity set is null.");
        }

        // GET: Authors/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("AuthorId,Name,Info")] Author author, IFormFile? imageData)
        {
            if (ModelState.IsValid && author != null)
            {
                try
                {
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageData.CopyTo(memoryStream);
                            author.ImageData = memoryStream.ToArray();
                        }
                    }
                    
                    
                    _databaseManager.AddAuthor(author);

                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }          
            }

            return author != null ?
                          View(author) :
                          Problem("Entity set is null.");
            
        }

        // GET: Authors/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            Author? author = null;
            try
            {
            if (id == null )
            {
                return NotFound();
            }

            author = _databaseManager.GetAuthorById(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Console.WriteLine(ex.Message);

                Debug.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Debug.WriteLine(ex.Message);

            }

            return author != null ?
                          View(author) :
                          Problem("Entity set is null.");
           
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Name,Info,ImageData")] Author author, IFormFile? imageData)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid && author != null)
            {
                try
                {
                    var existingAuthor = _databaseManager.GetAuthorById(id);
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageData.CopyTo(memoryStream);
                            existingAuthor.ImageData = memoryStream.ToArray();
                        }
                    }
                    if (existingAuthor != null)
                    {
                        existingAuthor.Name = author.Name;
                        existingAuthor.Info = author.Info;

                        _databaseManager.UpdateAuthor(existingAuthor);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            Author? author = null;
            try
            {
            if (id == null )
            {
                return NotFound();
            }

            author = _databaseManager.GetAuthorById(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Console.WriteLine(ex.Message);

                Debug.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Debug.WriteLine(ex.Message);

            }
            return author != null ?
                          View(author) :
                          Problem("Entity set is null.");
            
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
            if (id>=0)
            {
                _databaseManager.DeleteAuthor(id);
            }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Console.WriteLine(ex.Message);

                Debug.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Debug.WriteLine(ex.Message);

            }
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_databaseManager.GetAuthorById(id)!=null);
        }

        public IActionResult GetImage(int id)
        {
            try
            {
            var author = _databaseManager.GetAuthorById(id);
            if (author != null)
            {
                byte[] imageData = author.ImageData;
                if (imageData != null)
                {
                    string imageUrl = Url.Action("GetImage", "Authors", new { id = author.AuthorId, width = 100, height = 100, random = DateTime.Now.Ticks });

                    return File(author.ImageData, "image/jpeg");
                }
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Console.WriteLine(ex.Message);

                Debug.WriteLine($"Произошла ошибка в {MethodBase.GetCurrentMethod().Name} ");
                Debug.WriteLine(ex.Message);

            }
            return File("~/images/basic_authors_imgs/default_images/default-author-image.jpg", "image/jpeg");
        }
    }
}
