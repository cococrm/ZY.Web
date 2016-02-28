using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZY.Core.Web.Model;
using ZY.Identity;

namespace ZY.Web.MVC.Filter
{
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
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!UserAuthorize.IsAuthorized(Module, Operation))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new AjaxResponse(AjaxResponseStatus.NotAuthorized, "你没有权限操作！"),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Error/NotAuthorized");
                }
            }
        }


    }
}
