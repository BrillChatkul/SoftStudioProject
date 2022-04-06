using buddaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace buddaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
            // UMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
        }

        public IActionResult Content()
        {
            return View();
        }
        
        public IActionResult Travel()
        {
            return View();
        }

        public IActionResult Viewblog()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}