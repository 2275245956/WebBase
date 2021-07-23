using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class MallStore
    {
       
        public string Id { get; set; }
        public string ItemCode { get; set; }
        public string  ItemName { get; set; }
        public string SimpleSpelling { get; set; }
        public string Logo { get; set; }
        public Byte State  { get; set; }
        public string  RegionId { get; set; }
        public string StoreRankId { get; set; }
        public string  StoreIndustriesId { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string  QQ { get; set; }
        public string WX { get; set; }
        public Decimal? Depoint { get; set; }
        public Decimal? Sepoint { get; set; }
        public Decimal? Shpoint { get; set; }
        public int? Honesties { get; set; }
        public DateTime? StateEndTime { get; set; }
        public string  Theme { get; set; }
        public string Banner { get; set; }
        public string Announcement { get; set; }
        public Single? GpsX { get; set; }
        public Single? GpsY { get; set; }
        public int? DisplayNo { get; set; }
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
