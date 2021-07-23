using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.PM.Controllers
{
    [ModuleAction(ModuleId = "748EF5D3-70B7-4BC8-98C0-A90F7CE051E2", ModuleName = "Bug分类",DisplayNo = 3)]
    public class BugTypeDicController : DicBaseController
    {
        public override string CategoryCode
        {
            get
            {
                return "BugType";
            }
        }
    }
}