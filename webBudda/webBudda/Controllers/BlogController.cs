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

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("blogData/"+id);
            blog data = JsonConvert.DeserializeObject<blog>(response.Body);
            
            response = client.Get("commentBlog");
            dynamic CommentList = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var listCommentAll = new List<Comment>();
            var listComment = new List<Comment>();
            foreach (var item in CommentList)
            {
                listCommentAll.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
            }
            foreach (var item in listCommentAll)
            {
            if(item.blogID == id)
            {
            listComment.Add(item);
            }
            }
            data.CommentList = listComment;
            ViewBag.blogComment = data.CommentList.ToList();
            ViewBag.blog = data;

            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = await auth.GetUserAsync(token);
                ViewBag.user = user.DisplayName;
            }
            else
            {
                ViewBag.user = "Unknown";
            }

            return View();
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
            return RedirectToAction("Index", "Travel");
        }
        private void addBlogToFirebase(blog blog)
        {
            blog.Created = DateTime.Now.ToString();
            client = new FireSharp.FirebaseClient(config);
            var data = blog;
            PushResponse response = client.Push("blogData/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("blogData/"+data.Id, data);
        }
    }
}
