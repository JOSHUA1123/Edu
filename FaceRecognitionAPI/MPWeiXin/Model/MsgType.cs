using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model
{
    /// <summary>
    /// 消息类型MsgType
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        NormalMessage,
        /// <summary>
        /// 接收事件推送
        /// </summary>
        EventPush,
        /// <summary>
        /// 未知
        /// </summary>
        Unknow
    }
}
