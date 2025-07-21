using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_edit_contact : System.Web.UI.Page
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
        List<bl_gtli_contact> contact_list = new List<bl_gtli_contact>();

        //Empty GridView
        gvContact.DataSource = contact_list;
        gvContact.DataBind();

        string company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompanyName.Text.Trim());

        if (company_id != "0")
        {
            bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(company_id);

           
            contact_list.Add(contact);

            gvContact.DataSource = contact_list;
            gvContact.DataBind();
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

    //Update Contact Click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            bl_gtli_contact contact = new bl_gtli_contact();
            contact.GTLI_Contact_ID = hdfContactID.Value;
            contact.Contact_Name = txtContactNameEdit.Text.Trim();
            contact.Contact_Phone = txtContactPhoneEdit.Text.Trim();
            contact.Contact_Email = txtContactEmailEdit.Text.Trim();

            if (da_gtli_contact.UpdateContact(contact))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update contact successfull.')", true);
                Load_Data();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update contact failed.Please try again.')", true);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnDelete_Click] in class [Pages_GTLI_contact]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update contact failed.Please try again.')", true);
        }
    }

    //Search Contact By Compnay
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Data();
    }
}