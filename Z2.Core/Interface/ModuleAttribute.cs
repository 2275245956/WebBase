using System;

namespace Z2.Core.Interface
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class ModuleActionAttribute : Attribute
    {
        public string ParentModuleId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public bool IsIgnore { get; set; }
        /// <summary>
        /// 手动添加
        /// </summary>
        public bool Manual { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayNo { get; set; }
    }
}
