using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public partial class AuthoritiesCheckRep
    {
        /// <summary>
        /// 是否分配了相应的模块
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public SysModule HasAuthorities(string userId, string moduleId)
        {
            const string sql = @"with usermodules as(
                                     select distinct sra.ItemId 
                                                from[dbo].[SysRoleAuthorize]  sra
                                                inner join
                                                [dbo].[SysRoleAuthorizeOperate]  srao 
                                                on  srao.AuthorizeId=sra.Id
                                                where sra.ItemType=1 and sra.ObjectType=1 
                                                and sra.ObjectId in (select [RoleId] 
                                                from [dbo].[SysRoleUser]  
                                                where UserId=@userId )           
                                                )
                            select * from [dbo].[SysModule] sysmodule
                            inner join usermodules     on usermodules.ItemId=sysmodule.Id
							where sysmodule.ModuleId=@moduleId";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModel<SysModule>(sql, new { userId = userId, moduleId = moduleId });
                    return res;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 模块是否分配了操作码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public List<SysRoleAuthorizeOperate> HasOperateAuthorities(string userId, string moduleId)
        {
            const string sql = @"     select * from SysRoleAuthorizeOperate where AuthorizeId  in (
									select sra.Id
                                                from[dbo].[SysRoleAuthorize]  sra
                                                inner join
                                                [dbo].[SysRoleAuthorizeOperate]  srao 
                                                on  srao.AuthorizeId=sra.Id
                                                where sra.ItemType=1 and sra.ObjectType=1 and 
												sra.ItemId=@moduleId
                                                and sra.ObjectId in (select [RoleId] 
                                                from [dbo].[SysRoleUser]  
                                                where UserId=@userId )  )  ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModelList<SysRoleAuthorizeOperate>(sql, new { userId = userId, moduleId = moduleId });
                    return res.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 获得角色  模块Id对应的  Id集合  来判断是否存在操作码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public List<SysRoleAuthorize> GetAuthorizesIds(string userId,string moduleId)
        {
            var sql = @"select  id 
                            from SysRoleAuthorize
                            where itemId = @moduleId
                            and ObjectId in 
                                (select[RoleId]
                                   from[dbo].[SysRoleUser]
                                   where UserId = @userId ) ";
            using (var db=DbUtility.GetInstance())
            {
                var data = db.ReaderModelList<SysRoleAuthorize>(sql, new {userId = userId, moduleId = moduleId});
                return data.ToList();
            }
           
        }
        /// <summary>
        /// Id 对应的AuthorizeId 是否有操作码
        /// </summary>
        /// <param name="id">Id  对应AuthorizeId</param>
        /// <returns></returns>
        public bool IsExist(string id)
        {
            var sql = "select  * from SysRoleAuthorizeOperate where AuthorizeId=@Id";
            using (var db=DbUtility.GetInstance())
            {
                var data = db.ReaderModelList<SysRoleAuthorizeOperate>(sql, new {Id = id});
                return data.ToList().Count > 0;

            }
        }
        /// <summary>
        /// 没有分配操作码  就删除用户对应的模块分配
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public bool DeleteModuleAuthorties(string id)
        {
            var sql = "delete from SysRoleAuthorize where Id=@Id";
            using (var db=DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ExecuteNonQuery(sql,new {Id=id});
                    return res == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

    }
}
