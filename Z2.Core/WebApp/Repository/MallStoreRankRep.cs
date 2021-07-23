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
    public partial class MallStoreRankRep
    {
        private readonly MallStoreRank mdl = new MallStoreRank();

        public List<MallStoreRank> GetList(string keyWord)
        {
            var sql = string.Empty;
            using (var db = DbUtility.GetInstance())
            {
                sql = @"SELECT 
                               [Id]
                              ,[ItemCode]
                              ,[ItemName]
                              ,[SimpleSpelling]
                              ,[Avatar]
                              ,[HonestiesLower]
                              ,[HonestiesUpper]
                              ,[ProductCount]
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
                          FROM  [dbo].[mall_store_rank]
                          WHERE [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyWord))
                {
                    sql += " and [ItemName] like @keyWord";
                }

                sql += " ORDER BY [DisplayNo]";
                try
                {
                    var resList =
                        db.ReaderModelList<MallStoreRank>(sql, new { DeleteFlag = 0, keyWord = "%" + keyWord + "%" });
                    return resList.ToList();

                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        public MallStoreRank GetForm(string keyValue)
        {
            string sql = string.Empty;
            using (var db = DbUtility.GetInstance())
            {
                sql = @"SELECT
                         [Id]
                        ,[ItemCode]
                        ,[ItemName]
                        ,[SimpleSpelling]
                        ,[Avatar]
                        ,[HonestiesLower]
                        ,[HonestiesUpper]
                        ,[ProductCount]
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
                    FROM [dbo].[mall_store_rank]
                    WHERE [DeleteFlag]=@DeleteFlag
                            AND [Id]=@Id";
                try
                {
                    var res = db.ReaderModel<MallStoreRank>(sql, new { DeleteFlag = 0, Id = keyValue });
                    return res;
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
                    update [dbo].[mall_store_rank] 
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


        public bool SubmitForm(MallStoreRank mallStoreRank)
        {
            var sql = string.Empty;
            //初始化数据
            mallStoreRank.LastUpdateUserId = "admin";
            mallStoreRank.LastUpdateTime = DateTime.Now;
            //根据ID来判断是修改还是增加
            if (string.IsNullOrEmpty(mallStoreRank.Id))//id为空  增加
            {
                mallStoreRank.Id = ResultHelper.NewId();
                mallStoreRank.HonestiesLower = 0;
                mallStoreRank.HonestiesUpper = 100;
                mallStoreRank.ProductCount = 100;
                mallStoreRank.DeleteFlag = false;
                mallStoreRank.CreaterUserId = "admin";
                mallStoreRank.CreaterTime = DateTime.Now;
                sql = @"insert into mall_store_rank(
                                     [Id]
                                    ,[ItemCode]
                                    ,[ItemName]
                                    ,[SimpleSpelling]
                                    ,[Avatar]
                                    ,[HonestiesLower]
                                    ,[HonestiesUpper]
                                    ,[ProductCount]
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
                                  ,@Avatar 
                                  ,@HonestiesLower 
                                  ,@HonestiesUpper 
                                  ,@ProductCount 
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
                sql = @"UPDATE  mall_store_rank  SET
                                [ItemCode]=@ItemCode
                                ,[ItemName]=@ItemName
                                ,[SimpleSpelling]=@SimpleSpelling
                                ,[Avatar]=@Avatar
                                ,[HonestiesLower]=@HonestiesLower
                                ,[HonestiesUpper]=@HonestiesUpper
                                ,[ProductCount]=@ProductCount
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
                    var effRow = db.ExecuteNonQuery(sql, mallStoreRank);
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
