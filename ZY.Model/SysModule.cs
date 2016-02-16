using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 系统模块
    /// </summary>
    public class SysModule : EntityBase<Guid>, ICreatedTime
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 模块代码
        /// </summary>
        [Required, StringLength(100)]
        public string Code { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }
        /// <summary>
        /// Url链接
        /// </summary>
        [StringLength(255)]
        public string Url { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(255)]
        public string Icon { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        [ForeignKey("ModuleId")]
        public virtual ICollection<UserModuleMap> UserModules { get; set; }

        [ForeignKey("ModuleId")]
        public virtual ICollection<RoleModuleMap> RoleModules { get; set; }

        [ForeignKey("ModuleId")]
        public virtual ICollection<ModuleOperationMap> Operations { get; set; }

        public SysModule()
        {
            this.CreateTime = DateTime.Now;
        }        
    }
}
