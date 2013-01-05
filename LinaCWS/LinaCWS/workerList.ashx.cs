using System;
using System.Collections.Generic;
using System.Web;
using LinaCWS.COM;
using LitJson;
using System.Web.SessionState;

namespace LinaCWS
{
    /// <summary>
    /// worker_list 的摘要说明
    /// 获取员工信息的列表
    /// </summary>
    public class workerList : IHttpHandler,IRequiresSessionState
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
                        + " * from  workers_tb order by " + sort + " " + order + " ";
                }
                else
                {
                    lnSql.cmd.CommandText = "select top " + pageSize +
                        " * from workers_tb where wkID not in ( select top " + page * pageSize +
                        " wkID from workers_tb order by order by " + sort + " " + order + " )  order by " + sort + " " + order + " )";
                }

                lnSql.dr = lnSql.cmd.ExecuteReader();
                if (lnSql.dr.Read())
                {
                    do
                    {
                        JsonData row = new JsonData();
                        row["wkID"] = lnSql.dr["wkID"].ToString();
                        row["wkName"] = lnSql.dr["wkName"].ToString();
                        row["wkPosition"] = lnSql.dr["wkPosition"].ToString();
                        row["wkSalary"] = lnSql.dr["wkSalary"].ToString();
                        row["wkEntryTime"] = ((DateTime)lnSql.dr["wkEntryTime"]).ToString("yyyy年MM月dd日");                        
                        rows.Add(row);
                    } while (lnSql.dr.Read());
                }
                else
                {
                    rows = "";
                }
                //查询数量
                lnSql.dr.Close();

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