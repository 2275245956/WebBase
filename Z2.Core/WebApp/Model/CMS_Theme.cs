using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class CMS_Theme
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UrlDebugger { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsActived { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
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
}
