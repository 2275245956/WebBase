using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{

    [ModuleAction(ModuleId = "0D32A6BB-2680-40ED-9211-675AD6C93A78", ModuleName = "数据库类型", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 7)]  
    public class DbTypeController : DicBaseController
    {
        // GET: DicMng/DbType
        public override string CategoryCode => "DbType";
    }
}