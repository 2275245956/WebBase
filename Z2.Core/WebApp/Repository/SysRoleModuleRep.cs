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
    public class SysRoleModuleRep
    {
        public List<SysModuleOperate> GetOperateDataJson(string roleValue, string moduleValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT 
                    auth.Id as AuthId,
                    auth.AuthorizeId,
                           smo.[Id]
                          ,smo.[Name]
                          ,smo.[KeyCode]
                          ,smo.[ModuleId]
                          ,smo.[Icon]  
                          ,smo.Sort
                      FROM [dbo].[SysModuleOperate] smo
                       left join (
                     SELECT 
	                     srao.Id
	                    ,srao.AuthorizeId
	                    ,srao.KeyCode
                      FROM [dbo].[SysRoleAuthorize] sra
                      inner join  [dbo].[SysRoleAuthorizeOperate] srao on srao.AuthorizeId=sra.Id
                      Where sra.ItemType=1 and sra.ItemId=@ModuleId
                      and sra.ObjectType=1 and sra.ObjectId=@RoleId
                      ) auth on auth.KeyCode=smo.[KeyCode]
                      where smo.ModuleId=@ModuleId 
                      order by smo.Sort";
                try
                {
                    var result = db.ReaderModelList<SysModuleOperate>(sql, new { RoleId = roleValue, ModuleId = moduleValue });
                    return result.ToList();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        private ModuleRep mr = new ModuleRep();
        public bool SaveRoleAuthorize(string roleValue,
            string moduleValue,
            List<SysModuleOperate> roleAuthorizeEntity)
        {
            var AuthorizeId = ResultHelper.NewId();
            var bl = 0;
            /*暂时   可以    只使用最多三级菜单   可重用性不强    待完善*/
            var pidTwo = mr.GetForm(moduleValue).ParentId;
            var pid = mr.GetForm(pidTwo).ParentId;
            if (pid != "0")
            {
                moduleValue = pidTwo;
            }

            using (var db = DbUtility.GetInstance())
            {
                db.BeginTransaction();
                var sql = @"delete from [dbo].[SysRoleAuthorize] where [ItemId]=@moduleValue
                        and [ObjectId]=@roleValue
                        insert into [dbo].[SysRoleAuthorize]
                        (Id
                        ,ItemType
                        ,ItemId
                        ,ObjectType
                        ,ObjectId
                        ,DisplayNo
                        ,CreaterTime
                        ,CreaterUserId)
                        values
                        (@AuthorizeId
                        ,1
                        , @moduleValue
                        ,1
                        , @roleValue
                        , @DisplayNo
                        , @CreaterTime
                        , @CreaterUserId)";
                try
                {
                    bl = db.ExecuteNonQuery(sql, new { AuthorizeId = AuthorizeId, moduleValue = moduleValue, roleValue = roleValue, DisplayNo = 1, CreaterTime = DateTime.Now, CreaterUserId = "admin" });
                }
                catch (Exception e)
                {
                    throw;
                }
                if (roleAuthorizeEntity != null)
                {
                    for (int i = 0; i < roleAuthorizeEntity.Count(); i++)
                    {
                        var sb = @"insert into [dbo].[SysRoleAuthorizeOperate]
                            (Id
                            ,AuthorizeId
                            ,KeyCode
                            ,IsValid
                            ,Icon
                            ,Name
                            ,Sort)
                            values
                            (@Id
                            ,@AuthorizeId
                            ,@KeyCode
                            ,@IsValid       
                            ,@Icon
                            ,@Name
                            ,@Sort)";
                        bl = db.ExecuteNonQuery(sb, new { Id = ResultHelper.NewId(), AuthorizeId = AuthorizeId, KeyCode = roleAuthorizeEntity[i].KeyCode, IsValid = "",Icon=roleAuthorizeEntity[i].Icon,Name=roleAuthorizeEntity[i].Name,Sort = roleAuthorizeEntity[i].Sort });
                    }
                }
                db.Commit();
                return bl == 1;
            }
        }
    }
}
