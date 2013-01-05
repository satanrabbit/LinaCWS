using System;
using System.Collections.Generic;
using System.Web;
using LinaCWS.COM;
using LitJson;
using System.Web.SessionState;
using System.Data;

namespace LinaCWS
{
    /// <summary>
    /// wagesList 的摘要说明
    /// </summary>
    public class wagesList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            int total = 0;
            JsonData rows = new JsonData();
            if (context.Session["adminID"] == null)
            {
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write("未登录或登录超时！请重新登陆！");
                context.Response.End();
            }
            else
            {
                int y = DateTime.Now.Year;
                int m = DateTime.Now.Month;
                if (context.Request.Params["y"] == "" || context.Request.Params["y"] == null)
                {
                }
                else
                {
                    y = Convert.ToInt32(context.Request.Params["y"]);
                }
                if (context.Request.Params["y"] == "" || context.Request.Params["y"] == null)
                {
                }
                else
                {
                    m = Convert.ToInt32(context.Request.Params["m"]);
                }

                int page = 0;
                if (context.Request["page"] != null && context.Request["page"] != "")
                {
                    page = Convert.ToInt32(context.Request["page"]) - 1;
                }
                int pageSize = 15;
                if (context.Request["rows"] != null && context.Request["rows"] != "")
                {
                    pageSize = Convert.ToInt32(context.Request["rows"]);
                }

                string sort = "wkEntryTime";
                if (context.Request["sort"] != null && context.Request["sort"] != "")
                {
                    sort = (context.Request["sort"]);
                }
                string order = "asc";
                if (context.Request["order"] != null && context.Request["order"] != "")
                {
                    order = (context.Request["order"]);
                }

                LNSql lnSql = new LNSql();

                lnSql.conn.Open();
                if (page == 0)
                {
                    lnSql.cmd.CommandText = "select top " + pageSize
                        + " * FROM  workers_tb  order by " + sort + " " + order + " ";
                }
                else
                {
                    lnSql.cmd.CommandText = "select top " + pageSize +
                        " *  FROM workers_tb  where wkID not in ( select top " + page * pageSize +
                        " wkID from from workers_tb order by order by " + sort + " " + order + " )  order by " + sort + " " + order + " )";
                }

                lnSql.da.SelectCommand = lnSql.cmd;
                lnSql.da.Fill(lnSql.ds, "workers_tb");

                //wkID字符串
                string wstr = "0";
                if (lnSql.ds.Tables["workers_tb"].Rows.Count > 0)
                {
                    foreach (DataRow dr in lnSql.ds.Tables["workers_tb"].Rows)
                    {
                        wstr += "," + dr["wkID"].ToString();
                    }
                    //查询工资情况
                    lnSql.cmd.CommandText = "select * from ( wkPay_tb left join admins_tb on wkPay_tb.wlpAdmin= admins_tb.adminID ) where pWorker in (" + wstr + ") and wkpYear = " + y + " and wkpMonth = " + m + " ";

                    lnSql.da.SelectCommand = lnSql.cmd;
                    lnSql.da.Fill(lnSql.ds, "wkp_tb");
                     

                    //转化为字符串

                    foreach (DataRow dr in lnSql.ds.Tables["workers_tb"].Rows)
                    {
                        JsonData row = new JsonData();
                        row["wkID"] = dr["wkID"].ToString();
                        row["wkName"] = dr["wkName"].ToString();
                        row["wkPosition"] = dr["wkPosition"].ToString();
                        row["wkSalary"] = float.Parse( dr["wkSalary"].ToString());
                        row["wkYear"] = y;
                        row["wkMonth"] = m;
                        //工资情况
                        DataRow[] wkpDRs = (lnSql.ds.Tables["wkp_tb"].Select("pWorker=" + dr["wkID"].ToString()));
                        if (wkpDRs.Length > 0)
                        {
                            row["wkpAll"] = float.Parse(wkpDRs[0]["wkpAll"].ToString());
                            row["wlpYL"] = float.Parse(wkpDRs[0]["wlpYL"].ToString());
                            row["wlpSY"] = float.Parse(wkpDRs[0]["wlpSY"].ToString());
                            row["wlpYLiao"] = float.Parse(wkpDRs[0]["wlpYLiao"].ToString());
                            row["wlpSYe"] = float.Parse(wkpDRs[0]["wlpSYe"].ToString());
                            row["wlpGS"] = float.Parse(wkpDRs[0]["wlpGS"].ToString());
                            row["wlpZF"] = float.Parse(wkpDRs[0]["wlpZF"].ToString());
                            row["wlpTax"] = float.Parse(wkpDRs[0]["wlpTax"].ToString());
                            row["wlpSubsidy"] = float.Parse(wkpDRs[0]["wlpSubsidy"].ToString());
                            row["wlpShould"] = float.Parse(wkpDRs[0]["wlpShould"].ToString());
                            row["wlpReal"] = float.Parse(wkpDRs[0]["wlpReal"].ToString());
                            row["wkpID"] =Convert.ToInt32( wkpDRs[0]["wkpID"].ToString());
                            if (wkpDRs[0]["wlpAdmin"].ToString() == null)
                            {
                                row["wlpAdmin"] = "";
                                row["adminName"] = "未发";
                            }
                            else
                            {
                                row["wlpAdmin"]= wkpDRs[0]["wlpAdmin"].ToString();
                                row["adminName"] = wkpDRs[0]["adminName"].ToString();
                            }
                            if (wkpDRs[0]["wlpTime"].ToString() == "")
                            {
                                row["wlpTime"] = "";
                            }
                            else
                            {
                                row["wlpTime"] = ((DateTime)wkpDRs[0]["wlpTime"]).ToString("yyyy-MM-dd");
                            }                             
                        }
                        else
                        {
                            row["wkpAll"] = "";
                            row["wlpYL"] = "";
                            row["wlpSY"] ="";
                            row["wlpYLiao"] = "";
                            row["wlpSYe"] = "";
                            row["wlpGS"] = "";
                            row["wlpZF"] = "";
                            row["wlpTax"] = "";
                            row["wlpSubsidy"] = "";
                            row["wlpShould"] = "";
                            row["wlpReal"] = "";
                            row["wlpAdmin"] ="";
                            row["wlpTime"] ="";
                            row["adminName"] = "未发";
                            row["wkpID"] = "";
                        }
                        
                        rows.Add(row);
                    }


                }
                else
                {
                    rows = "";
                }

                lnSql.cmd.CommandText = "";

                //查询数量 

                lnSql.cmd.CommandText = "select count(wkID) from workers_tb";

                total = Convert.ToInt32(lnSql.cmd.ExecuteScalar());

                lnSql.conn.Close();

                JsonData jsonData = new JsonData();
                jsonData["total"] = total;
                jsonData["rows"] = rows;
                string echoString = jsonData.ToJson();

                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(echoString);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}