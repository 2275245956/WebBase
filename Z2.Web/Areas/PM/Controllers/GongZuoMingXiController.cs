using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Operator;
using Z2.Core.Web;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.PM.Controllers
{
    [ModuleAction(ModuleId = "22E4FB9B-F9E3-4EDC-9A5C-43F6DD1D5439", ModuleName = "任务明细", DisplayNo = 1)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    //  [AuthoritiesCheck]
    public class GongZuoMingXiController : HandlerLoginInfoController
    {
        PmTaskRep pmrep = new PmTaskRep();
        // GET: PM/GongZuoMingXi
        public override ActionResult Index()
        {
            
            ViewBag.ProjectList = new SelectList(PmTaskRep.getProjects(), "ProjectId", "ProjectName");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);

            return View();
        }

        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult LoadMingXiGrid(string projectId, string workerId, DateTime stratDate, DateTime endDate)
        {
            //
            return new JsonResult();// (result, JsonRequestBehavior.AllowGet);
        }

    }
}