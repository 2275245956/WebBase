using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{

[ModuleAction(ModuleId = "B1E5DC17-B941-4C50-9C10-A381B87F3A42", ModuleName = "角色类型", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 12)]
    public class RoleTypeController : DicBaseController 
        // GET: DicMng/RoleType
    {
        public override string CategoryCode => "RoleType";
    }
}