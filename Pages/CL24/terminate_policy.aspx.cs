using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CL24_Terminate_Policy : System.Web.UI.Page
{
    DataTable list_renewal_prem = new DataTable();
    DataTable tbl = new DataTable();
    DataTable tbl_new_policy = new DataTable();
    string user_name = "";
    string message = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        DateTime reportDate;

        if (txtReportDate.Text.Trim() != "")
        {
            reportDate = DateTime.Parse(txtReportDate.Text, dtfi);

            list_renewal_prem = da_policy_cl24.GetPolicyByReportDateList(reportDate);
            txtReportDate.Text = "";
        }
        else
        {
            message = "Please check your input!!!";
            list_renewal_prem = new DataTable();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
        }

        if (list_renewal_prem.Rows.Count > 0)
        {
            ViewState["POLICY_GENERATED"] = list_renewal_prem;
        }

        gv_report.DataSource = list_renewal_prem;
        gv_report.DataBind();

        foreach (GridViewRow row in gv_report.Rows)
        {
            Label Policy_Number = (Label)row.FindControl("lblPolNo");
            Label Pay_Year = (Label)row.FindControl("lblPayYear");

            DataTable dt = da_policy_cl24.GetPolicyStatusRemark(Policy_Number.Text, Pay_Year.Text != "" ? Convert.ToInt16(Pay_Year.Text) : 0);

            if (dt.Rows.Count > 0)
            {
                Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                Button btnEDIT = (Button)row.FindControl("btnEDITRemark");
                btnINSERT.Visible = false;
                btnEDIT.Visible = true;
            }
        }

        /*show record count*/
        lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + list_renewal_prem.Rows.Count;
        ViewState["RENEWAL_PREM_DATA"] = list_renewal_prem;
    }

    protected void ImgBtnTerminate_Click(object sender, ImageClickEventArgs e)
    {
        DataTable myTable = new DataTable();
        DataTable dt_policy = new DataTable();

        try
        {
            myTable = (DataTable)ViewState["POLICY_GENERATED"];
            if (myTable.Rows.Count > 0)
            {
                dt_policy = myTable.Clone();
                foreach (GridViewRow row in gv_report.Rows)
                {
                    Label Policy_Number = (Label)row.FindControl("lblPolNo");
                    CheckBox myChkBox = (CheckBox)row.FindControl("Chkbx");

                    foreach (DataRow item in myTable.Rows)
                    {
                        if (Policy_Number.Text == item["Policy_Number"].ToString().Trim())
                        {
                            if (myChkBox.Checked)
                            {

                                bl_cl24_renewal_premium renewal_prem;
                                renewal_prem = new bl_cl24_renewal_premium();
                                renewal_prem.Policy_ID = item["Policy_ID"].ToString().Trim();
                                renewal_prem.Policy_Number = item["Policy_Number"].ToString().Trim();
                                renewal_prem.Pay_Year = Convert.ToInt16(item["Pay_Year"].ToString().Trim());

                                DataTable dt = da_policy_cl24.GetPolicyStatusRemark(renewal_prem.Policy_Number, renewal_prem.Pay_Year);
                                if (dt.Rows.Count > 0)
                                {
                                    renewal_prem.Paid_Off_Date = Convert.ToDateTime(dt.Rows[0]["Paid_Off_Date"].ToString().Trim());
                                    renewal_prem.Status_Remark = dt.Rows[0]["Status"].ToString();
                                    renewal_prem.Remark = dt.Rows[0]["Remark"].ToString();

                                }

                                if (da_policy_cl24.Save_CL24_Paid_Off_Policy(renewal_prem) != false)
                                { da_policy_cl24.RemoveStatusRemark(renewal_prem.Policy_Number, renewal_prem.Pay_Year); } // ROLL BACK REMARK 

                                Policy_Number.Text = "";
                                break;
                            }
                            else
                            {
                                dt_policy.ImportRow(item);
                            }

                            Policy_Number.Text = "";
                            break;
                        }
                    }
                }

                ViewState["POLICY_GENERATED"] = dt_policy;
                gv_report.DataSource = dt_policy;
                gv_report.DataBind();

                foreach (GridViewRow row in gv_report.Rows)
                {
                    Label Policy_Number = (Label)row.FindControl("lblPolNo");
                    Label Pay_Year = (Label)row.FindControl("lblPayYear");

                    DataTable dt = da_policy_cl24.GetPolicyStatusRemark(Policy_Number.Text, Pay_Year.Text != "" ? Convert.ToInt16(Pay_Year.Text) : 0);

                    if (dt.Rows.Count > 0)
                    {
                        Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                        Button btnEDIT = (Button)row.FindControl("btnEDITRemark");
                        btnINSERT.Visible = false;
                        btnEDIT.Visible = true;
                    }
                }

                /*show record count*/
                lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + dt_policy.Rows.Count;
                ViewState["RENEWAL_PREM_DATA"] = dt_policy;

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ImgBtnSave_Click()], detail:" + ex.Message);
        }

    }

    protected void btnSaveRemark_Click(object sender, EventArgs e)
    {
        string policy_number = "", pay_year = "", status = "", remark = "";
        DateTime TerminateDate;

        policy_number = hdfINSERTPolicyNumber.Value;
        pay_year = hdfPayYear.Value;
        status = ddlINSERTStatus.SelectedValue;
        TerminateDate = DateTime.ParseExact(txtINSERTPaidOFFDate.Text, "dd/MM/yyyy", null); 
        remark = txtINSERTRemarks.Text;

        if (policy_number != "" && status.Trim() != "--" && txtINSERTPaidOFFDate.Text != "")
        {
            if (da_policy_cl24.InsertStatusRemark(policy_number, Convert.ToInt16(pay_year), status, remark, TerminateDate, user_name))
            {
                foreach (GridViewRow row in gv_report.Rows)
                {
                    Label Policy_Number = (Label)row.FindControl("lblPolNo");
                    Label Pay_Year = (Label)row.FindControl("lblPayYear");

                    if (policy_number == Policy_Number.Text && pay_year == Pay_Year.Text)
                    {
                        Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                        Button btnEDIT = (Button)row.FindControl("btnEDITRemark");
                        btnINSERT.Visible = false;
                        btnEDIT.Visible = true;
                    }
                }
            }
        }
        else
        {
            message = "Please choose inputs!!";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
        }

    }

    protected void btnEDITRemark_Click(object sender, EventArgs e)
    {
        string policy_number = "", pay_year = "", status = "", remark = "";
        DateTime PaidOFFDate;

        policy_number = hdfEDITPolicyNumber.Value;
        status = ddlEditStatus.SelectedValue;
        PaidOFFDate = DateTime.ParseExact(txtEDITPaidOFFDate.Text, "dd/MM/yyyy", null); 

        pay_year = hdfPayYear.Value;

        remark = txtEDITRemarks.Text;

        if (policy_number != "" )
        {
            if (status.Trim() != "--")
                da_policy_cl24.EditStatusRemark(policy_number, pay_year != "" ? Convert.ToInt16(pay_year) : 0, status, remark, PaidOFFDate);
            else
                da_policy_cl24.RemoveStatusRemark(policy_number, pay_year != "" ? Convert.ToInt16(pay_year) : 0);

            foreach (GridViewRow row in gv_report.Rows)
            {
                Label Policy_Number = (Label)row.FindControl("lblPolNo");

                if (policy_number == Policy_Number.Text && pay_year == hdfPayYear.Value)
                {
                    Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                    Button btnEDIT = (Button)row.FindControl("btnEDITRemark");

                    if (status.Trim() != "--") { btnINSERT.Visible = false; btnEDIT.Visible = true; } 
                    else { btnINSERT.Visible = true; btnEDIT.Visible = false; }
                }
            }
        }

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        string policy_number = "", pay_year = "";
        policy_number = hdfEDITPolicyNumber.Value;
        pay_year = hdfPayYear.Value;

        if (policy_number != "")
        {
            da_policy_cl24.RemoveStatusRemark(policy_number, Convert.ToInt16(pay_year));

            foreach (GridViewRow row in gv_report.Rows)
            {
                Label Policy_Number = (Label)row.FindControl("lblPolNo");
                Label Pay_Year = (Label)row.FindControl("lblPayYear");

                if (policy_number == Policy_Number.Text && pay_year == Pay_Year.Text)
                {
                    Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                    Button btnEDIT = (Button)row.FindControl("btnEDITRemark");

                    btnINSERT.Visible = true; btnEDIT.Visible = false; 
                }
            }
        }

    }
}