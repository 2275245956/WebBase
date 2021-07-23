using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class SysItemCategory
    {   
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnabledFlag { get; set; }
    }
}
