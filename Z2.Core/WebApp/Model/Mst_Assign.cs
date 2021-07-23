using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z2.Core.WebApp.Model
{
    public class Mst_Assign
    {
        public string TaskAssignId { get; set; }
        public string TaskId { get; set; }
        public string AssignTypeId { get; set; }
        public string AssignUserId { get; set; }
        public decimal? EstimatedHours { get; set; }
        
        public DateTime? StartDate { get; set; }
        public string StartDateStr
        {
            get
            {
                return string.Format("{0:yyyy-MM-dd}", StartDate);
            }
        }
        public string StartDateStr1
        {
            get
            {
                return string.Format("{0:yyyy/MM/dd}", StartDate);

            }
        }

        public DateTime? DueDate { get; set; }
        public string DueDateStr
        {
            get
            {
                return string.Format("{0:yyyy-MM-dd}", DueDate);
            }
        }
        public string DueDateStr1
        {
            get
            {
                return string.Format("{0:yyyy/MM/dd}", DueDate);

            }
        }
        public int AssignPercent { get; set; }

        public string AssignPercentStr
        {
            get
            {
                if (this.AssignPercent != 0)
                {
                    return this.AssignPercent + "%";

                }
                else
                {
                    return "0";
                }
            }
        }
        public int? AssignOrder { get; set; }
        public string AssignDescription { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime DeleteTime { get; set; }
        public string DeleteUserId { get; set; }

        public string AssignUserName { get; set; }
        
        public string AssignTypeName
        {
            get; set;
            //get {
            //    switch (this.AssignTypeId) {
            //        case "Dev":return "开发";
            //        case "Test": return "测试";
            //        case "Review": return "代码检查";
            //        case "Architecture": return "设计";
            //        default:return "未知";
            //    }
            //}
        }

        public string AssignTypeCode { get; set; }

        //public int TaskStatus { get; set; }
        public string TaskName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        //public string RealName { get; set; }
        //public string TaskStatusStr
        //{
        //    get
        //    {
        //        switch (this.TaskStatus)
        //        {
        //            case 0: return "草稿";
        //            case 10: return "开发开始";
        //            case 29: return "开发完毕";
        //            case 30: return "测试开始";
        //            case 31: return "测试OK";
        //            case 32: return "测试NG";
        //            case 50: return "代码检查开始";
        //            case 51: return "代码检查OK";
        //            case 52: return "代码检查NG";
        //            case 1000: return "关闭";
        //            default: return "未知";
        //        }
        //    }
        //}
        //public string ItemCode { get; set; }
        //public string ItemName { get; set; }

        public string Type_Description { get
            {
                return this.AssignTypeName + "("+this.AssignDescription+")";

            }
        }

        public int? pm_IsRemind { get; set; }

        public string AssignTypeExtendData { get; set; }

        public string LinkTaskId { get; set; }
    }
}