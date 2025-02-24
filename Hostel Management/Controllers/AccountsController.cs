using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;

namespace Hostel_Management.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AuthDbContext _context;

        public AccountsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Accounts - only show accounts for the current user.
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var accounts = _context.Accounts
                .Include(a => a.Currency)
                .Include(a => a.User)
                .Where(a => a.UserId == currentUserId);
            return View(await accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var account = await _context.Accounts
                .Include(a => a.Currency)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null) return NotFound();

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            // Only show the Currency selection. The user is determined from the logged-in user.
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,CurrencyId,Balance")] Account account)
        {
            // Automatically assign the logged-in user's ID
            account.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code", account.CurrencyId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code", account.CurrencyId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountName,CurrencyId,Balance")] Account account)
        {
            if (id != account.Id) return NotFound();

            // Ensure the account remains associated with the logged-in user.
            account.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Accounts.Any(e => e.Id == account.Id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code", account.CurrencyId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var account = await _context.Accounts
                .Include(a => a.Currency)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null) return NotFound();

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
