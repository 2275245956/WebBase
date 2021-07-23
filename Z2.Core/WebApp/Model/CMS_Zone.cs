using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class CMS_Zone
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayoutId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZoneName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        public const string ZoneTag = "<zone>";
        public const string ZoneEndTag = "</zone>";
        public class ZonesCollection : Collection<CMS_Zone>
        {

        }
    }
}
