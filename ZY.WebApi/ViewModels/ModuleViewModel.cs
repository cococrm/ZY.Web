using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.WebApi.ViewModels
{
    public class SetModuleOperationViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonProperty(PropertyName = "_parentId", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        public IList<OperationViewModel> Operations { get; set; }

        public string selectAll { get; set; }
    }

    public class OperationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCheck { get; set; }

    }

    public class SaveModuleOperationViewModel
    {
        public int Id { get; set; }

        public IList<ModuleOperationViewModel> Module { get; set; }

        public SaveModuleOperationViewModel()
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
