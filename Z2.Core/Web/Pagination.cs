using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Web
{
    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        //public int total
        //{
        //    get
        //    {
        //        if (records > 0)
        //        {
        //            return records % this.rows == 0 ? records / this.rows : records / this.rows + 1;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}
        public int GetPageTotal(int rowCount)
        {
            if (rowCount > 0 && rows>0)
            {
                var cnt = rowCount / rows + (rowCount % rows > 0 ? 1 : 0);
                return cnt;
            }
            else
            {
                return 0;
            }
        }

    }
}
