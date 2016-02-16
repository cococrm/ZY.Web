using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Core.Page
{
    /// <summary>
    /// 分页请求数据
    /// </summary>
    public class PagerRequest
    {
        /// <summary>
        /// 页大小
        /// </summary>        
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sortField { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string sortOrder { get; set; }
    }
}
