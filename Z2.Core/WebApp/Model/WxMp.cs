using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class WxMp
    {
        //[wid]
        public string Wid { get; set; }
        //,[wxName]
        public string WxName { get; set; }

        //,[wxId]
        public string WxId { get; set; }

        //,[yixinId]
        public string YiXinId { get; set; }

        //,[weixinCode]
        public string WeiXinCode { get; set; }

        //,[wxPwd]
        public string WxPwd { get; set; }

        //,[avatar]
        public string Avatar { get; set; }

        //,[apiurl]
        public string ApiUrl { get; set; }
        //,[wxToken]
        public string WxToken { get; set; }

        //,[wxProvince]
        public string WxProvince { get; set; }
        //,[wxCity]
        public string WxCity { get; set; }

        //,[AppId]
        public string AppId { get; set; }
        //,[AppSecret]
        public string AppSecret { get; set; }

        //,[Access_Token]
        public string Access_Token { get; set; }
        //,[openIdStr]
        public string OpenIdStr { get; set; }

        //,[createDate]
        public DateTime? CreateDate { get; set; }

        //,[endDate]
        public DateTime? EndDate { get; set; }
        //,[wenziMaxNum]
        public int? WenziMaxNum { get; set; }

        //,[tuwenMaxNum]

        public int? TuWenMaxNum { get; set; }

        //,[yuyinMaxNum]
        public int? YuYinMaxNum { get; set; }

        //,[wenziDefineNum]
        public int? WenZiDefineNum { get; set; }

        //,[tuwenDefineNum]
        public int? TuWenDefineNum { get; set; }

        //,[yuyinDefineNum]
        public int? YuYinDefineNum { get; set; }

        //,[requestTTNum]
        public int RequestTtNum { get; set; }

        //,[requestUsedNum]
        public int? RequestUsedNum { get; set; }

        //,[smsTTNum]
        public int? SmsTtNum { get; set; }

        //,[smsUsedNum]
        public int? SmsUsedNum { get; set; }

        //,[deleteFlag]
        public bool DeleteFlag { get; set; }

        //,[deleteDate]
        public DateTime? DeleteDate { get; set; }
        //,[wxType]
        public int? WxType { get; set; }
        //,[remark]
        public string Remark { get; set; }
        //,[displayNo]
        public int? DisplayNo { get; set; }
    }
}
