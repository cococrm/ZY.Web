using System;
using System.Data;
using System.Threading.Tasks;

namespace ZY.Core.Repositories
{
    /// <summary>
    /// 业务单元操作接口
    /// </summary>
    public interface IUnitOfWork: IDependency, IDisposable
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        void Commit();
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);
        /// <summary>
        /// 异步执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <returns></returns>
        DataSet QueryProcedure(string storedProcName);
        /// <summary>
        /// 执行存储过程 带参数
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        DataSet QueryProcedure(string storedProcName, IDataParameter[] parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>存储过程返回值</returns>
        int RunProcedure(string storedProcName, IDataParameter[] parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int RunProcedure(string storedProcName);
    }
}
