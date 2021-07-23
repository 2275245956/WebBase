using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Z2.Core.Handler
{
    public class HttpAuthorizeAttribute : ActionFilterAttribute
    {
        #region 暂时不需要  使用新的AuthoritiesCheckAttribute ===============================
        //public bool Ignore { get; set; }
        //public HttpAuthorizeAttribute(bool ignore = true)
        //{
        //    Ignore = ignore;
        //}
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    //if (OperatorProvider.Provider.GetCurrent().IsSystem)
        //    //{
        //    //    return;
        //    //}
        //    //if (Ignore == false)
        //    //{
        //    //    return;
        //    //}
        //    //if (!this.ActionAuthorize(filterContext))
        //    //{
        //    //    StringBuilder sbScript = new StringBuilder();
        //    //    sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');</script>");
        //    //    filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
        //    //    return;
        //    //}
        //    base.OnActionExecuting(filterContext);
        //}
        ////private bool ActionAuthorize(ActionExecutingContext filterContext)
        ////{
        ////    var operatorProvider = OperatorProvider.Provider.GetCurrent();
        ////    var roleId = operatorProvider.RoleId;
        ////    var moduleId = WebHelper.GetCookie("nfine_currentmoduleid");
        ////    var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
        ////    return new RoleAuthorizeApp().ActionValidate(roleId, moduleId, action);
        ////} 
        #endregion
    }

}
