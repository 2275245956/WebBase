using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Presentation;
using Z2.Core.DataBases;
using Z2.Core.Operator;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class LayOutRep
    {


        #region 获取组件类型===========

        public List<CMS_Layout> GetList(string keyword)
        {
            var sql = @"
                           SELECT 
                               [ID]
                              ,[LayoutName]
                              ,[Title]
                              ,[ContainerClass]
                              ,[EnabledFlag]
                              ,[Description]
                              ,[Script]
                              ,[Style]
                              ,[DisplayNo]
                              ,[DeleteFlag]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                              ,[ImageUrl]
                              ,[ImageThumbUrl]
                              ,[Theme]
                              ,[DeleteTime]
                              ,[DeleteUserId]
                          FROM [dbo].[CMS_Layout]
                          Where [DeleteFlag]=0 
                    ";
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " and [LayoutName] like '%" + keyword + "%'";//可以有其他的模糊查询  此处只按照名称
            }

            sql += " order by [DisplayNo]";
            using (var db = DbUtility.GetInstance())
            {
                var data = db.ReaderModelList<CMS_Layout>(sql);
                return data.ToList();
            }
        }


        #endregion

        #region 删除信息===============

        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteForm(string id)
        {
            var delTime = DateTime.Now;
            var delUserId = OperatorProvider.Provider.GetCurrent().UserId;
            var sql = @"Update [dbo].[CMS_Layout]
                        Set    [DeleteFlag]=1
                              ,[DeleteTime]=@DeleteTime
                              ,[DeleteUserId]=@DeleteUserId
                        Where [ID]=@Id
                        ";
            using (var db = DbUtility.GetInstance())
            {
                try

                {
                    var data = db.ExecuteNonQuery(sql, new { DeleteTime = delTime, DeleteUserId = delUserId, Id = id });
                    return data == 1;
                }

                catch (Exception)
                {

                    throw;
                }
            }

        }

        #endregion

        #region 获取一条数据===========

        public CMS_Layout GetForm(string Id)
        {
            var sql = @"
                            SELECT 
                               [ID]
                              ,[LayoutName]
                              ,[Title]
                              ,[ContainerClass]
                              ,[EnabledFlag]
                              ,[Description]
                              ,[Script]
                              ,[Style]
                              ,[DisplayNo]
                              ,[DeleteFlag]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                              ,[ImageUrl]
                              ,[ImageThumbUrl]
                              ,[Theme]
                              ,[DeleteTime]
                              ,[DeleteUserId]
                          FROM [dbo].[CMS_Layout]
                          Where [DeleteFlag]=0  and [ID]=@Id
                       ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var data = db.ReaderModel<CMS_Layout>(sql, new { Id = Id });
                    return data;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion


        #region 获取组件的布局=========

        private static string layoutId = "";
        public List<CMS_LayoutHtml> GetLayOutHtml(string ID)
        {
            layoutId = ID;
            var sql = @"
                        SELECT [LayoutHtmlId]
                              ,[LayoutId]
                              ,[Html]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                          FROM [dbo].[CMS_LayoutHtml]
                          WHERE [LayoutId]=@LayoutId
                          Order By LayoutHtmlId
                        ";
            using (var db = DbUtility.GetInstance())
            {
                var data = db.ReaderModelList<CMS_LayoutHtml>(sql, new { LayoutId = ID });
                return data.ToList();
            }
        }


        public List<CMS_WidgetBase> GetWidgetBases(string Id)
        {
            var sql = @"
                        SELECT [ID]
                              ,[WidgetName]
                              ,[Title]
                              ,[Position]
                              ,[LayoutId]
                              ,[PageId]
                              ,[ZoneId]
                              ,[PartialView]
                              ,[AssemblyName]
                              ,[ServiceTypeName]
                              ,[ViewModelTypeName]
                              ,[FormView]
                              ,[StyleClass]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                              ,[Description]
                              ,[Status]
                              ,[IsTemplate]
                              ,[Thumbnail]
                              ,[IsSystem]
                              ,[ExtendData]
                          FROM [dbo].[CMS_WidgetBase]
                          WHERE [LayoutId]=@LayoutId
                        ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {

                    var data = db.ReaderModelList<CMS_WidgetBase>(sql, new { LayoutId = Id });

                    return data.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }


        public CMS_Zone GetZoneList(string Id)
        {
            var sql = @"
                        SELECT [ID]
                              ,[LayoutId]
                              ,[ZoneName]
                              ,[Title]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                              ,[Description]
                              ,[Status]
                          FROM [dbo].[CMS_Zone]
                        Where [ID]=@ID
                          
                    ";
            using (var db = DbUtility.GetInstance())
            {
                var data = db.ReaderModel<CMS_Zone>(sql, new { ID = Id });
                return data;
            }
        }

        #endregion

        #region 提交表单增加/修改操作==

        public bool SubmitForm(CMS_Layout layout, string keyValue)
        {
            var sql = string.Empty;
            layout.Title = layout.LayoutName;  //初始化数据

            if (string.IsNullOrEmpty(keyValue))
            {
                //数据初始化
                layout.ID = Guid.NewGuid().ToString();
                layout.CreateDate = DateTime.Now;
                layout.DeleteFlag = false;
                layout.CreateBy = OperatorProvider.Provider.GetCurrent().UserId;
                sql = @"Insert into [dbo].[CMS_Layout]
                        (
                               [ID]
                              ,[LayoutName]
                              ,[Title]
                              ,[ContainerClass]
                              ,[EnabledFlag]
                              ,[Description]
                              ,[Script]
                              ,[Style]
                              ,[DisplayNo]
                              ,[DeleteFlag]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                              ,[ImageUrl]
                              ,[ImageThumbUrl]
                              ,[Theme]
                              ,[DeleteTime]
                              ,[DeleteUserId]
                            )
                            Values
                            (
                               @ID                    
                              ,@LayoutName            
                              ,@Title                 
                              ,@ContainerClass        
                              ,@EnabledFlag           
                              ,@Description           
                              ,@Script                
                              ,@Style                 
                              ,@DisplayNo             
                              ,@DeleteFlag            
                              ,@CreateBy              
                              ,@CreatebyName          
                              ,@CreateDate            
                              ,@LastUpdateBy          
                              ,@LastUpdateByName      
                              ,@LastUpdateDate        
                              ,@ImageUrl              
                              ,@ImageThumbUrl         
                              ,@Theme                 
                              ,@DeleteTime            
                              ,@DeleteUserId                  
                            )                            
                            ";
            }
            else
            {
                layout.LastUpdateBy = OperatorProvider.Provider.GetCurrent().UserId;
                layout.LastUpdateByName = OperatorProvider.Provider.GetCurrent().UserName;
                layout.LastUpdateDate = DateTime.Now;
                layout.ID = keyValue;
                sql = @"Update [dbo].[CMS_Layout]
                        Set
                              [LayoutName]      = @LayoutName      
                             ,[Title]           = @Title           
                             ,[EnabledFlag]     = @EnabledFlag     
                             ,[Description]     = @Description     
                             ,[DisplayNo]       = @DisplayNo       
                             ,[LastUpdateBy]    = @LastUpdateBy    
                             ,[LastUpdateByName]= @LastUpdateByName
                             ,[LastUpdateDate]  = @LastUpdateDate  
                         Where [ID]=@ID
                         ";
            }

            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    if (layout.DisplayNo == 0 || layout.DisplayNo == null)
                    {
                        var countSql = "select max(DisplayNo) from [dbo].[CMS_Layout]";
                        layout.DisplayNo = (int)db.ExecuteScalar(countSql) + 1;
                    }
                    var data = db.ExecuteNonQuery(sql, layout);
                    return data == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        #endregion


        #region 保存  html布局=========

        private readonly CMS_LayoutHtml _cmsLayoutHtml = new CMS_LayoutHtml();
        public bool SaveLayoutHtl(LayoutHtmlCollection html)
        {

            using (var db = DbUtility.GetInstance())
            {
                db.BeginTransaction();
                var sqlDel = string.Empty;
                //先删除  在增加
                if (!string.IsNullOrEmpty(layoutId))
                    sqlDel = @"Delete from CMS_LayoutHtml where LayoutId=@LayoutId";
                try
                {
                    var r = db.ExecuteNonQuery(sqlDel, new { LayoutId = layoutId });
                    var res = InsertIntoCMS_LayoutHtml(html, db);
                    db.Commit();
                    return r >= 1 && res;
                }
                catch (Exception e)
                {
                    db.Rollback();
                    throw e;
                }
            }

        }

        private bool InsertIntoCMS_LayoutHtml(LayoutHtmlCollection html, DbUtility db)
        {
            int effRows = 0;
            var creator = OperatorProvider.Provider.GetCurrent().UserId;
            var creatorName = OperatorProvider.Provider.GetCurrent().UserName;
            var createDate = DateTime.Now;
            var lastUpdateuserId = creator;
            var lastUpdatebyName = creatorName;
            var lastupdateDate = createDate;
            var sql = @"INSERT INTO [dbo].[CMS_LayoutHtml]
			                (
                             LayoutId
			                ,Html
			                ,CreateBy
			                ,CreatebyName
			                ,CreateDate
			                ,LastUpdateBy
			                ,LastUpdateByName
			                ,LastUpdateDate)
                       VALUES 
			                (
                            @LayoutId
			                ,@Html
			                ,@CreateBy
			                ,@CreatebyName
			                ,@CreateDate
			                ,@LastUpdateBy
			                ,@LastUpdateByName
			                ,@LastUpdateDate
			                 )";

            foreach (var item in html)
            {
                _cmsLayoutHtml.Html = item.Html;
                _cmsLayoutHtml.CreateBy = creator;
                _cmsLayoutHtml.CreateDate = createDate;
                _cmsLayoutHtml.CreatebyName = creatorName;
                _cmsLayoutHtml.LastUpdateBy = lastUpdateuserId;
                _cmsLayoutHtml.LastUpdateByName = lastUpdatebyName;
                _cmsLayoutHtml.LayoutId = layoutId;
                _cmsLayoutHtml.LastUpdateDate = lastupdateDate;
                effRows += db.ExecuteNonQuery(sql, _cmsLayoutHtml);
            }

            return effRows >= 1;
        }


        #endregion

        #region 保存  Zone布局=========

        public bool SaveZone(CMS_Zone.ZonesCollection zones)
        {
            var upsuccess = false;
            var insertSuccess = false;
            using (var db = DbUtility.GetInstance())
            {
                db.BeginTransaction();
                if (string.IsNullOrEmpty(layoutId))
                    return false;
                try
                { //先判断是否存在在表中  存在修改     不存在增加
                    foreach (var item in zones)
                    {

                        if (CheckExist(item.ID, db)) //更新
                            upsuccess = UpdateZone(item, db);
                        else //增加
                            insertSuccess = InsertIntoCMS_Zone(item, db);

                    }
                    db.Commit();
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
                return upsuccess && insertSuccess;
            }
        }

        /// <summary>
        /// 判断是否存在数据库中
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool CheckExist(string id, DbUtility db)
        {
            try
            {
                var sql = "select * from CMS_Zone where ID=@Id";
                var count = db.ReaderModel<CMS_Zone>(sql, new { Id = id });
                return count != null;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public bool UpdateZone(CMS_Zone zones, DbUtility db)
        {

            zones.CreateDate = DateTime.Now;
            zones.CreateBy = OperatorProvider.Provider.GetCurrent().UserId;
            zones.CreatebyName = OperatorProvider.Provider.GetCurrent().UserName;
            zones.LastUpdateBy = OperatorProvider.Provider.GetCurrent().UserId;
            zones.LastUpdateByName = OperatorProvider.Provider.GetCurrent().UserName;
            zones.LastUpdateDate = DateTime.Now;
            zones.LayoutId = layoutId;
            var sql = @"
                        UPDATE [dbo].[CMS_Zone]
		                        SET	 [ID]=@ID
			                        ,[LayoutId]=@LayoutId
			                        ,[ZoneName]=@ZoneName
			                        ,[Title]=@Title
			                        ,[CreateBy]=@CreateBy
			                        ,[CreatebyName]=@CreatebyName
			                        ,[CreateDate]=@CreateDate
			                        ,[LastUpdateBy]=@LastUpdateBy
			                        ,[LastUpdateByName]=@LastUpdateByName
			                        ,[LastUpdateDate]=@LastUpdateDate
			                        ,[Description]=@Description
			                        ,[Status]=@Status
                         WHERE  [ID]=@ID
                        ";
            return db.ExecuteNonQuery(sql, zones) >= 1;



        }

        public bool InsertIntoCMS_Zone(CMS_Zone zones, DbUtility db)
        {
            var sql = @" INSERT INTO [dbo].[CMS_Zone]
			                            (ID
			                            ,LayoutId
			                            ,ZoneName
			                            ,Title
			                            ,CreateBy
			                            ,CreatebyName
			                            ,CreateDate
			                            ,LastUpdateBy
			                            ,LastUpdateByName
			                            ,LastUpdateDate
			                            ,Description
			                            ,Status)
                            VALUES 
			                            (
                                         @ID
			                            ,@LayoutId
			                            ,@ZoneName
			                            ,@Title
			                            ,@CreateBy
			                            ,@CreatebyName
			                            ,@CreateDate
			                            ,@LastUpdateBy
			                            ,@LastUpdateByName
			                            ,@LastUpdateDate
			                            ,@Description
			                            ,@Status
			                            )
                                ";

            zones.CreateDate = DateTime.Now;
            zones.CreateBy = OperatorProvider.Provider.GetCurrent().UserId;
            zones.CreatebyName = OperatorProvider.Provider.GetCurrent().UserName;
            zones.LastUpdateBy = OperatorProvider.Provider.GetCurrent().UserId;
            zones.LastUpdateByName = OperatorProvider.Provider.GetCurrent().UserName;
            zones.LastUpdateDate = DateTime.Now;
            zones.LayoutId = layoutId;

            return db.ExecuteNonQuery(sql, zones) >= 1;
        }



    }

    #endregion
}
