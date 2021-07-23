using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class CMS_LayoutHtml
    {
        /// <summary>
        /// 
        /// </summary>
        public int? LayoutHtmlId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayoutId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreatebyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastUpdateBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastUpdateByName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

    }


    public class LayoutHtmlCollection : Collection<CMS_LayoutHtml>
    {

    }


}
