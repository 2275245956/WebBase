using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.WebApp.Enums;

namespace Z2.Core.WebApp.Model
{
    public class CMS_Media
    {

        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MediaType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Status { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        public string MediaTypeImage
        {
            get
            {
                Debug.Assert(MediaType != null, nameof(MediaType) + " != null");
                return ((MediaType) MediaType).ToString();
            }
        }
    }
}
