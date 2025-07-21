using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

public partial class Pages_Flate_Rate_approve_policy : System.Web.UI.Page
{
    DataTable tbl;
    string user_name = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        user_name = Membership.GetUser().UserName;

        if (!Page.IsPostBack)
        {
            //string page_name = Helper.GetPageName();

            //string user_id = Membership.GetUser().ProviderUserKey.ToString();

            //da_user_access uaccess = new da_user_access();
            ////check user acccess page.
            //if (uaccess.GetActiveUserAccessPage(page_name, user_id).UserId == user_id.ToUpper())
            //{

            //    loadData();
            //}
            //else
            //{
            //    message.Attributes.CssStyle.Add("display", "block");
            //    message.InnerHtml = "Page​ Is Forbidden.";

            //    //processScript("show_control(0);");
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script>show_control(0);</script>", false);

            //}

            loadData();
            
        }
       
    }
 
    /// <summary>
    /// Get policy list filter by status New
    /// </summary>
    void loadData()
    {
        //List<bl_policy_flat_rate.bl_policy_customer_search> obj_arr = new List<bl_policy_flat_rate.bl_policy_customer_search>();
        try
        {
            //tbl = da_policy_flat_rate.GetPolicyCustomerSearch("","", "", "", -1, "", (int) bl_policy_flat_rate.Status.New + "");
            List<bl_policy_flat_rate> arr_obj = da_policy_flat_rate.GetPolicyByParameters(" policy_status_id=" + (int) bl_policy_flat_rate.Status.New);
            if (arr_obj.Count > 0)
            {
                message.Attributes.CssStyle.Add("display", "none");
                processScript("show_control(1);");
               
            }
            else
            {
                //show div message and hide panel
                message.Attributes.CssStyle.Add("display", "block");
                message.InnerHtml = "No pending policy to approve.";

                //processScript("show_control(0);");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script>show_control(0);</script>", false);
            }
            gvPolicy.DataSource = arr_obj;
            gvPolicy.DataBind();
           
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script>alert('" + ex.Message + "');</script>", false);
        }
       
    }
   
    protected void btnDecline_Click(object sender, EventArgs e)
    {
        List<string> policy_id = new List<string>();
        foreach (GridViewRow row in gvPolicy.Rows)
        {
            CheckBox ckb = (CheckBox)row.FindControl("ckbPolicy");
            if (ckb.Checked)
            {
                Label lbl = (Label)row.FindControl("lblPolicyID");
                policy_id.Add(lbl.Text.Trim());
            }
        }
        string str_policy = "";
        foreach (string policy in policy_id)
        {
            str_policy += policy + ",";
        }
        if (str_policy.Trim() != "")
        {
            str_policy = str_policy.Substring(0, str_policy.Length - 1);
            bool status = da_policy_flat_rate.ApprovedPolicy(bl_policy_flat_rate.Status.Decline.ToString() ,str_policy, (int)bl_policy_flat_rate.Status.Decline + "", System.Web.Security.Membership.GetUser().UserName, DateTime.Now, Helper.FormatDateTime(txtIssuedDate.Text), txtRemarks.Text);
            if (status)
            {

                processScript("alert('Decline successfully.');");
                loadData();
                
            }
            else
            {
                processScript("alert('Decline fail.');");
            }

        }
        else
        {
            processScript("alert('Please check record(s) to decline.');");
        }
    }


    protected void btnApproved_Click(object sender, EventArgs e)
    {
        List<string> policy_id = new List<string>();
        foreach (GridViewRow row in gvPolicy.Rows)
        {
            CheckBox ckb = (CheckBox)row.FindControl("ckbPolicy");
            if (ckb.Checked)
            {
                Label lbl = (Label)row.FindControl("lblPolicyID");
                policy_id.Add(lbl.Text.Trim());
            }
        }

        string str_policy = "";
        foreach (string policy in policy_id)
        {
            str_policy += policy + ",";
        }

        if (str_policy.Trim() != "")
        {
            str_policy = str_policy.Substring(0, str_policy.Length - 1);
            bl_policy_flat_rate policyObj = da_policy_flat_rate.GetPolicy(str_policy);
            //Save premium pay

             da_policy.InsertPolicyPremiumPay(new bl_policy_prem_pay()
            {
                Policy_Prem_Pay_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "Ct_Policy_Prem_Pay" }, { "FIELD", "POLICY_PREM_PAY_ID" } }),
                Policy_ID = policyObj.PolicyID,
                Office_ID = "HQ",
                Sale_Agent_ID = policyObj.SaleAgentID,
                Pay_Mode_ID = policyObj.PayModeID,
                Prem_Year = 1,
                Prem_Lot = 1,
                Due_Date = policyObj.EffectiveDate,
                Pay_Date = policyObj.EffectiveDate,
                Amount = (policyObj.PremiumByMode + policyObj.ExtraPremiumByMode) - policyObj.Discount,
                Created_By = Membership.GetUser().UserName,
                Created_On = DateTime.Now,
                Created_Note=""
            });

            //Save policy number
           // da_policy.InsertPolicyNumber(policyObj.PolicyID);
            //Save policy status
            da_policy.InsertPolicyStatus(policyObj.PolicyID, Membership.GetUser().UserName, DateTime.Now);

            bool status = da_policy_flat_rate.ApprovedPolicy(bl_policy_flat_rate.Status.Approved.ToString(), str_policy, (int)bl_policy_flat_rate.Status.Approved + "", System.Web.Security.Membership.GetUser().UserName, DateTime.Now, Helper.FormatDateTime(txtIssuedDate.Text), txtRemarks.Text);
            if (status)
            {

               processScript("alert('Approved successfully.');");
               loadData();
            }
            else
            {
                processScript("alert('Approved fail.');");
            }

        }
        else
        {
            processScript("alert('Please check record(s) to approve.');");
        }

    }

    void processScript(string str_script)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "", "<script>" + str_script +"</script>",true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "",  str_script , true);
    }

    protected void gvPolicy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
        }
    }
}