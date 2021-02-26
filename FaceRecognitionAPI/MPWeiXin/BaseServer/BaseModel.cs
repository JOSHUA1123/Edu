using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.BaseServer
{
    public class BaseModel
    {
        /// <summary>
        /// 微信公众号
        /// </summary>
        private static string _appid;
        /// <summary>
        /// 微信
        /// </summary>
        private static string _secret;

        private static string _qyappid;
        private static string _qysecret;

        private static string _agentid;
        private static string _redirect_uri;


        public static string agentid
        {
            get
            {
                if (string.IsNullOrEmpty(_agentid))
                    _agentid = System.Configuration.ConfigurationManager.AppSettings["agentid"];
                return _agentid;
            }
        }

        public static string redirect_uri
        {
            get
            {
                if (string.IsNullOrEmpty(_redirect_uri))
                    _redirect_uri = System.Configuration.ConfigurationManager.AppSettings["redirect_uri"];
                return _redirect_uri;
            }
        }


        public static string appid
        {
            get
            {
                if (string.IsNullOrEmpty(_appid))
                    _appid = System.Configuration.ConfigurationManager.AppSettings["wxappid"];
                return _appid;
            }
        }
        public static string secret
        {
            get
            {
                if (string.IsNullOrEmpty(_secret))
                    _secret = System.Configuration.ConfigurationManager.AppSettings["wxsecret"];

                return _secret;
            }
        }

        public static string qyappid
        {
            get
            {
                if (string.IsNullOrEmpty(_qyappid))
                    _qyappid = System.Configuration.ConfigurationManager.AppSettings["qyappid"];
                return _qyappid;
            }
        }
        public static string qysecret
        {
            get
            {
                if (string.IsNullOrEmpty(_qysecret))
                    _qysecret = System.Configuration.ConfigurationManager.AppSettings["qysecret"];

                return _qysecret;
            }
        }


    }
}
