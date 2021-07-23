using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Z2.Web.Extend
{
    public static class UrlHelperExtend
    {
        public static string PathContent(this UrlHelper helper, string path)
        {
            return helper.Content(string.IsNullOrEmpty(path) ? "~/" : path);
        }
    }
}