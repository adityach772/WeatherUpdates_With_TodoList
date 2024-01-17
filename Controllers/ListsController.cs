using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Lists //This method is returning when Index action is taken
        
        public async Task<IActionResult> Index()
        {
              return _context.List != null ? 
                          View(await _context.List.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.List'  is null.");
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchResults(string SearchPhrase)
        {
            var filteredList = _context.List.Where(item => item.Task.Contains(SearchPhrase)) .ToList();
            return View(filteredList);
                          
        }



        // GET: Lists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .FirstOrDefaultAsync(m => m.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // GET: Lists/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Task,Priority")] List list)
        {
            if (ModelState.IsValid)
            {
                _context.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }

        // GET: Lists/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }

            var list = await _context.List.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        // POST: Lists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Task,Priority")] List list)
        {
            if (id != list.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.Id))
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
            return View(list);
        }

        // GET: Lists/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .FirstOrDefaultAsync(m => m.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // POST: Lists/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.List == null)
            {
                return Problem("Entity set 'ApplicationDbContext.List'  is null.");
            }
            var list = await _context.List.FindAsync(id);
            if (list != null)
            {
                _context.List.Remove(list);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(int id)
        {
          return (_context.List?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
