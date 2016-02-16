using ZY.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ZY.Model
{
    /// <summary>
    /// 模块权限用户对应关系
    /// </summary>
    public class UserModuleMap : EntityBase<int>
    {
        
        public virtual int UserId { get; set; }
        
        public virtual Guid ModuleId { get; set; }

        public virtual int OperationId { get; set; }


    }
}
