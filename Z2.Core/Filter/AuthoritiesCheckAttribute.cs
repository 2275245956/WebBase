using System;
using System.Web.Mvc;
using MySql.Data;
using Z2.Core.Interface;
using Z2.Core.Operator;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Core.Filter
{
    /// <inheritdoc />
    /// <summary>
    /// 检验用户是否有该功能模块的权限  有则显示  否则返回403
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public partial class AuthoritiesCheckAttribute : ActionFilterAttribute
    {
        public AuthoritiesCheckAttribute(string[] operationCodeName)
        {
            for (int i = 0; i < operationCodeName.Length; i++)
            {
                OperationCode[i] = operationCodeName[i];
            }
        }

        public AuthoritiesCheckAttribute()
        {
            this.OperationCode = new[] { "Index" };
        }
        public AuthoritiesCheckAttribute(bool ignore)//如果ignore未true  则忽略权限
        {
            this.Ignore = ignore;
        }

        private readonly AuthoritiesCheckRep _authCheckRep = new AuthoritiesCheckRep();
        /// <summary>
        /// 操作码的类型   为空默认未Index
        /// Create  Delete Edit Detail
        /// </summary>
        public string[] OperationCode { get; set; }
        /// <summary>
        /// 是否忽略  True  忽略   false   权限判断
        /// </summary>
        public bool Ignore { get; set; }
        private bool _hasAuthotity = false;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Ignore || OperationCode.Length==1&& OperationCode[0] == "Index")  //忽略权限判断 
            {
                _hasAuthotity = true;
            }
            else     //不忽略
            {
                //当前登录的用户id
                var userId = OperatorProvider.Provider.GetCurrent().UserId;
                var moduleId = string.Empty;
                //获取当前请求的控制器模块特性
                var module =
                    filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(ModuleActionAttribute),
                        false);
                foreach (ModuleActionAttribute item in module)
                {
                    moduleId = item.ModuleId;
                }

                //判断该用户是否具有该模块的操作权限
                SysModule sysModule = _authCheckRep.HasAuthorities(userId, moduleId);
                var opCodelist = _authCheckRep.HasOperateAuthorities(userId, moduleId);
                if (OperationCode.Length == 1)//一个Action只有一种操作
                {
                    foreach (var opCode in opCodelist)
                    {
                        if (OperationCode[0].ToLower().Equals(opCode.KeyCode.ToLower()))
                        {
                            _hasAuthotity = true;
                        }
                    }
                }
                else//一个Action有多个操作  只需要判断该操作码 存在即可
                {
                    for (int i = 0; i < OperationCode.Length; i++)
                    {
                        foreach (var opCode in opCodelist)
                        {
                            if (OperationCode[i].ToLower().Equals(opCode.KeyCode.ToLower()))
                            {
                                _hasAuthotity = true;
                            }
                        }
                    }

                }

            }
            if (!_hasAuthotity) //有模块分配 但是没有该操作
            {
                filterContext.Result = new HttpUnauthorizedResult(StringToISO_8859_1("对不起！ 权限不足，请联系管理员！"));
                return;
            }
            if (_hasAuthotity) //有模块分配    有操作
            {
                return;
            }

        }

        //信息提示中文乱码问题
        private string StringToISO_8859_1(string srcText)
        {
            string dst = "";
            char[] src = srcText.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                string str = @"&#" + (int)src[i] + ";";
                dst += str;
            }
            return dst;
        }

        /// <summary>
        /// 返回  无权限
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="url"></param>
        private static void AuthorizeError(ControllerContext filterContext, string url)
        {
            filterContext.HttpContext.Response.Clear(); //清除在返回前已经设置好的标头信息
            filterContext.HttpContext.Response.BufferOutput = true; //设置输出缓冲
            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected) //在跳转之前做判断,防止重复
            {
                // new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.HttpContext.Response.Redirect(url, false);
            }
        }
    }
}
