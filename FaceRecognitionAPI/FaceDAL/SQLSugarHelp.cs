using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceDAL
{
    public class SQLSugarHelp
    {
        //禁止实例化
        private SQLSugarHelp()
        {

        }
        public static string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        /// <summary>
        /// 服务器链接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string Serverlink(string connStr)
        {
            return ConfigurationManager.AppSettings[connStr];
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(string connString)
        {
            SqlSugarClient db = null;
            try
            {
                db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = connString, DbType = DbType.SqlServer, IsAutoCloseConnection = true });
                MappingTablesConfig(db);
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return db;
        }



        /// <summary>
        /// 全局配置类
        /// </summary>
        /// <param name="db"></param>
        public static void MappingTablesConfig(SqlSugarClient db)
        {
            db.MappingTables.Add("OrderDEntity", "tbSupplierDeliverOrder_Detail");
            db.MappingTables.Add("FlowAttrValueEntity", "tbFlowAttrValue");
            db.MappingTables.Add("FlowLineEntity", "tbFlowLine");
            db.MappingTables.Add("ElementAttributesEntity", "tbFlowAttributes");
        }

        /// <summary>
        /// 打印Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        private static void PrintSql(string sql, string pars)
        {
            Console.WriteLine("sql:" + sql);
            if (pars != null)
            {
                Console.WriteLine(" pars:" + pars);
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// 生成数据分页sql，适用于Sql Server2005及以上版本
        /// </summary>
        /// <param name="selectSql">例如select t1.aaa, t2.bbb from table1 t1, table2 t2 where t1.id = t2.id and t1.aaa = 'abc'</param>
        /// <param name="pageIndex">页索引，第一页从0开始</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBySql">用于排序的sql语句，例如 t1.id desc</param>
        /// <returns></returns>
        public static string GetSqlServerPagedSql(string selectSql, int pageIndex, int pageSize, string orderBySql = null)
        {
            string select = "SELECT";
            int sIndex = selectSql.IndexOf(select, 0, StringComparison.CurrentCultureIgnoreCase);
            if (sIndex != -1) selectSql = " " + selectSql.Substring(sIndex + select.Length);
            int start = pageSize * (pageIndex - 1) + 1;
            int end = pageSize * (pageIndex - 1) + pageSize;
            if (orderBySql == null)
                orderBySql = "(SELECT 0)";
            string strSql = string.Format("with t as (select row_number() over(order by {0}) as rowNum, {1} "
                + ") select * from t where rowNum between {2} and {3} ORDER BY rowNum ", orderBySql, selectSql, start, end);
            return strSql;
        }

    }
   
}
