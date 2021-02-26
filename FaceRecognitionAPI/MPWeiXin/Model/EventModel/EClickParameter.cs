using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model.EventModel
{
    public class EClickParameter : BaseParameter
    {
        /// <summary>
        /// 事件类型，CLICK
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
    }
}
