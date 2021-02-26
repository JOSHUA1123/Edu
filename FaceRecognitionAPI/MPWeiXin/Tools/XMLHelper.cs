using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MPWeiXin.Tools
{
    class XMLHelper
    {
        public static string GetNodeValue(string source, string nodeName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(source);
            var documentElement = xmlDocument.DocumentElement;
            var selectSingleNode = (XmlElement)documentElement?.SelectSingleNode("//" + nodeName);
            //if (selectSingleNode != null)
            return selectSingleNode?.FirstChild.InnerText;
        }
        public static int GetNodeIntValue(string source, string nodeName)
        {
            return int.Parse(GetNodeValue(source, nodeName));
        }
        public static long GetNodeInt32Value(string source, string nodeName)
        {
            return long.Parse(GetNodeValue(source, nodeName));
        }
    }
}
