using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Pages_CI_Report_frmCIDetailReport : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    string product_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (Request.QueryString.Count > 0)
        {
            product_id = Request.QueryString["pro_type"];
        }
        else
        {
            product_id = "";
        }

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string url = "";
        if (rbtlReportType.SelectedValue == "1")
        {
            url = "frmCILoadReport.aspx";
        }
        else if (rbtlReportType.SelectedValue == "2")
        {
            url = "frmLoadPremiumDetailReport.aspx";
        }
        Session["PARAS"] = new string[] { (txtFromDate.Text.Trim() != "") ? txtFromDate.Text.Trim() : "", (txtTodate.Text.Trim()!="") ? txtTodate.Text.Trim() : "", txtCustomerName.Text.Trim(), txtPolicyNumber.Text.Trim(),"pdf", product_id=="" ? "SO,CI": "AL" };
        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string url = "";
        if (rbtlReportType.SelectedValue == "1")
        {
            url = "frmCILoadReport.aspx";
        }
        else if (rbtlReportType.SelectedValue == "2")
        {
            url = "frmLoadPremiumDetailReport.aspx";
        }
        Session["PARAS"] = new string[] { (txtFromDate.Text.Trim() != "") ? txtFromDate.Text.Trim() : "", (txtTodate.Text.Trim() != "") ? txtTodate.Text.Trim() : "", txtCustomerName.Text.Trim(), txtPolicyNumber.Text.Trim(), "excel", product_id == "" ? "SO,CI" : "AL" };
        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
    }
}