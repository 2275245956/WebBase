using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Logger
{
    public class Log: ILogger
    {
        private ILog logger;
        public Log(ILog log)
        {
            this.logger = log;
        }
        public void Debug(LogContent message)
        {
            this.logger.Debug(message);
        }
        public void Error(LogContent message)
        {
            this.logger.Error(message);
        }
        public void Info(LogContent message)
        {
            this.logger.Info(message);
        }
        public void Warn(LogContent message)
        {
            this.logger.Warn(message);
        }
    }

}
