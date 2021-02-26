using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model.EventModel
{
    public class EViewParameter : BaseParameter
    {
        /// <summary>
        /// 事件类型，VIEW
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }
}
