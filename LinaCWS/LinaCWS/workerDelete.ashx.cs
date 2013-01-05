using System;
using System.Collections.Generic;
using System.Web;
using LinaCWS.COM;
using LitJson;
using System.Web.SessionState;

namespace LinaCWS
{
    /// <summary>
    /// workerDelete 的摘要说明
    /// </summary>
    public class workerDelete : IHttpHandler,IRequiresSessionState
    
    {

        public void ProcessRequest(HttpContext context)
        {
            int status = 0;
            string msg = "未知错误！";

            if (context.Request.Params["did"] == "" || context.Request.Params["did"] == null)
            {
                msg = "未提交删除数据！";
            }
            else
            {
                LNSql lnSql = new LNSql();
                lnSql.conn.Open();
                lnSql.cmd.CommandText = "delete from  workers_tb where wkID in (0," + context.Request.Params["did"] + ")";
                try
                {
                    int effect = lnSql.cmd.ExecuteNonQuery();
                    status = 1;
                    msg = "成功删除" + effect + "条记录！";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                finally
                {
                    lnSql.conn.Close();
                }
            }



            JsonData jsonData = new JsonData();
            jsonData["status"] = status;
            jsonData["msg"] = msg;
            string echoString = jsonData.ToJson();

            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(echoString);
            context.Response.End();
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