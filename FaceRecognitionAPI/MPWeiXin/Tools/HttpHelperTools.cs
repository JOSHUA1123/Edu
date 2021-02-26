using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Tools
{
    public class HttpHelperTools
    {
        public static HttpResult SendGet(string url)
        {
            HttpHelper httpHelper = new HttpHelper();
            HttpResult result = httpHelper.GetHtml(CreateDefalutItemGet(url));
            return result;
        }
        public static Image SendGetImage(string url)
        {
            HttpHelper httpHelper = new HttpHelper();
           return  httpHelper.GetImage(CreateDefalutItemGet(url));
          
        }
        public static HttpResult SendPost(string url, string postData)
        {
            HttpHelper httpHelper = new HttpHelper();
            var item = CreateDefalutItemGet(url);
            item.Method = "Post";
            item.Postdata = postData;
            item.Encoding = Encoding.UTF8;
            item.PostEncoding = Encoding.UTF8; 
            HttpResult result = httpHelper.GetHtml(item);
            return result;
        }

        public static HttpItem CreateDefalutItemGet(string url)
        {
            return new HttpItem() { Allowautoredirect = true, AutoRedirectCookie = true, URL = url, Method = "GET", UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1", Referer = url, Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8",ContentType= "text/html; charset=utf-8" };
        }
    }
}
