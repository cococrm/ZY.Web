using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public string TokenStr { get; set; }
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult Token()
        {
            HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("grant_type", "password"));
            param.Add(new KeyValuePair<string, string>("username", "zhangsan"));
            param.Add(new KeyValuePair<string, string>("password", "123123"));
            var res = client.PostAsync("http://localhost:16169/token", new FormUrlEncodedContent(param)).Result;

            return Json(res.Content.ReadAsStringAsync().Result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList()
        {
            HttpClient client = new HttpClient();
            var res = client.GetAsync("http://localhost:16169/api/users/list",new HttpCompletionOption() { }).Result;

            return Json(res.Content.ReadAsStringAsync().Result, JsonRequestBehavior.AllowGet);
        }
    }
}