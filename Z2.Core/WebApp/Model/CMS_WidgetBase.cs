using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.Interface;

namespace Z2.Core.WebApp.Model
{
    public class CMS_WidgetBase : IWidgetModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WidgetId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayoutId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StyleClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WidgetData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
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
        public DateTime? CreaterTime { get; set; }
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

    }
}
