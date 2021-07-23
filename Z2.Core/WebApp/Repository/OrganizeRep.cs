using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class OrganizeRep
    {
        public List<SysOrganize> GetList(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT [Id]
                      ,[ParentId]
                      ,[Layers]
                      ,[EnCode]
                      ,[Name]
                      ,[ShortName]
                      ,[CategoryId]
                      ,[ManagerId]
                      ,[TelePhone]
                      ,[MobilePhone]
                      ,[WeChat]
                      ,[Fax]
                      ,[Email]
                      ,[AreaId]
                      ,[Address]
                      ,[AllowEdit]
                      ,[AllowDelete]
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
                  FROM [dbo].[SysOrganize]
                  Where [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and [Name] like @keyword";
                }
                sql = sql + @" order by [DisplayNo]";
                //if (!string.IsNullOrEmpty(keyword))
                //{
                //    db.AddParameter("keyword", keyword);
                //}
                try
                {
                    var rst = db.ReaderModelList<SysOrganize>(sql, new { DeleteFlag = 0, keyword = "%" + keyword + "%" });
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        public bool SubmitForm(SysOrganize organizeEntity)
        {
            organizeEntity.CreaterTime = DateTime.Now;
            organizeEntity.LastUpdateTime = DateTime.Now;

            var sql = string.Empty;
            if (string.IsNullOrEmpty(organizeEntity.Id))
            {
                sql = @"
                    INSERT INTO [dbo].[SysOrganize]
                               ([Id]
                               ,[ParentId]
                               ,[Layers]
                               ,[EnCode]
                               ,[Name]
                               ,[ShortName]
                               ,[CategoryId]
                               ,[ManagerId]
                               ,[TelePhone]
                               ,[MobilePhone]
                               ,[WeChat]
                               ,[Fax]
                               ,[Email]
                               ,[AreaId]
                               ,[Address]
                               ,[AllowEdit]
                               ,[AllowDelete]
                               ,[DisplayNo]
                               ,[EnabledFlag]
                               ,[Description]
                               ,[CreaterTime]
                               ,[CreaterUserId]
                               ,[DeleteFlag]
                            )
                         VALUES
                               (newid()
                               ,@ParentId
                               ,@Layers
                               ,@EnCode
                               ,@Name
                               ,@ShortName
                               ,@CategoryId
                               ,@ManagerId
                               ,@TelePhone
                               ,@MobilePhone
                               ,@WeChat
                               ,@Fax
                               ,@Email
                               ,@AreaId
                               ,@Address
                               ,@AllowEdit
                               ,@AllowDelete
                               ,@DisplayNo
                               ,@EnabledFlag
                               ,@Description
                               ,@CreaterTime
                               ,@CreaterUserId
                               ,@DeleteFlag
                                )";
            }
            else
            {
                sql = @"
                UPDATE [dbo].[SysOrganize]
                   SET [ParentId] = @ParentId
                      ,[Layers] = @Layers
                      ,[EnCode] = @EnCode
                      ,[Name] = @Name
                      ,[ShortName] = @ShortName
                      ,[CategoryId] = @CategoryId
                      ,[ManagerId] = @ManagerId
                      ,[TelePhone] = @TelePhone
                      ,[MobilePhone] = @MobilePhone
                      ,[WeChat] = @WeChat
                      ,[Fax] = @Fax
                      ,[Email] = @Email
                      ,[AreaId] = @AreaId
                      ,[Address] = @Address
                      ,[EnabledFlag] = @EnabledFlag
                      ,[Description] = @Description
                      ,[LastUpdateTime] = @LastUpdateTime
                      ,[LastUpdateUserId] = @LastUpdateUserId
                 WHERE [Id] = @Id";
            }

            using (var db = DbUtility.GetInstance())
            {
                var effRow = db.ExecuteNonQuery(sql, organizeEntity);
                return effRow == 1;
            }
        }

        public SysOrganize GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT [Id]
                      ,[ParentId]
                      ,[Layers]
                      ,[EnCode]
                      ,[Name]
                      ,[ShortName]
                      ,[CategoryId]
                      ,[ManagerId]
                      ,[TelePhone]
                      ,[MobilePhone]
                      ,[WeChat]
                      ,[Fax]
                      ,[Email]
                      ,[AreaId]
                      ,[Address]
                      ,[AllowEdit]
                      ,[AllowDelete]
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
                  FROM [dbo].[SysOrganize]
                  Where DeleteFlag=@DeleteFlag
                  and Id=@keyValue
                ";
                var rst = db.ReaderModel<SysOrganize>(sql, new { DeleteFlag = false, keyValue = keyValue });
                return rst;
            }
        }

        public bool DeleteForm(string keyValue, string userId)
        {
            var sql = @"
                UPDATE [dbo].[SysOrganize]
                   SET [DeleteFlag] = 1
                      ,[DeleteTime] = @DeleteTime
                      ,[DeleteUserId] = @DeleteUserId
                 WHERE [Id] = @keyValue";

            using (var db = DbUtility.GetInstance())
            {
                var effRow = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return effRow == 1;
            }
        }
    }
}
