using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.Web.MVC.Controllers;
using ZY.Web.MVC.Filter;

namespace ZY.Web.MVC.Areas.Systems.Controllers
{
    public class RoleController : BaseController
    {
        [UserPermission(Module = "SystemRole")]
        public ActionResult Index()
        {            
            return View();
        }
    }
}