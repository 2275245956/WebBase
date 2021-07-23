using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    [ModuleAction(ModuleId ="8C025215-5CF5-4437-986C-67805F393522", ModuleName = "生育", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 4)]
    public class BirthController : DicBaseController
    {
        // GET: DicMng/Birth
        public override string CategoryCode => "102";

    }
}