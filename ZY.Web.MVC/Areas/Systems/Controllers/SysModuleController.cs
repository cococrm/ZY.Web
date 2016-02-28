using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.Web.MVC.Controllers;

namespace ZY.Web.MVC.Areas.Systems.Controllers
{
    public class SysModuleController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View("Form");
        }

        public ActionResult Update(Guid id)
        {
            ViewBag.Action = "Update";
            return View("Form");
        }

        public ActionResult RoleModule(int id)
        {
            return View("SetModule");
        }
    }
}