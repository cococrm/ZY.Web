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
        /// <summary>
        /// 操作模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Operation { get; set; }

        public UserPermissionAttribute() { }

        public UserPermissionAttribute(string moudle)
            : this(moudle, OperationCode.Show)
        {

        }

        public UserPermissionAttribute(string moudle, string operation)
        {
            this.Module = moudle;
            this.Operation = operation;
        }
        //重写验证方法
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
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

            //获取当前账号Id
            var userId = ClaimsUser.UserId;
            var permissionCheck = IocManager.Resolve<IPermissionCheck>(new AuthorizedModule());
            if (!permissionCheck.IsGranted(userId, Module, Operation))
            {
                var message = new AjaxResponse(AjaxResponseStatus.NotAuthorized, "你没有权限操作！");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, message, "application/json");
                return;
            }
        }

        ///// <summary>
        ///// 权限验证
        ///// </summary>
        ///// <param name="actionContext"></param>
        ///// <returns></returns>
        //protected override bool IsAuthorized(HttpActionContext actionContext)
        //{            
        //    if (!base.IsAuthorized(actionContext))
        //        return false;
        //    try
        //    {
        //        //获取当前账号Id
        //        var userId = ClaimsUser.UserId;
        //        var permissionCheck = IocManager.Resolve<IPermissionCheck>(new AuthorizedModule());
        //        return permissionCheck.IsGranted(userId, Module, Operation);
        //    }
        //    catch
        //    {
        //        return false;
        //        throw new Exception();
        //    }
        //}
    }
}
