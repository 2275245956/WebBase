using System;
using System.Web.Mvc;
using Z2.Core;

namespace Z2.Web.Areas.Pay
{
    public class PayAreaRegistration : AreaRegistration, IAppPlugin
    {
        public override string AreaName 
        {
            get 
            {
                return "Pay";
            }
        }

        public string PlugInId
        {
            get
            {
                return "61790450-3574-4004-8696-A38F4EAB7C84";
            }
        }

        public string PlugInName
        {
            get
            {
                return "支付接口";
            }
        }

        public string RouteName => "Pay_default";

        public string GetUrl(string routes, string controller)
        {
            //string stmp = Assembly.GetExecutingAssembly().Location;
            //stmp = stmp.Substring(0, stmp.LastIndexOf('\\'));//删除文件名
            //return stmp+"/" + routes + "/" + controller + "/Index";
            return routes + "/" + controller + "/Index";
        }
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Pay_default",
                "Pay/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}