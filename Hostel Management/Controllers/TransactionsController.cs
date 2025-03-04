using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Hostel_Management.Models.Model;

namespace Hostel_Management.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AuthDbContext _context;

        public TransactionsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int? Id)
        {
            var authDbContext = _context.Transactions.Include(t => t.Currency).Include(t => t.FromAccount).Include(t => t.ToAccount);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Currency)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name");
            ViewData["FromAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber");
            ViewData["ToAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,FromAccountId,ToAccountId,CurrencyId,Timestamp")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", transaction.CurrencyId);
            ViewData["FromAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber", transaction.FromAccountId);
            ViewData["ToAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber", transaction.ToAccountId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", transaction.CurrencyId);
            ViewData["FromAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber", transaction.FromAccountId);
            ViewData["ToAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber", transaction.ToAccountId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,FromAccountId,ToAccountId,CurrencyId,Timestamp")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", transaction.CurrencyId);
            ViewData["FromAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber", transaction.FromAccountId);
            ViewData["ToAccountId"] = new SelectList(_context.BankAccounts, "Id", "AccountNumber", transaction.ToAccountId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Currency)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
