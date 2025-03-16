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

namespace Hostel_Management.Controllers.Users
{
    public class UserTransactionsController : Controller
    {
        private readonly AuthDbContext _context;

        public UserTransactionsController(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home"); // Handle the case where id is null or empty
            }

            ViewBag.Id = id; // Safe to use now

            var transactions = await _context.UserTransactions
                .Include(u => u.Wallet)
                .ThenInclude(w => w.Currency) // Include Currency to access CurrencyCode
                .Where(u => u.WalletId == id.Value).OrderByDescending(u=>u.CreatedAt)
                .ToListAsync();



            // Get the Currency Code from the first transaction's Wallet
            var Code = transactions.FirstOrDefault()?.Wallet?.Currency?.Code ?? "$";

            if (Code == "PKR")
            {
                ViewBag.CurrencyCode = "Rs";
            }
            else if (Code == "USD")
            {
                ViewBag.CurrencyCode = "$";
            }
            else
            {
                ViewBag.CurrencyCode = Code;
            }

            return View(transactions);
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

        [HttpGet]
        [Route("UserTransactions/Create/{id?}")]
        public async Task<IActionResult> Create(Guid? id, Guid? walletId, string createdType)
        {
            ViewBag.Id = walletId;
            ViewBag.CreatedType = createdType;
            ViewBag.TransactionId = id;

            if (id.HasValue) // If id is provided, fetch the existing transaction for editing
            {
                var existingTransaction = await _context.UserTransactions.FindAsync(id.Value);
                if (existingTransaction == null)
                {
                    return NotFound();
                }
                return View(existingTransaction); // Return the view with existing transaction data
            }

            return View(new UserTransaction()); // Return an empty model for new transaction
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserTransactionDTO userTran, Guid? id)
        {
            var userTransaction = new UserTransaction();

            if (id.HasValue) // If id exists, update the existing transaction
            {
                userTransaction = await _context.UserTransactions.FindAsync(id.Value);
                if (userTransaction == null)
                {

                    Console.WriteLine("working fine");
                    return NotFound();
                }


                userTransaction.Note = userTran.Note;
                userTransaction.Amount = userTran.Amount;
                _context.Update(userTransaction);

            }
            else
            {
                Console.WriteLine("Wallet ID: " + userTran.WalletId);
                Console.WriteLine("Transaction type: " + userTran.TransactionType);

                if (ModelState.IsValid)
                {
                    if (userTran.TransactionType == "send")
                    {
                        userTransaction.TransactionType = TransactionType.Debit;
                    }
                    else if (userTran.TransactionType == "receive")
                    {
                        userTransaction.TransactionType = TransactionType.Credit;
                    }
                    else
                    {
                        return View(userTransaction);
                    }

                    userTransaction.WalletId = userTran.WalletId;
                    userTransaction.Id = Guid.NewGuid();
                    userTransaction.Note = userTran.Note;
                    userTransaction.Amount = userTran.Amount;
                    _context.Add(userTransaction);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = userTransaction.WalletId });
        }


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
                return RedirectToAction(nameof(Index), new { id = userTransaction.WalletId });
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
            return RedirectToAction(nameof(Index), new { id = userTransaction.WalletId });
        }

        private bool UserTransactionExists(Guid id)
        {
            return _context.UserTransactions.Any(e => e.Id == id);
        }
    }
}
