using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

using ZY.Core.Repositories;
using ZY.Utils;

namespace ZY.Repositories.EntityFramework
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext; //数据库上下文对象
        private readonly ILog log; //日志接口

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            log = new Log();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public void Commit()
        {
            try
            {
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbEntityValidationException exception)
                {
                    throw new DataException("保存数据时,数据验证引发异常--", exception);
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DataException("保存数据更改时引发异常--", ex);
            }
        }
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            try
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbEntityValidationException exception)
                {
                    throw new DataException("保存数据时,数据验证引发异常--", exception);
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DataException("保存数据更改时引发异常--",ex);
            }
        }
        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlCommandAsync(sql, parameters);
        }
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataSet QueryProcedure(string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection connection = (SqlConnection)_dbContext.Database.Connection)
            {
                try
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();
                    SqlDataAdapter sqlDA = new SqlDataAdapter();
                    sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                    sqlDA.Fill(dataSet);
                    connection.Close();
                    return dataSet;
                }
                catch (Exception exception)
                {
                    string error = string.Format("执行存储过程 名称：{0} 参数 {1}", storedProcName, GetParamterValue(parameters));
                    log.Error(error, exception);
                    throw new Exception(error, exception);
                }
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <returns></returns>
        public DataSet QueryProcedure(string storedProcName)
        {
            using (SqlConnection connection = (SqlConnection)_dbContext.Database.Connection)
            {
                try
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();
                    SqlDataAdapter sqlDA = new SqlDataAdapter();
                    SqlCommand command = new SqlCommand(storedProcName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = command;
                    sqlDA.Fill(dataSet);
                    connection.Close();
                    return dataSet;
                }
                catch (Exception exception)
                {
                    string error = string.Format("执行存储过程 名称：{0}", storedProcName);
                    log.Error(error, exception);
                    throw new Exception(error, exception);
                }
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>存储过程返回值</returns>
        public int RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection connection = (SqlConnection)_dbContext.Database.Connection)
            {
                try
                {
                    connection.Open();
                    SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                    SqlParameter resultParam = new SqlParameter(); //构建存储过程返回值
                    resultParam.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(resultParam);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return (int)resultParam.Value;
                }
                catch (Exception exception)
                {
                    string error = string.Format("执行存储过程 名称：{0} 参数 {1}", storedProcName, GetParamterValue(parameters));
                    log.Error(error, exception);
                    throw new Exception(error, exception);
                }
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程</param>
        /// <returns>返回值</returns>
        public int RunProcedure(string storedProcName)
        {
            using (SqlConnection connection = (SqlConnection)_dbContext.Database.Connection)
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter resultParam = new SqlParameter(); //构建存储过程返回值
                    resultParam.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(resultParam);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return (int)resultParam.Value;
                }
                catch (SqlException exception)
                {
                    string error = string.Format("执行存储过程 名称：{0}", storedProcName);
                    log.Error(error, exception);
                    throw new Exception(error, exception);
                }
            }
        }
        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
        /// <summary>
        /// 根据参数列表获取参数的名称和值，用于记录日志
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetParamterValue(IDataParameter[] parameters)
        {
            StringBuilder paramStr = new StringBuilder();
            foreach (SqlParameter param in parameters)
            {
                paramStr.AppendFormat("{0} ：{1}", param.ParameterName, param.Value);
            }
            return paramStr.ToString();
        }
        //处理回收机制
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //处理回收机制
        private void Dispose(bool isdispose)
        {
            if (isdispose)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
    }
}
