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
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.DicMng.Controllers
{
    //[Guid("89827093-F3BB-42DA-A3EA-94E905E1D955")]
    [ModuleAction(ModuleId = "89827093-F3BB-42DA-A3EA-94E905E1D955", ModuleName = "字段管理",DisplayNo = 1)]
    [AuthoritiesCheck]
    public class DicController : HandlerLoginInfoController
    {
        private SysItemRep rep = new SysItemRep();
        // GET: DicMng/Dic
       
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
        /// 根据category加载相应的视图
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
        
      
        
        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="category"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult CategoryDetails(string category, string keyValue)
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
            return View("Details", itemCategory);

        }


        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetDicInfoJson(string category, string keyValue)
        {
            var data = rep.GetItemsByCategoryCodeAndKeyValue(category, keyValue);
            return Content(data.ToJson());
        }



        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="category"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult CategoryDeleteForm(string category, string keyValue)
        {
            rep.DeleteInfoByCategoryCode(keyValue, "admin");
            return Success("删除成功。");
        }

        /// <summary>
        /// 修改和添加
        /// </summary>
        /// <param name="category"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult CategoryForm(string category, string keyValue)
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
            return View("Form", itemCategory);
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult SubmitForm(string category, string keyValue, SysItem sysItem)
        {
            sysItem.LastUpdateUserId = "admin";
            sysItem.CreaterUserId = "admin";
            sysItem.ItemCategoryId = category;
            sysItem.Id = keyValue;
            var data = rep.SubmitForm(sysItem);
            return Success("操作成功！");

        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="category"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
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