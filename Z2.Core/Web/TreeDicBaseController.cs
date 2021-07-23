using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.Web.Results;
using Z2.Core.WebApp.Repository;

namespace Z2.Core.Web
{
    public class TreeDicBaseController : DicBaseController
    {
        private SysItemRep rep = new SysItemRep();
        // GET: DicMng/TreeDic
        public override ActionResult Index()
        {

            if (string.IsNullOrEmpty(CategoryCode))
            {
                return new HttpNotFoundResult();
            }
            var itemCategory = rep.GetCategory(CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }

            return View("~/Areas/DicMng/Views/TreeDic/DicIndex.cshtml", itemCategory);
        }

        public override ActionResult Form()
        {
            if (string.IsNullOrEmpty(this.CategoryCode))
            {
                return new HttpNotFoundResult();
            }
            var itemCategory = rep.GetCategory(this.CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }
            //ItemList
            //var projects = rep.GetItemListByItemCategoryId(itemCategory.Id);
            //projects.Insert(0, new WebApp.Model.SysItem { Id = "0", ItemName = "未指定" });
            //ViewBag.ItemList = new SelectList(projects, "Id", "ItemName");
            return View("~/Areas/DicMng/Views/TreeDic/DicForm.cshtml", itemCategory);
        }

        /// <summary>
        /// 根据种类加载相应的信息
        /// </summary>
        /// <param name="category">种类</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeGridArea( string keyword)
        {
            var itemCategory = rep.GetCategory(this.CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }

            var data = rep.GetItemListByItemCategoryId(itemCategory.Id,keyword);
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
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
        public ActionResult GetTreeSelectJson()
        {
            var itemCategory = rep.GetCategory(this.CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }

            var keyword = "";
            var data = rep.GetItems(this.CategoryCode, keyword);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.ItemName;
                treeModel.parentId = item.ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        public override ActionResult Export( string keyword)
        {
            var itemCategory = rep.GetCategory(CategoryCode);

            var exportSpource = GetExportData(CategoryCode, keyword);
            var dt = JsonConvert.DeserializeObject<System.Data.DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                CategoryCode,
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");

            return new ExportExcelResult
            {
                SheetName = itemCategory.CategoryName + "列表",
                FileName = exportFileName,
                ExportData = dt
            };
        }

        private JArray GetExportData(string category, string keyword)
        {
            var list = rep.GetItems(category, keyword);
            JArray jObjects = new JArray();
            foreach (var item in list)
            {
                var jo = new JObject();
                jo.Add("名称", item.ItemName);
                //jo.Add("编号", item.ItemCode);
                //jo.Add("默认", item.IsDefault);
                jo.Add("有效", item.EnabledFlag);
                jo.Add("创建时间", item.CreaterTime);
                jo.Add("排序", item.DisplayNo);
                jo.Add("备注", item.Description);
                jObjects.Add(jo);
            }
            return jObjects;
        }
    }
}
