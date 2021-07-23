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
    public class SysOfficeRep : MasterModelRep<SysOffice, string>
    {
        public override bool AddModel(SysOffice model)
        {
            throw new NotImplementedException();
        }

        public override bool EditModel(SysOffice model)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 获取所有的信息列表
        /// </summary>
        /// <param name="keyword">参数 ， 搜索关键字</param>
        /// <returns>返回一个List的集合</returns>
        public List<SysOffice> GetList(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {

                string sql = @"SELECT  [Id]
                          ,[Name]
                          ,[EnCode]
                          ,[EnabledFlag]
                          ,[Description]
                          ,[CreaterTime]
                          FROM [dbo].[SysOffice]
                          Where [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " and [Name] like @keyword";
                }
                sql += " order by  DisplayNo";
                try
                {
                    var rst = db.ReaderModelList<SysOffice>(sql, new { DeleteFlag = 0, keyword = "%" + keyword + "%" });
                    return rst.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        /// <summary>
        /// 删除 某个事业所
        /// </summary>
        /// <param name="keyValue">删除关键字  默认为Id</param>
        /// <param name="userId">删除此事业所的用户Id</param>
        /// <returns></returns>
        public bool DeleteForm(string keyValue, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysOffice] 
                    set [DeleteFlag]=1
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                try
                {
                    var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                    return bl == 1;
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }


        /// <summary>
        /// 获取某个事业所的具体信息
        /// </summary>
        /// <param name="keyValue">根据此项进行查询</param>
        /// <returns>返回一个 对象</returns>
        public SysOffice GetForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT  [Id]
                      ,[ParentId]
                      ,[EnCode]
                      ,[Name]
                      ,[SimpleSpelling]
                      ,[Layers]
                      ,[DisplayNo]
                      ,[DeleteFlag]
                      ,[EnabledFlag]
                      ,[StartDate]
                      ,[EndDate]
                      ,[WithdrawalDate]
                      ,[Description]
                      ,[CreaterTime]
                      ,[CreaterUserId]
                      ,[LastUpdateTime]
                      ,[LastUpdateUserId]
                      ,[DeleteTime]
                      ,[DeleteUserId]
                  FROM 　[dbo].[SysOffice]
                  Where [DeleteFlag]=@DeleteFlag
                  and Id=@keyValue";
                try
                {
                    var rst = db.ReaderModel<SysOffice>(sql, new { DeleteFlag = false, keyValue = keyValue });
                    return rst;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        /// <summary>
        /// 修改 或者 添加 事业所的信息 根据id的是否为空来选择是添加还是修改
        /// </summary>
        /// <param name="roleEntity">objectEntity 实体对象</param>
        /// <returns>返回是否改变  为bool</returns>
        public bool SubmitForm(SysOffice officeEntity)
        {
            //初始化部分属性值
            officeEntity.CreaterTime = DateTime.Now;
            officeEntity.LastUpdateTime = DateTime.Now;
            officeEntity.DeleteFlag = Convert.ToBoolean(0);

            officeEntity.ParentId = "";
            var sql = string.Empty;
            //如果id为空    添加信息
            if (string.IsNullOrEmpty(officeEntity.Id))
            {
                //GUID自动产生ID
                officeEntity.Id = ResultHelper.NewId();

                sql = @"
                    INSERT INTO  [dbo].[SysOffice]
                   ( 　[Id]
                      ,[ParentId]
                      ,[EnCode]
                      ,[Name]
                      ,[SimpleSpelling]
                      ,[Layers]
                      ,[DisplayNo]
                      ,[DeleteFlag]
                      ,[EnabledFlag]
                      ,[StartDate]
                      ,[EndDate]
                      ,[WithdrawalDate]
                      ,[Description]
                      ,[CreaterTime]
                      ,[CreaterUserId]
                      ,[LastUpdateTime]
                      ,[LastUpdateUserId]
                      ,[DeleteTime]
                      ,[DeleteUserId])
                    VALUES
                    (@Id
                    ,@ParentId
                    ,@EnCode
                    ,@Name
                    ,@SimpleSpelling
                    ,@Layers
                    ,@DisplayNo
                    ,@DeleteFlag
                    ,@EnabledFlag
                    ,@StartDate
                    ,@EndDate
                    ,@WithdrawalDate
                    ,@Description
                    ,@CreaterTime
                    ,@CreaterUserId
                    ,@LastUpdateTime
                    ,@LastUpdateUserId
                    ,@DeleteTime
                    ,@DeleteUserId)";
            }
            else   //否则就是修改 某个事业所的信息
            {
                sql = @"
                    update [dbo].[SysOffice] set
                      [ParentId]=@ParentId
                      ,[EnCode]=@EnCode
                      ,[Name]=@Name
                      ,[SimpleSpelling]=@SimpleSpelling
                      ,[Layers]=@Layers
                      ,[DisplayNo]=@DisplayNo
                      ,[DeleteFlag]=@DeleteFlag
                      ,[EnabledFlag]=@EnabledFlag
                      ,[StartDate]=@StartDate
                      ,[EndDate]=@EndDate
                      ,[WithdrawalDate]=@WithdrawalDate
                      ,[Description]=@Description
                      ,[CreaterTime]=@CreaterTime
                      ,[CreaterUserId]=@CreaterUserId
                      ,[LastUpdateTime]=@LastUpdateTime
                      ,[LastUpdateUserId]=@LastUpdateUserId
                      ,[DeleteTime]=@DeleteTime
                      ,[DeleteUserId] =@DeleteUserId
                     where [Id]=@Id";
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    if (officeEntity.DisplayNo < 0)
                    {
                        officeEntity.DisplayNo = db.ReaderModel<SysOffice>("select max(DisplayNo) as DisplayNo from sysOffice", null).DisplayNo + 1;
                    }
                    var effRow = db.ExecuteNonQuery(sql, officeEntity);
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
