using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using LinaCWS.COM;

namespace LinaCWS
{
    public partial class pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int status = 0;
                string msg = "未知错误";
                int wkpID = Convert.ToInt32(Request.Params["wkpID"]);
                float wlpReal = float.Parse(Request.Params["wlpReal"]);
                int wlpAdmin = Convert.ToInt32(Session["adminID"].ToString());
                string wlpTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                try
                {
                    lnSql.cmd.CommandText = "update wkPay_tb set wlpReal=@wlpReal , wlpAdmin=@wlpAdmin,wlpTime=@wlpTime where wkpID=" + wkpID;
                    lnSql.cmd.Parameters.AddWithValue("@wlpReal", wlpReal);
                    lnSql.cmd.Parameters.AddWithValue("@wlpAdmin", wlpAdmin);
                    lnSql.cmd.Parameters.AddWithValue("@wlpTime", wlpTime);
                    lnSql.cmd.Parameters.AddWithValue("@wkpID", wkpID);
                    lnSql.cmd.ExecuteNonQuery();
                    status = 1;
                    msg = "保存成功！";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
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
                int wkpID = Convert.ToInt32(Request.Params["did"]);
                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                lnSql.cmd.CommandText = "select wlpReal from wkPay_tb where wkpID=" + wkpID;
                lnSql.dr = lnSql.cmd.ExecuteReader();
                if (lnSql.dr.Read())
                {
                    if (lnSql.dr["wlpReal"] != null)
                    {
                        this.wlpReal.Text = lnSql.dr["wlpReal"].ToString();
                    }
                }

                lnSql.conn.Close();
                this.wkpID.Value = wkpID.ToString();
            }
        }
    }
}