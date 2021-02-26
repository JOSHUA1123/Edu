using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MPWeiXin.Tools
{
    class JsonTools
    {
        public static T GetIdValue<T>(string source, string JsonCol)
        {
            //System.IO.File.AppendAllText("d:/GetIdValuetext.txt", "\r\n" + source);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic modelDy = jss.Deserialize<dynamic>(source);
            return modelDy[JsonCol];
        }

        public static T GetUserInfo<T>(string source)
        {
            //System.IO.File.AppendAllText("d:/GetIdValuetext.txt", "\r\n" + source);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic modelDy = jss.Deserialize<dynamic>(source);
            return modelDy;
        }
    }
}
