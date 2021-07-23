using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class SysPage
    {
        [DisplayName("ID")]
        public int ID { get; set; }
        [DisplayName("页面ID")]
        public string PageID { get; set; }
        [DisplayName("页面名称")]
        public string PageName { get; set; }
        [DisplayName("Url")]
        public string Url { get; set; }
        [DisplayName("是否分页")]
        public byte PageDiv { get; set; }
        [DisplayName("每页显示条数")]
        public byte PagingCount { get; set; }
        [DisplayName("列名")]
        public string ColInfoName { get; set; }
        [DisplayName("列ID")]
        public string ColInfoID { get; set; }
        [DisplayName("创建人ID")]
        public string CreateUserID { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateDateTime { get; set; }
        [DisplayName("修改人ID")]
        public string UpdateUserID { get; set; }
        [DisplayName("修改时间")]
        public DateTime UpdateDateTime { get; set; }
    }
}
