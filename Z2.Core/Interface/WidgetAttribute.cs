using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Interface
{
    public class WidgetAttribute : Attribute
    {
        public string WidgetKey { get; set; }
        public string WidgetTitle { get; set; }
        public string WidgetIcon { get; set; }

        public string WidgetArea { get; set; }
    }
}
