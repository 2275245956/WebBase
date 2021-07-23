using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Core.Web
{

    public class WidgetBaseController<T> : AppBaseController where T: IWidgetModel
    {
        public virtual ActionResult Index(string Id)
        {
            var widgetData = CMS_WidgetBaseRep.GetWidgetData(Id);
            var m = ConvertModel(widgetData);
            return PartialView(m);
        }

        public virtual ActionResult Setting(string Id)
        {
            var widgetData = CMS_WidgetBaseRep.GetWidgetData(Id);
            var m = ConvertModel(widgetData);
            return View(m);
        }

        public virtual T ConvertModel(string widgetData)
        {
            T m = default(T);
            if (!string.IsNullOrEmpty(widgetData))
            {
                try
                {
                    m = JsonConvert.DeserializeObject<T>(widgetData);
                    return m;
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
            return m;
        }

        [HttpPost]
        public virtual ActionResult SaveSetting(string Id,T model)
        {
            string jsonStr = null;
            if (model != null)
            {
                jsonStr = JsonConvert.SerializeObject(model);
            }
            try
            {
                var bl = CMS_WidgetBaseRep.SetWidgetData(Id, jsonStr);
                if (bl)
                {
                    var obj = new ApiResponseMessage { state = ResultType.success.ToString(), message = "OK" };
                    return Content(obj.ToJson());
                }
                else
                {
                    var obj = new ApiResponseMessage { state = ResultType.error.ToString(), message = "Save Failed" };
                    return Content(obj.ToJson());
                }
            }
            catch (Exception ex)
            {
                var obj = new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message };
                return Content(obj.ToJson());
            }
        }
    }
}
