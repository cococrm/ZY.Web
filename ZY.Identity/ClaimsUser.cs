using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace ZY.Identity
{
    /// <summary>
    /// 登陆账号信息
    /// </summary>
    public class ClaimsUser
    {
        /// <summary>
        /// 当前登录账号Id
        /// </summary>
        public static int UserId
        {
            get
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return 0;
                }

                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
                if (claimsIdentity == null)
                {
                    return 0;
                }

                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                {
                    return 0;
                }

                int userId;
                if (!int.TryParse(userIdClaim.Value, out userId))
                {
                    return 0;
                }

                return userId;
            }
        }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static bool IsSuperManager
        {
            get
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return false;
                }

                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
                if (claimsIdentity == null)
                {
                    return false;
                }

                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
                if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                {
                    return false;
                }

                bool isSuperManager;
                if (!bool.TryParse(userIdClaim.Value, out isSuperManager))
                {
                    return false;
                }

                return isSuperManager;
            }
        }
    }
}
