using Firebase.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webBudda.Models;


namespace webBudda.Controllers
{
    public class AdminController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "qt3bdVVIXN4rcf0NACZkQLivDFnyKkZerngugoLM",
            BasePath = "https://budbudworld-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
        FirebaseAuthProvider auth;
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyAYbfDjXRx9S0dnwub_BH5bk75rJMPDAbU"));
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.Email = user.Email;
                if(user.Email == "admin@budworld.com")
                {
                    return View();
                }
            }
                        
            return RedirectToAction("Index","Home");
        }
        //create blog
        [HttpPost]
        public ActionResult Create(blog blog)
        {
            try
            {
                addBlogToFirebase(blog);
                ModelState.AddModelError(string.Empty, "Added successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("ContentAdmin","Admin");
        }
        private void addBlogToFirebase(blog blog)
        {
            blog.Created = DateTime.Now.ToString();
            client = new FireSharp.FirebaseClient(config);
            var data = blog;
            PushResponse response = client.Push("blogData/", data);
            data.Id = response.Result.name;
            data.topfeed = false;
            SetResponse setResponse = client.Set("blogData/" + data.Id, data);
        }
        
        public ActionResult setTopFeed(string id,Boolean oldstatus)
        {
            if(oldstatus == true) { oldstatus = false; }
            else { oldstatus = true; }
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("blogData/" + id + "/topfeed", oldstatus);
            return RedirectToAction("ContentAdmin");
        }

        public ActionResult Deleteblog(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("blogData/" + id);
            return RedirectToAction("ContentAdmin");
        }

        public async Task<IActionResult> ContentAdmin()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.Email = user.Email;
                if (user.Email == "admin@budworld.com")
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("blogData");
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<blog>();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<blog>(((JProperty)item).Value.ToString()));
                        }
                    }
                    return View(list);
                }
            }

            return RedirectToAction("Index", "Home");
            
        }

        public async Task<ActionResult> EditBlog(string id)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.Email = user.Email;
                if (user.Email == "admin@budworld.com")
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("blogData/" + id);
                    blog data = JsonConvert.DeserializeObject<blog>(response.Body);
                    return View(data);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult EditBlog(blog blog)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("blogData/" + blog.Id, blog);
            return RedirectToAction("ContentAdmin");
        }
        public async Task<ActionResult> ManageUser()
        {
            return View();
        }
        public async Task<IActionResult> AdminViewBlog(string id)
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

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("blogData/" + id);
            blog data = JsonConvert.DeserializeObject<blog>(response.Body);
            ViewBag.blogComment = new List<Comment>();
            ViewBag.blog = new blog();
            if (data != null)
            {
                data.UserLike = false;
                data.Like = 0;

                response = client.Get("LikeList"); //GET LIKE
                dynamic LikeList = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var listLikeall = new List<Likeblog>();
                var listLike = new List<Likeblog>();
                if (LikeList != null)
                {
                    foreach (var item in LikeList)
                    {
                        listLikeall.Add(JsonConvert.DeserializeObject<Likeblog>(((JProperty)item).Value.ToString()));
                    }
                    foreach (var item in listLikeall)
                    {
                        if (item.Idblog == id)
                        {
                            listLike.Add(item);
                            if (item.Email == ViewBag.Email)
                            {
                                data.UserLike = true;
                            }
                        }
                    }
                    data.Like = listLike.Count;
                }

                response = client.Get("LikeCommentList");//Get like
                dynamic LikeCommentList = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var listlikeCommentAll = new List<LikeComment>();

                if (LikeCommentList != null)
                {
                    foreach (var item in LikeCommentList)
                    {
                        listlikeCommentAll.Add(JsonConvert.DeserializeObject<LikeComment>(((JProperty)item).Value.ToString()));
                    }
                }

                response = client.Get("commentBlog");//Get comment
                dynamic CommentList = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var listCommentAll = new List<Comment>();
                var listComment = new List<Comment>();
                if (CommentList != null)
                {
                    foreach (var item in CommentList)
                    {
                        listCommentAll.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
                    }
                    foreach (var item in listCommentAll)
                    {
                        if (item.blogID == id)
                        {
                            item.UserLike = false;
                            var listlikeComment = new List<LikeComment>();
                            foreach (var like in listlikeCommentAll)
                            {
                                if (item.Id == like.IdComment)
                                {
                                    listlikeComment.Add(like);
                                    if (like.Email == ViewBag.Email)
                                    {
                                        item.UserLike = true;
                                    }
                                }

                            }
                            item.Like = listlikeComment.Count;
                            listComment.Add(item);

                        }
                    }
                }
                data.CommentList = listComment;
                ViewBag.blogComment = data.CommentList.ToList();
                ViewBag.blog = data;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> sentComment(Comment comment)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                try
                {   //string id,string name,string email,string comment
                    addCommentToFirebase(comment);
                    ModelState.AddModelError(string.Empty, "Added successfully");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToAction("AdminViewblog", "Admin", new { id = comment.blogID });
            }
            else
            {
                return RedirectToAction("SignIn", "Home");
            }


        }

        private void addCommentToFirebase(Comment comment)
        {
            comment.Created = DateTime.Now.ToString();
            client = new FireSharp.FirebaseClient(config);
            var data = comment;
            PushResponse response = client.Push("commentBlog/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("commentBlog/" + data.Id, data);
        }

        public ActionResult addLikeToFirebase(string id, string email)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                Likeblog likeblog = new Likeblog { Idblog = id, Email = email };

                Boolean likeUser = false;
                FirebaseResponse GetResponse = client.Get("LikeList");
                dynamic LikeList = JsonConvert.DeserializeObject<dynamic>(GetResponse.Body);
                var listLikeall = new List<Likeblog>();
                var listLike = new List<Likeblog>();
                if (LikeList != null)
                {
                    foreach (var item in LikeList)
                    {
                        listLikeall.Add(JsonConvert.DeserializeObject<Likeblog>(((JProperty)item).Value.ToString()));
                    }
                    foreach (var item in listLikeall)
                    {
                        if (item.Idblog == id)
                            if (item.Email == email)
                            {
                                likeUser = true;
                                likeblog.Id = item.Id;
                            }
                    }
                }
                if (likeUser == true)
                {
                    GetResponse = client.Delete("LikeList/" + likeblog.Id);
                }
                else
                {
                    PushResponse response = client.Push("LikeList/", likeblog);
                    likeblog.Id = response.Result.name;
                    SetResponse setResponse = client.Set("LikeList/" + likeblog.Id, likeblog);
                }
                return RedirectToAction("AdminViewblog", "Admin", new { id = id });
            }


        }

        public ActionResult addLikeComment(string id, string email, string blogid)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                LikeComment likeblog = new LikeComment { IdComment = id, Email = email };

                Boolean likeUser = false;
                FirebaseResponse GetResponse = client.Get("LikeCommentList");
                dynamic LikeList = JsonConvert.DeserializeObject<dynamic>(GetResponse.Body);
                var listLikeall = new List<LikeComment>();
                var listLike = new List<LikeComment>();
                if (LikeList != null)
                {
                    foreach (var item in LikeList)
                    {
                        listLikeall.Add(JsonConvert.DeserializeObject<LikeComment>(((JProperty)item).Value.ToString()));
                    }
                    foreach (var item in listLikeall)
                    {
                        if (item.IdComment == id)
                            if (item.Email == email)
                            {
                                likeUser = true;
                                likeblog.Id = item.Id;
                            }
                    }
                }
                if (likeUser == true)
                {
                    GetResponse = client.Delete("LikeCommentList/" + likeblog.Id);
                }
                else
                {
                    PushResponse response = client.Push("LikeCommentList/", likeblog);
                    likeblog.Id = response.Result.name;
                    SetResponse setResponse = client.Set("LikeCommentList/" + likeblog.Id, likeblog);
                }
                return RedirectToAction("AdminViewblog", "Admin", new { id = blogid });
            }
        }

        public async Task<ActionResult> DeleteComment(string id, string email, string blogid)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                try
                {
                    client = new FireSharp.FirebaseClient(config);
                    User user = await auth.GetUserAsync(token);
                    if (user.Email == "admin@budworld.com")
                    {
                        FirebaseResponse response = client.Delete("commentBlog/" + id);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToAction("AdminViewblog", "Admin", new { id = blogid });
            }
            else
            {
                return RedirectToAction("AdminViewblog", "Admin", new { id = blogid });
            }
        }
        public async Task<ActionResult> SetonFeedComment(string id, string blogid, Boolean feed)
        {
            if (feed == true) { feed = false; }
            else { feed = true; }
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("commentBlog/" + id + "/onfeed", feed);
            return RedirectToAction("AdminViewblog", "Admin", new { id = blogid });
        }
    }
}
