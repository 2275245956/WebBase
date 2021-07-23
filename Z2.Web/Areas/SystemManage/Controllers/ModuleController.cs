using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z2.Core.WebApp.Repository;
using System.Web.Mvc;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.Handler;
using Z2.Core;
using Z2.Core.Interface;
using Z2.Web.Filter;

namespace Z2.Web.Areas.SystemManage.Controllers
{

   // [LoginAuthority(AuthorityLevel = 2)]
    [ModuleAction(ModuleId = "D5454E28-2ED6-4093-A032-A08FAAF180A1", ModuleName = "模块管理",DisplayNo = 2)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class ModuleController : HandlerLoginInfoController
    {
        private ModuleRep moduleApp = new ModuleRep();
        // GET: SystemManage/Module
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = moduleApp.GetList(keyword);
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    data = data.TreeWhere(t => t.Name.Contains(keyword));
            //}
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetOperateDataJson(string keyValue)
        {
            var data = moduleApp.GetOperateDataJson(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            moduleApp.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = moduleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = moduleApp.GetList("");
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.Name;
                treeModel.parentId = item.ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysModule moduleEntity, string keyValue)
        {
            moduleEntity.Id = keyValue;
            moduleEntity.CreaterUserId = "admin";
            moduleEntity.LastUpdateUserId = "admin";
            var bl = moduleApp.SubmitForm(moduleEntity);
            return Success("操作成功。");
        }
        [HttpGet]
        public ActionResult addOpcode()
        {
            return View();
        }
        [HttpGet]
        public ActionResult delOpcode()
        {
            return View();
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetModuleName(string keyValue)
        {
            var data = moduleApp.GetModuleName(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetModuleData(string keyValue)
        {
            var data = moduleApp.GetModuleData(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitOpcode(SysModuleOperate OpcodeEntity)
        {
            var bl = moduleApp.SubmitOpcode(OpcodeEntity);
            return Success("操作成功");
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult DelOpcode(string keyValue)
        {
            var bl = moduleApp.DelOpcode(keyValue);
            return Success("删除成功");
        }

    }
}