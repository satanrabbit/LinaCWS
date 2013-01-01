using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
namespace LinaCWS
{
    public partial class LogOn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                JsonData data2 = new JsonData();
                data2["name"] = "peiandsky";
                data2["info"] = new JsonData();
                data2["info"]["sex"] = "male";
                data2["info"]["age"] = 28;
                string json2 = data2.ToJson();
                Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                Response.Write(json2);
                Response.End();
            }
        }
    }
}