using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Operator;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class CMS_WidgetBaseRep
    {
        public static string GetWidgetData(string Id)
        {
            var sql = @"
                    SELECT 
                       [WidgetData]
                    FROM [dbo].[CMS_WidgetBase]
                    Where Id=@ID";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.AddParameter("ID", Id);
                    var data = Utility.Converts.ToTryString(db.ExecuteScalar(sql));
                    return data;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static bool SetWidgetData(string Id, string WidgetData)
        {
            var sql = @"
                UPDATE [dbo].[CMS_WidgetBase]
                    SET [WidgetData] = @WidgetData
                WHERE Id=@ID";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.AddParameter("ID", Id);
                    db.AddParameter("WidgetData", WidgetData);
                    var data = db.ExecuteNonQuery(sql) == 1;
                    return data;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        #region  布局--》加载、删除、修改==========

        public List<CMS_WidgetBase> GetWidgetByLayoutId(string LayoutId)
        {
            var sql = @"
                        SELECT [ID]
                              ,[WidgetId]
                              ,[Title]
                              ,[Position]
                              ,[LayoutId]
                              ,[PageId]
                              ,[ZoneId]
                              ,[StyleClass]
                              ,[WidgetData]
                              ,[ExtendData]
                              ,[IsSystem]
                              ,[Description]
                              ,[DeleteFlag]
                              ,[EnabledFlag]
                              ,[CreaterTime]
                              ,[CreaterUserId]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserId]
                              ,[DeleteTime]
                              ,[DeleteUserId]
                          FROM [dbo].[CMS_WidgetBase]
                          WHERE [LayoutId]=@LayoutId
                        ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var data = db.ReaderModelList<CMS_WidgetBase>(sql, new { LayoutId = LayoutId });
                    return data.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 修改组件的信息  
        /// </summary>
        /// <param name="cmsWidgetBase"></param>
        /// <returns></returns>
        public bool UpdateWidget(CMS_WidgetBase cmsWidgetBase)
        {
            var sql = @"
                        UPDATE [dbo].[CMS_WidgetBase]
		                        SET	 
			                         [WidgetId]=@WidgetId
			                        ,[Title]=@Title
			                        ,[Position]=@Position
			                        ,[LayoutId]=@LayoutId
			                        ,[PageId]=@PageId
			                        ,[ZoneId]=@ZoneId
			                        ,[StyleClass]=@StyleClass
			                        ,[WidgetData]=@WidgetData
			                        ,[ExtendData]=@ExtendData
			                        ,[Description]=@Description
			                        ,[EnabledFlag]=@EnabledFlag
			                        ,[LastUpdateTime]=@LastUpdateTime
			                        ,[LastUpdateUserId]=@LastUpdateUserId
			              WHERE  [ID]=@ID
                       ";
            using (var db = DbUtility.GetInstance())
            {
                cmsWidgetBase.LastUpdateTime = DateTime.Now;
                cmsWidgetBase.LastUpdateUserId = OperatorProvider.Provider.GetCurrent().UserId;

                try
                {
                    var data = db.ExecuteNonQuery(sql, cmsWidgetBase);
                    return data >= 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }


        /// <summary>
        /// 删除 组件    直接删除  
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns></returns>
        public bool DeleteWidget(string Id)
        {
            var sql = @"DELETE FROM [dbo].[CMS_WidgetBase]
                        WHERE [ID]=@Id
                        ";
            using (var db = DbUtility.GetInstance())
            {

                try
                {
                    var res = db.ExecuteNonQuery(sql, new { Id = Id });
                    return res == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion

        #region 组件--》加载、删除、修改

        /// <summary>
        /// 根据Id获取一个组件的信息  主要是获取WidgetId
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SysWidget GetWidgetModel(string id)
        {
            var sql = @"
                       SELECT 
                           [WidgetId]
                          ,[EnCode]
                          ,[GroupName]
                          ,[Name]
                          ,[ShortName]
                          ,[Icon]
                          ,[UrlAddress]
                          ,[SetUrlAddress]
                          ,[SaveUrlAddress]
                          ,[Thumbnail]
                          ,[StyleClass]
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
                      FROM  [dbo].[SysWidget] 
                      WHERE SysWidget.WidgetId =  (
                              SELECT WidgetId 
                              FROM CMS_WidgetBase
                              WHERE CMS_WidgetBase.ID=@Id
                        )";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModel<SysWidget>(sql, new { Id = id });
                    return res;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion




        #region 组件注册

        public bool CheckExist(SysWidget sysWidget)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                            SELECT COUNT([Name])
                            FROM [dbo].[SysWidget]
                            WHERE [WidgetId]=@WidgetId
                            ";
                try
                {
                    db.AddParameter("WidgetId", sysWidget.WidgetId);

                    return (int)db.ExecuteScalar(sql) >= 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public bool InsertData(SysWidget sysWidget)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                            INSERT INTO [dbo].[sysWidget]
			                            (WidgetId
			                            ,EnCode
			                            ,GroupName
			                            ,Name
			                            ,ShortName
			                            ,Icon
			                            ,UrlAddress
			                            ,SetUrlAddress
			                            ,SaveUrlAddress
			                            ,Thumbnail
			                            ,StyleClass
			                            ,DisplayNo
			                            ,DeleteFlag
			                            ,EnabledFlag
			                            ,Description
			                            ,CreaterTime
			                            ,CreaterUserId
			                            ,LastUpdateTime
			                            ,LastUpdateUserId
			                            ,DeleteTime
			                            ,DeleteUserId)
                                   VALUES 
			                            (@WidgetId
			                            ,@EnCode
			                            ,@GroupName
			                            ,@Name
			                            ,@ShortName
			                            ,@Icon
			                            ,@UrlAddress
			                            ,@SetUrlAddress
			                            ,@SaveUrlAddress
			                            ,@Thumbnail
			                            ,@StyleClass
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
			                            )

                            ";
                try
                {

                  
                    sysWidget.DisplayNo = db.ReaderModel<SysWidget>("SELECT MAX(DisplayNo) FROM [dbo].[sysWidget]",null).DisplayNo+1;
                    var res = db.ExecuteNonQuery(sql, sysWidget);
                    return res == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public bool ChangeWidgetInfo(SysWidget sysWidget)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                            UPDATE [dbo].[sysWidget]
		                            SET	 [WidgetId]=@WidgetId
			                            ,[EnCode]=@EnCode
			                            ,[GroupName]=@GroupName
			                            ,[Name]=@Name
			                            ,[ShortName]=@ShortName
			                            ,[Icon]=@Icon
			                            ,[UrlAddress]=@UrlAddress
			                            ,[SetUrlAddress]=@SetUrlAddress
			                            ,[SaveUrlAddress]=@SaveUrlAddress
			                            ,[Thumbnail]=@Thumbnail
			                            ,[StyleClass]=@StyleClass
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
                             WHERE       [WidgetId]=@WidgetId       
                            ";

                try
                {
                    var res = db.ExecuteNonQuery(sql, sysWidget);
                    return res == 1;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion

    }
}
