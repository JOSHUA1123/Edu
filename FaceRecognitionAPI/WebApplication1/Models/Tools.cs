using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication1.Models
{
    ///<summary>
    /// 版 本：v1.0.0
    /// 创建人：Joshua
    /// 日 期：2020-04-18 14:12:06
    /// 描 述：
    ///</summary>
    public class Tools
    {

        public static string appId = ConfigurationManager.AppSettings["appId"]; //;"dingdul0a05gzik3v2um";
        public static string appSecret = ConfigurationManager.AppSettings["appSecret"]; //"zU6w54Wg21DieRC3SH7uq8kaIVlKHOP4jsdVGlF2uQTBQ0FNQH83Yx707-AP6DZG";
        public static string keys = ConfigurationManager.AppSettings["keys"];//"sqywqmzb";

        public static string URL= ConfigurationManager.AppSettings["URL"];

        /// <summary>
        /// 日志打印方法
        /// </summary>
        /// <param name="msg"></param>
        public static void TT1(string msg)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var pathLog = path + @"\log\"+DateTime.Now.ToString("yyyyMMddHH")+@"\";
            if (!Directory.Exists(pathLog))
            {
                Directory.CreateDirectory(pathLog);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ", " + msg);
            System.IO.File.AppendAllText(pathLog + DateTime.Now.ToString("yyyyMMddHHmm") + "_Log.txt", sb.ToString());
        }

        public static string GetToken()
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
            OapiGettokenRequest request = new OapiGettokenRequest();
            request.Appkey= appId;
            request.Appsecret= appSecret;
            request.SetHttpMethod("GET");
            OapiGettokenResponse response = client.Execute(request);

            return response.AccessToken;
        }

        public static string GetstaffNO(string uid)
        {
            var staffNO = "";
            var accessToken = GetToken();
            DefaultDingTalkClient clients = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getUseridByUnionid");
            OapiUserGetUseridByUnionidRequest request = new OapiUserGetUseridByUnionidRequest();
            request.Unionid= uid;
            request.SetHttpMethod("GET");
            OapiUserGetUseridByUnionidResponse responses = clients.Execute(request, accessToken);
            if (responses.Userid!=null)
            {
                DefaultDingTalkClient clientss = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get");
                OapiUserGetRequest requests = new OapiUserGetRequest();
                requests.Userid = responses.Userid;
                requests.SetHttpMethod("GET");
                OapiUserGetResponse response = clientss.Execute(requests, accessToken);
                staffNO = response.Jobnumber;
            }



            return staffNO;
        }

        /// <summary>
        /// 把对象转成json字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string Serialize(object o)

        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            json.Serialize(o, sb);
            return sb.ToString();

        }


        /// <summary> 
        /// DES加密 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string DesEncrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary> 
        /// DES解密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string DesDecrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// DES 加密 注意:密钥必须为８位 sqywqmzb
        /// </summary>
        /// <param name="inputString">待加密字符串</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string DesEncrypt1(string inputString, string encryptKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// DES 解密 注意:密钥必须为８位 sqywqmzb
        /// </summary>
        /// <param name="inputString">待解密字符串</param>
        /// <param name="decryptKey">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecrypt1(string inputString, string decryptKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[inputString.Length];
            byKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(inputString);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }

        }

        public static string RandomStr()
        {
            Random random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(chars.Length)]).ToArray());
        }






    }
    public class JsonData
    {
        public int errcode { set; get; }
        public string errmsg { set; get; }
        public userinfoDing user_info { set; get; }

    }
    public class userinfoDing
    {
        public string nick { get; set; }
        public string unionid { get; set; }
        public string dingId { get; set; }
        public string openid { get; set; }
        public bool main_org_auth_high_level { get; set; }
    }
}