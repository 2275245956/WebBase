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
    public class Mall_BrandRep
    {
        public List<Mall_brand> GetList(string keyword)
        {
            using (var DB = DbUtility.GetInstance())
            {
                var sql = @"SELECT Id,
                            ItemCode,
                            ItemName,
                            SimpleSpelling,
                            Logo, 
                            DisplayNo,
                            DeleteFlag,
                            EnabledFlag,
                            Description,
                            CreaterTime,
                            CreaterUserId,
                            LastUpdateTime,
                            LastUpdateUserId,
                            DeleteTime,
                            DeleteUserId,
                            LogoPath
                            from mall_brand Where [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and [ItemName] like @keyword";
                }

                sql = sql + @" order by [DisplayNo]";
                try
                {
                    
                    var rst = DB.ReaderModelList<Mall_brand>(sql, new { DeleteFlag = 1, keyword = "%" + keyword + "%"});
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }

        }
        public Mall_brand GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {

                var sql = @"  SELECT Id,
                            ItemCode,
                            ItemName,
                            SimpleSpelling,
                            Logo,
                            DisplayNo,
                            DeleteFlag,
                            EnabledFlag,
                            Description,
                            CreaterTime,
                            CreaterUserId,
                            LastUpdateTime,
                            LastUpdateUserId,
                            DeleteTime,
                            DeleteUserId,
                            LogoPath
                            from mall_brand Where [DeleteFlag]=@DeleteFlag
                            and Id=@keyValue";
                    var rst = db.ReaderModel<Mall_brand>(sql, new { DeleteFlag = true, keyValue = keyValue });
                    return rst;
            }
        }

        public   List<Mall_brand>  GetHomePage()
        {
            using (var db = DbUtility.GetInstance())
            {

                var sql = @"  SELECT Id,
                            ItemCode,
                            ItemName,
                            SimpleSpelling,
                            Logo,
                            DisplayNo,
                            DeleteFlag,
                            EnabledFlag,
                            Description,
                            CreaterTime,
                            CreaterUserId,
                            LastUpdateTime,
                            LastUpdateUserId,
                            DeleteTime,
                            DeleteUserId,
                            LogoPath
                            from mall_brand Where [DeleteFlag]=@DeleteFlag";

                try
                {
                    var rst = db.ReaderModelList<Mall_brand>(sql, new { DeleteFlag = true });
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw e;
                }
               
            }
            }

        public bool DeleteForm(string keyValue, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[mall_brand] 
                    set [DeleteFlag]=0
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return bl == 1;
            }
        }

        public bool UpdateMallData(Mall_brand mallbrand)
        {
            mallbrand.Createruserid = "admin";
            mallbrand.Lastupdateuserid = "admin";
            mallbrand.Lastupdatetime = DateTime.Now;
            using (var db = DbUtility.GetInstance())
            {
                var  sql = @"
                    update mall_brand set
                     ItemCode=@ItemCode
                     ,ItemName=@ItemName
                     ,SimpleSpelling=@SimpleSpelling
                     ,Logo=@Logo
                     ,DisplayNo=@DisplayNo
                     ,EnabledFlag=@EnabledFlag
                     ,Description=@Description
                     ,CreaterUserId=@CreaterUserId
                    ,LogoPath=@LogoPath
                    ,LastUpdateTime=@LastUpdateTime
                    ,LastUpdateUserId=@LastUpdateUserId
                      where [Id]=@Id";
                try
                {
                    var bl = db.ExecuteNonQuery(sql, mallbrand);
                    return bl == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        public bool SubmitForm(Mall_brand mallbrand)
        {
            var sql = string.Empty;
            if (string.IsNullOrEmpty(mallbrand.Id))
            {
                mallbrand.Id = ResultHelper.NewId();
                sql = @"
                    insert into mall_brand
                    (Id,
                     ItemCode,
                     ItemName,
                     SimpleSpelling,
                     Logo,
                     DisplayNo,
                     DeleteFlag,
                     EnabledFlag,
                     Description,
                     CreaterTime,
                     CreaterUserId,
                            LogoPath)
                    values
                    (
                     @Id
                    ,@ItemCode
                    ,@ItemName
                    ,@SimpleSpelling
                    ,@Logo
                    ,(select max(DisplayNo) from mall_brand)+1
                    ,@DeleteFlag
                    ,@EnabledFlag
                    ,@Description
                    ,@CreaterTime
                    ,@CreaterUserId
                            ,@LogoPath)";
                 //
            }
            else
            {

                //
                sql = @"
                    update mall_brand set
                     ItemCode=@ItemCode
                     ,ItemName=@ItemName
                     ,SimpleSpelling=@SimpleSpelling
                     ,Logo=@Logo
                     ,DisplayNo=@DisplayNo
                     ,EnabledFlag=@EnabledFlag
                     ,Description=@Description
                     ,CreaterUserId=@CreaterUserId
                    ,LogoPath=@LogoPath
                      where [Id]=@Id";
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, mallbrand);
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}
