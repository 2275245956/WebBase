using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;
using static Z2.Core.WebApp.Enums.TaskEnum;

namespace Z2.Web.Areas.PM.Controllers
{
    //[Guid("4F04F297-1EB2-40D4-9F82-7201CADFEE47")]
    [ModuleAction(ModuleId = "4F04F297-1EB2-40D4-9F82-7201CADFEE47", ModuleName = "指派类型",DisplayNo = 4)]
    public class AssignTypeDicController : DicBaseController
    {
        public override string CategoryCode
        {
            get
            {
                return "AssignType";
            }
        }

        //public override ActionResult GetJqGridSelectJson()
        //{
        //    var keyword = "";
        //    var data = Rep.GetItems(this.CategoryCode, keyword);
        //    data.RemoveAll(m => m.ItemCode.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString()) ||
        //                        m.ItemCode.Equals(TaskDetailProcessTypeEnum.Redo.ToString()) ||
        //                        m.ItemCode.Equals(TaskDetailProcessTypeEnum.Close.ToString()));            

        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("<select>");
        //    data.ForEach(m =>
        //    {
        //        if (sb.Length > 0)
        //        {
        //            sb.AppendFormat("<option value='{0}'>{1}</option>", m.Id, m.ItemName);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("<option value='{0}'>{1}</option>", m.Id, m.ItemName);
        //        }
        //    });
        //    sb.AppendLine("</select>");

        //    return Content(sb.ToString());
        //}
    }

    public class AssignTypeDicExtendData
    {
        public int? ProcessType { get; set; }
    }
}