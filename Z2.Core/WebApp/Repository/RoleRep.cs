using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Utility;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class RoleRep
    {
        public List<SysRole> GetList(string type, string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT [SysRole].[Id]
                  ,[SysRole].[OrganizeId]
                  ,[SysRole].[Category]
                  ,[SysRole].[EnCode]
                  ,[SysRole].[Name]
                  ,[SysRole].[Type]
                  ,[SysRole].[AllowEdit]
                  ,[SysRole].[AllowDelete]
                  ,[SysRole].[DisplayNo]
                  ,[SysRole].[DeleteFlag]
                  ,[SysRole].[EnabledFlag]
                  ,[SysRole].[Description]
                  ,[SysRole].[CreaterTime]
                  ,[SysRole].[CreaterUserId]
                  ,[SysRole].[LastUpdateTime]
                  ,[SysRole].[LastUpdateUserId]
                  ,[SysRole].[DeleteTime]
                  ,[SysRole].[DeleteUserId]
				  ,[SysOrganize].[Name] as OrganizeName
                  FROM [dbo].[SysRole]
				  left join [SysOrganize] on [SysOrganize].Id=[SysRole].OrganizeId
                  Where [SysRole].[DeleteFlag]=@DeleteFlag and [Type]=@type";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and [SysRole].[Name] like @keyword";
                }

                sql = sql + @" order by [DisplayNo]";
                try
                {
                    var rst = db.ReaderModelList<SysRole>(sql, new { DeleteFlag = 0, type = type, keyword = "%" + keyword + "%" });
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        public SysRole GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT  [Id]
                  ,[OrganizeId]
                  ,[Category]
                  ,[EnCode]
                  ,[Name]
                  ,[Type]
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
                  FROM [dbo].[SysRole]
                  Where [DeleteFlag]=@DeleteFlag
                  and Id=@keyValue";
                var rst = db.ReaderModel<SysRole>(sql, new { DeleteFlag = false, keyValue = keyValue });
                return rst;
            }
        }
        public bool DeleteForm(string keyValue, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysRole] 
                    set [DeleteFlag]=1
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return bl == 1;
            }
        }
        public bool SubmitForm(SysRole roleEntity)
        {
            roleEntity.CreaterTime = DateTime.Now;
            roleEntity.LastUpdateTime = DateTime.Now;
            roleEntity.DeleteFlag = Convert.ToBoolean(roleEntity.DeleteFlag);
            var sql = string.Empty;
            if (string.IsNullOrEmpty(roleEntity.Id))
            {
                roleEntity.Id = ResultHelper.NewId();
                sql = @"
                    insert into [dbo].[SysRole]
                    ([Id]
                    ,[OrganizeId]
                  ,[Category]
                  ,[EnCode]
                  ,[Name]
                  ,[Type]
                  ,[DisplayNo]
                  ,[AllowEdit]
                  ,[AllowDelete]
                  ,[DeleteFlag]
                  ,[EnabledFlag]
                  ,[Description]
                  ,[CreaterTime]
                  ,[CreaterUserId]
                  ,[LastUpdateTime]
                  ,[LastUpdateUserId]
                  ,[DeleteTime]
                  ,[DeleteUserId])
                    values
                    (
                    @Id
                    ,@OrganizeId
                    ,@Category
                    ,@EnCode
                    ,@Name
                    ,@Type
                    ,@DisplayNo
                    ,@AllowEdit
                    ,@AllowDelete
                    ,@DeleteFlag
                    ,@EnabledFlag
                    ,@Description
                    ,@CreaterTime
                    ,@CreaterUserId
                    ,@LastUpdateTime
                    ,@LastUpdateUserId
                    ,@DeleteTime
                    ,@DeleteUserId)";
            }
            else
            {
                sql = @"
                    update [dbo].[SysRole] set
                   [OrganizeId]=@OrganizeId
                  ,[Category]=@Category
                  ,[EnCode]=@EnCode
                  ,[Name]=@Name
                  ,[Type]=@Type
                  ,[DisplayNo]=@DisplayNo
                  ,[AllowEdit]=@AllowEdit
                  ,[AllowDelete]=@AllowDelete
                  ,[DeleteFlag]=@DeleteFlag
                  ,[EnabledFlag]=@EnabledFlag
                  ,[Description]=@Description
                  ,[CreaterTime]=@CreaterTime
                  ,[CreaterUserId]=@CreaterUserId
                  ,[LastUpdateTime]=@LastUpdateTime
                  ,[LastUpdateUserId]=@LastUpdateUserId
                  ,[DeleteTime]=@DeleteTime
                  ,[DeleteUserId]=@DeleteUserId
                  where [Id]=@Id";
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, roleEntity);
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

    }
}
