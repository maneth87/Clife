using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections;

public partial class Pages_GTLI_company : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ddlBusinessTypeEdit.DataSource = da_gtli_business.GetListOfBusinessName();
           
            ddlBusinessTypeEdit.DataBind();

            Load_Data();
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
    
    //Search Company
    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Load_Data();
    }

    //Load_Data by company name
    protected void Load_Data()
    {
        //refresh hdfCompanyID
        hdfCompanyID.Value = "";

        gvCompany.DataSource = da_gtli_company.GetListOfCompanyByCompanyName(txtCompanyName.Text.Trim());
        gvCompany.DataBind();
    }

    //Update Company
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            bl_gtli_company company = new bl_gtli_company();
            company.Type_Of_Business = ddlBusinessTypeEdit.SelectedItem.Text;
            company.Company_Name = txtCompanyNameEdit.Text;
            company.Company_Email = txtCompanyEmailEdit.Text;
            company.Company_Address = txtCompanyAddressEdit.Text;
            company.GTLI_Company_ID = hdfCompanyID.Value;

            if (da_gtli_company.UpdateCompany(company))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Updatge company successfull.')", true);
                Load_Data();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update company failed.Please try again.')", true);
            }
        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [btnUpdate_Click] in class [Pages_GTLI_company]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update company failed.Please try again.')", true);
        }
    }

    //Delete Company
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //Wait for sale to complete
            string company_id = hdfCompanyID.Value;

            //Check company id
            if (da_gtli_policy.CheckCompanyId(company_id))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This company could not be deleted. It has been used.')", true);
                return;
            }

            if (da_gtli_company.DeleteCompany(company_id))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete company successfull.')", true);
                Load_Data();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete company failed. Please try again.')", true);
            }
        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [btnDelete_Click] in class [Pages_GTLI_company]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete company failed.Please try again.')", true);
        }
    }
}