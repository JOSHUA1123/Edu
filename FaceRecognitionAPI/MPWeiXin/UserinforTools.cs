using MPWeiXin.BaseServer;
using MPWeiXin.Model;
using MPWeiXin.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MPWeiXin
{
    public class UserinforTools
    {
        /// <summary>
        /// 将获取openid后带有openid参数 并去除code 之后跳转[必须在微信浏览器中！]
        /// </summary>
        /// <returns></returns>
        public static void GetOpenIdParameter()
        {
            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;
            //不在微信浏览器中
            if (!Request.UserAgent.Contains("MicroMessenger"))
                return;

            string currurl = Request.Url.AbsoluteUri;
            //System.IO.File.AppendAllText("d:/log.txt", currurl);



            if (Request["code"] != null)
            {
                string code = string.Empty;
                if (Request["code"].IndexOf(",") >= 0)
                {
                    //存在多个 code 值 获取32位这个
                    string[] codes = Request["code"].Split(",".ToCharArray());

                    foreach (var codestr in codes)
                        if (codestr.Length == 32)
                        {
                            code = codestr;
                            break;
                        }
                }
                else if (Request["code"].Length == 32)
                {
                    code = Request["code"];
                }


                if (!string.IsNullOrEmpty(code))
                {
                    var item = Tools.HttpHelperTools.SendGet(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", BaseModel.appid, BaseModel.secret, code));
                   // System.IO.File.AppendAllText("d:/log.txt", Request["code"] + "\r\n");

                    //移除code
                    currurl = currurl.Replace("&code=" + code + "&state=ok", "");
                    currurl = currurl.Replace("code=" + code + "&state=ok&", "");
                    currurl = currurl.Replace("code=" + code + "&state=ok", ""); 
                    string resultUrl = currurl + "&openid=" + Tools.JsonTools.GetIdValue<string>(item.Html, "openid");
                    System.IO.File.AppendAllText("d:/log.txt", "resultUrl:------" + resultUrl + "\r\n");

                  

                    Response.Redirect(resultUrl);
                }

            }


            Response.StatusCode = 301;
            Response.Status = "301 Moved Permanently"; 
            Response.AppendHeader("Location", string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=ok#wechat_redirect", BaseModel.appid, System.Web.HttpUtility.UrlEncode(currurl)));
            Response.AppendHeader("Cache-Control", "no-cache");
            Response.End();



            //  Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=ok#wechat_redirect", BaseModel.appid, System.Web.HttpUtility.UrlEncode(currurl)));

        }

        /// <summary>
        /// 获取用户
        /// </summary>
        public static UserModel GetUserInfo(string openid,string token){
            var item = Tools.HttpHelperTools.SendGet(string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={0}", token, openid));

           return Tools.JsonTools.GetUserInfo<UserModel>(item.Html);
        }


        public static string token = TokenTools.QYAccessToken;//GetQYAccessToken();
        /// <summary>
        /// 将获取企业微信UserId后带有UserId参数 并去除code 之后跳转[必须在微信浏览器中！]
        /// </summary>
        /// <returns></returns>
        public static void GetUserIdParameter()
        {
            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;
            string currurl = Request.Url.AbsoluteUri;
           
            if (Request["code"] != null)
            {
                string code = string.Empty;
                if (Request["code"].IndexOf(",") >= 0)
                {
                    //存在多个 code 值 获取32位这个
                    string[] codes = Request["code"].Split(",".ToCharArray());

                    foreach (var codestr in codes)
                        if (codestr.Length == 32)
                        {
                            code = codestr;
                            break;
                        }
                }
                    code = Request["code"];
               
                if (!string.IsNullOrEmpty(code))
                {
                    var item = Tools.HttpHelperTools.SendGet(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", token, code));
                    TTJS(item.Html);
                    currurl = currurl.Split('?')[0];
                    string resultUrl = currurl + "?UserId=" + Tools.JsonTools.GetIdValue<string>(item.Html, "UserId");
                    TTJS("------resultUrl:" + resultUrl + "------");

                    Response.Redirect(resultUrl);
                }

            }
            Response.StatusCode = 301;
            Response.Status = "301 Moved Permanently"; 
            Response.AppendHeader("Location", string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=ok#wechat_redirect", BaseModel.qyappid, System.Web.HttpUtility.UrlEncode(currurl)));
            Response.AppendHeader("Cache-Control", "no-cache");
            Response.End();
        }

        /// <summary>
        /// 根据code获取UserId（企业员工）或openid（非企业员工）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserIdOrOpenid(string code)
        {
            var item = Tools.HttpHelperTools.SendGet(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", token, code));
            return Tools.JsonTools.GetIdValue<string>(item.Html, "UserId");
        }


        public static string Ticket()
        {
            string tokenUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}", token);
            //var item = Tools.HttpHelperTools.SendGet(string.Format("https://qyapi.weixin.qq.com/cgi-bin/ticket/get?access_token={0}&type=agent_config", token));
            var item = Tools.HttpHelperTools.SendGet(tokenUrl);
            return Tools.JsonTools.GetIdValue<string>(item.Html, "ticket");

        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Source_String"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToLower();
        }


        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
       {
           byte[] b = new byte[4];
           new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
           Random r = new Random(BitConverter.ToInt32(b, 0));
           string s = null, str = custom;
           if (useNum == true) { str += "0123456789"; }
           if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
           if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
           if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
           for (int i = 0; i<length; i++)
           {
               s += str.Substring(r.Next(0, str.Length - 1), 1);
           }
           return s;
       }





        public static void TTJS(string msg)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var wj = DateTime.Now.ToString("yyyyMMdd");
            var pathLog = path + @"\log\" + wj + @"\";
            if (!Directory.Exists(pathLog))
            {
                Directory.CreateDirectory(pathLog);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ", " + msg);
            System.IO.File.AppendAllText(pathLog + DateTime.Now.ToString("yyyyMMddHH") + "_LogJS.txt", sb.ToString());
        }
        public static void TTERRORJS(string msg)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var wj = DateTime.Now.ToString("yyyyMMdd");
            var pathLog = path + @"\log\" + wj + @"\error\";
            if (!Directory.Exists(pathLog))
            {
                Directory.CreateDirectory(pathLog);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ", " + msg);
            System.IO.File.AppendAllText(pathLog + DateTime.Now.ToString("yyyyMMddHH") + "_LogJS.txt", sb.ToString());
        }


    }
}
