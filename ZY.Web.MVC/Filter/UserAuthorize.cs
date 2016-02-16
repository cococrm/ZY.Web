using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Core.Autofac;
using ZY.Identity;
using ZY.Web.MVC.Autofac;

namespace ZY.Web.MVC.Filter
{
    public class UserAuthorize
    {
        public static bool IsAuthorized(string module, string operation)
        {
            //获取当前账号Id
            var userId = ClaimsUser.UserId;
            var permissionCheck = IocManager.Resolve<IPermissionCheck>(new AuthorizedModule());
            return permissionCheck.IsGranted(userId, module, operation);
        }
    }
}
