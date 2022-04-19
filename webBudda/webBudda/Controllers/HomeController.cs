using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webBudda.Models;

namespace webBudda.Controllers
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

        public IActionResult Content1()
        {
            return View();
        }

        public IActionResult Content2()
        {
            return View();
        }

        public IActionResult Content2_2()
        {
            return View();
        }

        public IActionResult Content2_3()
        {
            return View();
        }

        public IActionResult Content2_4()
        {
            return View();
        }

        public IActionResult Content3()
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