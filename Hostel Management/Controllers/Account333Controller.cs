using Hostel_Management.Areas.Identity.Data;
using Hostel_Management.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hostel_Management.Controllers
{
    [Authorize]
    public class Account333Controller : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<Account333Controller> _logger;

        public Account333Controller(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<Account333Controller> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Accounts.Include(a => a.Currency).Include(a => a.User).ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Currencies = new SelectList(_context.Currencies.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("AccountName,CurrencyId,Balance")] Account account)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogError("❌ User is NULL! Make sure you are logged in.");
                return RedirectToAction("Login", "Account");
            }

            account.UserId = user.Id;

            _logger.LogInformation($"✅ User ID Assigned: {account.UserId}");
            _logger.LogInformation($"Account Name: {account.AccountName}");
            _logger.LogInformation($"Currency ID: {account.CurrencyId}");
            _logger.LogInformation($"Balance: {account.Balance}");

            if (!ModelState.IsValid)
            {
                _logger.LogError("❌ ModelState is INVALID");
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        _logger.LogError($"Validation Error: {subError.ErrorMessage}");
                    }
                }

                ViewBag.Currencies = new SelectList(_context.Currencies.ToList(), "Id", "Name");
                return View(account);
            }

            _logger.LogInformation("✅ ModelState is VALID - Saving to DB");

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string IdPass(int id)
        {
            return "Id is " + id;
        }
    }
}
