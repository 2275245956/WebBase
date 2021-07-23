using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Z2.Core.Logger;
using Z2.Core.Web;

namespace Z2.WebAPI.Controllers
{
    public class _ApiBaseController : ApiController
    {
        public ILogger LogWriter => LogFactory.GetLogger();
        public virtual string UserId { get; protected set; }
        public virtual string IPAddress { get; protected set; }


        public virtual void WriteInfo(string ModuleId,string ModuleName,string Result, string Message, object Description)
        {
            LogWriter.Info(new LogContent()
            {
                Account = UserId,
                IPAddress = IPAddress,
                ModuleId = ModuleId,
                ModuleName = ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
            });
        }

        public virtual void WriteDebug(string ModuleId, string ModuleName, string Result, string Message, object Description)
        {
            LogWriter.Debug(new LogContent()
            {
                Account = UserId,
                IPAddress = IPAddress,
                ModuleId = ModuleId,
                ModuleName = ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
            });
        }

        public virtual void WriteWarn(string ModuleId, string ModuleName, string Result, string Message, object Description)
        {
            LogWriter.Warn(new LogContent()
            {
                Account = UserId,
                IPAddress = IPAddress,
                ModuleId = ModuleId,
                ModuleName = ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
            });
        }


        public virtual void WriteError(string ModuleId, string ModuleName, string Result, string Message, object Description, Exception ex)
        {
            LogWriter.Error(new LogContent()
            {
                Account = UserId,
                IPAddress = IPAddress,
                ModuleId = ModuleId,
                ModuleName = ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
                Source = ex?.Source,
                StackTrace = ex?.StackTrace,
                TargetSite = ""
            });
        }

    }
}
