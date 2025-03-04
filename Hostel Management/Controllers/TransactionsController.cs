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

namespace Hostel_Management.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AuthDbContext _context;

        public TransactionsController(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? Id)
        {
            ViewBag.id = Id;
            var transactions = _context.Transactions
                .Where(t => Id == null || t.WalletId == Id)
                .Include(t => t.Currency)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount);
            return View(await transactions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = await _context.Transactions
                .Include(t => t.Currency)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(m => m.Id == id);

            return transaction == null ? NotFound() : View(transaction);
        }

        public IActionResult Create(int id)
        {
            ViewBag.WalletId = id;
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name");
            ViewData["FromAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountDisplay");
            ViewData["ToAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountDisplay");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionDTO tran)
        {
            if (!ModelState.IsValid)
                return View(tran);


            var transaction = new Transaction
            {
                WalletId = tran.WalletId,
                CurrencyId = tran.CurrencyId,
                Amount = tran.Amount,
                FromAccountId = tran.FromAccountId,
                ToAccountId = tran.ToAccountId,
                Timestamp = DateTime.UtcNow
            };

            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { Id = transaction.WalletId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", transaction.CurrencyId);
            ViewData["FromAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountDisplay", transaction.FromAccountId);
            ViewData["ToAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountDisplay", transaction.ToAccountId);
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,FromAccountId,ToAccountId,CurrencyId,Timestamp")] Transaction transaction)
        {
            if (id != transaction.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(transaction);

            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = await _context.Transactions
                .Include(t => t.Currency)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(m => m.Id == id);

            return transaction == null ? NotFound() : View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
