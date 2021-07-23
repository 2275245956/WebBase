using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Filter;
using Z2.Core.Interface;
using Z2.Core.Operator;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.PM.Controllers
{
    [ModuleAction(ModuleId = "F9BCB032-79D8-4C13-BF68-39EE3DA56449", ModuleName = "项目管理", DisplayNo = 4)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    [AuthoritiesCheck]
    public class ProjectManagerController : HandlerLoginInfoController
    {
        private string currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
        public ActionResult GetFormJson(string keyValue, string keyWord)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var list = new PMProjectManagerRep().GetFormJson(keyValue, keyWord, "");
            var result = new
            {
                total = gridParam.GetPageTotal(list.Count()),
                page = gridParam.page,
                records = list.Count(),
                rows = list.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            if (!string.IsNullOrEmpty(keyValue))
            {
                return Json(list[0], JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEditJson(string keyValue, string keyWord)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var list = new PMProjectManagerRep().GetFormJson(keyValue, keyWord, "");
            var result = new
            {
                total = gridParam.GetPageTotal(list.Count()),
                page = gridParam.page,
                records = list.Count(),
                rows = list.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Json(list[0], JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SubmitForm(PMProjectManagerModel ProjectData)
        {
            ProjectData.LastUpdateTime = DateTime.Now;
            ProjectData.LastUpdateUserId = currentUserId;
            ProjectData.CreaterTime = DateTime.Now;
            ProjectData.CreaterUserId = currentUserId;
            if (!string.IsNullOrEmpty(ProjectData.ProjectId))
            {
                var bl = new PMProjectManagerRep().AlterProject(ProjectData);
                if (bl)
                {
                    return Success("修改成功");
                }
                else
                {
                    return Error("修改失败");
                }
            }
            else
            {
                ProjectData.ProjectId = System.Guid.NewGuid().ToString();
                ProjectData.DeleteFlag = false;
                var bl = new PMProjectManagerRep().AddProject(ProjectData);
                if (bl)
                {
                    return Success("添加成功");
                }
                else
                {
                    return Error("添加失败");
                }
            }
        }
        [HttpPost]
        public ActionResult DeleteForm(string keyValue)
        {
            var projectData = new PMProjectManagerModel();
            projectData.ProjectId = keyValue;
            projectData.DeleteTime = DateTime.Now;
            projectData.DeleteUserId = currentUserId;
            var bl = new PMProjectManagerRep().DeleteProject(projectData);
            if (bl)
            {
                return Success("删除成功");
            }
            else
            {
                return Error("删除失败");
            }

        }
    }
}