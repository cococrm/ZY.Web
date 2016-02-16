
using System.Collections.Generic;

namespace ZY.Core.Page
{
    /// <summary>
    /// EasyUi Datagrid 数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridData<T>
    {
        public GridData()
            :this(new List<T>(), 0)
        { }

        public GridData(IEnumerable<T> rows,int total)
        {
            this.Rows = rows;
            this.Total = total;
        }

        public int Total { get; set; }

        public IEnumerable<T> Rows { get; set; }
    }
}
