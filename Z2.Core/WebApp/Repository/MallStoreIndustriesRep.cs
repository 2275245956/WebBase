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
    public partial class MallStoreIndustriesRep
    {
        private readonly MallStoreIndustries mdl = new MallStoreIndustries();

        public List<MallStoreIndustries> GetList(string keyWord)
        {
            var sql = string.Empty;
            using (var db = DbUtility.GetInstance())
            {
                sql = @"SELECT 
                               [Id]
                              ,[ItemCode]
                              ,[ItemName]
                              ,[SimpleSpelling]
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
                          FROM  [dbo].[mall_store_industries]
                          WHERE [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyWord))
                {
                    sql += " and [ItemName] like @keyWord";
                }

                sql += " ORDER BY [DisplayNo]";
                try
                {
                    var resList =
                        db.ReaderModelList<MallStoreIndustries>(sql, new { DeleteFlag = 0, keyWord = "%" + keyWord + "%" });
                    return resList.ToList();

                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        public MallStoreIndustries GetForm(string keyValue)
        {
            string sql = string.Empty;
            using (var db = DbUtility.GetInstance())
            {
                sql = @"SELECT 
                               [Id]
                              ,[ItemCode]
                              ,[ItemName]
                              ,[SimpleSpelling]
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
                          FROM  [dbo].[mall_store_industries]
                          WHERE [DeleteFlag]=@DeleteFlag
                          AND [Id]=@Id";
                try
                {
                    var res = db.ReaderModel<MallStoreIndustries>(sql, new { DeleteFlag = 0, Id = keyValue });
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
                    update [dbo].[mall_store_industries] 
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


        public bool SubmitForm(MallStoreIndustries mallStoreIndustries)
        {
            var sql = string.Empty;
            //初始化数据
            mallStoreIndustries.LastUpdateUserId = "admin";
            mallStoreIndustries.LastUpdateTime = DateTime.Now;
            //根据ID来判断是修改还是增加
            if (string.IsNullOrEmpty(mallStoreIndustries.Id))//id为空  增加
            {
                mallStoreIndustries.Id = ResultHelper.NewId();

                mallStoreIndustries.DeleteFlag = false;
                mallStoreIndustries.CreaterUserId = "admin";
                mallStoreIndustries.CreaterTime = DateTime.Now;
                sql = @"insert into [mall_store_industries](
                                       [Id]
                                      ,[ItemCode]
                                      ,[ItemName]
                                      ,[SimpleSpelling]
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
                sql = @"UPDATE  mall_store_industries  SET
                                [ItemCode]=@ItemCode
                                ,[ItemName]=@ItemName
                                ,[SimpleSpelling]=@SimpleSpelling
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
                    var effRow = db.ExecuteNonQuery(sql, mallStoreIndustries);
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
