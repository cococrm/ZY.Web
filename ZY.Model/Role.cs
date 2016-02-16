using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class Role : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        [Required,StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 角色说明
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<UserRoleMap> Users { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<RoleModuleMap> Modules { get; set; }

    }
}
