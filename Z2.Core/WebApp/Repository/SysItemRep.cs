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
    public class SysItemRep
    {

        private SysItem sysItem = new SysItem();
        public SysItemCategory GetCategory(string code)
        {
            var sql = @"
            SELECT [Id]
                  ,[CategoryCode]
                  ,[CategoryName]
                  ,[IsTree]
                  ,[DeleteFlag]
                  ,[EnabledFlag]
              FROM [dbo].[SysItemCategory]
              Where [DeleteFlag]=0 and [EnabledFlag]=1 and [CategoryCode]=@CategoryCode
            ";

            using (var conn = DbUtility.GetInstance())
            {
                return conn.ReaderModel<SysItemCategory>(sql, new { CategoryCode = code });
            }
        }


        public List<SysItem> GetItemListByItemCategoryId(string ItemCategoryId, string keyword)
        {
            var sql = string.Empty;
            if (string.IsNullOrEmpty(keyword))
            {
                sql = @"SELECT [Id]
                          ,[ItemCategoryId]
                          ,[ParentId]
                          ,[ItemCode]
                          ,[ItemName]
                          ,[SimpleSpelling]
                          ,[IsDefault]
                          ,[Layers]
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
                      FROM [dbo].[SysItem]
                      where ItemCategoryId = @itemcategoryId
                      and   [DeleteFlag]=0
                      order by [DisplayNo]          
                    ";
            }
            else
            {
                sql = @"
	                with stru1 as (
	                select [SysItem].Id,[SysItem].ItemName,[SysItem].[ItemCode],[SysItem].ParentId,[SysItem].ItemCategoryId,10 as Lv,[SysItem].Description,[SysItem].[DisplayNo] from [dbo].[SysItem] where ItemCategoryId=@itemcategoryId and [ItemName]  like @keyword and DeleteFlag=0
	                union all
	                select SysItem.Id,SysItem.ItemName,[SysItem].[ItemCode],SysItem.ParentId,SysItem.ItemCategoryId,stru1.Lv-1 as Lv,[SysItem].Description,[SysItem].[DisplayNo] from [dbo].[SysItem] SysItem   
	                inner join stru1 on stru1.ParentId=SysItem.Id
	                where SysItem.ItemCategoryId=@itemcategoryId and SysItem.DeleteFlag=0
	                ),	
	                stru2 as (
	                select [SysItem].Id,[SysItem].ItemName,[SysItem].[ItemCode],[SysItem].ParentId,[SysItem].ItemCategoryId,10 as Lv,[SysItem].Description,[SysItem].[DisplayNo]  from [dbo].[SysItem] where ItemCategoryId=@itemcategoryId and [ItemName] like @keyword and DeleteFlag=0
	                union all
	                select SysItem.Id,SysItem.ItemName,[SysItem].[ItemCode],SysItem.ParentId,SysItem.ItemCategoryId,stru2.Lv+1 as Lv,[SysItem].Description,[SysItem].[DisplayNo]  from [dbo].[SysItem] SysItem  
	                inner join stru2 on stru2.Id=SysItem.ParentId
	                where SysItem.ItemCategoryId=@itemcategoryId and SysItem.DeleteFlag=0
	                )
	                select * from (
	                select distinct * from stru1
	                -- union
	                -- select distinct * from stru2
	                ) stru
	                order by Lv
                ";
            }
            using (var conn = DbUtility.GetInstance())
            {
                return conn.ReaderModelList<SysItem>(sql, new { itemcategoryId = ItemCategoryId, keyword = "%" + keyword + "%" }).ToList();
            }
        }

        public List<SysItem> GetItems(string categoryCode,string keyword)
        {
            var sql = @"SELECT 
               SysItem.[Id]
              ,SysItem.[ItemCategoryId]
              ,SysItem.[ParentId]
              ,SysItem.[ItemCode]
              ,SysItem.[ItemName]
              ,SysItem.[SimpleSpelling]
              ,SysItem.[IsDefault]
              ,SysItem.[Layers]
              ,SysItem.[DisplayNo]
              ,SysItem.[DeleteFlag]
              ,SysItem.[EnabledFlag]
              ,SysItem.[Description]
              ,SysItem.[CreaterTime]
              ,SysItem.[CreaterUserId]
              ,SysItem.[LastUpdateTime]
              ,SysItem.[LastUpdateUserId]
              ,SysItem.[DeleteTime]
              ,SysItem.[DeleteUserId]
          FROM [dbo].[SysItem] as SysItem
          Inner Join [dbo].[SysItemCategory] itemCategory on itemCategory.Id=SysItem.ItemCategoryId
          Where SysItem.[DeleteFlag]=0 and SysItem.[EnabledFlag]=1 and itemCategory.CategoryCode=@CategoryCode
          
            ";
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " and [ItemName] like @keyword";
            }
            sql += "  order by SysItem.[DisplayNo]";
            using (var conn = DbUtility.GetInstance())
            {
                return conn.ReaderModelList<SysItem>(sql, new { CategoryCode = categoryCode,keyword="%"+keyword+"%" }).ToList();
            }
        }

        public SysItem GetItemsByCategoryCodeAndKeyValue(string categoryCode, string keyValue)
        {
            var sql = @"SELECT 
               SysItem.[Id]
              ,SysItem.[ItemCategoryId]
              ,SysItem.[ParentId]
              ,SysItem.[ItemCode]
              ,SysItem.[ItemName]
              ,SysItem.[SimpleSpelling]
              ,SysItem.[IsDefault]
              ,SysItem.[Layers]
              ,SysItem.[DisplayNo]
              ,SysItem.[DeleteFlag]
              ,SysItem.[EnabledFlag]
              ,SysItem.[Description]
              ,SysItem.[CreaterTime]
              ,SysItem.[CreaterUserId]
              ,SysItem.[LastUpdateTime]
              ,SysItem.[LastUpdateUserId]
              ,SysItem.[ExtendData]
              ,SysItem.[DeleteTime]
              ,SysItem.[DeleteUserId]
              FROM [dbo].[SysItem] as SysItem
              Inner Join [dbo].[SysItemCategory] itemCategory on itemCategory.Id=SysItem.ItemCategoryId
              Where SysItem.[DeleteFlag]=0 
              and SysItem.[EnabledFlag]=1 
              and itemCategory.CategoryCode=@CategoryCode
              and SysItem.Id=@Id
              order by SysItem.[DisplayNo]";

            using (var db = DbUtility.GetInstance())
            {
                return db.ReaderModel<SysItem>(sql, new { CategoryCode = categoryCode, Id = keyValue });
            }
        }

        public bool SubmitForm(SysItem sysItem)
        {
            using (var db = DbUtility.GetInstance())
            {
                //初始化数据信息
                sysItem.CreaterTime = DateTime.Now;
                sysItem.LastUpdateTime = DateTime.Now;
                string sql = string.Empty;
                if (string.IsNullOrEmpty(sysItem.Id))
                {
                    sysItem.DeleteFlag = false;
                    sysItem.Id = ResultHelper.NewId();
                    sql = @"insert into SysItem(
	                                              [Id]
                                                  ,[ItemCategoryId]
                                                  ,[ParentId]
                                                  ,[ItemCode]
                                                  ,[ItemName]
                                                  ,[SimpleSpelling]
                                                  ,[IsDefault]
                                                  ,[ExtendData]
                                                  ,[Layers]
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

                                              values(
	                                                @Id
                                                  , @ItemCategoryId
                                                  , @ParentId
                                                  , @ItemCode
                                                  , @ItemName
                                                  , @SimpleSpelling
                                                  , @IsDefault
                                                  , @ExtendData
                                                  , @Layers
                                                  , @DisplayNo
                                                  , @DeleteFlag
                                                  , @EnabledFlag
                                                  , @Description
                                                  , @CreaterTime
                                                  , @CreaterUserId
                                                  , @LastUpdateTime
                                                  , @LastUpdateUserId
                                                  , @DeleteTime
                                                  , @DeleteUserId
	                                          )";

                }
                else
                {
                    sql = @"update SysItem set
	                                           [Id]=@Id
                                              ,[ItemCategoryId]=@ItemCategoryId
                                              ,[ParentId]=@ParentId
                                              ,[ItemCode]=@ItemCode
                                              ,[ItemName]=@ItemName
                                              ,[SimpleSpelling]=@SimpleSpelling
                                              ,[IsDefault]=@IsDefault
                                              ,[ExtendData]=@ExtendData
                                              ,[Layers]=@Layers
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
                                                where Id=@Id                                        
                                        ";
                }

                try
                {
                    var date = db.ExecuteNonQuery(sql, sysItem);
                    return date == 1;
                }
                catch (Exception)
                {

                    throw;
                }

            }


        }
        /// <summary>
        /// 删除字典内容
        /// </summary>
        /// <param name="keyValue">Id删除关键字</param>
        /// <param name="userId">删除用户Id 默认admin</param>
        /// <returns></returns>
        public bool DeleteInfoByCategoryCode(string keyValue, string userId)
        {

            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysItem] 
                    set [DeleteFlag]=1
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return bl == 1;
            }

        }


    }
}
