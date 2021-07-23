using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class CMS_Page
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReferencePageID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPublishedPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsHomePage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayoutId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? DisplayOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPublish { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MetaKeyWorlds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MetaDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Script { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Style { get; set; }
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
        public bool IsStaticCache { get; set; }

    }
}
