using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;

namespace Z2.Web.Areas.Pay.Controllers
{
    [ModuleAction(ModuleId = "D8E23F4B - 41F1 - 40F9 - B6D3 - C3D13308BE16", ModuleName ="阿里支付",DisplayNo = 1)]
    public class AliPayController : Controller
    {
        // GET: Pay/AliPay
        public ActionResult Index()
        {
            return View();
        }
    }
}