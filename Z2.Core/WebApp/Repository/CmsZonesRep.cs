using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class CmsZonesRep
    {
        public List<CMS_Zone> GetModel(string LayoutId)
        {
            var sql = @"
                        SELECT [ID]
                              ,[LayoutId]
                              ,[ZoneName]
                              ,[Title]
                              ,[CreateBy]
                              ,[CreatebyName]
                              ,[CreateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateByName]
                              ,[LastUpdateDate]
                              ,[Description]
                              ,[Status]
                          FROM [dbo].[CMS_Zone]
                          Where [LayoutId]=@LayoutId
                        ";
            using (var db=DbUtility.GetInstance())
            {
                try
                {

                    var data = db.ReaderModelList<CMS_Zone>(sql, new { LayoutId = LayoutId });
                    return data.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
    }
}
