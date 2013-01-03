using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using SRLib;
using System.Configuration;
using LinaCWS.COM;
namespace LinaCWS
{
    public partial class LogOn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int status = 0;
                string msg = "未知错误";
                string account = Request.Form["account"];
                string pwd = Request.Form["pwd"];
                SrCom srCom = new SrCom();
                //加密
                pwd = srCom.HashPassword(pwd);

                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                lnSql.cmd.CommandText = "select * from admins_tb where adminAccount=@account";
                lnSql.cmd.Parameters.AddWithValue("@account",account);
                lnSql.dr = lnSql.cmd.ExecuteReader();
                if (lnSql.dr.Read())
                {
                    if (pwd == lnSql.dr["adminPwd"].ToString())
                    {
                        //登录成功,设置登录sesseion
                        Session["adminID"] = lnSql.dr["adminID"].ToString();
                        Session["adminName"] = lnSql.dr["adminName"].ToString();
                        Session["adminClass"] = lnSql.dr["adminClass"].ToString();
                        status = 1;
                        msg = "登录成功";
                    }
                    else
                    {
                        msg = "密码错误！" + pwd;
                    }
                }
                else
                {
                    msg = "账号不存在！";
                }
                lnSql.conn.Close();

                JsonData data = new JsonData();
                data["status"] =status;
                data["msg"] = msg; 
                string json = data.ToJson();
                Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                Response.Write(json);
                Response.End();
            }
        }
    }
}