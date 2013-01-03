using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace LinaCWS.COM
{
    /// <summary>
    /// 数据查询类
    /// </summary>
    public class LNSql
    {
        public LNSql()
        {
            connStr = ConfigurationManager.ConnectionStrings["CWSConnStr"].ConnectionString;
            conn = new OleDbConnection(connStr);
            cmd = conn.CreateCommand();
            da = new OleDbDataAdapter(cmd);
            ds = new DataSet();
        }
        public string connStr { get; set; }
        public OleDbConnection conn { get; set; }
        public OleDbCommand cmd { get; set; }
        public OleDbDataAdapter da { get; set; }
        public OleDbDataReader dr { get; set; }
        public DataSet ds { get; set; }


    }
}