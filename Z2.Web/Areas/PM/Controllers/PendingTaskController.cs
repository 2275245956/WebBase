using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.PM.Controllers
{
    //[Guid("E339C384-35E2-48BD-8CDB-1135D617EDB8")]
    [Widget(WidgetKey = "E339C384-35E2-48BD-8CDB-1135D617EDB8", WidgetTitle = "待处理任务",WidgetArea = "PM")]
    public class PendingTaskController : WidgetBaseController<PendingTaskViewModel>
    {
        // GET: PM/PendingTask
        public override ActionResult Index(string Id)
        {
            return base.Index(Id);
        }

        public override ActionResult Setting(string Id)
        {
            return base.Setting(Id);
        }

        [HttpPost]
        public override ActionResult SaveSetting(string Id,PendingTaskViewModel model)
        {
            return base.SaveSetting(Id, model);
        }
    }

    public class PendingTaskViewModel: IWidgetModel
    {
        public string WidgetId { get; set; }
        public string WString { get; set; }
        public DateTime WDate { get; set; }
    }
}