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
    /// workLlist 的摘要说明
    /// </summary>
    public class workList : IHttpHandler, IRequiresSessionState
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
                lnSql.da.Fill(lnSql.ds,"workers_tb");

                //wkID字符串
                string wstr = "0";
                if (lnSql.ds.Tables["workers_tb"].Rows.Count > 0)
                {
                    foreach (DataRow dr in lnSql.ds.Tables["workers_tb"].Rows)
                    {
                        wstr+=","+dr["wkID"].ToString();
                    }
                    //查询缺勤情况
                    lnSql.cmd.CommandText = "select * from unworks_tb where uworker in ("+wstr+") and uwkYear = "+y+" and uwkMonth = "+ m+" ";

                    lnSql.da.SelectCommand = lnSql.cmd;
                    lnSql.da.Fill(lnSql.ds,"uwks_tb");

                    //查询加班情况
                    lnSql.cmd.CommandText = "select * from overworks_tb where oworker in (" + wstr + ") and owkYear = " + y + " and owkMonth = " + m + " ";

                    lnSql.da.SelectCommand = lnSql.cmd;
                    lnSql.da.Fill(lnSql.ds, "owks_tb");

                    //转化为字符串

                    foreach (DataRow dr in lnSql.ds.Tables["workers_tb"].Rows)
                    {
                        JsonData row = new JsonData();
                        row["wkID"] = dr["wkID"].ToString();
                        row["wkName"] = dr["wkName"].ToString();
                        row["wkPosition"] = dr["wkPosition"].ToString();
                        row["wkYear"] = y;
                        row["wkMonth"] = m;
                        //缺勤
                        DataRow[] uwkDRs=(lnSql.ds.Tables["uwks_tb"].Select("uworker=" + dr["wkID"].ToString()));
                        if(uwkDRs.Length>0){
                            row["uwkTimes"] = float.Parse(uwkDRs[0]["uwkTimes"].ToString());
                        }else{
                             row["uwkTimes"] = 0;
                        }
                        //缺勤
                        DataRow[] owkDRs = (lnSql.ds.Tables["owks_tb"].Select("oworker=" + dr["wkID"].ToString()));
                        if (owkDRs.Length > 0)
                        {
                            row["owkTimes"] = float.Parse(owkDRs[0]["owkTimes"].ToString());
                        }
                        else
                        {
                            row["owkTimes"] = 0;
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