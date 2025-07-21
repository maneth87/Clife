using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Pages_Business_customer_list : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            //authentication auth = new authentication();
            //string page_name = Path.GetFileName(Request.Url.AbsolutePath);
            //auth.RequestPage = page_name;
            //if (auth.Authorize)
            //{
            //    div_message.Attributes.Clear();
            //    VisibleSaveButton(false);
            //    VisibleEditButton(false);

            //    EnableCustomer(false);
            //    EnableContact(false);
            //    EnableAddress(false);

            //    Options.Bind(ddlIDType, Options.IDType.GetIDTypeList(), "Text", "Values", 0);
            //    Options.Bind(ddlCountry, Options.Country.GetCountryList(), "Text", "Values", 0);
            //    Options.Bind(ddlNationality, Options.Country.GetNationalityList(), "Text", "Values", 0);
            //    Options.Bind(ddlSearchGender, Options.Gender.GetGender(), "Text", "Values", 0);
            //}
            //else
            //{
            //    div_body.Attributes.CssStyle.Add("display", "none");
            //    div_message.InnerText = "No permission.";
            //}

            div_message.Attributes.Clear();
            VisibleSaveButton(false);
            VisibleEditButton(false);

            EnableCustomer(false);
            EnableContact(false);
            EnableAddress(false);

            Options.Bind(ddlIDType, Options.IDType.GetIDTypeList(), "Text", "Values", 0);
            Options.Bind(ddlCountry, Options.Country.GetCountryList(), "Text", "Values", 0);
            Options.Bind(ddlNationality, Options.Country.GetNationalityList(), "Text", "Values", 0);
            Options.Bind(ddlSearchGender, Options.Gender.GetGender(), "Text", "Values", 0);
        }
        
    }
    void VisibleSaveButton(bool t)
    {
        btnSaveAddress.Visible = t;
        btnSaveContact.Visible = t;
        btnSaveCustPersonal.Visible = t;
    }
    void VisibleEditButton(bool t)
    {
        btnEditAddress.Visible = t;
        btnEditContact.Visible = t;
        btnEditCustomer.Visible = t;
    }
    void EnableCustomer(bool t)
    {
        txtFirstName.Enabled = t;
        txtLastName.Enabled = t;
        ddlIDType.Enabled = t;
        txtIDNo.Enabled = t;
        ddlNationality.Enabled = t;
        txtKhmerFirstName.Enabled = t;
        txtKhmerLastName.Enabled = t;
        txtpriorFirstName.Enabled = t;
        txtPriorLastName.Enabled = t;
        txtMotherFirstName.Enabled = t;
        txtMotherLastName.Enabled = t;
        txtFatherFirstName.Enabled = t;
        txtFatherLastName.Enabled = t;


    }
    void EnableContact(bool t)
    {
        txtMobile1.Enabled = t;
        txtMobile2.Enabled = t;
        txtHomePhone1.Enabled = t;
        txtHomePhone2.Enabled = t;
        txtOfficePhone1.Enabled = t;
        txtOfficePhone2.Enabled = t;
        txtFax1.Enabled = t;
        txtFax2.Enabled = t;
        txtEmail.Enabled = t;
    }
    void EnableAddress(bool t)
    {
        txtAddress1.Enabled = t;
        txtAddress2.Enabled = t;
        txtAddress3.Enabled = t;
        txtProvice.Enabled = t;
        ddlCountry.Enabled = t;
        
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // ScriptManager.RegisterStartupScript(up, up.GetType(), "alert", "alert('Hello world.');$('#modal_search_customer').modal('hide');", true);
        //lblMessage.Text = "Hello world.";
        //loadData(txtSearchCustName.Text.Trim(), ddlSearchCustType.SelectedValue, txtSearchIDCard.Text.Trim(), "", txtSearchCustNo.Text.Trim());
     List<bl_customer> custList=   da_customer.GetCustomerList(txtSearchCustNo.Text.Trim(),txtSearchCustName.Text.Trim(),txtSearchBirthDate.Text.Trim()!="" ? Helper.FormatDateTime(txtSearchBirthDate.Text.Trim()): new DateTime(1900,1,1), ddlSearchGender.SelectedIndex>0 ? Int32.Parse(ddlSearchGender.SelectedValue): -1,txtSearchIDCard.Text.Trim() );
     gvCustomer.DataSource = custList;
     gvCustomer.DataBind();
     if (custList.Count > 0)
     {
         btnOK.Visible = true;
     }
     else
     {
         btnOK.Visible = false;
     }

    // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "$('#modal_search_customer').modal();", true);
    }
    
    #region load data
    #region load personal
    /// <summary>
    /// Get customer information by customer ID
    /// </summary>
    /// <param name="customer_id"></param>
    void loadPersonal(string customer_id)
    {
        EnableCustomer(false);
        EnableContact(false);
        EnableAddress(false);
        try
        {
            bl_customer cust = new bl_customer();
            cust = da_customer.GetCustomerByCustomerID(customer_id);

            txtCustNo.Text = cust.Customer_ID;
            txtFirstName.Text = cust.First_Name;
            txtLastName.Text = cust.Last_Name;
            txtIDNo.Text = cust.ID_Card;
            txtGender.Text = Helper.GetGenderText(cust.Gender, true);
            txtBirthDate.Text = cust.Birth_Date.ToString("dd/MM/yyyy");
            Helper.SelectedDropDownListIndex("Value", ddlIDType, cust.ID_Type + "");
            Helper.SelectedDropDownListIndex("Value", ddlNationality, cust.Country_ID);
            txtKhmerFirstName.Text = cust.Khmer_First_Name;
            txtKhmerLastName.Text = cust.Khmer_Last_Name;
            txtpriorFirstName.Text = cust.Prior_First_Name;
            txtPriorLastName.Text = cust.Prior_Last_Name;
            txtMotherFirstName.Text = cust.Mother_First_Name;
            txtMotherLastName.Text = cust.Mother_Last_Name;
            txtFatherFirstName.Text = cust.Father_First_Name;
            txtFatherLastName.Text = cust.Father_Last_Name;

            List<bl_policy> poList = da_policy.GetPolicyByCustomerID(cust.Customer_ID);


            var pol = poList[0];
           
           
            if (cust.Customer_ID != null && cust.Customer_ID != "")
            {
                //VisibleSaveButton(true);
                VisibleEditButton(true);
            }
            else
            {
                //VisibleSaveButton(false);
                VisibleEditButton(false);
            }
           
           
            hdfpolicyID.Value = pol.Policy_ID;
            hdfAddressID.Value = pol.Address_ID;
           
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error " + Log.GenerateLog(ex));
        }
    }
    #endregion load personal
    #region load contact
    void loadContact(string policy_id)
    {
        bl_app_info_contact contact = da_policy.GetPolicyContact(policy_id);
        txtMobile1.Text = contact.Mobile_Phone1;
        txtMobile2.Text = contact.Mobile_Phone2;
        txtHomePhone1.Text = contact.Home_Phone1;
        txtHomePhone2.Text = contact.Home_Phone2;
        txtOfficePhone1.Text = contact.Office_Phone1;
        txtOfficePhone2.Text = contact.Office_Phone2;
        txtFax1.Text = contact.Fax1;
        txtFax2.Text = contact.Fax2;
        txtEmail.Text = contact.EMail;
    }
    #endregion load contact
                #region load address
    void loadAddress(string address_id)
    {
        bl_policy_address add = da_policy.GetAddress(address_id);
        txtAddress1.Text = add.Address1;
        txtAddress2.Text = add.Address2;
        txtAddress3.Text = add.Address3;
        txtProvice.Text = add.Province;
        Helper.SelectedDropDownListIndex("Value", ddlCountry, add.Country_ID);
        txtZipCode.Text = add.Zip_Code;
    }
            #endregion load address

    #endregion load data
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        string zipCode = da_application.GetZipCode(ddlCountry.SelectedValue);
        txtZipCode.Text = zipCode;

    }
    protected void btnSaveContact_Click(object sender, EventArgs e)
    {
        bl_app_info_contact contact = new bl_app_info_contact();
        contact.PolicyID = hdfpolicyID.Value;
        contact.Mobile_Phone1 = txtMobile1.Text.Trim();
        contact.Mobile_Phone2 = txtMobile2.Text.Trim();
        contact.Home_Phone1 = txtHomePhone1.Text.Trim();
        contact.Home_Phone2 = txtHomePhone2.Text.Trim();
        contact.Office_Phone1 = txtOfficePhone1.Text.Trim();
        contact.Office_Phone2 = txtOfficePhone2.Text.Trim();
        contact.Fax1 = txtFax1.Text.Trim();
        contact.Fax2 = txtFax2.Text.Trim();
        contact.EMail = txtEmail.Text.Trim();
        

        if (da_policy.UpdatePolicyContact(contact))
        {
            alert("Contact was updated successfully.");
            btnEditContact.Text = "Edit";
            btnSaveContact.Visible = false;
            //reload updated contact
            loadContact(hdfpolicyID.Value);
            EnableContact(false);
        }
        else
        {
            alert("Contact was updated fail.");
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "$('#"+pnSearch.ClientID+"').modal('hide');", true);
        loadPersonal(hdfCustomerID.Value);
        loadContact(hdfpolicyID.Value);
        loadAddress(hdfAddressID.Value);
       
    }
    protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
   
    protected void btnSaveCustPersonal_Click(object sender, EventArgs e)
    {
        bl_customer cust = new bl_customer();
        cust.Customer_ID = hdfCustomerID.Value;
        cust.First_Name = txtFirstName.Text.Trim();
        cust.Last_Name = txtLastName.Text.Trim();
        cust.ID_Type = Int32.Parse(ddlIDType.SelectedValue);
        cust.ID_Card = txtIDNo.Text.Trim();
        cust.Country_ID = ddlNationality.SelectedValue;
        cust.Khmer_First_Name = txtKhmerFirstName.Text.Trim();
        cust.Khmer_Last_Name = txtKhmerLastName.Text.Trim();
        cust.Mother_First_Name = txtMotherFirstName.Text.Trim();
        cust.Mother_Last_Name = txtMotherLastName.Text.Trim();
        cust.Father_First_Name = txtFatherFirstName.Text.Trim();
        cust.Father_Last_Name = txtFatherLastName.Text.Trim();
        cust.Prior_First_Name = txtpriorFirstName.Text.Trim();
        cust.Prior_Last_Name = txtPriorLastName.Text.Trim();

        if (da_customer.UpdateCustomerByID(cust))
        {
            alert("Customer personal detail was updated successfully.");
            btnEditCustomer.Text = "Edit";
            btnSaveCustPersonal.Visible = false;
            loadPersonal(hdfCustomerID.Value);
            EnableCustomer(false);
        }
        else
        {
            alert("Customer personal detail was updated fail.");
        }
    }
    protected void btnSaveAddress_Click(object sender, EventArgs e)
    {
        bl_policy_address add = new bl_policy_address();
        add.Address_ID = hdfAddressID.Value;
        add.Address1 = txtAddress1.Text.Trim();
        add.Address2 = txtAddress2.Text.Trim();
        add.Address3 = txtAddress3.Text.Trim();
        add.Province = txtProvice.Text.Trim();
        add.Country_ID = ddlCountry.SelectedValue;
        add.Zip_Code = txtZipCode.Text.Trim();

        if (da_policy.UpdatePolicyAddress(add))
        {
            alert("Address was updated successfully.");
            btnEditAddress.Text = "Edit";
            btnSaveAddress.Visible = false;
            //reload updated address
            loadAddress(hdfAddressID.Value);
            EnableAddress(false);
        }
        else
        {
            alert("Address was updated fail.");
        }
    }
    void alert(string message)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert('"+ message+"');", true);
    }
    protected void btnEditAddress_Click(object sender, EventArgs e)
    {
        if (btnEditAddress.Text == "Edit")
        {
            btnEditAddress.Text = "Cancel";
            btnSaveAddress.Visible = true;
            EnableAddress(true);
        }
        else
        {
            btnEditAddress.Text = "Edit";
            btnSaveAddress.Visible = false;
            EnableAddress(false);
        }
    }
    protected void btnEditCustomer_Click(object sender, EventArgs e)
    {
        if (btnEditCustomer.Text == "Edit")
        {
            btnEditCustomer.Text = "Cancel";
            btnSaveCustPersonal.Visible = true;
            EnableCustomer(true);
        }
        else
        {
            btnEditCustomer.Text = "Edit";
            btnSaveCustPersonal.Visible = false;
            EnableCustomer(false);
        }
    }
    protected void btnEditContact_Click(object sender, EventArgs e)
    {
        if (btnEditContact.Text == "Edit")
        {
            btnEditContact.Text = "Cancel";
            btnSaveContact.Visible = true;
            EnableContact(true);
        }
        else
        {
            btnEditContact.Text = "Edit";
            btnSaveContact.Visible = false;
            EnableContact(false);
        }
    }
}