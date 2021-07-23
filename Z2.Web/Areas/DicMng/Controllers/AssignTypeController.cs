using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    [ModuleAction(ModuleId = "21AED6C6-87A8-486C-A9CD-195F432F9EE9", ModuleName ="指派类型",ParentModuleId = "A824B5F6-2974-41DD-8887-4CA1C9C683675", DisplayNo = 2)]
    public class AssignTypeController : DicBaseController
    {
        // GET: DicMng/AssignType
        public override string CategoryCode => "AssignType";
    }
}