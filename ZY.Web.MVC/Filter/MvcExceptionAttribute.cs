using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZY.Core.Web.Model;
using ZY.Utils;

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
            content.AppendFormat("\r\nRequest：{0}", GetRequestValues(request));
            content.AppendFormat("\r\nUrl：{0}", request.Url.AbsoluteUri);
            _log.Error(content.ToString(), filterContext.Exception);//记录日志
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new AjaxResponse(AjaxResponseStatus.Error, "服务器发生错误，请查看日志", filterContext.Exception.Message),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.ExceptionHandled = true;
            }
        }
        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        private string GetRequestValues(HttpRequest request)
        {
            if (request.HttpMethod.ToUpper() == "POST")
            {
                return request.Form.ToString();
            }
            return "";
        }
    }
}
