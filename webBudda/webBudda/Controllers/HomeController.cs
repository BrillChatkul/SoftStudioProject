using Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using webBudda.Models;

namespace webBudda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "qt3bdVVIXN4rcf0NACZkQLivDFnyKkZerngugoLM",
            BasePath = "https://budbudworld-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
        FirebaseAuthProvider auth;
        IConfiguration _config { get; set; }
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAYbfDjXRx9S0dnwub_BH5bk75rJMPDAbU"));
            
        }

         public async Task<IActionResult> Index()
        {

            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                //User user = await auth.GetUserAsync(token);
                User user = await auth.GetUserAsync(token);
                if (user.Email == "admin@budworld.com")
                {
                    return RedirectToAction("Index", "Admin");
                }
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
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("EmailList");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<UserFirebase>();
            Boolean canAccess = true;
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<UserFirebase>(((JProperty)item).Value.ToString()));
                }
                foreach (var item in list)
                {
                    if (item.Email == userModel.Email)
                    {
                        canAccess = false;
                    }
                }
            }
            if(canAccess == true)
            {
                //create the user
                await auth.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password, userModel.Name);
                client = new FireSharp.FirebaseClient(config);
                PushResponse responseP = client.Push("EmailList/", userModel.Email);
                UserFirebase EmailRegister = new UserFirebase { Email = userModel.Email,Ban=false,password=userModel.Password};
                EmailRegister.Id = responseP.Result.name;
                SetResponse setResponse = client.Set("EmailList/" + EmailRegister.Id, EmailRegister);
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
            return View();
            
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserModel userModel)
        {
            //Check if email are ban
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("EmailList");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<UserFirebase>();
            Boolean canAccess = true;
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<UserFirebase>(((JProperty)item).Value.ToString()));
                }
                foreach (var item in list)
                {
                    if(item.Email == userModel.Email)
                    {
                        if(item.Ban == true)
                        {
                            canAccess = false;
                        }
                    }
                }
            }
            
            if (canAccess == true)
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
            }
            
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

        public async Task<IActionResult> EditUser()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                try
                {
                    //Get Firebase Configuration from appsettings.json and Bind to Firebase object
                    FirebaseJ fbconfig = new FirebaseJ();
                    _config.Bind("Firebase", fbconfig);

                    var json = JsonConvert.SerializeObject(fbconfig);
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(json),
                    });
                }
                catch { }
                User user = await auth.GetUserAsync(token);
                UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(user.Email);
                ViewBag.User = user.DisplayName;
                ViewBag.Email = user.Email;
                ViewBag.fa = false;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> EditUsername(string Name)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User Getuser = await auth.GetUserAsync(token);
                    try
                    {
                        //Get Firebase Configuration from appsettings.json and Bind to Firebase object
                        FirebaseJ fbconfig = new FirebaseJ();
                        _config.Bind("Firebase", fbconfig);

                        var json = JsonConvert.SerializeObject(fbconfig);
                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromJson(json),
                        });
                    }
                    catch { }
                    UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(Getuser.Email);
                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = userRecord.Uid,
                        DisplayName = Name
                    };
                    UserRecord userRecordUpdate = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(args);

                //Change name from comment
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("commentBlog");
                dynamic dataCM = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var listCM = new List<Comment>();
                if (dataCM != null)
                {
                    foreach (var item in dataCM)
                    {
                        listCM.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
                    }
                    foreach (var item in listCM)
                    {
                        if (item.Email == Getuser.Email)
                        {
                            SetResponse responseR = client.Set("commentBlog/" + item.Id + "/Name/", Name);
                        }
                    }
                }
                var data = new { status = true, statusToken = true };
                var json1 = JsonConvert.SerializeObject(data);
                return Json(json1);
                }
            var data1 = new { status = true, statusToken = false };
            var json2 = JsonConvert.SerializeObject(data1);
            return Json(json2);
        }

        
        public async Task<JsonResult> EditPassword(string Pold, string Pnew)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User Getuser = await auth.GetUserAsync(token);
                try
                {   //Get Firebase Configuration from appsettings.json and Bind to Firebase object
                    FirebaseJ fbconfig = new FirebaseJ();
                    _config.Bind("Firebase", fbconfig);

                    var json = JsonConvert.SerializeObject(fbconfig);
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(json),
                    });
                }
                catch { }
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("EmailList");
                dynamic dataCM = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var listCM = new List<UserFirebase>();
                var UserF = new UserFirebase();
                if (dataCM != null)
                {
                    foreach (var item in dataCM)
                    {
                        listCM.Add(JsonConvert.DeserializeObject<UserFirebase>(((JProperty)item).Value.ToString()));
                    }
                    foreach (var item in listCM)
                    {
                        if (item.Email == Getuser.Email)
                        {
                            UserF = item;
                        }
                    }
                }
                
                if (UserF.password == Pold)
                {
                    UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(Getuser.Email);
                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = userRecord.Uid,
                        Password = Pnew,
                    };
                    UserRecord userRecordUpdate = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(args);
                    SetResponse responseR = client.Set("EmailList/" + UserF.Id + "/password/", Pnew);
                    var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(UserF.Email, Pnew);
                    if (fbAuthLink != null)
                    {
                        string tokenk = fbAuthLink.FirebaseToken;
                        if (tokenk != null)
                        {
                            HttpContext.Session.SetString("_UserToken", tokenk);
                        }
                    }
                    var data = new { status = true, statusToken = true };
                    var json1 = JsonConvert.SerializeObject(data);
                    return Json(json1);
                }
                else
                {
                    var data1 = new { status = false, statusToken = true };
                    var json2 = JsonConvert.SerializeObject(data1);
                    return Json(json2);
                }
                               
            }
            var data2 = new { status = false, statusToken = false };
            var json3 = JsonConvert.SerializeObject(data2);
            return Json(json3);
        }

        public async Task<ActionResult> DeleteUser()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                try
                {
                    client = new FireSharp.FirebaseClient(config);
                    User user = await auth.GetUserAsync(token);
                    //Delete from comment
                    FirebaseResponse response = client.Get("EmailList");
                    dynamic dataEM = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var listEM = new List<Comment>();
                    if (dataEM != null)
                    {
                        foreach (var item in dataEM)
                        {
                            listEM.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
                        }
                        foreach (var item in listEM)
                        {
                            if (item.Email == user.Email)
                            {
                                response = client.Delete("EmailList/" + item.Id);
                            }
                        }
                    }
                    
                        //Delete from authen
                        try
                        {
                            //Get Firebase Configuration from appsettings.json and Bind to Firebase object
                            FirebaseJ fbconfig = new FirebaseJ();
                            _config.Bind("Firebase", fbconfig);

                            var json = JsonConvert.SerializeObject(fbconfig);
                            FirebaseApp.Create(new AppOptions()
                            {
                                Credential = GoogleCredential.FromJson(json),
                            });
                        }
                        catch { }

                        //Delete from comment
                        response = client.Get("commentBlog");
                        dynamic dataCM = JsonConvert.DeserializeObject<dynamic>(response.Body);
                        var listCM = new List<Comment>();
                        if (dataCM != null)
                        {
                            foreach (var item in dataCM)
                            {
                                listCM.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
                            }
                            foreach (var item in listCM)
                            {
                                if (item.Email == user.Email)
                                {
                                    response = client.Delete("commentBlog/" + item.Id);
                                }
                            }
                        }

                        //Delete from Likecomment
                        response = client.Get("LikeCommentList");
                        dynamic dataLC = JsonConvert.DeserializeObject<dynamic>(response.Body);
                        var listLC = new List<LikeComment>();
                        if (dataLC != null)
                        {
                            foreach (var item in dataLC)
                            {
                                listLC.Add(JsonConvert.DeserializeObject<LikeComment>(((JProperty)item).Value.ToString()));
                            }
                            foreach (var item in listLC)
                            {
                                if (item.Email == user.Email)
                                {
                                    response = client.Delete("LikeCommentList/" + item.Id);
                                }
                            }
                        }

                        //Delete from Likeblog
                        response = client.Get("LikeList");
                        dynamic dataLB = JsonConvert.DeserializeObject<dynamic>(response.Body);
                        var listLB = new List<Likeblog>();
                        if (dataLB != null)
                        {
                            foreach (var item in dataLB)
                            {
                                listLB.Add(JsonConvert.DeserializeObject<Likeblog>(((JProperty)item).Value.ToString()));
                            }
                            foreach (var item in listLB)
                            {
                                if (item.Email == user.Email)
                                {
                                    response = client.Delete("LikeList/" + item.Id);
                                }
                            }
                        }

                        UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(user.Email);
                        await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.DeleteUserAsync(userRecord.Uid);
                        HttpContext.Session.Remove("_UserToken");
                    return RedirectToAction("Index", "Home");
                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}