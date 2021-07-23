using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.SystemManage
{
    public class SystemManageAreaRegistration : AreaRegistration, IAppPlugin
    {
        public override string AreaName
        {
            get
            {
                return "SystemManage";
            }
        }
        public string PlugInId
        {
            get
            {
                return "1887495F-BF0A-40E9-BF25-94E0E576B1C2";
            }
        }

        public string PlugInName
        {
            get
            {
                return "系统管理";
            }
        }

        public string RouteName => "SystemManage_default";
        public string GetUrl(string routes, string controller)
        {
            //string stmp = Assembly.GetExecutingAssembly().Location;
            //stmp = stmp.Substring(0, stmp.LastIndexOf('\\'));//删除文件名
            //return stmp+"/" + routes + "/" + controller + "/Index";
            var virualPath = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            if (!virualPath.EndsWith("/"))
            {
                virualPath += "/";
            }
            return  routes + "/" + controller + "/Index";
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SystemManage_default",
                "SystemManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}