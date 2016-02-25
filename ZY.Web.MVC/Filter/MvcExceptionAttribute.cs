using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZY.Core.Logging;
using ZY.Core.Web;
using ZY.Core.Web.Model;

namespace ZY.Web.MVC.Filter
{
    public class MvcExceptionAttribute : HandleErrorAttribute
    {
        private readonly ILog _log;
        public MvcExceptionAttribute()
        {
            this._log = new Log();
        }
        public override void OnException(ExceptionContext filterContext)
        {
            StringBuilder content = new StringBuilder();
            HttpRequest request = HttpContext.Current.Request;
            content.AppendFormat("\r\nNavigator：{0}", request.UserAgent);
            content.AppendFormat("\r\nIp：{0}", request.UserHostAddress);
            content.AppendFormat("\r\nUrlReferrer：{0}", request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "");
            content.AppendFormat("\r\nRequest：{0}", Utils.GetRequestValues(request));
            content.AppendFormat("\r\nUrl：{0}", request.Url.AbsoluteUri);
            _log.Error(content.ToString(), filterContext.Exception);//记录日志
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new AjaxResponse(AjaxResponseStatus.SystemError, "服务器发生错误，请查看日志", filterContext.Exception.Message),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult("/Error");
            }
            filterContext.ExceptionHandled = true;
        }
    }
}
