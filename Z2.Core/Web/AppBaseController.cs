using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Logger;
using Z2.Core.Operator;

namespace Z2.Core.Web
{
    public abstract class AppBaseController : Controller
    {
        public ILogger LogWriter => LogFactory.GetLogger(this.GetType().ToString());
        public OperatorModel CurrentUser => OperatorProvider.Provider.GetCurrent();

        public virtual string ModuleId
        {
            get
            {
                ModuleActionAttribute moduleAttr = (ModuleActionAttribute)this.GetType().GetCustomAttributes(typeof(ModuleActionAttribute), true).FirstOrDefault();
                if (moduleAttr != null)
                {
                    return moduleAttr.ModuleId;
                }
                else
                {
                    return this.GetType().Name;
                }
            }
        }
        public virtual string ModuleName
        {
            get
            {
                ModuleActionAttribute moduleAttr = (ModuleActionAttribute)this.GetType().GetCustomAttributes(typeof(ModuleActionAttribute), true).FirstOrDefault();
                if (moduleAttr != null)
                {
                    return moduleAttr.ModuleName;
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual void WriteInfo( string Result, string Message, object Description)
        {
            LogWriter.Info(new LogContent()
            {
                Account = CurrentUser?.UserId,
                IPAddress = this.Request.UserHostAddress,
                ModuleId = this.ModuleId,
                ModuleName = this.ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
            });
        }

        public virtual void WriteDebug( string Result, string Message, object Description)
        {
            LogWriter.Debug(new LogContent()
            {
                Account = CurrentUser?.UserId,
                IPAddress = this.Request.UserHostAddress,
                ModuleId = this.ModuleId,
                ModuleName = this.ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
            });
        }

        public virtual void WriteWarn( string Result, string Message, object Description)
        {
            LogWriter.Warn(new LogContent()
            {
                Account = CurrentUser?.UserId,
                IPAddress = this.Request.UserHostAddress,
                ModuleId = this.ModuleId,
                ModuleName = this.ModuleName,
                Result = Result,
                Message = Message,
                Description = Description?.ToJson(),
            });
        }


        public virtual void WriteError( string Result, string Message, object Description, Exception ex)
        {
            LogWriter.Error(new LogContent()
            {
                Account = CurrentUser?.UserId,
                IPAddress = this.Request.UserHostAddress,
                ModuleId = this.ModuleId,
                ModuleName = this.ModuleName,
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
