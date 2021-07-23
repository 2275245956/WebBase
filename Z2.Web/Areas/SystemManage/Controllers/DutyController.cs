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
    [ModuleAction(ModuleId = "F298F868-B689-4982-8C8B-9268CBF0308D", ModuleName = "岗位管理",DisplayNo = 1)]
    [AuthoritiesCheck]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    public class DutyController : HandlerLoginInfoController
    {

        private RoleRep roleApp = new RoleRep();
        // GET: SystemManage/Duty

        /// <summary>
        /// 加载岗位信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            string type = "2";
            var data = roleApp.GetList(type, keyword);
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
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var keyword = "";
            string type = "2";
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
        /// <summary>
        /// 加载详细信息  或者增加信息
        /// </summary>
        /// <param name="keyValue">Id 如果Id为空就是增加,否则就是加载修改信息</param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = roleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 添加/修改信息的提交 
        /// </summary>
        /// <param name="roleEntity">实体对象</param>
        /// <param name="keyValue">Id</param>
        /// <returns></returns>
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
        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
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