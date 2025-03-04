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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Hostel_Management.Areas.Identity.Data;

namespace Hostel_Management.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CurrenciesController(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Currencies
        public async Task<IActionResult> Index()
        {
            var currencies = await _context.Currencies.ToListAsync();
            return View(currencies);
        }


        // GET: Currencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // GET: Currencies/Create
        public async Task<IActionResult> Create()
        {
            var currencies = new List<SelectListItem>();
            string apiKey = "e3099febd00359ce3a666d58"; // Replace with your actual API key
            string url = $"https://openexchangerates.org/api/currencies.json?app_id={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var currencyData = JObject.Parse(response);

                foreach (var currency in currencyData)
                {
                    currencies.Add(new SelectListItem
                    {
                        Value = currency.Key,
                        Text = $"{currency.Value} ({currency.Key})"
                    });
                }
            }

            ViewBag.CurrencyList = currencies.OrderBy(c => c.Text).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string selectedCurrencyCode)
        {
            if (string.IsNullOrEmpty(selectedCurrencyCode))
            {
                ModelState.AddModelError("", "Please select a currency.");
                return View();
            }

            // Fetch currency name again from API (if needed)
            string apiKey = "e3099febd00359ce3a666d58";
            string url = $"https://openexchangerates.org/api/currencies.json?app_id={apiKey}";
            string currencyName = selectedCurrencyCode;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var currencyData = JObject.Parse(response);
                if (currencyData.ContainsKey(selectedCurrencyCode))
                {
                    currencyName = currencyData[selectedCurrencyCode].ToString();
                }
            }
            var user = await _userManager.GetUserAsync(User);
            var newCurrency = new Currency
            {
                Code = selectedCurrencyCode,
                Name = currencyName,
                UserId = user.Id
            };

            _context.Currencies.Add(newCurrency);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect after saving
        }
    


   

        // GET: Currencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency != null)
            {
                _context.Currencies.Remove(currency);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currencies.Any(e => e.Id == id);
        }
    }
}
