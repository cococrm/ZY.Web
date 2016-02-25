using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Core.Page;
using ZY.Core.Sort;

namespace ZY.Core.Extensions
{
    public static class PagerRequestExtensions
    {
        public static Pager ToPager(this PagerRequest request)
        {
            Pager pager = new Pager();
            pager.PageNumber = request.PageIndex;
            pager.PageSize = request.PageSize;
            //组合排序条件
            if (!request.sortField.IsNullOrEmpty() && !request.sortOrder.IsNullOrEmpty())
            {
                string[] field = request.sortField.Split(",", true);
                string[] order = request.sortOrder.Split(",", true);
                if (field.Length != order.Length)
                {
                    throw new ArgumentException("查询列表的排序参数个数不一致。");
                }
                List<SortCondition> sortConditions = new List<SortCondition>();
                for (int i = 0; i < field.Length; i++)
                {
                    ListSortDirection direction = order[i].ToLower() == "desc"
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;
                    sortConditions.Add(new SortCondition(field[i], direction));
                }
                pager.SortConditions = sortConditions.ToArray();
            }
            else
            {
                pager.SortConditions = new SortCondition[] { };
            }
            return pager;
        }
        /// <summary>
        /// 转换为EasyUi datagrid格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static GridData<T> ToGridData<T>(this PagedResult<T> result)
        {
            return new GridData<T>(result.Data, result.TotalRecords);
        }
    }
}
