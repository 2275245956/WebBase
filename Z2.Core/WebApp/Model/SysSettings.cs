using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class SysSettings
    {
        public string SettingID { get; set; }
        public string SettingKey { get; set; }
        public string OfficeID { get; set; }
        public string UserID { get; set; }
        public string SettingName { get; set; }
        public string SettingNote { get; set; }
        public string SettingValue { get; set; }
        public int? DisplayNo { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }


    }
}
