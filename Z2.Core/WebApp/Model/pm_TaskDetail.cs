using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class pm_TaskDetail
    {
        public DateTime dangyueriqi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskDetailId { get; set; }
        /// <summary>
        /// 任务Id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 循环次数
        /// </summary>
        public int LoopCount { get; set; }

        [Display(Name = "TaskStatus")]
        //[Required(ErrorMessage = "TaskStatusを入力してください。")]
        public int TaskStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "TaskPercent")]
        public int? TaskPercent {
            get
            {
                return _taskPercent;
            }
            set
            {
                if (value.HasValue)
                {
                    _taskPercent = (Math.Max(0, Math.Min(100, value.Value)));
                }
                else
                {
                    _taskPercent = value;
                }
            }
        }

        private int? _taskPercent;

        /// <summary>
        /// 处理类型//1.增加对应内容; 2 其它; 10开发;30测试;1000 关闭
        /// </summary>
        public int ProcessType { get; set; }
        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 实际时间
        /// </summary>
        public decimal ActualHours { get; set; }
        /// <summary>
        /// 任务内容
        /// </summary>
        public string TaskContent { get; set; }
        /// <summary>
        ///  处理结果//0 OK;1 NG
        /// </summary>
        public string ProcessResult { get; set; }
        /// <summary>
        /// 创建任务的用户Id
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建任务的用户Id
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 上传用户Id
        /// </summary>
        public string UpdateUserId { get; set; }
        /// <summary>
        /// 更新者名
        /// </summary>
        public string UpdateUserName { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }


        /// <summary>
        /// 文件ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        //public string TaskId { get; set; }
        /// <summary>
        /// 任务详细Id
        /// </summary>
        //public string TaskDetailId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 是否删除  0 ： 未删除   1 ： 已删除
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 文件下载地址
        /// </summary>
        public string DownloadUrl { get; set; }
        /// <summary>
        /// 文件预览地址
        /// </summary>
        public string ShowFileUrl { get; set; }
        /// <summary>
        /// 文件数据字节数组
        /// </summary>
        public byte[] FileData { get; set; }
        public List<pm_TaskFile> TaskFiles { get; set; }

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
                    case 49: return "测试完毕";
                    case 1000: return "关闭";
                    default: return "未知";
                }
            }
        }

        public string TaskAssignId { get; set; }

        public string AssignTypeId { get; set; }

        public string AssignUserId { get; set; }

        public string AssignTypeName { get; set; }

        public int? TaskPercentAve { get; set; }

    }
}
