using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ZY.Core.Page;
using ZY.Core.Sort;

namespace ZY.Core.Extensions
{
    /// <summary>
    /// Queryable扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        private static readonly ConcurrentDictionary<string, LambdaExpression> Cache = new ConcurrentDictionary<string, LambdaExpression>();

        /// <summary>
        /// 根据条件判断是否执行之句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        #region IQueryable扩展方法，分页
        /// <summary>
        /// IQueryable扩展方法，分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPage<T>(this IQueryable<T> query)
        {
            return query.ToPage(new Pager());
        }

        /// <summary>
        /// IQueryable扩展方法，分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public static PagedResult<T> ToPage<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return query.ToPage(predicate, new Pager());
        }

        /// <summary>
        /// IQueryable扩展方法，分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPage<T>(this IQueryable<T> query, Pager pager)
        {
            return query.ToPage(null, pager);
        }

        /// <summary>
        /// IQueryable扩展方法，分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">查询条件</param>
        /// <param name="pager">分页对象</param>
        /// <returns></returns>
        public static PagedResult<T> ToPage<T>(this IQueryable<T> query, 
            Expression<Func<T, bool>> predicate, 
            Pager pager)
        {
            if (predicate == null)
                predicate = (o => true);
            query = query.Where(predicate); //条件查询
            var total = query.Count(); //条件查询总记录数
            if (pager.SortConditions == null || pager.SortConditions.Length == 0)
            {
                query = query.OrderBy("Id", ListSortDirection.Ascending); //默认按照Id排序
            }
            else
            {
                int count = 0;
                IOrderedQueryable<T> orderSource=null;
                foreach (SortCondition sort in pager.SortConditions) //多组合条件排序
                {
                    orderSource = count == 0
                        ? query.OrderBy(sort.SortFiled, sort.ListSortDirection)
                        : orderSource.ThenBy(sort.SortFiled, sort.ListSortDirection);
                    count++;
                }
                query = orderSource;
            }
            query = query != null
                ? query.Skip(pager.Skip).Take(pager.PageSize)
                : Enumerable.Empty<T>().AsQueryable();

            return new PagedResult<T>(pager.PageSize) { TotalRecords = total, Data = query.ToList() };
        }
        #endregion

        #region IQueryable扩展方法，排序
        /// <summary>
        /// 按指定的属性名称对<see cref="IQueryable{T}"/>序列进行排序
        /// </summary>
        /// <param name="source">IQueryable{T}序列</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            dynamic keySelector = GetKeySelector<T>(propertyName);
            return sortDirection == ListSortDirection.Ascending
                ? Queryable.OrderBy(source, keySelector)
                : Queryable.OrderByDescending(source, keySelector);
        }

        /// <summary>
        /// 按指定的属性名称对<see cref="IOrderedQueryable{T}"/>序列进行排序
        /// </summary>
        /// <param name="source">IOrderedQueryable{T}序列</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            dynamic keySelector = GetKeySelector<T>(propertyName);
            return sortDirection == ListSortDirection.Ascending
                ? Queryable.ThenBy(source, keySelector)
                : Queryable.ThenByDescending(source, keySelector);
        }

        private static LambdaExpression GetKeySelector<T>(string keyName)
        {
            Type type = typeof(T);
            string key = type.FullName + "." + keyName;
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            ParameterExpression param = Expression.Parameter(type);
            string[] propertyNames = keyName.Split('.');
            Expression propertyAccess = param;
            foreach (string propertyName in propertyNames)
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentNullException();
                }
                type = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
            LambdaExpression keySelector = Expression.Lambda(propertyAccess, param);
            Cache[key] = keySelector;
            return keySelector;
        }

        #endregion
    }
}
