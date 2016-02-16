using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Model;

namespace ZY.Identity
{
    /// <summary>
    /// 用户权限缓存
    /// </summary>
    [Serializable]
    public class UserPermissionCacheItem
    {
        //缓存Key
        public const string CacheStoreName = "ZYUserPermissions";
        //过期时间
        public TimeSpan CacheExpireTime { get; private set; }
        //账号Id
        public int UserId { get; set; }
        //账号所属权限
        public List<UserPermission> UserPermissions { get; set; }
        
        public UserPermissionCacheItem()
        {
            CacheExpireTime = TimeSpan.FromMinutes(20);
            UserPermissions = new List<UserPermission>();
        }

        public UserPermissionCacheItem(int userId)
            :this()
        {
            this.UserId = userId;
        }

        
    }
}
