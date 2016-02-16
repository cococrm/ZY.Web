using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using ZY.Core.Repositories;
using ZY.Core.Entities;
using System.Linq.Expressions;

namespace ZY.Repositories.EntityFramework
{
    /// <summary>
    /// EntityFramework 仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected DbContext _dbContext;
        protected IDbSet<TEntity> _dbSet;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        /// <summary>
        /// 全部实体对象
        /// </summary>
        public IQueryable<TEntity> Entities
        {
            get
            {
                return _dbSet.AsQueryable();
            }
        }

        #region 同步添加方法
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        /// <summary>
        /// 批量添加实体对象
        /// </summary>
        /// <param name="entitys"></param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
        }
        #endregion

        #region 同步删除方法
        /// <summary>
        /// 删除对象实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(TKey key)
        {
            var entity = GetByKey(key);
            if (entity == null)
                return;
            Remove(entity);
        }
        /// <summary>
        /// 删除对象列表
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Remove(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                return;
            if (!entities.Any())
                return;
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate"></param>
        public virtual void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _dbSet.Where(predicate);
            Remove(entities);
        }
        /// <summary>
        /// 根据主键列表删除
        /// </summary>
        /// <param name="keys"></param>
        public virtual void Remove(IEnumerable<TKey> keys)
        {
            if (keys == null)
                return;
            Remove(Query(t => keys.Contains(t.Id)));
        }
        #endregion

        #region 同步修改方法
        /// <summary>
        /// 修改对象实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region 同步判断方法
        /// <summary>
        /// 判断主键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Exists(TKey key)
        {
            return GetByKey(key) == null ? false : true;
        }
        /// <summary>
        /// 根据条件判断对象是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Query(predicate).Any();
        }
        #endregion

        #region 同步查询单条
        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TEntity GetByKey(TKey key)
        {
            return _dbContext.Set<TEntity>().Find(key);
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity FirstOfDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Query(predicate).FirstOrDefault();
        }
        #endregion

        #region 同步查询列表
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        #endregion

        #region 异步添加方法
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(_dbSet.Add(entity));
        }
        /// <summary>
        /// 批量添加实体对象
        /// </summary>
        /// <param name="entitys"></param>
        public virtual Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(_dbContext.Set<TEntity>().AddRange(entities));
        }
        #endregion

        #region 同步修改方法
        /// <summary>
        /// 修改对象实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }
        #endregion

        #region 异步判断方法
        /// <summary>
        /// 判断主键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(TKey key)
        {
            return await GetByKeyAsync(key) == null ? false : true;
        }
        /// <summary>
        /// 根据条件判断对象是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query(predicate).AnyAsync();
        }
        #endregion

        #region 异步查询单条
        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByKeyAsync(TKey key)
        {
            return await _dbContext.Set<TEntity>().FindAsync(key);
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstOfDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query(predicate).FirstOrDefaultAsync();
        }
        #endregion

        #region 异步查询列表
        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        #endregion

        #region 同步sql方法
        /// <summary>
        /// 根据sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> SqlQuery(string sql, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<TEntity>(sql, parameters);
        }
        #endregion
    }
}
