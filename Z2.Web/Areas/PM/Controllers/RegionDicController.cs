using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.PM.Controllers
{
    //[Guid("22CCA472-88BF-4D32-B85B-F98185860489")]
    [ModuleAction(ModuleId = "22CCA472-88BF-4D32-B85B-F98185860489", ModuleName = "省市县",DisplayNo = 6)]
    public class RegionDicController : TreeDicBaseController
    {
        public override string CategoryCode
        {
            get
            {
                return "Area";
            }
        }
    }
}