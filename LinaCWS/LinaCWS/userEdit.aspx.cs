using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinaCWS.COM;
using LitJson;
using SRLib;

namespace LinaCWS
{
    public partial class userEidt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int status = 0;
                string msg = "未定义错误";
                SrCom srCom = new SrCom();
                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                //保存提交数据
                try
                {
                    int resetPwd = Convert.ToInt32(Request.Params["resetPwd"]);
                    string adminName = Request.Params["adminName"];
                    string adminAccount = Request.Params["adminAccount"];
                    string adminCpPwd = srCom.HashPassword(Request.Params["adminCpPwd"]);
                    string adminPWD = srCom.HashPassword(Request.Params["adminPWD"]);
                    if (Request.Params["adminID"] == "" || Request.Params["adminID"] == null)
                    {
                        //添加用户，全部初始化密码
                       
                        lnSql.cmd.CommandText = "insert into admins_tb (adminName,adminAccount,adminPWD) values(@adminName,@adminAccount,@adminPWD)";
                        lnSql.cmd.Parameters.AddWithValue("@adminName", adminName);
                        lnSql.cmd.Parameters.AddWithValue("@adminAccount", adminAccount);
                        lnSql.cmd.Parameters.AddWithValue("@adminPWD", adminPWD);
                        lnSql.cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        int adminID = Convert.ToInt32(Request.Params["adminID"]);
                        //修改用户
                        
                        if (resetPwd == 1)
                        {
                            lnSql.cmd.CommandText = "update admins_tb set adminName=@adminName,adminAccount=@adminAccount,adminPWD=@adminPWD where adminID=@adminID";
                            lnSql.cmd.Parameters.AddWithValue("@adminName", adminName);
                            lnSql.cmd.Parameters.AddWithValue("@adminAccount", adminAccount);
                            lnSql.cmd.Parameters.AddWithValue("@adminPWD", adminPWD);
                            lnSql.cmd.Parameters.AddWithValue("@adminID", adminID);
                        }
                        else
                        {
                            lnSql.cmd.CommandText = "update admins_tb set adminName=@adminName,adminAccount=@adminAccount where adminID=@adminID";
                            lnSql.cmd.Parameters.AddWithValue("@adminName", adminName);
                            lnSql.cmd.Parameters.AddWithValue("@adminAccount", adminAccount);
                            lnSql.cmd.Parameters.AddWithValue("@adminID", adminID);
                        }
                        lnSql.cmd.ExecuteNonQuery();
                    }
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
                //初始化页面
                if (Request.Params["did"] == null || Request.Params["did"] == "")
                {
                    //初始化增加页面
                    this.adminCpPwd.TextMode = TextBoxMode.Password;
                    this.adminPWD.TextMode = TextBoxMode.Password;
                    this.adminPWD.Text = "12345";
                    this.adminCpPwd.Text ="12345";
                }
                else
                {
                    //初始化修改页面
                    int adminID = Convert.ToInt32(Request.Params["did"]);
                    LNSql lnSql = new LNSql();
                    lnSql.conn.Open();
                    lnSql.cmd.CommandText = "select * from admins_tb where adminID="+adminID;
                    lnSql.dr = lnSql.cmd.ExecuteReader();
                    if (lnSql.dr.Read())
                    {
                        this.adminID.Value = adminID.ToString();
                        this.adminName.Text = lnSql.dr["adminName"].ToString();
                        this.adminAccount.Text = lnSql.dr["adminAccount"].ToString();
                        
                    }
                    this.adminCpPwd.TextMode = TextBoxMode.Password;
                    this.adminPWD.TextMode = TextBoxMode.Password;

                }
            }
        }
    }
}