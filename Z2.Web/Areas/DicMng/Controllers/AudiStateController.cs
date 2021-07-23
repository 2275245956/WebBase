using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    [ModuleAction(ModuleId = "07EFDF3D-FB4A-4308-A006-D5A69E4195E3", ModuleName = "审核状态", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 3)]
    public class AudiStateController : DicBaseController
    {
        // GET: DicMng/AudiState
        public override string CategoryCode => "AudiState";
    }
}