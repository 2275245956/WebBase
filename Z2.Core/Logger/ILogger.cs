using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Logger
{
    public interface ILogger
    {
        void Debug(LogContent message);

        void Error(LogContent message);

        void Info(LogContent message);

        void Warn(LogContent message);
    }

    public class LogContent
    {
        public LogContent()
        {
            this.Type = LogType.Info.ToString();
        }
        public LogContent(LogType type)
        {
            this.Type = type.ToString();
        }
        public string Account { get; set; }
        public string IPAddress { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Type { get; private set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string TargetSite { get; set; }
        public string Description { get; set; }

        public void SetLogType(LogType type)
        {
            this.Type = type.ToString();
        }
    }

    public enum LogType
    {
        Debug = 0,
        Error,
        Info,
        Warn
    }
}
