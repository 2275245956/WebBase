using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core
{
    public interface IAppPlugin
    {
        string PlugInId { get; }
        string PlugInName { get; }
        /*
         2018/11/6  范文强添加
             */
        /// <summary>
        /// 路由名
        /// </summary>
        string RouteName { get; }
        /// <summary>
        /// 返回完整的路径  主要区别Action
        /// </summary>
        /// <param name="routes">模块</param>
        /// <param name="controller">控制器</param>
        /// <returns></returns>
        string GetUrl(string routes, string controller);
    }
}
