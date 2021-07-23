using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    //[ModuleAction]
    [ModuleAction(ModuleId = "224B8776-6B8E-4FD7-BCEE-A3548A66C555", ModuleName = "区域字典",ParentModuleId = "9135BCB1-BA71-4152-AF2F-E4F320D79147", DisplayNo = 1)]
    public class AreaController : TreeDicBaseController
    {
        // GET: DicMng/Area
        public override string CategoryCode => "Area";
    }
}