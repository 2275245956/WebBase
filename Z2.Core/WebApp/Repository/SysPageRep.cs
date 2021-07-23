using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class SysPageRep
    {

        public List<SysPage> GetGridJson(string keywords)
        {
            using (var db = DbUtility.GetInstance())
            {
                string sql = @"
                    SELECT [ID]
                      ,[PageID]
                      ,[PageName]
                      ,[Url]
                      ,[PageDiv]
                      ,[PagingCount]
                      ,[ColInfoName]
                      ,[ColInfoID]
                      ,[CreateUserID]
                      ,[CreateDateTime]
                      ,[UpdateUserID]
                      ,[UpdateDateTime]
                  FROM [dbo].[SysPage]
                        where 1=1 ";
                if (!string.IsNullOrEmpty(keywords))
                {
                    sql += " and [PageName] like   @keywords";
                }
                sql += "  Order by UpdateDateTime";

                try
                {
                    var res = db.ReaderModelList<SysPage>(sql, new { keywords = "%" + keywords + "%" });
                    return res.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public bool DeleteForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    delete from [dbo].[SysPage] 
                    where [PageID]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue });
                return bl == 1;
            }
        }
        public SysPage GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                string sql = @"
                    SELECT Top 1 [ID]
                      ,[PageID]
                      ,[PageName]
                      ,[Url]
                      ,[PageDiv]
                      ,[PagingCount]
                      ,[ColInfoName]
                      ,[ColInfoID]
                      ,[CreateUserID]
                      ,[CreateDateTime]
                      ,[UpdateUserID]
                      ,[UpdateDateTime]
                  FROM [dbo].[SysPage]
                    where 1=1";
                if (!string.IsNullOrEmpty(keyValue))
                {
                    sql += " and [PageID] =@keyValue";
                }
                sql += "  Order by UpdateDateTime";

                try
                {
                    var res = db.ReaderModel<SysPage>(sql, new { keyValue = keyValue });
                    return res;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public bool SubmitForm(SysPage pageEntity, string keyValue)
        {
            var sql = string.Empty;
            if (string.IsNullOrEmpty(keyValue))
            {
                sql = @"
                    insert into [dbo].[SysPage]
                    ([PageID]
                    ,[PageName]
                    ,[Url]
                    ,[PageDiv]
                    ,[PagingCount]
                    ,[ColInfoName]
                    ,[ColInfoID]
                    ,[CreateUserID]
                    ,[CreateDateTime])
                    values
                    (@PageID
                    ,@PageName
                    ,@Url
                    ,@PageDiv
                    ,@PagingCount
                    ,@ColInfoName
                    ,@ColInfoID
                    ,@CreateUserID
                    ,@CreateDateTime)";
                pageEntity.CreateUserID = "admin";
                pageEntity.CreateDateTime = DateTime.Now;
            }
            else
            {
                sql = @"
                    update [dbo].[SysPage] set 
                    [PageName]=@PageName
                    ,[Url]=@Url
                    ,[PageDiv]=@PageDiv
                    ,[PagingCount]=@PagingCount
                    ,[ColInfoName]=@ColInfoName
                    ,[ColInfoID]=@ColInfoID
                    ,[UpdateUserID]=@UpdateUserID
                    ,[UpdateDateTime]=@UpdateDateTime
                    where [PageID]=@PageID";
                pageEntity.PageID = keyValue;
                pageEntity.UpdateUserID = "admin";
                pageEntity.UpdateDateTime = DateTime.Now;
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var result = db.ExecuteNonQuery(sql, pageEntity);
                    return result == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}
