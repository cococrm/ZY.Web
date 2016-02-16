using System.Collections;
using System.Collections.Generic;

namespace ZY.Core.Page
{
    /// <summary>
    /// 分页返回结果对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        #region 构造函数
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="totalPages">总页数</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="data">页面数据</param>
        public PagedResult(int pageSize)
        {
            this.pageSize = pageSize;
        }
        #endregion

        #region 私有属性

        /// <summary>
        /// 获取或设置页面大小
        /// </summary>
        private int pageSize;

        #endregion

        #region 公共属性
        /// <summary>
        /// 获取总记录数
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// 获取或者设置总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (TotalRecords == 0)
                    return 0;
                if ((TotalRecords % pageSize) == 0)
                    return TotalRecords / pageSize;
                return (TotalRecords / pageSize) + 1;
            }

        }
        /// <summary>
        /// 获取当前页面数据
        /// </summary>
        public List<T> Data { get; set; }
        #endregion
    }
}
