using System.Web;
using System.Web.Mvc;
using ZY.Web.MVC.Filter;

namespace ZY.Web.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcExceptionAttribute());
        }
    }
}
