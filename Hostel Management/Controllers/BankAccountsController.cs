using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Hostel_Management.Models.Model;
using Microsoft.AspNetCore.Identity;
using Hostel_Management.Areas.Identity.Data;
using Hostel_Management.Models.DTOs;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Hostel_Management.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;

        public BankAccountsController(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BankAccounts
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.BankAccounts.Include(b => b.Currency).Include(b => b.User);
            return View(await authDbContext.ToListAsync());
        }

        // GET: BankAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .Include(b => b.Currency)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        //api=e3099febd00359ce3a666d58

        public async Task<IActionResult> Create()
        {

            var user =await _userManager.GetUserAsync(User);
            ViewBag.CurrencyId = new SelectList(_context.Currencies.Where(u=>u.UserId==user.Id),"Id","Name");
            return View();
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,AccountNumber,Balance,CurrencyId")] BankAccountDTO bankAcc)
        {
            var bankAccount = new BankAccount();
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {

                bankAccount.AccountName = bankAcc.AccountName;
                bankAccount.AccountNumber = bankAcc.AccountNumber;
                bankAccount.Balance = bankAcc.Balance;
                bankAccount.UserId = user.Id;

                bankAccount.CurrencyId =int.Parse( bankAcc.CurrencyId);

                _context.Add(bankAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", bankAccount.CurrencyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Code", bankAccount.UserId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", bankAccount.CurrencyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bankAccount.UserId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountNumber,Balance,UserId,CurrencyId")] BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.Id))
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
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", bankAccount.CurrencyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bankAccount.UserId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .Include(b => b.Currency)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount != null)
            {
                _context.BankAccounts.Remove(bankAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }
    }
}
