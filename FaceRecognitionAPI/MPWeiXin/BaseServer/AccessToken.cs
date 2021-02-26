using CsharpHttpHelper;
using MPWeiXin.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.BaseServer
{
    public class AccessToken
    {
        private static string _AccessToken { get; set; }
        private static DateTime _LastAccessTokenDateTime { get; set; }

        public static string GetAccessToken()
        {
            return getAccessToken(false);
        }
        public static string GetAccessToken(bool ReFlushToken)
        {
            return getAccessToken(ReFlushToken);
        }
        private static string getAccessToken(bool ReFlushToken)
        {
            if (string.IsNullOrEmpty(_AccessToken) || (DateTime.Now - _LastAccessTokenDateTime).TotalMinutes >= 7100 || ReFlushToken == true)
            {
                HttpResult result = HttpHelperTools.SendGet("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + BaseModel.appid + "&secret=" + BaseModel.secret);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _AccessToken = JsonTools.GetIdValue<string>(result.Html, "access_token");
                    _LastAccessTokenDateTime = DateTime.Now;
                }
                else
                {
                    throw new Exception("ERROR getAccessToken");
                }
            }

            return _AccessToken;

        }
    }
}
