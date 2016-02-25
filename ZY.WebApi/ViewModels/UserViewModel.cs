using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.WebApi.ViewModels
{

    public class SaveUserViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [Required, StringLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(100)]
        public string NickName { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required, StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string Roles { get; set; }
    }

    public class DeleteViewModel
    {
        public int[] Ids { get; set; }
    }

    public class SetUserRole
    {
        public int UserId { get; set; }

        public int[] RoleIds { get; set; }
    }

    public class SetUserModule
    {
        public int UserId { get; set; }

        public List<Module> Modules { get; set; }

    }

    public class Module
    {
        public Guid Id { get; set; }

        public List<Operation> Operations { get; set; }
    }

    public class Operation
    {
        public int Id { get; set; }
    }
}
