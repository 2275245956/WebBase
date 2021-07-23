using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z2.Core.Configs;
using Z2.Core.WebApp.Repository;
using static Z2.Core.WebApp.Enums.TaskEnum;

namespace Z2.Web.Areas.PM.Common
{
    public class PmCommon
    {
        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetTaskPriority()
        {
            return new Dictionary<int, string>()
                {
                    { (int)TaskPriorityEnum.Low,"低"},
                    { (int)TaskPriorityEnum.Ordinary,"普通"},
                    { (int)TaskPriorityEnum.Height,"高"},
                    { (int)TaskPriorityEnum.Urgent,"紧急"},
                };
        }
        /// <summary>
        /// 获取任务类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetTaskCategory()
        {
            return new Dictionary<int, string>()
                {
                    { (int)TaskCategory.demand,"需求"},
                    { (int)TaskCategory.mistake,"错误"},
                    { (int)TaskCategory.survey,"调查"},
                };
        }
        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetTaskStatus()
        {
            return new Dictionary<int, string>()
                    {
                        { (int)TaskStatusEnum.Prepare,"草稿"},
                        { (int)TaskStatusEnum.DevStart,"开发开始"},
                        { (int)TaskStatusEnum.DevComplete,"开发完毕"},
                        { (int)TaskStatusEnum.TestStart,"测试开始"},
                        { (int)TaskStatusEnum.TestOK,"测试OK"},
                        { (int)TaskStatusEnum.TestNG,"测试NG"},
                        { (int)TaskStatusEnum.CodeTestStart,"代码检查开始"},
                        { (int)TaskStatusEnum.CodeTestOK,"代码检查OK"},
                        { (int)TaskStatusEnum.CodeTestNG,"代码检查NG"},
                        { (int)TaskStatusEnum.Close,"关闭"},
                    };
        }

        public static Dictionary<int, string> GetTaskDetailProcessType()
        {
            return new Dictionary<int, string>()
                    {
                        //{ (int)TaskDetailProcessTypeEnum.AddContent,"增加对应内容"},
                        { (int)TaskDetailProcessTypeEnum.Other,"其他"},
                        //{ (int)TaskDetailProcessTypeEnum.Architecture,"文档"},
                        //{ (int)TaskDetailProcessTypeEnum.Dev,"开发"},
                        //{ (int)TaskDetailProcessTypeEnum.Test,"测试"},
                        //{ (int)TaskDetailProcessTypeEnum.CheckCode,"代码检查"},
                        { (int)TaskDetailProcessTypeEnum.AnalyzeBug,"Bug分析"},
                        { (int)TaskDetailProcessTypeEnum.Redo,"返工"},
                        //{ (int)TaskDetailProcessTypeEnum.Close,"关闭"},
                    };
        }
        public static Dictionary<string, string> GetFileType()
        {
            return new Dictionary<string, string>()
            {
                { $"{(int)FileTypeEnum.Uncategorized}","未分类"},
                { $"{(int)FileTypeEnum.GeneralDesign}","概设"},
                { $"{(int)FileTypeEnum.DetailDesign}","详设"},
                { $"{(int)FileTypeEnum.DbFile}","数据库文档"},

            };
        }

        public static Dictionary<string, string> GetResList()
        {

            return new Dictionary<string, string>()
            {
                { $"{(int)ResultTypeEnum.OK}","OK"},
                { $"{(int)ResultTypeEnum.NG}","NG"},
            };

        }

        static SysItemRep _rep = new SysItemRep();
        public static Dictionary<string, string> GetBugList()
        {
            var data = _rep.GetItems("BugType", "");
            Dictionary<string, string> dic = null;
            if (data != null)
            {
                dic = new Dictionary<string, string>();
                for (int i = 0; i < data.Count; i++)
                {
                    dic.Add(data[i].Id,data[i].ItemName);
                }
            }

            return dic;
        }

    }
}