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
    [Authorize(Roles = "Admin,Manager")]
    public class ReadersController : Controller
    {
        //private readonly LibraryContext _context;
        private readonly DatabaseManager _databaseManager;

        public ReadersController(LibraryContext context)
        {
            //_context = context;
            _databaseManager = new DatabaseManager(context);
        }

        // GET: Readers
        public async Task<IActionResult> Index()
        {
            List<Reader>? readers = _databaseManager.GetAllReaders();
              return readers.Count != 0 ?
                          View(readers) :
                          Problem("Entity set 'LibraryContext.Readers'  is null.");
        }

        // GET: Readers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var reader = _databaseManager.GetReaderById(id.Value);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // GET: Readers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Readers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReaderId,FirstName,LastName,Patronymic,Address,PhoneNumber")] Reader reader)
        {
            /*if (reader != null)
            {
                _databaseManager.AddReader(reader);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }*/
            RedirectToAction("Register", "Access");
            return View(reader);
        }

        // GET: Readers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var reader = _databaseManager.GetReaderById(id.Value);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
        }

        // POST: Readers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReaderId,FirstName,LastName,Patronymic,Address,PhoneNumber")] Reader reader)
        {
            if (id != reader.ReaderId)
            {
                return NotFound();
            }

            if (reader != null)
            {
                try
                {
                    _databaseManager.UpdateReader(reader);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaderExists(reader.ReaderId))
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
            return View(reader);
        }

        // GET: Readers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var reader = _databaseManager.GetReaderById(id.Value);
             
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // POST: Readers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id >= 0)
            {
                _databaseManager.DeleteReader(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReaderExists(int id)
        {
          return (_databaseManager.GetReaderById(id) != null);
        }
    }
}
