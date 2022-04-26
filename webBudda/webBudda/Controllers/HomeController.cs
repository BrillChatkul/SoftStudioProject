using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webBudda.Models;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

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
            //ViewData["events"] = new[]
            //{
            //    new Event { Id = 1, Title = "Video for Marisa", StartDate = "2020-11-14"},
            //    new Event { Id = 2, Title = "Preparation", StartDate = "2020-11-12"},
            //};

            return View();
        }

        //public JsonResult GetEvents()
        //{
        //    using (MyDatabaseEntities dc = new MyDatabaseEntities())
        //    {
        //        var events = dc.Events.ToList();
        //        return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //}

        //title: v.Subject,
        //                    description: v.Description,
        //                    start: moment(v.Start),
        //                    end: v.End != null ? moment(v.End) : null,
        //                    color: v.ThemeColor,
        //                    allDay : v.IsFullDay

        //public ActionResult GetEvents()
        //{
        //    return Json(
        //        new { title = "KRA JOOK MAK",description = "pai none",start = "2022-04-26",end = "null",color = "blue",allDay = "true" }
        //        );
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////
        //    private List<EventsModel> GetEvents()
        //    {
        //        var eventsList = new List<EventsModel>
        //        {
        //            new EventsModel
        //            {
        //                EventId = 1,
        //                Subject = "Ram",
        //                Description = "Mindfire Solutions",
        //                Start = "26/4/2022, 0:00:00",
        //                End = "NULL",
        //                ThemeColor = "blue",
        //                isFullDay = true
        //            },
        //            new EventsModel
        //            {
        //                EventId = 2,
        //                Subject = "chand",
        //                Description = "Mindfire Solutions",
        //                Start = "27/4/2022, 0:00:00",
        //                End = "NULL",
        //                ThemeColor = "red",
        //                isFullDay = true
        //            },
        //            new EventsModel
        //            {
        //                EventId = 3,
        //                Subject = "Abc",
        //                Description = "Abc Solutions",
        //                Start = "28/4/2022, 0:00:00",
        //                End = "NULL",
        //                ThemeColor = "green",
        //                isFullDay = true
        //            }
        //        };

        //        return Json(eventsList, new Newtonsoft.Json.JsonSerializerSettings());
        //}

        public ActionResult GetEvents()
        {
            return Json(new
            {
                EventId = 1,
                Subject = "Abc",
                Description = "Abc Solutions",
                Start = "28/4/2022, 0:00:00",
                End = "NULL",
                ThemeColor = "green",
                isFullDay = true
            });
        }

        //public ActionResult GetEvents()
        //{
        //    var events = Events();
        //    //return Json(events, JsonRequestBehavior.AllowGet);
        //    return Json(new { data = events, JsonRequestBehavior.AllowGet });
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public ActionResult PostCalendarData()
        {
            return Json(new { title = "Free Pizza", allday = "false", borderColor = "#5173DA", color = "#99ABEA", textColor = "#000000", description = "<p>This is just a fake description for the Free Pizza.</p><p>Nothing to see!</p>", start = "2015-01-04T22:00:49", end = "2015-01-01", url = "http=//www.mikesmithdev.com/blog/worst-job-titles-in-internet-and-info-tech/" });
        }

        //[HttpGet]
        //public ActionResult GetCalendarData()
        //{
        //    return Json(new { title = "Free Pizza", allday = "false", borderColor = "#5173DA", color = "#99ABEA", textColor = "#000000", description = "<p>This is just a fake description for the Free Pizza.</p><p>Nothing to see!</p>", start = "2015-01-04T22:00:49", end = "2015-01-01", url = "http=//www.mikesmithdev.com/blog/worst-job-titles-in-internet-and-info-tech/" }, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetCalendarData()
        //{
        //    return Json(new { title = "Free Pizza", allday = "false", borderColor = "#5173DA", color = "#99ABEA", textColor = "#000000", description = "<p>This is just a fake description for the Free Pizza.</p><p>Nothing to see!</p>", start = "2015-01-04T22:00:49", end = "2015-01-01", url = "http=//www.mikesmithdev.com/blog/worst-job-titles-in-internet-and-info-tech/" }, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult DATACRUD(string XmlParms)
        //{
        //    return Json(new { data = XmlParms });
        //}

        public IActionResult Content()
        {
            return View();
        }

        public IActionResult Travel()
        {
            return View();
        }

        public IActionResult Viewblog()
        {
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