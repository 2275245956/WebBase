using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Web.Areas.CMS.Controllers
{
    public class WidgetController : WidgetBaseController<CMS_WidgetBase>
    {
        private readonly CMS_WidgetBaseRep _cmsWidgetBase = new CMS_WidgetBaseRep();
        /// <summary>
        /// 删除组件
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteWidget(string ID)
        {
            if (_cmsWidgetBase.DeleteWidget(ID))
            {
                this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { Id = ID });
                return Json(true);
            }
            else
            {
                this.WriteInfo(ResultType.error.ToString(), "操作失败。", new { Id = ID });
                return Json(false);
            }



        }

        /// <summary>
        /// 保存组件的位置  信息
        /// </summary>
        /// <param name="widgets"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePosition(List<CMS_WidgetBase> widgets)
        {
            foreach (var widget in widgets)
            {
                _cmsWidgetBase.UpdateWidget(widget);
            }

            return Content("");
        }

        public ActionResult Edit(string Id)
        {
            var widget = _cmsWidgetBase.GetWidgetModel(Id);
            if (widget != null)
            {
                //保存WidgetId
                ViewBag.WidgetSetUrl = widget.SetUrlAddress;
            }
            return Content("");
        }

        public ActionResult SelectWidget(CMS_WidgetBase widgetBase)
        {
            //可以获取到 LayoutId   PageId   ZoneId    
            //待完成   
            return Content($"PageId：{widgetBase.PageId ?? "空"}     LayoutId：{widgetBase.LayoutId}      ZoneId：{widgetBase.ZoneId}");
        }

    }
}