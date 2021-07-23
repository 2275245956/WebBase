using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.SystemManage.Controllers
{
  //  [LoginAuthority(AuthorityLevel = 2)]
    [ModuleAction(ModuleId = "91A6CFAD-B2F9-4294-BDAE-76DECF412C6C", ModuleName = "角色管理",DisplayNo = 6)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class RoleController : HandlerLoginInfoController
    {
        private RoleRep roleApp = new RoleRep();
        // GET: SystemManage/Role
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            string type = "1";
            var data = roleApp.GetList(type, keyword);
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);//更改gridParam模型中的实例
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),//总页数
                page = gridParam.page,//当前页
                records = data.Count(),//总记录数
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),//当前行数
            };
            return Content(result.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = roleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var keyword = "";
            string type = "1";
            var data = roleApp.GetList(type, keyword);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.Name;
                treeModel.parentId = item.OrganizeId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysRole roleEntity, string keyValue)
        {
            roleEntity.Id = keyValue;
            //roleEntity.AllowEdit = true;
            //roleEntity.AllowDelete = true;
            roleEntity.CreaterUserId = "admin";
            roleEntity.LastUpdateUserId = "admin";
            var bl = roleApp.SubmitForm(roleEntity);
            return Success("操作成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            roleApp.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }

    }
}