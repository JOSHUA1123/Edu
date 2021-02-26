using MPWeiXin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFace
{
    public partial class Form1 : Form
    {
        //我的百度key
        private static string API_KEY = "hEqnBzOEkoAe4AoKUGSfWsZt";
        private static string SECRET_KEY = "p1j0mDKs2RAYdPn0rDUMVdcb0aazzlp6";

        //正式key
        //private static string API_KEY = "pBmZaAV4U5dGZegXan9wDp67";
        //private static string SECRET_KEY = "VEunEKWOzY1QXTNQHFs9M5zL5Gxfuqin";

        private Baidu.Aip.Face.Face client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
        

        public Form1()
        {
            InitializeComponent();
            client.Timeout = 60000;  // 修改超时时间
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                FileStream fs = File.OpenRead(path); //OpenRead
                int filelength = 0;
                filelength = (int)fs.Length; //获得文件长度 
                Byte[] image = new Byte[filelength]; //建立一个字节数组 
                fs.Read(image, 0, filelength); //按字节流读取 
                                               //System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
                fs.Close();
                string imgData64 = Convert.ToBase64String(image);

                var imageType = "BASE64";  //BASE64   URL
                
                string staffNO = "296";
                var guid = Guid.NewGuid().ToString();
                //注册人脸
                var groupId = "face_01";//face_01
                var userId = staffNO;
                //首先查询是否存在人脸
                var result2 = client.Search(imgData64, imageType, groupId);  //会出现222207（未找到用户）这个错误
                var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(result2);
                var o2 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson) as JObject;
                // 如果有可选参数
                var options = new Dictionary<string, object>{
                            {"user_info", guid}
                        };

                //判断是否存在当前人脸，相识度是否大于80
                if (o2["error_code"].ToString() == "0" && o2["error_msg"].ToString() == "SUCCESS")
                {
                    var result_list = Newtonsoft.Json.JsonConvert.DeserializeObject(o2["result"].ToString()) as JObject;
                    var user_list = result_list["user_list"];
                    var Obj = JArray.Parse(user_list.ToString());
                    foreach (var item in Obj)
                    {
                        //80分以上可以判断为同一人，此分值对应万分之一误识率
                        var score = Convert.ToInt32(item["score"]);
                        if (score > 89)
                        {
                            //var sss=client.UserDelete(groupId, userId, options).ToString();
                        }
                    }
                }

                TTJS("groupId:" + groupId + "userId:" + userId+ "  user_info:"+ guid);
                // 调用人脸注册，可能会抛出网络等异常，请使用try/catch捕获

                // 带参数调用人脸注册
                var resultData = client.UserAdd(imgData64, imageType, groupId, userId, options);

                var result3 = client.Search(imgData64, imageType, groupId);  //会出现222207（未找到用户）这个错误
                var strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(result3);
                var o3 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson2) as JObject;

                var list=client.FaceGetlist(userId, groupId, options);

                richTextBox1.Text = list.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var options = new Dictionary<string, object>{
                            {"user_info", null}
                        };
            var sss = client.UserDelete("face_01", "296").ToString();

            richTextBox1.Text = sss.ToString();
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

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                FileStream fs = File.OpenRead(path); //OpenRead
                int filelength = 0;
                filelength = (int)fs.Length; //获得文件长度 
                Byte[] image = new Byte[filelength]; //建立一个字节数组 
                fs.Read(image, 0, filelength); //按字节流读取 
                                               //System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
                fs.Close();
                string imgData64 = Convert.ToBase64String(image);
                var imageType = "BASE64";  //BASE64   URL
                var options = new Dictionary<string, object>{
                            {"user_info",null }//"364d7fbd-38fe-46f3-90c0-d260849ac865"
                        };
                var sss = client.UserUpdate(imgData64, imageType, "face_01", "296", options).ToString();

                richTextBox1.Text = sss.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           var sss= client.UserGet("296","face_01").ToString();
            richTextBox1.Text = sss.ToString();
        }

    }
}
