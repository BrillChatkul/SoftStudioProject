using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webBudda.Models;

namespace webBudda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FirebaseAuthProvider auth;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAYbfDjXRx9S0dnwub_BH5bk75rJMPDAbU"));
        }

         public async Task<IActionResult> Index()
        {

            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                //User user = await auth.GetUserAsync(token);
                User user = await auth.GetUserAsync(token);
                return View(user);
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            //create the user
            await auth.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password, userModel.Name);
            //log in the new user
            var fbAuthLink = await auth
                            .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            string token = fbAuthLink.FirebaseToken;
            //saving the token in a session variable
            if (token != null)
            {
                HttpContext.Session.SetString("_UserToken", token);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserModel userModel)
        {
            try
            {
                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                if (fbAuthLink != null)
                {
                    string token = fbAuthLink.FirebaseToken;
                    if (token != null)
                    {
                        HttpContext.Session.SetString("_UserToken", token);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex) { }
            return View();
        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("SignIn");
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