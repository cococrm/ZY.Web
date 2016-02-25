using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
    }
}
