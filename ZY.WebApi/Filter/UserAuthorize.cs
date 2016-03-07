using ZY.Core.Autofac;
using ZY.Identity;
using ZY.WebApi.Autofac;

namespace ZY.WebApi.Filter
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
