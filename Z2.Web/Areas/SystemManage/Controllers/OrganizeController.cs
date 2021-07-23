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

    //[LoginAuthority(AuthorityLevel = 2)]
    [ModuleAction(ModuleId = "14032AF7-DECF-46C3-A04B-1E4E65165A68", ModuleName = "机构管理",DisplayNo = 4)]
    [AuthoritiesCheck]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class OrganizeController : HandlerLoginInfoController
    {
        private OrganizeRep organizeApp = new OrganizeRep();

        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var keyword = "";
            var data = organizeApp.GetList(keyword);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.Name;
                treeModel.parentId = item.ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeJson()
        {
            var keyword = "";
            var data = organizeApp.GetList(keyword);
            var treeList = new List<TreeViewModel>();
            foreach (SysOrganize item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.Name;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = organizeApp.GetList(keyword);
            var treeList = new List<TreeGridModel>();
            foreach (SysOrganize item in data)
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

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = organizeApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysOrganize organizeEntity, string keyValue)
        {
            organizeEntity.Id = keyValue;
            organizeEntity.AllowEdit = true;
            organizeEntity.AllowDelete = true;
            organizeEntity.CreaterUserId = "admin";
            organizeEntity.LastUpdateUserId = "admin";
            var bl = organizeApp.SubmitForm(organizeEntity);
            return Success("操作成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var bl = organizeApp.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }
    }


}