using System;

namespace ZY.Core.Logging
{
    public interface ILog
    {
        /// <summary>
        /// 一般信息
        /// </summary>
        /// <param name="msg"></param>
        void Info(string msg);
        /// <summary>
        /// Debug信息
        /// </summary>
        /// <param name="msg"></param>
        void Debug(string msg);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="msg"></param>
        void Error(string msg);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="exception"></param>
        void Error(Exception exception);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        void Error(string msg, Exception exception);
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="msg"></param>
        void Warn(string msg);
    }
}
