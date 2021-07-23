using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public partial class WxAutoReplyRule

    {
        //[ruleId]
        /// <summary>
        /// 规则ID  自动生成
        /// </summary>
        public string  RuleId { get; set; }
        //,[wid]
        /// <summary>
        /// 微信账号
        /// </summary>
        public string Wid { get; set; }

        //,[rule_type]
        /// <summary>
        /// 规则回复类型    0关注时，1默认，2关键字  
        /// </summary>
        public int  RuleType { get; set; }
        //,[rule_name]
        /// <summary>
        /// 规则回复的名称
        /// </summary>
        public string  RuleName { get; set; }
        //,[reply_type]
        /// <summary>
        /// 回复数据类型
        /// 关注后自动回复和消息自动回复的类型仅支持0:文本（text）、1:图片（img）、
        /// 2:语音（voice）、3:视频（video）
        /// </summary>
        public int  ReplyType { get; set; }
        //,[create_time]
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        //,[reply_mode]
        /// <summary>
        /// 回复模式
        /// 全部回复(reply_all) 还是 只回复随机一条(random_one)
        /// </summary>
        public int ReplyMode { get; set; }
        //,[match_mode]
        /// <summary>
        /// 匹配模式
        /// contain代表消息中含有该关键词即可，equal表示消息内容必须和关键词严格相同
        /// </summary>
        public int  MatchMode { get; set; }
        //,[rule_content]
        /// <summary>
        /// 回复数据内容
        /// 对于文本类型，content是文本内容
        /// 对于图文、图片、语音、视频类型，content是mediaID
        /// 关键字时为关键词内容
        /// </summary>
        public string RuleContent { get; set; }
    }
}
