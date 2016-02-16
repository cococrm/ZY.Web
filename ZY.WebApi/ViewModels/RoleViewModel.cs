using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.WebApi.ViewModels
{

    public class SaveRoleViewModel
    {
        public List<RoleViewModel> AddList { get; set; }

        public List<RoleViewModel> UpdateList { get; set; }

        public List<RoleViewModel> DeleteList { get; set; }

        public SaveRoleViewModel()
        {
            AddList = new List<RoleViewModel>();
            UpdateList = new List<RoleViewModel>();
            DeleteList = new List<RoleViewModel>();
        }
    }

    public class RoleViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        [Required, StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 角色说明
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
    }

    public class SetRoleModule
    {
        public int RoleId { get; set; }

        public List<Module> Modules { get; set; }

    }
}
