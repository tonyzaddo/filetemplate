using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace filetemplate
{
    public class LogHelper
    {
//        public static log4net.ILog GetLogger([CallerFilePath]string filename = "")
        public static log4net.ILog GetLogger(string filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }
    }
}
