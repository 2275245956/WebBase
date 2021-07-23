using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z2.Core.Operator;
using Z2.Core.Web;

namespace Z2.Core.Handler
{
    public class HttpLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HttpLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("nfine_login_error", "overdue");
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Login/Index';</script>");
                return;
            }
            //base.OnAuthorization(filterContext);
        }
    }

}
