using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Globalization;

public partial class Pages_Wing_wing_account : System.Web.UI.Page
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

            if (txtPolicyNumber.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please enter a policy number!')", true);

                return;
            }


            //Get user input
            //wing_sk = txtWINGSK.Text.Trim();
            wing_sk = ddlWINGSK.SelectedValue;
            policy_number = txtPolicyNumber.Text.Trim();
            

            //Get policy details for WING by passing policy number
            bl_policy_detail my_policy_detail = da_policy.GetPolicyDetailByPolicyNumber(policy_number);

            bool check_customer_wing = da_customer.CheckCustomerWINGAccount(my_policy_detail.Customer_ID);

            if (check_customer_wing) //Already have WING account
            {

                //Get Wing_SK from existing custoer in table ct_policy_wing
                wing_sk = da_customer.GetCustomerWINGSK(my_policy_detail.Customer_ID);

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Customer: " + my_policy_detail.Customer_Fullname + " already have a WING account (" + wing_sk +  ")')", true);

                return;
                //bool insert_wing = da_policy.InsertPolicyWingAccount(my_policy_detail.Policy_ID, my_policy_detail.Policy_Number, my_policy_detail.Customer_ID, my_policy_detail.Customer_Fullname, my_policy_detail.Gender, my_policy_detail.ID_Type.ToString(), my_policy_detail.ID_Card, my_policy_detail.Birth_Date, my_policy_detail.Mobile_Phone1, wing_sk, myUser.UserName);

            }
            else //No account
            {
                //Get last non-used WING_SK
                wing_sk = da_customer.GetLastUsableWINGSK();

                bool insert_wing = da_policy.InsertPolicyWingAccount(my_policy_detail.Policy_ID, my_policy_detail.Policy_Number, my_policy_detail.Customer_ID, my_policy_detail.Customer_Fullname, my_policy_detail.Gender, my_policy_detail.ID_Type.ToString(), my_policy_detail.ID_Card, my_policy_detail.Birth_Date, my_policy_detail.Mobile_Phone1, wing_sk, myUser.UserName);

                //Update status of WING account
                da_policy.UpdateWINGAccountStatus(wing_sk);

                ddlWINGSK.DataBind();

            }

            gvWingAccount.DataBind();

        }

        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnSave_Click] in class [wing_account.aspx]. Details: " + ex.Message);
        }
    }

    //Handle search for wing account
    protected void btnSearchWING_Click(object sender, EventArgs e)
    {
        try
        {
            string policy_number = txtPolicyNumberSearch.Text;
            string customer_name = txtSurnameSearch.Text.ToUpper() + " " + txtFirstnameSearch.Text.ToUpper();

            string wing_sk = txtSearchWingSK.Text.Trim();
            string wing_number = txtWingNo.Text.Trim();

            if (policy_number != "") //Search by policy number 
            {
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtSearchWingSK.Text = "";
                txtWingNo.Text = "";
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.Policy_Number = " + policy_number;
            }
            else if (wing_sk != "") //Search by wing serial key 
            {
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtSearchWingSK.Text = "";
                txtWingNo.Text = "";
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.WING_SK = " + wing_sk;

            }
            else if (wing_number != "") //Search by wing number
            {
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtSearchWingSK.Text = "";
                txtWingNo.Text = "";
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.WING_Number = " + wing_number;

            }

            else if (customer_name != "") //Search by customer name
            {
                txtPolicyNumberSearch.Text = "";
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtSearchWingSK.Text = "";
                txtWingNo.Text = "";
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.Customer_Name LIKE '%" + customer_name + "%' ORDER BY Created_On DESC, Ct_Wing_Account.SK DESC";
            }

            gvWingAccount.DataBind();

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnSearchWING_Click] in page [policy_printing.aspx.cs]. Details: " + ex.Message);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            //Set date format for SQL Server
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime dob = Convert.ToDateTime(txtEditWingDob.Text, dtfi);
            DateTime created_on = Convert.ToDateTime(txtEditWingCreatedOn.Text, dtfi);

            bl_policy_wing my_policy_wing = new bl_policy_wing();

            my_policy_wing.Policy_WING_ID = hdfPolicyWingID.Value;
            my_policy_wing.Policy_ID = hdfPolicyID.Value;
            my_policy_wing.Policy_Number = txtEditWingPolicyNumber.Text.Trim();
            my_policy_wing.Customer_Name = txtEditWingCustomerName.Text.Trim();
            my_policy_wing.Gender = ddlEditWingGender.SelectedItem.Value;
            my_policy_wing.ID_Type = ddlEditWingIDType.SelectedItem.Value;
            my_policy_wing.ID_Number = txtEditWingIDNumber.Text.Trim();
            my_policy_wing.Birth_Date = dob;
            my_policy_wing.Contact_Number = txtEditWingContactNumber.Text.Trim();
            my_policy_wing.WING_SK = txtEditWingSK.Text.Trim();
            my_policy_wing.WING_Number = txtEditWingNumber.Text.Trim();
            my_policy_wing.Created_On = DateTime.Now;
            my_policy_wing.Created_By = myUser.UserName;

            //Call function to update policy_wing single row
            bool update_pol_wing = da_policy.UpdatePolicyWingAccount(my_policy_wing);

            if (update_pol_wing)
            {
                gvWingAccount.DataBind();
            }
            else
                return;

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnUpdate_Click] in class [wing_account.aspx]. Details: " + ex.Message);
        }
    }

    
    protected void gvWingAccount_RowDataBound(object sender, GridViewRowEventArgs e)
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