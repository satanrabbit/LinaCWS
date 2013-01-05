using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LinaCWS.COM;
using LitJson;
using System.Data;
using System.Data.OleDb;


namespace LinaCWS
{
    public partial class workEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int status = 0;
                string msg = "未知错误！";
                int y = Convert.ToInt32(Request.Form["wkY"]);
                int m = Convert.ToInt32(Request.Form["wkM"]);
                int wkID = Convert.ToInt32(Request.Form["wkID"]);
                float uwkTimes = float.Parse(Request.Form["uwkTimes"]);
                float owkTimes = float.Parse(Request.Form["owkTimes"]);

                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                OleDbTransaction trans = lnSql.conn.BeginTransaction();
                lnSql.cmd.Transaction = trans;
                
                try
                {
                    //删除缺勤记录
                    lnSql.cmd.CommandText = "delete from unworks_tb where uworker = @wkID and uwkYear=@y and uwkMonth=@m ";
                    lnSql.cmd.Parameters.Clear();
                    lnSql.cmd.Parameters.AddWithValue("@wkID", wkID);
                    lnSql.cmd.Parameters.AddWithValue("@y", y);
                    lnSql.cmd.Parameters.AddWithValue("@m", m);
                    lnSql.cmd.ExecuteNonQuery();
                    //插入新缺勤记录
                    lnSql.cmd.CommandText = "insert into unworks_tb (uworker , uwkYear,uwkMonth , uwkTimes)values( @wkID, @y , @m ,@uwkTimes) ";
                    lnSql.cmd.Parameters.Clear();
                    lnSql.cmd.Parameters.AddWithValue("@wkID", wkID);
                    lnSql.cmd.Parameters.AddWithValue("@y", y);
                    lnSql.cmd.Parameters.AddWithValue("@m", m);
                    lnSql.cmd.Parameters.AddWithValue("@uwkTimes", uwkTimes);
                    lnSql.cmd.ExecuteNonQuery();
                    //删除加班记录
                    lnSql.cmd.CommandText = "delete from overworks_tb where oworker = @wkID and owkYear=@y and owkMonth=@m ";
                    lnSql.cmd.Parameters.Clear();
                    lnSql.cmd.Parameters.AddWithValue("@wkID", wkID);
                    lnSql.cmd.Parameters.AddWithValue("@y", y);
                    lnSql.cmd.Parameters.AddWithValue("@m", m);
                    lnSql.cmd.ExecuteNonQuery();
                    //插入新加班记录

                    lnSql.cmd.CommandText = "insert into overworks_tb (oworker , owkYear,owkMonth , owkTimes)values( @wkID, @y , @m ,@owkTimes) ";
                    lnSql.cmd.Parameters.Clear();
                    lnSql.cmd.Parameters.AddWithValue("@wkID", wkID);
                    lnSql.cmd.Parameters.AddWithValue("@y", y);
                    lnSql.cmd.Parameters.AddWithValue("@m", m);
                    lnSql.cmd.Parameters.AddWithValue("@owkTimes", owkTimes);
                    lnSql.cmd.ExecuteNonQuery();

                    trans.Commit();
                    status = 1;
                    msg = "保存成功！";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    trans.Rollback();
                }
                finally
                {

                    lnSql.conn.Close();
                    JsonData jsonData = new JsonData();
                    jsonData["status"] = status;
                    jsonData["msg"] = msg;
                    string echoString = jsonData.ToJson();

                    Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    Response.Write(echoString);
                    Response.End();
                }
            }
            else
            {
                int y = Convert.ToInt32(Request.Params["wy"]);
                int m = Convert.ToInt32(Request.Params["wm"]);
                int wkID = Convert.ToInt32(Request.Params["did"]);
                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                //查询缺勤记录
                lnSql.cmd.CommandText = "select uwkTimes from unworks_tb where uworker = @wkID and uwkYear=@y and uwkMonth=@m ";
                lnSql.cmd.Parameters.AddWithValue("@wkID",wkID);
                lnSql.cmd.Parameters.AddWithValue("@y", y);
                lnSql.cmd.Parameters.AddWithValue("@m", m);
                lnSql.dr = lnSql.cmd.ExecuteReader();
                if (lnSql.dr.Read())
                {
                    this.uwkTimes.Text = lnSql.dr["uwkTimes"].ToString();
                }
                lnSql.dr.Close();
                //查询加班记录
                lnSql.cmd.CommandText = "select owkTimes from overworks_tb where oworker = @wkID and owkYear=@y and owkMonth=@m ";
                lnSql.cmd.Parameters.AddWithValue("@wkID", wkID);
                lnSql.cmd.Parameters.AddWithValue("@y", y);
                lnSql.cmd.Parameters.AddWithValue("@m", m);
                lnSql.dr = lnSql.cmd.ExecuteReader();
                if (lnSql.dr.Read())
                {
                    this.owkTimes.Text = lnSql.dr["owkTimes"].ToString();
                }
                lnSql.dr.Close();
                lnSql.conn.Close();
                this.wkID.Value = wkID.ToString();
                this.wkM.Value = m.ToString();
                this.wkY.Value = y.ToString();
                
            }
        }
    }
}