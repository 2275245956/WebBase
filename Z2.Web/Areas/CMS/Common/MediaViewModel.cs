using System.Collections.Generic;
using Z2.Core.WebApp.Model;

namespace Z2.Web.Areas.CMS.Common
{
    public class MediaViewModel
    {
        public string ParentId { get; set; }
        public List<CMS_Media> Parents { get; set; }
        public CMS_Media Parent { get; set; }
        public IEnumerable<CMS_Media> Medias { get; set; }
        public CmsPagination Pagin { get; set; }

    }
}
