using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZY.Utils
{
    public class JsonHelper
    {
        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T ToObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(object target, bool isConvertSingleQuotes = false)
        {
            if (target == null)
                return "{}";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var result = JsonConvert.SerializeObject(target, Newtonsoft.Json.Formatting.Indented, timeFormat);
            if (isConvertSingleQuotes)
                result = result.Replace("\"", "'");
            return result;
        }
    }
}
