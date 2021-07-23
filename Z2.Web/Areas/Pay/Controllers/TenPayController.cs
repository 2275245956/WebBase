using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;

namespace Z2.Web.Areas.Pay.Controllers
{
    [ModuleAction(ModuleId = "7A47093B - 0016 - 43D6 - B6E8 - 78E8B5D7F588", ModuleName ="腾讯支付",DisplayNo = 2)]
    public class TenPayController : Controller
    {
        private TenPayV3Info _tenPayV3Info;
        private string TenPayV3_Key;
        // GET: Pay/TenPay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PayMobileOrder(int orderid)
        {
            if (orderid == 0)
            {
                return RedirectToRoute("mobile_default", new { controller = "home", action = "index" });
            }

            var sessionKey = string.Format("order_viewdata_{0}", orderid);
            var viewData = new Dictionary<string, string>();
            if (viewData == null)
            {
                var productName = "在此指定商品名";                
                //if (products.Count > 1)
                //{
                //    productName += "等";
                //}

                var notify_url = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "") + Url.Action("WeiXinMobilePayNotify");
                string UnifiedorderXML;
                decimal SurplusMoney = 0.01m;
                viewData = CreateJsApiView(string.Format("carwash{0}_{1}", DateTime.Now.Ticks, orderid), notify_url, productName, SurplusMoney, out UnifiedorderXML);// orderInfo.SurplusMoney);
                if (viewData == null)
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "创建订单失败";
                    ViewBag.UnifiedorderXML = UnifiedorderXML;
                    return View();
                }

            }

            ViewData["appId"] = viewData["appId"];
            ViewData["timeStamp"] = viewData["timeStamp"];
            ViewData["nonceStr"] = viewData["nonceStr"];
            ViewData["package"] = viewData["package"];
            ViewData["paySign"] = viewData["paySign"];

            ViewData["OrderTitle"] = "订单列表";
            ViewData["OrderListUrl"] = Url.RouteUrl("mobile_default", new { controller = "ucenter", action = "orderlist" });
            ViewData["OrderListFailUrl"] = Url.RouteUrl("mobile_default", new { controller = "ucenter", action = "orderlist", orderState = "WaitPaying" });

            ViewBag.ErrorCode = 0;
            ViewBag.ErrorMsg = "OK";
            ViewBag.UnifiedorderXML = "";

            //===========================
            var appId = "config.WeixinAppId";
            var secret = "config.WeixinAppSecret";

            //获取时间戳
            var timestamp = viewData["timeStamp"];// JSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = viewData["nonceStr"];// JSSDKHelper.GetNoncestr();
            string ticket = JsApiTicketContainer.TryGetJsApiTicket(appId, secret);
            //获取签名
            var url = Request.Url.AbsoluteUri;
            var signature = JSSDKHelper.GetSignature(ticket, nonceStr, timestamp, url);

            ViewData["Config_AppId"] = appId;
            ViewData["Config_TimeStamp"] = timestamp;
            ViewData["Config_NonceStr"] = nonceStr;
            ViewData["Config_Signature"] = signature;

            //===========================

            return View("PayOrder");
        }

        private Dictionary<string, string> CreateJsApiView(string sp_billno, string notify_url, string Name, decimal Price, out string UnifiedorderXML)
        {
#if DEBUG
            //Price = 0.01m;
#endif
            string timeStamp = "";
            string nonceStr = "";
            string paySign = "";
            string openId = "";

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();

            timeStamp = TenPayV3Util.GetTimestamp();
            nonceStr = TenPayV3Util.GetNoncestr();

            //设置package订单参数
            packageReqHandler.SetParameter("appid", _tenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", _tenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("body", Name);    //商品信息
            packageReqHandler.SetParameter("out_trade_no", sp_billno);		//商家订单号
            packageReqHandler.SetParameter("total_fee", string.Format("{0}", Convert.ToInt32(Price * 100)));			        //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.SetParameter("spbill_create_ip", "");   //用户的公网ip，不是商户服务器IP
            packageReqHandler.SetParameter("notify_url", notify_url);		    //接收财付通通知的URL
            packageReqHandler.SetParameter("trade_type", "JSAPI");	                    //交易类型
            packageReqHandler.SetParameter("openid", openId);	                    //用户的openId

            string sign = packageReqHandler.CreateMd5Sign("key", _tenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.Unifiedorder(data);
            UnifiedorderXML = result;
            var res = System.Xml.Linq.XDocument.Parse(result);

            var return_code = res.Element("xml").Element("return_code").Value;
            if (!return_code.ToUpper().Equals("SUCCESS"))
            {
                return null;
            }
            if (res.Element("xml").Element("result_code") != null)
            {
                var result_code = res.Element("xml").Element("result_code").Value;
                if (!result_code.ToUpper().Equals("SUCCESS"))
                {
                    return null;
                }
            }

            string prepayId = res.Element("xml").Element("prepay_id").Value;

            //设置支付参数
            RequestHandler paySignReqHandler = new RequestHandler(null);
            paySignReqHandler.SetParameter("appId", _tenPayV3Info.AppId);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
            paySignReqHandler.SetParameter("signType", "MD5");
            paySign = paySignReqHandler.CreateMd5Sign("key", _tenPayV3Info.Key);

            Dictionary<string, string> viewData = new Dictionary<string, string>();
            viewData["appId"] = _tenPayV3Info.AppId;
            viewData["timeStamp"] = timeStamp;
            viewData["nonceStr"] = nonceStr;
            viewData["package"] = string.Format("prepay_id={0}", prepayId);
            viewData["paySign"] = paySign;

            return viewData;
        }


        public ActionResult WeiXinPayNotify()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;

            resHandler.SetKey(TenPayV3_Key);
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign())
            {
                res = "success";
                string result_code = resHandler.GetParameter("result_code");
                int orderid = 0;
                var tradeno = resHandler.GetParameter("out_trade_no");
                var reg = new System.Text.RegularExpressions.Regex(@"carwash\d+_(\d+)");
                var match = reg.Match(tradeno);
                if (match.Success)
                {
                    orderid = Convert.ToInt32(match.Groups[1].Value);
                }

                if (orderid > 0)
                {
                    string tradeSN = resHandler.GetParameter("transaction_id");//财付通订单号
                    DateTime tradeTime = DateTime.Now;//交易时间
                    //完成订单 

                    //给客户发送信息

                    //等等.....
                                   
                }
            }
            else
            {
                res = "wrong";
                //错误的订单处理
            }

            string xml = string.Format(@"<xml>
                   <return_code><![CDATA[{0}]]></return_code>
                   <return_msg><![CDATA[{1}]]></return_msg>
                </xml>", return_code, return_msg,res);

            return Content(xml, "text/xml");
        }


    }
}