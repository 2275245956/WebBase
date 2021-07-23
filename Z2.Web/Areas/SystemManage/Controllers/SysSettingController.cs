using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Filter;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.SystemManage.Controllers
{
   // [LoginAuthority(AuthorityLevel = 2)]
    [ModuleAction(ModuleId = "958983E6-D109-4322-9CA2-F477FEDADBEA", ModuleName = "系统管理",DisplayNo = 8)]
    [AuthoritiesCheck]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class SysSettingController : HandlerLoginInfoController
    {
        public override ActionResult Index()
        {
            return View();
        }
        SysSettingRep settingRep = new SysSettingRep();
        #region System global configuration
        /// <summary>
        ///  load configuration information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = settingRep.GetSysSettings(keyword);
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),
                page = gridParam.page,
                records = data.Count(),
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 加载Form时 发出的请求的处理
        /// </summary>
        /// <param name="keyValue">关键字  要获取信息的Id</param>
        /// <returns>返回Json对象</returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = settingRep.GetSysSettingsFrom(keyValue);
            return Content(data.ToJson());
        }


        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysSettings sysSettingEntity, string keyValue)
        {
            sysSettingEntity.SettingID = keyValue;

            sysSettingEntity.LastUpdateUserId = "admin";
            var bl = settingRep.SubmitForm(sysSettingEntity);
            return Success("操作成功。");
        }


        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            settingRep.DeleteForm(keyValue);
            return Success("删除成功。");
        }


        #endregion












        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetOperateDataJson(string roleValue, string moduleValue)
        {
            var data = settingRep.GetOperateDataJson(roleValue, moduleValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRoleAuthorize(RoleAuthorizeData roleAuthorize)
        {
            var bl = settingRep.SaveRoleAuthorize(roleAuthorize.roleValue, roleAuthorize.moduleValue, roleAuthorize.roleAuthorizeEntity);
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