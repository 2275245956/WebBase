using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.WeChat
{
    public class WeChatAreaRegistration : AreaRegistration, IAppPlugin
    {
        public override string AreaName => "WeChat";

        public string PlugInId => "9C1B81C1-0B9E-4C0E-A97B-119DCA9B9FF7";

        public string PlugInName => "WeChat";

        public string RouteName => "WeChat_default";

        public string GetUrl(string routes, string controller)
        {
            return routes + "/" + controller + "/Index";
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WeChat_default",
                "WeChat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}