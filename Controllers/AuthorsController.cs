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

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "User")]
    public class AuthorsController : Controller
    {
        //private readonly LibraryContext _context;
        private readonly DatabaseManager _databaseManager;

        public AuthorsController(LibraryContext context)
        {
            //_context = context;
            _databaseManager = new DatabaseManager(context);
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var authors = _databaseManager.GetAllAuthors();
            return authors.Count != 0 ? 
                          View(authors) :
                          Problem("Entity set 'LibraryContext.Authors'  is null.");
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var author = _databaseManager.GetAuthorById(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,Name")] Author author)
        {
            if (author != null)
            {
                _databaseManager.AddAuthor(author);
                
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var author = _databaseManager.GetAuthorById(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Name")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (author != null)
            {
                try
                {
                    _databaseManager.UpdateAuthor(author);
                    
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var author = _databaseManager.GetAuthorById(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id>=0)
            {
                _databaseManager.DeleteAuthor(id);
            }
            
            
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_databaseManager.GetAuthorById(id)!=null);
        }
    }
}
