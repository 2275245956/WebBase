using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.PM
{
    public class PMAreaRegistration : AreaRegistration, IAppPlugin
    {
        public override string AreaName
        {
            get
            {
                return "PM";
            }
        }
        //[Guid("BFF3465F-0FE1-4380-A7DB-354E8B1BF8B9")]
        public string PlugInId => "BFF3465F-0FE1-4380-A7DB-354E8B1BF8B9";

        public string PlugInName => "PM";

        public string RouteName => "PM_default";

        public string GetUrl(string routes, string controller)
        {
            return routes + "/" + controller + "/Index";
        }


        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PM_default",
                "PM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
               "PM_Image", // ルート名
               "PMDownLoadFile/{controller}/{action}/Files/{taskfileid}/{fileName}", // パラメーター付きの URL
               new { controller = "Task", action = "DownLoadFile", taskfileid = UrlParameter.Optional, filename = @UrlParameter.Optional } // パラメーターの既定値
           );

        }
    }
}