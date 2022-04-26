using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;

namespace webBudda.Controllers
{
    public class BubbhaGameController : Controller
    {
        private readonly ILogger<BubbhaGameController> _logger;
        FirebaseAuthProvider auth;

        public BubbhaGameController(ILogger<BubbhaGameController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAYbfDjXRx9S0dnwub_BH5bk75rJMPDAbU"));
        }
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.user = user.DisplayName;
                ViewBag.Email = user.Email;
            }
            else
            {
                ViewBag.user = "";
                ViewBag.Email = "Unknown";
            }
            return View();
        }

        public async Task<IActionResult> SpinnerGame()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.user = user.DisplayName;
                ViewBag.Email = user.Email;
            }
            else
            {
                ViewBag.user = "";
                ViewBag.Email = "Unknown";
            }
            return View();
        }

        public async Task<IActionResult> CardMemoryGame()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.user = user.DisplayName;
                ViewBag.Email = user.Email;
            }
            else
            {
                ViewBag.user = "";
                ViewBag.Email = "Unknown";
            }
            return View();
        }

        public async Task<IActionResult> BlockGame()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.user = user.DisplayName;
                ViewBag.Email = user.Email;
            }
            else
            {
                ViewBag.user = "";
                ViewBag.Email = "Unknown";
            }
            return View();
        }

    }
}
