using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Core.Configs;
using Z2.Core.Handler;
using Z2.Core.Operator;
using static Z2.Core.WebApp.Enums.TaskEnum;
using Z2.Web.Filter;
using Z2.Core.Interface;
using System.IO;
using System.Data;
using Z2.Core.DataBases;
using System.Security.AccessControl;
using Z2.Core.Utility;
using Newtonsoft.Json;
using System.Text;
using Z2.Web.Areas.PM.Common;

namespace Z2.Web.Areas.PM.Controllers
{
    [ModuleAction(ModuleId = "22E4FB9B-F9E3-4EDC-9A5C-43F6DD1D5428", ModuleName = "任务", DisplayNo = 1)]
    //过滤器特性 获取lang值  加载语言资源文件
    [Localization]
    //  [AuthoritiesCheck]
    public class TaskController : HandlerLoginInfoController
    {
        PmTaskRep pmrep = new PmTaskRep();
        private UserRep userApp = new UserRep();
        private readonly TaskDetailRep _pmTaskDetailRep = new TaskDetailRep();

        // GET: PM/Task
        public override ActionResult Index()
        {
            var projects = PmTaskRep.getProjects();
            projects.Insert(0, new Mst_ProjectModel { ProjectId = "", ProjectName = "未指定" });
            ViewBag.ProjectList = new SelectList(projects, "ProjectId", "ProjectName");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);
            ViewBag.Urgent = TaskPriorityEnum.Urgent;
            var types = pmTaskRep.GetTaskStatusType();
            types.Insert(0, new SysItem() { ItemCode = "", ItemName = "未指定" });

            ViewBag.types = new SelectList(types, "ItemCode", "ItemName");
            return View();
        }

        private PmTaskRep pmTaskRep = new PmTaskRep();
        public ActionResult TaskIndex()
        {
            var projects = PmTaskRep.getProjects();
            projects.Insert(0, new Mst_ProjectModel { ProjectId = "", ProjectName = "未指定" });
            ViewBag.ProjectList = new SelectList(projects, "ProjectId", "ProjectName");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);
            ViewBag.Urgent = TaskPriorityEnum.Urgent;
            var types = pmTaskRep.GetTaskStatusType();
            types.Insert(0, new SysItem() { ItemCode = "", ItemName = "未指定" });

            ViewBag.types = new SelectList(types, "ItemCode", "ItemName");

            return View();
        }

        [HttpGet]
        [HttpAjaxOnly]
        public JsonResult GeTasktList(string searchtxt, string ProjectId, string WorkerId, string AssignTypeId, bool isclosed)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            if (searchtxt.StartsWith("#"))
            {
                if (searchtxt.Length > searchtxt.LastIndexOf("#", StringComparison.Ordinal))
                    searchtxt = searchtxt.Substring(searchtxt.LastIndexOf("#", StringComparison.Ordinal) + 1);
            }
            //var currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
            var list = pmrep.GeTasktList(searchtxt, ProjectId, WorkerId, AssignTypeId, isclosed);

            var result = new
            {
                total = gridParam.GetPageTotal(list.Count()),
                page = gridParam.page,
                records = list.Count(),
                rows = list.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TaskPreView(string TaskID,string StartDate,string EndDate)
        {
            var TaskData = new Mst_TaskModel();
            TaskData = new PmTaskRep().GetTaskData(TaskID);
            TaskData.Assigns = pmrep.GetAssignList(TaskID);
            TaskData.Assigns.RemoveAll(m => m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString()) ||
                        m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Redo.ToString()) ||
                        m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Close.ToString()));

            var dt1 = toNullableDateTime(StartDate);
            var dt2 = toNullableDateTime(EndDate);

            TaskData.TaskDetails = new PmTaskRep().GetFileDetail(TaskID, string.Empty, dt1, dt2);
            //TaskData.TaskFiles = new PmTaskRep().GetFileData(TaskID, string.Empty);

            var list = new List<pm_TaskFile>();
            for (int i = 0; i < TaskData.TaskDetails.Count; i++)
            {
                var TaskFileModel = new PmTaskRep().GetFileData(TaskID, TaskData.TaskDetails[i].TaskDetailId);
                TaskData.TaskDetails[i].TaskFiles = TaskFileModel;
            }

            ViewBag.FileTypeList = Common.PmCommon.GetFileType();
            ViewBag.ResultList = Common.PmCommon.GetResList();
            ViewBag.BugTypeList = Common.PmCommon.GetBugList();

            return View(TaskData);
        }

        private DateTime? toNullableDateTime(string dt)
        {
            if (string.IsNullOrEmpty(dt)) return null;
            DateTime rt;
            if (DateTime.TryParse(dt,out rt))
            {
                return rt;
            }
            return null;
        }

        //public ActionResult GetTaskData(string TaskID)
        //{
        //    var TaskData = new Mst_TaskModel();
        //    TaskData = new PmTaskRep().GetTaskData(TaskID);
        //    TaskData.TaskDetails = new PmTaskRep().GetFileDetail(TaskID);
        //    for (int i = 0; i < TaskData.TaskDetails.Count; i++)
        //    {
        //        var TaskFileModel = new PmTaskRep().GetFileData(TaskID, TaskData.TaskDetails[i].TaskDetailId);
        //        TaskData.TaskDetails[i].TaskFiles.Add(TaskFileModel);
        //    }
        //    //for (int i = 0; i < TaskDetail.Count; i++)
        //    //{
        //    //    var TaskFileModel = new pm_TaskFile();
        //    //    TaskFileModel = new PmTaskRep().GetFileData(TaskID, TaskDetail[i].TaskDetailId);
        //    //    if (TaskFileModel.Id != null)
        //    //    {
        //    //        TaskDetail[i].Id = TaskFileModel.Id;
        //    //        TaskDetail[i].FileName = TaskFileModel.FileName;
        //    //        var fileName = TaskFileModel.FileName.Replace(".", "_");
        //    //        TaskDetail[i].FilePath = TaskFileModel.FilePath;
        //    //        TaskDetail[i].DeleteFlag = TaskFileModel.DeleteFlag;
        //    //        TaskDetail[i].DownloadUrl = Url.RouteUrl("PM_Image", new { controller = "Task", action = "DownLoadFile", taskfileid = TaskFileModel.Id, fileName = fileName });
        //    //        TaskDetail[i].ShowFileUrl = Url.RouteUrl("PM_Image", new { controller = "Task", action = "ShowFile", taskfileid = TaskFileModel.Id, fileName = fileName });
        //    //    }
        //    //}
        //    //var result = new
        //    //{
        //    //    Task = Task,
        //    //    TaskDetail = TaskDetail,
        //    //};
        //    //return Content(result.ToJson());
        //    //return View(result);
        //}
        public ActionResult DownLoadFile(string taskfileid, string fileName)
        {
            var TaskFileModel = new PmTaskRep().GetTaskFile(taskfileid);
            var contentType = GetContentType(TaskFileModel.FileName);
            //var path = System.IO.Path.Combine(TaskFileModel.FilePath, TaskFileModel.FileName);
            var path = TaskFileModel.FilePath;
            if (!System.IO.File.Exists(path))
            {
                return new HttpNotFoundResult();
            }
            var sr = new FilePathResult(path, contentType);
            sr.FileDownloadName = TaskFileModel.FileName;
            return sr;
        }
        public ActionResult ShowFile(string taskfileid, string fileName)
        {
            var TaskFileModel = new PmTaskRep().GetTaskFile(taskfileid);
            var contentType = GetContentType(TaskFileModel.FileName);
            var sr = new FilePathResult(TaskFileModel.FilePath, contentType);
            return sr;
        }
        public static string GetContentType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "";
            }
            string contentType = "";
            var lastIndex = fileName.LastIndexOf(".");
            if (lastIndex >= 0)
            {
                contentType = fileName.Substring(lastIndex).ToLower();
            }
            return contentType.Replace(".", "");
        }

        [HttpGet]
        [HttpAjaxOnly]//2019/1/31  增加一个字段   AssignTypeId 来选择类型查询
        public JsonResult GetList(string searchtxt, string ProjectId, string WorkerId, string AssignTypeId, bool isall, bool isclosed)
        {
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            if (searchtxt.StartsWith("#"))
            {
                if (searchtxt.Length > searchtxt.LastIndexOf("#", StringComparison.Ordinal))
                    searchtxt = searchtxt.Substring(searchtxt.LastIndexOf("#", StringComparison.Ordinal) + 1);
            }
            //var currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
            var list = pmrep.GetList(searchtxt, ProjectId, WorkerId, AssignTypeId, isall, isclosed);
            var resultstr = new StringBuilder();
            //var resultPer = 0;
            //var assignList = new List<Mst_Assign>();
            foreach (var task in list)
            {
                var assignList = pmrep.GetAssignList(task.TaskId).Where(m => m.TaskAssignId != "").ToList();
                assignList.RemoveAll(m => m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString()) ||
                                        m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Redo.ToString()) ||
                                        m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Close.ToString()));

                resultstr.Clear();

                for (int i = 0; i < assignList.Count; i++)
                {
                    var assign = assignList[i];
                    resultstr.Append($"{assign.AssignTypeName}/{assign.AssignUserName}/{assign.EstimatedHours}/{assign.AssignPercentStr}");
                    if (i != assignList.Count - 1)
                    {
                        resultstr.AppendLine();
                    }
                    //resultPer += assign.AssignPercent;
                }
                //foreach (var assign in assignList)
                //{
                //    resultstr = assign.AssignTypeName + "/" + assign.AssignUserName + "/" + assign.EstimatedHours + "/" + assign.AssignPercent + "\n";
                //}
                //总进度
                //if (assignList.Count > 0)
                //{
                //    task.TaskPercent = resultPer / assignList.Count;
                //}
                //else {
                //    task.TaskPercent = 0;
                //}
                task.AssignUsersStr = resultstr.ToString();
            }

            var result = new
            {
                total = gridParam.GetPageTotal(list.Count()),
                page = gridParam.page,
                records = list.Count(),
                rows = list.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFile(string FileID)
        {
            var CreateUserId = OperatorProvider.Provider.GetCurrent().UserId;
            var bl = new PmTaskRep().DeleteFile(FileID, "", CreateUserId);
            this.WriteInfo(ResultType.success.ToString(), "删除成功。", new { Id = FileID });
            return Success("删除成功。", FileID);
        }

        //public ActionResult GetTaskEditPartialView(string TaskId)
        //{
        //    Models.Mst_TaskModel mdl = new Models.Mst_TaskModel();
        //    var dicTaskPriority = Common.PmCommon.GetTaskPriority();
        //    var dicTaskStatus = Common.PmCommon.GetTaskStatus();
        //    List<SysUser> dicUserInfo = UserRep.GetUserSelectList(true);
        //    ViewBag.TaskPriority = dicTaskPriority;
        //    ViewBag.TaskStatus = dicTaskStatus;
        //    ViewBag.UserInfo = dicUserInfo;
        //    return View(mdl);
        //}

        //public ActionResult TaskEditPartialView(string TaskId)
        //{
        //    Models.Mst_TaskModel mdl = new Models.Mst_TaskModel();
        //    var dicTaskPriority = Common.PmCommon.GetTaskPriority();
        //    var dicTaskStatus = Common.PmCommon.GetTaskStatus();
        //    List<SysUser> dicUserInfo = UserRep.GetUserSelectList(true);
        //    ViewBag.TaskPriority = dicTaskPriority;
        //    ViewBag.TaskStatus = dicTaskStatus;
        //    ViewBag.UserInfo = dicUserInfo;
        //    return View(mdl);
        //}
        ///PM/Task/CreateTaskView
        public ActionResult CreateTaskView()
        {
            var AssignTypeList = pmrep.GetAssignTypeList();
            ViewBag.AssignType = new SelectList(AssignTypeList, "ItemCode", "ItemName");

            ViewBag.LogUserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);
            ViewBag.ProjectList = new SelectList(PmTaskRep.getProjects(), "ProjectId", "ProjectName");
            ViewBag.TaskPriorityList = new SelectList(Common.PmCommon.GetTaskPriority(), "Key", "Value");
            ViewBag.TaskCatepory = new SelectList(Common.PmCommon.GetTaskCategory(), "Key", "Value");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName");
            ViewBag.TaskStatusList = new SelectList(Common.PmCommon.GetTaskStatus(), "Key", "Value");

            Mst_TaskModel mdl = new Mst_TaskModel();
            var currentUser = OperatorProvider.Provider.GetCurrent();
            mdl.TaskId = Guid.NewGuid().ToString();
            mdl.TaskPriority = (int)TaskPriorityEnum.Ordinary;
            mdl.CreateUserId = currentUser.UserId;//当前用户
            mdl.StartDate = DateTime.Now.Date;
            mdl.TaskPercent = 0;
            return View(mdl);
        }

        public ActionResult GetUserForJqGridSelectJson()
        {
            var data = UserRep.GetUserSelectList(true);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<select>");
            data.ForEach(m =>
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", m.Id, m.RealName);
            });
            sb.AppendLine("</select>");
            return Content(sb.ToString());
        }


        [HttpPost]
        public ActionResult CreateTask(Mst_TaskModel task)
        {
            if (task == null)
            {
                return Error("model is null");
            }
            if (!ModelState.IsValid)
            {
                return Error("请检查数据");
            }
            var currentUser = OperatorProvider.Provider.GetCurrent();//当前用户
            var TaskContent = Request["TaskContent"];//detaile相关
            var FileName = Request["FileName"];//file相关
            var de = new TaskDetailController();
            //task
            task.CreateUserId = currentUser.UserId;
            task.CreateDateTime = DateTime.Now;
            task.TaskId = Guid.NewGuid().ToString();
            task.EstimatedHours = Converts.ToTryDecimal(task.DevEstimatedHours) + Converts.ToTryDecimal(task.ReviewEstimatedHours) + Converts.ToTryDecimal(task.TestEstimatedHours);
            if (!string.IsNullOrEmpty(task.TaskDescription))
            {
                var desc = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(task.TaskDescription));
                task.TaskDescription = desc;
            }
            if (!string.IsNullOrEmpty(task.AssignsStrFromView))
            {
                var assignJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(task.AssignsStrFromView));
                var assignList = JsonConvert.DeserializeObject<List<Mst_Assign>>(assignJson);
                var ordIdx = 0;
                assignList.ForEach(m =>
                {
                    m.TaskId = task.TaskId;
                    m.CreateUserId = task.CreateUserId;
                    m.CreateDateTime = task.CreateDateTime;
                    m.AssignOrder = ordIdx++;
                });

                task.Assigns = assignList;
            }
            if (task.Assigns == null)
            {
                task.Assigns = new List<Mst_Assign>();
            }
            //task.Assigns.Add(new Mst_Assign { AssignTypeId = TaskDetailProcessTypeEnum.AnalyzeBug.ToString(), AssignOrder = 10000, TaskId = task.TaskId, CreateUserId = task.CreateUserId, CreateDateTime = task.CreateDateTime });
            //task.Assigns.Add(new Mst_Assign { AssignTypeId = TaskDetailProcessTypeEnum.Redo.ToString(), AssignOrder = 10001, TaskId = task.TaskId, CreateUserId = task.CreateUserId, CreateDateTime = task.CreateDateTime });
            //task.Assigns.Add(new Mst_Assign { AssignTypeId = TaskDetailProcessTypeEnum.Close.ToString(), AssignOrder = 10002, TaskId = task.TaskId, CreateUserId = task.CreateUserId, CreateDateTime = task.CreateDateTime });

            task.Assigns.Add(new Mst_Assign { AssignTypeId = TaskDetailProcessTypeEnum.Dev.ToString(),AssignUserId = task.DevUserId, AssignOrder = 10000, TaskId = task.TaskId, CreateUserId = task.CreateUserId, CreateDateTime = task.CreateDateTime ,EstimatedHours = task.DevEstimatedHours,StartDate = task.StartDate,DueDate = task.DueDate});
            task.Assigns.Add(new Mst_Assign { AssignTypeId = TaskDetailProcessTypeEnum.Test.ToString(), AssignUserId= task.TestUserId, AssignOrder = 10001, TaskId = task.TaskId, CreateUserId = task.CreateUserId, CreateDateTime = task.CreateDateTime ,EstimatedHours = task.TestEstimatedHours ,StartDate = task.StartDate,DueDate = task.DueDate});
            task.Assigns.Add(new Mst_Assign { AssignTypeId = TaskDetailProcessTypeEnum.Review.ToString(),AssignUserId = task.ReviewUserId, AssignOrder = 10002, TaskId = task.TaskId, CreateUserId = task.CreateUserId, CreateDateTime = task.CreateDateTime ,EstimatedHours = task.ReviewEstimatedHours,StartDate = task.StartDate,DueDate = task.DueDate});
            
            //taskdetaile
            var taskDetail = new pm_TaskDetail();
            taskDetail.TaskId = task.TaskId;
            taskDetail.TaskDetailId = Guid.NewGuid().ToString();
            taskDetail.Date = DateTime.Now.Date;
            taskDetail.TaskContent = TaskContent;
            taskDetail.CreateUserId = task.CreateUserId;
            taskDetail.CreateDateTime = task.CreateDateTime;
            task.TaskDetails = new List<pm_TaskDetail>() { taskDetail };
            var files = Request.Files;
            try
            {
                if (files.Count > 0 && files[0].ContentLength > 0)
                {
                    var file = Request.Files[0];
                    var savePath = GetPath(file.FileName);
                    FileInfo fileInfo = new FileInfo(savePath);
                    if (fileInfo.Directory != null && !fileInfo.Directory.Exists) //获取指示目录是否存在的值
                    {
                        Directory.CreateDirectory(fileInfo.DirectoryName); //在指定路径创建所有目录和子目录,获取表示目录的完整路径的字符串
                    }
                    //taskfile
                    var taskFile = new pm_TaskFile();
                    taskFile.TaskId = task.TaskId;
                    taskFile.TaskDetailId = taskDetail.TaskDetailId;
                    taskFile.Id = Guid.NewGuid().ToString();
                    taskFile.CreateUserId = task.CreateUserId;
                    taskFile.CreateDateTime = task.CreateDateTime;
                    task.TaskFiles = new List<pm_TaskFile>() { taskFile };
                    taskFile.FileName = Path.GetFileName(file.FileName);
                    taskFile.FilePath = savePath;
                    file.SaveAs(savePath);//上传
                }
            }
            catch (Exception e)
            {
                return Error(e.InnerException.ToString());
            }

            //var list = new List<Mst_Assign>();
            //for (int i = 0; i < 3; i++)
            //{
            //    var data = new Mst_Assign();
            //    data.TaskAssignId = Guid.NewGuid().ToString();
            //    data.TaskId = task.TaskId;
            //    if (i == 0)
            //    {
            //        data.AssignTypeId = "Dev";
            //        data.EstimatedHours = task.DevEstimatedHours;
            //        data.AssignUserId = task.DevUserId;
            //    }
            //    else if (i == 1)
            //    {
            //        data.AssignTypeId = "Review";
            //        data.EstimatedHours = task.ReviewEstimatedHours;
            //        data.AssignUserId = task.ReviewUserId;
            //    }
            //    else
            //    {
            //        data.AssignTypeId = "Test";
            //        data.EstimatedHours = task.TestEstimatedHours;
            //        data.AssignUserId = task.TestUserId;
            //    }
            //    data.StartDate = task.StartDate;
            //    data.DueDate = task.DueDate;
            //    data.AssignDescription = task.TaskDescription;
            //    data.CreateUserId = task.CreateUserId;
            //    data.CreateDateTime = DateTime.Now;
            //    data.DeleteFlag = "0";
            //    list.Add(data);
            //}
            //task.Assigns = list;

            try
            {
                var bl = pmrep.CreateTask(task);
                //for (int i = 0; i < task.Assigns.Count; i++)
                //{
                //    bl = pmrep.CreateAssign(task.Assigns[i]);
                //}
                if (bl)
                {
                    ViewBag.state = ResultType.success.ToString();
                    ViewBag.message = "操作成功";
                    ViewBag.TaskId = task.TaskId;
                    return View("_TaskUploadFile");
                }
                else
                {
                    ViewBag.state = ResultType.error.ToString();
                    ViewBag.message = "操作失败";
                    return View("_TaskUploadFile");
                }
            }
            catch (Exception e)
            {
                ViewBag.state = ResultType.error.ToString();
                ViewBag.message = e.Message.ToString();
                return View("_TaskUploadFile");
            }
        }

        public ActionResult EditTask(string TaskId)
        {
            //获取项目；
            var AssignTypeList = pmrep.GetAssignTypeList();
            var models = pmrep.GetAssignDataList(TaskId).ToList();
            ViewBag.AssignType = new SelectList(AssignTypeList, "ItemCode", "ItemName");
            ViewBag.LogUserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);
            ViewBag.ProjectList = new SelectList(PmTaskRep.getProjects(), "ProjectId", "ProjectName");
            ViewBag.TaskPriorityList = new SelectList(Common.PmCommon.GetTaskPriority(), "Key", "Value");
            ViewBag.TaskCatepory = new SelectList(Common.PmCommon.GetTaskCategory(), "Key", "Value");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName");
            ViewBag.TaskStatusList = new SelectList(Common.PmCommon.GetTaskStatus(), "Key", "Value");

            var TaskData = new Mst_TaskModel();
            TaskData = new PmTaskRep().GetTaskData(TaskId);

            var DevList = models.Where(a => a.AssignTypeId == "Dev").ToList();
            var Review = models.Where(a => a.AssignTypeId == "Review").ToList();
            var Test = models.Where(a => a.AssignTypeId == "Test").ToList();
            var AssignDevList = DevList.GroupBy(c => c.CreateDateTime).Select(c => c.First()).ToList();
            var AssignReviewList = Review.GroupBy(c => c.CreateDateTime).Select(c => c.First()).ToList();
            var AssignTestList = Test.GroupBy(c => c.CreateDateTime).Select(c => c.First()).ToList();
            TaskData.Assigns = models;
            if (models.Count > 0)
            {
                if (AssignDevList.Count > 0)
                {
                    TaskData.DevUserId = AssignDevList[0].AssignUserId;
                    TaskData.DevEstimatedHours = AssignDevList[0].EstimatedHours;
                    TaskData.DevAssignTypeId = AssignDevList[0].AssignTypeId;
                }
                if (AssignReviewList.Count > 0)
                {
                    TaskData.ReviewUserId = AssignReviewList[0].AssignUserId;
                    TaskData.ReviewEstimatedHours = AssignReviewList[0].EstimatedHours;
                    TaskData.ReviewAssignTypeId = AssignReviewList[0].AssignTypeId;
                }
                if (AssignTestList.Count > 0)
                {
                    TaskData.TestUserId = AssignTestList[0].AssignUserId;
                    TaskData.TestEstimatedHours = AssignTestList[0].EstimatedHours;
                    TaskData.TestAssignTypeId = AssignTestList[0].AssignTypeId;
                }
            }
            return View(TaskData);
        }

        [HttpPost]
        public ActionResult EditTask(Mst_TaskModel task)
        {
            if (task == null)
            {
                return Error("model is null");
            }
            if (!ModelState.IsValid)
            {
                return Error("请检查数据");
            }
            //var oldTaskData = new PmTaskRep().GetTaskData(task.TaskId);
            var currentUserId = OperatorProvider.Provider.GetCurrent()?.UserId;
            task.UpdateUserId = currentUserId;
            task.UpdateDateTime = DateTime.Now;

            if (!string.IsNullOrEmpty(task.TaskDescription))
            {
                var desc = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(task.TaskDescription));
                task.TaskDescription = desc;
            }
            if (!string.IsNullOrEmpty(task.AssignsStrFromView))
            {
                var assignJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(task.AssignsStrFromView));
                var assignList = JsonConvert.DeserializeObject<List<Mst_Assign>>(assignJson);
                var ordIdx = 0;
                assignList.ForEach(m =>
                {
                    m.TaskId = task.TaskId;
                    m.CreateUserId = task.UpdateUserId;
                    m.CreateDateTime = task.UpdateDateTime;
                    m.UpdateUserId = task.UpdateUserId;
                    m.UpdateDateTime = task.UpdateDateTime;
                    if (!m.DeleteFlag)
                    {
                        m.AssignOrder = ordIdx++;
                    }
                    else
                    {
                        m.AssignOrder = int.MaxValue;
                    }
                });
                task.Assigns = assignList;
            }
            else
            {
                task.Assigns = new List<Mst_Assign>();
            }

            task.EstimatedHours = task.Assigns.Sum(m => m.EstimatedHours);//  Converts.ToTryDecimal(task.DevEstimatedHours) + Converts.ToTryDecimal(task.ReviewEstimatedHours) + Converts.ToTryDecimal(task.TestEstimatedHours);
            //var list = new List<Mst_Assign>();
            //for (int i = 0; i < 3; i++)
            //{
            //    var data = new Mst_Assign();
            //    data.TaskId = task.TaskId;
            //    if (i == 0)
            //    {
            //        data.TaskAssignId = task.DevAssignTypeId;
            //        data.AssignTypeId = "Dev";
            //        data.EstimatedHours = task.DevEstimatedHours;
            //        data.AssignUserId = task.DevUserId;
            //    }
            //    else if (i == 1)
            //    {
            //        data.TaskAssignId = task.ReviewAssignTypeId;
            //        data.AssignTypeId = "Review";
            //        data.EstimatedHours = task.ReviewEstimatedHours;
            //        data.AssignUserId = task.ReviewUserId;
            //    }
            //    else
            //    {
            //        data.TaskAssignId = task.TestAssignTypeId;
            //        data.AssignTypeId = "Test";
            //        data.EstimatedHours = task.TestEstimatedHours;
            //        data.AssignUserId = task.TestUserId;
            //    }
            //    data.StartDate = task.StartDate;
            //    data.DueDate = task.DueDate;
            //    data.AssignDescription = task.TaskDescription;
            //    data.UpdateUserId = task.UpdateUserId;
            //    data.UpdateDateTime = DateTime.Now;
            //    data.DeleteFlag = "0";
            //    list.Add(data);
            //}
            //task.Assigns = list;
            try
            {
                var bl = pmrep.UpdateTask(task);
                if (bl)
                {
                    return Success("操作成功");
                }
                else
                {
                    return Error("操作失败");
                }
            }
            catch (Exception e)
            {
                return Error(e.Message.ToString());
            }
        }

        [HttpPost]
        public ActionResult EditAssign(string assignData)
        {
            var assignList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Mst_Assign>>(assignData);
            var bl = false;
            if (assignData == null)
            {
                return Content("保存失败");
            }
            if (assignList.Count > 0)
            {
                for (int i = 0; i < assignList.Count; i++)
                {
                    assignList[i].CreateUserId = OperatorProvider.Provider.GetCurrent().UserId;
                    assignList[i].CreateDateTime = DateTime.Now;
                    var isNew = assignList[i].TaskAssignId;
                    if (isNew[0].ToString() == "-")//新数据
                    {
                        assignList[i].TaskAssignId = assignList[i].TaskAssignId.Substring(1);
                        //bl = pmrep.CreateAssign(assignList[i]);
                    }
                    else//旧数据
                    {
                        assignList[i].UpdateDateTime = DateTime.Now;
                        assignList[i].UpdateUserId = OperatorProvider.Provider.GetCurrent().UserId;
                        bl = false;// pmrep.EditAssign(assignList[i]);
                    }
                }
            }
            if (bl)
            {
                return Success("保存成功");
            }
            else
            {
                return Content("保存失败");
            }
        }

        //[HttpPost]
        //public ActionResult DeleteAssign(string assignData)
        //{
        //    var bl = false;
        //    var assignList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Mst_Assign>>(assignData);
        //    for (int i = 0; i < assignList.Count; i++)
        //    {
        //        assignList[i].DeleteTime = DateTime.Now;
        //        assignList[i].DeleteUserId = OperatorProvider.Provider.GetCurrent().UserId;
        //        bl = pmrep.DeleteAssign(assignList[i]);
        //    }
        //    if (bl)
        //    {
        //        return Success("保存成功");
        //    }
        //    else
        //    {
        //        return Content("保存失败");
        //    }
        //}

        private string GetPath(string filename)
        {
            var uploadPath = ConfigWrap.GetValue("FileUploadPath", "");
            var dt = DateTime.Now;
            //Server.MapPath("/") + ConfigWrap.GetValue("FileUploadPath", "") + filename;//
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(filename);
            var savePath = Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), newFileName);
            return savePath;
        }

        [HttpPost]
        public ActionResult GetDescription(string TaskId)
        {
            var model = new PmTaskRep().GetTaskData(TaskId);
            return Json(model);
        }
        [HttpGet]
        public ActionResult GetAssignDataList(string taskId)
        {
            if (string.IsNullOrEmpty(taskId))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            //var AssignTypeList = pmrep.GetAssignTypeList();
            //ViewBag.AssignType = new SelectList(AssignTypeList, "ItemCode", "ItemName");
            //ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", OperatorProvider.Provider.GetCurrent()?.UserId);
            var assigns = pmrep.GetAssignDataList(taskId).ToList();
            assigns.RemoveAll(m => m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString()) ||
                m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Redo.ToString()) ||
                m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Close.ToString()));

            var models = assigns.Select(m => new
            {
                id = m.TaskAssignId,
                cell = new
                {
                    AssignId = m.TaskAssignId,
                    m.AssignTypeId,
                    m.AssignTypeName,
                    m.AssignUserId,
                    m.AssignUserName,
                    m.EstimatedHours,
                    StartDate = m.StartDateStr,
                    DueDate = m.DueDateStr,
                    m.AssignPercent,
                    m.AssignDescription,
                    m.AssignOrder
                }
            });


            var result = new
            {
                total = 1,
                page = 1,
                records = models.Count(),
                rows = models,
            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetGUID()
        {
            var GUID = Guid.NewGuid().ToString();
            return Json(GUID);
        }
    }
    public class taskData
    {
        public List<Mst_Assign> AssignList { get; set; }
        public Mst_TaskModel task { get; set; }
    }
}