
using ZY.Core.Entities;

namespace ZY.Core.Tests
{
    public class User : EntityBase<int>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}
