
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Z2.Core.Handler;
using Z2.Core.Web;
using Z2.Core.WebApp.Repository;
using System.Linq;
using Z2.Core.WebApp.Model;
using System.IO;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.Entities;
using Z2.Core;
using Z2.Core.Interface;
using Z2.Core.Operator;
using Z2.Core.Utility;
using Z2.Web.Filter;
using Newtonsoft.Json;

namespace Z2.Web.Controllers
{
    /// <summary>
    /// 多语言设置  lang的值的 过滤器
    /// </summary>
    [Localization]
    public class HomeController : HandlerLoginInfoController
    {

        private Mall_BrandRep mbr = new Mall_BrandRep();
        private SysSettingRep settingRep = new SysSettingRep();

        /// <summary>
        /// 下面加载根节点使用
        /// </summary>
        SysModule sysModule = new SysModule();

        /// <summary>
        /// 根据用户加载功能菜单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        private ModuleRep moduleApp = new ModuleRep();

        public override ActionResult Index()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View("x_admin_index");
        }

        public ActionResult Unicode()
        {
            return View("x_admin_unicode");
        }

        public ActionResult FontAweSome()
        {
            return View();
        }

        /// <summary>
        /// 通过FilePathResult返回文件路径传送到客户端
        /// </summary>
        /// <returns></returns>
        public ActionResult LogoImage()
        {
            var data = settingRep.GetSetting("SysLogo", "");
            var fr = new FilePathResult(data,
                ContentTypeHelper.ContentType(".jpg")); //获取查询到文件的路径通过FilePathResult以路径形式传送到客户端
            return fr;

            #region  获取到文件路径后以流的形式传送到客户端

            //if (!System.IO.File.Exists(data))//判断指定文件路径是否存在
            //{
            //    return new HttpNotFoundResult();
            //}
            //FileStream fr = new FileStream(data,FileMode.Open);//stream公开以文件为主的 System.IO.Stream，支持同步读写操作，也支持异步读写操作
            //BinaryReader br = new BinaryReader(fr);//BinaryReader基于所指定的流和特定的 UTF-8 编码，参数：输入的流
            //byte[] img = br.ReadBytes((int)fr.Length);//转字节
            //br.Close();//关闭当前流
            //fr.Close();
            //return Json(Convert.ToBase64String(img));//转成base64位字符串发送到前台

            #endregion
        }

        [HttpPost]
        public ActionResult GetHomeColor()
        {
            var data = mbr.GetHomePage();
            var homeColor = data[0].Description;
            return Json(homeColor);
        }

        //拼接菜单字符串
        private StringBuilder _sb = new StringBuilder();

        [HttpGet]
        [HttpAjaxOnly]
        //根据登录用户的id去匹配相对应权限显示节点
        public ActionResult GetRootNodeMenu(string userId)
        {
            List<SysModule> list = new List<SysModule>();
            var data = moduleApp.LoadFuncMenuByUserId(userId);
            foreach (SysModule item in data)
            {
                list.Add(GetRoot(item));
            }

            return Content(list.ToJson());
        }

        #region XAdmin  Menu   ===========================

        public ActionResult GetMenuForXAdmin()
        {
            string _userId = null;
            try
            {
                //未登陆前会抛异常  为将对象引用到实例
                _userId = OperatorProvider.Provider.GetCurrent().UserId;
            }
            catch (Exception)
            {
                return Content("");
            }

            var list = new List<SysModule>();
            var data = moduleApp.LoadFuncMenuByUserId(_userId); //获取所有的ParentId
            if (data == null)
            {
                return Content("");
            }

            foreach (var item in data)
            {
                list.Add(GetRoot(item)); //获取根节点（此处需优化  可减轻数据库负担）
            }

            //拼接根节点的
            foreach (var item in list)
            {
                var href = "javascript:;";
                var icon = "fa fa-gears";
                if (!string.IsNullOrEmpty(item.UrlAddress))
                {
                    href = item.UrlAddress;
                }

                if (!string.IsNullOrEmpty(item.Icon))
                {
                    icon = item.Icon;
                }

                _sb.AppendLine($"<li id = 'menu_{item.Id}'>");
                _sb.Append($"   <a class='dropdown-toggle' href='{href}' ");
                _sb.Append($"      data-id='{item.ParentId}'> ");
                _sb.Append($"      <i class='{icon}'></i><span>{item.Name}</span>");
                _sb.Append("       <i class='iconfont nav_right'>&#xe697;</i>");
                _sb.AppendLine("    </a>");
                _sb.AppendLine("    <ul class='sub-menu'>"); //style='display: none;'

                GetSubMenusForXAdmin(_userId, item, 1); //加载子节点

                _sb.AppendLine("    </ul>");
                _sb.AppendLine("</li>");
            }

            return Content(_sb.ToString());
        }

        private void GetSubMenusForXAdmin(string _userId, SysModule item, int menuLevel)
        {
            var funcNodeList = moduleApp.LoadFuncMenu(_userId, item.Id);
            var dataIndex = 0;
            //拼接功能菜单html
            foreach (var funcNode in funcNodeList)
            {
                var href = funcNode.UrlAddress;
                var icon = "<i class='iconfont'>&#xe6a7;</i>";
                if (!string.IsNullOrEmpty(funcNode.UrlAddress))
                {
                    href = funcNode.UrlAddress;
                }

                if (!string.IsNullOrEmpty(funcNode.Icon) && !"&nbsp;".Equals(funcNode.Icon))
                {
                    icon = $"<i class=\"{funcNode.Icon}\"></i>";
                }

                //var t = moduleApp.LoadFuncMenu(_userId, funcNode.Id);
                _sb.Append($"<li id='menu_{funcNode.Id}'>");
                _sb.Append($"  <a class='menuItem'  _href='{Url.Content(href)}'");
                _sb.Append($"     data-id='{funcNode.ParentId}'");
                _sb.Append($"     data-index='{dataIndex}'>");
                _sb.Append(icon);
                _sb.Append($"<cite>{funcNode.Name}</cite>");
                _sb.Append("   </a>");
                //先判断是否有子节点
                if (moduleApp.GetSubMenuItemExist(_userId, funcNode.Id))
                {
                    _sb.AppendLine("    <ul class='sub-menu'>"); //style='display: none;'

                    GetSubMenusForXAdmin(_userId, funcNode, 2); //加载子节点

                    _sb.AppendLine("    </ul>");
                }




                _sb.Append(" </li>");
                _sb.AppendLine();
                dataIndex += 1;
            }
        }

        /// <summary>
        ///  获取功能节点
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="item">根节点</param>
        /// <param name="menuLevel">菜单层级</param>
        private void GetSubMenus(string userId, SysModule item, int menuLevel)
        {
            var funcNodeList = moduleApp.LoadFuncMenu(userId, item.Id);
            var dataIndex = 0;
            //拼接功能菜单html
            foreach (var funcNode in funcNodeList)
            {
                //var t = moduleApp.LoadFuncMenu(_userId, funcNode.Id);
                _sb.Append($"<li id='menu_{funcNode.Id}'>");
                _sb.Append($"  <a class='menuItem'  href='{Url.Content(funcNode.UrlAddress)}'");
                _sb.Append($"     data-id='{funcNode.ParentId}'");
                _sb.Append($"     data-index='{dataIndex}'>");
                _sb.Append(funcNode.Name);
                _sb.Append("   </a>");

                GetSubMenus(userId, funcNode, 2); //加载子节点

                _sb.Append(" </li>");
                _sb.AppendLine();
                dataIndex += 1;
            }
        }

        #endregion

        #region Index Menu  ======================

        public ActionResult GetMenu()
        {
            string _userId = null;
            try
            {
                //未登陆前会抛异常  为将对象引用到实例
                _userId = OperatorProvider.Provider.GetCurrent().UserId;
            }
            catch (Exception)
            {

                return Content("");
            }

            var list = new List<SysModule>();
            var data = moduleApp.LoadFuncMenuByUserId(_userId);
            if (data == null)
            {
                return Content("");
            }
            else
            {

                foreach (var item in data)
                {
                    list.Add(GetRoot(item)); //获取根节点
                }

                //拼接根节点的
                foreach (var item in list)
                {
                    var href = "#";
                    var icon = "fa fa-gears";
                    if (!string.IsNullOrEmpty(item.UrlAddress))
                    {
                        href = item.UrlAddress;
                    }

                    if (!string.IsNullOrEmpty(item.Icon))
                    {
                        icon = item.Icon;
                    }

                    _sb.AppendLine($"<li id = 'menu_{item.Id}'>");
                    _sb.Append($"   <a class='dropdown-toggle' href='{href}' ");
                    _sb.Append($"      data-id='{item.ParentId}'> ");
                    _sb.Append($"      <i class='{icon}'></i><span>{item.Name}</span>");
                    _sb.Append("       <i class='fa fa-angle-right drop-icon'></i>");
                    _sb.AppendLine("    </a>");
                    _sb.AppendLine("    <ul class='submenu'>"); //style='display: none;'

                    GetSubMenus(_userId, item, 1); //加载子节点

                    _sb.AppendLine("    </ul>");
                    _sb.AppendLine("</li>");
                }

            }

            return Content(_sb.ToString());
        }

        #endregion

        /// <summary>
        /// 获取父节点下的功能表单
        /// </summary>
        /// <param name="userId">登陆的Id</param>
        /// <param name="parentId">父节点</param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFuncNodeMenu(string userId, string parentId) //获取所点击的菜单节点
        {

            var data = moduleApp.LoadFuncMenu(userId, parentId);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 递归获取根节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private SysModule GetRoot(SysModule data)
        {

            if (data.ParentId != "0")
            {
                sysModule = moduleApp.GetForm(data.ParentId);
                GetRoot(sysModule);
            }

            return sysModule;

        }

        private readonly LayOutRep _lorep = new LayOutRep();

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }


        #region 获取待处理的任务================

        private readonly PmTaskRep _pmrep = new PmTaskRep();

        [HttpPost]
        [HttpAjaxOnly]
        //获取需要提醒的任务
        public ActionResult GetNeedRemindTaskList()
        {
            var datas = _pmrep.GetNeedRemindTaskListByCurrentUserId(OperatorProvider.Provider.GetCurrent().UserId);
            return Content(datas.ToJson());
        }
        [HttpPost]
        public ActionResult updateRemindTask(string ids) {
            List<string> list = new List<string>(ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            if (list.Count > 0) {
                _pmrep.updateRemindTask(list);
            }
            return Json("OK");
        }

        #endregion


        #region 组件处理================
        /// <summary>
        /// Default页面加载组件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public ActionResult Default()
        {
            //获取组件的类型
            var data = _lorep.GetList("");
            return View(data);
        }

        /// <summary>
        /// 获取某种 组件下的所有布局
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
      
        public ActionResult Design(string id)
        {
            var dataHtml = _lorep.GetLayOutHtml(id);
            return View(dataHtml);
        }

        public ActionResult LayoutZones(string id)
        {
            var res = _lorep.GetLayOutHtml(id);
            return View(res);
        }








        #endregion

        #region 获取组件  关闭状态============

        //public ActionResult GetWidgetList()
        //{
        //    var all = AppDomain.CurrentDomain.GetAssemblies()
        //        .SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(Controller))));
        //    var widgetsArray = new JArray();
        //    foreach (var itemWidget in all)
        //    {
        //        var allWidget = itemWidget.GetCustomAttribute(typeof(WidgetAttribute)) as WidgetAttribute;
        //        if (allWidget != null)
        //        {
        //            var widget = new JObject { { "widgetid", allWidget.WidgetKey } , { "widgetTitle", allWidget.WidgetTitle },{ "widgetIcon", allWidget.WidgetIcon }
        //            };
        //            widgetsArray.Add(widget);
        //        };
        //    }

        //    return Content(widgetsArray.ToJson());

        //}
        #endregion


        public ActionResult WidgetTest()
        {
            return View();
        }
    }
}