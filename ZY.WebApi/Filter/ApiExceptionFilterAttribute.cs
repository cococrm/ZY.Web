using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using ZY.Utils;
using ZY.Core.Web.Model;
using System.Web;

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
            content.AppendFormat("\r\nRequest：{0}", GetRequestValues(request));
            content.AppendFormat("\r\nUrl：{0}", request.Url.AbsoluteUri);
            _log.Error(content.ToString(), actionExecutedContext.Exception);//记录日志
            string message = JsonHelper.ToJson(new AjaxResponse(AjaxResponseStatus.Error, "服务器发生错误，请查看日志", actionExecutedContext.Exception.Message));
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(message)
            };
            base.OnException(actionExecutedContext);
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
