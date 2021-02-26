using MPWeiXin.BaseServer;
using MPWeiXin.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MPWeiXin
{/// <summary>
///获取全局token
/// </summary>
    public class TokenTools
    {
        private static DateTime? LastDateTime = null;
        private static string _AccessToken;
        private static string _QYAccessToken;
        /// <summary>
        /// 微信公众号token
        /// </summary>
        public static string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(_AccessToken) || !LastDateTime.HasValue || (DateTime.Now - LastDateTime.Value).TotalMinutes >= 4)
                {
                    _AccessToken = GetAccessToken();
                    LastDateTime = DateTime.Now;
                }
                return _AccessToken;
            }
        }

        /// <summary>
        /// 企业微信token
        /// </summary>
        public static string QYAccessToken
        {
            get
            {
                //if (LastDateTime.Value == null || string.IsNullOrEmpty(LastDateTime.ToString()))
                //{
                //    _QYAccessToken = null;
                //}

                if (string.IsNullOrEmpty(_QYAccessToken) || !LastDateTime.HasValue || (DateTime.Now - LastDateTime.Value).TotalMinutes >= 4)
                    {
                        _QYAccessToken = GetQYAccessToken();
                        LastDateTime = DateTime.Now;
                    }
                    return _QYAccessToken;
                
            }
        }

        private static string GetAccessToken()                   
        {
            var result = HttpHelperTools.SendGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", BaseModel.appid, BaseModel.secret));
            try
            {
                return JsonTools.GetIdValue<string>(result.Html, "access_token");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取企业微信Token
        /// </summary>
        /// <returns></returns>
        public static string GetQYAccessToken()                             
        {
            var result = HttpHelperTools.SendGet(string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", BaseModel.qyappid, BaseModel.qysecret));//wwc964cecd9737ae67,8GJie59_xuWAtNesyqBBlDfa12JDWaF95wAoQlz-WdA

            UserinforTools.TTJS(result.Html);
            UserinforTools.TTJS(JsonTools.GetIdValue<string>(result.Html, "access_token"));
            try
            {
                return JsonTools.GetIdValue<string>(result.Html, "access_token");
            }
            catch
            {
                return null;
            }
        }
    }
}