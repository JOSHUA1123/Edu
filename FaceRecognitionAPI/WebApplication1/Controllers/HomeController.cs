using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AForge;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using FaceBLL;
using FaceModel;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;
using DingTalk.Api.Response;
using DingTalk.Api;
using DingTalk.Api.Request;
using Newtonsoft.Json;
using System.Configuration;
using MPWeiXin;
using MPWeiXin.BaseServer;
using System.Text;
using System.Web.Security;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        /// <summary>
        /// 百度AI API Key
        /// </summary>
        public string API_KEY = ConfigurationManager.AppSettings["API_KEY"]; //"hEqnBzOEkoAe4AoKUGSfWsZt";                   //你的 Api Key
        /// <summary>
        /// 百度AI API Secret Key
        /// </summary>
        public string SECRET_KEY = ConfigurationManager.AppSettings["SECRET_KEY"];// "p1j0mDKs2RAYdPn0rDUMVdcb0aazzlp6";        //你的 Secret Key

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FaceIndex()
        {
            return View();
        }

        public ActionResult FaceIndex2()
        {
            return View();
        }
        /// <summary>
        /// 超时页面
        /// </summary>
        /// <returns></returns>
        public ActionResult error()
        {
            var msg = Request["msg"];
            ViewBag.msg = msg;
            return View();
        }

        /// <summary>
        /// 人脸识别注册页
        /// </summary>
        /// <returns></returns>
        public ActionResult FaceRe()
        {
            var staffNO = Request["staffNO"];
            var name = Request["name"];
            UserinforTools.TTJS("Name:" + name);
            if (staffNO != null)
            {
                staffNO = Tools.DesDecrypt(staffNO, Tools.keys);//解密
                var ss = staffNO.Split(',');
                staffNO = ss[0];
                var tt = DateTime.Parse(ss[1]);
                var ts = DateTime.Now - tt;
                int sec = (int)ts.TotalSeconds;
                if (sec > 120)
                {
                    var msg = "该页面已失效，请重新打开应用";
                    return Redirect("/home/error?msg=" + msg);
                }
            }

            ViewBag.staffNO = staffNO;
            ViewBag.UserName = name;
            return View();
        }
        /// <summary>
        /// 人脸识别页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FaceSB()
        {
            var staffNO = Request["staffNO"];
            var redirectUrl = Tools.URL + staffNO;
            ViewBag.redirectUrl = redirectUrl;
            ViewBag.staffNO = staffNO;
            return View();
        }

        /// <summary>
        /// 该方法页面未使用
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult GetUserId(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserinforTools.GetUserIdParameter();
            }
            else
            {
                UserinforTools.TTJS("GetUserId:" + UserId);
                Response.Redirect("/home/FaceView1?UserId=" + UserId);
            }

            return View();
        }

        /// <summary>
        /// 企业微信用户进入页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FaceView1(string UserId)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    UserinforTools.GetUserIdParameter();
                }
                else
                {
                    UserinforTools.TTJS("FaceView1:" + UserId);
                    var staffNO = string.Empty;
                    var name = string.Empty;
                    if (UserId == "ZhuXinQuan")
                    {
                        staffNO = "007";
                        name = "朱新全";
                    }
                    else
                    {
                        var ss = GetStaffNO(UserId);
                        staffNO = ss.Split('?')[0];
                        name = ss.Split('?')[1];
                    }
                    //Session["staffNO"] = staffNO;
                    UserinforTools.TTJS("staffNO:" + staffNO + "name:" + name);
                    LogService<SerializeHandler>.Instance.Info("staffNO:" + staffNO + "name:" + name);

                    if (!string.IsNullOrEmpty(staffNO))
                    {
                        var entys = new Face_UserInfoBLL().GetfaceinfoByStaffNo(staffNO);//查询是否存在
                        if (entys == null)
                        {
                            UserinforTools.TTJS("entys:NULL");
                        }
                        var tt = DateTime.Now;
                        staffNO = staffNO + "," + tt + "," + Tools.RandomStr();
                        staffNO = Tools.DesEncrypt(staffNO, Tools.keys);//参数加密
                        UserinforTools.TTJS("staffNO1:" + staffNO);
                        LogService<SerializeHandler>.Instance.Info("staffNO1:" + staffNO);
                        if (entys != null && entys.Count > 0)//存在则跳转到刷脸界面
                        {
                            Response.Redirect("/home/FaceSB?staffNO=" + staffNO);
                        }
                        else//不存在则跳转到刷脸注册页面
                        {
                            Response.Redirect("/home/FaceRe?staffNO=" + staffNO + "&name=" + name);
                        }
                    }
                    else
                    {
                        Response.Redirect("/home/error?msg=未找到您的工号");
                    }
                }
            }
            catch (Exception ex)
            {
                UserinforTools.TTERRORJS(ex.Message);
                LogService<SerializeHandler>.Instance.Error(ex.Message);
                Response.Redirect("/home/error?msg=系统故障");
            }
            //ViewBag.flag = flag;
            //ViewBag.staffNO = staffNO;

            return View();
        }



        public ActionResult GZInfoWeb(string staffNO)
        {

            var redirectUrl = Tools.URL + staffNO;

            ViewBag.UrlRe = redirectUrl;

            return View();
        }

        ///// <summary>
        ///// 钉钉用户进入页面
        ///// </summary>
        ///// <returns></returns>
        public ActionResult FaceView()
        {
            return View();
        }

        public JsonResult GetCodes()
        {
            var code = Request["code"];
            var staffNO = "";
            var flag = 0;
            if (!string.IsNullOrEmpty(code))
            {
                staffNO = DingLogin(code);
                if (!string.IsNullOrEmpty(staffNO))
                {
                    var entys = new Face_UserInfoBLL().GetfaceinfoByStaffNo(staffNO);//查询是否存在
                    if (entys != null && entys.Count > 0)//存在则跳转到刷脸界面
                    {
                        //Response.Redirect("/home/FaceSB");
                        flag = 1;
                    }
                    else//不存在则跳转到刷脸注册页面
                    {
                        //Response.Redirect("/home/FaceRe?staffNO=" + staffNO);
                        flag = 2;
                    }
                    var tt = DateTime.Now;
                    staffNO = staffNO + "," + tt + "," + Tools.RandomStr();
                    staffNO = Tools.DesEncrypt(staffNO, Tools.keys);//参数加密
                }
            }

            return Json(new { flag = flag, staffNO = staffNO }, JsonRequestBehavior.AllowGet);
        }

        #region 对接钉钉接口操作
        /// <summary>
        /// 获取用户工号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string DingLogin(string code)
        {
            var staffNO = string.Empty;
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getuserinfo");
            OapiUserGetuserinfoRequest request = new OapiUserGetuserinfoRequest();
            request.Code = code;
            request.SetHttpMethod("GET");
            OapiUserGetuserinfoResponse response = client.Execute(request, Tools.GetToken());
            string userId = response.Userid;
            return GetStaffNO(userId);

        }
        public string GetStaffNO(string userId)
        {
            var staffNO = string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                DefaultDingTalkClient clientss = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get");
                OapiUserGetRequest requests = new OapiUserGetRequest();
                requests.Userid = userId;
                requests.SetHttpMethod("GET");
                OapiUserGetResponse responses = clientss.Execute(requests, Tools.GetToken());
                //UserinforTools.TTJS("GetStaffNO:" + Tools.Serialize(responses));
                staffNO = responses.Jobnumber;
                var name = responses.Name;
                staffNO = staffNO + "?" + name;
            }

            return staffNO;

        }
        #endregion

        #region 人脸识别的相关操作

        //人脸注册
        public JsonResult Face_Registration()
        {
            // 设置APPID/AK/SK
            //var API_KEY = "XFPA49myCG7S37XP1DxjLbXF";                   //你的 Api Key
            //var SECRET_KEY = "ZvZKigrixMLXNZOLmkrG6iDx9QprlGuT";        //你的 Secret Key
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            var imageType = "BASE64";  //BASE64   URL
            string imgData64 = Request["imgData64"];
            string staffNO = Request["staffNO"];

            imgData64 = imgData64.Substring(imgData64.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            ResultInfo result = new ResultInfo();
            try
            {
                var guid = Guid.NewGuid().ToString();
                //注册人脸
                var groupId = "face_01";
                var userId = staffNO;
                //首先查询是否存在人脸
                var result2 = client.Search(imgData64, imageType, groupId);  //会出现222207（未找到用户）这个错误
                var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(result2);
                var o2 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson) as JObject;


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
                            result.info = result2.ToString();
                            result.res = true;
                            result.startcode = 221;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


                // 调用人脸注册，可能会抛出网络等异常，请使用try/catch捕获
                // 如果有可选参数
                var options = new Dictionary<string, object>{
                            {"user_info", guid}
                        };
                // 带参数调用人脸注册
                var resultData = client.UserAdd(imgData64, imageType, groupId, userId, options);

                var result3 = client.Search(imgData64, imageType, groupId);  //会出现222207（未找到用户）这个错误
                var strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(result3);
                var o3 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson2) as JObject;


                ////判断是否存在当前人脸，相识度是否大于80
                //if (o3["error_code"].ToString() == "0" && o3["error_msg"].ToString() == "SUCCESS")
                //{
                //    var result_list = Newtonsoft.Json.JsonConvert.DeserializeObject(o3["result"].ToString()) as JObject;
                //    var user_list = result_list["user_list"];
                //    var Obj = JArray.Parse(user_list.ToString());
                //    foreach (var item in Obj)
                //    {
                //        result.other = item["user_info"].ToString();
                //    }
                //}
                result.other = guid;
                result.info = resultData.ToString();
                result.res = true;

            }
            catch (Exception e)
            {
                result.info = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //人脸识别
        public JsonResult Face_Distinguish()
        {
            // 设置APPID/AK/SK
            //var API_KEY = "XFPA49myCG7S37XP1DxjLbXF";                   //你的 Api Key
            //var SECRET_KEY = "ZvZKigrixMLXNZOLmkrG6iDx9QprlGuT";        //你的 Secret Key
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            var imageType = "BASE64";  //BASE64   URL staffNO
            string imgData64 = Request["imgData64"];
            //string staffNO = Request["staffNO"];
            imgData64 = imgData64.Substring(imgData64.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            ResultInfo result = new ResultInfo();
            try
            {
                var groupId = "face_01";
                //var userId = "user1";
                var result323 = client.Detect(imgData64, imageType);
                //活体检测阈值是多少
                //0.05 活体误拒率：万分之一；拒绝率：63.9%
                //0.3 活体误拒率：千分之一；拒绝率：90.3%
                //0.9 活体误拒率：百分之一；拒绝率：97.6%
                //1误拒率: 把真人识别为假人的概率. 阈值越高，安全性越高, 要求也就越高, 对应的误识率就越高
                //2、通过率=1-误拒率
                //所以你thresholds参数返回 和 face_liveness 比较大于推荐值就是活体
                ////活体判断
                var faces = new JArray
                        {
                            new JObject
                            {
                                {"image", imgData64},
                                {"image_type", "BASE64"}
                            }
                        };
                var Living = client.Faceverify(faces);  //活体检测交互返回
                var LivingJson = Newtonsoft.Json.JsonConvert.SerializeObject(Living);
                var LivingObj = Newtonsoft.Json.JsonConvert.DeserializeObject(LivingJson) as JObject;
                if (LivingObj["error_code"].ToString() == "0" && LivingObj["error_msg"].ToString() == "SUCCESS")
                {
                    var Living_result = Newtonsoft.Json.JsonConvert.DeserializeObject(LivingObj["result"].ToString()) as JObject;
                    var Living_list = Living_result["thresholds"];
                    double face_liveness = Convert.ToDouble(Living_result["face_liveness"]);
                    var frr = Newtonsoft.Json.JsonConvert.SerializeObject(Living_list.ToString());
                    var frr_1eObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Living_list.ToString()) as JObject;
                    double frr_1e4 = Convert.ToDouble(frr_1eObj["frr_1e-4"]);
                    if (face_liveness < frr_1e4)
                    {
                        result.info = "识别失败：不是活体！";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                //首先查询是否存在人脸
                var result2 = client.Search(imgData64, imageType, groupId);
                var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(result2);
                var o2 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson) as JObject;
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
                        //Tools.TT1("工号："+ Session["staffNO"] + "人脸识别相似度： "+ score.ToString());
                        UserinforTools.TTJS("人脸识别相似度： " + score.ToString());
                        LogService<SerializeHandler>.Instance.Info("人脸识别相似度： " + score.ToString());
                        if (score > 89)
                        {
                            result.info = result2.ToString();
                            result.res = true;
                            result.startcode = 221;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            result.info = "未识别，请重新打开识别";
                            result.res = false;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    //result.info = strJson.ToString();
                    result.info = "未识别，请重新打开识别";
                    Tools.TT1(strJson.ToString());
                    result.res = false;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                //result.info = e.Message;
                Tools.TT1(e.Message);
                result.info = "未识别，请重新打开识别";
                result.res = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //用户信息入库
        public JsonResult face_userInfoSace()
        {
            ResultInfo result = new ResultInfo();

            try
            {
                //这里就不进行非空判断了，后期根据实际情况进行优化
                var UserName = Request["UserName"];
                var staffNO = Request["staffNO"];
                //var Sex = Request["Sex"];
                //var Works = Request["Works"];
                var face_token = Request["face_token"];
                var Guid_Id = Request["Guid_Id"];
                UserinforTools.TTJS("Guid_Id:" + Guid_Id);
                Face_UserInfo model = new Face_UserInfo();
                model.UserName = UserName;
                model.staffNO = staffNO;
                //model.Sex = Sex;
                //model.Works = Works;
                model.face_token = face_token;
                model.Guid_Id = Guid_Id;

                //根据人脸唯一标识判断是否存在数据
                List<Face_UserInfo> strlist = new Face_UserInfoBLL().GetfaceinfoByToken(Guid_Id);
                if (strlist.Count > 0)
                {
                    result.res = true;
                    result.info = "当前用户已注册过！";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (new Face_UserInfoBLL().face_userInfoSace(model) > 0)
                {
                    result.res = true;
                    var tt = DateTime.Now;
                    staffNO = staffNO + "," + tt + "," + Tools.RandomStr();
                    staffNO = Tools.DesEncrypt(staffNO, Tools.keys);//参数加密
                    result.other = staffNO;
                    result.info = "注册成功";
                }
                else
                    result.info = "注册失败";
            }
            catch (Exception e)
            {
                result.info = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //识别成功，查询数据库
        public JsonResult Face_UserInfoList()
        {
            ResultInfo result = new ResultInfo();
            //这里就不进行非空判断了，后期根据实际情况进行优化
            var Guid_Id = Request["Guid_Id"];
            var staffNO = Request["staffNO"];

            staffNO = Tools.DesDecrypt(staffNO, Tools.keys);//解密
            var ss = staffNO.Split(',');
            staffNO = ss[0];

            //根据人脸唯一标识判断是否存在数据
            Face_UserInfo strlist = new Face_UserInfoBLL().GetfaceinfoByToken(Guid_Id).FirstOrDefault();
            var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(strlist);
            if (staffNO == strlist.staffNO)
            {
                result.res = true;
            }
            else
            {
                result.res = false;
            }
            result.info = strJson;
            UserinforTools.TTJS("result:" + strJson);
            LogService<SerializeHandler>.Instance.Info("result:" + strJson);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 图片上传到服务器 Base64解码

        /// <summary>
        /// 图片上传 Base64解码
        /// </summary>
        /// <param name="dataURL">Base64数据</param>
        /// <param name="path">保存路径</param>
        /// <param name="imgName">图片名字</param>
        /// <returns>返回一个相对路径</returns>

        public JsonResult decodeBase64ToImage(string dataURL, string imgName)
        {
            string filename = "";//声明一个string类型的相对路径

            String base64 = dataURL.Substring(dataURL.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            System.Drawing.Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片
            ResultInfo result = new ResultInfo();
            try//会有异常抛出，try，catch一下
            {
                String inputStr = base64;//把纯净的Base64资源扔给inpuStr,这一步有点多余

                byte[] arr = Convert.FromBase64String(inputStr);//将纯净资源Base64转换成等效的8位无符号整形数组

                System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);//将MemoryStream对象转换成Bitmap对象
                ms.Close();//关闭当前流，并释放所有与之关联的资源
                bitmap = bmp;
                filename = "/upload/" + imgName + ".png";//所要保存的相对路径及名字
                string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录 
                string imagesurl2 = tmpRootDir + filename.Replace(@"/", @"\"); //转换成绝对路径 

                bitmap.Save(imagesurl2, System.Drawing.Imaging.ImageFormat.Png);//保存到服务器路径
                result.info = imagesurl2;  //返回相对路径
                result.res = true;
            }
            catch (Exception)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region 测试调用摄像头

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult EWView()
        {
            ViewBag.appid = BaseModel.qyappid;
            ViewBag.agentid = BaseModel.agentid;
            ViewBag.redirect_uri = BaseModel.redirect_uri;

            return View();
        }

        public ActionResult GetUserInfo(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var userId = UserinforTools.GetUserIdOrOpenid(code);
                UserinforTools.TTJS(code);
                if (!string.IsNullOrEmpty(userId))
                {
                    Response.Redirect("/home/error?msg=" + userId);
                }
            }
            else
            {
                var msg = "未获取企业微信相应的code";
                return Redirect("/home/error?msg=" + msg);
            }

            return View();
        }

        public ActionResult Test1(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserinforTools.GetUserIdParameter();
            }

            ViewBag.userID = UserId;

            return View();
        }

        #endregion


        #region 测试js调用相机

        public ActionResult TestPic()
        {
            return View();
        }

        public JsonResult GetParam(string link)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string tss = Convert.ToInt64(ts.TotalSeconds).ToString();
            var noncestr = UserinforTools.GetRandomString(16, true, true, true, false, "");
            string url = link;//"http://chenguang.rijiyuan.com:156/home/TestPic";
            var jsapi_ticket = "";
            try
            {
                jsapi_ticket = UserinforTools.Ticket();
            }
            catch (Exception ex)
            {
                var ssss = ex.Message;
                //throw;
            }
           
            var jmdata = "";
            jmdata = "jsapi_ticket="+ jsapi_ticket + "&noncestr="+ noncestr + "&timestamp="+ tss + "&url="+ url ;
            string outStr = "";
            string signature= GetSignature(jsapi_ticket, noncestr, long.Parse(tss), url, out outStr);
            //try
            //{
            //     signature = UserinforTools.SHA1_Encrypt(jmdata);
            //}
            //catch (Exception)
            //{
            //}
            
            var sss = "";
            //System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(jmdata, "SHA1").ToLower();
            var data = new
            {
                timestamp = tss,
                nonceStr = noncestr,
                signature = signature,
                appid = BaseModel.qyappid,
                url=url
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult JsTestPic()
        {
            return View();
        }



        #endregion


        #region 签名算法
        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="jsapi_ticket">jsapi_ticket</param>
        /// <param name="noncestr">随机字符串(必须与wx.config中的nonceStr相同)</param>
        /// <param name="timestamp">时间戳(必须与wx.config中的timestamp相同)</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分(必须是调用JS接口页面的完整URL)</param>
        /// <returns></returns>
        public static string GetSignature(string jsapi_ticket, string noncestr, long timestamp, string url, out string string1)
        {
            var string1Builder = new StringBuilder();
            string1Builder.Append("jsapi_ticket=").Append(jsapi_ticket).Append("&")
                          .Append("noncestr=").Append(noncestr).Append("&")
                          .Append("timestamp=").Append(timestamp).Append("&")
                          .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
            string1 = string1Builder.ToString();
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
        }
        #endregion





        //枚举所有视频输入设备(暂时不使用)
        public string videoDevices()
        {
            //枚举所有视频输入设备
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
                throw new ApplicationException();

            string bb = "";
            foreach (AForge.Video.DirectShow.FilterInfo device in videoDevices)
            {
                bb += device.Name + ",";
            }
            bb = bb.Substring(0, bb.Length - 1);
            return bb;
        }

        //定义一个返回类型的实体
        public class ResultInfo
        {
            public ResultInfo()
            {  //默认值
                res = false;
                startcode = 449;
                info = "";
                other = "";
            }
            public bool res { get; set; }  //返回状态（true or false）
            public int startcode { get; set; } //返回http状态码
            public string info { get; set; }  //返回结果
            public string other { get; set; }  //其他
        }

    }
}