using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z2.WebAPI
{
    public class ErrorCode
    {
        /// <summary>
        /// 错误列表
        /// </summary>
        public static readonly Dictionary<int, string> ErrCodes = new Dictionary<int, string>();

        static ErrorCode()
        {
            ErrCodes.Add(0, "成功");
            ErrCodes.Add(1, "登录失败");
            ErrCodes.Add(2, "未登录");
            ErrCodes.Add(3, "参数不完整");
            ErrCodes.Add(4, "未找到指定数据");
            ErrCodes.Add(5, "操作失败");
        }

        //public static KeyValuePair<int,string> GetCode(int key)
        //{
        //    return new KeyValuePair<int, string>(key, ErrCodes[key]);
        //}
    }

}