using System;
using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.DicMng
{
    public class DicMngAreaRegistration : AreaRegistration , IAppPlugin
    {
        public override string AreaName
        {
            get
            {
                return "DicMng";
            }
        }

        public string PlugInId
        {
            get
            {
                return "A824B5F6-2974-41DD-8887-4CA1C9C68367";
            }
        }

        public string PlugInName
        {
            get
            {
                return "字典管理";
            }
        }

        public string RouteName => "DicMng_default";

        public string GetUrl(string routes, string controller)
        {
            if (controller.Equals("TreeDic"))
                return  routes + "/" + controller + "/CategoryIndex/Area";
            return  routes + "/" + controller + "/CategoryIndex";
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                   "DicMng_default",
                   "DicMng/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
               );
        }
    }
}