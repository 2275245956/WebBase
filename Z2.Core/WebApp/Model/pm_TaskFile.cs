using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class pm_TaskFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 任务详细Id
        /// </summary>
        public string TaskDetailId { get; set; }
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
        /// 创建任务的用户Id
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 创建文件的Id
        /// </summary>
        public string  CreateUserId { get; set; }
        
    }
}
