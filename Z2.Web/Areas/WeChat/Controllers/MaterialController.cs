using System.Web.Mvc;
using Z2.Core.Filter;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Web.Filter;

namespace Z2.Web.Areas.WeChat.Controllers
{
    [ModuleAction(ModuleId = "41E1CC91-C5FC-443E-8642-F0417A395D80", ModuleName = "素材管理")]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    [AuthoritiesCheck]
    public class MaterialController : HandlerLoginInfoController
    {
        // GET: SystemManage/Material
        public ActionResult GetGridList(string  keyword)
        {

            return Content("");
        }
    }
}