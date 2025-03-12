using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Hostel_Management.Models.Model;

namespace Hostel_Management.Controllers.Users
{
    public class UserTransactionsController : Controller
    {
        private readonly AuthDbContext _context;

        public UserTransactionsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: UserTransactions
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.UserTransactions.Include(u => u.Wallet);
            return View(await authDbContext.ToListAsync());
        }

        // GET: UserTransactions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTransaction == null)
            {
                return NotFound();
            }

            return View(userTransaction);
        }

        // GET: UserTransactions/Create
        public IActionResult Create()
        {
            ViewData["WalletId"] = new SelectList(_context.UserWallets, "Id", "Name");
            return View();
        }

        // POST: UserTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WalletId,Amount,TransactionType,Note,CreatedAt")] UserTransaction userTransaction)
        {
            if (ModelState.IsValid)
            {
                userTransaction.Id = Guid.NewGuid();
                _context.Add(userTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WalletId"] = new SelectList(_context.UserWallets, "Id", "Name", userTransaction.WalletId);
            return View(userTransaction);
        }

        // GET: UserTransactions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions.FindAsync(id);
            if (userTransaction == null)
            {
                return NotFound();
            }
            ViewData["WalletId"] = new SelectList(_context.UserWallets, "Id", "Name", userTransaction.WalletId);
            return View(userTransaction);
        }

        // POST: UserTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,WalletId,Amount,TransactionType,Note,CreatedAt")] UserTransaction userTransaction)
        {
            if (id != userTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTransactionExists(userTransaction.Id))
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
            ViewData["WalletId"] = new SelectList(_context.UserWallets, "Id", "Name", userTransaction.WalletId);
            return View(userTransaction);
        }

        // GET: UserTransactions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTransaction == null)
            {
                return NotFound();
            }

            return View(userTransaction);
        }

        // POST: UserTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userTransaction = await _context.UserTransactions.FindAsync(id);
            if (userTransaction != null)
            {
                _context.UserTransactions.Remove(userTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTransactionExists(Guid id)
        {
            return _context.UserTransactions.Any(e => e.Id == id);
        }
    }
}
