using System;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using ZY.Core.Page;
using ZY.Utils;
using ZY.Core.Sort;
using ZY.Core.Extensions;
using ZY.WebApi.Filter;
using ZY.Core.Web.Model;

namespace ZY.WebApi.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [UserPermission]
    public class ApiControllerBase : ApiController
    {
        /// <summary>
        /// 重新返回json格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="serializerSettings"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        protected override JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding)
        {
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            });
            return base.Json<T>(content, serializerSettings, encoding);
        }

        protected new IHttpActionResult Ok()
        {
            return Json(new AjaxResponse());
        }

        protected new IHttpActionResult NotFound()
        {
            return Json(new AjaxResponse(AjaxResponseStatus.NotFound));
        }

        protected IHttpActionResult ValidError()
        {
            return Json(new AjaxResponse(AjaxResponseStatus.ValidError, "数据验证错误", ModelState.FirstOrDefault()));
        }

        protected IHttpActionResult Error(int status = 0, string message = "", object data = null)
        {
            return Json(new AjaxResponse(status, message, data));
        }

        #region 基类分页查询
        /// <summary>
        /// 分页查询结果
        /// </summary>
        /// <typeparam name="T">查询对象</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="request">分页请求参数</param>
        /// <returns></returns>
        protected virtual PagedResult<T> GetPageResult<T>(IQueryable<T> source)
        {
            return GetPageResult<T>(source, null);
        }
        /// <summary>
        /// 分页查询结果
        /// </summary>
        /// <typeparam name="T">查询对象</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="request">分页请求参数</param>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        protected virtual PagedResult<T> GetPageResult<T>(IQueryable<T> source,
            Expression<Func<T, bool>> predicate)
        {
            Pager pager = GetPager();
            //查询分页
            return source.ToPage<T>(predicate, pager);
        }
        /// <summary>
        /// 分页查询结果
        /// </summary>
        /// <typeparam name="T">查询对象</typeparam>
        /// <typeparam name="TResult">查询返回对象</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="request">分页请求参数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询返回对象字段</param>
        /// <returns></returns>
        protected virtual PagedResult<TResult> GetPageResult<T, TResult>(IQueryable<T> source,
            Expression<Func<T, TResult>> selector)
        {
            return GetPageResult<T, TResult>(source, null, selector);
        }
        /// <summary>
        /// 分页查询结果
        /// </summary>
        /// <typeparam name="T">查询对象</typeparam>
        /// <typeparam name="TResult">查询返回对象</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="request">分页请求参数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询返回对象字段</param>
        /// <returns></returns>
        protected virtual PagedResult<TResult> GetPageResult<T, TResult>(IQueryable<T> source,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector)
        {
            Pager pager = GetPager();
            //查询分页
            PagedResult<T> result = source.ToPage<T>(predicate, pager);
            var datas = result.Data.AsQueryable().Select(selector);
            return new PagedResult<TResult>(pager.PageSize) { Data = datas.ToList(), TotalRecords = result.TotalRecords };
        }
        //获取分页参数对象
        private Pager GetPager()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Pager pager = new Pager();
            pager.PageNumber = request.Params["page"].ToInt(1);
            pager.PageSize = request.Params["rows"].ToInt(20);
            string sortField = request.Params["sortField"];
            string sortOrder = request.Params["sortOrder"];
            //组合排序条件
            if (!sortField.IsNullOrEmpty() && !sortOrder.IsNullOrEmpty())
            {
                string[] field = sortField.Split(",", true);
                string[] order = sortOrder.Split(",", true);
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
        #endregion



    }
}
