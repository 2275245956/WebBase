using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Model.Mall
{
    public enum OrderState
    {
        /// <summary>
        /// 已提交
        /// </summary>
        Submitted = 10,
        /// <summary>
        /// 等待付款
        /// </summary>
        WaitPaying = 30,
        /// <summary>
        /// 确认中
        /// </summary>
        Confirming = 50,
        /// <summary>
        /// 已确认
        /// </summary>
        Confirmed = 70,
        /// <summary>
        /// 备货中（派单）
        /// </summary>
        PreProducting = 90,
        /// <summary>
        /// 已发货(服务中)
        /// </summary>
        Sended = 110,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 140,
        /// <summary>
        /// 退货
        /// </summary>
        Returned = 160,
        /// <summary>
        /// 锁定
        /// </summary>
        Locked = 180,
        /// <summary>
        /// 取消
        /// </summary>
        Cancelled = 200
    }
}
