using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrator_Policy_policy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

        return String.Format("{0:N0}", total_premium);

    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        gvPolicy.DataBind();
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

    //protected void btnUpdatePersonalDetails_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bl_customer my_customer = new bl_customer();
    //        my_customer.Customer_ID = hdfCustomerID.Value;
    //        my_customer.ID_Type = Convert.ToInt32(ddlIDTypeEdit.SelectedValue);
    //        my_customer.ID_Card = txtIDNoEdit.Text.Trim();
    //        my_customer.First_Name = txtFirstNameEdit.Text.Trim();
    //        my_customer.Last_Name = txtSurnameEdit.Text.Trim();
    //        my_customer.Khmer_First_Name = txtFirstNameKhEdit.Text.Trim();
    //        my_customer.Khmer_Last_Name = txtSurnameKhEdit.Text.Trim();
    //        my_customer.Father_First_Name = txtFatherFirstNameEdit.Text.Trim();
    //        my_customer.Father_Last_Name = txtFatherSurnameEdit.Text.Trim();
    //        my_customer.Mother_First_Name = txtMotherFirstNameEdit.Text.Trim();
    //        my_customer.Mother_Last_Name = txtMotherSurnameEdit.Text.Trim();
    //        my_customer.Prior_First_Name = txtPreviousFirstNameEdit.Text.Trim();
    //        my_customer.Prior_Last_Name = txtPreviousSurnameEdit.Text.Trim();
    //        my_customer.Country_ID = ddlNationalityEdit.SelectedValue;

    //        bool update_record = da_customer.UpdateCustomerByID(my_customer);

    //        if (update_record)
    //        {
    //            gvPolicy.DataBind();
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        //Add error to log 
    //        Log.AddExceptionToLog("Error in function [btnUpdatePersonalDetails_Click] in page [policy.aspx.cs]. Details: " + ex.Message);
    //    }

    //}

    //Search policy btnUpdatePersonalDetails
    protected void btnSearchPolicy_Click(object sender, EventArgs e)
    {
        try
        {
            string policy_number = txtPolicyNumberSearch.Text;
            string surname = txtSurnameSearch.Text;
            string firstname = txtFirstnameSearch.Text;

            if (policy_number != "") //Search by policy number CONTAINS(Column, 'test')
            {
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                PolicyDataSource.SelectCommand = "SELECT * FROM Cv_Basic_Policy WHERE Policy_Status_Type_ID = 'IF' AND Policy_Number LIKE '%" + policy_number + "%' ORDER BY Effective_Date DESC";
            }
            else //Search by customer name
            {
                txtPolicyNumberSearch.Text = "";
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                PolicyDataSource.SelectCommand = "SELECT * FROM Cv_Basic_Policy WHERE Policy_Status_Type_ID = 'IF' AND First_Name LIKE '%" + firstname + "%' AND Last_Name LIKE '%" + surname + "%' ORDER BY Effective_Date DESC";
            }

            gvPolicy.DataBind();

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnSearchPolicy_Click] in page [policy.aspx.cs]. Details: " + ex.Message);
        }
    }

}