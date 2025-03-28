﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace Hostel_Management.Controllers.Users
{
    [Authorize]
    public class UserWalletsController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserWalletsController(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var authDbContext = _context.UserWallets
                .Include(u => u.Currency)
                .Where(u=>u.UserId==user.Id)
                .Include(u => u.Transactions); // Include Transactions

            return View(await authDbContext.ToListAsync());
        }

        // GET: UserWallets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWallet = await _context.UserWallets
                .Include(u => u.Currency)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userWallet == null)
            {
                return NotFound();
            }

            return View(userWallet);
        }

        // GET: UserWallets/Create
        public IActionResult Create()
        {
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code");
            return View();
        }

        // POST: UserWallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserWalletDTO userWal)
        {

            var user = await _userManager.GetUserAsync(User);
            var userWallet = new UserWallet();
            if (ModelState.IsValid)
            {
                userWallet.Id = Guid.NewGuid();

                userWallet.UserId = user.Id;
                userWallet.CurrencyId = userWal.CurrencyId;
                userWallet.Name = userWal.Name;
                userWallet.Timestamp = DateTime.Now;
                _context.Add(userWallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code", userWallet.CurrencyId);
            return View(userWallet);
        }

        // GET: UserWallets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWallet = await _context.UserWallets.FindAsync(id);
            if (userWallet == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code", userWallet.CurrencyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userWallet.UserId);
            return View(userWallet);
        }

        // POST: UserWallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,CurrencyId,UserId,Timestamp")] UserWallet userWallet)
        {
            if (id != userWallet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userWallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserWalletExists(userWallet.Id))
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
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code", userWallet.CurrencyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userWallet.UserId);
            return View(userWallet);
        }

        // GET: UserWallets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWallet = await _context.UserWallets
                .Include(u => u.Currency)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userWallet == null)
            {
                return NotFound();
            }

            return View(userWallet);
        }

        // POST: UserWallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userWallet = await _context.UserWallets.FindAsync(id);
            if (userWallet != null)
            {
                _context.UserWallets.Remove(userWallet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserWalletExists(Guid id)
        {
            return _context.UserWallets.Any(e => e.Id == id);
        }
    }
}
