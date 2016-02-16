using System.Web;
using System.Web.Optimization;

namespace ZY.Web.MVC
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Css/css").Include(
                    "~/Scripts/easyui/themes/default/easyui.css",
                    "~/Scripts/easyui/themes/icon.css",
                    "~/Content/icon.css",
                    "~/Scripts/easyui/themes/color.css",
                    "~/Content/common.css"
                ));
        }
    }
}
