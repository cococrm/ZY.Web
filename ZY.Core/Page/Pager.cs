using ZY.Core.Sort;

namespace ZY.Core.Page
{
    /// <summary>
    /// 分页传入参数
    /// </summary>
    public class Pager
    {
        public Pager()
            : this(1)
        { }

        /// <summary>
        /// 初始化分页
        /// </summary>
        /// <param name="pageNumber">页索引，即第几页，从1开始</param> 
        public Pager(int pageNumber)
            : this(pageNumber, 20)
        {

        }

        /// <summary>
        /// 初始化分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数,默认20</param> 
        public Pager(int pageNumber, int pageSize)
            : this(pageNumber, pageSize, new SortCondition[] { })
        { }

        /// <summary>
        /// 初始化分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数,默认20</param> 
        /// <param name="sortConditions">排序条件</param>
        public Pager(int pageNumber, int pageSize, SortCondition[] sortConditions)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.SortConditions = sortConditions;
        }

        /// <summary>
        /// 获取或设置页面大小
        /// </summary>
        public int PageSize { get; set; }
        private int _pageIndex;
        /// <summary>
        /// 页索引，即第几页，从1开始
        /// </summary>
        public int PageNumber
        {
            get
            {
                if (_pageIndex <= 0)
                    _pageIndex = 1;
                return _pageIndex;
            }
            set { _pageIndex = value; }
        }
        /// <summary>
        /// 查询起始
        /// </summary>
        public int Skip
        {
            get
            {
                return PageSize * (PageNumber - 1);
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public SortCondition[] SortConditions { get; set; }
    }
}
