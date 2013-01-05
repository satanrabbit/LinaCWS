using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using LitJson;
using LinaCWS.COM;

namespace LinaCWS
{
    public partial class wagesCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int status = 0;
                string msg = "未知错误！";
                //统计
                int y = Convert.ToInt32(Request.Params["wkYear"]);
                int m = Convert.ToInt32(Request.Params["wkMonth"]);
                int wkID = Convert.ToInt32(Request.Params["wkID"]);
                //本月补贴
                float wlpSubsidy = float.Parse(Request.Params["wlpSubsidy"]);
                
                //查询起薪
                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                lnSql.cmd.CommandText = "select wkSalary from workers_tb where wkID= " + wkID ;
                float wkSalary = float.Parse(lnSql.cmd.ExecuteScalar().ToString());
                
                //查询缺勤次数
                float unwork = 0;
                lnSql.cmd.CommandText = "select uwkTimes from unworks_tb where uworker= " + wkID + " and uwkYear= " + y + " and uwkMonth = "+m;
                if (lnSql.cmd.ExecuteScalar()!= null)
                {
                    unwork = float.Parse(lnSql.cmd.ExecuteScalar().ToString());
                }
                //本月天数
                int mday = (m > 12 || m <= 0) ? 0 : (m == 2 ? ((y % 4 == 0 && (y % 100 == 0 ? y % 400 == 0 : true)) ? 29 : 28) : (30 + (m <= 7 ? m : m + 1) % 2));
                
                //查询统计系数
                lnSql.cmd.CommandText = "select * from rate_tb  ";
                lnSql.da.SelectCommand = lnSql.cmd;
                lnSql.da.Fill(lnSql.ds,"rate_tb");
                
                //查询加班次数
                lnSql.cmd.CommandText = "select owkTimes from overworks_tb where oworker= " + wkID + " and owkYear= " + y + " and owkMonth = " + m;
                float owk = 0;
                if (lnSql.cmd.ExecuteScalar()!= null)
                {
                    owk = float.Parse(lnSql.cmd.ExecuteScalar().ToString());
                }

                 
                //统计工资=起薪/本月天数（本月天数-缺勤次数）+加班补贴系数*加班次数
                float wkpAll = (wkSalary / mday) * (mday - unwork) + owk * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rOverWork"].ToString()));

                //五险一金计算：
                //养老保险=统计工资*养老保险金缴纳系数
                float wlpYL = wkpAll * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rYL"].ToString()));
                //生育险=统计工资*生育险缴纳系数
                float wlpSY = wkpAll * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rSYu"].ToString()));
                //医疗保险=统计工资*医疗保险缴纳系数
                float wlpYLiao = wkpAll * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rYiLiao"].ToString()));
                //失业保险=统计工资*失业保险缴纳系数
                float wlpSYe = wkpAll * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rSYe"].ToString()));
                //工伤保险=统计工资*工伤保险缴纳系数
                float wlpGS = wkpAll * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rGS"].ToString()));
                //住房公积金=统计工资*住房公积金缴纳系数
                float wlpZF = wkpAll * (float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rZF"].ToString()));
	            //五险一金总额计算：
                //五险一金总额=养老保险+生育险+医疗保险+失业保险+工伤保险+住房公积金
                float issAll = wlpYL + wlpSY + wlpYLiao + wlpSYe + wlpGS + wlpZF;
	            //个人所得税计算
		        //个人所得税=(统计工资- 五险一金总额 – 免征额)*适用税率 - 速算扣除数
                
                //征税额
                float forTax = wkpAll - issAll - float.Parse(lnSql.ds.Tables["rate_tb"].Rows[0]["rTaxPoint"].ToString());

                float wlpTax;

                if (forTax > 0)
                {
                    //查询税率
                    lnSql.cmd.CommandText = "select * from rateTax_tb order by rt desc";
                    lnSql.da.SelectCommand = lnSql.cmd;
                    lnSql.da.Fill(lnSql.ds, "tax_tb");
                    float tax = 0;
                    float rtQuick = 0;
                    int rocount = lnSql.ds.Tables["tax_tb"].Rows.Count;
                    for (int i = 1; i < rocount; i++)
                    {
                        if (i == rocount)
                        {
                            tax = float.Parse(lnSql.ds.Tables["tax_tb"].Rows[i - 1]["rt"].ToString());
                            rtQuick = float.Parse(lnSql.ds.Tables["tax_tb"].Rows[i - 1]["rtQuick"].ToString());
                        }
                        else
                        {
                            string rtp = lnSql.ds.Tables["tax_tb"].Rows[i]["rtPoint"].ToString();
                            if (forTax > float.Parse(rtp))
                            {
                                tax = float.Parse(lnSql.ds.Tables["tax_tb"].Rows[i - 1]["rt"].ToString());
                                rtQuick = float.Parse(lnSql.ds.Tables["tax_tb"].Rows[i - 1]["rtQuick"].ToString());
                            }
                        }
                    }
                    wlpTax = forTax * tax - rtQuick;
                }
                else
                {
                    wlpTax = 0;
                }
               
                //应发工资
                //本月员工应发工资 =统计工资 - 五险一金总额 - 个人所得税+补贴额度
                float wlpShould = wkpAll - wlpTax - issAll + wlpSubsidy;
                //保存至数据库

                OleDbTransaction trans = lnSql.conn.BeginTransaction();
                lnSql.cmd.Transaction = trans;
                try
                {
                    //删除原有当年月数据
                    lnSql.cmd.CommandText = "delete from wkPay_tb where  pWorker = " +
                        wkID + "and  wkpYear= " + y
                        + " and wkpMonth =  " + m;
                    lnSql.cmd.ExecuteNonQuery();
                    //插入新数据
                    lnSql.cmd.CommandText = "insert into wkPay_tb(pWorker,wkpYear,wkpMonth,wkpAll,wlpYL,wlpSY,wlpYLiao,wlpSYe,wlpGS,wlpZF,wlpTax,wlpSubsidy,wlpShould) values (@pWorker,@wkpYear,@wkpMonth,@wkpAll,@wlpYL,@wlpSY,@wlpYLiao,@wlpSYe,@wlpGS,@wlpZF,@wlpTax,@wlpSubsidy,@wlpShould) ";
                    lnSql.cmd.Parameters.Clear();
                    lnSql.cmd.Parameters.AddWithValue("@pWorker", wkID);
                    lnSql.cmd.Parameters.AddWithValue("@wkpYear", y);
                    lnSql.cmd.Parameters.AddWithValue("@wkpMonth", m);
                    lnSql.cmd.Parameters.AddWithValue("@wkpAll", wkpAll);
                    lnSql.cmd.Parameters.AddWithValue("@wlpYL", wlpYL);
                    lnSql.cmd.Parameters.AddWithValue("@wlpSY", wlpSY);
                    lnSql.cmd.Parameters.AddWithValue("@wlpYLiao", wlpYLiao);
                    lnSql.cmd.Parameters.AddWithValue("@wlpSYe", wlpSYe);
                    lnSql.cmd.Parameters.AddWithValue("@wlpGS", wlpGS);
                    lnSql.cmd.Parameters.AddWithValue("@wlpZF", wlpZF);
                    lnSql.cmd.Parameters.AddWithValue("@wlpTax", wlpTax);
                    lnSql.cmd.Parameters.AddWithValue("@wlpSubsidy", wlpSubsidy);
                    lnSql.cmd.Parameters.AddWithValue("@wlpShould", wlpShould);

                    lnSql.cmd.ExecuteNonQuery();
                    trans.Commit();
                    status = 1;
                    msg = "统计完成";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    trans.Rollback();
                }
                finally
                {
                    lnSql.conn.Close();
                }
                JsonData jsonData = new JsonData();
                jsonData["status"] = status;
                jsonData["msg"] = msg;
                string echoString = jsonData.ToJson();

                Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                Response.Write(echoString);
                Response.End();

            }
            else
            {
                int y = Convert.ToInt32(Request.Params["wy"]);
                int m = Convert.ToInt32(Request.Params["wm"]);
                int wkID = Convert.ToInt32(Request.Params["did"]);

                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                lnSql.cmd.CommandText = "select wlpSubsidy from wkPay_tb where pWorker= " + wkID + "and wkpYear= " + y + "and wkpMonth = " + m;
                lnSql.dr = lnSql.cmd.ExecuteReader();
                if (lnSql.dr.Read())
                {
                    this.wlpSubsidy.Text = lnSql.dr["wlpSubsidy"].ToString();
                }
                else
                {
                    this.wlpSubsidy.Text = "0";
                }
                this.wkID.Value = wkID.ToString();
                this.wkMonth.Value = m.ToString();
                this.wkYear.Value = y.ToString();
                lnSql.dr.Close();
                lnSql.conn.Close();
            }
        }
    }
}