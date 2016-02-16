
using log4net.Config;
using System.IO;

namespace ZY.Utils
{
    public class Log4NetInit
    {
        public static void Init(string filePath)
        {
            var logCgf = new FileInfo(filePath);
            XmlConfigurator.ConfigureAndWatch(logCgf);
        }
    }
}
