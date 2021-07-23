using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class MallStoreRank
    {

        public string Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string SimpleSpelling { get; set; }

        /// <summary>
        /// 店面 图片
        /// </summary>
        public string Avatar { get; set; }

        public int HonestiesLower { get; set; }

        public int HonestiesUpper { get; set; }

        public int ProductCount { get; set; }

        public int DisplayNo { get; set; }

        public bool DeleteFlag { get; set; }

        public bool EnabledFlag { get; set; }

        public string Description { get; set; }

        public DateTime? CreaterTime { get; set; }

        public string CreaterUserId { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public string LastUpdateUserId { get; set; }

        public DateTime? DeleteTime { get; set; }


        public string DeleteUserId { get; set; }


    }
}
