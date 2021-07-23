using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.NeuChar.Context;
using Senparc.NeuChar.Entities;


namespace Z2.Web.Areas.WeChat.CustomMessageHandler
{
    public partial class CustomMessageContext : MessageContext<IRequestMessageBase, IResponseMessageBase>
    {
        public CustomMessageContext()
        {
            base.MessageContextRemoved += CustomMessageContext_MessageContextRemoved;
        }

        void CustomMessageContext_MessageContextRemoved(object sender,
            WeixinContextRemovedEventArgs<IRequestMessageBase, IResponseMessageBase> e)
        {
            var messageContext = e.MessageContext as CustomMessageContext;
            if (messageContext == null)
            {
                return;   //如果正常调用 messageContext不会为null
            }
            //TODO:这里根据需要执行消息过期时候的逻辑
        }
    }
}