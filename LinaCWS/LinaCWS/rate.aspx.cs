using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinaCWS.COM;

namespace LinaCWS
{
    public partial class rate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LNSql lnSql = new LNSql();
            lnSql.conn.Open();
            lnSql.cmd.CommandText = "select * from rate_tb ";
            lnSql.dr = lnSql.cmd.ExecuteReader();
            if (lnSql.dr.Read())
            {
                this.rOverWork.Text = lnSql.dr["rOverWork"].ToString();
                this.rYL.Text = (float.Parse(lnSql.dr["rYL"].ToString()) * 100).ToString();
                this.rSYu.Text = (float.Parse(lnSql.dr["rSYu"].ToString()) * 100).ToString();
                this.rYiLiao.Text = (float.Parse(lnSql.dr["rYiLiao"].ToString()) * 100).ToString();
                this.rSYe.Text = (float.Parse(lnSql.dr["rSYe"].ToString()) * 100).ToString();
                this.rGS.Text = (float.Parse(lnSql.dr["rGS"].ToString()) * 100).ToString();
                this.rZF.Text = (float.Parse(lnSql.dr["rZF"].ToString()) * 100).ToString();
                this.rTaxPoint.Text = ( lnSql.dr["rTaxPoint"].ToString())  ;
            }
            lnSql.dr.Close();
            //设置个税率
            lnSql.cmd.CommandText = "select * from rateTax_tb ";
            lnSql.da.SelectCommand = lnSql.cmd;
            lnSql.da.Fill(lnSql.ds,"rts_tb");
            this.RepeatRt.DataSource = lnSql.ds.Tables["rts_tb"].DefaultView;
            this.RepeatRt.DataBind();
            lnSql.conn.Close();

        }
    }
}