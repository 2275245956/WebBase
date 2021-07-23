using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public  partial class SysRoleAuthorizeOperate
    {
        public string  Id { get; set; }
        public string  AuthorizeId { get; set; }
        public string Icon { get; set; }
        public string KeyCode { get; set; }
        public bool IsValid { get; set; }
        public string Name { get; set; }
    }
}
