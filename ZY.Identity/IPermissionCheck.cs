using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Identity
{
    /// <summary>
    /// 判断权限接口
    /// </summary>
    public interface IPermissionCheck
    {
        /// <summary>
        /// 根据用户Id，模块名称，操作类型 判断权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moudle"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task<bool> IsGrantedAsync(string moudle, string operation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moudle"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        bool IsGranted(string moudle, string operation);
    }
}
