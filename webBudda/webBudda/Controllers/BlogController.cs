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
    public class blogController : Controller
    {

        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "qt3bdVVIXN4rcf0NACZkQLivDFnyKkZerngugoLM",
            BasePath = "https://budbudworld-default-rtdb.firebaseio.com"
        };
        FirebaseAuthProvider auth;
        IFirebaseClient client;

        private readonly ILogger<blogController> _logger;
        public blogController(ILogger<blogController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyAYbfDjXRx9S0dnwub_BH5bk75rJMPDAbU"));
        }
        [HttpGet]
        public IActionResult Index(String typep)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("blogData");
            dynamic data =JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<blog>();
            if(data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<blog>(((JProperty)item).Value.ToString()));
                }
            }
            var showlist = new List<blog>();
            foreach (var item in list)
            {
                if(item.typep == typep)
                {
                    showlist.Add(item);
                }
            }
            return View(showlist);
        }
        [HttpGet]
        public async Task<IActionResult> Viewblog(string id)
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
            if(token != null)
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
                return RedirectToAction("Viewblog", "Blog", new { id = comment.blogID });
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
            if (token == null) {
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
                return RedirectToAction("Viewblog", "Blog", new { id = id });
            }
            

        }

        public ActionResult addLikeComment(string id, string email,string blogid)
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
                return RedirectToAction("Viewblog", "Blog", new { id = blogid });
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
                    if (user.Email == email) {
                        FirebaseResponse response = client.Delete("commentBlog/" + id);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToAction("Viewblog", "Blog", new { id = blogid });
            }
            else
            {
                return RedirectToAction("Viewblog", "Blog", new { id = blogid });
            }
        }


        //public IActionResult Content(string typep)
        //{
        //    blogRepo blogRepo = new blogRepo();
        //    List<blog> blogList = blogRepo.GetBlogList();
        //    List<blog> blogFilter = new List<blog>();
        //    foreach (blog blog in blogList)
        //    {
        //        if (blog.typep == typep)
        //        {
        //            blogFilter.Add(blog);
        //        }
        //    }
        //    return View(blogFilter.ToList());
        //}


    }
}
