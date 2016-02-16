using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : EntityBase<int>, IUser<int>,ICreatedTime
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required, StringLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(100)]
        public string NickName { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required, StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 安全标识，当用户认证过期（修改密码、退出等）时将变更的随机值
        /// </summary>
        [StringLength(200)]
        public string SecurityStmp { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 管理员状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 获取或设置 登录锁定UTC时间，在此时间前登录将被锁定
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set;}

        /// <summary>
        /// 获取或设置 是否允许锁定用户
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 获取或设置 当前登录失败次数，达到设定值将被锁定
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuperManager { get; set; }

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }   
        
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }    

        /// <summary>
        /// 登陆次数
        /// </summary>
        public int LoginCount { get; set; }
        
        [ForeignKey("UserId")]
        public virtual ICollection<UserRoleMap> Roles { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserModuleMap> Modules { get; set; }

        public User()
        {
            this.CreateTime = DateTime.Now;
            this.IsSuperManager = false;
            this.LoginCount = 0;
        }

    }
}
