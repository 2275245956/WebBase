using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;

namespace Z2.Core.Logger
{
    class DBLogger : ILogger
    {
        public void Debug(LogContent message)
        {
            message.SetLogType(LogType.Debug);
            WriteLog(message);
        }

        public void Error(LogContent message)
        {
            message.SetLogType(LogType.Error);
            WriteLog(message);
        }

        public void Info(LogContent message)
        {
            message.SetLogType(LogType.Info);
            WriteLog(message);
        }

        public void Warn(LogContent message)
        {
            message.SetLogType(LogType.Warn);
            WriteLog(message);
        }

        private void WriteLog(LogContent log)
        {
            if (log == null)
            {
                return;
            }
            Task.Factory.StartNew((obj) =>
            {
                using (var db = DbUtility.GetInstance())
                {
                    var sql = @"
                        INSERT INTO [dbo].[SysLog]
                                   ([Id]
                                   ,[Account]
                                   ,[IPAddress]
                                   ,[ModuleId]
                                   ,[ModuleName]
                                   ,[Type]
                                   ,[Result]
                                   ,[Message]
                                   ,[Source]
                                   ,[StackTrace]
                                   ,[TargetSite]
                                   ,[Description]
                                   ,[CreateTime])
                             VALUES
                                   (NewId()
                                   ,@Account
                                   ,@IPAddress
                                   ,@ModuleId
                                   ,@ModuleName
                                   ,@Type
                                   ,@Result
                                   ,@Message
                                   ,@Source
                                   ,@StackTrace
                                   ,@TargetSite
                                   ,@Description
                                   ,getdate())
                            ";
                    try
                    {
                         db.ExecuteNonQuery(sql, obj);
                    }
                    catch (Exception)
                    {
                    }
                }
            }, log);
        }
    }
}
