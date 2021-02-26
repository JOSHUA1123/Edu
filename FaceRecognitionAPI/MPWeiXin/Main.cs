using MPWeiXin.Model;
using MPWeiXin.Model.EventModel;
using MPWeiXin.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MPWeiXin
{
    /// <summary>
    /// 主要的驱动类[E开头事件为event N开头为normal]
    /// </summary>
    public class Main
    {
        #region 委托与事件

        /// <summary>
        /// xml接收总委托[可用于过滤器]
        /// </summary>
        public delegate void XMLReceived(MPResult mPResult, BaseParameter baseParameter);
        /// <summary>
        /// NormalMessage Text委托
        /// </summary>
        public delegate void NmText(NmTextParameter nmTextParameter);

        public delegate void NmImage(NmImageParameter nmImageParameter);
        public delegate void NmVoice(NmVoiceParameter nmVoiceParameter);
        public delegate void NmVideo(NmVideoParameter nmVideoParameter);
        public delegate void NmShortvideo(NmShortvideoParameter nmShortvideoParameter);
        public delegate void NmLocation(NmLocationParameter nmLocationParameter);
        public delegate void NmLink(NmLinkParameter nmLinkParameter);

        public delegate void ESubscribe(ESubscribeParameter eSubscribeParameter);
        public delegate void EUnsubscribe(EUnsubscribeParameter eUnsubscribeParameter);
        public delegate void EScanUnsubscribe(EScanUnsubscribeParameter eScanUnsubscribeParameter);
        public delegate void EScansubscribe(EScansubscribeParameter eScansubscribeParameter);
        public delegate void ELatitude(ELatitudeParameter eLatitudeParameter);
        public delegate void EClick(EClickParameter eClickParameter);
        public delegate void EView(EViewParameter eViewParameter);

        /// <summary>
        /// xml接收总事件[可用于过滤器][请谨慎防止回复冲突]
        /// </summary>
        public static event XMLReceived OnXMLReceived;
        /// <summary>
        /// 普通消息-文本事件
        /// </summary>
        public static event NmText OnNmText;
        /// <summary>
        /// 普通消息-图片事件
        /// </summary>
        public static event NmImage OnNmImage;
        /// <summary>
        /// 普通消息-语音事件
        /// </summary>
        public static event NmVoice OnNmVoice;
        /// <summary>
        /// 普通消息-视频事件
        /// </summary>
        public static event NmVideo OnNmVideo;
        /// <summary>
        /// 普通消息-小视频事件
        /// </summary>
        public static event NmShortvideo OnNmShortvideo;
        /// <summary>
        /// 普通消息-地理位置
        /// </summary>
        public static event NmLocation OnNmLocation;
        /// <summary>
        /// 普通消息-链接事件
        /// </summary>
        public static event NmLink OnNmLink;


        /// <summary>
        /// 事件推送-关注
        /// </summary>
        public static event ESubscribe OnESubscribe;
        /// <summary>
        /// 事件推送-取消关注
        /// </summary>
        public static event EUnsubscribe OnEUnsubscribe;
        /// <summary>
        /// 事件推送-扫码带参数二维码-未关注时，进行关注后的事件推送
        /// </summary>
        public static event EScansubscribe OnEScansubscribe;
        /// <summary>
        /// 事件推送-扫码带参数二维码-已关注时的事件推送
        /// </summary>
        public static event EScanUnsubscribe OnEScanUnsubscribe;
        /// <summary>
        /// 事件推送-上报地理
        /// </summary>
        public static event ELatitude OnELatitude;
        /// <summary>
        /// 事件推送-点击菜单拉取消息时
        /// </summary>
        public static event EClick OnEClick;
        /// <summary>
        /// 事件推送-点击菜单跳转链接时
        /// </summary>
        public static event EView OnEView;

        #endregion

        private static bool _HasXMLData = false;
        /// <summary>
        /// 初始化 置于主页方法中[OnLoad/Action] 事件绑定请在页面载入之前绑定[放置于global中]
        /// </summary>
        public static void HandleEvent()
        {
            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;
            byte[] bts = new byte[Request.InputStream.Length];
            int pos = Request.InputStream.Read(bts, 0, (int)Request.InputStream.Length);
            string data = Encoding.UTF8.GetString(bts, 0, pos);
            if (pos > 0)
            {
                _HasXMLData = true;
                var MsgTypeData = XMLHelper.GetNodeValue(data, "MsgType"); //还有file等
                MsgType msgType = MsgTypeData.ToLower() == "event" ? MsgType.EventPush : MsgType.NormalMessage;
                MPResult mPResult = new MPResult(msgType, data);

                 BaseParameter baseParameter = new BaseParameter()
                {
                    CreateTime = XMLHelper.GetNodeIntValue(data, "CreateTime"),
                    FromUserName = XMLHelper.GetNodeValue(data, "FromUserName"),
                    ToUserName = XMLHelper.GetNodeValue(data, "ToUserName")
                };

                OnXMLReceived?.Invoke(mPResult, baseParameter);

                if (mPResult.MsgTypeData == MsgType.NormalMessage)
                {
                    switch (mPResult.NormalMessageType)
                    {
                        case NormalMessage.Text:
                            OnNmText?.Invoke(FillOtherValueByXML<NmTextParameter>(baseParameter, data));
                            break;
                        case NormalMessage.Image:
                            OnNmImage?.Invoke(FillOtherValueByXML<NmImageParameter>(baseParameter, data));
                            break;
                        case NormalMessage.Voice:
                            OnNmVoice?.Invoke(FillOtherValueByXML<NmVoiceParameter>(baseParameter, data));
                            break;
                        case NormalMessage.Video:
                            OnNmVideo?.Invoke(FillOtherValueByXML<NmVideoParameter>(baseParameter, data));
                            break;
                        case NormalMessage.Shortvideo:
                            OnNmShortvideo?.Invoke(FillOtherValueByXML<NmShortvideoParameter>(baseParameter, data));
                            break;
                        case NormalMessage.Location:
                            OnNmLocation?.Invoke(FillOtherValueByXML<NmLocationParameter>(baseParameter, data));
                            break;
                        case NormalMessage.Link:
                            OnNmLink?.Invoke(FillOtherValueByXML<NmLinkParameter>(baseParameter, data));
                            break;
                    }
                }
                else if (mPResult.MsgTypeData == MsgType.EventPush)
                {
                    switch (mPResult.EventPushType)
                    {
                        case EventPush.Subscribe:
                            OnESubscribe?.Invoke(FillOtherValueByXML<ESubscribeParameter>(baseParameter, data));
                            break;
                        case EventPush.Unsubscribe:
                            OnEUnsubscribe?.Invoke(FillOtherValueByXML<EUnsubscribeParameter>(baseParameter, data));
                            break;
                        case EventPush.Scansubscribe:
                            OnEScansubscribe?.Invoke(FillOtherValueByXML<EScansubscribeParameter>(baseParameter, data));
                            break;
                        case EventPush.ScanUnsubscribe:
                            OnEScanUnsubscribe?.Invoke(FillOtherValueByXML<EScanUnsubscribeParameter>(baseParameter, data));
                            break;
                        case EventPush.Latitude:
                            OnELatitude?.Invoke(FillOtherValueByXML<ELatitudeParameter>(baseParameter, data));
                            break;
                        case EventPush.Click:
                            OnEClick?.Invoke(FillOtherValueByXML<EClickParameter>(baseParameter, data));
                            break;
                        case EventPush.View:
                            OnEView?.Invoke(FillOtherValueByXML<EViewParameter>(baseParameter, data));
                            break;
                    }
                }
                else
                {
                    //otherType
                }
                //Response.Write("<xml><ToUserName><![CDATA[o7PGE0sTN77Iz9-UQfwmGDn-YnSI]]></ToUserName><FromUserName><![CDATA[MiniNetTools]]></FromUserName><CreateTime>1537671502</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[你好]]></Content></xml>");
            }
            else
            {
                _HasXMLData = false;
            }
        }
        /// <summary>
        /// 填充所有参数[从xml中寻找]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseParameter"></param>
        /// <returns></returns>
        private static T FillOtherValueByXML<T>(BaseParameter baseParameter, string xml)
            where T : BaseParameter, new()
        {
            T result = new T();
            SetSameValue(result, baseParameter);
            foreach (var prop in result.GetType().GetProperties())
            {
                if (prop.PropertyType.Name.ToLower().Contains("int64"))
                    prop.SetValue(result, XMLHelper.GetNodeInt32Value(xml, prop.Name));
                else if (prop.PropertyType.Name.ToLower().Contains("int"))
                    prop.SetValue(result, XMLHelper.GetNodeIntValue(xml, prop.Name));
                else
                    prop.SetValue(result, XMLHelper.GetNodeValue(xml, prop.Name));
            }
            return result;
        }
        /// <summary>
        /// 设置相同的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="baseParameter"></param>
        private static void SetSameValue<T>(T result, BaseParameter baseParameter) where T : BaseParameter
        {
            foreach (var prop in result.GetType().GetProperties())
            {
                foreach (var proptarget in baseParameter.GetType().GetProperties())
                {
                    if (prop.Name == proptarget.Name)
                    {
                        prop.SetValue(result, proptarget.GetValue(baseParameter));
                    }
                }
            }
        }
        /// <summary>
        /// 是否获取到Xml数据
        /// </summary>
        public static bool HasXMLData { get { return _HasXMLData; } }


    }
}
