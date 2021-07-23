using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Z2.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*添加一个路由  多语言设置*/
            routes.MapRoute(
                name: "Localization", // 路由名称
                url: "{lang}/{controller}/{action}/{id}", // 带有参数的 URL
                constraints: new { lang = "zh-CN|en-US|ja-JP" }, //限制可输入的语言项
                defaults: new { lang = "zh-CN", controller = "Home", action = "Index", id = UrlParameter.Optional }//参数默认值
            );




            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
