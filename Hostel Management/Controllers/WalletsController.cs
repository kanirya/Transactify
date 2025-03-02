using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Hostel_Management.Models.Model;
using Hostel_Management.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Hostel_Management.Areas.Identity.Data;

namespace Hostel_Management.Controllers
{
    public class WalletsController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public WalletsController(AuthDbContext context ,UserManager<ApplicationUser>  userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Wallets.Include(w => w.ConnectedUser).Include(w => w.Owner);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Wallets/Details/5
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

        // GET: Wallets/Create
        public IActionResult Create()
        {
            ViewData["ConnectedUserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WalletDTO wal)
        {
            var wallet = new Wallet();
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                wallet.Name = wal.Name;
                wallet.OwnerId = user.Id ;
                wallet.ConnectedUserId = wal.ConnectedUserId;
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConnectedUserId"] = new SelectList(_context.Users, "Id", "Name", wallet.ConnectedUserId);
            return View(wallet);
        }

        // GET: Wallets/Edit/5
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

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,ConnectedUserId")] Wallet wallet)
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

        // GET: Wallets/Delete/5
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

        // POST: Wallets/Delete/5
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
