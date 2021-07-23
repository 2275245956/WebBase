using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z2.Core.WebApp.Model;

namespace Z2.Web.Areas.CMS.Common
{
    public class LayoutZonesViewModel
    {
        public CMS_Layout Layout { get; set; }
        //public PageEntity Page { get; set; }
        public string PageID { get; set; }
        public string LayoutID { get; set; }
        public IEnumerable<CMS_Zone> Zones { get; set; }
        public IEnumerable<CMS_WidgetBase> Widgets { get; set; }
        public LayoutHtmlCollection LayoutHtml { get; set; }
    }
}