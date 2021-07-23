using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class CMS_ThemeRep
    {
        public CMS_Theme GetTheme(string Id)
        {
            var sql = @"
                            select  [CMS_Theme].* 
                            from CMS_layout,[CMS_Theme] 
                            where [CMS_Theme].Id=CMS_layout.Theme
                            and  CMS_layout.Id=@ID";
            using (var db=DbUtility.GetInstance() )
            {
                try
                {
                    var data = db.ReaderModel<CMS_Theme>(sql, new { ID = Id });
                    return data;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
