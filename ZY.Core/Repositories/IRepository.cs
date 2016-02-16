using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ZY.Core.Entities;

namespace ZY.Core.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, TKey> : IDependency where TEntity : IEntity<TKey>
    {
        IQueryable<TEntity> Entities { get; }

        #region 同步添加方法
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);
        /// <summary>
        /// 批量添加实体对象
        /// </summary>
        /// <param name="entitys"></param>
        void Insert(IEnumerable<TEntity> entitys);
        #endregion

        #region 同步删除方法
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="key"></param>
        void Remove(TKey key);
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate"></param>
        void Remove(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 批量删除对象
        /// </summary>
        /// <param name="entitys"></param>
        void Remove(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 根据主键批量删除
        /// </summary>
        /// <param name="keys"></param>
        void Remove(IEnumerable<TKey> keys);
        #endregion

        #region 同步修改方法
        /// <summary>
        /// 修改对象实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        #endregion

        #region 同步判断方法
        /// <summary>
        /// 根据主键判断实体是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(TKey key);
        /// <summary>
        /// 根据添加判断是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 同步查询单条
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity GetByKey(TKey key);
        /// <summary>
        /// 根据条件获取单条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FirstOfDefault(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 同步查询列表
        /// <summary>
        /// 根据条件获取数据列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 异步添加方法
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="entity"></param>
        Task<TEntity> InsertAsync(TEntity entity);
        /// <summary>
        /// 批量添加实体对象
        /// </summary>
        /// <param name="entitys"></param>
        Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entitys);
        #endregion

        #region 异步修改方法
        /// <summary>
        /// 修改对象实体
        /// </summary>
        /// <param name="entity"></param>
        Task<TEntity> UpdateAsync(TEntity entity);
        #endregion

        #region 异步判断方法
        /// <summary>
        /// 根据主键判断实体是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(TKey key);
        /// <summary>
        /// 根据添加判断是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 异步查询单条
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TEntity> GetByKeyAsync(TKey key);
        /// <summary>
        /// 根据条件获取单条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOfDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 异步查询列表
        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync();
        /// <summary>
        /// 根据条件获取数据列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate);
        
        #endregion

        #region 同步sql查询
        /// <summary>
        /// 根据sql查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> SqlQuery(string sql, params object[] parameters);
        #endregion
    }
}
