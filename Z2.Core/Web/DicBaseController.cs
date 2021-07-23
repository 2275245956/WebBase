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
using Z2.Core.Operator;
using Z2.Core.Web;
using Z2.Core.Web.Results;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Core.Web
{
    public class DicBaseController : HandlerLoginInfoController
    {
        public virtual string CategoryCode { get; private set; }
        protected SysItemRep Rep = new SysItemRep();

        public override ActionResult Index()
        {
            if (string.IsNullOrEmpty(this.CategoryCode))
            {
                return new HttpNotFoundResult();
            }
            var itemCategory = Rep.GetCategory(this.CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }
            return View("~/Areas/DicMng/Views/Dic/DicIndex.cshtml", itemCategory);
        }

        public override ActionResult Details()
        {
            if (string.IsNullOrEmpty(this.CategoryCode))
            {
                return new HttpNotFoundResult();
            }
            var itemCategory = Rep.GetCategory(this.CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }
            return View("~/Areas/DicMng/Views/Dic/DicDetails.cshtml", itemCategory);
        }

        public override ActionResult Form()
        {
            if (string.IsNullOrEmpty(this.CategoryCode))
            {
                return new HttpNotFoundResult();
            }
            var itemCategory = Rep.GetCategory(this.CategoryCode);
            if (itemCategory == null || string.IsNullOrEmpty(itemCategory.Id))
            {
                return new HttpNotFoundResult();
            }
            return View("~/Areas/DicMng/Views/Dic/DicForm.cshtml", itemCategory);
        }

        [HttpGet]
        [HttpAjaxOnly]
        public virtual ActionResult GetGridJson(string keyword)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var data = Rep.GetItems(this.CategoryCode, keyword);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),
                page = gridParam.page,
                records = data.Count(),
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Content(result.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetDicInfoJson(string keyValue)
        {
            var data = Rep.GetItemsByCategoryCodeAndKeyValue(this.CategoryCode, keyValue);
            return Content(data.ToJson());
        }



        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public virtual ActionResult DeleteForm(string keyValue)
        {
            var currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
            Rep.DeleteInfoByCategoryCode(keyValue, currentUserId);
            return Success("删除成功。");
        }

        [HttpPost]
        [HttpAjaxOnly]
        public virtual ActionResult SubmitForm(string keyValue, SysItem sysItem)
        {
            if (string.IsNullOrEmpty(this.CategoryCode))
            {
                return Error("CategoryCode is Empty");
            }
            var itemCategory = Rep.GetCategory(this.CategoryCode);
            if (itemCategory == null)
            {
                return Error("Category is null");
            }
            if (!string.IsNullOrEmpty(sysItem.ExtendData))
            {
                var ExtendData = Encoding.UTF8.GetString(Convert.FromBase64String(sysItem.ExtendData));
                sysItem.ExtendData = ExtendData;
            }
            var currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
            sysItem.LastUpdateUserId = currentUserId;
            sysItem.CreaterUserId = currentUserId;
            sysItem.ItemCategoryId = itemCategory.Id;
            sysItem.Id = keyValue;
            var data = Rep.SubmitForm(sysItem);
            return Success("操作成功！");
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual ActionResult Export(string keyword)
        {
            var itemCategory = Rep.GetCategory(this.CategoryCode);

            var exportSpource = GetExportData(this.CategoryCode, keyword);
            var dt = JsonConvert.DeserializeObject<System.Data.DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                this.CategoryCode,
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
            var list = Rep.GetItems(category, keyword);
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

        public ActionResult GetTreeSelectJson()
        {
            var keyword = "";
            var data = Rep.GetItems(this.CategoryCode, keyword);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.ItemName;
                treeModel.parentId = item.Id;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }

        public virtual ActionResult GetJqGridSelectJson()
        {
            var keyword = "";
            var data = Rep.GetItems(this.CategoryCode, keyword);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<select>");
            data.ForEach(m =>
            {
                if (sb.Length > 0)
                {
                    sb.AppendFormat("<option value='{0}'>{1}</option>", m.Id, m.ItemName);
                }
                else
                {
                    sb.AppendFormat("<option value='{0}'>{1}</option>", m.Id, m.ItemName);
                }
            });
            sb.AppendLine("</select>");

            return Content(sb.ToString());
        }



    }

}
