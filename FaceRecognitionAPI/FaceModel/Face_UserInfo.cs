using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceModel
{
    public class Face_UserInfo  //人脸用户表
    {
        public int NumberId { get; set; }  //主键自增
        public string UserName { get; set; }  //姓名
        public string staffNO { get; set; }  //工号
       
        public string face_token { get; set; }  //人脸唯一标识
        public string Guid_Id { get; set; } //人脸和数据库表关联字段
    }
}
