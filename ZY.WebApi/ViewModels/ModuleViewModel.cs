using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.WebApi.ViewModels
{
    /// <summary>
    /// 保存角色权限
    /// </summary>
    public class SaveRoleModuleOperationViewModel
    {
        public int Id { get; set; }

        public IList<ModuleOperationViewModel> Module { get; set; }

        public SaveRoleModuleOperationViewModel()
        {
            this.Module = new List<ModuleOperationViewModel>();
        }

    }
    /// <summary>
    /// 保存账号权限
    /// </summary>
    public class SaveUserModuleOperationViewModel
    {
        public int Id { get; set; }

        public IList<ModuleOperationViewModel> Module { get; set; }

        public SaveUserModuleOperationViewModel()
        {
            this.Module = new List<ModuleOperationViewModel>();
        }

    }

    public class ModuleOperationViewModel
    {
        public Guid Id { get; set; }

        public int operation { get; set; }
    }
}
