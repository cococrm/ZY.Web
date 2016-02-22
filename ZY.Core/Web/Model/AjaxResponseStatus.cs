using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Core.Web.Model
{
    public static class AjaxResponseStatus
    {
        /// <summary>
        /// 成功状态
        /// </summary>
        public readonly static int Success = 200;
        /// <summary>
        /// 系统错误
        /// </summary>
        public readonly static int SystemError = 500;
        /// <summary>
        /// 未登陆
        /// </summary>
        public readonly static int NotLogin = 401;
        /// <summary>
        /// 未授权
        /// </summary>
        public readonly static int NotAuthorized = 403;
        /// <summary>
        /// 未找到
        /// </summary>
        public readonly static int NotFound = 404;
        /// <summary>
        /// 参数验证错误
        /// </summary>
        public readonly static int ValidError = 100;
        /// <summary>
        /// 
        /// </summary>
        public readonly static int Error = 0;
    }
}
