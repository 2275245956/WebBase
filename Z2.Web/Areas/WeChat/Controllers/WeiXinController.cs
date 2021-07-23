using Senparc.Weixin.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;

namespace Z2.Web.Areas.WeChat.Controllers
{
    [ModuleAction(ModuleId = "F415F732-A95A-43C7-B2DA-5BEA28C0DB6D", ModuleName = "微信管理")]
    public class WeiXinController : Controller
    {
        // GET: WeChat/WeiXin
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        public ActionResult MPCheck(Senparc.Weixin.MP.Entities.Request.PostModel postModel, string echostr)
        {
            return Content(echostr); //返回随机字符串则表示验证通过
            //var Token = "zhujingcheng";
            //if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            //{
            //    return Content(echostr); //返回随机字符串则表示验证通过
            //}
            //else
            //{
            //    return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
            //        "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            //}
        }

    }
}