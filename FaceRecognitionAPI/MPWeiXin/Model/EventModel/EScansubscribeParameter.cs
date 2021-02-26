using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model.EventModel
{
    /// <summary>
    /// 用户已关注时的事件推送
    /// </summary>
    public class EScansubscribeParameter : BaseParameter
    {
        /// <summary>
        /// 	事件类型，SCAN
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}
