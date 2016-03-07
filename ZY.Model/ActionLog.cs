using System;
using System.ComponentModel.DataAnnotations;
using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 运行日志
    /// </summary>
    public class ActionLog : EntityBase<int>, ICreatedTime
    {
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        [StringLength(100)]
        public string Ip { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 执行地址
        /// </summary>
        [StringLength(255)]
        public string ActionUrl { get; set; }
        /// <summary>
        /// 执行时长
        /// </summary>
        public int ActionTime { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
