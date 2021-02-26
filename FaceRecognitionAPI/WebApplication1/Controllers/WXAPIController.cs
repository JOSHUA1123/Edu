using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using FaceBLL;
using FaceModel;
using MPWeiXin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WXAPIController : Controller
    {
        /// <summary>
        /// 百度AI API Key
        /// </summary>
        public string API_KEY = ConfigurationManager.AppSettings["API_KEY"]; //"hEqnBzOEkoAe4AoKUGSfWsZt";                   //你的 Api Key
        /// <summary>
        /// 百度AI API Secret Key
        /// </summary>
        public string SECRET_KEY = ConfigurationManager.AppSettings["SECRET_KEY"];// "p1j0mDKs2RAYdPn0rDUMVdcb0aazzlp6";        //你的 Secret Key
        #region 小程序先关数据接口

        /// <summary>
        /// 判断是否已注册
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult IsFaceRe(string UserId)
        {
            ResultInfo info = new ResultInfo();
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    UserinforTools.GetUserIdParameter();
                }
                else
                {
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
                    var staffNONew = staffNO;
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
                            //Response.Redirect("/home/FaceSB?staffNO=" + staffNO);
                            info.res = true;
                            info.startcode = 200;//已注册
                            info.info = staffNO;
                            info.staffNO = staffNONew;
                        }
                        else//不存在则跳转到刷脸注册页面
                        {
                            //Response.Redirect("/home/FaceRe?staffNO=" + staffNO + "&name=" + name);
                            info.res = true;
                            info.startcode = 201;//未注册
                            info.info = staffNO;
                            info.other = name;
                            info.staffNO = staffNONew;
                        }
                    }
                    else
                    {
                        //Response.Redirect("/home/error?msg=未找到您的工号");
                        info.res = false;
                        info.startcode = 400;
                        info.info = "未找到您的工号";
                    }
                }
            }
            catch (Exception)
            {
                info.res = false;
                info.startcode = 401;
                info.info = "系统异常请重新打开！！";
            }


            return Json(info, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 人脸识别
        /// </summary>
        /// <returns></returns>
        public JsonResult FaceDistinguish()
        {
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            var imageType = "BASE64";  //BASE64   URL staffNO
            string imgData64 = Request["imgData64"];
            string staffNO = Request["staffNO"];
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
                        var guid = Convert.ToString(item["user_info"]);
                        //Tools.TT1("工号："+ Session["staffNO"] + "人脸识别相似度： "+ score.ToString());
                        UserinforTools.TTJS("人脸识别相似度： " + score.ToString());
                        LogService<SerializeHandler>.Instance.Info("人脸识别相似度： " + score.ToString());
                        if (score > 89)
                        {

                            if (!string.IsNullOrEmpty(staffNO))
                            {
                                //var redirectUrl = Tools.URL + staffNO;
                                var redirectUrl =  staffNO;
                                staffNO = Tools.DesDecrypt(staffNO, Tools.keys);//解密
                                var ss = staffNO.Split(',');
                                staffNO = ss[0];
                                Face_UserInfo faceUserInfo = new Face_UserInfoBLL().GetfaceinfoByToken(guid).FirstOrDefault();
                                if (faceUserInfo != null)
                                {
                                    if (staffNO== faceUserInfo.staffNO)
                                    {
                                        result.res = true;
                                        result.info = redirectUrl;
                                        result.startcode = 221;
                                    }else
                                    {
                                        result.res = false;
                                        result.info = "非本人操作！！";
                                    }
                                    
                                }
                                else
                                {
                                    result.res = false;
                                    result.info = "查询注册信息失败！！";
                                }
                            }
                            else
                            {
                                result.res = false;
                                result.info = "参数获取失败！！";
                            }

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

        //人脸注册
        public JsonResult FaceRegistration()
        {
            // 设置APPID/AK/SK
            //var API_KEY = "XFPA49myCG7S37XP1DxjLbXF";                   //你的 Api Key
            //var SECRET_KEY = "ZvZKigrixMLXNZOLmkrG6iDx9QprlGuT";        //你的 Secret Key
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            var imageType = "BASE64";  //BASE64   URL
            string imgData64 = Request["imgData64"];
            string staffNO = Request["staffNO"];
            var UserName = Request["UserName"];
            

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
                            result.info = "已存在与该图像相似度"+ score+"%的图像";
                            result.res = false;
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


                ////判断存在当前人脸是否上传成功
                if (o3["error_code"].ToString() == "0" && o3["error_msg"].ToString() == "SUCCESS")
                {
                    var result_list03 = Newtonsoft.Json.JsonConvert.DeserializeObject(o3["result"].ToString()) as JObject;
                    var face_token = result_list03["face_token"];
                    Face_UserInfo model = new Face_UserInfo();
                    model.UserName = UserName;
                    model.staffNO = staffNO;
                    model.Guid_Id = guid;
                    model.face_token = face_token.ToString();
                    if (new Face_UserInfoBLL().face_userInfoSace(model) > 0)
                    {
                        result.res = true;
                        var tt = DateTime.Now;
                        staffNO = staffNO + "," + tt + "," + Tools.RandomStr();
                        staffNO = Tools.DesEncrypt(staffNO, Tools.keys);//参数加密
                        result.other = staffNO;
                        result.info = "注册成功";
                    }
                    else { 
                        result.info = "注册失败";
                    }

                }else
                {
                    //result.other = guid;
                    //result.info = resultData.ToString();
                    result.res = false;
                    result.info = "图像上传失败！请重新上传";
                }
                

            }
            catch (Exception e)
            {
                result.info = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 人脸注册保存数据库
        /// </summary>
        /// <returns></returns>
        public JsonResult FaceUserInfoSace()
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


        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult TestGetInfo(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                Response.Redirect(Request.Url.AbsoluteUri + "?UserId=1");
            }
            return Json(Request.Url.AbsoluteUri + "  (" + UserId + ")  成功！！", JsonRequestBehavior.AllowGet);
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
            public string staffNO { set; get; }//未加密的工号
        }
        #endregion
    }
}