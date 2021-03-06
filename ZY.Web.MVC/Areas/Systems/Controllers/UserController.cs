﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.Web.MVC.Controllers;
using ZY.Web.MVC.Filter;
using ZY.Identity;

namespace ZY.Web.MVC.Areas.Systems.Controllers
{
    public class UserController : BaseController
    {

        [UserPermission(Module = "SystemUser")]
        public ActionResult Index()
        {
            return View();
        }

        [UserPermission(Module = "SystemUser",Operation = "Add")]
        public ActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Form");
        }

        [UserPermission(Module = "SystemUser", Operation = "Update")]
        public ActionResult Update(int id)
        {
            ViewBag.Action = "Update";
            return View("Form");
        }
    }
}