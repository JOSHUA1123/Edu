using FaceDAL;
using FaceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceBLL
{
    public class Face_UserInfoBLL
    {
        Face_UserInfoDAL dal = new Face_UserInfoDAL();

         //人脸注册绑定信息
        public int face_userInfoSace(Face_UserInfo model)
        {
            return dal.face_userInfoSace(model);
        }
        
        //根据人脸唯一标识判断是否存在数据
        public List<Face_UserInfo> GetfaceinfoByToken(string Guid_Id)
        {
            return dal.GetfaceinfoByToken(Guid_Id);
        }
        public List<Face_UserInfo> GetfaceinfoByStaffNo(string staffNo)
        {
            return dal.GetfaceinfoByStaffNo(staffNo);
        }
    }
}
