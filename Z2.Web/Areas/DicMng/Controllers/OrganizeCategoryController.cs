using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{

[ModuleAction(ModuleId = "5A021901-D464-49B7-AC81-7643636BA228", ModuleName = "机构分类", ParentModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", DisplayNo = 11)]

    public class OrganizeCategoryController : DicBaseController
    {
        // GET: DicMng/OrganizeCategory
        public override string CategoryCode => "OrganizeCategory";
    }
}