using System;

namespace ZY.Core.Logging
{
    /// <summary>
    /// log4net日志记录
    /// </summary>
    public class Log : ILog
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 一般信息
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            log.Info("[Info] ： " + msg);
        }
        /// <summary>
        /// Debug信息
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg)
        {
            log.Debug("[Debug] ： " + msg);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg)
        {
            log.Error("[Error] ： " + msg);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            log.Error("[Error] ： " + ex.Message, ex);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Error(string msg, Exception ex)
        {
            log.Error("[Error] ：" + msg, ex);
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="msg"></param>
        public void Warn(string msg)
        {
            log.Warn("[Warn] ： " + msg);
        }
    }
}
