using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.CO2NET.Extensions;
using Z2.Core.Web;
using Z2.Core.WebApp.Enums;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Core.Configs;
using System.IO;
using Z2.Core.Interface;
using Z2.Core.Operator;
using static Z2.Core.WebApp.Enums.TaskEnum;

namespace Z2.Web.Areas.PM.Controllers
{
    //[Guid("D68FCC2E-9238-47A4-BD15-A2D22CC46145")]
    [ModuleAction(ModuleId = "D68FCC2E-9238-47A4-BD15-A2D22CC46145", ModuleName = "任务详细",DisplayNo = 2, Manual = true)]
    public class TaskDetailController : HandlerLoginInfoController
    {
        private readonly TaskDetailRep _pmTaskDetailRep = new TaskDetailRep();
        private readonly PmTaskRep pmrep = new PmTaskRep();
        // GET: PM/TaskDetail
        //TaskAssignId + '&AssignTypeId=' + AssignTypeId + '&AssignTypeName
        public ActionResult DetailEdit(string TaskId,string TaskDetailId,string TaskAssignId,string AssignUserId)//, string AssignTypeId,string AssignTypeName
        {
            ViewBag.TaskStatusList = new SelectList(Common.PmCommon.GetTaskStatus(), "Key", "Value");
            ViewBag.TaskDetailProcessTypeList = new SelectList(Common.PmCommon.GetTaskDetailProcessType(), "Key", "Value");
            ViewBag.FileTypeList = new SelectList(Common.PmCommon.GetFileType(), "Key", "Value");
            ViewBag.ResultList = new SelectList(Common.PmCommon.GetResList(), "Key", "Value");
            ViewBag.BugTypeList = new SelectList(Common.PmCommon.GetBugList(), "Key", "Value");
            ViewBag.UserInfoList = new SelectList(UserRep.GetUserSelectList(true), "Id", "RealName", AssignUserId);

            var assignList = pmrep.GetAssignList(TaskId);
            assignList.Insert(0, new Mst_Assign { TaskId = TaskId, TaskAssignId = "", AssignTypeName = "未指定" });
            //if (assignList.FindIndex(m => m.TaskAssignId.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString())) < 0)
            //{
            //    assignList.Add(new Mst_Assign { TaskId = TaskId, TaskAssignId = TaskDetailProcessTypeEnum.AnalyzeBug.ToString(), AssignTypeName = "Bug分析" });
            //}
            //if (assignList.FindIndex(m => m.TaskAssignId.Equals(TaskDetailProcessTypeEnum.Redo.ToString())) < 0)
            //{
            //    assignList.Add(new Mst_Assign { TaskId = TaskId, TaskAssignId = TaskDetailProcessTypeEnum.Redo.ToString(), AssignTypeName = "返工" });
            //}
            //if (assignList.FindIndex(m => m.TaskAssignId.Equals(TaskDetailProcessTypeEnum.Close.ToString())) < 0)
            //{
            //    assignList.Add(new Mst_Assign { TaskId = TaskId, TaskAssignId = TaskDetailProcessTypeEnum.Close.ToString(), AssignTypeName = "关闭" });
            //}

            ViewBag.TaskAssignTypeList = new SelectList(assignList, "TaskAssignId", "AssignTypeName");
            ViewBag.TaskAssignId = TaskAssignId;
            ViewBag.AssignUserId = AssignUserId;
            ViewBag.TaskDetailId = TaskDetailId;
            if (!string.IsNullOrEmpty(TaskAssignId))
            {
                var assign = pmrep.GetAssign(TaskAssignId);
                ViewBag.AssignTypeId = assign.AssignTypeId;// AssignTypeId;
                ViewBag.AssignTypeName = assign.AssignTypeName;// AssignTypeName;
            }
            return View();
        }

        /// <summary>
        /// 获取任务详细信息
        /// </summary>
        /// <param name="keyValue">任务Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFormJson(string TaskId, string TaskDetailId, string TaskAssignId)
        {            
            var taskModel = new PmTaskRep().GetTaskData(TaskId);
            if (string.IsNullOrEmpty(TaskDetailId))
            {
                var td = new pm_TaskDetail()
                {
                    TaskPercent = 0,// taskModel.TaskPercent,
                    TaskStatus = taskModel.TaskStatus,
                    Date = DateTime.Now.Date,
                    ProcessType = (int)TaskDetailProcessTypeEnum.Other,
                    //AssignTypeId = taskModel.AssignTypeId,
                    //AssignTypeName = taskModel.AssignTypeName,
                    //TaskAssignId = taskModel.TaskAssignId,
                };
                if (!string.IsNullOrEmpty(TaskAssignId))
                {
                    var assign = pmrep.GetAssign(TaskAssignId);
                    td.TaskAssignId = TaskAssignId;
                    td.AssignUserId = assign.AssignUserId;  //this.CurrentUser.UserId
                    td.AssignTypeId = assign.AssignTypeId;
                    td.AssignTypeName = assign.AssignTypeName;
                }
                return Content(td.ToJson());
            }
            var taskDetail = _pmTaskDetailRep.GetModel(TaskDetailId);
            return Content(taskDetail.ToJson());
        }
        /// <summary>
        /// 获取文件详细信息
        /// </summary>
        /// <param name="TaskId">任务Id</param>
        /// <param name="TaskDetailId">获取任务详细Id</param>
        /// <returns></returns>
        public ActionResult GetFileForm(string TaskId, string TaskDetailId)
        {
            var date = _pmTaskDetailRep.GetModelFile(TaskId, TaskDetailId);
            return Content(date.ToJson());
        }
        [HttpPost]
        public ActionResult GetAssign(string AssignId) {
            var assign = pmrep.GetAssign(AssignId);
            if (!string.IsNullOrEmpty(assign.TaskAssignId))
            {
                var result = new
                {
                    AssignTypeCode = assign.AssignTypeCode,
                    AssignPercent = assign.AssignPercent
                };
                return Content(result.ToJson());
            }
            else
            {
                if (AssignId.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString()) ||
                    AssignId.Equals(TaskDetailProcessTypeEnum.Redo.ToString()) ||
                    AssignId.Equals(TaskDetailProcessTypeEnum.Close.ToString()))
                {
                    var result = new
                    {
                        AssignTypeCode = AssignId,
                        AssignPercent = 0
                    };
                    return Content(result.ToJson());
                }
                else
                {
                    var result = new
                    {
                        AssignTypeCode = string.Empty,
                        AssignPercent = 0
                    };
                    return Content(result.ToJson());
                }
            }
        }


        /// <summary>
        /// 提交任务信息
        /// </summary>
        /// <param name="pmTaskDetail"></param>
        /// <returns></returns>
        public ActionResult SubmitForm(pm_TaskDetail pmTaskDetail, string TaskDetailId, string TaskId)
        {
            var userId = OperatorProvider.Provider.GetCurrent().UserId;

            //if (!string.IsNullOrWhiteSpace(pmTaskDetail.AssignUserId)) {
            //    userId = pmTaskDetail.AssignUserId;
            //}

            pmTaskDetail.TaskDetailId = TaskDetailId;

            #region 计算进度加权平均
            var AssignList = pmrep.GetAssignList(TaskId);
            AssignList.RemoveAll(m => m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.AnalyzeBug.ToString()) ||
                        m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Redo.ToString()) ||
                        m.AssignTypeId.Equals(TaskDetailProcessTypeEnum.Close.ToString()));
            if (AssignList != null && AssignList.Count > 0)
            {
                var count = AssignList.Count();
                //剔除当前修改的指派类型
                AssignList = AssignList.Where(m => m.TaskAssignId != pmTaskDetail.TaskAssignId).ToList();
                var x = 0;
                foreach (var ass in AssignList)
                {
                    x += ass.AssignPercent;
                }
                pmTaskDetail.TaskPercentAve = (pmTaskDetail.TaskPercent + x) / count;
            }

            #endregion


            if (!string.IsNullOrEmpty(pmTaskDetail.TaskContent))
            {
                var desc = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(pmTaskDetail.TaskContent));
                pmTaskDetail.TaskContent = desc;
            }
            //var assign = pmrep.GetAssign(pmTaskDetail.TaskAssignId);            
            var res = _pmTaskDetailRep.SubmitForm(pmTaskDetail, TaskDetailId, TaskId, userId);
            this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { Id = pmTaskDetail });
            return Success("操作成功。");

        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        public ActionResult UpFile(pm_TaskFile ptf)
        {
            ptf.CreateDateTime = DateTime.Now;
            if (Request.Files.Count <= 0)
            {
                return new EmptyResult();
            }
            var saveType = ConfigWrap.GetValue("FileUploadSaveType", 0);
            var uploadPath = ConfigWrap.GetValue("FileUploadPath", "");
            var dt = DateTime.Now;

            for (int i = 0; i < Request.Files.Count;)
            {
                var httpPostedFileBase = Request.Files[i];
                if (httpPostedFileBase != null)
                {
                    if (saveType == 0)
                    {
                        var savePath = Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(),
                            dt.Day.ToString(), ptf.FileName);
                        FileInfo fileInfo = new FileInfo(savePath);
                        if (fileInfo.Directory != null && !fileInfo.Directory.Exists) //获取指示目录是否存在的值
                        {
                            if (fileInfo.DirectoryName != null)
                                Directory.CreateDirectory(fileInfo.DirectoryName); //在指定路径创建所有目录和子目录,获取表示目录的完整路径的字符串
                        }

                        httpPostedFileBase.SaveAs(savePath);
                        ptf.FilePath = savePath;
                    }
                    else
                    {
                        var bytes = new byte[httpPostedFileBase.ContentLength];
                        using (var sr = new BinaryReader(httpPostedFileBase.InputStream))
                        {
                            sr.Read(bytes, 0, bytes.Length);
                        }

                        ptf.FilePath = bytes.ToString();
                    }
                    break;
                }
            }
            ptf.CreateUserId = OperatorProvider.Provider.GetCurrent().UserId.ToString();
            var result = _pmTaskDetailRep.UpFile(ptf);
            ptf.Id = result.Id;
            ptf.CreateUserName = result.CreateUserName;
            ptf.FilePath = Url.RouteUrl("PM_Image", new { controller = "Task", action = "DownLoadFile", taskfileid = ptf.Id, fileName = ptf.FileName.Replace(".", "_") });
            return Success("操作成功。", ptf);

        }
    }
}