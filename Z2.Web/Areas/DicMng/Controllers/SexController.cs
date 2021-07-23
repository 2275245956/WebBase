using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{

[ModuleAction(ModuleId = "2F0B678F-0E39-4C96-9AFF-276E87F3E947", ModuleName = "性别", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 12)]
    public class SexController : DicBaseController
    {
        // GET: DicMng/Sex
        public override string CategoryCode => "104";
    }
}