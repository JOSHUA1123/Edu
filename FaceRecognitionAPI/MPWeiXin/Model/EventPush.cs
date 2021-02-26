using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model
{
    public enum EventPush
    {
        /// <summary>
        /// 关注关注事件
        /// </summary>
        Subscribe,
        /// <summary>
        /// 取消关注事件
        /// </summary>
        Unsubscribe,
        /// <summary>
        /// 扫描带参数二维码事件-未关注时，进行关注后的事件推送
        /// </summary>
        ScanUnsubscribe,
        /// <summary>
        /// 扫描带参数二维码事件-已关注时的事件推送
        /// </summary>
        Scansubscribe,
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        Latitude,
        /// <summary>
        /// 点击菜单拉取消息时的事件
        /// </summary>
        Click,
        /// <summary>
        /// 点击菜单跳转链接时的事件
        /// </summary>
        View

    }
}
