using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class SysItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ItemCategoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SimpleSpelling { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Layers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DisplayNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnabledFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreaterTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreaterUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastUpdateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeleteUserId { get; set; }

        public string ExtendData { get; set; }
    }
}
