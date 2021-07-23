using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{

[ModuleAction(ModuleId = "D665A627-75A3-4028-9800-32EB9232CF93", ModuleName = "婚姻", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 9)]
    public class MarriageController : DicBaseController
    {
        // GET: DicMng/Marriage
        public override string CategoryCode => "101";
    }
}