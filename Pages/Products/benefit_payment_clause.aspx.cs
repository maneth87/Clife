using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Products_benefit_payment_clause : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtClause.Text = "";
        }
    }
  
    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        bl_benefit_payment_clause benefit_payment_clause = new bl_benefit_payment_clause();

        if (hdfBenefitPaymentClauseID.Value == "" || hdfBenefitPaymentClauseID.Value == null ) // Case save new benefit clause
        {
            try
            {
                //Get new Benefit_Payment_Clause_ID
                string new_guid = Helper.GetNewGuid("SP_Check_Benefit_Payment_Clause_ID", "@Benefit_Payment_Clause_ID").ToString();

                benefit_payment_clause.Benefit_Payment_Clause_ID = new_guid;
                benefit_payment_clause.Product_ID = ddlProduct.SelectedValue;
                benefit_payment_clause.Benefit_Clasue = txtClause.Text.ToString();

                //Save new 
                string benefit_clause_id = da_benefit_payment_clause.InsertBenefitPaymentClause(benefit_payment_clause);

                if (benefit_clause_id != "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save successful!')", true);

                    //Get last row
                    hdfBenefitPaymentClauseID.Value = benefit_clause_id;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new benefit payment clause failed. Please check it again.')", true);
                    hdfBenefitPaymentClauseID.Value = "";
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [btnSave_Click] in class [Pages_Products_benefit_payment_clause]. Details: " + ex.Message);

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new benefit payment clause failed. Please check it again.')", true);

            }
        }
        else //Case edit benefit clause
        {
            try
            {
                benefit_payment_clause.Benefit_Payment_Clause_ID = hdfBenefitPaymentClauseID.Value;
                benefit_payment_clause.Product_ID = ddlProduct.SelectedValue;
                benefit_payment_clause.Benefit_Clasue = txtClause.Text;

                //Edit
                if (da_benefit_payment_clause.UpdateBenefitPaymentClause(benefit_payment_clause))
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update successful!')", true);
                }
                else
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update benefit payment clause failed. Please check it again.')", true);
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [btnSave_Click] in class [Pages_Products_benefit_payment_clause]. Details: " + ex.Message);

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update benefit payment clause failed. Please check it again.')", true);

            }

        }
    }

    //Dropdownlist product change
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        string product_id = ddlProduct.SelectedValue;

        bl_benefit_payment_clause benefit_clause = new bl_benefit_payment_clause();
        benefit_clause = da_benefit_payment_clause.GetBenefitPaymentClauseByProductID(product_id);

        txtClause.Text = benefit_clause.Benefit_Clasue;
        hdfBenefitPaymentClauseID.Value = benefit_clause.Benefit_Payment_Clause_ID;
    }
}