using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections;

public partial class Pages_GTLI_register_plan : System.Web.UI.Page
{
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

            txtCompanyName.Attributes.Add("onchange", "return GetPlan()");
        }
    }

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

    //Register Plan  
    protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (da_gtli_company.CheckCompany(txtCompanyName.Text.Trim()))
        {
            //found continue registration of new  plan for this company
            //available plan then add new plan
            string company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompanyName.Text.Trim());
            string plan_id = Helper.GetNewGuid("SP_Check_GTLI_Plan_ID", "@GTLI_Plan_ID");
            string plan = txtPlan.Text.ToUpper();

            double sum_insured = 0;

            if (ckbDynamicSumInsure.Checked)
            {
                sum_insured = Convert.ToDouble(txtAmount.Text);
            }
            else
            {
                sum_insured = 0;
            }
            int tpd = Convert.ToInt16(radTPD.SelectedValue);
            int dhc = Convert.ToInt32(ddlDHC.SelectedValue);
            int accidental_100plus = Convert.ToInt16(rad100Plus.SelectedValue);

            //check existing plan for this company
            if (da_gtli_plan.CheckExistingPlan(plan, sum_insured, tpd, dhc, accidental_100plus, company_id))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan already exist.')", true);
            }
            else
            {
                //Insert new plan
                if (da_gtli_plan.InsertPlan(plan_id, plan, sum_insured, tpd, dhc, accidental_100plus, company_id, System.DateTime.Now, hdfusername.Value))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new plan successfull.')", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new plan failed. Please try again.')", true);
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Company not found.')", true);
        }
    }
  
}