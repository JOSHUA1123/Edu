using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin
{
    public class QRCodeTools
    {
        /// <summary>
        /// 创建一个qrcode[临时]
        /// </summary>
        /// <param name="parm">参数字符串</param>
        /// <param name="expire_seconds">该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）</param>
        /// <returns></returns>
        public static Image CreateQRCodeTemporary(string parm, int expire_seconds)
        {
            string result = createQRCodeTemp(parm, null, expire_seconds);
            string ticket = Tools.JsonTools.GetIdValue<string>(result, "ticket");
            //二维码图片解析后的地址
            string url = Tools.JsonTools.GetIdValue<string>(result, "url");

            return GetQrCodeImageByTicket(ticket);
        }
        /// <summary>
        /// 创建一个qrcode[临时]
        /// </summary>
        /// <param name="parm">参数int</param>
        /// <param name="expire_seconds">该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）</param>
        /// <returns></returns>
        public static Image CreateQRCodeTemporary(int parm, int expire_seconds)
        {
            string result = createQRCodeTemp(null, parm, expire_seconds);
            string ticket = Tools.JsonTools.GetIdValue<string>(result, "ticket");
            //二维码图片解析后的地址
            string url = Tools.JsonTools.GetIdValue<string>(result, "url");

            return GetQrCodeImageByTicket(ticket);
        }

        private static string createQRCodeTemp(string strParm, int? intParm, int expire_seconds)
        {
            string postData = "{'expire_seconds': " + expire_seconds.ToString() + ", 'action_name': 'QR_STR_SCENE', 'action_info': {'scene': {'scene_str': '" + strParm + "'}}}";
            if (intParm.HasValue)
                postData = "{'expire_seconds': " + expire_seconds.ToString() + ", 'action_name': 'QR_STR_SCENE', 'action_info': {'scene': {'scene_id': '" + intParm + "'}}}";

            var url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + BaseServer.AccessToken.GetAccessToken();

            return Tools.HttpHelperTools.SendPost(url, postData.Replace("'", "\"")).Html;
        }
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns>图片内容[jpg]</returns>
        public static Image GetQrCodeImageByTicket(string ticket)
        {
          return Tools.HttpHelperTools.SendGetImage("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + System.Web.HttpUtility.UrlEncode(ticket));
        }


        /// <summary>
        /// 创建一个qrcode[永久]
        /// </summary>
        /// <param name="parm">参数字符串</param>
        /// <returns></returns>
        public static Image CreateQRCodeForever(string parm)
        {
            string result = createQRCodeForever(parm, null);
            string ticket = Tools.JsonTools.GetIdValue<string>(result, "ticket");
            //二维码图片解析后的地址
            string url = Tools.JsonTools.GetIdValue<string>(result, "url");

            return GetQrCodeImageByTicket(ticket);
        }
        /// <summary>
        /// 创建一个qrcode[永久]
        /// </summary>
        /// <param name="parm">参数int</param>
        /// <returns></returns>
        private static Image CreateQRCodeForever(int parm)
        {
            string result = createQRCodeForever(null, parm);
            string ticket = Tools.JsonTools.GetIdValue<string>(result, "ticket");
            //二维码图片解析后的地址
            string url = Tools.JsonTools.GetIdValue<string>(result, "url");

            return GetQrCodeImageByTicket(ticket);
        }

        private static string createQRCodeForever(string strParm, int? intParm)
        {
            string postData = "{'action_name': 'QR_LIMIT_STR_SCENE', 'action_info': {'scene': {'scene_str': '" + strParm + "'}}}";
            if (intParm.HasValue)
                postData = "{'action_name': 'QR_LIMIT_STR_SCENE', 'action_info': {'scene': {'scene_id': '" + intParm.Value + "'}}}";

            var url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + BaseServer.AccessToken.GetAccessToken();

            return Tools.HttpHelperTools.SendPost(url, postData.Replace("'", "\"")).Html;
        }

    }
}
