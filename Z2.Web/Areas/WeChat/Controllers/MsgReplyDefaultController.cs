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
    /// 默认回复消息
    /// </summary>
    [ModuleAction(ModuleId = "22072F31-B712-4545-974F-E2277FAD2F59", ModuleName = "默认回复",ParentModuleId = "F415F732-A95A-43C7-B2DA-5BEA28C0DB6D")]
    public class MsgReplyDefaultController : HandlerLoginInfoController
    {
        // GET: WeChat/MsgReplyDefault
        public override ActionResult Index()
        {
            return View();
        }
    }
}