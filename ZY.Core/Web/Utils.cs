using System;
using System.Web;
using System.Configuration;

namespace ZY.Core.Web
{
    public class Utils
    {
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestValues(HttpRequest request)
        {
            if (request.HttpMethod.ToUpper() == "POST")
            {
                return request.Form.ToString();
            }
            return request.QueryString.ToString();
        }

        /// <summary>
        /// 获取webconfig值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                throw new ArgumentNullException(string.Format("config key:{0} is null", key));
            return value;
        }
    }
}
