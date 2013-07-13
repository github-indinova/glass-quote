using System;
using System.Collections.Generic;
using System.Text;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository()]

namespace glass_quote
{
    class LogHelper
    {
        #region Data Members
        public static readonly ILog testLogger = LogManager.GetLogger("TestApplication");
        #endregion

        public LogHelper()
        {
        }
    }
}
