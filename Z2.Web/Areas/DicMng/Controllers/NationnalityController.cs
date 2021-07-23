using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    [ModuleAction(ModuleId = "C4272F8B-93A9-49D7-AEBA-D8D6F63C84B6", ModuleName = "民族", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 10)]
    public class NationnalityController : DicBaseController
    {
        // GET: DicMng/Nationnality
        public override string CategoryCode => "103";

    }
}