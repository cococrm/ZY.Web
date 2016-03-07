using System;

namespace ZY.WebApi.ViewModels
{
    public class OperationDeleteViewModel
    {
        public int[] Ids { get; set; }
    }

    public class SaveModuleOperationViewModel
    {
        public Guid Id { get; set; }

        public int[] Ids { get; set; }

        public SaveModuleOperationViewModel()
        {
            this.Ids = new int[0];
        }
    }
}
