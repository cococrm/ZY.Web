﻿using ZY.Core.Autofac;
using ZY.Identity;
using ZY.Web.MVC.Autofac;

namespace ZY.Web.MVC.Filter
{
    public class UserAuthorize
    {
        public static bool IsAuthorized(string module, string operation)
        {
            var permissionCheck = IocManager.Resolve<IPermissionCheck>(new AuthorizedModule());
            return permissionCheck.IsGranted(module, operation);
        }
    }
}
