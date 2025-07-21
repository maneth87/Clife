using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
public partial class Pages_CI_terminated_policy : System.Web.UI.Page
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
            product_id = "SO,CI";
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        process_report("excel");
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        process_report("pdf");
    }
    
    void process_report( string report_type)
    {
        string url = "frmLoadTerminateReport.aspx";

        string logDec= txtTodate.Text.Trim() !="" && txtFromDate.Text.Trim()!="" ? string.Concat( " Terminated date from:", txtFromDate.Text.Trim()," to:", txtTodate.Text.Trim()) :"";
        logDec += txtCustomerName.Text.Trim() != "" ? (logDec == "" ? string.Concat(" Customer name:", txtCustomerName.Text.Trim()) : string.Concat(", Customer name:", txtCustomerName.Text.Trim())) : "";
        logDec += txtPolicyNumber.Text.Trim() != "" ? (logDec == "" ? string.Concat(" Policy Number:", txtPolicyNumber.Text.Trim()) : string.Concat(", Policy Number:", txtPolicyNumber.Text.Trim())) : "";
        if (report_type == "excel")
        {
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports terminated policy report to excel file with criteria [",logDec,"]"));
        }
        else if (report_type == "pdf")
        {
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User view terminated policy report in pdf with criteria [",logDec,"]"));
        }
        Session["PARAS"] = new string[] { (txtFromDate.Text.Trim() != "") ? txtFromDate.Text.Trim() : "", (txtTodate.Text.Trim() != "") ? txtTodate.Text.Trim() : "", txtCustomerName.Text.Trim(), txtPolicyNumber.Text.Trim(), report_type, product_id  };
        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
    }
}