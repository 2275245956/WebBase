using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class SysWidgetUser
    {
        public string Id { get; set; }
        public string WidgetId { get; set; }
        public string UserId { get; set; }
        public int? Row { get; set; }
        public int? Col { get; set; }
        public int? ColWidth { get; set; }
        public int? Height { get; set; }
       
    }
}
