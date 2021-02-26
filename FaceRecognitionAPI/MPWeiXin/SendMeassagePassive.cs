using MPWeiXin.Model.EventModel;
using MPWeiXin.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MPWeiXin
{
    /// <summary>
    /// 被动回复用户消息[仅仅当用户发送消息给公众号时]
    /// </summary>
    public class SendMeassagePassive
    {
        /// <summary>
        /// 回复文本消息
        /// </summary>
        /// <param name="text">回复内容</param>
        /// <param name="baseParameter">传入参数即可</param>
        public static void ReplyText(string text, BaseParameter baseParameter)
        {
            WriteText(@"<xml><ToUserName><![CDATA[" + baseParameter.FromUserName + @"]]></ToUserName><FromUserName><![CDATA[" + baseParameter.ToUserName + @"]]></FromUserName><CreateTime>" + DateTimeTools.GetDateTimeInt().ToString() + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + text + "]]></Content></xml>");
        }
        /// <summary>
        /// 回复图片消息
        /// </summary>
        /// <param name="MediaId">MediaId</param>
        /// <param name="baseParameter">传入参数即可</param>
        public static void ReplyImage(string MediaId, BaseParameter baseParameter)
        {
            WriteText(@"<xml><ToUserName><![CDATA[" + baseParameter.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + baseParameter.ToUserName + "]]></FromUserName><CreateTime>" + DateTimeTools.GetDateTimeInt().ToString() + "</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + MediaId + "]]></MediaId></Image></xml>");
        }
        /// <summary>
        /// 回复语音消息
        /// </summary>
        /// <param name="MediaId"></param>
        /// <param name="baseParameter"></param>
        public static void ReplyVoice(string MediaId, BaseParameter baseParameter)
        {
            WriteText("<xml><ToUserName><![CDATA[" + baseParameter.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + baseParameter.ToUserName + "]]></FromUserName><CreateTime>" + DateTimeTools.GetDateTimeInt().ToString() + "</CreateTime><MsgType><![CDATA[voice]]></MsgType><Voice><MediaId><![CDATA[" + MediaId + "]]></MediaId></Voice></xml>");
        }
        /// <summary>
        /// 回复视频消息
        /// </summary>
        /// <param name="MediaId"></param>
        /// <param name="baseParameter"></param>
        public static void ReplyVIdeo(string MediaId, string Title, string Description, BaseParameter baseParameter)
        {
            WriteText("<xml><ToUserName><![CDATA[" + baseParameter.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + baseParameter.ToUserName + "]]></FromUserName><CreateTime>" + DateTimeTools.GetDateTimeInt().ToString() + "</CreateTime><MsgType><![CDATA[video]]></MsgType><Video><MediaId><![CDATA[" + MediaId + "]]]></MediaId><Title><![CDATA[" + Title + "]]></Title><Description><![CDATA[" + Description + "]]></Description></Video></xml>");
        }
        /// <summary>
        /// 回复音乐消息
        /// </summary>
        /// <param name="baseParameter"></param>
        public static void ReplyMusic(string Title, string Description, string MusicUrl, string HQMusicUrl, string ThumbMediaId, BaseParameter baseParameter)
        {
            WriteText("<xml><ToUserName><![CDATA[" + baseParameter.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + baseParameter.ToUserName + "]]></FromUserName><CreateTime>" + DateTimeTools.GetDateTimeInt().ToString() + "</CreateTime><MsgType><![CDATA[music]]></MsgType><Music><Title><![CDATA[" + Title + "]]></Title><Description><![CDATA[" + Description + "]]></Description><MusicUrl><![CDATA[" + MusicUrl + "]]></MusicUrl><HQMusicUrl><![CDATA[" + HQMusicUrl + "]]></HQMusicUrl><ThumbMediaId><![CDATA[" + ThumbMediaId + "]]></ThumbMediaId></Music></xml>");
        }
        /// <summary>
        /// 回复图文消息
        /// </summary>
        /// <param name="baseParameter"></param>
        public static void ReplyNews(int ArticleCount, string Articles, string Title, string Description, string PicUrl, string Url, BaseParameter baseParameter)
        {
            throw new Exception("need change");
            WriteText("<xml><ToUserName><![CDATA[" + baseParameter.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + baseParameter.ToUserName + "]]></FromUserName><CreateTime>" + DateTimeTools.GetDateTimeInt().ToString() + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>"+ ArticleCount .ToString()+ "</ArticleCount><Articles>"+Articles+"<![CDATA["+Title+"]]></Title><Description><![CDATA["+Description+"]]></Description><PicUrl><![CDATA["+PicUrl+"]]></PicUrl><Url><![CDATA[url]]></Url></item><item><Title><![CDATA[title]]></Title><Description><![CDATA[description]]></Description><PicUrl><![CDATA[picurl]]></PicUrl><Url><![CDATA[url]]></Url></item></Articles></xml>");
        }

        /// <summary>
        /// 写入自定义的xml文件
        /// </summary>
        /// <param name="text"></param>
        public static void WriteText(string text)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.Flush();
        }
    }
}
