using System;
using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.FileManage
{
    public class FileManageAreaRegistration : AreaRegistration, IAppPlugin
    {
        public override string AreaName
        {
            get
            {
                return "FileManage";
            }
        }

        public string PlugInId
        {
            get
            {
                return "23B249D2-B397-45C0-BDC7-62B3FA226D8E";
            }
        }

        public string PlugInName
        {
            get
            {
                return "文件管理";
            }
        }

        public string RouteName => "FileManage_default";

        public string GetUrl(string routes, string controller)
        {
            return  routes + "/" + controller + "/Index";
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                 "FileManage_default",
                 "FileManage/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
                "FileManage_GetFile",
                "FileManage/{controller}/{action}/{FileID}/{FileName}",
                new { action = "GetFile", FileName = UrlParameter.Optional }
            );

        }
    }
}