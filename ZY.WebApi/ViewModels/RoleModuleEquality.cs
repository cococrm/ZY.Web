using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Model;

namespace ZY.WebApi.ViewModels
{
    public class RoleModuleEquality : IEqualityComparer<RoleModuleMap>
    {

        public bool Equals(RoleModuleMap one, RoleModuleMap two)
        {
            return (one.ModuleId == two.ModuleId && one.OperationId == two.OperationId && one.RoleId == two.RoleId);
        }

        public int GetHashCode(RoleModuleMap obj)
        {
            if (obj == null)
                return 0;
            return obj.ToString().GetHashCode();
        }
    }
}
