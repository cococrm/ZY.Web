using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.Web.MVC.Controllers;
using ZY.Web.MVC.Filter;

namespace ZY.Web.MVC.Areas.Systems.Controllers
{
    public class SysModuleController : BaseController
    {

        [UserPermission(Module = "SystemModule")]
        public ActionResult Index()
        {
            return View();
        }

        [UserPermission(Module = "SystemModule",Operation = "Add")]
        public ActionResult Add()
        {
            return View("Form");
        }

        [UserPermission(Module = "SystemModule", Operation = "Update")]
        public ActionResult Update(Guid id)
        {
            ViewBag.Action = "Update";
            return View("Form");
        }

        public ActionResult UserModule(int id)
        {
            ViewBag.getUrl = "sysModules/getUserModule?id=" + id;
            ViewBag.saveUrl = "sysModules/saveUserModule";
            return View("SetModule");
        }

        public ActionResult RoleModule(int id)
        {
            ViewBag.getUrl = "sysModules/getRoleModule?id=" + id;
            ViewBag.saveUrl = "sysModules/saveRoleModule";
            return View("SetModule");
        }
    }
}