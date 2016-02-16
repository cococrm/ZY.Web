using System;
using ZY.Core.Entities;

namespace ZY.Model
{
    /// <summary>
    /// 模块角色对应关系
    /// </summary>
    public class RoleModuleMap : EntityBase<int>
    {
        public virtual int RoleId { get; set; }

        public virtual Guid ModuleId { get; set; }

        public virtual int OperationId { get; set; }
    }
}
