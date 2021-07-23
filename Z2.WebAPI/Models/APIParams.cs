using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z2.WebAPI
{
    public class loginInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
    }

    public class Oup_UserInfo
    {
        /// <summary>
        /// GUID主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string DepName { get; internal set; }
    }

}