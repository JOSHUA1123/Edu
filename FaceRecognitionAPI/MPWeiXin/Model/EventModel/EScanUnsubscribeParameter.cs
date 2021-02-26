using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model.EventModel
{
    /// <summary>
    /// 用户未关注时，进行关注后的事件推送
    /// </summary>
    public class EScanUnsubscribeParameter : BaseParameter
    {
        /// <summary>
        /// 	事件类型，subscribe
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}
