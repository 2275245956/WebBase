using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{
    [ModuleAction(ModuleId = "793D324E-AA47-47CA-AE3F-CDC2DE1A34A0", ModuleName = "系统日志", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 6)]
    public class DbLogController : DicBaseController
    {
        // GET: DicMng/DbLog
        public override string CategoryCode => "DbLogType";
    }
}