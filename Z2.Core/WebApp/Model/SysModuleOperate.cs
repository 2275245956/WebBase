using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class SysModuleOperate
    {
        public string Id { get; set; }
        public string AuthId { get; set; }
        /* 2018/11/28 范文强  添加属性 */
        public string Icon { get; set; }
        public string AuthorizeId { get; set; }
        public string Name { get; set; }
        public string KeyCode { get; set; }
        public string ModuleId { get; set; }
        public byte IsVaild { get; set; }
        public int Sort { get; set; }

        public string ModuleName { get; set; }
    }
}
