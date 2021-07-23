using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Utility;
using Z2.Model.Mall;


namespace Z2.Repository.Mall
{
    public class ProductCategoryRep
    {
        /// <summary>
        /// 数据库查询所有数据
        /// </summary>
        /// <param name="keyword">搜索框参数</param>
        /// <returns></returns>
        public List<ProductCategory> GetProductsList(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT Id,
                                              ParentId,
                                              ItemCode,
                                              ItemName,
                                              SimpleSpelling,
                                              Layers,
                                              PriceRange,
                                              TagText,
                                              SubTitle,
                                              CategoryImg,
                                              Path,
                                              DisplayNo,
                                              DeleteFlag,
                                              EnabledFlag,
                                              Description,
                                              CreaterTime,
                                              CreaterUserId,
                                              LastUpdateTime,
                                              LastUpdateUserId,
                                              DeleteTime,
                                              DeleteUserId
                                        from mall_category where 1=1 and DeleteFlag = @DeleteFlag ";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and ItemName like @keyword";
                }
                sql = sql + @" order by DisplayNo";
                try
                {
                    var rst = db.ReaderModelList<ProductCategory>(sql, new { DeleteFlag = 1, keyword = "%" + keyword + "%" });
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="pc">前台发送的数据</param>
        /// <param name="keyValue">判断新增界面和修改界面</param>
        /// <returns></returns>
        public bool AddImgFileData(ProductCategory pc, string keyValue)
        {
            pc.CreaterTime = DateTime.Now;
            pc.CreaterUserId = "admin";
            pc.DeleteTime = DateTime.Now;
            pc.LastUpdateTime = DateTime.Now;
            pc.LastUpdateUserId = "admin";
            var sql = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                pc.Id = ResultHelper.NewId();//获取新的id,以判断是新增还是修改
                sql = @"insert into mall_product_category 
                                        (Id,
                                        ParentId,
                                        ItemCode,
                                        ItemName,
                                        SimpleSpelling,
                                        Layers,
                                        PriceRange,
                                        TagText,
                                        SubTitle,
                                        CategoryImg,
                                        Path,
                                        DisplayNo,
                                        DeleteFlag,
                                        EnabledFlag,
                                        Description,
                                        CreaterTime,
                                        CreaterUserId)
                               values
                                        (@Id,
                                         @ParentId
                                         ,@ItemCode
                                         ,@ItemName
                                         ,@SimpleSpelling
                                         ,@Layers
                                         ,@PriceRange
                                         ,@TagText
                                         ,@SubTitle
                                         ,@CategoryImg
                                         ,@Path
                                         ,(select max(DisplayNo) from mall_product_category)+1
                                         ,@DeleteFlag
                                         ,@EnabledFlag
                                         ,@Description
                                         ,@CreaterTime
                                         ,@CreaterUserId)";
            }
            else
            {
                sql = @"update mall_product_category  set 
                                        ParentId=@ParentId,
                                        ItemCode=@ItemCode,
                                        ItemName=@ItemName,
                                        SimpleSpelling=@SimpleSpelling,
                                        Layers=@Layers,
                                        PriceRange=@PriceRange,
                                        TagText=@TagText,
                                        SubTitle=@SubTitle,
                                        CategoryImg=@CategoryImg,
                                        Path=@Path,
                                        DisplayNo=@DisplayNo,
                                        DeleteFlag=@DeleteFlag,
                                        EnabledFlag=@EnabledFlag,
                                        Description=@Description,
                                        LastUpdateUserId=@LastUpdateUserId,
                                        LastUpdateTime=@LastUpdateTime
                                        where Id=@Id";
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    if (keyValue != "")
                    {
                        pc.Id = keyValue;
                    }
                    var effRow = db.ExecuteNonQuery(sql, pc);
                    return effRow == 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="keyValue">用于判断新增界面和详情界面</param>
        /// <returns></returns>
        public ProductCategory GetDetails(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT 
                                          Id,
                                          ParentId,
                                          ItemCode,
                                          ItemName,
                                          SimpleSpelling,
                                          PriceRange,
                                          TagText,
                                          SubTitle,
                                          CategoryImg,
                                          Path,
                                          DisplayNo,
                                          DeleteFlag,
                                          EnabledFlag,
                                          Description,
                                          CreaterTime,
                                          CreaterUserId,
                                          LastUpdateTime,
                                          LastUpdateUserId,
                                          DeleteTime,
                                          DeleteUserId
                                          from mall_product_category where DeleteFlag=@DeleteFlag and Id = @keyValue";
                var data = db.ReaderModel<ProductCategory>(sql, new { DeleteFlag = true, keyValue = keyValue });
                return data;
            }
        }
        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="keyValue">用于获取当前列的参数 keyValue=id</param>
        /// <param name="userId">用户删除的userId</param>
        /// <returns></returns>
        public bool GetDelete(string keyValue, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                                    UPDATE mall_product_category
                                     SET DeleteFlag=0,
                                            DeleteTime=@DeleteTime,
                                            DeleteUserId=@DeleteUserId
                                      WHERE Id=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return bl == 1;
            }
        }
    }
}
