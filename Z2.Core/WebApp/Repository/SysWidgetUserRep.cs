using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class SysWidgetUserRep
    {
        public bool SubmitForm(SysWidgetUser sysWidgetUser)
        {
            var sql = "";
            using (var db=DbUtility.GetInstance())
            {

                sql = @"
                        Insert into [dbo].[SysWidgetUser]
                        (
                               [Id]
                              ,[WidgetId]
                              ,[UserId]
                              ,[Row]
                              ,[Col]
                              ,[ColWidth]
                              ,[Height]
                        )
                        Values
                        (
                               @Id      
                              ,@WidgetId
                              ,@UserId  
                              ,@Row     
                              ,@Col     
                              ,@ColWidth
                              ,@Height        
                        )
                              
                        ";
                var data = db.ExecuteNonQuery(sql, sysWidgetUser);
                return data == 1;

            }

        }
    }
}
