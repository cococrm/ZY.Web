using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.Web.MVC.Controllers;

namespace ZY.Web.MVC.Areas.Systems.Controllers
{
    public class UserController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit");
        }
        
        public ActionResult Update(int id)
        {
            ViewBag.Action = "Update";
            return View("Edit");
        }
    }
}