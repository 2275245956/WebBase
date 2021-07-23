using System.Web;
using System.Web.Mvc;

namespace Z2.Core.Filter
{
    public class LoginAuthorityAttribute : AuthorizeAttribute
    {
        public int AuthorityLevel { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return base.AuthorizeCore(httpContext);
            return AuthorityLevel == 2;//判断条件是否满足，如果是true则继续执行，如果false 则执行HandleUnauthorizedRequest方法

        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            filterContext.HttpContext.Response.Redirect("/LogOn/Index", false);
        }
    }
}