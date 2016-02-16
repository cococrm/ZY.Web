using System;
using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 系统模块，操作对应
    /// </summary>
    public class ModuleOperationMap : EntityBase<int>
    {
        public virtual int OperationId { get; set; }

        public virtual Guid ModuleId { get; set; }
    }
}
