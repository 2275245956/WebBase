using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Z2.Core.WebApp.Enums.TaskEnum;

namespace Z2.Core.WebApp.Enums
{
    public class TaskEnum
    {

        /// <summary>
        /// 优先级
        /// 0低 1普通 2高 3紧急
        /// </summary>
        public enum TaskPriorityEnum
        {
            Low = 0,
            Ordinary = 1,
            Height = 2,
            Urgent = 3,
        }
        /// <summary>
        /// 优先级
        /// 0 需求，1 错误 ，2 调查
        /// </summary>
        public enum TaskCategory
        {
            demand = 0,
            mistake = 1,
            survey = 2,
        }
        /// <summary>
        /// 任务状态
        /// 0草稿;10开发开始;29开发完毕;30测试开始;31测试OK 32测试NG 49测试完毕;1000关闭
        /// </summary>
        public enum TaskStatusEnum
        {
            [Display(Name = "草稿")]
            Prepare = 0,
            [Display(Name = "开发开始")]
            DevStart = 10,
            [Display(Name = "开发完毕")]
            DevComplete = 29,
            [Display(Name = "测试开始")]
            TestStart = 30,
            [Display(Name = "测试OK")]
            TestOK = 31,
            [Display(Name = "测试NG")]
            TestNG = 32,
            [Display(Name = "代码检查开始")]
            CodeTestStart = 50,
            [Display(Name = "代码检查OK")]
            CodeTestOK = 51,
            [Display(Name = "代码检查NG")]
            CodeTestNG = 52,
            [Display(Name = "关闭")]
            Close = 1000
        }

        public enum TaskDetailProcessTypeEnum
        {
            AddContent = 1,
            Other = 2,
            Architecture = 3,
            Dev = 10,
            Test = 30,
            Review = 40,
            AnalyzeBug = 50,
            Redo = 999,
            Close = 1000
        }



        /// <summary>
        /// 文档类型
        /// </summary>
        public enum FileTypeEnum
        {
            Uncategorized = 0,//未分类
            GeneralDesign = 1,//概设
            DetailDesign = 2,//详设
            DbFile = 3      //数据库文件
        }

        /// <summary>
        /// 结果类型
        /// </summary>
        public enum ResultTypeEnum
        {

            OK = 0,
            NG = 1,
        }




    }
    public static class TaskView
    {
        public static string ToEnum(int value)
        {
            var result = "";
            switch (value)
            {
                case (int)TaskCategory.demand:
                    result = "需求";
                    break;
                case (int)TaskCategory.mistake:
                    result = "错误";
                    break;
                case (int)TaskCategory.survey:
                    result = "调查";
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
