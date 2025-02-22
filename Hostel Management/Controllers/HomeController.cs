using Microsoft.AspNetCore.Mvc;

namespace Hostel_Management.Controllers
{
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
