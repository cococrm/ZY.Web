using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZY.Web.MVC.Controllers
{
    public class ErrorController : Controller
    {
        
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}