using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Filter;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Operator;
using Z2.Core.Web;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.PM.Controllers
{
    [ModuleAction(ModuleId = "22E4FB9B-F9E3-4EDC-9A5C-43F6DD1D5440", ModuleName = "明细一览", DisplayNo = 1)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class MingXiController : HandlerLoginInfoController
    {
        PmTaskRep pmrep = new PmTaskRep();
        // GET: PM/MingXi
        public override ActionResult Index()
        {
            ViewBag.ProjectList = new SelectList(PmTaskRep.getProjects(), "ProjectId", "ProjectName");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);

            var strat = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            var end = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
            ViewBag.stratDate = string.Format("{0:yyyy/MM/dd}", strat);
            ViewBag.endDate = string.Format("{0:yyyy/MM/dd}", end);
            return View();
        }


        [HttpGet]
        [HttpAjaxOnly]
        
        public ActionResult LoadMingXiGrid(string projectId, string workerId, string stratDate, string endDate)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            //var currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
            var list = pmrep.GetRenWuMingXiList(projectId, workerId, stratDate, endDate);

            var result = new
            {
                total = gridParam.GetPageTotal(list.Count()),
                page = gridParam.page,
                records = list.Count(),
                rows = list.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}