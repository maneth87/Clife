using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;

public partial class Pages_Business_policy_printing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Old Query
            //COMBIND WITH POLICY FLATE RATE
            //PolicyDataSource.SelectCommand
            string query = " SELECT TOP 50 App_Register_ID, Policy_ID, CASE WHEN Policy_Number IS NULL  THEN (SELECT (mProduct.group_code + '-' + mPolicy.seq_number) AS 'policy_number' " + 
		                     "FROM ct_group_master_policy mPolicy " + 
		                     "INNER JOIN  ct_group_master_product mProduct ON mProduct.group_master_id = mPolicy.group_master_id " +
                             "WHERE mPolicy.policy_id = Cv_Basic_Policy.Policy_ID)  ELSE Policy_Number END AS 'policy_number', app_number, customer_id, first_name, last_name, " + 
		                     "gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date " +  
		                     "FROM Cv_Basic_Policy ORDER BY Effective_Date DESC " ;
            #endregion
            DataTable tbl = DataSetGenerator.Get_Data_Soure(query);
            gvPolicy.DataSource = tbl;
            gvPolicy.DataBind();

        }
    }

    //Get extra amount
    protected string GetExtraPremium(string app_register_id, string policy_id = "", string product_id = "")
    {
        string extra_premium = "";
        if (product_id == "CL24")
        {
            extra_premium = da_policy.GetEMAmountByPolicyID(policy_id);
        }
        else
        {
            extra_premium = da_underwriting.GetEMAmount(app_register_id);
        }

        return extra_premium;

    }
    //Get discount
    protected string GetDiscount(string policy_id)
    {
        string discount = da_policy.GetDiscountFromPolicyPremium(policy_id);
        return discount.ToString();
    }
    //Get extra amount
    protected string GetTotalPremium(string app_register_id, string system_premium, string policy_id, string product_id = "")
    {
        double sys_premium = 0.0;
        double total_premium = 0.0;
        if (system_premium.Trim() == "")
        {
            sys_premium = 0.0;
        }
        else
        {
            sys_premium = Convert.ToDouble(system_premium);
        }
        //double total_premium = (Convert.ToDouble(system_premium) + Convert.ToDouble(GetExtraPremium(app_register_id)) - Convert.ToDouble(da_policy.GetDiscountFromPolicyPremium(policy_id)));
        if (product_id == "CL24")
        {
            total_premium = (sys_premium + Convert.ToDouble(GetExtraPremium(app_register_id, policy_id, product_id)) - Convert.ToDouble(da_policy.GetDiscountFromPolicyPremium(policy_id)));
        }
        else
        {
            total_premium = (sys_premium + Convert.ToDouble(GetExtraPremium(app_register_id)) - Convert.ToDouble(da_policy.GetDiscountFromPolicyPremium(policy_id)));
        }

        return String.Format("{0:N0}", total_premium);

    }

    protected void ImgBtnPrintPolicy_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [ImgBtnPrintPolicy_Click] in page [policy_printing.aspx.cs]. Details: " + ex.Message);
        }

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
    
    //Search policy
    protected void btnSearchPolicy_Click(object sender, EventArgs e)
    {
        try
        {
            string policy_number = txtPolicyNumberSearch.Text;
            string surname = txtSurnameSearch.Text;
            string firstname = txtFirstnameSearch.Text;
            //string query = "";
            
            if (policy_number != "") 
            {
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                #region old query
                // PolicyDataSource.SelectCommand = "SELECT * FROM Cv_Basic_Policy WHERE Policy_Number LIKE '%" + policy_number + "%' ORDER BY Effective_Date DESC";  
                //PolicyDataSource.SelectCommand 
                    //query= " SELECT App_Register_ID, Policy_ID, Policy_Number, app_number, customer_id, first_name, last_name, gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date " +
                    //                       " FROM " +
                    //                         "(SELECT '' as 'APP_REGISTER_ID', Policy_ID, Policy_Number, app_number, customer_id, first_name, last_name, gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date FROM V_PRINT_POLICY_FLAT_RATE_SCHEDULE where policy_status_id=2 " +
                    //                     " UNION ALL " +
                    //                     " SELECT App_Register_ID, Policy_ID, case when Policy_Number is null  then (select (mProduct.group_code + '-' + mPolicy.seq_number) as 'policy_number' from ct_group_master_policy mPolicy inner join " +
                    //                      " ct_group_master_product mProduct on mProduct.group_master_id = mPolicy.group_master_id where mPolicy.policy_id = Cv_Basic_Policy.Policy_ID) " +
                    //                      " else Policy_Number end as 'policy_number', app_number, customer_id, first_name, last_name, gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date FROM Cv_Basic_Policy " +
                    //                      "UNION ALL " +
                    //                      "SELECT '' As 'APP_REGISTER_ID', Ct_Policy.Policy_ID, Policy_Number, '' as 'app_number', Ct_Customer.Customer_ID, Ct_Customer.First_Name, Ct_Customer.Last_Name, " +
                    //                         "Gender, Age_Insure, Product_ID, Sum_Insure, Premium, '' As 'Rounded_Amount', Effective_Date, Issue_Date, Maturity_Date " +
                    //                         "FROM Ct_Policy " +
                    //                         "INNER JOIN Ct_Policy_Number ON Ct_Policy_Number.Policy_ID = Ct_Policy.Policy_ID " +
                    //                         "INNER JOIN Ct_Customer ON Ct_Customer.Customer_ID = Ct_Policy.Customer_ID " +
                    //                         "INNER JOIN ct_policy_premium ON Ct_Policy_Premium.Policy_ID = Ct_Policy.Policy_ID " +
                    //                         "WHERE Ct_Policy_Number.MAIN_POLICY IS NULL " +
                //                     " )#TEMP WHERE Policy_Number LIKE '%" + policy_number + "%' ORDER BY Effective_Date DESC;";
                #endregion
                gvPolicy.DataSource = DataSetGenerator.Get_Data_Soure("SP_Get_Policy_Search_By_Policy_Number", new string[,] { { "@POLICY_NUMBER", policy_number } });
            }
            else //Search by customer name 
            {
                txtPolicyNumberSearch.Text = "";
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                #region old query
                //PolicyDataSource.SelectCommand = "SELECT * FROM Cv_Basic_Policy WHERE First_Name LIKE '%" + firstname + "%' AND Last_Name LIKE '%" + surname + "%' ORDER BY Effective_Date DESC";

                //PolicyDataSource.SelectCommand 
                //query= " SELECT App_Register_ID, Policy_ID, Policy_Number, app_number, customer_id, first_name, last_name, gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date " +
                //                        " FROM (SELECT '' as 'APP_REGISTER_ID', Policy_ID, Policy_Number, app_number, customer_id, first_name, last_name, gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date " +
                //                        " FROM V_PRINT_POLICY_FLAT_RATE_SCHEDULE where policy_status_id=2 " +
                //                        " UNION ALL " +
                //                        " SELECT App_Register_ID, Policy_ID, case when Policy_Number is null  then (select (mProduct.group_code + '-' + mPolicy.seq_number) as 'policy_number' from ct_group_master_policy mPolicy inner join " +
                //                          " ct_group_master_product mProduct on mProduct.group_master_id = mPolicy.group_master_id where mPolicy.policy_id = Cv_Basic_Policy.Policy_ID) " +
                //                          " else Policy_Number end as 'policy_number', app_number, customer_id, first_name, last_name, gender,Age_Insure, Product_ID, Sum_Insure, Premium, Rounded_Amount, effective_date, issue_date, maturity_date FROM Cv_Basic_Policy " +
                //                        " )#TEMP WHERE First_Name LIKE '%" + firstname + "%' AND Last_Name LIKE '%" + surname + "%' ORDER BY Effective_Date DESC;";
                #endregion
                gvPolicy.DataSource = DataSetGenerator.Get_Data_Soure("SP_Get_Policy_Search_By_Surname", new string[,] { { "@FIRST_NAME", firstname }, { "@SURNAME", surname } });
            
            }

            //gvPolicy.DataSource = DataSetGenerator.Get_Data_Soure(query);
            gvPolicy.DataBind();

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [ImageBtnSearch_Click] in page [policy_printing.aspx.cs]. Details: " + ex.Message);
        }
    }
}