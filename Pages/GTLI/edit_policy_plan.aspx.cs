using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_edit_policy_plan : System.Web.UI.Page
{
    //get autocomplete for company name
    [WebMethod()]
    public static string[] GetCompanyName()
    {
        ArrayList list_of_company = new ArrayList();
        list_of_company = da_gtli_company.GetListOfCompanyName();

        string[] companyArray = new string[list_of_company.Count];

        for (int i = 0; i <= list_of_company.Count - 1; i++)
        {
            companyArray[i] = list_of_company[i].ToString();
        }

        return companyArray;
    }


    private void Load_Data()
    {
        List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();

        //Empty GridView
        gvPlan.DataSource = plan_list;
        gvPlan.DataBind();

        //Populate plan by company
        plan_list = da_gtli_plan.GetObjectPlanListByCompanyName(txtCompanyName.Text.Trim());

        hdfCompanyID.Value = da_gtli_company.GetCompanyIDByCompanyName(txtCompanyName.Text.Trim());

        gvPlan.DataSource = plan_list;
        gvPlan.DataBind();
    }

    //Grid view row data bound
    protected void gvPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (gvPlan.EditIndex == -1)
        {
            // Check for that the row is a data row & not header or footer

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbltpd = (Label)e.Row.FindControl("lblTPD");
                Label lbldhc = (Label)e.Row.FindControl("lbldhc");
                Label lblsum_insured = (Label)e.Row.FindControl("lblsum_insured");

                if (lbltpd.Text == "1")
                {
                    lbltpd.Text = "Yes";

                }
                else
                {
                    lbltpd.Text = "No";

                }

                if (lbldhc.Text == "0")
                {
                    lbldhc.Text = "No";

                }
                else
                {
                    lbldhc.Text = "Option: " + lbldhc.Text + "$/year";
                }

                int sum_insured = Convert.ToInt32(lblsum_insured.Text);

                lblsum_insured.Text = "$" + string.Format("{0:#,###0}", sum_insured);
            }
        }

    }

    //Button Update click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //check existing plan id in premium
        if(!da_gtli_premium.CheckExistingPlanInPremium(hdfPlanID.Value)){

            bl_gtli_plan plan = new bl_gtli_plan();
            plan.GTLI_Plan = txtPlanNameEdit.Text.Trim().ToUpper();
            plan.GTLI_Plan_ID = hdfPlanID.Value;

            if (ckbDynamicSumInsure.Checked)
            {
                plan.Sum_Insured = Convert.ToDouble(txtSumInsuredEdit.Text);
            }
            else
            {
                plan.Sum_Insured = 0;
            }

            
            plan.TPD_Option_Value = Convert.ToInt32(rbtnlTPD.SelectedValue);
            plan.DHC_Option_Value = Convert.ToInt32(ddlDHC.SelectedValue);
            plan.Accidental_100Plus_Option_Value = Convert.ToInt32(rbtnlAccidental100Plus.SelectedValue);

            string plan_name = "";

            if (hdfPlanName.Value == plan.GTLI_Plan)
            {
                plan_name = "";
            }
            else
            {
                plan_name = plan.GTLI_Plan;
            }
            
            //Check existing plan (can not update plan to be the same as other existing plans)
            if (da_gtli_plan.CheckExistingPlan(plan_name, plan.Sum_Insured, plan.TPD_Option_Value, plan.DHC_Option_Value, plan.Accidental_100Plus_Option_Value, hdfCompanyID.Value))
            {
                  ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan already exist.')", true);
            }else{
                //edit plan
                if (da_gtli_plan.UpdatePlan(plan)){
                     ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Edit plan successfull.')", true);
                     Load_Data();
                }else{
                      ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Edit plan failed. Please try again.')", true);
                }
            }
        
        }else{
             ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan has already been used.')", true);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (!da_gtli_premium.CheckExistingPlanInPremium(hdfPlanID.Value))
        {
            if (da_gtli_plan.DeletePlan(hdfPlanID.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete plan successfull.')", true);
                Load_Data();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete plan failed. Please try again.')", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan has already been used.')", true);
        }
    }

    //Search Plan
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Data();
    }
}