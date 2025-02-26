using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hostel_Management.Controllers
{



    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }   
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
