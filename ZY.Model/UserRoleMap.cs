
using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 账号，角色对应关系
    /// </summary>
    public class UserRoleMap : EntityBase<int>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}
