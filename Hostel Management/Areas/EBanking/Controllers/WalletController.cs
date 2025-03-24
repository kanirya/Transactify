using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Hostel_Management.Models.Model;

namespace Hostel_Management.Areas.EBanking.Controllers
{
    [Area("EBanking")]
    public class WalletController : Controller
    {
        private readonly AuthDbContext _context;

        public WalletController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: EBanking/Wallets
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Wallets.Include(w => w.ConnectedUser).Include(w => w.Owner);
            return View(await authDbContext.ToListAsync());
        }

        // GET: EBanking/Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.ConnectedUser)
                .Include(w => w.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // GET: EBanking/Wallets/Create
        public IActionResult Create()
        {
            ViewData["ConnectedUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: EBanking/Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OwnerId,ConnectedUserId")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConnectedUserId"] = new SelectList(_context.Users, "Id", "Id", wallet.ConnectedUserId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", wallet.OwnerId);
            return View(wallet);
        }

        // GET: EBanking/Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["ConnectedUserId"] = new SelectList(_context.Users, "Id", "Id", wallet.ConnectedUserId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", wallet.OwnerId);
            return View(wallet);
        }

        // POST: EBanking/Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OwnerId,ConnectedUserId")] Wallet wallet)
        {
            if (id != wallet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id))
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
            ViewData["ConnectedUserId"] = new SelectList(_context.Users, "Id", "Id", wallet.ConnectedUserId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", wallet.OwnerId);
            return View(wallet);
        }

        // GET: EBanking/Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.ConnectedUser)
                .Include(w => w.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: EBanking/Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletExists(int id)
        {
            return _context.Wallets.Any(e => e.Id == id);
        }
    }
}
