using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Utility;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class ModuleRep
    {
        private static void OneToOne(string sqlConnectionString)
        {

            var userList = new List<SysModule>();
            using (var conn = DbUtility.GetInstance())
            {
                var sql = "select * from Person where id in @ids";
                userList = conn.ReaderModelList<SysModule>(sql).ToList();
            }
        }
        public List<SysModule> GetList(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                  SELECT [Id]
                  ,[ParentId]
                  ,[Layers]
                  ,[EnCode]
                  ,[Name]
                  ,[Icon]
                  ,[UrlAddress]
                  ,[Target]
                  ,[IsMenu]
                  ,[IsExpand]
                  ,[IsPublic]
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
                  FROM  [dbo].[SysModule]
                  where [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and [Name] like @keyword";
                }
                sql = sql + @" order by [DisplayNo]";
                try
                {
                    var rst = db.ReaderModelList<SysModule>(sql, new { DeleteFlag = 0, keyword = "%" + keyword + "%" });
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 通过用户的Id和父Id来加载所分配的功能菜单
        /// </summary>
        /// <param name="_userId">用户Id</param>
        /// <param name="_parentId">父Id 根节点的_parentId =0  </param>
        /// <returns></returns>
        public List<SysModule> LoadFuncMenuByUserId(string _userId)
        {
            using (var db = DbUtility.GetInstance())
            {

                string sql = @"
                              with usermodules as(
                                                select distinct sra.ItemId 
                                                from[dbo].[SysRoleAuthorize]  sra
                                                inner join
                                                [dbo].[SysRoleAuthorizeOperate]  srao 
                                                on  srao.AuthorizeId=sra.Id
                                                where sra.ItemType=1 and sra.ObjectType=1 
                                                and sra.ObjectId in (select [RoleId] 
                                                from [dbo].[SysRoleUser]  
                                                where UserId=@UserId )
                                                )
                            select distinct sysmodule.ParentId from [dbo].[SysModule] sysmodule
                            inner join usermodules     on usermodules.ItemId=sysmodule.Id 
                            and sysmodule.layers <=1 and  sysmodule.layers>=0  
                            ";

                try
                {
                    var rst = db.ReaderModelList<SysModule>(sql, new { UserId = _userId });
                    return rst.ToList();
                }
                catch (Exception)
                {

                    throw;
                }

                #region 测试代码
                //string sql = string.Empty;
                //if (string.IsNullOrEmpty(_parentId))
                //{

                //    sql = "select  *  from  SysModule where ParentId='0'";
                //}
                //else
                //{
                //    sql = "select  *  from  SysModule where ParentId='"+_parentId+"'";
                //}
                //try
                //{
                //    var rst = db.ReaderModelList<SysModule>(sql);
                //    return rst.ToList();

                //}
                //catch (Exception)
                //{

                //    throw;
                //} 
                #endregion
            }

        }

        /// <summary>
        /// 加载某一根节点下面的功能节点
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<SysModule> LoadFuncMenu(string userId, string parentId)
        {
            using (var db = DbUtility.GetInstance())
            {

                string sql = @"
                              with usermodules as(
                                                select distinct sra.ItemId 
                                                from[dbo].[SysRoleAuthorize]  sra
                                                inner join
                                                [dbo].[SysRoleAuthorizeOperate]  srao 
                                                on  srao.AuthorizeId=sra.Id
                                                where sra.ItemType=1 and sra.ObjectType=1 
                                                and sra.ObjectId in (select [RoleId] 
                                                from [dbo].[SysRoleUser]  
                                                where UserId=@UserId )
                                                )
                            select sysmodule.* from [dbo].[SysModule] sysmodule
                            inner join usermodules     on usermodules.ItemId=sysmodule.Id
                           where sysmodule.ParentId=@ParentId
                           order by sysmodule.DisplayNo 
                            ";

                try
                {
                    var rst = db.ReaderModelList<SysModule>(sql, new { UserId = userId, ParentId = parentId });
                    return rst.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public List<SysModuleOperate> GetOperateDataJson(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT smo.[Id]
                  ,smo.[Name]
                  ,smo.[KeyCode]
                  ,smo.[Icon]
                  ,smo.[ModuleId]  
                  ,smo.[Sort]
                  FROM [dbo].[SysModuleOperate] smo
                  where [ModuleId]=@ModuleId
                      order by smo.Sort";
                try
                {
                    var result = db.ReaderModelList<SysModuleOperate>(sql, new { ModuleId = keyValue, RoleId = "" });
                    return result.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public bool DeleteForm(string keyValue, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysModule] 
                    set [DeleteFlag]=1
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return bl == 1;
            }
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public SysModule GetForm(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                  SELECT  [Id]
                  ,[ParentId]
                  ,[Layers]
                  ,[EnCode]
                  ,[Name]
                  ,[Icon]
                  ,[UrlAddress]
                  ,[Target]
                  ,[IsMenu]
                  ,[IsExpand]
                  ,[IsPublic]
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
                  FROM [dbo].[SysModule]
                  where [DeleteFlag]=@DeleteFlag
                  and [Id]=@Id
                  order by [DisplayNo]";
                try
                {
                    var rst = db.ReaderModel<SysModule>(sql, new { DeleteFlag = 0, EnabledFlag = true, Id = keyword });
                    return rst;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        /// <summary>
        /// 提交数据后自动增加
        /// </summary>
        /// <param name="moduleEntity"></param>
        /// <returns></returns>
        public bool SubmitForm(SysModule moduleEntity)
        {
            moduleEntity.CreaterTime = DateTime.Now;
            moduleEntity.LastUpdateTime = DateTime.Now;

            var sql = string.Empty;
            var newModuleId = "";
            if (string.IsNullOrEmpty(moduleEntity.Id))
            {
                moduleEntity.Id = ResultHelper.NewId();
                newModuleId = moduleEntity.Id;
                moduleEntity.ModuleId = moduleEntity.Id;
                sql = @"
                    insert into [dbo].[SysModule]
                   ([Id]
                  ,[ModuleId]
                  ,[ParentId]
                  ,[Layers]
                  ,[EnCode]
                  ,[Name]
                  ,[Icon]
                  ,[UrlAddress]
                  ,[Target]
                  ,[IsMenu]
                  ,[IsExpand]
                  ,[IsPublic]
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
                  ,[DeleteUserId]) 
                  output inserted.Id
                    values
                    (
                    @Id
                    ,@ModuleId
                    ,@ParentId
                    ,@Layers
                    ,@EnCode
                    ,@Name
                    ,@Icon
                    ,@UrlAddress
                    ,@Target
                    ,@IsMenu
                    ,@IsExpand
                    ,@IsPublic
                    ,@AllowEdit
                    ,@AllowDelete
                    ,@DisplayNo
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
                moduleEntity.ModuleId = moduleEntity.Id;
                sql = @"
                    update [dbo].[SysModule] set
                   [ModuleId]=@ModuleId
                  ,[ParentId]=@ParentId
                  ,[Layers]=@Layers
                  ,[EnCode]=@EnCode
                  ,[Name]=@Name
                  ,[Icon]=@Icon
                  ,[UrlAddress]=@UrlAddress
                  ,[Target]=@Target
                  ,[IsMenu]=@IsMenu
                  ,[IsExpand]=@IsExpand
                  ,[IsPublic]=@IsPublic
                  ,[AllowEdit]=@AllowEdit
                  ,[AllowDelete]=@AllowDelete
                  ,[DisplayNo]=@DisplayNo
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
                    //插入数据
                    var effRow = db.ExecuteNonQuery(sql, moduleEntity);
                    //插入成功  并且Id和产生的Id相同  以区分是增加还是修改
                    if (effRow == 1 && newModuleId == moduleEntity.Id)
                    {
                        return AddDefaultFunc(db, newModuleId);

                    }
                    else
                        return false;
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }

        public  bool AddDefaultFunc(DbUtility db, string newModuleId)
        {
            var sql2 = @"insert into [dbo].[SysModuleOperate] ([Id],[Name],[Icon],[KeyCode],[ModuleId],[Sort])
                                values 
                                (@Id,@Name,@Icon,@KeyCode,@ModuleId,@Sort)";
            var res = db.ExecuteNonQuery(sql2, new[] {
                new { Id = ResultHelper.NewId(), Name = "增加",Icon= "fa fa-plus", KeyCode = "Create", ModuleId = newModuleId, Sort = 1 },
                new { Id = ResultHelper.NewId(), Name = "修改", Icon = "fa fa-pencil-square-o", KeyCode = "Edit", ModuleId = newModuleId, Sort = 2 },
                new { Id = ResultHelper.NewId(), Name = "删除", Icon = "fa fa-trash", KeyCode = "Delete", ModuleId = newModuleId, Sort = 3 },
                new { Id = ResultHelper.NewId(), Name = "查看", Icon = "fa fa-search-plus", KeyCode = "Detail", ModuleId = newModuleId, Sort = 4 },
                new { Id = ResultHelper.NewId(), Name = "导出", Icon = "fa fa-file-excel-o", KeyCode = "Export", ModuleId = newModuleId, Sort = 5 } });
            return res == 1;
        }

        public SysModule GetModuleName(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"select Name from [dbo].[SysModule] where Id=@keyValue";
                try
                {
                    var result = db.ReaderModel<SysModule>(sql, new { keyValue = keyValue });
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public SysModuleOperate GetModuleData(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @" select  [SysModuleOperate].Id
                                 ,[SysModuleOperate].[Icon]
                                 ,[SysModuleOperate].[Name]
	                             ,[SysModuleOperate].[KeyCode]
                                 ,SysModule.[Name] as ModuleName
                                 ,[Sort] from [dbo].[SysModuleOperate]  
                             inner join SysModule on SysModule.Id=[SysModuleOperate].ModuleId
                             where [SysModuleOperate].Id=@keyValue";


                try
                {
                    var result = db.ReaderModel<SysModuleOperate>(sql, new { keyValue = keyValue });

                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public bool SubmitOpcode(SysModuleOperate OpcodeEntity)
        {
            var bl = 0;
            using (var db = DbUtility.GetInstance())
            {
                var sql = string.Empty;
                try
                {
                    // db.BeginTransaction();
                    if (!string.IsNullOrEmpty(OpcodeEntity.Id))
                    {
                        sql = @"
                            UPDATE [dbo].[SysModuleOperate]
                            SET
                               [Icon]=@Icon
                              ,[Name]=@Name
                              ,[KeyCode]=@KeyCode
                              ,[Sort]=@Sort 
                              WHERE [SysModuleOperate].Id=@Id
                               ";
                    }
                    else
                    {
                        sql = @"
                            INSERT INTO [dbo].[SysModuleOperate]
                                ([Id],[Name],[Icon],[KeyCode],[ModuleId],[Sort])
                            VALUES 
                                (newID(),@Name,@Icon,@KeyCode,@ModuleId,@Sort)";
                    }
                    //var sql2 = @"  UPDATE [dbo].[SysModuleOperate]
                    //          SET
                    //              [Sort]+=1 
                    //          WHERE [SysModuleOperate].[Sort]>=@Sort
                    //          ";

                    //db.ExecuteNonQuery(sql2, OpcodeEntity);//后移大于或等于改排序的数

                    bl = db.ExecuteNonQuery(sql, OpcodeEntity);

                    //  db.Commit();
                    return bl == 1;
                }
                catch (Exception)
                {
                    // db.Rollback();
                    throw;
                }
            }
        }
        public bool DelOpcode(string keyValue)
        {
            var bl = 0;
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"Delete from [dbo].[SysModuleOperate] where Id=@Id";
                try
                {
                    bl = db.ExecuteNonQuery(sql, new { Id = keyValue });
                    return bl == 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        /*
       修改标识：范文强 2018/10/15
       修改描述：动态注册功能模块
       */
        #region 动态注册模块到数据库

        /// <summary>
        ///动态注册功能模块前先检测是否已经存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public string CheckExist(string Id, string pId)
        {
            string sql = @"select [ParentId] from [SysModule] where [Id]=@Id";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModel<SysModule>(sql, new { Id = Id });
                    if (res == null)
                    {
                        return "None";
                    }
                    else
                        return res.ParentId == pId ? "Exist" : "NotEuqal";
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="moduleEntity">实体</param>
        /// <returns></returns>
        public bool AddModule(SysModule moduleEntity)
        {
            moduleEntity.CreaterTime = DateTime.Now;
            moduleEntity.LastUpdateTime = DateTime.Now;
            moduleEntity.DeleteFlag = false;
            var sql = string.Empty;
            sql = @"
                    insert into [dbo].[SysModule]
                   ([Id]
                  ,[ModuleId]
                  ,[ParentId]
                  ,[Layers]
                  ,[EnCode]
                  ,[Name]
                  ,[Icon]
                  ,[UrlAddress]
                  ,[Target]
                  ,[IsMenu]
                  ,[IsExpand]
                  ,[IsPublic]
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
                  ,[DeleteUserId]) 
                  output inserted.Id
                    values
                    (
                    @Id
                    ,@ModuleId
                    ,@ParentId
                    ,@Layers
                    ,@EnCode
                    ,@Name
                    ,@Icon
                    ,@UrlAddress
                    ,@Target
                    ,@IsMenu
                    ,@IsExpand
                    ,@IsPublic
                    ,@AllowEdit
                    ,@AllowDelete
                    ,@DisplayNo
                    ,@DeleteFlag
                    ,@EnabledFlag
                    ,@Description
                    ,@CreaterTime
                    ,@CreaterUserId
                    ,@LastUpdateTime
                    ,@LastUpdateUserId
                    ,@DeleteTime
                    ,@DeleteUserId)";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, moduleEntity);
                    return effRow == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 修改模块信息  根据Id
        /// </summary>
        /// <param name="moduleEntity"></param>
        /// <returns></returns>
        public bool UpdateModule(SysModule moduleEntity)
        {
            moduleEntity.CreaterTime = DateTime.Now;
            moduleEntity.LastUpdateTime = DateTime.Now;
            string sql = @"
                    update [dbo].[SysModule] set
                    [ModuleId]=@ModuleId
                  , [ParentId]=@ParentId
                  ,[Layers]=@Layers
                  ,[EnCode]=@EnCode
                  ,[Name]=@Name
                 
                  ,[UrlAddress]=@UrlAddress
                  ,[Target]=@Target
                  ,[IsMenu]=@IsMenu
                  ,[IsExpand]=@IsExpand
                  ,[IsPublic]=@IsPublic
                  ,[AllowEdit]=@AllowEdit
                  ,[AllowDelete]=@AllowDelete
                  ,[DisplayNo]=@DisplayNo
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

            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, moduleEntity);
                    return effRow == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public bool UpdateModuleS(SysModule moduleEntity)
        {
            string sql = @"
                  UPDATE [dbo].[SysModule] set                    
                       [Name]=@Name
                  WHERE [Id]=@Id and [Name]<>@Name;

                  UPDATE [dbo].[SysModule] set      
                       [UrlAddress]=@UrlAddress
                  WHERE [Id]=@Id and [UrlAddress]<>@UrlAddress;
                ";

            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, moduleEntity);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion

        /*
         *2018/12/29  范文强  添加
         *添加子节点  先判断是否有子节点
         */
        public bool GetSubMenuItemExist(string userId,string pId)
        {

            var sql = @"    with usermodules as(
                                                select distinct sra.ItemId 
                                                from[dbo].[SysRoleAuthorize]  sra
                                                inner join
                                                [dbo].[SysRoleAuthorizeOperate]  srao 
                                                on  srao.AuthorizeId=sra.Id
                                                where sra.ItemType=1 and sra.ObjectType=1 
                                                and sra.ObjectId in (select [RoleId] 
                                                from [dbo].[SysRoleUser]  
                                                where UserId=@UserId)
                                                )
                            select COUNT(*) from [dbo].[SysModule] sysmodule
                            inner join usermodules     on usermodules.ItemId=sysmodule.Id
                           where sysmodule.ParentId=@ParentId
                           
						   ";
            using (DbUtility  db= DbUtility.GetInstance())
            {
                db.AddParameter("UserId", userId);
                db.AddParameter("ParentId", pId);
                var res = Convert.ToInt32(db.ExecuteScalar(sql));
                return res >= 1;
            }
        }


    }
}
