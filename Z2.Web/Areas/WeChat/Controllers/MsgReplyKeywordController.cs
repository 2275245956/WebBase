using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.WeChat.Controllers
{
    /// <summary>
    /// 关键字回复消息管理
    /// </summary>
    [ModuleAction(ModuleId = "9CB48FC1-66EA-463A-8FF4-56B785D2F915", ModuleName = "关键字回复",ParentModuleId = "F415F732-A95A-43C7-B2DA-5BEA28C0DB6D")]
    public class MsgReplyKeywordController : HandlerLoginInfoController
    {
        // GET: WeChat/MsgReplyKeyword
        public override ActionResult Index()
        {
            return View();
        }
    }
}