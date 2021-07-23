using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;

namespace Z2.Core.WebApp.Repository
{
    public class SysWidgetRep
    {
        public bool SubmitForm(SysWidgetRep sysWidget)
        {
            var sql = "";
            using (var db=DbUtility.GetInstance())
            {
                sql = @"
                        Insert into [dbo].[SysWidget]
                        (
                               [Id]
                              ,[WidgetId]
                              ,[EnCode]
                              ,[Name]
                              ,[ShortName]
                              ,[Icon]
                              ,[UrlAddress]
                              ,[ColWidth]
                              ,[Height]
                              ,[DisplayNo]
                              ,[DeleteFlag]
                              ,[EnabledFlag]
                              ,[Description]
                              ,[CreaterTime]
                              ,[CreaterUserId]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserId]
                              ,[DeleteTime]
                              ,[DeleteUserId]
                        )
                        Values
                        (
                               @Id              
                              ,@WidgetId        
                              ,@EnCode          
                              ,@Name            
                              ,@ShortName       
                              ,@Icon            
                              ,@UrlAddress      
                              ,@ColWidth        
                              ,@Height          
                              ,@DisplayNo       
                              ,@DeleteFlag      
                              ,@EnabledFlag     
                              ,@Description     
                              ,@CreaterTime     
                              ,@CreaterUserId   
                              ,@LastUpdateTime  
                              ,@LastUpdateUserId
                              ,@DeleteTime      
                              ,@DeleteUserId             
                        )";
                                        var data = db.ExecuteNonQuery(sql, sysWidget);
                return data == 1;
            }
        }
    }
}
