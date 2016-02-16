using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace ZY.Core.Sort
{
    /// <summary>
    /// 排序条件
    /// </summary>
    public class SortCondition
    {
        /// <summary>
        /// 构造一个默认为按Id字段升序排序的排序条件
        /// </summary>
        public SortCondition()
            : this("Id")
        { }

        /// <summary>
        /// 构造一个指定字段升序排序的排序条件
        /// </summary>
        /// <param name="sortFiled"></param>
        public SortCondition(string sortFiled)
            : this(sortFiled, ListSortDirection.Ascending)
        { }

        /// <summary>
        /// 构造一个指定字段指定排序的排序条件
        /// </summary>
        /// <param name="sortFiled">排序字段</param>
        /// <param name="listSortDirection">排序方向</param>
        public SortCondition(string sortFiled, ListSortDirection listSortDirection)
        {
            this.SortFiled = sortFiled;
            this.ListSortDirection = listSortDirection;
        }
        /// <summary>
        /// 获取设置排序字段名称
        /// </summary>
        public string SortFiled { get; set; }

        /// <summary>
        /// 获取设置排序字段方向（升序，降序）
        /// </summary>
        public ListSortDirection ListSortDirection { get; set; }
    }

    /// <summary>
    /// 泛型排序字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortCondition<T> : SortCondition
    {
        /// <summary>
        /// 排序字段初始化
        /// </summary>
        /// <param name="sortFiled"></param>
        public SortCondition(Expression<Func<T, object>> sortFiled)
            : this(sortFiled, ListSortDirection.Ascending)
        {

        }

        /// <summary>
        /// 排序字段初始化
        /// </summary>
        /// <param name="sortFiled"></param>
        /// <param name="listSortDirecation"></param>
        public SortCondition(Expression<Func<T, object>> sortFiled, ListSortDirection listSortDirecation)
            : base(GetPropertyName(sortFiled), listSortDirecation)
        {

        }

        /// <summary>
        /// 从泛型委托获取属性名
        /// </summary>
        private static string GetPropertyName(Expression<Func<T, object>> keySelector)
        {
            string param = keySelector.Parameters.First().Name;
            string operand = (((dynamic)keySelector.Body).Operand).ToString();
            operand = operand.Substring(param.Length + 1, operand.Length - param.Length - 1);
            return operand;
        }

    }

}
