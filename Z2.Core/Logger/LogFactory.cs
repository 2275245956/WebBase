using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Z2.Core.Logger
{
    public class LogFactory
    {
        static LogFactory()
        {
            FileInfo configFile = new FileInfo(HttpContext.Current.Server.MapPath("/Configs/log4net.config"));
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        public static ILogger GetLogger(Type type)
        {
            return new DBLogger();// new Log(LogManager.GetLogger(type));
        }
        public static ILogger GetLogger(string str)
        {
            return new DBLogger();//new Log(LogManager.GetLogger(str));
        }
        public static ILogger GetLogger()
        {
            return new DBLogger();
        }
    }

}
