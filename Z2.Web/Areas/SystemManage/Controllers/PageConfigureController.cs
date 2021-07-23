using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.SystemManage.Controllers
{
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    [ModuleAction(ModuleId = "5FF56B0D-BE7F-4D67-93F4-1ECD1828281C", ModuleName = "页面配置",DisplayNo = 5)]

    public class PageConfigureController : HandlerLoginInfoController
    {
        // GET: SystemManage/PageConfigure
        SysPageRep pageRep = new SysPageRep();
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = pageRep.GetGridJson(keyword);
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
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            pageRep.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = pageRep.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysPage pageEntity, string keyValue)
        {
            var bl = pageRep.SubmitForm(pageEntity, keyValue);
            return Success("操作成功。");
        }
        public ActionResult Test()
        {
            return View();
        }
    }
}