using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LinaCWS.COM;
using LitJson;

namespace LinaCWS
{
    public partial class workerEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //验证登录
            if (Session["adminID"] == null)
            {
                this.FormPanel.Visible = false;
                this.LogOnLink.Visible = true; 
            }
            else
            {
                this.FormPanel.Visible = true;
                this.LogOnLink.Visible =false;


                if (IsPostBack)
                {
                    int status = 0;
                    string msg = "未知错误！";
                    string wkName = Request.Form["wkName"];
                    string wkEntryTime = Request.Form["wkEntryTime"];
                    string wkPosition = Request.Form["wkPosition"];
                    string wkSalary = Request.Form["wkSalary"];                    
                    LNSql lnSql = new LNSql();
                    lnSql.conn.Open();
                    if (Request.Form["wkID"] == "" || Request.Form["wkID"] == null)
                    {
                        //新增

                        lnSql.cmd.CommandText = "insert into workers_tb (wkName,wkEntryTime,wkPosition,wkSalary) values(@wkName,@wkEntryTime,@wkPosition,@wkSalary)";
                        lnSql.cmd.Parameters.AddWithValue("@wkName", wkName);
                        lnSql.cmd.Parameters.AddWithValue("@wkEntryTime", wkEntryTime);
                        lnSql.cmd.Parameters.AddWithValue("@wkPosition", wkPosition);
                        lnSql.cmd.Parameters.AddWithValue("@wkSalary", wkSalary);


                    }
                    else
                    {
                        //修改
                        int wkID = Convert.ToInt32(Request.Params["wkID"]);
                        lnSql.cmd.CommandText = "update  workers_tb set wkName=@wkName,wkEntryTime=@wkEntryTime,wkPosition=@wkPosition,wkSalary=@wkSalary where wkID=@wkID";
                        lnSql.cmd.Parameters.AddWithValue("@wkName", wkName);
                        lnSql.cmd.Parameters.AddWithValue("@wkEntryTime", wkEntryTime);
                        lnSql.cmd.Parameters.AddWithValue("@wkPosition", wkPosition);
                        lnSql.cmd.Parameters.AddWithValue("@wkSalary", wkSalary);
                        lnSql.cmd.Parameters.AddWithValue("@wkID", wkID);
                    }
                    try
                    {

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
                    if (Request.Params["did"] == "" || Request.Params["did"] == null)
                    {
                        //新增员工
                        this.wkEntryTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        int wkID = Convert.ToInt32(Request.Params["did"]);
                        this.wkID.Value = wkID.ToString();
                        LNSql lnSql = new LNSql();
                        lnSql.conn.Open();
                        lnSql.cmd.CommandText = "select * from workers_tb where wkID="+wkID;
                        lnSql.dr = lnSql.cmd.ExecuteReader();
                        if (lnSql.dr.Read())
                        {
                            this.wkName.Text=lnSql.dr["wkName"].ToString();
                            this.wkPosition.Text=lnSql.dr["wkPosition"].ToString();
                            this.wkSalary.Text=lnSql.dr["wkSalary"].ToString();
                            this.wkEntryTime.Text=((DateTime)lnSql.dr["wkEntryTime"]).ToString("yyyy-MM-dd");
                        }
                        lnSql.dr.Close();
                        lnSql.conn.Close();
                        //修改员工
                        int did = Convert.ToInt32(Request.Params["did"]);

                    }

                }

            }
        }
    }
}