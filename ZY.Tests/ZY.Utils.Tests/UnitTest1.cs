using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZY.Utils.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Log4NetInit.Init(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            ILog log = new Log();
            log.Debug("test");
        }

        
    }
}
