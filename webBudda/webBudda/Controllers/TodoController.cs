using Microsoft.AspNetCore.Mvc;

namespace webBudda.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }


    }
}
