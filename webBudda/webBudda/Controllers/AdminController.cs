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
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "qt3bdVVIXN4rcf0NACZkQLivDFnyKkZerngugoLM",
            BasePath = "https://budbudworld-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
        public ActionResult Create()
        {
            return View();
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

        public IActionResult ContentAdmin()
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
}
