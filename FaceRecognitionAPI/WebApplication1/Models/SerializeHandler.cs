using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// 序列化和反序列化处理类
    /// </summary>
    public class SerializeHandler
    {
        #region 序列化和反序列化

        /// <summary>
        /// 序列化指定类型的对象到指定的文件
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="fileName">保存对象数据的完整文件名</param>
        public static void Serialize<T>(T obj, string fileName)
        {
            lock (fileName)
            {
                try
                {
                    string dir = Path.GetDirectoryName(fileName);       //获取文件路径
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    formatter.Serialize(fs, obj);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    LogService<SerializeHandler>.Instance.Error(ex.Message, ex);
                }
            }
        }
        #endregion
    }

}