using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    /// <summary>
    /// 自动回复
    /// </summary>
    public partial class WxAutoReplyRuleRep
    {
        public bool SubmitForm(WxAutoReplyRule wxAutoReplyRule)
        {
            //初始化数据
            wxAutoReplyRule.CreateTime = DateTime.Now;
            wxAutoReplyRule.RuleId = Guid.NewGuid().ToString();

            var sql = @"Insert into [dbo].[wx_autoreply_rule](
                                 [ruleId]
                                ,[wid]
                                ,[rule_type]
                                ,[rule_name]
                                ,[reply_type]
                                ,[create_time]
                                ,[reply_mode]
                                ,[match_mode]
                                ,[rule_content])
                    Values (
                                 @RuleId
                                ,@Wid
                                ,@RuleType
                                ,@RuleName
                                ,@ReplyType
                                ,@CreateTime
                                ,@ReplyMode
                                ,@MatchMode
                                ,@RuleContent
                        )

                    ";
            using (DbUtility db = DbUtility.GetInstance())
            {
                try
                {
                    return db.ExecuteNonQuery(sql, wxAutoReplyRule) > 0;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

    }
}
