using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZY.Web.MVC.EasyUI
{
    public static class EasyUIHtmlExtensions
    {
        public static LinkButton LinkButton(this HtmlHelper helper, string id, string text)
        {
            return new LinkButton(id, text);
        }

        public static LinkButton LinkButton(this HtmlHelper helper, string text)
        {
            return new LinkButton(text);
        }

    }
}
