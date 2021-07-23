using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Web.Areas.WeChat.Controllers
{
    /// <summary>
    /// 关注时回复消息管理
    /// </summary>
    [ModuleAction(ModuleId = "529130B4-E418-4EC0-A706-FE8847234B19", ModuleName = "关注时回复",ParentModuleId = "F415F732-A95A-43C7-B2DA-5BEA28C0DB6D")]
    public class MsgReplySubscribeController : HandlerLoginInfoController
    {
        // GET: WeChat/MsgReplySubscribe
        private readonly WxAutoReplyRuleRep _wxAutoReplyRuleRep = new WxAutoReplyRuleRep();

        public override ActionResult Index()
        {
            return base.Index();
        }

        /// <summary>
        /// 提交 关注回复的信息
        /// </summary>
        /// <param name="wxAutoReplyRule"></param>
        /// <returns></returns>
        public ActionResult SubmitForm(WxAutoReplyRule wxAutoReplyRule)    
        {
            return Content(_wxAutoReplyRuleRep.SubmitForm(wxAutoReplyRule) ? "操作成功" : "操作失败");
        }
    }
}