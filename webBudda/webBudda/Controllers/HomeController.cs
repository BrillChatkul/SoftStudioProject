﻿using Microsoft.AspNetCore.Mvc;
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
            blogRepo blogRepo = new blogRepo();
            return View(blogRepo.GetBlogList());
        }

        public ActionResult ModalPopUp() { return View(); }

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

        public IActionResult Travel()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Viewblog(int id)
        {
            blog blogger = new blog();
            blogRepo blogRepo = new blogRepo();
            List<blog> blogList = blogRepo.GetBlogList();
            
            foreach (blog blog in blogList)
            {
                if (blog.Id == id)
                {
                    blogger = blog;
                }
            }
            List<Comment> comments = new List<Comment>();
            if(blogger.CommentList != null)
            {
                comments = blogger.CommentList.ToList();
            }
            ViewBag.blogComment = comments.ToList();
            ViewBag.blog = blogger;
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}