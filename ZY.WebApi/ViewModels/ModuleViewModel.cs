using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.WebApi.ViewModels
{
    public class SetOperationViewModel
    {
        public Guid Id { get; set; }

        public int[] OperationIds { get; set; }
    }
}
