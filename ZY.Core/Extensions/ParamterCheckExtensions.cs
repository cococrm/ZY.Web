using System;

namespace ZY.Core.Extensions
{
    public static class ParamterCheckExtensions
    {
        /// <summary>
        /// 检查参数不能为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static void CheckNotNull<T>(this T value, string message = "") where T : class
        {
            if (message.IsNullOrEmpty())
                message = string.Format("{0} Is Not Null", typeof(T).Name);
            Require<ArgumentNullException>(value != null, message);
        }

        /// <summary>
        /// 验证指定的断言是否为真，如果不为真，抛出异常
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="assertion"></param>
        /// <param name="message"></param>
        private static void Require<TException>(bool assertion, string message = "") where TException : Exception
        {
            if (assertion)
                return;
            TException exception = (TException)Activator.CreateInstance(typeof(Exception), message);
            throw exception;
        }
    }
}
