using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Configs;
using Z2.Core.Filter;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.Mall.Controllers
{

    [ModuleAction(ModuleId = "AA69997F-D75D-44F4-BE94-2A5CBD579444", ModuleName = "店铺行业",DisplayNo = 4)]
    [AuthoritiesCheck]
    public class StoreIndustriesController : HandlerLoginInfoController
    {


        // GET: Mall/StoreRank
        private readonly MallStoreIndustries _mdl = new MallStoreIndustries();
        private readonly MallStoreIndustriesRep _mallStoreIndustriesRep = new MallStoreIndustriesRep();
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _mallStoreIndustriesRep.GetList(keyword);
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count),
                page = gridParam.page,
                records = data.Count,
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows)
            };
            return Content(result.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _mallStoreIndustriesRep.GetForm(keyValue);

            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var data = _mallStoreIndustriesRep.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(MallStoreIndustries mallStoreIndustries, string keyValue)
        {
            mallStoreIndustries.Id = keyValue;

            mallStoreIndustries.LastUpdateUserId = "admin";
            var bl = _mallStoreIndustriesRep.SubmitForm(mallStoreIndustries);
            return Success("操作成功。");

        }




    }
}