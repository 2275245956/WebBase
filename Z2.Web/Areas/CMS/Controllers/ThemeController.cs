using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Web.Areas.CMS.Controllers
{
    public class ThemeController : HandlerLoginInfoController
    {
        // GET: CMS/Theme
        private readonly CMS_ThemeRep _thenm = new CMS_ThemeRep();


        [HttpPost]
        public ActionResult GetCurrentTheme()
        {
            return Json(Url.Content("~/CmsContent/Themes/Default/css/Theme.min.css"));
        }
    }
}