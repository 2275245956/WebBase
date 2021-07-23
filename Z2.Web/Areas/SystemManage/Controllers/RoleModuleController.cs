using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;


namespace Z2.Web.Areas.SystemManage.Controllers
{

   // [LoginAuthority(AuthorityLevel = 2)]
    [ModuleAction(ModuleId = "7554389A-C25A-4E38-B45B-9A7D39D9900", ModuleName = "权限管理",DisplayNo = 7)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class RoleModuleController : HandlerLoginInfoController
    {
        public SysRoleModuleRep roleModuleRep = new SysRoleModuleRep();
        // GET: SystemManage/RoleModule
        public override ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetOperateDataJson(string roleValue, string moduleValue)
        {
            var data = roleModuleRep.GetOperateDataJson(roleValue, moduleValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRoleAuthorize(RoleAuthorizeData roleAuthorize)
        {
            var bl = roleModuleRep.SaveRoleAuthorize(roleAuthorize.roleValue, roleAuthorize.moduleValue, roleAuthorize.roleAuthorizeEntity);
            return Success("保存成功!");
        }
        public class RoleAuthorizeData
        {
            public string roleValue { get; set; }
            public string moduleValue { get; set; }
          
            public List<SysModuleOperate> roleAuthorizeEntity { get; set; }
        }
    }
}