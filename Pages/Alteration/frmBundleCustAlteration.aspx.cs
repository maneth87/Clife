using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Data;
using System.IO;
public partial class Pages_Alteration_frmBundleCustAlteration : System.Web.UI.Page
{
    private bool PerView { get { return (bool)ViewState["V_PERM_VIEW"]; } set { ViewState["V_PERM_VIEW"] = value; } }
    private bool PerAdd { get { return (bool)ViewState["V_PERM_ADD"]; } set { ViewState["V_PERM_ADD"] = value; } }
    private bool PerUpdate { get { return (bool)ViewState["V_PERM_UPDATE"]; } set { ViewState["V_PERM_UPDATE"] = value; } }
    private bool PerAdmin { get { return (bool)ViewState["V_PERM_ADM"]; } set { ViewState["V_PERM_ADM"] = value; } }
    private ButtonType ButtonEdit { get { return (ButtonType)ViewState["V_EDIT_TYPE"]; } set { ViewState["V_EDIT_TYPE"] = value; } }
    private ButtonType ButtonUpdate { get { return (ButtonType)ViewState["V_UPDATE_TYPE"]; } set { ViewState["V_UPDATE_TYPE"] = value; } }
    private string LoginUser { get { return ViewState["V_USER_LOGIN"] + ""; } set { ViewState["V_USER_LOGIN"] = value; } }
    private List<bl_micro_group_policy> PolicyList { get { return (List<bl_micro_group_policy>)ViewState["VS_POLICY_LIST"]; } set { ViewState["VS_POLICY_LIST"] = value; } }
    private List<bl_micro_group_policy_detail> PolicyDetailList { get { return (List<bl_micro_group_policy_detail>)ViewState["VS_POLICY_DETAIL_LIST"]; } set { ViewState["VS_POLICY_DETAIL_LIST"] = value; } }

    private enum ButtonType { CUSTOMER, CONTACT, ADDRESS, BENEFICIARY }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            PolicyList = new List<bl_micro_group_policy>();
            PolicyDetailList = new List<bl_micro_group_policy_detail>();
        }
        LoginUser = Membership.GetUser().UserName;

        string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
        List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
        bl_sys_user_role ur = new bl_sys_user_role();
        if (Lobj != null)
        {
            bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);

            PerView = u.IsView == 1 ? true : false;
            PerAdd = u.IsAdd == 1 ? true : false;
            PerAdmin = u.IsAdmin == 1 ? true : false;
            PerUpdate = u.IsUpdate == 1 ? true : false;
            lblError.Text = "";
            if (!Page.IsPostBack)
            {
                PageTransaction(Helper.FormTransactionType.FIRST_LOAD);
            }
        }
        else
        {
            Response.Redirect("../../unauthorize.aspx");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtCustomerNoSearch.Text.Trim() != "")
        {
            PageTransaction(Helper.FormTransactionType.SEARCH);
        }
        else
        {
            Helper.Alert(false, "Please input Customer Number.", lblError);
        }
    }
    protected void btnEditCustomer_Click(object sender, EventArgs e)
    {
        if (btnEditCustomer.Text == "Edit")
        {
            ButtonEdit = ButtonType.CUSTOMER;// "btnEditCustomer";
            PageTransaction(Helper.FormTransactionType.EDIT);
        }
        else
        {
            ButtonEdit = ButtonType.CUSTOMER;// "btnEditCustomer";
            PageTransaction(Helper.FormTransactionType.CANCEL);
        }
    }
    protected void btnUpdateContact_Click(object sender, EventArgs e)
    {

        ButtonUpdate = ButtonType.CONTACT;
        PageTransaction(Helper.FormTransactionType.UPDATE);

    }
    protected void btnEditContact_Click(object sender, EventArgs e)
    {
        if (btnEditContact.Text == "Edit")
        {
            ButtonEdit = ButtonType.CONTACT;// "btnEditContact";
            PageTransaction(Helper.FormTransactionType.EDIT);
        }
        else
        {
            ButtonUpdate = ButtonType.CONTACT;
            PageTransaction(Helper.FormTransactionType.CANCEL);
        }
    }
    protected void btnUpdateAddress_Click(object sender, EventArgs e)
    {
        ButtonUpdate = ButtonType.ADDRESS;
        PageTransaction(Helper.FormTransactionType.UPDATE);
    }
    protected void btnUpdateBen_Click(object sender, EventArgs e)
    {
        ButtonUpdate = ButtonType.BENEFICIARY;
        PageTransaction(Helper.FormTransactionType.UPDATE);
    }
    protected void btnUpdateCust_Click(object sender, EventArgs e)
    {
        ButtonUpdate = ButtonType.CUSTOMER;
        PageTransaction(Helper.FormTransactionType.UPDATE);
    }

    void EnableCusotmer(bool t = true)
    {
        txtLastNameKh.Enabled = t;
        txtFirstNameKh.Enabled = t;
        txtLastNameEn.Enabled = t;
        txtFirstNameEn.Enabled = t;
        ddlGender.Enabled = t;
        txtDOB.Enabled = t;
        ddlIdType.Enabled = t;
        txtIdNo.Enabled = t;
        ddlNationality.Enabled = t;
        ddlOccupation.Enabled = t;
        ddlMaritalStatus.Enabled = t;
        btnUpdateCust.Enabled = t;
        btnEditCustomer.Enabled = t;
    }
    void EnableContact(bool t = true)
    {
        txtPhoneNumber.Enabled = t;
        txtPhoneNumber2.Enabled = t;
        txtPhoneNumber3.Enabled = t;
        txtEmail.Enabled = t;
        txtEmail2.Enabled = t;
        txtEmail3.Enabled = t;
        btnUpdateContact.Enabled = t;
        btnEditContact.Enabled = t;
    }
    void EnableAddress(bool t = true)
    {
        txtAddress.Enabled = t;
        txtRemarksAddress.Enabled = t;
        btnUpdateAddress.Enabled = t;
        btnEditAddress.Enabled = t;
    }
    void EnableBeneficiary(bool t = true)
    {
        ddlPolicyNumber.Enabled = t;
        txtBenFullName.Enabled = t;
        ddlBenGender.Enabled = t;
        txtBenAge.Enabled = t;
        ddlBenRelation.Enabled = t;
        txtPercentage.Enabled = t;
        txtBenAddress.Enabled = t;
        txtBenRemarks.Enabled = t;
        btnUpdateBen.Enabled = t;
        btnEditBen.Enabled = t;
    }

    void EnableEditButtons(bool t = true)
    {
        btnEditCustomer.Enabled = t;
        btnEditContact.Enabled = t;
        btnEditAddress.Enabled = t;
        btnEditBen.Enabled = t;
    }
    void ClearBen()
    {
        ddlPolicyNumber.SelectedIndex = ddlPolicyNumber.Items.Count > 0 ? 0 : -1;
        hdfBenId.Value = "";
        txtBenFullName.Text = "";
        ddlBenGender.SelectedIndex = 0;
        txtBenAge.Text = "";
        ddlBenRelation.SelectedIndex = 0;
        txtPercentage.Text = "";
        txtBenAddress.Text = "";
        txtBenRemarks.Text = "";
    }
    void ClearAddress()
    {
        hdfAddressId.Value = "";
        txtAddress.Text = "";
        txtRemarksAddress.Text = "";
    }
    void ClearContact()
    {
        hdfContactId.Value = "";
        txtPhoneNumber.Text = "";
        txtPhoneNumber2.Text = "";
        txtPhoneNumber3.Text = "";
        txtEmail.Text = "";
        txtEmail2.Text = "";
        txtEmail3.Text = "";
    }
    void ClearCustomer()
    {
        hdfCustomerId.Value = "";
        txtCustomerNo.Text = "";
        txtLastNameKh.Text = "";
        txtFirstNameKh.Text = "";
        txtLastNameEn.Text = "";
        txtFirstNameEn.Text = "";
        ddlGender.SelectedIndex = ddlGender.Items.Count > 0 ? 0 : -1;
        txtDOB.Text = "";
        ddlIdType.SelectedIndex = ddlIdType.Items.Count > 0 ? 0 : -1;
        txtIdNo.Text = "";
        ddlNationality.SelectedIndex = ddlNationality.Items.Count > 0 ? 0 : -1;
        ddlOccupation.SelectedIndex = ddlOccupation.Items.Count > 0 ? 0 : -1;
        ddlMaritalStatus.SelectedIndex = ddlMaritalStatus.Items.Count > 0 ? 0 : -1;
    }

    void ClearAll()
    {
        ClearCustomer();
        ClearContact();
        ClearAddress();
        ClearBen();
    }

    void InitialDefalt()
    {
        txtCustomerNo.Enabled = false;

        Options.Bind(ddlGender, da_master_list.GetMasterList("GENDER"), "DescEn", "Code", -1);
        Options.Bind(ddlIdType, da_master_list.GetMasterList("ID_TYPE"), "DescEn", "Code", -1);
        Options.Bind(ddlNationality, da_master_list.GetMasterList("NATIONALITY"), "DescKh", "Code", -1);
        Options.Bind(ddlMaritalStatus, da_master_list.GetMasterList("MARITAL_STATUS"), "DescKh", "Code", -1);

        Options.Bind(ddlBenGender, da_master_list.GetMasterList("GENDER"), "DescEn", "Code", -1);
        Options.Bind(ddlBenRelation, da_master_list.GetMasterList("BENEFICIARY_RELATION"), "DescKh", "Code", -1);

        Helper.BindOccupation(ddlOccupation, "KH");
    }

    void PageTransaction(Helper.FormTransactionType type)
    {

        #region First Load
        if (type == Helper.FormTransactionType.FIRST_LOAD)
        {
            InitialDefalt();
            EnableCusotmer(false);
            EnableContact(false);
            EnableAddress(false);
            EnableBeneficiary(false);

            //if (PerUpdate || PerAdmin)
            //{
            //    //btnEditCustomer.Enabled = true;
            //}
            //else
            //{
            // //   btnEditCustomer.Enabled = false;
            //    Helper.Alert(false, "Your user does not have privilege to edit customer information. ", lblError);
            //}

        }
        #endregion first load
        #region edit
        else if (type == Helper.FormTransactionType.EDIT)
        {
            if (PerUpdate || PerAdmin)
            {
                if (ButtonEdit == ButtonType.CUSTOMER)
                {
                    EnableCusotmer();
                    EnableContact(false);
                    EnableAddress(false);
                    EnableBeneficiary(false);
                    btnEditCustomer.Text = "Cancel";
                }
                else if (ButtonEdit == ButtonType.CONTACT)
                {
                    EnableContact();
                    EnableCusotmer(false);
                    EnableAddress(false);
                    EnableBeneficiary(false);
                    btnEditContact.Text = "Cancel";
                }
                else if (ButtonEdit == ButtonType.ADDRESS)
                {
                    EnableAddress();
                    EnableContact(false);
                    EnableCusotmer(false);
                    EnableBeneficiary(false);
                    btnEditAddress.Text = "Cancel";

                }
                else if (ButtonEdit == ButtonType.BENEFICIARY)
                {

                    EnableAddress(false);
                    EnableContact(false);
                    EnableCusotmer(false);
                    EnableBeneficiary();
                    btnEditBen.Text = "Cancel";
                }
            }
            else
            {
                btnEditCustomer.Enabled = false;
                Helper.Alert(false, "Your user does not have privilege to edit customer information. ", lblError);
            }
        }
        #endregion edit
        #region cancel
        else if (type == Helper.FormTransactionType.CANCEL)
        {
            if (ButtonEdit == ButtonType.CUSTOMER)
            {
                EnableCusotmer(false);
                EnableContact(false);
                EnableAddress(false);
                EnableBeneficiary(false);
                btnEditCustomer.Text = "Edit";
            }
            else if (ButtonEdit == ButtonType.CONTACT)
            {
                EnableContact(false);
                EnableCusotmer(false);
                EnableAddress(false);
                EnableBeneficiary(false);
                btnEditContact.Text = "Edit";
            }
            else if (ButtonEdit == ButtonType.ADDRESS)
            {
                EnableAddress(false);
                EnableContact(false);
                EnableCusotmer(false);
                EnableBeneficiary(false);
                btnEditAddress.Text = "Edit";
            }
            else if (ButtonEdit == ButtonType.BENEFICIARY)
            {

                EnableAddress(false);
                EnableContact(false);
                EnableCusotmer(false);
                EnableBeneficiary(false);
                btnEditBen.Text = "Edit";
            }
            EnableEditButtons();
        }
        #endregion cancel
        #region search
        else if (type == Helper.FormTransactionType.SEARCH)
        {
            if (PerView || PerUpdate || PerAdmin)
            {
                ClearAll();
                EnableCusotmer(false);
                EnableContact(false);
                EnableAddress(false);
                EnableBeneficiary(false);
                btnEditCustomer.Text = "Edit";
                btnEditContact.Text = "Edit";
                btnEditAddress.Text = "Edit";
                btnEditBen.Text = "Edit";
                /*process search*/
                Search(txtCustomerNoSearch.Text);
            }
            else
            {
                // btnEditCustomer.Enabled = false;
                Helper.Alert(false, "Your user does not have privilege to perform any activities. ", lblError);
            }
        }
        #endregion search
        #region update
        else if (type == Helper.FormTransactionType.UPDATE)
        {
            try
            {
                if (PerUpdate || PerAdmin)
                {

                    string message;
                    string appID = "";
                    int newAge = 0;
                    string strResultUpdate = "";

                    #region update customer
                    if (ButtonUpdate == ButtonType.CUSTOMER)
                    {

                        if (UpdateCustomer(out message))
                        {
                            strResultUpdate = "Customer information [" + txtCustomerNo.Text.Trim() + "] - Updated successfully.<br />";
                            string strUpateApplicationInfo = "";
                            string appNo = "";
                            foreach (bl_micro_group_policy pol in PolicyList)
                            {
                                DataTable tbl = da_group_micro_application.GetApplicationDetail(pol.ApplicationId);
                                if (tbl.Rows.Count > 0)
                                {
                                    var r = tbl.Rows[0];
                                    appNo = r["application_number"].ToString();
                                }

                                appNo = appNo == "" ? pol.ApplicationId : appNo;

                                if (UpdateApplicationCustomerInfo(pol.ApplicationId, out message))
                                {
                                    strUpateApplicationInfo += "Application information [" + appNo + "]- Updated successfully. <br />";
                                }
                                else
                                {
                                    strUpateApplicationInfo += "Application information [" + appNo + "] - Updated fail. <br />";
                                }
                                if (UpdateCertificateCustomerInfo(pol.PolicyNumber, out message))
                                {

                                    strUpateApplicationInfo += "Certificate information [" + pol.PolicyNumber + "] - Updated successfully. <br />";
                                }
                                else
                                {
                                    strUpateApplicationInfo += "Certificate information [" + pol.PolicyNumber + "] - Updated fail. <br />";
                                }

                            }


                            /*update issue age in policy detail*/
                            DateTime newDob = Helper.FormatDateTime(txtDOB.Text.Trim());
                            if (newDob != Helper.FormatDateTime(hdfDOB.Value))
                            {
                                foreach (var a in PolicyDetailList)
                                {
                                    newAge = Calculation.Culculate_Customer_Age_Micro(newDob, a.IssuedDate);
                                    if (da_micro_group_policy_detail.UpdatedAge(a.PolicyID, newAge, LoginUser, DateTime.Now, "Update issue age"))
                                    {
                                        strUpateApplicationInfo += "Policy Detail [" + a.PolicyDetailId + "] - Updated issue age successfully. <br />";
                                    }
                                    else
                                    {
                                        strUpateApplicationInfo += "Policy Detail [" + a.PolicyDetailId + "] - Updated issue age fail. <br />";
                                    }
                                }
                            }

                            strResultUpdate += strUpateApplicationInfo;
                            Helper.Alert(false, strResultUpdate + appID, lblError);

                            btnEditCustomer_Click(null, null);
                        }
                        else
                        {
                            Helper.Alert(true, message, lblError);
                        }
                    }
                    #endregion update customer
                    #region update contact
                    else if (ButtonUpdate == ButtonType.CONTACT)
                    {
                        if (UpdateCustomerContact(out message))
                        {
                            strResultUpdate = "Customer contact [" + txtCustomerNo.Text.Trim() + "] - Updated successfully. <br />";
                            string appNo = "";
                            foreach (bl_micro_group_policy pol in PolicyList)
                            {
                                DataTable tbl = da_group_micro_application.GetApplicationDetail(pol.ApplicationId);
                                if (tbl.Rows.Count > 0)
                                {
                                    var r = tbl.Rows[0];
                                    appNo = r["application_number"].ToString();
                                }

                                appNo = appNo == "" ? pol.ApplicationId : appNo;

                                if (UpdateApplicationContact(pol.ApplicationId, out message))
                                {
                                    strResultUpdate += "Application contact [" + appNo + "] - Updated successfully. <br />";
                                }
                                else
                                {
                                    strResultUpdate += "Application contact [" + appNo + "] - Updated fail. <br />";
                                }
                            }
                            Helper.Alert(false, strResultUpdate, lblError);
                            btnEditContact_Click(null, null);
                        }
                        else
                        {
                            Helper.Alert(true, message, lblError);
                        }
                    }
                    #endregion update contact
                    #region update address
                    else if (ButtonUpdate == ButtonType.ADDRESS)
                    {
                        if (UpdateCustomerAddress(out message))
                        {
                            strResultUpdate = "Customer address [" + txtCustomerNo.Text.Trim() + "] - Updated successfully. <br />";
                            string appNo = "";
                            foreach (bl_micro_group_policy pol in PolicyList)
                            {
                                DataTable tbl = da_group_micro_application.GetApplicationDetail(pol.ApplicationId);
                                if (tbl.Rows.Count > 0)
                                {
                                    var r = tbl.Rows[0];
                                    appNo = r["application_number"].ToString();
                                }

                                appNo = appNo == "" ? pol.ApplicationId : appNo;
                                if (UpdateApplicationAddress(pol.ApplicationId, out message)) {
                                    strResultUpdate += "Application address [" + appNo + "] - Updated successfully. <br />";
                                }
                                else
                                {
                                    strResultUpdate += "Application address [" + appNo + "] - Updated fail. <br />";
                                }
                                if (UpdateCertificateAddress(pol.PolicyNumber, out message))
                                {
                                    strResultUpdate += "Certificate address [" + pol.PolicyNumber + "] - Updated successfully. <br />";
                                }
                                else {
                                    strResultUpdate += "Certificate address [" + pol.PolicyNumber + "] - Updated fail. <br />";
                                }
                            }
                            Helper.Alert(false, strResultUpdate, lblError);
                            btnEditAddress_Click(null, null);
                        }
                        else
                        {
                            Helper.Alert(true, message, lblError);
                        }
                    }
                    #endregion update address

                    #region update beneficiary
                    else if (ButtonUpdate == ButtonType.BENEFICIARY)
                    {
                        if (ValidateBeneficiary(out message))
                        {
                            foreach (var pol in PolicyList.Where(_ => _.PolicyNumber == ddlPolicyNumber.SelectedItem.Text))
                            {
                                //if (UpdateApplicationBeneficiary(pol.ApplicationId, out message))
                                //{
                                //    Helper.Alert(false, "update beneficairy successully.", lblError);
                                //}
                                //else
                                //{
                                //    Helper.Alert(true, message, lblError);
                                //}
                                if (UpdateApplicationBeneficiary(pol.ApplicationId, out message))
                                {
                                    strResultUpdate += "Application beneficiary [" + pol.ApplicationId + "] - Updated successfully. <br />";
                                }
                                else {
                                    strResultUpdate += "Application beneficiary [" + pol.ApplicationId + "] - Updated successfully. <br />";
                                }
                                if (UpdatePolicyBeneficiary(hdfBenId.Value, out message))
                                {
                                    strResultUpdate += "Policy beneficiary [" + pol.PolicyNumber + "] - Updated successfully. <br />";
                                }
                                else
                                {
                                    strResultUpdate += "Policy beneficiary [" + pol.PolicyNumber + "] - Updated fail. <br />";
                                }
                            }
                            Helper.Alert(false, strResultUpdate, lblError);
                            btnEditBen_Click(null, null);
                        }
                        else
                        {
                            Helper.Alert(true, message, lblError);
                        }
                    }
                    #endregion update beneficiary

                }
                else
                {
                    btnEditCustomer.Enabled = false;
                    Helper.Alert(false, "Your user does not have privilege to update any information. ", lblError);
                }
            }

            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [PageTransaction(Helper.FormTransactionType type) , Blog UPDATE] in class [frmBundleCustAlteration.aspx.cs], detail: " + ex.Message + "=>" + ex.StackTrace);
            }

        }
        #endregion update
    }

    private bool UpdateCustomer(out string message)
    {
        bool result = false;
        bl_micro_group_customer cus = new bl_micro_group_customer();
        try
        {
            message = "";
            cus.FIRST_NAME_EN = txtFirstNameEn.Text.Trim();
            cus.LAST_NAME_EN = txtLastNameEn.Text.Trim();
            cus.FIRST_NAME_KH = txtFirstNameKh.Text.Trim();
            cus.LAST_NAME_KH = txtLastNameKh.Text.Trim();
            cus.FULL_NAME_EN = cus.LAST_NAME_EN + " " + cus.FIRST_NAME_EN;
            cus.FULL_NAME_KH = cus.LAST_NAME_KH + " " + cus.FIRST_NAME_KH;
            cus.GENDER = Convert.ToInt32(ddlGender.SelectedValue);
            cus.ID_TYPE = Convert.ToInt32(ddlIdType.SelectedValue);
            cus.ID_NUMBER = txtIdNo.Text.Trim();
            cus.NATIONALITY = ddlNationality.SelectedValue;
            cus.OCCUPATION = ddlOccupation.SelectedValue;
            cus.DOB = Helper.FormatDateTime(txtDOB.Text.Trim());
            cus.STATUS = 1;
            cus.MARITAL_STATUS = ddlMaritalStatus.SelectedValue;

            cus.UPDATED_BY = LoginUser;
            cus.UPDATED_ON = DateTime.Now;

            result = da_micro_group_customer.Update(cus, hdfCustomerId.Value);
            if (!result)
            {
                message = "Update customer information fail.";
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateCustomer(out string  messsage)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update customer information is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateCustomerContact(out string message)
    {
        bool result = false;
        bl_micro_group_customer_contact cus = new bl_micro_group_customer_contact();
        try
        {
            message = "";
            cus.CUSTOMER_ID = hdfCustomerId.Value;
            cus.PHONE_NUMBER1 = txtPhoneNumber.Text.Trim();
            cus.PHONE_NUMBER2 = txtPhoneNumber2.Text.Trim();
            cus.PHONE_NUMBER3 = txtPhoneNumber3.Text.Trim();
            cus.EMAIL1 = txtEmail.Text.Trim();
            cus.EMAIL2 = txtEmail2.Text.Trim();
            cus.EMAIL3 = txtEmail3.Text.Trim();
            cus.UPDATED_BY = LoginUser;
            cus.UPDATED_ON = DateTime.Now;

            result = da_micro_group_customer.Contact.Update(cus, hdfContactId.Value);
            if (!result)
            {
                message = "Update customer contact fail.";
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateCustomerContact(out string  messsage)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update customer contact is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateCustomerAddress(out string message)
    {
        bool result = false;
        bl_address cus = new bl_address();
        try
        {
            message = "";
            cus.CUSTOMER_ID = hdfCustomerId.Value;
            cus.ADDRESS1 = txtAddress.Text.Trim();
            cus.UPDATED_BY = LoginUser;
            cus.UPDATED_ON = DateTime.Now;
            cus.REMARKS = txtRemarksAddress.Text.Trim();
            result = da_micro_group_customer.Address.Update(cus, hdfAddressId.Value);
            if (!result)
            {
                message = "Update customer address fail.";
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateCustomerContact(out string  messsage)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update customer address is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }


    private bool UpdateApplicationCustomerInfo(string applicationId, out string message)
    {
        bool result = false;
        try
        {
            message = "";
            int idtype = Convert.ToInt32(ddlIdType.SelectedValue);
            int gender = Convert.ToInt32(ddlGender.SelectedValue);

            result = da_group_micro_application.UpdateCustomerInfo(applicationId, idtype, Helper.GetIDCardTypeText(idtype), Helper.GetIDCardTypeTextKh(idtype), txtIdNo.Text.Trim(), txtFirstNameEn.Text.Trim(), txtLastNameEn.Text.Trim(), txtFirstNameKh.Text.Trim(), txtLastNameKh.Text.Trim(), gender, Helper.GetGenderText(gender, false), Helper.GetGenderText(gender, false, true), Helper.FormatDateTime(txtDOB.Text.Trim()), ddlNationality.SelectedItem.Text, ddlMaritalStatus.SelectedValue, ddlMaritalStatus.SelectedItem.Text, ddlOccupation.SelectedValue, ddlOccupation.SelectedValue);
            if (!result)
                message = "Update application customer information fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateApplication(string applicationNumber, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);
            message = "Update application customer information is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }
    private bool UpdateApplicationCustomerContact(string applicationId, out string message)
    {
        bool result = false;
        try
        {
            message = "";
            result = da_group_micro_application.UpdateCustomerContact(applicationId, txtPhoneNumber.Text.Trim(), txtEmail.Text.Trim());
            if (!result)
                message = "Update application customer contact fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateApplication(string applicationNumber, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);
            message = "Update application customer contact is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateCertificateCustomerInfo(string policyNumber, out string message)
    {
        bool result = false;
        try
        {
            message = "";
            int idtype = Convert.ToInt32(ddlIdType.SelectedValue);
            int gender = Convert.ToInt32(ddlGender.SelectedValue);
            string fullName = (txtFirstNameKh.Text.Trim() + txtLastNameKh.Text.Trim()) == "" ? txtLastNameEn.Text.Trim() + " " + txtFirstNameEn.Text.Trim() : txtFirstNameKh.Text.Trim() + " " + txtLastNameKh.Text.Trim();

            result = da_group_micro_certificate.UpdateCusotmerInfo(idtype, Helper.GetIDCardTypeText(idtype), Helper.GetIDCardTypeTextKh(idtype), txtIdNo.Text.Trim(), fullName, gender, Helper.GetGenderText(gender, false, true), Helper.GetGenderText(gender, false), Helper.FormatDateTime(txtDOB.Text.Trim()), ddlNationality.SelectedItem.Text, policyNumber);
            if (!result)
                message = "Update certificate customer information fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateCertificateCustomerInfo(string policyNumber, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update certificate customer information is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateApplicationContact(string applicationId, out string message)
    {
        bool result = false;
        try
        {
            message = "";
            int idtype = Convert.ToInt32(ddlIdType.SelectedValue);
            int gender = Convert.ToInt32(ddlGender.SelectedValue);

            result = da_group_micro_application.UpdateCustomerContact(applicationId, txtPhoneNumber.Text.Trim(), txtEmail.Text.Trim());
            if (!result)
                message = "Update application customer contact fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateApplicationContact(string applicationNumber, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update application customer contact is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateApplicationAddress(string applicationNumber, out string message)
    {
        bool result = false;
        try
        {
            message = "";
            int idtype = Convert.ToInt32(ddlIdType.SelectedValue);
            int gender = Convert.ToInt32(ddlGender.SelectedValue);

            bool isAddressKh = Helper.CheckContentUnicode(txtAddress.Text.Trim());

            result = da_group_micro_application.UpdateCustomerAddress(applicationNumber, isAddressKh == true ? "" : txtAddress.Text.Trim(), isAddressKh == true ? txtAddress.Text.Trim() : "");
            if (!result)
                message = "Update application customer address fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateApplicationAddress(string applicationNumber, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update application customer contact is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateCertificateAddress(string policyNumber, out string message)
    {
        bool result = false;
        try
        {
            message = "";
            int idtype = Convert.ToInt32(ddlIdType.SelectedValue);
            int gender = Convert.ToInt32(ddlGender.SelectedValue);

            result = da_group_micro_certificate.UpdateCusotmerAddress(txtAddress.Text.Trim(), policyNumber);
            if (!result)
                message = "Update certificate customer address fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateCertificateAddress(string policyNumber, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update certificate customer contact is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    private bool UpdateApplicationBeneficiary(string applicationId, out string message)
    {
        bool result = false;
        try
        {
            message = "";

            result = da_group_micro_application.UpdateCustomerBeneficiary(applicationId, txtBenFullName.Text.Trim(), txtBenAge.Text.Trim(), txtBenAddress.Text.Trim(), float.Parse(txtPercentage.Text.Trim()), ddlBenRelation.SelectedValue, ddlBenRelation.SelectedValue);
            if (!result)
                message = "Update application customer beneficiary fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateApplicationAddress(string applicationId, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update application customer beneficiary is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }
    private bool UpdatePolicyBeneficiary(string benId, out string message)
    {
        bool result = false;
        try
        {
            message = "";

            result = da_micro_group_policy_beneficiary.Update(new bl_micro_policy_beneficiary()
            {
                FULL_NAME = txtBenFullName.Text.Trim(),
                AGE = txtBenAge.Text.Trim(),
                Gender = ddlBenGender.SelectedValue,
                RELATION = ddlBenRelation.SelectedItem.Text,
                PERCENTAGE_OF_SHARE = Convert.ToDouble(txtPercentage.Text.Trim()),
                ADDRESS = txtBenAddress.Text.Trim(),
                REMARKS = txtBenRemarks.Text.Trim(),
                UPDATED_BY = LoginUser,
                UPDATED_ON = DateTime.Now
            }, benId);
            if (!result)
                message = "Update policy beneficiary fail.";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdatePolicyBeneficiary(string benId, out string message)] in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message);
            message = "Update policy beneficiary is getting error, Please contact your system administrator.";
            result = false;
        }
        return result;
    }

    void Search(string customerNumber)
    {

        try
        {

            bl_micro_group_customer cusObj = da_micro_group_customer.Get(customerNumber);
            if (da_micro_group_customer.SUCCESS && cusObj != null)
            {
                EnableEditButtons();

                hdfCustomerId.Value = cusObj.ID;
                txtCustomerNo.Text = cusObj.CUSTOMER_NUMBER;
                txtLastNameEn.Text = cusObj.LAST_NAME_EN;
                txtFirstNameEn.Text = cusObj.FIRST_NAME_EN;
                txtLastNameKh.Text = cusObj.LAST_NAME_KH;
                txtFirstNameKh.Text = cusObj.FIRST_NAME_KH;
                txtIdNo.Text = cusObj.ID_NUMBER;
                Helper.SelectedDropDownListIndex("VALUE", ddlIdType, cusObj.ID_TYPE + "");
                Helper.SelectedDropDownListIndex("VALUE", ddlGender, cusObj.GENDER + "");
                Helper.SelectedDropDownListIndex("VALUE", ddlMaritalStatus, cusObj.MARITAL_STATUS);
                Helper.SelectedDropDownListIndex("VALUE", ddlOccupation, cusObj.OCCUPATION);
                Helper.SelectedDropDownListIndex("VALUE", ddlNationality, cusObj.NATIONALITY);
                txtDOB.Text = cusObj.DOB.ToString("dd/MM/yyyy");
                hdfDOB.Value = cusObj.DOB.ToString("dd/MM/yyyy");

                bl_micro_group_customer_contact cont = da_micro_group_customer.Contact.Get(customerNumber);
                if (da_micro_group_customer.SUCCESS)
                {
                    hdfContactId.Value = cont.CONTACT_ID;
                    txtPhoneNumber.Text = cont.PHONE_NUMBER1;
                    txtPhoneNumber2.Text = cont.PHONE_NUMBER2;
                    txtPhoneNumber3.Text = cont.PHONE_NUMBER3;
                    txtEmail.Text = cont.EMAIL1;
                    txtEmail2.Text = cont.EMAIL2;
                    txtEmail3.Text = cont.EMAIL3;
                }

                bl_address ad = da_micro_group_customer.Address.Get(customerNumber);
                if (da_micro_group_customer.SUCCESS)
                {
                    hdfAddressId.Value = ad.ADDRESS_ID;
                    txtAddress.Text = ad.ADDRESS1;
                    txtRemarksAddress.Text = ad.REMARKS;
                }

                List<bl_micro_group_policy> poList = da_micro_group_policy.GetPolicyList(customerNumber);
                PolicyList = poList;


                if (da_micro_group_policy.SUCCESS)
                {

                    ddlPolicyNumber.Items.Clear();
                    Options.Bind(ddlPolicyNumber, poList, "PolicyNumber", "PolicyId", 0);
                }

                PolicyDetailList = da_micro_group_policy_detail.GetPolicyDetailList(customerNumber);

            }
            else
            {
                Helper.Alert(false, "No record found. for Customer No. <strong>" + txtCustomerNoSearch.Text + "</strong>", lblError);
                EnableEditButtons(false);
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
            Log.AddExceptionToLog("Error Function [Search(string customerNumber)]  in class [frmBundleCustAlteration.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
    }
    protected void ddlPolicyNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPolicyNumber.SelectedIndex > 0)
        {
            var ben = da_micro_group_policy_beneficiary.Get(ddlPolicyNumber.SelectedValue);
            if (da_micro_group_policy_beneficiary.SUCCESS)
            {
                hdfBenId.Value = ben.ID;
                txtBenFullName.Text = ben.FULL_NAME;
                Helper.SelectedDropDownListIndex("VALUE", ddlBenGender, ben.Gender);
                txtBenAge.Text = ben.AGE;
                Helper.SelectedDropDownListIndex("TEXT", ddlBenRelation, ben.RELATION);
                txtBenAddress.Text = ben.ADDRESS;
                txtPercentage.Text = ben.PERCENTAGE_OF_SHARE + "";
                txtRemarksAddress.Text = ben.REMARKS;
            }
        }
        else
        {
            ClearBen();


        }

    }
    protected void btnEditAddress_Click(object sender, EventArgs e)
    {
        if (btnEditAddress.Text == "Edit")
        {
            ButtonEdit = ButtonType.ADDRESS;// "btnEditAddress";
            PageTransaction(Helper.FormTransactionType.EDIT);
        }
        else {
            ButtonEdit = ButtonType.ADDRESS;// "btnEditAddress";
            PageTransaction(Helper.FormTransactionType.CANCEL);
        }
    }
    protected void btnEditBen_Click(object sender, EventArgs e)
    {
        if (btnEditBen.Text == "Edit")
        {
            ButtonEdit = ButtonType.BENEFICIARY;// "btnEditBen";
            PageTransaction(Helper.FormTransactionType.EDIT);
        }
        else
        {
            ButtonEdit = ButtonType.BENEFICIARY;// "btnEditBen";
            PageTransaction(Helper.FormTransactionType.CANCEL);
        }
    }

    private bool ValidateBeneficiary(out string message)
    {
        bool isValid = true;
        message = "";
        if (txtBenFullName.Text.Trim() == "")
        {
            isValid = false;
            message = "Full name is required.";
        }
        else if (ddlBenRelation.SelectedValue == "")
        {
            isValid = false;
            message = "Relation is required.";
        }
        else if (txtBenAddress.Text.Trim() == "")
        {
            isValid = false;
            message = "Address is required.";
        }
        else if (!Helper.IsNumber(txtPercentage.Text.Trim()))
        {
            isValid = false;
            message = "Percentage is required as numeric.";
        }
        else if (Convert.ToDouble(txtPercentage.Text.Trim()) != 100)
        {
            isValid = false;
            message = "Percentage must be equal to 100.";
        }

        return isValid;
    }



}