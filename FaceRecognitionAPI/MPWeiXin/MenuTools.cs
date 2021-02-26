using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin
{
    public class MenuTools
    {
        public static string SetMenu()
        {
            string json = @"{
    'button': [
        {
            'name': '我们道尔',
            'sub_button': [
                {
                    'type': 'view',
                    'url': 'http://xkd.hn0370.com:1100/Mobileterminal/Index.aspx',
                    'name': '道尔官网'
                },
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/home/ScanQrode',
                    'name': '产品跟踪'
                },
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/home/Developing',
                    'name': '投诉建议'
                }
            ]
        },
        {
            'name': '合作同盟',
            'sub_button': [
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/Shop/index',
                    'name': '产品商城'
                },
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/home/Developing',
                    'name': '合作申请'
                },
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/Home/SearchCertificate',
                    'name': '证书查询'
                },
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/home/MyTeachingVideo',
                    'name': '课程直播'
                }
            ]
        },
        {
            'name': '我的',
            'sub_button': [
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/shop/ListOrder',
                    'name': '我的订单'
                },
                {
                    'type': 'view',
                    'url': 'http://center.doerpaint.com/home/index',
                    'name': '会员中心'
                }
            ]
        }
    ]
}";
            var item = Tools.HttpHelperTools.SendPost("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + BaseServer.AccessToken.GetAccessToken(), json.Replace("'", "\""));
            return item.Html;
        }
    }
}
