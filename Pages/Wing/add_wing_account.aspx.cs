using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Wing_add_wing_account : System.Web.UI.Page
{

    //get logged-in username
    MembershipUser myUser = Membership.GetUser();
    

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        gvWingAccount.DataBind();
    }
   

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string policy_number, wing_sk;

            //Get user input
            wing_sk = txtWINGSK.Text.Trim();
            policy_number = txtPolicyNumber.Text.Trim();

          
            //Get policy details for WING by passing policy number
            bl_policy_detail my_policy_detail = da_policy.GetPolicyDetailByPolicyNumber(policy_number);


            bool check_customer_wing = da_customer.CheckCustomerWINGAccount(my_policy_detail.Customer_ID);

            if (check_customer_wing) //Already have WING account
            {
                //Get Wing_SK from existing custoer in table ct_policy_wing
                wing_sk = da_customer.GetCustomerWINGSK(my_policy_detail.Customer_ID);

                bool insert_wing = da_policy.InsertPolicyWingAccount(my_policy_detail.Policy_ID, my_policy_detail.Policy_Number, my_policy_detail.Customer_ID, my_policy_detail.Customer_Fullname, my_policy_detail.Gender, my_policy_detail.ID_Type.ToString(), my_policy_detail.ID_Card, my_policy_detail.Birth_Date, my_policy_detail.Mobile_Phone1, wing_sk, myUser.UserName);

            }
            else //No account
            {
                //Get last non-used WING_SK
                wing_sk = da_customer.GetLastUsableWINGSK();

                bool insert_wing = da_policy.InsertPolicyWingAccount(my_policy_detail.Policy_ID, my_policy_detail.Policy_Number, my_policy_detail.Customer_ID, my_policy_detail.Customer_Fullname, my_policy_detail.Gender, my_policy_detail.ID_Type.ToString(), my_policy_detail.ID_Card, my_policy_detail.Birth_Date, my_policy_detail.Mobile_Phone1, wing_sk, myUser.UserName);

                //Update status of WING account
                da_policy.UpdateWINGAccountStatus(wing_sk);
               
            }

            gvWingAccount.DataBind();

        
        }

        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnSave_Click] in class [add_wing_account.aspx]. Details: " + ex.Message);
        }
    }

}