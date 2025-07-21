using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Globalization;

using System.IO;
using System.Data;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;


public partial class Pages_Business_policy_issuance : System.Web.UI.Page
{

    //get logged-in username
    MembershipUser myUser = Membership.GetUser();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        
      
        string user_name = myUser.UserName;
        lblPrintedBy.Text = user_name.ToString();
        lblDateOfPrinting.Text = DateTime.Now.ToString("d MMM yyyy");
      
    }


    //Get payment mode from Payment Mode table
    protected string GetPayMode(string paymode)
    {
        string payment_mode = da_underwriting.GetPaymentMode(paymode);
        return payment_mode;
    }


    //Get extra amount
    protected string GetExtraPremium(string app_register_id)
    {
        string extra_premium = da_underwriting.GetEMAmount(app_register_id);
        return extra_premium;

    }


    //Get discount
    protected string GetDiscount(string app_register_id)
    {
        string discount = da_underwriting.GetDiscount(app_register_id);
        return discount.ToString();
    }


    //Get extra amount
    protected string GetTotalPremium(string app_register_id, string system_premium)
    {
        double total_premium = (Convert.ToDouble(system_premium) + Convert.ToDouble(GetExtraPremium(app_register_id)) - Convert.ToDouble(GetDiscount(app_register_id)));
        
        return total_premium.ToString();

    }
    
    protected void btnIssuePolicy_Click(object sender, EventArgs e)
    {
       
        try
        {            
            foreach (GridViewRow row in gvApplication.Rows)
            {

                //Get row that check is true
                CheckBox myChkBox = (CheckBox)row.FindControl("ckb1");

                //To be delete after inserting data done. Use datetime.now function
                //Get Effective_Date             
                //DateTime effective_date = da_policy.GetPolicyEffectiveDate(hdfAppRegisterID.Value);

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";

                DateTime effectiveDate = Convert.ToDateTime(txtIssueDate.Text, dtfi);

                DateTime effective_date = effectiveDate;
             
                if (myChkBox.Checked)
                {
                    //Get App_Register_ID from hidden then pass to function to get all inputs
                    string app_register_id = hdfAppRegisterID.Value;
                    if (app_register_id.Trim() != "")
                    {
                        ////Calculate new total premium
                        //double system_premium = Convert.ToDouble(hdfSystemPremium.Value);
                        //double extra_premium = Convert.ToDouble(GetExtraPremium(app_register_id));
                        //double discount = Convert.ToDouble(GetDiscount(app_register_id));
                        //double total_premium = (system_premium + extra_premium) - discount;

                        bl_underwriting uw = da_underwriting.GetUnderwritingObject(app_register_id);
                        string productid = "";
                        string customer_id = "";
                        string address_id = "";

                        productid = uw.Product_ID;
                        productid = productid.Substring(0, 3).ToUpper().Trim();
                        if (productid == "NFP" || productid == "FPP" || productid == "SDS")//new family protection
                        {
                            #region check rider underwriting status
                            if (da_application_fp6.GetUWRiderStatus(app_register_id) == "")
                            {
                                MessageAlert("You cannot issue this policy, because riders do not do underwriting yet.");
                                return;
                            }
                            else
                            {
                                //1.Add new customer then return customer ID
                                customer_id = da_customer.InsertCustomer(app_register_id, myUser.UserName, effective_date);
                                if (customer_id != "")
                                {
                                    //2. Add new customer rider
                                    string customer_rider_id = da_customer.InsertCustomerRider(app_register_id, myUser.UserName);

                                    //3.Add new policy address then return policy address ID
                                    address_id = da_policy.InsertPolicyAddress(app_register_id);

                                    //3.Add new policy then return policy ID
                                    string policy_id = da_policy.InsertPolicy(app_register_id, customer_id, address_id, myUser.UserName, effective_date);

                                    //4.Insert Policy_ID
                                    bool insert_policy_id = da_policy.InsertPolicyID(policy_id, 1);

                                    //5.Add policy data: policy premium, policy premium pay, policy contact, policy pay mode, policy number, app policy
                                    bool insert_data = da_policy.InsertPolicyDetails(app_register_id, policy_id, myUser.UserName, effective_date);
                                }
                            }

                            #endregion
                        }
                        else // Product CL24
                        {
                            if (uw.Product_ID == "CL24") //change
                            {
                                #region Split policy of credit life <By: Meassun Date:12-03-2020>

                                int totalPolicy = 0;
                                int numOfPolicy = 0;
                                int gender;
                                double lastPolicy = 0.0;
                                string premium = "0,0";
                                string[] arrPrem;
                                double premium_rate = 0.0;
                                double new_premium = 0.0;
                                double last_premium = 0.0;

                                #region Reserve Policy
                                bl_app_reserve_policy reserve_policy = new bl_app_reserve_policy();
                                reserve_policy = da_application.GetReservePolicyByAppNumber(uw.App_Number);

                                #endregion
                                if (reserve_policy.Policy_Number != null && reserve_policy.Customer_ID != null)
                                {
                                    //1.Add new customer then return customer ID
                                    customer_id = da_customer.InsertCustomerReserved(app_register_id, reserve_policy.Customer_ID, myUser.UserName, effective_date);

                                    if (customer_id != reserve_policy.Customer_ID)
                                    {
                                        MessageAlert("You cannot issue this policy, Please contact sys-administrator!");
                                        return;
                                    }
                                }
                                else
                                {
                                    //1.Add new customer then return customer ID
                                    customer_id = da_customer.InsertCustomer(app_register_id, myUser.UserName, effective_date);
                                }

                                //2.Add new policy address then return policy address ID
                                address_id = da_policy.InsertPolicyAddress(app_register_id);

                                if (ckb2.Checked) // Policy is Splited 5000 USD Blocked
                                {
                                    #region Split 5000 USD

                                    //Split Policy > 5000 USD
                                    if (uw.System_Sum_Insure >= 5000)
                                    {
                                        numOfPolicy = (int)(uw.System_Sum_Insure / 5000);
                                        totalPolicy += numOfPolicy;
                                        DataTable tbl = new DataTable();

                                        //Old 
                                        uw.Old_Sum_Insure = uw.System_Sum_Insure;
                                        uw.Old_Premium = uw.System_Premium;
                                        uw.Old_Original_Amount = uw.Original_Amount;

                                        //Find premium of policy after split
                                        if (uw.Gender == "ប្រុស")
                                            gender = 1;
                                        else
                                            gender = 0;

                                        //Get prem rate 
                                        premium = Calculation.CalculatePremiumCreditLife24(uw.Product_ID, uw.Age_Insure, Convert.ToInt32(uw.System_Sum_Insure), gender, uw.Pay_Mode, uw.Assure_Year);
                                        arrPrem = premium.Split(',');
                                        premium_rate = Convert.ToDouble(arrPrem[0]);

                                        new_premium = Math.Round((premium_rate * 5000) / uw.System_Sum_Insure, 2);

                                        //Find last policy & Premium of last policy 
                                        lastPolicy = (uw.System_Sum_Insure) - (5000 * numOfPolicy);
                                        if (lastPolicy > 0)
                                        {
                                            totalPolicy += 1;
                                            last_premium = Math.Round((premium_rate - (new_premium * numOfPolicy)), 2);
                                        }

                                        string ChannelItemID = da_policy.GetChannelItemID(uw.App_Register_ID);
                                        string GroupPolicy_ID = Helper.GetNewGuid("SP_Check_GroupPolicy_ID", "@Group_Policy_ID").ToString();
                                        string Group_Code = "GR-CR-005";
                                        string main_policy_number = "";
                                        string sub_policy_number = "";
                                        string Main_Policy_ID = "";

                                        //Insertion Policy
                                        for (int i = 1; i <= numOfPolicy; i++)
                                        {
                                            uw.System_Sum_Insure = 5000; uw.System_Premium = new_premium; uw.Original_Amount = new_premium;

                                            //1.INSERT NEW TO CT_POLICY then return policy ID
                                            string policy_id = da_policy.InsertPolicy(app_register_id, customer_id, address_id, myUser.UserName, effective_date);

                                            //2.INSERT NEW TO CT_POLICY_PREMIUM 
                                            da_policy.InsertPolicyPremium(uw, policy_id, myUser.UserName, effective_date, totalPolicy);

                                            //3.INSERT NEW TO CT_Policy_ID
                                            bool insert_policy_id = da_policy.InsertPolicyID(policy_id, 1);

                                            //4.INSERT NEW TO CT_POLICY_CONTACT
                                            da_policy.InsertPolicyContact(app_register_id, policy_id);

                                            #region CHECK FIRST FINANCE <By: SHUN Date:17-March-2020>

                                            if (i == 1) //Main Policy Number
                                            {
                                                Main_Policy_ID = policy_id;

                                                if (reserve_policy.Policy_Number != null && reserve_policy.Customer_ID != null)
                                                {
                                                    //5.INSERT NEW TO CT_Group_Master_Policy 
                                                    sub_policy_number = da_policy.InsertGroupMasterPolicyReserved(uw.Product_ID, policy_id, GroupPolicy_ID, reserve_policy.Policy_Number, main_policy_number, ChannelItemID);
                                                }
                                                else
                                                {
                                                    main_policy_number = da_policy.InsertGroupMasterPolicy(uw.Product_ID, policy_id, GroupPolicy_ID, main_policy_number, ChannelItemID);
                                                }

                                            }
                                            else //Sub Policy Number
                                            {
                                                if (reserve_policy.Policy_Number != null && reserve_policy.Customer_ID != null)
                                                {
                                                    //5.INSERT NEW TO CT_Group_Master_Policy 
                                                    sub_policy_number = da_policy.InsertGroupMasterPolicyReserved(uw.Product_ID, policy_id, GroupPolicy_ID, sub_policy_number, reserve_policy.Policy_Number, ChannelItemID);
                                                }
                                                else
                                                {
                                                    sub_policy_number = da_policy.InsertGroupMasterPolicyForSub(uw.Product_ID, policy_id, GroupPolicy_ID, main_policy_number, ChannelItemID);
                                                }
                                                
                                            }
                                            #endregion

                                            //5.INSERT NEW TO CT_APP_POLICY 
                                            da_policy.InsertAppPolicy(app_register_id, policy_id);

                                            //6.INSERT NEW TO CT_POLICY_PAY_MODE  - first due date & month is the extract of policy effective date
                                            da_policy.InsertPolicyPayMode(app_register_id, policy_id, uw.Pay_Mode, uw.Effective_Date, myUser.UserName, effective_date);

                                            //7.INSERT NEW TO CT_POLICY_BENEFIT
                                            da_policy.InsertPolicyBenefit(uw.Benefit_Note, policy_id, myUser.UserName, effective_date);

                                            //8.INSERT NEW TO CT_POLICY_BENEFIT_ITEM
                                            da_policy.InsertPolicyBenefitItem(app_register_id, policy_id);  //change relationship status from english to khmer in app_benefit_item

                                            //9.INSERT NEW TO CT_POLICY_STATUS
                                            da_policy.InsertPolicyStatus(policy_id, myUser.UserName, effective_date);

                                            uw.Old_Sum_Insure = 0.0;
                                            uw.Old_Premium = 0.0;
                                            uw.Old_Original_Amount = 0.0;
                                        }

                                        //INSERT LAST POLICY
                                        if (lastPolicy > 0)            
                                        {
                                            uw.System_Sum_Insure = lastPolicy; uw.System_Premium = last_premium; uw.Original_Amount = last_premium;

                                            //1.INSERT NEW TO CT_POLICY then return policy ID
                                            string policy_id = da_policy.InsertPolicy(app_register_id, customer_id, address_id, myUser.UserName, effective_date);

                                            //2.INSERT NEW TO CT_POLICY_PREMIUM 
                                            da_policy.InsertPolicyPremium(uw, policy_id, myUser.UserName, effective_date, totalPolicy);

                                            //3.INSERT NEW TO CT_Policy_ID
                                            bool insert_policy_id = da_policy.InsertPolicyID(policy_id, 1);

                                            //4.INSERT NEW TO CT_POLICY_CONTACT
                                            da_policy.InsertPolicyContact(app_register_id, policy_id);

                                            #region CHECK FIRST FINANCE <By: Sun Date:17032020>
                                            //if product is FF not insert insert policy number into CT_POLICY_PREMIUM table

                                            if (reserve_policy.Policy_Number != null && reserve_policy.Customer_ID != null)
                                            {
                                                //5.INSERT NEW TO CT_Group_Master_Policy 
                                                sub_policy_number = da_policy.InsertGroupMasterPolicyReserved(uw.Product_ID, policy_id, GroupPolicy_ID, sub_policy_number, reserve_policy.Policy_Number, ChannelItemID);
                                            }
                                            else
                                            {
                                                da_policy.InsertGroupMasterPolicy(uw.Product_ID, policy_id, GroupPolicy_ID, main_policy_number, ChannelItemID);
                                            }

                                            #endregion

                                            //5.INSERT NEW TO CT_APP_POLICY 
                                            da_policy.InsertAppPolicy(app_register_id, policy_id);

                                            //6.INSERT NEW TO CT_POLICY_PAY_MODE  - first due date & month is the extract of policy effective date
                                            da_policy.InsertPolicyPayMode(app_register_id, policy_id, uw.Pay_Mode, uw.Effective_Date, myUser.UserName, effective_date);

                                            //7.INSERT NEW TO CT_POLICY_BENEFIT
                                            da_policy.InsertPolicyBenefit(uw.Benefit_Note, policy_id, myUser.UserName, effective_date);

                                            //8.INSERT NEW TO CT_POLICY_BENEFIT_ITEM
                                            da_policy.InsertPolicyBenefitItem(app_register_id, policy_id);  //change relationship status from english to khmer in app_benefit_item

                                            //9.INSERT NEW TO CT_POLICY_STATUS
                                            da_policy.InsertPolicyStatus(policy_id, myUser.UserName, effective_date);

                                        }

                                        //////10.INSERT NEW TO CT_POLICY_PREM_PAY
                                        //da_policy.InsertPolicyPremiumPay(app_register_id, Main_Policy_ID, uw.Sale_Agent_ID, uw.Office_ID, myUser.UserName, effective_date);

                                        //11.GENERATE NEW ISSUE FF PREMIUM
                                        string my_policy_number = "", sub_policy_no = "";
                                        if (sub_policy_number != "")
                                        {
                                            if (sub_policy_number.Length >= 5) 
                                            {
                                                sub_policy_no = sub_policy_number.TrimStart('0');
                                            }
                                        }

                                        my_policy_number = Group_Code + "-" + main_policy_number + "-" + sub_policy_no;
                                        tbl = da_policy_cl24.GenerateNewIssueFirstFinancePremium(my_policy_number);

                                        if (tbl.Rows.Count > 0)
                                        {
                                            //12.INSERT NEW TO CT_CL24_RENEWAL_PREMIUM (1st ISSUE)
                                            da_policy_cl24.InsertNewIssueFirstFinancePremium(tbl, myUser.UserName);
                                        }

                                        //End None-Split Policy to each 5000 USD
                                    }

                                    #endregion
                                }
                                else
                                {
                                    //3.Add new policy then return policy ID
                                    string policy_id = da_policy.InsertPolicy(app_register_id, customer_id, address_id, myUser.UserName, effective_date);

                                    //4.Insert Policy_ID
                                    bool insert_policy_id = da_policy.InsertPolicyID(policy_id, 1);

                                    //5.Add policy data: policy premium, policy premium pay, policy contact, policy pay mode, policy number, app policy
                                    bool insert_data = da_policy.InsertPolicyDetails(app_register_id, policy_id, myUser.UserName, effective_date);
                                }

                                // Clear Reserved Policy Number after success issued policy
                                bool result = da_application.DeleteReservePolicy(reserve_policy.Reserve_Policy_ID);

                            #endregion
                            }
                            else
                            {
                                //1.Add new customer then return customer ID
                                customer_id = da_customer.InsertCustomer(app_register_id, myUser.UserName, effective_date);

                                //2.Add new policy address then return policy address ID
                                address_id = da_policy.InsertPolicyAddress(app_register_id);

                                //3.Add new policy then return policy ID
                                string policy_id = da_policy.InsertPolicy(app_register_id, customer_id, address_id, myUser.UserName, effective_date);

                                //4.Insert Policy_ID
                                bool insert_policy_id = da_policy.InsertPolicyID(policy_id, 1);

                                //5.Add policy data: policy premium, policy premium pay, policy contact, policy pay mode, policy number, app policy
                                bool insert_data = da_policy.InsertPolicyDetails(app_register_id, policy_id, myUser.UserName, effective_date);
                            }
                        }

                        #region block code
                        //6.Insert WING account
                        //Check if customer already have WING account
                        //bool check_customer_wing = da_customer.CheckCustomerWINGAccount(customer_id);

                        //if (check_customer_wing) //Have account
                        //{
                        //    //Get Wing_SK from existing custoer in table ct_policy_wing
                        //    string wing_sk = da_customer.GetCustomerWINGSK(customer_id);
                        //    bool insert_wing = da_policy.InsertPolicyWingAccount(policy_id, customer_id, wing_sk, myUser.UserName);

                        //}
                        //else //No account
                        //{
                        //    //Get last non-used WING_SK
                        //    string wing_sk = da_customer.GetLastUsableWINGSK();
                        //    bool insert_wing = da_policy.InsertPolicyWingAccount(policy_id, customer_id, wing_sk, myUser.UserName);

                        //    //Update status of WING account
                        //    da_policy.UpdateWINGAccountStatus(wing_sk);

                        //    //Alert to UWD which card should be selected
                        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('WING SK: " + wing_sk.ToString() + "')", true);

                        //}


                        //7.Calculate commission using window application

                        #endregion

                        //refresh gridview
                        gvApplication.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                        "<script> alert('Issued fail, this application was already issued.'); </script>", false);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnIssuePolicy_Click] in class [policy_issuance.aspx.cs]. Details: " + ex.Message);
        }
        
    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        gvApplication.DataBind();
    }
    protected void gvApplication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


        }
    }
    protected void btnUndoUW_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvApplication.Rows)
        {
            //Get row that check is true
            CheckBox myChkBox = (CheckBox)row.FindControl("ckb1");

            if (myChkBox.Checked)
            {
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;
                if (hdfAppRegisterID.Value.Trim() != "")
                {
                    //Delete rows from table underwriting, uw_life_product, uw_co
                    da_underwriting.UndoUnderwritingAll(hdfAppRegisterID.Value);
                    txtNote.Text = "Undo successfully";
                   
                    //refresh gridview
                    gvApplication.DataBind();
                }
                else
                {
                    txtNote.Text = "Undo fail, this application was already issued.";

                }
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
             "<script> $('#ResultModal').modal('show'); </script>", false);


            }
        }
    }
    private void MessageAlert(string message)
    {
        txtMessage.Text = message;
        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
          "<script> $('#ModalMessage').modal('show'); </script>", false);
    }

}