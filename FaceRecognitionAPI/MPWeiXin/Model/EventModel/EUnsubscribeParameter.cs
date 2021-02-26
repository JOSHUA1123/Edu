using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model.EventModel
{
    public class EUnsubscribeParameter : BaseParameter
    {
        /// <summary>
        /// 事件类型unsubscribe(取消订阅)
        /// </summary>
        public string Event { get; set; }
    }
}
