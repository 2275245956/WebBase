using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    [ModuleAction(ModuleId = "19CDE7E1-AEF8-4053-B229-5A3D2D0D711D", ModuleName = "学历", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 8)]
    public class EducationController : DicBaseController
    {
        // GET: DicMng/Education
        public override string CategoryCode => "Education";
    }
}