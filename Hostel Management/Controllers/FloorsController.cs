using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Hostel_Management.Models;

namespace Hostel_Management.Controllers
{
    public class FloorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FloorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Floors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Floors.ToListAsync());
        }

        // GET: Floors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (floor == null)
            {
                return NotFound();
            }

            return View(floor);
        }

        // GET: Floors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Floors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedAt")] Floor floor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(floor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(floor);
        }

        // GET: Floors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors.FindAsync(id);
            if (floor == null)
            {
                return NotFound();
            }
            return View(floor);
        }

        // POST: Floors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt")] Floor floor)
        {
            if (id != floor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(floor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FloorExists(floor.Id))
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
            return View(floor);
        }

        // GET: Floors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (floor == null)
            {
                return NotFound();
            }

            return View(floor);
        }

        // POST: Floors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var floor = await _context.Floors.FindAsync(id);
            if (floor != null)
            {
                _context.Floors.Remove(floor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FloorExists(int id)
        {
            return _context.Floors.Any(e => e.Id == id);
        }
    }
}
