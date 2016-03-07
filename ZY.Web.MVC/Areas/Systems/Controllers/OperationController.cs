using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZY.Web.MVC.Areas.Systems.Controllers
{
    public class OperationController : Controller
    {
        
        public ActionResult Index(Guid id)
        {
            return View();
        }
    }
}