using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Model
{
    public class CMS_Layout
    {
       
       
            /// <summary>
            /// 
            /// </summary>
            public string ID { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string LayoutName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ContainerClass { get; set; }

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
            public string Script { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Style { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int? DisplayNo { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool DeleteFlag { get; set; }

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
            public string ImageUrl { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ImageThumbUrl { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Theme { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public DateTime? DeleteTime { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string DeleteUserId { get; set; }

        public CMS_Zone.ZonesCollection Zones { get; set; }
        public LayoutHtmlCollection Html { get; set; }


    }

 
}

