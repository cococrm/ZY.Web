using System;
using System.ComponentModel.DataAnnotations;

using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 系统操作类型
    /// </summary>
    public class Operation : EntityBase<int>
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        [Required,StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 按钮代码
        /// </summary>
        [Required,StringLength(100)]
        public string Code { get; set; }
        /// <summary>
        /// 按钮图标
        /// </summary>
        [StringLength(255)]
        public string Icon { get; set; }
        /// <summary>
        /// 按钮备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
    }
}
