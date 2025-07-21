using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CL24_renewal_premium : System.Web.UI.Page
{

    DataTable tbl = new DataTable();
    DataTable tbl_new_policy = new DataTable();
    string user_name = "";

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
            ImgBtnSave.Enabled = false;

            DateTime month = new DateTime((DateTime.Now.Year - 5), 1, 01);
            DateTime NextMonth = new DateTime();

            for (int i = 0; i <= 12; i++)
            {
                ListItem list = new ListItem();
                if (i == 0)
                {
                    list.Text = "-- Select --";
                    list.Value = "0";
                }
                else if(i == 1)
                {
                    list.Text = NextMonth.ToString("MMMM");
                    list.Value = NextMonth.Month.ToString();
                }
                else
                {
                    NextMonth = month.AddMonths(i - 1);
                    list.Text = NextMonth.ToString("MMMM");
                    list.Value = NextMonth.Month.ToString();
                }

                ddlNextDueMonth.Items.Add(list);
            }

            for (int i = 0; i < 7; i++)
            {
                ListItem list = new ListItem();

                if (i == 0)
                {
                    list.Text = "-- Select --";
                    list.Value = "0";
                }
                else
                {
                    DateTime DueYear = month.AddYears(i);
                    list.Text = DueYear.Year.ToString();
                    list.Value = DueYear.Year.ToString();
                }

                ddlNextDueYear.Items.Add(list);
                txtReport_date.Enabled = false;
            }

        }

    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        string message = "";

        if (ddlNextDueMonth.SelectedValue != "0" && ddlNextDueYear.SelectedValue != "0")
        {
            //tbl_new_policy = da_policy_cl24.GetNewPremiumList(ddlNextDueMonth.SelectedValue, ddlNextDueYear.SelectedValue);
            tbl = da_policy_cl24.Generate_CL24_Renewal_Premium(ddlNextDueMonth.SelectedValue, ddlNextDueYear.SelectedValue, hdfusername.Value);

            // NEW PREMIUM LIST
            //if (tbl_new_policy.Rows.Count > 0)
            //{
            //    ViewState["NEW_POLICY"] = tbl_new_policy;
            //    ImgBtnSave.Enabled = true;

            //    gv_new_policy.DataSource = tbl_new_policy;
            //    gv_new_policy.DataBind();
            //    dvNewPolicy.Visible = true;
            //}
            //else
            //{
            //    dvNewPolicy.Visible = false;
            //}

            // RENEWAL PREMIUM LIST
            if (tbl.Rows.Count > 0)
            {
                ViewState["POLICY_GENERATED"] = tbl;
                ImgBtnSave.Enabled = true;
                txtReport_date.Enabled = true;

                gv_report.DataSource = tbl;
                gv_report.DataBind();
            }

            foreach (GridViewRow row in gv_report.Rows)
            {
                Label Policy_Number = (Label)row.FindControl("lblPolNo");

                if (da_policy_cl24.GetPolicyRemark(Policy_Number.Text) != "")
                {
                    Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                    Button btnEDIT = (Button)row.FindControl("btnEDITRemark");
                    btnINSERT.Visible = false;
                    btnEDIT.Visible = true;
                }
            }
        }
        else
        {
            message = "Choose Month & Year!!!";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showFailMessage('" + message + "');", true);
        }
    }

    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        DataTable myTable = new DataTable();
        DataTable dt_policy = new DataTable();
        string message = "";
        try
        {
            myTable = (DataTable)ViewState["POLICY_GENERATED"];
            if (myTable.Rows.Count > 0)
            {
                if (txtReport_date.Text.Trim() != "")
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
                                if (myChkBox.Checked) {

                                    bl_cl24_renewal_premium renewal_prem;
                                    renewal_prem = new bl_cl24_renewal_premium();
                                    renewal_prem.Policy_ID = item["Policy_ID"].ToString().Trim();
                                    renewal_prem.Policy_Number = item["Policy_Number"].ToString().Trim();
                                    renewal_prem.Customer_ID = item["Customer_ID"].ToString().Trim();
                                    renewal_prem.Insured_Name = item["Insured_Name"].ToString().Trim();
                                    renewal_prem.Birth_Date = Convert.ToDateTime(item["Birth_Date"].ToString().Trim());
                                    renewal_prem.Gender = item["Gender"].ToString().Trim();
                                    renewal_prem.Phone_Number = item["Phone_Number"].ToString().Trim();
                                    renewal_prem.Sum_Insure = item["Sum_Insure"].ToString() != "" ? float.Parse(item["Sum_Insure"].ToString().Trim()) : 0;
                                    renewal_prem.Effective_Date = Convert.ToDateTime(item["Effective_Date"].ToString().Trim());
                                    renewal_prem.Issued_Date = Convert.ToDateTime(item["Issue_Date"].ToString().Trim());
                                    renewal_prem.Start_Date = Convert.ToDateTime(item["Start_Date"].ToString().Trim());
                                    renewal_prem.End_Date = Convert.ToDateTime(item["End_Date"].ToString().Trim());
                                    renewal_prem.Due_Date = Convert.ToDateTime(item["Due_Date"].ToString().Trim());
                                    renewal_prem.Product_ID = item["Product_ID"].ToString().Trim();
                                    renewal_prem.Product_Name = item["Product_Name"].ToString().Trim();
                                    renewal_prem.Premium = item["Premium"].ToString() != "" ? float.Parse(item["Premium"].ToString().Trim()) : 0;
                                    renewal_prem.EM_Amount = item["EM_Amount"].ToString() != "" ? float.Parse(item["EM_Amount"].ToString().Trim()) : 0;
                                    renewal_prem.Next_Due_Date = Convert.ToDateTime(item["Next_Due"].ToString().Trim());
                                    renewal_prem.Pay_Year = Convert.ToInt16(item["Pay_Year"].ToString().Trim());
                                    renewal_prem.Pay_Lot = Convert.ToInt16(item["Pay_Lot"].ToString().Trim());
                                    renewal_prem.Agent_Code = item["Agent_Code"].ToString().Trim();
                                    renewal_prem.Agent_Name = item["Agent_Name"].ToString().Trim();
                                    renewal_prem.Policy_Year = item["Policy_Year"].ToString().Trim();
                                    renewal_prem.Pay_Mode_ID = Convert.ToInt16(item["Pay_Mode_ID"].ToString().Trim());
                                    renewal_prem.Paymode_Mode = item["Payment_Mode"].ToString().Trim();
                                    renewal_prem.Factor = Convert.ToInt16(item["Factor"].ToString().Trim());
                                    renewal_prem.Total_Premium = item["Total_Premium"].ToString() != "" ? float.Parse(item["Total_Premium"].ToString().Trim()) : 0;
                                    renewal_prem.Created_By = user_name;
                                    renewal_prem.Report_Date = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
                                    renewal_prem.Remark = da_policy_cl24.GetPolicyRemark(renewal_prem.Policy_Number);

                                    if (da_policy_cl24.Save_CL24_Renewal_Premium(renewal_prem) != false)
                                    { da_policy_cl24.RollBackRemark(renewal_prem.Policy_Number); } // ROLL BACK REMARK 

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

                    txtReport_date.Text = "";

                    ViewState["POLICY_GENERATED"] = dt_policy;
                    gv_report.DataSource = dt_policy;
                    gv_report.DataBind();

                    foreach (GridViewRow row in gv_report.Rows)
                    {
                        Label Policy_Number = (Label)row.FindControl("lblPolNo");

                        if (da_policy_cl24.GetPolicyRemark(Policy_Number.Text) != "")
                        {
                            Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                            Button btnEDIT = (Button)row.FindControl("btnEDITRemark");
                            btnINSERT.Visible = false;
                            btnEDIT.Visible = true;
                        }
                    }
                }
                else
                {
                    message = "Please choose Report Date!!";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
                }

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ImgBtnSave_Click()], detail:" + ex.Message);
        }

    }

    protected void btnSaveRemark_Click(object sender, EventArgs e)
    {
        string policy_number = "", remark = "";
        policy_number = hdfINSERTPolicyNumber.Value;
        remark = txtINSERTRemarks.Text;

        if (policy_number != "" && remark.Trim() != "")
        {
            if (da_policy_cl24.InsertRemark(policy_number, remark))
            {
                foreach (GridViewRow row in gv_report.Rows)
                {
                    Label Policy_Number = (Label)row.FindControl("lblPolNo");

                    if (policy_number == Policy_Number.Text)
                    {
                        Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                        Button btnEDIT = (Button)row.FindControl("btnEDITRemark");
                        btnINSERT.Visible = false;
                        btnEDIT.Visible = true;
                    }
                }
            }
        }
    }

    protected void btnEDITRemark_Click(object sender, EventArgs e)
    {
        string policy_number = "", remark = "";
        policy_number = hdfEDITPolicyNumber.Value;
        remark = txtEDITRemarks.Text;

        if (policy_number != "" )
        {
            if (remark.Trim() != "")
                da_policy_cl24.EditRemark(policy_number, remark);
            else
                da_policy_cl24.RollBackRemark(policy_number);

            foreach (GridViewRow row in gv_report.Rows)
            {
                Label Policy_Number = (Label)row.FindControl("lblPolNo");

                if (policy_number == Policy_Number.Text)
                {
                    Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                    Button btnEDIT = (Button)row.FindControl("btnEDITRemark");

                    if (remark.Trim() != "") { btnINSERT.Visible = false; btnEDIT.Visible = true; } 
                    else { btnINSERT.Visible = true; btnEDIT.Visible = false; }
                }
            }
        }
        
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        string policy_number = "";
        policy_number = hdfEDITPolicyNumber.Value;

        if (policy_number != "")
        {
            da_policy_cl24.RollBackRemark(policy_number);

            foreach (GridViewRow row in gv_report.Rows)
            {
                Label Policy_Number = (Label)row.FindControl("lblPolNo");

                if (policy_number == Policy_Number.Text)
                {
                    Button btnINSERT = (Button)row.FindControl("btnADDRemark");
                    Button btnEDIT = (Button)row.FindControl("btnEDITRemark");

                    btnINSERT.Visible = true; btnEDIT.Visible = false;
                }
            }
        }

    }
}