using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class Mst_TaskModel
    {
        public Mst_TaskModel()
        {
            //Display = true;
        }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskId")]
        [Required(ErrorMessage = "TaskIdを入力してください。")]
        public string TaskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LinkTaskId")]
        public string LinkTaskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TeamId")]
        public string TeamId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "ProjectId")]
        [Required(ErrorMessage = "ProjectIdを入力してください。")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [Display(Name = "项目")]
        //[Required(ErrorMessage = "ProjectNameを入力してください。")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "标题")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "{0}を入力してください。")]
        public string TaskName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LoopCount")]
        //[Required(ErrorMessage = "LoopCountを入力してください。")]
        public int LoopCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskPriority")]
        [Required(ErrorMessage = "TaskPriorityを入力してください。")]
        public int TaskPriority { get; set; }
        [Display(Name = "任务类型")]
        public int TaskCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskStatus")]
        //[Required(ErrorMessage = "TaskStatusを入力してください。")]
        public int TaskStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskStatusTime")]
        [DataType(DataType.Date)]
        public DateTime? TaskStatusTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskDescription")]
        public string TaskDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskPercent")]
        public int? TaskPercent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "StartDate")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        public string StartDateStr
        {
            get
            {
                return string.Format("{0:yyyy-MM-dd}", StartDate);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DueDate")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public string DueDateStr
        {
            get
            {
                return string.Format("{0:yyyy-MM-dd}", DueDate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "EstimatedHours")]
        public decimal? EstimatedHours { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "ActualHours")]
        [Required(ErrorMessage = "ActualHoursを入力してください。")]
        public decimal ActualHours { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "ParentId")]
        public string ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DevUserId")]
        public string DevUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DevUserName")]
        public string DevUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TestUserId")]
        public string TestUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TestUserId")]
        public string TestUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "CreateUserId")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "CreateDateTime")]
        [DataType(DataType.Date)]
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "UpdateUserId")]
        public string UpdateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "UpdateDateTime")]
        [DataType(DataType.Date)]
        public DateTime UpdateDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DeleteFlag")]
        //[Required(ErrorMessage = "DeleteFlagを入力してください。")]
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DeleteTime")]
        [DataType(DataType.Date)]
        public DateTime DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DeleteUserId")]
        public string DeleteUserId { get; set; }

        //public string btnName { get; set; }
        public List<pm_TaskDetail> TaskDetails { get; set; }
        public List<pm_TaskFile> TaskFiles { get; set; }

        public string TaskContent { get; set; }
        public string FileName { get; set; }

        //public pm_TaskDetail TaskDetail { get; set; }
        //public pm_TaskFile TaskFile { get; set; }

        public string TaskPriorityStr
        {
            get
            {
                switch (this.TaskPriority)
                {
                    case 0: return "低";
                    case 1: return "普通";
                    case 2: return "高";
                    case 3: return "紧急";
                    default: return "未知";
                }
            }
        }

        public string TaskStatusStr
        {
            get
            {
                switch (this.TaskStatus)
                {
                    case 0: return "草稿";
                    case 10: return "开发开始";
                    case 29: return "开发完毕";
                    case 30: return "测试开始";
                    case 31: return "测试OK";
                    case 32: return "测试NG";
                    case 50: return "代码检查开始";
                    case 51: return "代码检查OK";
                    case 52: return "代码检查NG";
                    case 1000: return "关闭";
                    default: return "未知";
                }
            }
        }

        public string ReviewUserId { get; set; }
        public string ReviewUserName { get; set; }
        //开发工时
        public decimal? DevEstimatedHours { get; set; }
        //review工时
        public decimal? ReviewEstimatedHours { get; set; }
        //测试工时
        public decimal? TestEstimatedHours { get; set; }
        /// <summary>
        /// 开发分配ID
        /// </summary>
        public string DevAssignTypeId { get; set; }
        /// <summary>
        ///Review分配ID
        /// </summary>
        public string ReviewAssignTypeId { get; set; }
        /// <summary>
        /// 测试分配ID
        /// </summary>
        public string TestAssignTypeId { get; set; }
        //指派给
        public string UserName { get; set; }
        //类型
        public int TypeName { get; set; }

        //类型
        public string TypeNameStr
        {
            get
            {
                switch (this.TypeName)
                {
                    case 1: return "开发";
                    case 2: return "Review";
                    case 3: return "测试";

                    default: return "未知";
                }
            }
        }
        //预计工时
        public decimal? WorkTime { get; set; }

        public string AssignUsersStr { get; set; }

        public List<Mst_Assign> Assigns { get; set; }
        //public Mst_Assign Assigns { get; set; }
        //public string TaskAssignId { get; set; }

        //public string AssignTypeName { get; set; }
        //public string AssignTypeId { get; set; }
        //public string AssignTypeCode { get; set; }
        public string AssignsStrFromView { get; set; }
    }

}
