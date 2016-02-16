using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Identity
{
    /// <summary>
    /// 登陆结果返回
    /// </summary>
    public enum LoginResultType
    {
        Success,
        Failed,
        NotAllowed,
        LockedOut
    }
}
