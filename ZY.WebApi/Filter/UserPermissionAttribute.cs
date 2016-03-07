using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using ZY.WebApi.Autofac;
using ZY.Core.Autofac;
using System.Net;
using System.Net.Http;
using ZY.Core.Web.Model;
using ZY.Identity;
using ZY.Core.Extensions;

namespace ZY.WebApi.Filter
{
    /// <summary>
    /// 账号具体权限验证
    /// </summary>
    public class UserPermissionAttribute : AuthorizeAttribute
    {
        private string operation;
        /// <summary>
        /// 操作模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Operation
        {
            get
            {
                if (operation == null)
                    operation = OperationCode.Show;
                return operation;
            }
            set
            {
                operation = value;
            }
        }

        //重写验证方法
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //检查是否登陆
            if (!base.IsAuthorized(actionContext))
            {
                var message = new AjaxResponse(AjaxResponseStatus.NotLogin, "你没有登录，请先登录！");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, message, "application/json");
                return;
            }

            if (this.Module.IsNullOrEmpty())
            {
                return;
            }

            //判断权限
            if (!UserAuthorize.IsAuthorized(Module, Operation))
            {
                var message = new AjaxResponse(AjaxResponseStatus.NotAuthorized, "你没有权限操作！");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, message, "application/json");
                return;
            }
        }
    }
}
