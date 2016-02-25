using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using ZY.Core.Web.Model;
using System.Web;
using ZY.Core.Logging;
using ZY.Core.Json;
using ZY.Core.Web;

namespace ZY.WebApi.Filter
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log;
        public ApiExceptionFilterAttribute()
        {
            this._log = new Log();
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            StringBuilder content = new StringBuilder();
            HttpRequest request = HttpContext.Current.Request;
            content.AppendFormat("ActionName：{0}", actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
            content.AppendFormat("\r\nControllerName：{0}", actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            content.AppendFormat("\r\nNavigator：{0}", request.UserAgent);
            content.AppendFormat("\r\nIp：{0}", request.UserHostAddress);
            content.AppendFormat("\r\nUrlReferrer：{0}", request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "");
            content.AppendFormat("\r\nRequest：{0}", Utils.GetRequestValues(request));
            content.AppendFormat("\r\nUrl：{0}", request.Url.AbsoluteUri);
            _log.Error(content.ToString(), actionExecutedContext.Exception);//记录日志
            string message = JsonHelper.ToJson(new AjaxResponse(AjaxResponseStatus.SystemError, "服务器发生错误，请查看日志", actionExecutedContext.Exception.Message));
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(message)
            };
        }
    }
}
