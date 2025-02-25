using Hostel_Management.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hostel_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthDbContext _context;
        public AccountController(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Model = await _context.Accounts.ToListAsync();
            return View(Model);
        }

        [Route("Account/Details/{id}?{name}")]
        public string Details(string name, int id)
        {
            return "Id is: "+id + " Name is: "+name;
        }
        public string IdPass(int id)
        {
            return "Id is " + id;
        }
    }
}
