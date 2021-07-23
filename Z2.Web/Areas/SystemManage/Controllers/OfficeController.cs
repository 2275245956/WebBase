using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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

    [ModuleAction(ModuleId = "CB446310-D56D-4466-8BD8-E123D80801D5", ModuleName = "事业所管理", DisplayNo = 3)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    [AuthoritiesCheck]
    public class OfficeController : HandlerLoginInfoController
    {

        //private string userId = "9f2ec079-7d0f-4fe2-90ab-8b09a8302aba";

        private readonly SysOfficeRep _officeApp = new SysOfficeRep();
        // GET: SystemManage/Office
        [HttpGet]
        [HttpAjaxOnly]
        [AuthoritiesCheck(OperationCode = new []{"Index"})]
        public ActionResult GetGridJson(string keyword)
        {

            var data = _officeApp.GetList(keyword);
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
        [AuthoritiesCheck(OperationCode = new[] { "Detail" })]
        public ActionResult GetFormJson(string keyValue)
        {

            var data = _officeApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        [AuthoritiesCheck(OperationCode = new []{ "Create" ,"Edit"})]
        public ActionResult SubmitForm(SysOffice officeEntity, string keyValue)
        {
            officeEntity.Id = keyValue;
            officeEntity.CreaterUserId = "admin";
            officeEntity.LastUpdateUserId = "admin";
            var bl = _officeApp.SubmitForm(officeEntity);
            this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { Id = keyValue });
            return Success("操作成功。");
        }


        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        [AuthoritiesCheck(Ignore = true)]
        public ActionResult DeleteForm(string keyValue)
        {
            _officeApp.DeleteForm(keyValue, "admin");
            this.WriteInfo(ResultType.success.ToString(), "删除成功。", new { Id = keyValue });
            return Success("删除成功。");
        }


    }
}