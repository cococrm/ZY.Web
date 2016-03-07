using System;
using ZY.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ZY.Model
{
    /// <summary>
    /// 管理员操作日志
    /// </summary>
    public class UserLog : EntityBase<int>, ICreatedTime
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        [StringLength(50)]
        public string Type { get; set; }
        /// <summary>
        /// 操作账号
        /// </summary>
        [StringLength(100)]
        public string UserName { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        [StringLength(100)]
        public string Ip { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public UserLog()
        {
            this.CreateTime = DateTime.Now;
        }
    }
}
