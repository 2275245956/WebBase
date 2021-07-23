using System;
using System.Web.Mvc;

namespace Z2.Core.Filter
{
    [AttributeUsage(AttributeTargets.All | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SupportFilterAttribute : ActionFilterAttribute
    {
        public int[] Roles { get; set; }
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (Roles == null)
            {
                return;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}