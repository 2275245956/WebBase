using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public partial class WxMpRep
    {
        private readonly WxMp _wxMpEntity = new WxMp();

        /// <summary>
        /// 获取所有的信息列表
        /// </summary>
        /// <param name="keyword">参数 ， 搜索关键字</param>
        /// <returns>返回一个List的集合</returns>
        public List<WxMp> GetList(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {

                var sql = @"SELECT   [wid]
                                  ,[wxName]
                                  ,[wxId]
                                  ,[yixinId]
                                  ,[weixinCode]
                                  ,[wxPwd]
                                  ,[avatar]
                                  ,[apiurl]
                                  ,[wxToken]
                                  ,[wxProvince]
                                  ,[wxCity]
                                  ,[AppId]
                                  ,[AppSecret]
                                  ,[Access_Token]
                                  ,[openIdStr]
                                  ,[createDate]
                                  ,[endDate]
                                  ,[wenziMaxNum]
                                  ,[tuwenMaxNum]
                                  ,[yuyinMaxNum]
                                  ,[wenziDefineNum]
                                  ,[tuwenDefineNum]
                                  ,[yuyinDefineNum]
                                  ,[requestTTNum]
                                  ,[requestUsedNum]
                                  ,[smsTTNum]
                                  ,[smsUsedNum]
                                  ,[DeleteFlag]
                                  ,[DeleteDate]
                                  ,[wxType]
                                  ,[remark]
                                  ,[displayNo]
                              FROM [dbo].[wx_mp]
                              Where [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " and [wxName] like @keyword";
                }
                sql += " order by  displayNo";
                try
                {
                    var rst = db.ReaderModelList<WxMp>(sql, new {DeleteFlag = 0, keyword = "%" + keyword + "%"});
                    return rst.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
                
            }
        }

        public bool SubmitForm(WxMp wxMp)
        {

            var sql = string.Empty;
            if (string.IsNullOrEmpty(wxMp.Wid))
            {
                wxMp.CreateDate = DateTime.Now;
                wxMp.EndDate = wxMp.CreateDate.Value.AddYears(1);
                sql = @"
                        INSERT INTO [dbo].[wx_mp](
                               wid
                              ,wxName
                              ,wxId
                              ,yixinId
                              ,weixinCode
                              ,wxPwd
                              ,avatar
                              ,apiurl
                              ,wxToken
                              ,wxProvince
                              ,wxCity
                              ,AppId
                              ,AppSecret
                              ,Access_Token
                              ,openIdStr
                              ,createDate
                              ,endDate
                              ,wenziMaxNum
                              ,tuwenMaxNum
                              ,yuyinMaxNum
                              ,wenziDefineNum
                              ,tuwenDefineNum
                              ,yuyinDefineNum
                              ,requestTTNum
                              ,requestUsedNum
                              ,smsTTNum
                              ,smsUsedNum
                              ,deleteFlag
                              ,deleteDate
                              ,wxType
                              ,remark
                              ,displayNo)
                          Values(
                                    NEWID()
                                  ,@WxName
                                  ,@WxId
                                  ,@YiXinId
                                  ,@WeiXinCode
                                  ,@WxPwd
                                  ,@Avatar
                                  ,@ApiUrl
                                  ,@WxToken
                                  ,@WxProvince
                                  ,@WxCity
                                  ,@AppId
                                  ,@AppSecret
                                  ,@Access_Token
                                  ,@OpenIdStr
                                  ,@CreateDate
                                  ,@EndDate
                                  ,@WenZiMaxNum
                                  ,@TuWenMaxNum
                                  ,@YuYinMaxNum
                                  ,@WenZiDefineNum
                                  ,@TuWenDefineNum
                                  ,@YuYinDefineNum
                                  ,@RequestTtNum
                                  ,@RequestUsedNum
                                  ,@SmsTtNum
                                  ,@SmsUsedNum
                                  ,@DeleteFlag
                                  ,@DeleteDate
                                  ,@WxType
                                  ,@Remark
                                  ,@DisplayNo
                                )

                            ";
            }
            else
            {
              
                sql = @"
                        UPDATE  [dbo].[wx_mp] 
                        SET  
                            wid            =@Wid
                           ,wxName         =@WxName      
                           ,wxId           =@WxId 
                           ,yixinId        =@YiXinId  
                           ,weixinCode     =@WeiXinCode       
                           ,wxPwd          =@WxPwd   
                           ,avatar         =@Avatar
                           ,apiurl         =@ApiUrl
                           ,wxToken        =@WxToken
                           ,wxProvince     =@WxProvince
                           ,wxCity         =@WxCity
                           ,AppId          =@AppId
                           ,AppSecret      =@AppSecret
                           ,Access_Token   =@Access_Token
                           ,openIdStr      =@OpenIdStr
                           ,wenziMaxNum    =@WenZiMaxNum
                           ,tuwenMaxNum    =@TuWenMaxNum
                           ,yuyinMaxNum    =@YuYinMaxNum
                           ,wenziDefineNum =@WenZiDefineNum
                           ,tuwenDefineNum =@TuWenDefineNum
                           ,yuyinDefineNum =@YuYinDefineNum
                           ,requestTTNum   =@RequestTtNum
                           ,requestUsedNum =@RequestUsedNum
                           ,smsTTNum       =@SmsTtNum
                           ,smsUsedNum     =@SmsUsedNum
                           ,deleteFlag     =@DeleteFlag
                           ,deleteDate     =@DeleteDate
                           ,wxType         =@WxType
                           ,remark         =@Remark
                           ,displayNo      =@DisplayNo
                         
                    Where wid=@Wid
                    ";
            }

            using (var db = DbUtility.GetInstance())
            {
                try
                {

                    var effRow = db.ExecuteNonQuery(sql, wxMp);
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }

        public bool Delete(string keyValue)
        {
            var sql = @"Update [dbo].[wx_mp] 
                        Set deleteFlag =@DeleteFlag
                           ,deleteDate =@DeleteDate 
                        Where Wid=@Wid  
                      ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {

                    var effRow = db.ExecuteNonQuery(sql, new { DeleteFlag = 1, DeleteDate = DateTime.Now, Wid = keyValue });
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw e;
                }
            }

        }


        public WxMp GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {

                var sql = @"SELECT   [wid]
                                  ,[wxName]
                                  ,[wxId]
                                  ,[yixinId]
                                  ,[weixinCode]
                                  ,[wxPwd]
                                  ,[avatar]
                                  ,[apiurl]
                                  ,[wxToken]
                                  ,[wxProvince]
                                  ,[wxCity]
                                  ,[AppId]
                                  ,[AppSecret]
                                  ,[Access_Token]
                                  ,[openIdStr]
                                  ,[createDate]
                                  ,[endDate]
                                  ,[wenziMaxNum]
                                  ,[tuwenMaxNum]
                                  ,[yuyinMaxNum]
                                  ,[wenziDefineNum]
                                  ,[tuwenDefineNum]
                                  ,[yuyinDefineNum]
                                  ,[requestTTNum]
                                  ,[requestUsedNum]
                                  ,[smsTTNum]
                                  ,[smsUsedNum]
                                  ,[DeleteFlag]
                                  ,[DeleteDate]
                                  ,[wxType]
                                  ,[remark]
                                  ,[displayNo]
                              FROM [dbo].[wx_mp]
                              Where [DeleteFlag]=@DeleteFlag
                              AND  Wid=@Wid    
                            ";
                try
                {
                    var rst = db.ReaderModel<WxMp>(sql, new { DeleteFlag = 0, Wid = keyValue });
                    return rst;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
