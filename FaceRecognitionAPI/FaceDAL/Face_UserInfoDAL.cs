using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceModel;
using System.Data.SqlClient;
using SqlSugar;

namespace FaceDAL
{
    public class Face_UserInfoDAL
    {
        //人脸注册绑定信息
        public int face_userInfoSace(Face_UserInfo model)
        {
            using (var db = SQLSugarHelp.GetInstance(SQLSugarHelp.ConnString))
            {
                List<string> list = new List<string>();
                List<SugarParameter> parameters = null;
                string sql = "insert into Face_UserInfo(UserName,staffNO,face_token,Guid_Id) values('" + model.UserName + "','" + model.staffNO + "','" + model.face_token + "','" + model.Guid_Id + "')";
                return db.Ado.ExecuteCommand(sql, parameters);//执行数据库返回受影响行数
            }
        }


        //根据人脸唯一标识判断是否存在数据
        public List<Face_UserInfo> GetfaceinfoByToken(string Guid_Id)
        {
            using (var db = SQLSugarHelp.GetInstance(SQLSugarHelp.ConnString))
            {
                string sql = @"select * from dbo.Face_UserInfo where Guid_Id='" + Guid_Id + "'";
                return db.SqlQueryable<Face_UserInfo>(sql).ToList();

            }
        }

        public List<Face_UserInfo> GetfaceinfoByStaffNo(string staffNo)
        {
            using (var db = SQLSugarHelp.GetInstance(SQLSugarHelp.ConnString))
            {
                string sql = @"select * from dbo.Face_UserInfo where staffNO='" + staffNo + "'";
                return db.SqlQueryable<Face_UserInfo>(sql).ToList();

            }
        }

    }
}
