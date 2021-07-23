using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class PMProjectManagerModel
    {
        public string ProjectId { get; set; }
        [DisplayName("编码")]
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        [DisplayName("工时计算")]
        public bool ActualWork { get; set; }
        public int DisplayNo { get; set; }
        public bool DeleteFlag { get; set; }
        public bool EnabledFlag { get; set; }
        public string Description { get; set; }
        public DateTime CreaterTime { get; set; }
        public string CreaterUserId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }
        public DateTime DeleteTime { get; set; }
        public string DeleteUserId { get; set; }

    }
}
