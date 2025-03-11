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
    public class TransactionsController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TransactionsController(AuthDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? Id)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.id = Id;
            ViewBag.CurrentUserId = user?.Id;
            var transactions = _context.Transactions
                .Where(t => Id == null || t.WalletId == Id)
                .Include(t => t.Currency)
                .Include(t => t.FromAccount)
                .Include(t=>t.User)
                .Include(t => t.ToAccount).OrderByDescending(u=>u.Timestamp);
          
            ViewBag.SendAmount = await transactions.Where(t => t.WalletId == Id).Where(t=>t.UserId==user.Id).SumAsync(t => (decimal?)t.Amount) ?? 0;
            ViewBag.ReceiveAmount = await transactions.Where(t => t.WalletId == Id).Where(t=>t.UserId!=user.Id).SumAsync(t => (decimal?)t.Amount) ?? 0;
            ViewBag.TotalAmount = ViewBag.SendAmount - ViewBag.ReceiveAmount;
            ViewBag.TransactionsCount = await transactions.Where(t => t.WalletId == Id).CountAsync();
            ViewBag.ThisUser = await _context.Wallets
        .Include(w => w.Owner)           // Load Owner
              .Include(w => w.ConnectedUser)   // Load Connected User
             .FirstOrDefaultAsync(w => w.Id == Id);
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

        public async Task<IActionResult> Create(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(); // Handle unauthenticated user
            ViewBag.WalletId = id;
            var opposite_user = await _context.Wallets.FindAsync(id);
            if (opposite_user == null) return NotFound(); // Handle invalid wallet ID

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name");
            ViewData["FromAccountId"] = new SelectList(
                _context.BankAccounts.Where(u => u.UserId == user.Id), "Id", "AccountDisplay"
            );
            ViewData["ToAccountId"] = new SelectList(
                _context.BankAccounts.Where(u => u.UserId == opposite_user.ConnectedUserId), "Id", "AccountDisplay"
            );

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionDTO tran)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
                return View(tran);

            var opposite_Account = _context.BankAccounts.Find(tran.ToAccountId);
            opposite_Account.Balance += tran.Amount;
            var Owner_Account = _context.BankAccounts.Find(tran.FromAccountId);
            Owner_Account.Balance -= tran.Amount;

            var transaction = new Transaction
            {
                WalletId = tran.WalletId,
                CurrencyId = tran.CurrencyId,
                Amount = tran.Amount,
                FromAccountId = tran.FromAccountId,
                ToAccountId = tran.ToAccountId,
                UserId=user.Id,
                Timestamp = DateTime.Now
            };
            _context.Update(opposite_Account);
            _context.Update(Owner_Account);

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

            int walletId = transaction.WalletId; // Get the WalletId before deletion

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { Id = walletId }); // Redirect to the correct wallet
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
