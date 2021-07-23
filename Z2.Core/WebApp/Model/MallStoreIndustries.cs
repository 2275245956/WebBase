using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class MallStoreIndustries
    {
        //[Id]
        public string Id { get; set; }
        //,[ItemCode]
        public string ItemCode { get; set; }
        //,[ItemName]
        public string ItemName { get; set; }
        //,[SimpleSpelling]
        public string SimpleSpelling { get; set; }
        //,[DisplayNo]
        public int?  DisplayNo { get; set; }
        //,[DeleteFlag]
        public bool DeleteFlag { get; set; }
        //,[EnabledFlag]
        public bool EnabledFlag { get; set; }
       
        public string Description { get; set; }
     
        public DateTime? CreaterTime { get; set; }
        public string CreaterUserId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
       

    }
}
