using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Senparc.CO2NET.Extensions;
using Z2.Core.Filter;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Areas.CMS.Common;
using Z2.Web.Areas.PM.Controllers;
using Z2.Web.Filter;


namespace Z2.Web.Areas.CMS.Controllers
{
    [ModuleAction(ModuleId = "1F82EB65-44CE-4ED5-B5AD-031910D3E786", ModuleName = "布局管理", DisplayNo = 1)]
    [AuthoritiesCheck]
    [Localization]
    public class LayoutController : HandlerLoginInfoController
    {
        private readonly LayOutRep _layOut = new LayOutRep();

        private readonly CmsMediaRep _cmsMedia = new CmsMediaRep();

        private readonly CmsZonesRep _cmsZones = new CmsZonesRep();

        private readonly CMS_WidgetBaseRep _cmsWidgetBase = new CMS_WidgetBaseRep();
        //private readonly 
        // GET: CMS/Layout
        #region 布局操作 CRUD================
        /// <summary>
        /// 获取所有的布局
        /// </summary>
        /// <param name="keyword">关键字 </param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetLayoutList(string keyword)
        {
            var data = _layOut.GetList(keyword);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Delete(string keyValue)
        {
            if (_layOut.DeleteForm(keyValue))
                this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { Id = keyValue });
            else
                return Error("操作失败!");
            return Success("操作成功。");


        }

        /// <summary>
        /// 修改  详细  加载数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetForm(string keyValue)
        {
            var data = _layOut.GetForm(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 修改  添加 提交数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CMS_Layout layout, string keyValue)
        {

            if (_layOut.SubmitForm(layout, keyValue))
                this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { Id = keyValue });
            else
                return Error("操作失败!");
            return Success("操作成功。");
        }

        #endregion


        #region 布局设置=====================

        /// <summary>
        /// 获取某种 组件下的所有布局
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>

        public ActionResult Design(string keyValue)
        {
            var dataHtml = _layOut.GetLayOutHtml(keyValue);
            return View(dataHtml);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveLayout(string[] html, CMS_Layout layout, CMS_Zone.ZonesCollection zones)
        {
            layout.Html = GenerateHtml(html, zones);
            layout.Zones = zones;
            //更新CMS_LayoutHtml
            var resHtml = _layOut.SaveLayoutHtl(layout.Html);
            //添加或更新 CMS_Zone表
            var resZone = _layOut.SaveZone(layout.Zones);
            if (resHtml && resZone)
                this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { id = "布局更新" });
            return RedirectToAction("Index", "Home");


        }

        /// <summary>
        /// 解析html字符串
        /// </summary>
        /// <param name="html"></param>
        /// <param name="zones"></param>
        /// <returns></returns>
        private static LayoutHtmlCollection GenerateHtml(IReadOnlyList<string> html, CMS_Zone.ZonesCollection zones)
        {
            int zoneIndex = 0;

            var result = new LayoutHtmlCollection();
            for (int i = 0; i < html.Count; i++)
            {
                var item = html[i];
                if (item == CMS_Zone.ZoneTag)
                {
                    var zone = zones[zoneIndex];
                    if (zone.ID.IsNullOrWhiteSpace())
                    {
                        zone.ID = Guid.NewGuid().ToString("N");
                    }
                    result.Add(new CMS_LayoutHtml { Html = CMS_Zone.ZoneTag });
                    result.Add(new CMS_LayoutHtml { Html = zone.ID });
                    result.Add(new CMS_LayoutHtml { Html = CMS_Zone.ZoneEndTag });
                    i += 1;
                    zoneIndex++;
                }
                else
                {
                    result.Add(new CMS_LayoutHtml { Html = item });
                }
            }
            return result;
        }

        #endregion


        #region 图片背景设置 暂时不用========

        /// <summary>
        /// 获取文件夹下的媒体图片等
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult BgImageSelect(string parentId, int? pageIndex)
        {
            parentId = parentId ?? "#";
            CmsPagination pagin = new CmsPagination { PageIndex = pageIndex ?? 0 };
            //获取文件夹
            var medias = _cmsMedia.GetAllList(parentId);
            var viewModels = new MediaViewModel
            {
                ParentId = parentId,
                Medias = medias,
                Pagin = pagin
            };
            if (parentId == "#")
                return View(viewModels);
            viewModels.Parent = _cmsMedia.GetModel(parentId);
            viewModels.Parents = new List<CMS_Media>();
            LoadParents(viewModels.Parent, viewModels.Parents);

            return View(viewModels);
        }

        private void LoadParents(CMS_Media viewModelsParent, IList<CMS_Media> viewModelsParents)
        {
            while (true)
            {
                if (viewModelsParent != null)
                {
                    viewModelsParents.Insert(0, viewModelsParent);
                    if (viewModelsParent.ParentID != "#")
                    {
                        var p = _cmsMedia.GetModel(viewModelsParent.ParentID);
                        if (p != null)
                        {
                            viewModelsParent = p;
                            continue;
                        }
                    }
                }

                break;
            }
        }

        [HttpPost]
        public ActionResult SaveMedia(string id, string title, string parentId)
        {
            return Content("");
        }
        #endregion


        #region 布局概览 页面加载========

        public ActionResult LayoutZones(string ID)
        {
            var layout = _layOut.GetLayOutHtml(ID);
            LayoutHtmlCollection layouthtml = new LayoutHtmlCollection();

            for (int i = 0; i < layout.Count; i++)
            {
                layouthtml.Add(layout[i]);
            }
            var viewModel = new LayoutZonesViewModel
            {

                LayoutID = ID,
                Zones = _cmsZones.GetModel(ID),
                Widgets = _cmsWidgetBase.GetWidgetByLayoutId(ID),
                LayoutHtml = layouthtml
            };
            for (int i = 0; i < viewModel.Widgets.Count(); i++)
            {
                viewModel.Widgets.ToList()[i].WidgetData = string.IsNullOrEmpty(viewModel.Widgets.ToList()[i].WidgetData) ? "无标题" : (JsonConvert.DeserializeObject(viewModel.Widgets.ToList()[i].WidgetData) as PendingTaskViewModel)?.WString.ToString();
            }

            return View(viewModel);
        }

        #endregion
    }
}