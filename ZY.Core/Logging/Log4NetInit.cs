
using log4net.Config;
using System.IO;

namespace ZY.Core.Logging
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
