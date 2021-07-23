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
    public partial class MallStoreRep
    {

        public List<MallStore> GetList(string keyword)
        {
            using (var DB = DbUtility.GetInstance())
            {
                var sql = @"SELECT [Id]
                                  ,[ItemCode]
                                  ,[ItemName]
                                  ,[SimpleSpelling]
                                  ,[Logo]
                                  ,[State]
                                  ,[RegionId]
                                  ,[StoreRankId]
                                  ,[StoreIndustriesId]
                                  ,[Mobile]
                                  ,[Phone]
                                  ,[QQ]
                                  ,[WX]
                                  ,[Depoint]
                                  ,[Sepoint]
                                  ,[Shpoint]
                                  ,[Honesties]
                                  ,[StateEndTime]
                                  ,[Theme]
                                  ,[Banner]
                                  ,[Announcement]
                                  ,[GpsX]
                                  ,[GpsY]
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
                              FROM [dbo].[mall_store]
                              WHERE [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and [ItemName] like @keyword";
                }

                sql = sql + @" order by [DisplayNo]";
                try
                {

                    var rst = DB.ReaderModelList<MallStore>(sql, new { DeleteFlag = 0, keyword = "%" + keyword + "%" });
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }

        }
        public MallStore GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {

                var sql = @"SELECT [Id]
                                  ,[ItemCode]
                                  ,[ItemName]
                                  ,[SimpleSpelling]
                                  ,[Logo]
                                  ,[State]
                                  ,[RegionId]
                                  ,[StoreRankId]
                                  ,[StoreIndustriesId]
                                  ,[Mobile]
                                  ,[Phone]
                                  ,[QQ]
                                  ,[WX]
                                  ,[Depoint]
                                  ,[Sepoint]
                                  ,[Shpoint]
                                  ,[Honesties]
                                  ,[StateEndTime]
                                  ,[Theme]
                                  ,[Banner]
                                  ,[Announcement]
                                  ,[GpsX]
                                  ,[GpsY]
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
                              FROM [dbo].[mall_store]
                              WHERE [DeleteFlag]=@DeleteFlag
                              AND Id =@keyValue";
                try
                {
                    var rst = db.ReaderModel<MallStore>(sql, new { DeleteFlag = 0, keyValue = keyValue });
                    return rst;
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
                    update [dbo].[mall_store] 
                    set [DeleteFlag]=1
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                try
                {
                    var res = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                    return res == 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public bool SubmitForm(MallStore mallStore)
        {
            var sql = string.Empty;
            //初始化数据
            mallStore.DeleteFlag = false;
            mallStore.CreaterUserId = "admin";
            mallStore.CreaterTime = DateTime.Now;
            mallStore.LastUpdateUserId = "admin";
            mallStore.LastUpdateTime = DateTime.Now;
            //根据ID来判断是修改还是增加
            if (string.IsNullOrEmpty(mallStore.Id))//id为空  增加
            {
                mallStore.Id = ResultHelper.NewId();
                sql = @"insert into mall_store
                                   ([Id]
                                  ,[ItemCode]
                                  ,[ItemName]
                                  ,[SimpleSpelling]
                                  ,[Logo]
                                  ,[State]
                                  ,[RegionId]
                                  ,[StoreRankId]
                                  ,[StoreIndustriesId]
                                  ,[Mobile]
                                  ,[Phone]
                                  ,[QQ]
                                  ,[WX]
                                  ,[Depoint]
                                  ,[Sepoint]
                                  ,[Shpoint]
                                  ,[Honesties]
                                  ,[StateEndTime]
                                  ,[Theme]
                                  ,[Banner]
                                  ,[Announcement]
                                  ,[GpsX]
                                  ,[GpsY]
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
                            VALUES
                            (      @Id 
                                  ,@ItemCode 
                                  ,@ItemName 
                                  ,@SimpleSpelling 
                                  ,@Logo 
                                  ,@State 
                                  ,@RegionId 
                                  ,@StoreRankId 
                                  ,@StoreIndustriesId 
                                  ,@Mobile 
                                  ,@Phone 
                                  ,@QQ 
                                  ,@WX 
                                  ,@Depoint 
                                  ,@Sepoint 
                                  ,@Shpoint 
                                  ,@Honesties 
                                  ,@StateEndTime 
                                  ,@Theme 
                                  ,@Banner 
                                  ,@Announcement 
                                  ,@GpsX 
                                  ,@GpsY 
                                  ,@DisplayNo 
                                  ,@DeleteFlag 
                                  ,@EnabledFlag 
                                  ,@Description 
                                  ,@CreaterTime 
                                  ,@CreaterUserId 
                                  ,@LastUpdateTime 
                                  ,@LastUpdateUserId 
                                  ,@DeleteTime 
                                  ,@DeleteUserId )";
            }
            else
            {
                sql = @"UPDATE  mall_store  SET
                                [ItemCode]=@ItemCode
                                ,[ItemName]=@ItemName
                                ,[SimpleSpelling]=@SimpleSpelling
                                ,[Logo]=@Logo
                                ,[State]=@State
                                ,[RegionId]=@RegionId
                                ,[StoreRankId]=@StoreRankId
                                ,[StoreIndustriesId]=@StoreIndustriesId
                                ,[Mobile]=@Mobile
                                ,[Phone]=@Phone
                                ,[QQ]=@QQ
                                ,[WX]=@WX
                                ,[Depoint]=@Depoint
                                ,[Sepoint]=@Sepoint
                                ,[Shpoint]=@Shpoint
                                ,[Honesties]=@Honesties
                                ,[StateEndTime]=@StateEndTime
                                ,[Theme]=@Theme
                                ,[Banner]=@Banner
                                ,[Announcement]=@Announcement
                                ,[GpsX]=@GpsX
                                ,[GpsY]=@GpsY
                                ,[DisplayNo]=@DisplayNo
                                ,[DeleteFlag]=@DeleteFlag
                                ,[EnabledFlag]=@EnabledFlag
                                ,[Description]=@Description
                                ,[LastUpdateTime]=@LastUpdateTime
                                ,[LastUpdateUserId]=@LastUpdateUserId
                                ,[DeleteTime]=@DeleteTime
                                ,[DeleteUserId]=@DeleteUserId
                           WHERE [Id]=@Id";
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, mallStore);
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }


    }
}
