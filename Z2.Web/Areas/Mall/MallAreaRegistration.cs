using System;
using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.Mall
{
    public class MallAreaRegistration : AreaRegistration, IAppPlugin
    {
        public override string AreaName
        {
            get
            {
                return "Mall";
            }
        }

        public string PlugInId
        {
            get
            {
                return "BBC3D9F4-7795-49C2-93BD-F76616E8A0B3";
            }
        }

        public string PlugInName
        {
            get
            {
                return "商城";
            }
        }

        public string RouteName => "Mall_default";

        public string GetUrl(string routes, string controller)
        {

            return routes + "/" + controller + "/Index";

        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mall_default",
                "Mall/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}