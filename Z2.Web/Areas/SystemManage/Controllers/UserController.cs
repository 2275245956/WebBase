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
    [ModuleAction(ModuleId = "753D9C5C-7A04-4292-9A94-4D5E2ABAC9AE", ModuleName = "用户管理",DisplayNo = 9)]
    [AuthoritiesCheck]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class UserController : HandlerLoginInfoController
    {
        // GET: SystemManage/User
        private UserRep userApp = new UserRep();
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var data = userApp.GetList(keyword);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),
                page = gridParam.page,
                records = data.Count(),
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Content(result.ToJson());
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetFormGridData(string keyValue)
        {
            var data = userApp.GetFormGridData(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            userApp.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            SysUser userEntity = new SysUser();
            userEntity.Id = keyValue;
            userEntity.EnabledFlag = false;
            userApp.UpdateForm(userEntity, "admin");
            return Success("账户禁用成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            SysUser userEntity = new SysUser();
            userEntity.Id = keyValue;
            userEntity.EnabledFlag = true;
            userApp.UpdateForm(userEntity, "admin");
            return Success("账户启用成功。");
        }
        public ActionResult RevisePassword()
        {
            return View();
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(SysUser userEntity, string keyValue)
        {
            userApp.RevisePassword(userEntity.Password, keyValue);
            return Success("重置密码成功。");
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysUser userEntity, string keyValue)
        {
            userEntity.Id = keyValue;
            userEntity.CreaterUserId = "admin";
            userEntity.LastUpdateUserId = "admin";
            var bl = userApp.SubmitForm(userEntity);
            return Success("操作成功。");
        }
        [HttpGet]
        public ActionResult Assign()
        {
            return View();
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAssignRoles(List<SysUser> dataList, string keyValue)
        {
            var bl = userApp.SubmitAssignRoles(dataList, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetRoleIdList(string keyValue)
        {
            var data = userApp.GerRoleIdListByUserId(keyValue);
            return Content(data.ToJson());
        }

    }
}