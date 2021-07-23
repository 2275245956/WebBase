using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class CmsMediaRep
    {
        #region 获取所有的媒体图片等=========
        public List<CMS_Media> GetAllList(string parentId)
        {
            var sql = @"
                       SELECT
                           [ID]
                          ,[ParentID]
                          ,[Title]
                          ,[MediaType]
                          ,[Url]
                          ,[Status]
                          ,[CreateBy]
                          ,[CreatebyName]
                          ,[CreateDate]
                          ,[LastUpdateBy]
                          ,[LastUpdateByName]
                          ,[LastUpdateDate]
                          ,[Description]
                      FROM [dbo].[CMS_Media]
                      WHERE  [ParentID]=@ParentID
                      ORDER BY [CreateDate]
                    ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModelList<CMS_Media>(sql, new { ParentID = parentId });
                    return res.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion

        public CMS_Media GetModel(string parentId)
        {
            var sql = @"
                       SELECT
                           [ID]
                          ,[ParentID]
                          ,[Title]
                          ,[MediaType]
                          ,[Url]
                          ,[Status]
                          ,[CreateBy]
                          ,[CreatebyName]
                          ,[CreateDate]
                          ,[LastUpdateBy]
                          ,[LastUpdateByName]
                          ,[LastUpdateDate]
                          ,[Description]
                      FROM [dbo].[CMS_Media]
                      WHERE  [ParentID]=@ParentID
                      ORDER BY [CreateDate]
                    ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModel<CMS_Media>(sql, new { ParentID = parentId });
                    return res;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
