using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Z2.Web.Filter
{
    #region 多语言设置  本地化语言特性
    public class LocalizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在Action执行时获取路由的lang值
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //判断是否为空
            if (filterContext.RouteData.Values["lang"] != null &&
                     !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
            {
                //从路由数据(url)里设置语言
                var lang = filterContext.RouteData.Values["lang"].ToString();
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                //从cookie里读取语言设置
                var cookie = filterContext.HttpContext.Request.Cookies["Language"];
                var langHeader = string.Empty;
                if (cookie != null)
                {
                    //根据cookie设置语言
                    langHeader = cookie.Value;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                else
                {
                    //如果读取cookie失败则设置默认语言
                    langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                //把语言值设置到路由值里
                filterContext.RouteData.Values["lang"] = langHeader;
            }

            /// 把设置保存进cookie
            HttpCookie _cookie = new HttpCookie("Language", Thread.CurrentThread.CurrentUICulture.Name);
            _cookie.Expires = DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.SetCookie(_cookie);

            base.OnActionExecuting(filterContext);

        }
    }
    #endregion

}
