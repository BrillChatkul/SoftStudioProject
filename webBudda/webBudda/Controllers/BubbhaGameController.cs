using Microsoft.AspNetCore.Mvc;

namespace webBudda.Controllers
{
    public class BubbhaGameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SpinnerGame()
        {
            return View();
        }

        public IActionResult CardMemoryGame()
        {
            return View();
        }

        public IActionResult BlockGame()
        {
            return View();
        }

    }
}
