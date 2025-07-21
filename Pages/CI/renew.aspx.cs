using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

/*
 *Read Data
 * Write Data
 * 
 */

public partial class Pages_CI_renew : System.Web.UI.Page
{
    string user_name = "";
    DateTime transaction_date = new DateTime();
    List<da_report_approver.bl_report_approver> ApproverList;

    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        user_name = Membership.GetUser().UserName;
        ApproverList = da_report_approver.GetApproverList();
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (!Page.IsPostBack)
        {
            BindApprover();
        }
    }
    private void BindApprover()
    {
        ddlApprover.Items.Clear();
        ddlApprover.Items.Add(new ListItem(".", ""));
        foreach (da_report_approver.bl_report_approver approver in ApproverList)
        {
            ddlApprover.Items.Add(new ListItem(approver.NameEn, approver.ID + ""));
        }

    }
    protected void btnRenew_Click(object sender, EventArgs e)
    {
        transaction_date = DateTime.Now;
        DateTime effective_date = new DateTime();
        DateTime maturity_date = new DateTime();
        DateTime expiry_date = new DateTime();
        TimeSpan time = new TimeSpan(23, 59, 00);
        int age = 0;
        int last_pay_year = 0;
        int last_pay_lot = 0;
        int time_to_pay = 0;
        int approver_id = 0;
        
        //get policy information
        bl_ci.Policy policy = da_ci.Policy.GetPolicy(hfdPolicyID.Value);
        if (policy.PolicyID != null && policy.PolicyID != "")
        {
            if (ddlApprover.SelectedIndex>0)
            {
                approver_id = Int32.Parse(ddlApprover.SelectedValue);
                bl_product product = da_product.GetProductByProductID(policy.ProductID);

                age = Calculation.Culculate_Customer_Age(hfdDob.Value, Helper.FormatDateTime(txtNewEffectiveDate.Text.Trim()).Date);
                if (age >= product.Age_Min && age <= product.Age_Max)
                {
                    bl_ci.PolicyDetail last_policy_detail = da_ci.PolicyDetail.GetLastPolicyDetail(da_ci.Policy.GetPolicy(hfdPolicyID.Value).PolicyNumber);

                    last_pay_year = da_policy_prem_pay.GetLast_Policy_Prem_Year(policy.PolicyID);
                    last_pay_lot = da_policy_prem_pay.GetLast_Policy_Prem_Lot(policy.PolicyID, last_policy_detail.PayModeID, last_pay_year);
                    switch (last_policy_detail.PayModeID)
                    {
                        case 1:
                            time_to_pay = 1;
                            break;
                        case 2:
                            time_to_pay = 2;
                            break;
                        case 3:
                            time_to_pay = 4;
                            break;
                        case 4:
                            time_to_pay = 12;
                            break;
                    }
                    if (time_to_pay == last_pay_lot)
                    {
                        effective_date = Helper.FormatDateTime(txtNewEffectiveDate.Text.Trim()).Date + time;
                        maturity_date = effective_date.Date.AddYears(last_policy_detail.CoverYear);
                        expiry_date = maturity_date.Date.AddDays(-1) + time;
                        bl_ci.PolicyDetail policy_detail = new bl_ci.PolicyDetail();
                        policy_detail.PolicyDetailID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY_DETAIL" }, { "FIELD", "POLICY_DETAIL_ID" } });
                        policy_detail.PolicyID = policy.PolicyID;

                        policy_detail.EffectiveDate = effective_date;

                        policy_detail.MaturityDate = maturity_date;
                        policy_detail.ExpiryDate = expiry_date;
                        policy_detail.IssuedDate = DateTime.Now;
                        policy_detail.SumAssured = last_policy_detail.SumAssured;
                        policy_detail.PayModeID = last_policy_detail.PayModeID;
                        policy_detail.PaymentCode = last_policy_detail.PaymentCode;
                        policy_detail.PaymentBy = last_policy_detail.PaymentBy;
                        policy_detail.UserPremium = last_policy_detail.UserPremium;
                        policy_detail.Premium = last_policy_detail.Premium;
                        policy_detail.RETURN_PREMIUM = 0;// last_policy_detail.RETURN_PREMIUM;
                        policy_detail.OriginalPremium = last_policy_detail.OriginalPremium;
                        policy_detail.DiscountAmount = 0;// last_policy_detail.DiscountAmount;
                        policy_detail.Age = age;
                        policy_detail.CoverYear = 1;
                        policy_detail.PayYear = 1;
                        policy_detail.PayUpToAge = policy_detail.Age + policy_detail.CoverYear;
                        policy_detail.CoverUpToAge = policy_detail.Age + policy_detail.CoverYear;
                        policy_detail.PolicyStatusRemarks = "Renew";
                        policy_detail.CreatedBy = user_name;
                        policy_detail.CreatedDateTime = transaction_date;

                        if (da_ci.PolicyDetail.SavePolicyDetail(policy_detail))
                        {
                            #region //Save Approver Policy
                            da_report_approver.bl_report_approver_policy approver_policy;
                           
                            if (approver_id != 0) // new policy approved
                            {
                                //check existing approved policy
                               da_report_approver.bl_report_approver aprov_obj= da_report_approver.GetAproverInfo(policy.PolicyID);
                               if (aprov_obj.ID == 0)
                               {
                                   approver_policy = new da_report_approver.bl_report_approver_policy();
                                   approver_policy.Approver_ID = approver_id;
                                   approver_policy.Policy_ID = policy.PolicyID;
                                   approver_policy.Created_On = DateTime.Now;
                                   approver_policy.Created_By = user_name;
                                   if (da_report_approver.InsertApproverPolicy(approver_policy))
                                   {
                                       SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.RENEW, string.Concat("User process renew policy [", policy.PolicyNumber, "] successfully."));
                                       AlertMessage("Saved successfully.");
                                   }
                                   else
                                   {
                                       SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.RENEW, string.Concat("User process renew policy [", policy.PolicyNumber, "] fail."));
                                       //save fail
                                       AlertMessage("Saved fail.");
                                   }
                               }
                            }

                            #endregion //Save Approver Policy

                            //save success
                           
                        }
                        else
                        {
                            //save fail
                            AlertMessage("Saved fail.");
                        }
                    }
                    else
                    {
                        //cannot renew policy
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.RENEW, string.Concat("User process renew policy [", policy.PolicyNumber, "] fail. Policy cannot be renewed policy which is less than one year."));

                        AlertMessage("Policy cannot be renewed policy which is less than one year.");
                    }


                }
                else
                {
                    //age not in range
                    AlertMessage("Cusotmer's age is not in range.");
                }
            }
            else// not selected approver
            {
                AlertMessage("Please select approver.");
            }
        }
        else // policy not exist
        { 
        //policy information not found
            AlertMessage("System cannot found policy information.");
        }
        
    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        LoadData(txtCustomerName.Text.Trim() == "" ? "" : txtCustomerName.Text.Trim(), txtExpiryDate.Text.Trim(), txtPolicyNumber.Text.Trim() == "" ? "" : txtPolicyNumber.Text.Trim(), "CI,SO");
    }
    void LoadData(string customer_name, string expiry_date, string policy_number, string product_id)
    {


        string filter = "";
        //get all S.O policies
        DataTable tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), customer_name, policy_number, product_id);
        if (Helper.CheckDateFormat(expiry_date))
        {
            filter = " expiry_date >= '" + Helper.FormatDateTime(expiry_date).Date + "' and expiry_date <= '" + (Helper.FormatDateTime(expiry_date).Date + new TimeSpan(23,59,00)) + "'";
        }
       
        filter += filter.Trim() == "" ? " policy_status='IF'" : " and policy_status='IF'";//get only IF status
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

            string logDes = string.Concat(txtCustomerName.Text.Trim() == "" ? "" : " Customer Name:", txtCustomerName.Text.Trim());
            logDes += string.Concat(txtExpiryDate.Text.Trim() == "" ? "" : (logDes == "" ? string.Concat("Expiry Date:", txtExpiryDate.Text.Trim()) : string.Concat(", Expiry Date:", txtExpiryDate.Text.Trim())));
            logDes += string.Concat(txtPolicyNumber.Text.Trim() == "" ? "" : (logDes == "" ? string.Concat("Policy Number:", txtPolicyNumber.Text.Trim()) : string.Concat(", Policy Number:", txtPolicyNumber.Text.Trim())));

            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, String.Concat("User inquiries renew policies with criteria [", logDes, "], Record(s) found:", tbl_filter.Rows.Count));
        }
        else
        {
            gv_policy.DataSource = null;
            gv_policy.DataBind();
        }
    }

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
    
}