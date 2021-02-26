using MPWeiXin.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Model
{
    public class MPResult
    {

        public MsgType? MsgTypeData { get; set; }
        public NormalMessage? NormalMessageType { get; set; }
        public EventPush? EventPushType { get; set; }

        public string Xml { get; set; }

        public MPResult(MsgType msgType, string xmlSource)
        {
            MsgTypeData = msgType;
            Xml = xmlSource;
            CompleteType();
        }
        /// <summary>
        /// 计算内部类型
        /// </summary>
        private void CompleteType()
        {
            if (MsgTypeData == MsgType.NormalMessage)
            {
                var MsgTypeValue = XMLHelper.GetNodeValue(Xml, "MsgType");
                foreach (NormalMessage normalMessage in Enum.GetValues(typeof(NormalMessage)))
                {
                    if (normalMessage.ToString().ToLower() == MsgTypeValue.ToLower())
                    {
                        NormalMessageType = normalMessage;
                        break;
                    }
                }
            }
            else if (MsgTypeData == MsgType.EventPush)
            {
                var MsgTypeValue = XMLHelper.GetNodeValue(Xml, "Event");

                if (MsgTypeValue.ToLower() == "scan".ToLower())
                {
                    EventPushType = EventPush.ScanUnsubscribe;
                }
                else
                    foreach (EventPush eventPushType in Enum.GetValues(typeof(EventPush)))
                    {
                        if (eventPushType.ToString().ToLower() == MsgTypeValue.ToLower())
                        {
                            if (eventPushType == EventPush.Subscribe)
                            {
                                if (XMLHelper.GetNodeValue(Xml, "EventKey").Contains("qrscene_"))
                                {
                                    EventPushType = EventPush.ScanUnsubscribe;
                                    break;
                                }
                            }


                            EventPushType = eventPushType;
                            break;
                        }
                    }
            }
        }
    }
}
