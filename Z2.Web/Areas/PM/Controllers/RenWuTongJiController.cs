using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Web.Areas.PM.Controllers
{
    //[Guid("DDD7EF27-8A05-40DC-8849-351F175A8DD5")]
    [ModuleAction(ModuleId = "DDD7EF27-8A05-40DC-8849-351F175A8DD5", ModuleName = "工作统计", DisplayNo = 6)]
    public class RenWuTongJiController : HandlerLoginInfoController
    {
        private readonly TaskDetailRep _pmTaskDetailRep = new TaskDetailRep();
        private readonly UserRep _userRep = new UserRep();
        public override ActionResult Index()
        {
            //ViewBag.StartDate = new DateTime(DateTime.);
            return View();
        }

        public ActionResult tongji2()
        {
            //ViewBag.StartDate = new DateTime(DateTime.);
            return View();
        }


        #region 参照EHR
        public ActionResult LoadjqData(DateTime SearchData)
        {

            var gridParam = new Pagination();
            TryUpdateModel(gridParam);

            //var EndDate = SearchData.AddMonths(1);
            int days = DateTime.DaysInMonth(SearchData.Year, SearchData.Month);
            var userlist = _pmTaskDetailRep.getUserList2();

            var data = (
                from s in userlist
                select new jqgridTableSource
                {
                    id = s.Id,
                    cell = new List<object>()
                    {
                        s.RealName,
                        s.JiaDongLv,
                        "实际"
                    }
                }).ToArray();

            foreach (var user in data)
            {
                var riqilist = _pmTaskDetailRep.GetRenWuTongJi(SearchData, user.id);
                foreach (var riqi in riqilist) {
                    user.cell.Add(riqi.ActualHours);
                }
            }

            return new ContentResult() { Content = data.ToString() };


        }

        public ActionResult GetOrderHeader(DateTime SearchData) {

            var headers = new List<string>() { "username", "jiadonglv", "setname" };
            var header = new StringBuilder();

            int days = DateTime.DaysInMonth(SearchData.Year, SearchData.Month);

            var dt = 1;
            while (dt <= days)
            {
                headers.Add(string.Format("{0}", dt));
                dt ++;
            }

            foreach (var h in headers)
            {
                if (header.Length > 0)
                {
                    header.Append(",");
                }
                header.AppendFormat(string.Format("'{0}'", h));
            }

            return new ContentResult() { Content = header.ToString() };
        }

        public ActionResult GetOrderColModels(DateTime SearchData)
        {
            var colModels = new StringBuilder();
            colModels.Append(@"
                 { name: 'username', index: 'username', width: 80, fixed: true, align: 'left', sortable: false,title:false,frozen:true, cellattr:prescriptioncalendar_officecellattr}
                ,{ name: 'jiadonglv', index: 'jiadonglv', width: 80, fixed: true, align: 'center', sortable: false,title:false,hidden:true,frozen:true }
                ,{ name: 'setname', index: 'setname', width: 200, fixed: true, align: 'left', sortable: true,title:false,frozen:true,  }
            ");
            int days = DateTime.DaysInMonth(SearchData.Year, SearchData.Month);

            var dt = 1;
            while (dt <= days)
            {
                colModels.AppendFormat(@",{{ name: '{0}', index: '{0}', width: 50, fixed: true, align: 'center', sortable: false, formatter: prescriptioncalendar_formatter,title:false, cellattr:prescriptioncalendar_cellattr }}", dt);
                dt++;
            }
            return new ContentResult() { Content = colModels.ToString() };
        }

        #endregion
        public ActionResult GetGridData(DateTime SearchData) {
            var userlist = _pmTaskDetailRep.getUserList();
            var result = new StringBuilder();
            result.Append("[");
            for (var j = 0; j < userlist.Count; j++) {
                var riqilist = _pmTaskDetailRep.GetRenWuTongJi(SearchData, userlist[j].Id);
                result.Append("{");
                result.Append("\"username\":" + "\"" + userlist[j].RealName + "\",");
                result.Append("\"userId\":" + "\"" + userlist[j].Id + "\",");
                result.Append("\"jiadonglv\":" + "\"" + 0 + "\",");
                result.Append("\"setname\":" + "\"" + "实际" + "\",");
                for (var i = 1; i <= riqilist.Count; i++)
                {
                    result.Append("\"" + i.ToString() + "\":" + "\"" + $"{SearchData.AddDays(i-1):yyyy-MM-dd}" + "_" + formatActualHours(riqilist[i - 1].ActualHours) + "\"");
                    if (i < riqilist.Count)
                    {
                        result.Append(",");
                    }
                }
                if (j == userlist.Count-1)
                {
                    result.Append("}");
                }
                else
                {
                    result.Append("},");
                }
                
            }
            result.Append("]");
            //foreach (var user in userlist) {
                
            //}
            
            var JsonResult = result.ToJson();
            //var result = eval("(" + result + ")")


            return Json(result.ToJson(), JsonRequestBehavior.AllowGet);
        }

        private string formatActualHours(decimal ActualHours)
        {
            if (ActualHours <= 0) return "";
            return $"{ActualHours}";
        }

        public ActionResult GetCloumns(DateTime SearchData)
        {
            //DateTime dtNow = DateTime.Now;
            int days = DateTime.DaysInMonth(SearchData.Year, SearchData.Month);
            var test = new ArrayList();
            test.Add(new
            {
                label = "名称",
                name = "username",
                width = "100"
            });
            test.Add(new
            {
                label = "userId",
                name = "userId",
                width = "100",
                hidden = true
            });

            test.Add(new
            {
                label = "工时率",
                name = "jiadonglv",
                width = "80",
                align = "right",
            });
            test.Add(new
            {
                label = "分类",
                name = "setname",
                width = "50"
            });
            var tmpData = SearchData;
            for (var i = 1; i <= days; i++)
            {
                test.Add(new
                {
                    label =$"{i}({tmpData:ddd})",
                    name = i.ToString(),
                    width = "35",
                    formatter = "dayformatter",
                    cellattr="daycellattr",
                    align="right",
                });
                tmpData = tmpData.AddDays(1);
            }

            var result = new
            {
                colModel = test
            };
            //return Content(result.ToJson());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGridData2(DateTime SearchData)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var riqilist = _pmTaskDetailRep.getUserProject(SearchData);

            var result = new
            {
                total = gridParam.GetPageTotal(riqilist.Count()),
                page = gridParam.page,
                records = riqilist.Count(),
                rows = riqilist.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Json(result, JsonRequestBehavior.AllowGet);

            //var result = new StringBuilder();
            //result.Append(riqilist);

            //var JsonResult = result.ToJson();
            ////var result = eval("(" + result + ")")

            //return Json(result.ToJson(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserDateTask(string UserId, DateTime TaskDate)
        {
            var model = new UserDataTask()
            {
                User = _userRep.GetForm(UserId),
                TaskDate=TaskDate,
                TaskDetails = _pmTaskDetailRep.GetUserDateTask(TaskDate, UserId)
            };   
            if (model.TaskDetails != null)
            {
                model.TaskDetails.ForEach(m => {
                    m.TaskFiles = new List<pm_TaskFile>();
                });
            }
            ViewBag.FileTypeList = Common.PmCommon.GetFileType();
            ViewBag.ResultList = Common.PmCommon.GetResList();
            ViewBag.BugTypeList = Common.PmCommon.GetBugList();

            return View(model);
        }



    }

    public class UserDataTask
    {
        public SysUser User { get; set; }

        public DateTime TaskDate { get; set; }
        public List<pm_TaskDetail> TaskDetails { get; set; }
    }

    class jqgridTableSource
    {
        public string id { get; set; }
        public List<object> cell { get; set; }
    }
}