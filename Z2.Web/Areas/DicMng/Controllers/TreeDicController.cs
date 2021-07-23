using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using Z2.Core.Web.Results;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.DicMng.Controllers
{

    
    [ModuleAction(ModuleId = "9135BCB1-BA71-4152-AF2F-E4F320D79147", ModuleName ="树形字典",DisplayNo = 2)]
    [AuthoritiesCheck]
    public class TreeDicController : HandlerLoginInfoController
    {
        private SysItemRep rep = new SysItemRep();
        // GET: DicMng/TreeDic
        public override ActionResult Index()
        {
            return RedirectToAction("CategoryIndex", new { category = "" });
        }

        public override ActionResult Details()
        {
            return RedirectToAction("CategoryDetails", new { category = "" });
        }

        public override ActionResult Form()
        {
            return RedirectToAction("CategoryForm", new { category = "" });
        }


        /// <summary>
        /// 获取种类的Name 查询所有信息  返回到前台
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult CategoryIndex(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return new HttpNotFoundResult();
            }
            var itemCategory = rep.GetCategory(category);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }
            return View("Index", itemCategory);
        }
        /// <summary>
        /// 根据种类加载相应的信息
        /// </summary>
        /// <param name="category">种类</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetTreeGridArea(string category, string keyword)
        {

            var data = rep.GetItemListByItemCategoryId(category, keyword);
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
        public ActionResult GetGridJson(string category, string keyword)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var data = rep.GetItems(category,keyword);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),
                page = gridParam.page,
                records = data.Count(),
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Content(result.ToJson());
        }

        public ActionResult CategoryDetails(string category, string keyword)
        {
            return new EmptyResult();
        }

        public ActionResult CategoryDeleteForm(string category, string keyword)
        {
            return new EmptyResult();
        }

        public ActionResult CategoryForm(string category, string keyword)
        {
            return new EmptyResult();
        }

        public ActionResult Export(string category, string keyword)
        {
            var itemCategory = rep.GetCategory(category);

            var exportSpource = GetExportData(category, keyword);
            var dt = JsonConvert.DeserializeObject<System.Data.DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                category,
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
            var list = rep.GetItems(category,keyword);
            JArray jObjects = new JArray();
            foreach (var item in list)
            {
                var jo = new JObject();
                jo.Add("名称", item.ItemName);
                jo.Add("编号", item.ItemCode);
                jo.Add("排序", item.DisplayNo);
                jo.Add("默认", item.IsDefault);
                //jo.Add("创建时间", item.CreaterTime);
                jo.Add("有效", item.EnabledFlag);
                jo.Add("备注", item.Description);
                jObjects.Add(jo);
            }
            return jObjects;
        }



    }
}