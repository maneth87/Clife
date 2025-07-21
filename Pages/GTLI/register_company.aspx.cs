using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_register_company : System.Web.UI.Page
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

            //Load business
            Load_Business();
        }
    }

    protected void Load_Business()
    {
        ddlBusinessType.DataSource = da_gtli_business.GetListOfBusinessName();     
        ddlBusinessType.DataBind();
    }

    //Register Company
    protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Check if already exist company
            if (da_gtli_company.CheckCompany(txtCompany.Text.Trim())){
                  ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This company already registered.')", true);
            }else{                

                //Not exist continue registration
                DateTime today = System.DateTime.Today;

                bl_gtli_company company = new bl_gtli_company();
                company.GTLI_Company_ID = Helper.GetNewGuid("SP_Check_GTLI_Company_ID", "@GTLI_Company_ID");
                company.Created_On = today;
                company.Created_By = hdfusername.Value;
                company.Company_Name  = txtCompany.Text.Trim();
                company.Company_Email =  txtCompanyEmail.Text.Trim();
                company.Company_Address = txtAddress.Text.Trim();
                company.Type_Of_Business = ddlBusinessType.SelectedItem.Text.ToString();

                //Insert new company
                if(da_gtli_company.InsertCompany(company)){
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Register company sucessfull.')", true);
                    bl_gtli_contact contact = new bl_gtli_contact();
                    contact.GTLI_Contact_ID = Helper.GetNewGuid("SP_Check_GTLI_Contact_ID", "@GTLI_Contact_ID");
                    contact.Contact_Email = txtContactEmail.Text.Trim();
                    contact.Contact_Name = txtContactPerson.Text.Trim();
                    contact.Contact_Phone = txtPhone.Text.Trim();
                    contact.Created_By = hdfusername.Value;
                    contact.Created_On = System.DateTime.Today;
                    contact.GTLI_Company_ID = company.GTLI_Company_ID;

                    da_gtli_contact.InsertContact(contact);

                    Clear();
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ImgBtnAdd_Click] in class [Pages_GTLI_register_company]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Register company failed.Please try again.')", true);
        }
    }

    //Insert new business
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (da_gtli_business.CheckExistingBusinessName(txtBusinessName.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This business already exist.')", true);
            }
            else
            {
                bl_gtli_business business = new bl_gtli_business();
                business.Business_Name = txtBusinessName.Text.Trim();
                business.GTLI_Business_ID = Helper.GetNewGuid("SP_Check_GTLI_Business_ID", "@GTLI_Business_ID");
                business.Created_By = hdfusername.Value;
                business.Created_On = System.DateTime.Today;

                //Process Inserting
                if (da_gtli_business.InsertBusiness(business))
                {
                    ddlBusinessType.Items.Clear();
                    ddlBusinessType.Items.Add(".");
                    ddlBusinessType.Items[0].Value = "0";
                    ddlBusinessType.AppendDataBoundItems = true;
                    Load_Business();
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add new business successfull.')", true);


                }
            }

        }catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnSave_Click] in class [Pages_GTLI_register_company]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add new business failed.Please try again.')", true);
        }
    }

    protected void Clear()
    {
        txtAddress.Text = "";
        txtBusinessName.Text = "";
        txtCompany.Text = "";
        txtCompanyEmail.Text = "";
        txtContactEmail.Text = "";
        txtContactPerson.Text = "";
        txtPhone.Text = "";
                
    }
}