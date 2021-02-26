using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描   述：                                                    
*│　作   者：joshua                                              
*│　版   本：1.0                                                 
*│　创建时间：2020-03-19 11:29:27                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: MPWeiXin.Model                                   
*│　类   名：UserModel                                      
*└──────────────────────────────────────────────────────────────┘
*/

namespace MPWeiXin.Model
{
    public class UserModel
    {
        public string openId { set; get; }
        public string usernickname { set; get; }
        public int subscribe { set; get; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string subscribe_time { get; set; }
        public string unionid { get; set; }
    }
}
