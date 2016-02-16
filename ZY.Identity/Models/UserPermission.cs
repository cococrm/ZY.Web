
namespace ZY.Identity
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserPermission
    {
        /// <summary>
        /// 账号Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账号名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 模块代码
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 操作代码
        /// </summary>
        public string Operation { get; set; }
    }
}
