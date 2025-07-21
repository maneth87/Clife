using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
public partial class Pages_CI_TerminateSO : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    string user_name = "";
    string product_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            permission = (bl_sys_user_role)Session["SS_PERMISSION"];
            user_name = System.Web.Security.Membership.GetUser().UserName;

            if (Request.QueryString.Count > 0)
            {
                product_id = Request.QueryString["pro_type"];

            }
            else
            {
                product_id = "SO,CI";
            }
            // LoadData("", "", "", product_id);

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Data cannot display, please contact your system administrator.');", true);
            Log.AddExceptionToLog("Error function [Page_Load(object sender, EventArgs e)] in page [TerminateSO.aspx.cs], detail:" + ex.Message);
        }
    }

    void LoadData(string customer_name, string gender, string policy_number, string product_id)
    {


        string filter = "";
        //get all S.O policies
        DataTable tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), customer_name, policy_number, product_id);
        filter = gender.Trim() == "" ? "" : " gender='" + gender + "'";

        filter += filter.Trim() == "" ? " policy_status in ('IF','LAP')" : " and policy_status in ('IF','LAP')";//get only IF status
        DataTable tbl_filter = tbl.Clone();//clone propertity from table tbl such columns name
        int a = tbl_filter.Rows.Count;
        DataRow row_filter;
        foreach (DataRow row in tbl.Select(filter))//filter data in table tbl
        {
            row_filter = tbl_filter.NewRow();
            //copy data from tbl to tbl_filter base on codition
            for (int col = 0; col < tbl_filter.Columns.Count; col++)
            {
                row_filter[col] = row[col].ToString();
            }

            tbl_filter.Rows.Add(row_filter);
        }

        if (tbl_filter.Rows.Count > 0)
        {
            gv_policy.DataSource = tbl_filter;
            gv_policy.DataBind();

            string desc = txtCustomerName.Text.Trim() == "" ? "" : string.Concat(", Customer Name:", txtCustomerName.Text.Trim());
            desc += ddlGender.SelectedIndex == 0 ? "" : string.Concat(" , Gender:", ddlGender.SelectedItem.Text);
            desc += txtPolicyNumber.Text.Trim() == "" ? "" : string.Concat(" , Pol No:", txtPolicyNumber.Text.Trim());
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries policies for terminate with criteria:[", desc,"]."));
        }
    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        gv_policy.DataSource = null;
        gv_policy.DataBind();
        LoadData(txtCustomerName.Text.Trim(), ddlGender.SelectedValue, txtPolicyNumber.Text.Trim(), product_id);
    }
    protected void btnTerminate_Click(object sender, EventArgs e)
    {
        if (hfdPolicyID.Value.Trim() != "")
        {
            try
            {
                bool result = da_ci.Policy.TerminatePolicy(hfdPolicyID.Value, user_name, DateTime.Now, txtRemarks.Text.Trim());
                if (result)//terminate success
                {
                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE, string.Concat("User updates policy status to terminate.[Pol No:", hfdPolicyNumber.Value,"]."));
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Policy is terminated successfully.');", true);
                    LoadData("", "", "", product_id);
                }
                else//terminate fail or error
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Policy is terminated fail, please contact your system administrator.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Policy is terminated fail, please contact your system administrator.');", true);
                //save error log
                Log.AddExceptionToLog("Error function [btnTerminate_Click(object sender, EventArgs e)] in page [TerminateSO.apx.cs], detail:" + ex.Message);
            }
        }
    }

    protected void lbtUpload_Click(object sender, EventArgs e)
    {
        if (product_id.Trim() == "AL")
        {
            Response.Redirect("~/Pages/CI/Terminate_so_upload.aspx?pro_type=AL");
        }
        else
        {
            Response.Redirect("~/Pages/CI/Terminate_so_upload.aspx");
        }
    }
    protected void gv_policy_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}