using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.WebApi.ViewModels
{
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
