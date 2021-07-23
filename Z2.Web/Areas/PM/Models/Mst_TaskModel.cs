using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z2.Web.Areas.PM.Models
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
        [Required(ErrorMessage = "ProjectNameを入力してください。")]
        public string ProjcetName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskName")]
        [Required(ErrorMessage = "TaskNameを入力してください。")]
        public string TaskName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LoopCount")]
        [Required(ErrorMessage = "LoopCountを入力してください。")]
        public int LoopCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskPriority")]
        [Required(ErrorMessage = "TaskPriorityを入力してください。")]
        public int TaskPriority { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskStatus")]
        [Required(ErrorMessage = "TaskStatusを入力してください。")]
        public int TaskStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskStatusTime")]
        [DataType(DataType.Date)]
        public string TaskStatusTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskDescription")]
        public string TaskDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskPercent")]
        public byte TaskPercent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "StartDate")]
        [DataType(DataType.Date)]
        public string StartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DueDate")]
        [DataType(DataType.Date)]
        public string DueDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "EstimatedHours")]
        public decimal EstimatedHours { get; set; }
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
        [Display(Name = "TestUserId")]
        public string TestUserId { get; set; }
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
        public string CreateDateTime { get; set; }
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
        public string UpdateDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DeleteFlag")]
        [Required(ErrorMessage = "DeleteFlagを入力してください。")]
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DeleteTime")]
        [DataType(DataType.Date)]
        public string DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "DeleteUserId")]
        public string DeleteUserId { get; set; }

        public string btnName { get; set; }

    }
}