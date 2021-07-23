using System;
using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.CMS
{
    public class CMSAreaRegistration : AreaRegistration, IAppPlugin
    {

        public override string AreaName => "CMS";

        public string PlugInId => "0D0A2949-93FC-4DAA-ADE9-351A18BE3A3A";

        public string PlugInName => "CMS布局";

        public string RouteName => "CMS_default";

        public string GetUrl(string routes, string controller)
        {
            return routes + "/" + controller + "/Index";
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CMS_default",
                "CMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}