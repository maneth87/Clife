using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Data;
public partial class Pages_Business_banca_micro_application : System.Web.UI.Page
{
    private SESSION_PARA MYPARA { get { return (SESSION_PARA)ViewState["V_PARA"]; } set { ViewState["V_PARA"] = value; } }
    private bool IsUpgratePackage { get { return (bool)ViewState["V_UPGRATE"]; } set { ViewState["V_UPGRATE"] = value; } }
    private string LastApplicationType { get { return "" + ViewState["V_APP_TYPE"]; } set { ViewState["V_APP_TYPE"] = value; } }
    private bool isMobileAppPayment { get { return (bool)ViewState["V_MB_PAY"]; } set { ViewState["V_MB_PAY"] = value; } }
    private string postPaymentId { get { return "" + ViewState["V_POSTPAYMENT_ID"]; } set { ViewState["V_POSTPAYMENT_ID"] = value; } }
    private string LeadId { get { return ViewState["V_LEAD_ID"] + ""; } set { ViewState["V_LEAD_ID"] = value; } }
    private bl_micro_product_config ProductConfig { get { return (bl_micro_product_config)ViewState["V_PRODUCT_CONFIG"]; } set { ViewState["V_PRODUCT_CONFIG"] = value; } }
    private string AgentCode { get { return ViewState["V_AGENT_CODE"] + ""; } set { ViewState["V_AGENT_CODE"] = value; } }
    private string AgentName { get { return ViewState["V_AGENT_NAME"] + ""; } set { ViewState["V_AGENT_NAME"] = value; } }
    private bool IsValide { get { return (bool)ViewState["V_IS_VALIDE"]; } set { ViewState["V_IS_VALIDE"] = value; } }
    private string Message { get { return ViewState["V_MESSAGE"] + ""; } set { ViewState["V_MESSAGE"] = value; } }
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    string userName = "";

    void BindBaicSA(double[] sa)
    {
        ddlBasicSa.Items.Clear();
        ddlBasicSa.Items.Add(new ListItem("--- Select ---", "0"));
        foreach (double d in sa)
        {
            ddlBasicSa.Items.Add(new ListItem(d + "", d + ""));
        }

        if (sa.Count() == 1)
        {
            ddlBasicSa.SelectedIndex = 1;
            ddlBasicSa.Enabled = false;
        }
        else
        {
            ddlBasicSa.Enabled = true;
        }
    }
    void BindRiderSA(double[] sa)
    {
        ddlRiderSa.Items.Clear();
        ddlRiderSa.Items.Add(new ListItem("--- Select ---", "0"));
        foreach (double d in sa)
        {
            ddlRiderSa.Items.Add(new ListItem(d + "", d + ""));
        }

        if (sa.Count() == 1)
        {
            ddlRiderSa.SelectedIndex = 1;
            ddlRiderSa.Enabled = false;
        }
        else
        {
            ddlRiderSa.Enabled = true;
        }
    }
    void BindPayMode(int[] mode)
    {
        ddlPaymentMode.Items.Clear();
        ddlPaymentMode.Items.Add(new ListItem("--- Select ---", "-1"));
        bl_payment_mode objMode = new bl_payment_mode();
        foreach (int d in mode)
        {
            objMode = da_payment_mode.GetPaymentModeByPayModeID(d);
            ddlPaymentMode.Items.Add(new ListItem(objMode.Mode, objMode.Pay_Mode_ID + ""));
        }

        if (mode.Count() == 1)
        {
            ddlPaymentMode.SelectedIndex = 1;
            ddlPaymentMode.Enabled = false;
        }
        else
        {
            ddlPaymentMode.Enabled = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";

        userName = Membership.GetUser().UserName;
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (!Page.IsPostBack)
        {
            SESSION_PARA para1 = new SESSION_PARA();
            para1 = (SESSION_PARA)Session["SS_SESSION_LEAD"];
            if (para1 != null)
            {
                this.MYPARA = para1;
                this.IsUpgratePackage = false;
                if (Request.QueryString.Count == 0)
                {
                    // Alert("Parrameter not found.");
                    Helper.Alert(true, "Invarlid Parameters", lblError);
                    DisabledAllControls();
                    btnIssue.Attributes.Add("disabled", "disabled");
                    txtIssueDate.Attributes.Add("disabled", "disabled");
                    txtUserPremium.Attributes.Add("disabled", "disabled");
                    txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
                    ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                    ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                    ibtnSave.Attributes.Add("disabled", "disabled");
                }
                else
                {

                    //List<bl_payment_mode> modeList = da_payment_mode.GetPaymentModeList();
                    //Options.Bind(ddlPaymentMode, modeList, bl_payment_mode.NAME.Mode, bl_payment_mode.NAME.Pay_Mode_ID, 0, "--- Select ---");

                    LeadId = Request.QueryString["ID"].ToString();

                    string applicationID = Request.QueryString["APP_ID"].ToString();
                    //Helper.BindProvince(ddlProvinceKh, "KH");
                    BindProvince();

                    Helper.BindOccupation(ddlOccupation, "KH");
                    Helper.BindRelation(ddlRelation, "KH");
                    Helper.BindMasterRelation(ddlClientTypeRelation, "CLIENT_TYPE_RELATION");

                    /*bind saleable product list */
                    List<bl_micro_product_config> proConList = da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(para1.ChannelItemId, true);
                    Options.Bind(ddlPackage, proConList, bl_micro_product_config.NAME.MarketingName, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");

                    bl_customer_lead_app_temp cus = new bl_customer_lead_app_temp();

                    // txtRiderSumAssure.Text = proRider.SUM_ASSURE_MIN + "";
                    if (applicationID != "")
                    {
                        hdfApplicationID.Value = applicationID;
                    }
                    else
                    {
                        //recheck existing data
                        bl_customer_lead cusLead = da_customer_lead.GetCustomerLeadByLeadID(LeadId);
                        if (cusLead.ID != "" && cusLead.ID != null)
                        {
                            bl_micro_application app = da_micro_application.GetApplication(cusLead.InsuranceApplicationId);
                            if (app.APPLICATION_NUMBER != "" && app.APPLICATION_NUMBER != null)
                            {
                                hdfApplicationID.Value = app.APPLICATION_ID;
                            }
                        }
                    }

                    InitialDefault();
                    #region //check existing
                    if (hdfApplicationID.Value != "")
                    {
                        BindExisting();

                    }
                    #endregion //check existing
                    #region new application
                    else
                    {
                        //cus = da_customer_lead_app_temp.GetCustomerLeadAppTempByID("4E00C1B6-5651-4175-B73C-1B8F96A05F14");
                        cus = da_customer_lead_app_temp.GetCustomerLeadAppTempByID(LeadId);// (Request.QueryString["ID"].ToString());
                        //get channel 
                        hdfChannelItemID.Value = cus.CHANNEL_ITEM_ID;
                        hdfChannelLocationID.Value = cus.CHANNEL_LOCATION_ID;

                        //List<bl_referral> refList = da_referral.GetActiveReferral(hdfChannelItemID.Value, hdfChannelLocationID.Value, cus.ReferralStaffId, userName);
                        //if (refList.Count > 0)// varlid referral
                        //{
                        lblApplicationType.Text = this.LastApplicationType == "" ? MYPARA.ApplicationType : this.LastApplicationType;

                        //ViewState["AGENT_CODE"] = cus.SALE_AGENT_ID;
                        // ViewState["AGENT_NAME"] = cus.SALE_AGENT_NAME;
                        AgentCode = cus.SALE_AGENT_ID;
                        AgentName = cus.SALE_AGENT_NAME;
                        //applicaiton information
                        txtBranchCode.Text = cus.BranchCode;
                        txtBranchName.Text = cus.BranchName;

                        txtSaleAgentID.Text = cus.SALE_AGENT_ID;
                        txtSaleAgentName.Text = cus.SALE_AGENT_NAME;
                        hdfSaleAgentID.Value = cus.SALE_AGENT_ID;
                        txtApplicationDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                        txtReferrerId.Text = cus.ReferralStaffId;
                        txtReferrerName.Text = cus.ReferralStaffName;

                        if (!string.IsNullOrEmpty(MYPARA.OldPolicyNumber))//if (MYPARA.OldPolicyNumber != null)
                        {
                            //existing customer information
                            bl_micro_customer1 polCus = new bl_micro_customer1();
                            polCus = da_micro_customer.GetCustomer(MYPARA.CustomerId, userName);
                            Helper.SelectedDropDownListIndex("VALUE", ddlIDType, polCus.ID_TYPE);
                            txtIDNumber.Text = polCus.ID_NUMBER;
                            txtReletedCertificate.Text = MYPARA.OldPolicyNumber + "@" + MYPARA.OldPolicyStatus;

                            txtSurnameKh.Text = polCus.LAST_NAME_IN_KHMER;
                            txtSurnameEng.Text = polCus.LAST_NAME_IN_ENGLISH;
                            txtFirstNameEng.Text = polCus.FIRST_NAME_IN_ENGLISH;
                            txtFirstNameKh.Text = polCus.FIRST_NAME_IN_KHMER;
                            txtNationality.Text = polCus.NATIONALITY;
                            Helper.SelectedDropDownListIndex("VALUE", ddlGender, polCus.GENDER);
                            txtDateOfBirth.Text = polCus.DATE_OF_BIRTH.ToString("dd-MM-yyyy");
                            //txtDateOfBirth_TextChanged(null, null);
                            txtCustomerAge.Text = CalculateAge(txtDateOfBirth.Text.Trim(), txtApplicationDate.Text.Trim()) + "";
                            Helper.SelectedDropDownListIndex("VALUE", ddlMaritalStatus, polCus.MARITAL_STATUS);
                            Helper.SelectedDropDownListIndex("VALUE", ddlOccupation, polCus.OCCUPATION);
                            txtPhoneNumber.Text = polCus.PHONE_NUMBER1;
                            txtEmail.Text = polCus.EMAIL1;

                            Helper.SelectedDropDownListIndex("VALUE", ddlProvinceKh, polCus.PROVINCE_EN + "/" + da_address.province.GetProvinceName(polCus.PROVINCE_EN));
                            ddlProvinceKh_SelectedIndexChanged(null, null);
                            Helper.SelectedDropDownListIndex("VALUE", ddlDistrictKh, polCus.DISTRICT_EN + "/" + da_address.district.GetDistrictName(polCus.PROVINCE_EN, polCus.DISTRICT_EN));
                            ddlDistrictKh_SelectedIndexChanged(null, null);
                            Helper.SelectedDropDownListIndex("VALUE", ddlCommuneKh, polCus.COMMUNE_EN + "/" + da_address.commune.GetCommuneName(polCus.DISTRICT_EN, polCus.COMMUNE_EN));
                            ddlCommuneKh_SelectedIndexChanged(null, null);
                            Helper.SelectedDropDownListIndex("VALUE", ddlVillageKh, polCus.VILLAGE_EN + "/" + da_address.village.GetVillageName(polCus.COMMUNE_EN, polCus.VILLAGE_EN));
                            ddlVillageKh_SelectedIndexChanged(null, null);
                            txtStreetEn.Text = polCus.STREET_NO_EN;
                            txtStreetKh.Text = polCus.STREET_NO_KH;
                            txtHouseNoEn.Text = polCus.HOUSE_NO_EN;
                            txtHouseNoKh.Text = polCus.HOUSE_NO_KH;
                            bl_micro_policy pol = da_micro_policy.GetPolicyByID(MYPARA.OldPolicyId, userName);
                            bl_micro_policy_beneficiary ben = da_micro_policy_beneficiary.GetBeneficiary(pol.POLICY_ID, userName);
                            txtBenAddress.Text = ben.ADDRESS;
                            txtFullName.Text = ben.FULL_NAME;
                            txtAge.Text = ben.AGE;
                            txtPercentageOfShare.Text = ben.PERCENTAGE_OF_SHARE + "";
                            Helper.SelectedDropDownListIndex("VALUE", ddlRelation, ben.RELATION);

                            bl_micro_application_questionaire ques = da_micro_application_questionaire.GetQuestionaire(MYPARA.OldApplicationNumber);
                            Helper.SelectedDropDownListIndex("VALUE", ddlAnswer, ques.ANSWER + "");
                            txtAnswerRemarks.Text = ques.ANSWER_REMARKS;

                            //disable cusotmer information

                            txtSurnameKh.Attributes.Add("disabled", "disabled");
                            txtSurnameEng.Attributes.Add("disabled", "disabled");
                            txtFirstNameEng.Attributes.Add("disabled", "disabled");
                            txtFirstNameKh.Attributes.Add("disabled", "disabled");
                            txtNationality.Attributes.Add("disabled", "disabled");
                            ddlGender.Attributes.Add("disabled", "disabled");
                            txtDateOfBirth.Attributes.Add("disabled", "disabled");

                            bl_micro_application_insurance appin = da_micro_application_insurance.GetApplicationInsurance(MYPARA.OldApplicationNumber, userName);
                            Helper.SelectedDropDownListIndex("VALUE", ddlPackage, appin.PRODUCT_ID);

                            hdfPackage.Value = appin.SUM_ASSURE + ""; //ddlPackage.SelectedValue;

                            ddlPackage_SelectedIndexChanged(null, null);


                            //POLICY DETAIL

                            bl_micro_policy_detail polD = da_micro_policy_detail.GetPolicyDetailByPolicyID(pol.POLICY_ID);

                            /*issue policy information
                            effective date = the expiry date of old policy + 1 day
                            */
                            txtEffectiveDate.Text = MYPARA.OldPolicyNumber != "" && MYPARA.OldPolicyStatus == "IF" ? MYPARA.PolicyNewEffectiveDate.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy");
                        }
                        else
                        {

                            //customer information
                            //ddlIDType.SelectedValue = cus.DocumentType;
                            Helper.SelectedDropDownListIndex("TEXT", ddlIDType, cus.DocumentType.ToUpper() == "NATIONAL ID" ? "ID Card" : cus.DocumentType);
                            txtIDNumber.Text = cus.DocumentId;

                            string[] en_name = new string[] { };
                            string[] kh_name = new string[] { };
                            en_name = cus.ClientNameENG.Split(' ');
                            kh_name = cus.ClientNameKHM.Split(' ');
                            if (en_name.Length > 0)
                            {

                                if (en_name.Length == 1)
                                {
                                    txtSurnameEng.Text = en_name[0].ToString();
                                    txtFirstNameEng.Text = "";
                                }
                                else
                                {
                                    txtSurnameEng.Text = en_name[0].ToString();
                                    txtFirstNameEng.Text = en_name[1].ToString();
                                }
                            }
                            else
                            {
                                txtSurnameEng.Text = cus.ClientNameENG;
                                txtFirstNameEng.Text = "";
                            }
                            if (kh_name.Length > 0)
                            {
                                if (kh_name.Length == 1)
                                {
                                    txtSurnameKh.Text = kh_name[0].ToString();
                                    txtFirstNameKh.Text = "";
                                }
                                else
                                {
                                    txtSurnameKh.Text = kh_name[0].ToString();
                                    txtFirstNameKh.Text = kh_name[1].ToString();
                                }
                            }
                            else
                            {
                                txtSurnameKh.Text = cus.ClientNameKHM;
                                txtFirstNameKh.Text = "";
                            }

                            txtNationality.Text = cus.ClientNationality.ToUpper() == "CAMBODIAN" ? "ខ្មែរ" : cus.ClientNationality;

                            Helper.SelectedDropDownListIndex("Text", ddlGender, cus.ClientGender);
                            txtDateOfBirth.Text = cus.ClientDoB.ToString("dd-MM-yyyy");


                            //calculate age
                            txtCustomerAge.Text = CalculateAge(txtDateOfBirth.Text.Trim(), txtApplicationDate.Text.Trim()) + "";

                            SelectProvince(cus.ClientProvince);
                            selectDistrict(cus.ClientDistrict);
                            selectCommune(cus.ClientCommune);
                            selectVillage(cus.ClientVillage);

                            txtStreetEn.Text = "";// cus.ADDRESS;
                            //txtVillageEn.Text = cus.ClientVillage;
                            //txtCommuneEn.Text = cus.ClientCommune;
                            //txtDistrictEn.Text = cus.ClientDistrict;
                            //txtProvinceEn.Text = cus.ClientProvince;

                            txtPhoneNumber.Text = cus.ClientPhoneNumber;

                            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        }


                        //InitialDefault();

                        ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                        ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                        btnIssue.Attributes.Add("disabled", "disabled");
                        txtIssueDate.Attributes.Add("disabled", "disabled");
                        txtUserPremium.Attributes.Add("disabled", "disabled");
                        txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");

                        btnGetPayment.Attributes.Add("disabled", "disabled");//V4: add new
                        //}
                        //else// referral not found
                        //{
                        //    Helper.Alert(true, "Referral Id [" + cus.ReferralStaffId + "] is not found in white list.", lblError);
                        //    DisabledAllControls();
                        //    ibtnSave.Attributes.Add("disabled", "disabled");
                        //    ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                        //    ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                        //    btnIssue.Attributes.Add("disabled", "disabled");
                        //    txtIssueDate.Attributes.Add("disabled", "disabled");
                        //    txtUserPremium.Attributes.Add("disabled", "disabled");
                        //    txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
                        //    btnGetPayment.Attributes.Add("disabled", "disabled");//V4: add new
                        //}

                    }
                    #endregion new application

                    //address beneficiary
                    //txtBenAddress.Text = cus.ADDRESS + " " + txtProvinceEn.Text;

                    DisabledControls();
                }

            }
            else
            {
                Helper.Alert(true, "Page was expired. please reload the page.", lblError);
            }
        }
    }

    #region Existing Application
    void BindExisting()
    {
        // SESSION_PARA para = (SESSION_PARA)Session["SS_SESSION_LEAD"];
        //da_micro_application_customer.
        bool isBackupApp = true;
        DataTable tbl_app = da_micro_application.GetApplicationDetailByApplicationID(hdfApplicationID.Value);
        bl_micro_product_config proConf = new bl_micro_product_config();
        if (tbl_app.Rows.Count > 0)
        {
            #region check policy status
            bl_micro_policy pol = new bl_micro_policy();
            pol = da_micro_policy.GetPolicyByApplicationID(hdfApplicationID.Value);
            if (pol.POLICY_NUMBER != null)
            {
                txtPolicyNumber.Text = pol.POLICY_NUMBER;
                txtPolicyStatus.Text = pol.POLICY_STATUS;
                hdfPolicyID.Value = pol.POLICY_ID;
                #region issue policy
                DataTable tbl_pay_detail = da_micro_policy_payment.GetPolicyPaymentDetail(pol.POLICY_ID);
                //get first payment
                foreach (DataRow r in tbl_pay_detail.Select("premium_year=1 and premium_lot=1"))
                {
                    txtIssueDate.Text = Convert.ToDateTime(r["issued_date"].ToString()).ToString("dd-MM-yyyy");
                    txtUserPremium.Text = r["user_premium"].ToString();
                    txtPaymentReferenceNo.Text = r["TRANSACTION_REFERRENCE_NO"].ToString();
                    txtEffectiveDate.Text = Convert.ToDateTime(r["effective_date"].ToString()).ToString("dd-MM-yyyy");
                }

                DisabledAllControls();
                #endregion issue policy

                btnIssue.Attributes.Add("disabled", "disabled");
                txtIssueDate.Attributes.Add("disabled", "disabled");
                txtUserPremium.Attributes.Add("disabled", "disabled");
                txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
                ibtnSave.Attributes.Add("disabled", "disabled");
                ibtnPrintCertificate.Attributes.Remove("disabled");
                btnGetPayment.Attributes.Add("disabled", "disabled"); //V4:add new
            }
            else // applicatoin is not yet convert to policy
            {
                ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                txtUserPremium.Attributes.Add("disabled", "disabled"); //V4: changed to disabled
                txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");//V4: changed to disabled
                btnIssue.Attributes.Add("disabled", "disabled");//V4: changed to disabled
                btnGetPayment.Attributes.Remove("disabled");//V4: added new
                ibtnSave.Attributes.Remove("disabled");
                txtPolicyNumber.Text = "";
                txtPolicyStatus.Text = "";
                txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtEffectiveDate.Text = MYPARA.OldPolicyStatus == "IF" ? MYPARA.PolicyNewEffectiveDate.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy");
                //Backup applicaiton
                ViewState["VS_BAK_DATE"] = DateTime.Now;
                isBackupApp = da_micro_application.BackupApplication(hdfApplicationID.Value, "UPDATE", userName, Convert.ToDateTime(ViewState["VS_BAK_DATE"] + ""));
            }
            #endregion check policy status

            if (isBackupApp)
            {
                // this.LastApplicationType = this.LastApplicationType == "" ? MYPARA.ApplicationType : this.LastApplicationType;
                lblApplicationType.Text = this.LastApplicationType == "" ? MYPARA.ApplicationType : this.LastApplicationType;
                if (MYPARA.OldPolicyNumber == null)
                {
                    txtReletedCertificate.Text = "";
                }
                else
                {
                    txtReletedCertificate.Text = pol.POLICY_NUMBER != null ? pol.RenewFromPolicy + "@" + MYPARA.OldPolicyStatus : MYPARA.OldPolicyNumber + "@" + MYPARA.OldPolicyStatus;
                }
                var app_row = tbl_app.Rows[0];
                Helper.SelectedDropDownListIndex("VALUE", ddlClientType, app_row["client_type"].ToString());
                ddlClientType_SelectedIndexChanged(null, null);
                txtBankStaffName.Text = app_row["client_type_remarks"].ToString();
                Helper.SelectedDropDownListIndex("VALUE", ddlClientTypeRelation, app_row["client_type_relation"].ToString());
                hdfOldClientType.Value = app_row["client_type"].ToString();
                hdfOldBankStaffName.Value = app_row["client_type_remarks"].ToString();
                hdfOldClientTypeRelation.Value = app_row["client_type_relation"].ToString();

                hdfProductID.Value = app_row["product_id"].ToString();
                proConf = da_micro_product_config.ProductConfig.GetProductMicroProduct(hdfProductID.Value);
                ProductConfig = proConf;

                txtApplicationNumber.Text = app_row["application_number"].ToString();
                txtApplicationDate.Text = Convert.ToDateTime(app_row["application_date"].ToString()).ToString("dd-MM-yyyy");
                hdfOldApplicationDate.Value = Convert.ToDateTime(app_row["application_date"].ToString()).ToString("dd-MM-yyyy");
                txtBranchCode.Text = app_row["office_code"].ToString();
                txtBranchName.Text = app_row["office_name"].ToString();
                txtSaleAgentID.Text = app_row["sale_agent_id"].ToString();
                txtSaleAgentName.Text = app_row["full_name"].ToString();

                

                //ViewState["AGENT_CODE"] = txtSaleAgentID.Text;
                //ViewState["AGENT_NAME"] = txtSaleAgentName.Text;
                AgentCode = txtSaleAgentID.Text;
                AgentName = txtSaleAgentName.Text;

                hdfApplicationNumber.Value = app_row["application_number"].ToString();
                hdfChannelItemID.Value = app_row["channel_item_id"].ToString();
                hdfChannelLocationID.Value = app_row["channel_location_id"].ToString();
                hdfSaleAgentID.Value = app_row["sale_agent_id"].ToString();
                hdfApplicationID.Value = app_row["application_id"].ToString();

                txtReferrerId.Text = app_row["referrer_id"].ToString();
                hdfOldReferrerId.Value = app_row["referrer_id"].ToString();
                txtReferrerName.Text = app_row["referrer"].ToString();

                // hdfCustomerID.Value = app_row["application_customer_id"].ToString();
                hdfApplicationCustomerID.Value = app_row["application_customer_id"].ToString();
                hdfApplicationNumber.Value = app_row["application_number"].ToString();

                #region customer information
                Helper.SelectedDropDownListIndex("VALUE", ddlIDType, app_row["id_type"].ToString());
                hdfOldIdType.Value = ddlIDType.SelectedItem.Text;
                txtIDNumber.Text = app_row["id_number"].ToString();
                hdfOldIdNumber.Value = app_row["id_number"].ToString();
                txtSurnameEng.Text = app_row["last_name_in_english"].ToString();
                hdfSurnameEng.Value = app_row["last_name_in_english"].ToString();
                txtFirstNameEng.Text = app_row["first_name_in_english"].ToString();
                hdfFirstNameEng.Value = app_row["first_name_in_english"].ToString();
                txtSurnameKh.Text = app_row["last_name_in_khmer"].ToString();
                hdfSurnameKh.Value = app_row["last_name_in_khmer"].ToString();
                txtFirstNameKh.Text = app_row["first_name_in_khmer"].ToString();
                hdfFirstnameKh.Value = app_row["first_name_in_khmer"].ToString();
                txtNationality.Text = app_row["nationality"].ToString();
                hdfNationlity.Value = app_row["nationality"].ToString();
                Helper.SelectedDropDownListIndex("VALUE", ddlGender, app_row["gender"].ToString());
                hdfGender.Value = ddlGender.SelectedItem.Text;
                txtDateOfBirth.Text = Convert.ToDateTime(app_row["date_of_birth"].ToString()).ToString("dd-MM-yyyy");
                hdfDateOfBirth.Value = Convert.ToDateTime(app_row["date_of_birth"].ToString()).ToString("dd-MM-yyyy");
                Helper.SelectedDropDownListIndex("VALUE", ddlMaritalStatus, app_row["marital_status"].ToString());
                hdfMaritalStatus.Value = ddlMaritalStatus.SelectedItem.Text;
                //txtOccupation.Text = app_row["occupation"].ToString();
                Helper.SelectedDropDownListIndex("TEXT", ddlOccupation, app_row["occupation"].ToString());
                hdfOccupation.Value = ddlOccupation.SelectedItem.Text;
                txtPhoneNumber.Text = app_row["phone_number"].ToString();
                hdfPhoneNumber.Value = app_row["phone_number"].ToString();
                txtEmail.Text = app_row["email"].ToString();
                hdfEmail.Value = app_row["email"].ToString();
                int age = CalculateAge(txtDateOfBirth.Text, txtApplicationDate.Text);
                txtCustomerAge.Text = age + "";
                hdfCustomerAge.Value = age + "";
                txtHouseNoEn.Text = app_row["house_no_en"].ToString();
                hdfHouseNoEn.Value = app_row["house_no_en"].ToString();
                txtStreetEn.Text = app_row["street_no_en"].ToString();
                hdfStreetEn.Value = txtStreetEn.Text = app_row["street_no_en"].ToString();
                string village = app_row["village_CODE"].ToString() == "" ? app_row["village_en"].ToString() : app_row["village_CODE"].ToString() + "/" + app_row["village_en"].ToString();
                string commune = app_row["commune_CODE"].ToString() == "" ? app_row["commune_en"].ToString() : app_row["commune_CODE"].ToString() + "/" + app_row["commune_en"].ToString();
                string district = app_row["district_CODE"].ToString() == "" ? app_row["district_en"].ToString() : app_row["district_CODE"].ToString() + "/" + app_row["district_en"].ToString();
                string province = app_row["province_CODE"].ToString() == "" ? app_row["province_en"].ToString() : app_row["province_CODE"].ToString() + "/" + app_row["province_en"].ToString();
                txtVillageEn.Text = village;
                hdfVillageEn.Value = village;
                txtCommuneEn.Text = commune;
                hdfCommuneEn.Value = commune;
                txtDistrictEn.Text = district;
                hdfDistrictEn.Value = district;
                txtProvinceEn.Text = province;
                hdfProvinceEn.Value = province;
                txtHouseNoKh.Text = app_row["house_no_kh"].ToString();
                hdfHouseNoKh.Value = app_row["house_no_kh"].ToString();
                txtStreetKh.Text = app_row["street_no_Kh"].ToString();
                hdfStreetKh.Value = app_row["street_no_Kh"].ToString();
                Helper.SelectedDropDownListIndex("TEXT", ddlProvinceKh, app_row["province_kh"].ToString());
                hdfProvinceKh.Value = ddlProvinceKh.SelectedItem.Text;
                //bind district
                BindDistrict();
                Helper.SelectedDropDownListIndex("TEXT", ddlDistrictKh, app_row["district_kh"].ToString());
                hdfDistrictKh.Value = ddlDistrictKh.SelectedItem.Text;
                //bind commune
                BindCommune();
                Helper.SelectedDropDownListIndex("TEXT", ddlCommuneKh, app_row["commune_kh"].ToString());
                hdfCommuneKh.Value = ddlCommuneKh.SelectedItem.Text;
                //bind village
                BindVillage();
                Helper.SelectedDropDownListIndex("TEXT", ddlVillageKh, app_row["village_kh"].ToString());
                hdfVillagekh.Value = ddlVillageKh.SelectedItem.Text;


                if (MYPARA.OldPolicyNumber != "" && MYPARA.OldPolicyNumber != null)
                {
                    txtSurnameKh.Attributes.Add("disabled", "disabled");
                    txtSurnameEng.Attributes.Add("disabled", "disabled");
                    txtFirstNameEng.Attributes.Add("disabled", "disabled");
                    txtFirstNameKh.Attributes.Add("disabled", "disabled");
                    txtNationality.Attributes.Add("disabled", "disabled");
                    ddlGender.Attributes.Add("disabled", "disabled");
                    txtDateOfBirth.Attributes.Add("disabled", "disabled");
                }

                #endregion customer information

                #region insurance product

                Helper.SelectedDropDownListIndex("VALUE", ddlPackage, app_row["product_id"].ToString());
                hdfPackage.Value = app_row["sum_assure"].ToString();// ddlPackage.SelectedValue;
                txtProductName.Text = app_row["product_name"].ToString();
                txtTermOfCover.Text = app_row["term_of_cover"].ToString();
                txtPremiumPaymentPeriod.Text = app_row["payment_period"].ToString();
                // txtSumAssure.Text = app_row["sum_assure"].ToString();
                BindBaicSA(proConf.BasicSumAssuredRange);
                BindPayMode(proConf.PayMode);
                Helper.SelectedDropDownListIndex("VALUE", ddlBasicSa, app_row["sum_assure"].ToString());
                Helper.SelectedDropDownListIndex("VALUE", ddlPaymentMode, app_row["pay_mode"].ToString());
                txtPremium.Text = app_row["premium"].ToString();
                txtAnnualPremium.Text = app_row["annual_premium"].ToString();

                txtBasicDiscount.Text = app_row["discount_amount"].ToString();
                txtBasicAfterDiscount.Text = app_row["total_amount"].ToString();


                #endregion insurance product

                #region rider
                hdfRiderProductID.Value = app_row["rider_product_id"].ToString();
                if (hdfRiderProductID.Value != "")
                {
                    BindRiderSA(proConf.RiderSumAssuredRange);

                }
                else
                {
                    ddlRiderSa.Enabled = false;
                }
                txtRiderProduct.Text = app_row["rider_product_name"].ToString();
                txtRiderPremium.Text = app_row["rider_premium"].ToString();
                txtRiderAnnualPremium.Text = app_row["rider_annual_premium"].ToString();
                //txtRiderSumAssure.Text = app_row["rider_sum_assure"].ToString();
                Helper.SelectedDropDownListIndex("VALUE", ddlRiderSa, app_row["rider_sum_assure"].ToString());
                txtRiderDiscount.Text = app_row["rider_discount_amount"].ToString();
                txtRiderAfterDiscount.Text = app_row["rider_total_amount"].ToString();

                #endregion rider
                double basicPremium = txtPremium.Text.Trim() != "" ? Convert.ToDouble(txtPremium.Text) : 0;
                double riderPremium = txtRiderPremium.Text.Trim() != "" ? Convert.ToDouble(txtRiderPremium.Text) : 0;
                double basicDis = txtBasicDiscount.Text.Trim() != "" ? Convert.ToDouble(txtBasicDiscount.Text) : 0;
                double riderDis = txtRiderDiscount.Text.Trim() != "" ? Convert.ToDouble(txtRiderDiscount.Text) : 0;
                double basicAfterDis = txtBasicAfterDiscount.Text.Trim() != "" ? Convert.ToDouble(txtBasicAfterDiscount.Text) : 0;
                double riderAferDis = txtRiderAfterDiscount.Text.Trim() != "" ? Convert.ToDouble(txtRiderAfterDiscount.Text) : 0;

                txtTotalPremium.Text = basicPremium + riderPremium + "";// (Convert.ToDouble(txtPremium.Text) + Convert.ToDouble(txtRiderPremium.Text)) + "";
                txtTotalDiscountAmount.Text = (basicDis + riderDis) + "";// (Convert.ToDouble(txtBasicDiscount.Text) + Convert.ToDouble(txtRiderDiscount.Text)) + "";
                txtTotalPremiumAfterDiscount.Text = (basicAfterDis + riderAferDis) + "";// (Convert.ToDouble(txtBasicAfterDiscount.Text) + Convert.ToDouble(txtRiderAfterDiscount.Text)) + "";

                #region issue policy

                #endregion issue policy

                #region beneficiary
                txtBenAddress.Text = app_row["ben_address"].ToString();
                hdfBenAddress.Value = app_row["ben_address"].ToString();
                txtFullName.Text = app_row["ben_full_name"].ToString();
                hdfFullName.Value = app_row["ben_full_name"].ToString();
                //txtRelation.Text = app_row["relation"].ToString();
                Helper.SelectedDropDownListIndex("TEXT", ddlRelation, app_row["relation"].ToString());
                hdfRelation.Value = ddlRelation.SelectedItem.Text;
                txtPercentageOfShare.Text = app_row["percentage_of_share"].ToString();
                txtAge.Text = app_row["ben_age"].ToString();
                hdfAge.Value = app_row["ben_age"].ToString();
                #endregion beneficiary
                #region question
                Helper.SelectedDropDownListIndex("Value", ddlAnswer, app_row["answer"].ToString());
                hdfAnswer.Value = ddlAnswer.SelectedItem.Text;
                txtAnswerRemarks.Text = app_row["answer_remarks"].ToString();
                hdfAnswerRemarks.Value = app_row["answer_remarks"].ToString();
                #endregion


                ibtnPrintApplication.Attributes.Remove("disabled");

            }
            else//backup application fail
            {
                ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                DisabledAllControls();
                Helper.Alert(true, "System is error while try to backup application", lblError);
            }
        }
        else // application not found
        {
            btnGetPayment.Attributes.Add("disabled", "disabled");// V4: added
            btnIssue.Attributes.Add("disabled", "disabled");
            txtIssueDate.Attributes.Add("disabled", "disabled");
            txtUserPremium.Attributes.Add("disabled", "disabled");
            txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
            ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
            ibtnPrintApplication.Attributes.Add("disabled", "disabled");
            InitialDefault();
        }

    }
    #endregion Existing Application

    void DisabledAllControls()
    {
        DisabledControls();
        ddlClientType.Attributes.Add("disabled", "disabled");
        txtApplicationDate.Attributes.Add("disabled", "disabled");
        txtBankStaffName.Attributes.Add("disabled", "disabled");
        ddlClientTypeRelation.Attributes.Add("disabled", "disabled");
        /*customer information*/
        ddlIDType.Attributes.Add("disabled", "disabled");
        txtIDNumber.Attributes.Add("disabled", "disabled");
        txtSurnameKh.Attributes.Add("disabled", "disabled");
        txtFirstNameKh.Attributes.Add("disabled", "disabled");
        txtSurnameEng.Attributes.Add("disabled", "disabled");
        txtFirstNameEng.Attributes.Add("disabled", "disabled");
        txtNationality.Attributes.Add("disabled", "disabled");
        ddlGender.Attributes.Add("disabled", "disabled");
        txtDateOfBirth.Attributes.Add("disabled", "disabled");
        ddlMaritalStatus.Attributes.Add("disabled", "disabled");
        //txtOccupation.Attributes.Add("disabled", "disabled");
        ddlOccupation.Attributes.Add("disabled", "disabled");
        txtPhoneNumber.Attributes.Add("disabled", "disabled");
        txtEmail.Attributes.Add("disabled", "disabled");
        txtStreetEn.Attributes.Add("disabled", "disabled");
        txtHouseNoEn.Attributes.Add("disabled", "disabled");
        txtVillageEn.Attributes.Add("disabled", "disabled");
        txtCommuneEn.Attributes.Add("disabled", "disabled");
        txtDistrictEn.Attributes.Add("disabled", "disabled");
        txtProvinceEn.Attributes.Add("disabled", "disabled");
        txtHouseNoKh.Attributes.Add("disabled", "disabled");
        txtStreetKh.Attributes.Add("disabled", "disabled");

        ddlVillageKh.Attributes.Add("disabled", "disabled");
        ddlCommuneKh.Attributes.Add("disabled", "disabled");
        ddlDistrictKh.Attributes.Add("disabled", "disabled");

        //txtProvinceKh.Attributes.Add("disabled", "disabled");
        ddlProvinceKh.Attributes.Add("disabled", "disabled");
        /*insurance product*/
        ddlPackage.Attributes.Add("disabled", "disabled");
        /*beneficiary*/
        txtBenAddress.Attributes.Add("disabled", "disabled");
        txtAge.Attributes.Add("disabled", "disabled");
        ddlRelation.Attributes.Add("disabled", "disabled");
        txtPercentageOfShare.Attributes.Add("disabled", "disabled");
        txtFullName.Attributes.Add("disabled", "disabled");

        ddlAnswer.Attributes.Add("disabled", "disabled");
        txtAnswerRemarks.Attributes.Add("disabled", "disabled");

    }

    void DisabledControls()
    {
        //Application information
        txtApplicationNumber.Attributes.Add("disabled", "disabled");
        txtBranchName.Attributes.Add("disabled", "disabled");
        txtBranchCode.Attributes.Add("disabled", "disabled");
        txtSaleAgentID.Attributes.Add("disabled", "disabled");
        txtSaleAgentName.Attributes.Add("disabled", "disabled");
        txtPolicyNumber.Attributes.Add("disabled", "disabled");
        txtPolicyStatus.Attributes.Add("disabled", "disabled");
        txtReferrerId.Attributes.Add("disabled", "disabled");
        txtReferrerName.Attributes.Add("disabled", "disabled");
        //customer information

        txtCustomerAge.Attributes.Add("disabled", "disabled");

        //Insurance product and detail
        txtProductName.Attributes.Add("disabled", "disabled");
        txtTermOfCover.Attributes.Add("disabled", "disabled");
        txtPremiumPaymentPeriod.Attributes.Add("disabled", "disabled");
        //txtSumAssure.Attributes.Add("disabled", "disabled");
        // ddlPaymentMode.Attributes.Add("disabled", "disabled");
        txtPremium.Attributes.Add("disabled", "disabled");
        txtAnnualPremium.Attributes.Add("disabled", "disabled");
        txtBasicDiscount.Attributes.Add("disabled", "disabled");
        txtBasicAfterDiscount.Attributes.Add("disabled", "disabled");
        txtRiderProduct.Attributes.Add("disabled", "disabled");
        txtRiderPremium.Attributes.Add("disabled", "disabled");
        txtRiderAnnualPremium.Attributes.Add("disabled", "disabled");
        //txtRiderSumAssure.Attributes.Add("disabled", "disabled");
        txtRiderDiscount.Attributes.Add("disabled", "disabled");
        txtRiderAfterDiscount.Attributes.Add("disabled", "disabled");


        txtTotalPremium.Attributes.Add("disabled", "disabled");
        txtTotalDiscountAmount.Attributes.Add("disabled", "disabled");
        txtTotalPremiumAfterDiscount.Attributes.Add("disabled", "disabled");

        lblAnnualPremium.Attributes.CssStyle.Add("display", "none");
        txtAnnualPremium.Attributes.CssStyle.Add("display", "none");

        lblRiderAnnualPremium.Attributes.CssStyle.Add("display", "none");
        txtRiderAnnualPremium.Attributes.CssStyle.Add("display", "none");

        //Beneficiary
        //txtFullName.Attributes.Add("disabled", "disabled");
        //txtAge.Attributes.Add("disabled", "disabled");
        //txtRelation.Attributes.Add("disabled", "disabled");
        txtPercentageOfShare.Attributes.Add("disabled", "disabled");

        //btnIssue.Attributes.Add("disabled", "disabled");
        txtIssueDate.Attributes.Add("disabled", "disabled");
        //txtUserPremium.Attributes.Add("disabled", "disabled");
        //txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");


    }
    void InitialDefault()
    {

        ShowClientTypeRemarks(false);
        txtReletedCertificate.Attributes.Add("disabled", "disabled");
        txtApplicationDate.Attributes.Add("disabled", "disabled");

        txtProvinceEn.Attributes.Add("disabled", "disabled");
        txtDistrictEn.Attributes.Add("disabled", "disabled");
        txtCommuneEn.Attributes.Add("disabled", "disabled");
        txtVillageEn.Attributes.Add("disabled", "disabled");
        //Insurance product and detail
        //ddlPaymentMode.SelectedIndex = 1;//fixed annully

        txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtEffectiveDate.Attributes.Add("disabled", "disabled");
        Helper.SelectedDropDownListIndex("TEXT", ddlPaymentMode, "Annually");

        //Beneficiary
        // txtFullName.Text = "មរតកសាស្រ្ត";
        txtAge.Text = "-";
        //txtRelation.Text = "-";
        txtPercentageOfShare.Attributes.Add("disabled", "disabled");
        txtPercentageOfShare.Text = "100";
    }
    void CalculatePremium()
    {
        if (ProductConfig != null)
        {
            string productID = ProductConfig.Product_ID;  // hdfProductID.Value;
            string productRiderID = string.IsNullOrWhiteSpace(ProductConfig.RiderProductID) == true ? "" : ProductConfig.RiderProductID;  // hdfRiderProductID.Value;
            int gender = Convert.ToInt32(ddlGender.SelectedValue);
            int age = Convert.ToInt32(txtCustomerAge.Text == "" ? "0" : txtCustomerAge.Text.Trim());
            double sumAssure = Convert.ToDouble(ddlBasicSa.Items.Count > 0 ? ddlBasicSa.SelectedValue : "0");
            double sumAssureRider = Convert.ToDouble(ddlRiderSa.Items.Count > 0 ? ddlRiderSa.SelectedValue : "0");
            bool valid = false;
            string message = "";
            int payMode = Convert.ToInt32(ddlPaymentMode.Items.Count > 0 ? ddlPaymentMode.SelectedValue : "-1");
            //clear text box
            txtPremium.Text = "";
            txtAnnualPremium.Text = "";
            txtBasicDiscount.Text = "";
            txtBasicAfterDiscount.Text = "";
            txtRiderPremium.Text = "";
            txtRiderDiscount.Text = "";
            txtRiderAnnualPremium.Text = "";
            txtRiderAfterDiscount.Text = "";


            ValidateCalculatePremium();

            valid = IsValide;// (bool)ViewState["IS_VALID"];
            message = Message;// ViewState["MESSAGE"] + "";
            if (valid && ddlPackage.SelectedIndex > 0 && gender > -1 && age > 0 && sumAssure > 0 && payMode > -1)//(valid && ddlPackage.SelectedIndex > 0 && gender > -1 && age > 0 && sumAssure > 0 && sumAssureRider > 0)
            {
                double basic_premium = 0;
                double rider_premium = 0;
                double basicDiscount = 0;
                double riderDiscount = 0;
                double totalDiscount = 0;
                double totalPremium = 0;
                double totalPremiumAfterDiscount = 0;
                double basicAfterDiscount = 0;
                double riderAfterDiscount = 0;
                double[] basic = Calculation.GetMicroProducPremium(productID, gender, age, sumAssure, payMode);
                DateTime appDate = Helper.FormatDateTime(txtApplicationDate.Text.Trim());

                txtPremium.Text = basic[1].ToString();
                txtAnnualPremium.Text = basic[0].ToString();

                double[] rider = Calculation.GetMicroProductRiderPremium(productRiderID, gender, age, sumAssureRider, payMode);


                txtRiderPremium.Text = rider[1].ToString();
                txtRiderAnnualPremium.Text = rider[0].ToString();
                #region//check discount

                bl_micro_product_discount_config proConf = new bl_micro_product_discount_config();
                proConf = da_micro_product_config.DiscountConfig.GetProductDiscount(productID, productRiderID, sumAssure, sumAssureRider, ddlClientType.SelectedValue);
                /*Check override discount*/
                bl_override_discount_config ovDist = new bl_override_discount_config();
                /*get existing customer*/
                bl_micro_customer1 exCust = da_micro_customer.GetExistCustomer(txtFirstNameEng.Text.Trim(), txtSurnameEng.Text.Trim(), Helper.FormatDateTime(txtDateOfBirth.Text.Trim()));
                ovDist = da_micro_product_config.OverrideDiscountConfig.GetOverriderDiscount(string.IsNullOrWhiteSpace(exCust.CUSTOMER_NUMBER) == true ? "" : exCust.ID, MYPARA.ChannelItemId, productID);
                if (!string.IsNullOrWhiteSpace(ovDist.ProductId))//has override discount
                {
                    if (ovDist.Status && appDate >= ovDist.EffectiveDate && appDate <= ovDist.ExpiryDate)
                    {
                        basicDiscount = ovDist.BasicDiscountAmount;
                        riderDiscount = ovDist.RiderDiscountAmount;
                    }
                    else
                    {
                        if (proConf.ProductID != null)
                        {
                            if (proConf.Status && appDate >= proConf.EffectiveDate && appDate <= proConf.ExpiryDate)
                            {
                                basicDiscount = proConf.BasicDiscountAmount;
                                riderDiscount = proConf.RiderDiscountAmount;
                            }
                        }
                    }
                }
                else // no override discount
                {

                    //check expiry date of discount
                    // DateTime appDate = Helper.FormatDateTime(txtApplicationDate.Text.Trim());
                    if (proConf.ProductID != null)
                    {
                        if (proConf.Status && appDate >= proConf.EffectiveDate && appDate <= proConf.ExpiryDate)
                        {
                            basicDiscount = proConf.BasicDiscountAmount;
                            riderDiscount = proConf.RiderDiscountAmount;
                        }
                    }
                }

                #endregion


                basic_premium = txtPremium.Text.Trim() != "" ? Convert.ToDouble(txtPremium.Text.Trim()) : 0;
                rider_premium = txtRiderPremium.Text.Trim() != "" ? Convert.ToDouble(txtRiderPremium.Text.Trim()) : 0;
                totalPremium = basic_premium + rider_premium;

                totalPremium = basic_premium + rider_premium;
                totalDiscount = basicDiscount + riderDiscount;

                totalDiscount = basicDiscount + riderDiscount;
                basicAfterDiscount = basic_premium - basicDiscount;
                riderAfterDiscount = rider_premium - riderDiscount;

                totalPremiumAfterDiscount = basicAfterDiscount + riderAfterDiscount;

                txtBasicDiscount.Text = basicDiscount + "";
                txtBasicAfterDiscount.Text = basicAfterDiscount + "";

                txtRiderDiscount.Text = riderDiscount + "";
                txtRiderAfterDiscount.Text = riderAfterDiscount + "";

                txtTotalPremium.Text = totalPremium + "";
                txtTotalDiscountAmount.Text = totalDiscount + "";
                txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + "";
            }
            else
            {
                if (message != "")
                {
                    // Alert(my_session.MESSAGE);

                    Helper.Alert(true, message, lblError);
                }
            }

        }
    }

    protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
    {

        bool valid = false;
        string message = "";
        hdfProductID.Value = ddlPackage.SelectedValue;
        if (ddlPackage.SelectedIndex > 0)
        {

            bl_micro_product_config proConf = new bl_micro_product_config();
            proConf = da_micro_product_config.ProductConfig.GetProductMicroProduct(ddlPackage.SelectedValue);
            if (proConf != null)
            {
                ProductConfig = proConf;
                // txtSumAssure.Text = ddlPackage.SelectedValue;
                // txtSumAssure.Text = ProductConfig.BasicSumAssured;
                txtProductName.Text = proConf.En_Title;

                txtTermOfCover.Text = "1";
                txtPremiumPaymentPeriod.Text = "1";

                /*bind Basic sum assured*/

                BindBaicSA(proConf.BasicSumAssuredRange);

              

                /*bind rider product*/

                if (!string.IsNullOrWhiteSpace(proConf.RiderProductID)) // has rider
                {
                    hdfRiderProductID.Value = proConf.RiderProductID;
                    txtRiderProduct.Text = proConf.RiderProductID;
                    BindRiderSA(proConf.RiderSumAssuredRange);
                }
                else // no rider configured
                {
                    txtRiderProduct.Text = "";
                    txtRiderPremium.Text = "";
                    txtRiderDiscount.Text = "";
                    txtRiderAfterDiscount.Text = "";
                    ddlRiderSa.Items.Clear();
                    ddlRiderSa.Enabled = false;
                }

                /*bind Payment mode*/
                BindPayMode(proConf.PayMode);
                if (proConf.PayMode.Count() == 1)
                {

                    ddlPaymentMode_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                //alert error 
                Helper.Alert(true, "Get product information error!", lblError);
            }


            ValidateCalculateAge();
            valid = IsValide;// (bool)ViewState["IS_VALID"];
            message = Message;// ViewState["MESSAGE"] + "";

            if (valid)
            {
                Helper.SelectedDropDownListIndex("VALUE", ddlPaymentMode, "1");

                CalculatePremium();

                //Check upgrate plan for repayment
                //if client upgrate plan then system count as new policy
                #region V1
                //if (MYPARA.ApplicationType == "Repayment" && MYPARA.OldPolicyStatus=="EXP")
                //{
                //    if (ddlPackage.SelectedValue != hdfPackage.Value && ddlPackage.SelectedValue=="5000")
                //    {
                //        lblApplicationType.Text = "New";
                //        // txtReletedCertificate.Text = "";
                //        this.IsUpgratePackage = true;
                //        this.LastApplicationType = "New";
                //    }
                //    else
                //    {
                //        this.IsUpgratePackage = false;
                //        lblApplicationType.Text = MYPARA.ApplicationType;
                //        this.LastApplicationType = MYPARA.ApplicationType;
                //        //txtReletedCertificate.Text = MYPARA.OldPolicyNumber + "@" + MYPARA.OldPolicyStatus;
                //    }
                //}
                #endregion V1
                #region V2
                /*
                 Remarks repayment to New without check policy status
                 */
                if (MYPARA.ApplicationType == "Repayment")
                {
                    //if (ddlPackage.SelectedValue != hdfPackage.Value && ddlPackage.SelectedValue == "5000")
                    if (ddlPackage.SelectedValue != hdfPackage.Value && Convert.ToDouble(ddlPackage.SelectedValue) > Convert.ToDouble(hdfPackage.Value))
                    {
                        lblApplicationType.Text = "New";
                        // txtReletedCertificate.Text = "";
                        this.IsUpgratePackage = true;
                        this.LastApplicationType = "New";
                    }
                    else
                    {
                        this.IsUpgratePackage = false;
                        lblApplicationType.Text = MYPARA.ApplicationType;
                        this.LastApplicationType = MYPARA.ApplicationType;
                        //txtReletedCertificate.Text = MYPARA.OldPolicyNumber + "@" + MYPARA.OldPolicyStatus;
                    }
                }
                #endregion V2
            }
            else
            {
                if (message != "")
                {
                    //Alert(my_session.MESSAGE);
                    Helper.Alert(true, message, lblError);
                }
            }
        }
        else
        {
            ProductConfig = null;
            ClearInsuranceProduct();
        }
    }

    public int CalculateAge(string DATE_OF_BIRTH, string COMPARE_DATE)
    {
        int age = 0;
        age = Calculation.Culculate_Customer_Age(DATE_OF_BIRTH, Helper.FormatDateTime(COMPARE_DATE));

        return age;
    }

    protected void txtApplicationDate_TextChanged(object sender, EventArgs e)
    {

        bool valid = false;

        string message = "";

        ValidateCalculateAge();
        valid = IsValide;// (bool)ViewState["IS_VALID"];
        message = Message;// ViewState["MESSAGE"] + "";
        if (!valid)
        {
            txtCustomerAge.Text = "";
            ClearInsuranceProduct();
            // Alert(my_session.MESSAGE);
            Helper.Alert(true, message, lblError);
        }
        else
        {
            CalculatePremium();

        }
    }

    protected void txtDateOfBirth_TextChanged(object sender, EventArgs e)
    {
        txtApplicationDate_TextChanged(null, null);
    }

    void ClearBeneficiary()
    {
        txtBenAddress.Text = "";
        txtFullName.Text = "";
        txtAge.Text = "";
        // txtRelation.Text = "";
        ddlRelation.SelectedIndex = 0;
        txtPercentageOfShare.Text = "";

    }
    void ClearInsuranceProduct()
    {
        ddlPackage.SelectedIndex = 0;
        txtProductName.Text = "";
        txtTermOfCover.Text = "";
        txtPremiumPaymentPeriod.Text = "";
        ddlBasicSa.SelectedIndex = 0;
        txtPremium.Text = "";
        txtAnnualPremium.Text = "";
        txtBasicDiscount.Text = "";
        txtBasicAfterDiscount.Text = "";
        ddlPaymentMode.SelectedIndex = ddlPaymentMode.Items.Count>0 ?  0: -1;

        //Rider
        txtRiderProduct.Text = "";
        txtRiderPremium.Text = "";
        txtRiderAnnualPremium.Text = "";
        ddlRiderSa.SelectedIndex = ddlRiderSa.Items.Count > 0 ? 0 : -1;
        txtRiderPremium.Text = "";
        txtRiderDiscount.Text = "";
        txtRiderAfterDiscount.Text = "";
        txtTotalPremium.Text = "";
        txtTotalDiscountAmount.Text = "";
        txtTotalPremiumAfterDiscount.Text = "";
    }

    void ValidateForm()
    {

        string message = "";
        bool isValid = true;

        #region Application Information

        if (ddlClientType.SelectedValue == Helper.ClientTYpe.CLIENT_FAMILY.ToString())
        { 
            if(txtBankStaffName.Text.Trim()=="")
            {

                isValid = false;
                message = "Client name is required.";
            }
            else if (ddlClientTypeRelation.SelectedValue == "")
            {

                isValid = false;
                message = "Relation is required.";
            }
        }
        else if (ddlClientType.SelectedValue == Helper.ClientTYpe.BANK_STAFF_FAMILY.ToString())
        {
            if (txtBankStaffName.Text.Trim() == "")
            {

                isValid = false;
                message = "Bank staff name is required.";
            }
            else if (ddlClientTypeRelation.SelectedValue == "")
            {

                isValid = false;
                message = "Relation is required.";
            }
        }

        if (txtBranchName.Text.Trim() == "")
        {
            isValid = false;
            message = "Branch Name is required.";
        }
        else if (txtBranchCode.Text.Trim() == "")
        {
            isValid = false;
            message = "Branch Code is required.";
        }
        else if (txtSaleAgentID.Text.Trim() == "")
        {
            isValid = false;
            message = "Sale Agent ID is required.";
        }
        else if (txtSaleAgentName.Text.Trim() == "")
        {
            isValid = false;
            message = "Sale Agent Name is required.";
        }
        else if (txtApplicationDate.Text.Trim() == "")
        {
            isValid = false;
            message = "Application Date is required.";
        }
        //else if (txtReferrerId.Text.Trim() == "")
        //{
        //    isValid = false;
        //    message = "Referral Id is required.";

        //}
        if (ProductConfig.BusinessType == bl_micro_product_config.BusinussTypeOption.BANCA_REFERRAL.ToString())
        {
            if (ProductConfig.IsValidateReferralId)
            {
                if (txtReferrerId.Text.Trim() == "")
                {
                    isValid = false;
                    message = "Referral Id is required.";

                }
                else
                {
                    List<bl_referral> refList = da_referral.GetActiveReferral(hdfChannelItemID.Value, hdfChannelLocationID.Value, txtReferrerId.Text.Trim(), userName);
                    if (refList.Count == 0)
                    {
                        isValid = false;
                        message = "Referral Id [" + txtReferrerId.Text + "] is not in camlife white list.";
                    }
                }
            }
        }
        #endregion
        #region Customer information
        if (ddlIDType.SelectedIndex == 0)
        {
            isValid = false;
            message = "ID Type is required.";
        }
        else if (txtIDNumber.Text.Trim() == "")
        {
            isValid = false;
            message = "ID Number is required.";
        }
        else if (txtSurnameEng.Text.Trim() == "")
        {
            isValid = false;
            message = "Surname In English is required.";
        }
        else if (txtFirstNameEng.Text.Trim() == "")
        {
            isValid = false;
            message = "First Name In English is required.";
        }
        else if (txtSurnameKh.Text.Trim() == "")
        {
            isValid = false;
            message = "Surname In Khmer is required.";
        }
        else if (txtFirstNameKh.Text.Trim() == "")
        {
            isValid = false;
            message = "First Name In Khmer is required.";
        }
        else if (txtNationality.Text.Trim() == "")
        {
            isValid = false;
            message = "Nationality is required.";
        }
        else if (ddlGender.SelectedIndex == 0)
        {
            isValid = false;
            message = "Gender is required.";
        }
        else if (txtDateOfBirth.Text.Trim() == "")
        {
            isValid = false;
            message = "Date of Birth is required.";
        }

        else if (txtCustomerAge.Text.Trim() == "")
        {
            isValid = false;
            message = "Age is required.";
        }

        else if (ddlMaritalStatus.SelectedIndex == 0)
        {
            isValid = false;
            message = "Marital Status is required.";
        }
        else if (ddlOccupation.SelectedIndex == 0)// (txtOccupation.Text.Trim() == "")
        {
            isValid = false;
            message = "Occupation is required.";
        }

        else if (ddlProvinceKh.SelectedIndex == 0)//else if (txtProvinceKh.Text.Trim() == "")
        {
            isValid = false;
            message = "Provice In Khmer is required.";
        }
        else if (txtPhoneNumber.Text.Trim() == "")
        {
            isValid = false;
            message = "Phone Number is required.";
        }


        else if (txtApplicationDate.Text.Trim() != "" && !Helper.IsDate(txtApplicationDate.Text.Trim()))
        {

            isValid = false;
            message = "Application is invalid format.";

        }
        else if (txtDateOfBirth.Text.Trim() != "" && !Helper.IsDate(txtDateOfBirth.Text.Trim()))
        {

            isValid = false;
            message = "Date of Birth is invalid format.";

        }
        else if (txtCustomerAge.Text.Trim() != "" && !Helper.IsNumber(txtCustomerAge.Text.Trim()))
        {

            isValid = false;
            message = "Date of Birth is required.";

        }

        if (ddlIDType.SelectedValue == "0") //id card
        {
            if (txtIDNumber.Text.Trim().Length < 9)
            {
                isValid = false;
                message = "ID number must be 9 digits or more.";
            }
        }
        #endregion

        #region Insurance product
        if (ddlPackage.SelectedIndex == 0)
        {
            isValid = false;
            message = "Package is required.";
        }
        else if (txtProductName.Text.Trim() == "")
        {
            isValid = false;
            message = "Product Name is required.";
        }
        else if (txtTermOfCover.Text.Trim() == "")
        {
            isValid = false;
            message = "Term of Cover is required.";
        }
        else if (txtPremiumPaymentPeriod.Text.Trim() == "")
        {
            isValid = false;
            message = "Payment Period is required.";
        }
        else if (ddlBasicSa.SelectedIndex == 0)
        {
            isValid = false;
            message = "Sum Assure is required.";
        }
        else if (ddlPaymentMode.SelectedValue.Trim() == "")
        {
            isValid = false;
            message = "Payment Mode is required.";
        }
        else if (txtPremium.Text.Trim() == "")
        {
            isValid = false;
            message = "Premium is required.";
        }
        else if (txtTotalPremium.Text.Trim() == "")
        {
            isValid = false;
            message = "Total Amount is required.";
        }

        #endregion
        #region Rider
        

         if (!string.IsNullOrWhiteSpace(ProductConfig.RiderProductID))
         {
             if (ProductConfig.IsRequiredRider)
             {
                 if (txtRiderProduct.Text.Trim() == "")
                 {
                     isValid = false;
                     message = "Rider Product is required.";
                 }
                 else if (ddlRiderSa.SelectedIndex == 0)
                 {
                     isValid = false;
                     message = "Rider Sum Assure is required.";
                 }
                 else if (txtRiderPremium.Text.Trim() == "")
                 {
                     isValid = false;
                     message = "Rider Premium is required.";
                 }
             }
         }
        #endregion
         if (txtFullName.Text.Trim() == "")
        {
            isValid = false;
            message = "Beneficiary Full Name is required.";
        }
        else if (txtBenAddress.Text.Trim() == "")
        {
            isValid = false;
            message = "Beneficiary Address is required.";
        }
        else if (ddlRelation.SelectedIndex == 0)
        {
            isValid = false;
            message = "Relation is required.";
        }
        else if (ddlAnswer.SelectedIndex == 0)
        {
            isValid = false;
            message = "Please answer a question.";
        }
        else if (ddlAnswer.SelectedValue == "1" && txtAnswerRemarks.Text.Trim() == "")
        {
            isValid = false;
            message = "Please give detail of answer.";
        }

        //else if (ddlAnswer.SelectedValue == "1" && txtAnswerRemarks.Text.Trim() == "")
        //{
        //    isValid = false;
        //    message = "Pre-existing is detected, system is not allowed to save.";
        //}


        // ViewState["IS_VALID"] = isValid;
        // ViewState["MESSAGE"] = message;
        IsValide = isValid;
        Message = message;

    }
    void ValidateCalculateAge()
    {
        bool isValid = true;
        string message = "";

        if (txtApplicationDate.Text.Trim() == "")
        {
            isValid = false;
            message = "Application Date is required.";
        }
        else if (txtDateOfBirth.Text.Trim() == "")
        {
            isValid = false;
            message = "Date of Birth is required.";
        }
        else if (txtDateOfBirth.Text.Trim() != "" && !Helper.IsDate(txtDateOfBirth.Text))
        {
            isValid = false;
            message = "Date of Birth is invalid format.";
        }
        else if (txtApplicationDate.Text.Trim() != "" && !Helper.IsDate(txtApplicationDate.Text))
        {
            isValid = false;
            message = "Application Date is invalid format.";
        }
        else
        {
            int age = 0;
            age = CalculateAge(txtDateOfBirth.Text, txtApplicationDate.Text);
            txtCustomerAge.Text = age + "";
            if (hdfProductID.Value != "")
            {
                //bl_product product = new bl_product();
                //product = (bl_product)Session["PRODUCT"];

                if (ProductConfig.Product_ID != null)
                {
                    if (age >= ProductConfig.Age_Min && age <= ProductConfig.Age_Max)
                    {

                        ddlPackage.Attributes.Remove("disabled");
                    }
                    else
                    {
                        ddlPackage.Attributes.Add("disabled", "disabled");
                        txtCustomerAge.Text = "";
                        isValid = false;
                        message = "Age [" + age + "] is not allow.";
                    }
                }
            }
        }

        //  ViewState["IS_VALID"] = isValid;
        // ViewState["MESSAGE"] = message;
        IsValide = isValid;
        Message = message;

    }
    void ValidateCalculatePremium()
    {
        bool isValid = true;
        string message = "";

        if (txtCustomerAge.Text.Trim() == "")
        {
            isValid = false;
            message = "Customer Age is required.";
        }
        else if (txtCustomerAge.Text.Trim() != "" && (!Helper.IsNumber(txtCustomerAge.Text.Trim())))
        {
            isValid = false;
            message = "Customer Age is allowed only number.";
        }
        //ViewState["IS_VALID"] = isValid;
        //ViewState["MESSAGE"] = message;
        IsValide = isValid;
        Message = message;

    }
    void ValidateIssuePolicy()
    {
        ValidateForm();
        bool isValid = true;
        string message = "";
        isValid = IsValide;// (bool)ViewState["IS_VALID"];
        if (isValid)
        {
            if (hdfProductID.Value == "")
            {
                isValid = false;
                message = "Product id is required.";
            }
            else if (txtIssueDate.Text.Trim() == "")
            {
                isValid = false;
                message = "Issue Date is required.";
            }
            else if (txtIssueDate.Text.Trim() != "" && !Helper.IsDate(txtIssueDate.Text.Trim()))
            {
                isValid = false;
                message = "Issue Date is invalid format.";
            }
            else if (txtUserPremium.Text.Trim() == "")
            {
                isValid = false;
                message = "Input Collected Premium is required.";
            }
            else if (txtTotalPremiumAfterDiscount.Text.Trim() != "" && !Helper.IsAmount(txtTotalPremiumAfterDiscount.Text.Trim()))
            {
                isValid = false;
                message = "Total Premium after discount is allowed only number.";
            }
            else if (txtUserPremium.Text.Trim() != "" && !Helper.IsAmount(txtUserPremium.Text.Trim()))
            {
                isValid = false;
                message = "Input Collected Premium is allowed only number.";
            }

            else if (Convert.ToDouble(txtUserPremium.Text.Trim()) != Convert.ToDouble(txtTotalPremiumAfterDiscount.Text.Trim()))
            {
                isValid = false;
                message = "Collected Premium must be equal to Total Amount After Discount.";

            }

            else if (txtPaymentReferenceNo.Text.Trim() == "")
            {
                isValid = false;
                message = "Payment Reference No. is required.";
            }
            //  ViewState["IS_VALID"] = isValid;
            // ViewState["MESSAGE"] = message;
            IsValide = isValid;
            Message = message;
        }

    }
    protected void txtStreetEn_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        txtVillageEn.Focus();
    }
    protected void txtHouseNoEn_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        txtStreetEn.Focus();
    }
    protected void txtVillageEn_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        txtCommuneEn.Focus();
    }
    protected void txtCommuneEn_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        txtDistrictEn.Focus();
    }
    protected void txtDistrictEn_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        txtProvinceEn.Focus();
    }
    protected void txtProvinceEn_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);

    }
    protected void txtStreetKh_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);

    }
    protected void txtHouseNoKh_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        txtStreetKh.Focus();
    }
    protected void txtVillageKh_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);

    }
    protected void txtCommuneKh_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);

    }
    protected void txtDistrictKh_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);
        //txtProvinceKh.Focus();
        ddlProvinceKh.Focus();
    }
    protected void txtProvinceKh_TextChanged(object sender, EventArgs e)
    {
        txtCurrentAddress_TextChanged(null, null);

    }

    protected void txtCurrentAddress_TextChanged(object sender, EventArgs e)
    {
        if (ddlProvinceKh.SelectedValue.ToUpper() == "12/PHNOM PENH" || ddlProvinceKh.SelectedValue.ToUpper() == "PHNOM PENH")
        {
            txtBenAddress.Text = (txtHouseNoKh.Text.Trim() == "" ? "" : "ផ្ទះលេខ" + txtHouseNoKh.Text) + " " + (txtStreetKh.Text.Trim() == "" ? "" : "ផ្លូវលេខ" + txtStreetKh.Text.Trim()) + " " +
                (ddlVillageKh.SelectedValue.Trim() == "" ? "" : "ភូមិ" + ddlVillageKh.SelectedItem.Text.Trim()) + " " + (ddlCommuneKh.SelectedValue.Trim() == "" ? "" : "សង្កាត់" + ddlCommuneKh.SelectedItem.Text.Trim()) + " " +
                (ddlDistrictKh.SelectedValue.Trim() == "" ? "" : "ខណ្ឌ" + ddlDistrictKh.SelectedItem.Text.Trim()) + " " + (ddlProvinceKh.SelectedValue.Trim() == "" ? "" : "ក្រុង" + ddlProvinceKh.SelectedItem.Text);
        }
        else if (ddlProvinceKh.SelectedValue != "")
        {
            txtBenAddress.Text = (txtHouseNoKh.Text.Trim() == "" ? "" : "ផ្ទះលេខ" + txtHouseNoKh.Text) + " " + (txtStreetKh.Text.Trim() == "" ? "" : "ផ្លូវលេខ" + txtStreetKh.Text.Trim()) + " " +
        (ddlVillageKh.SelectedValue.Trim() == "" ? "" : "ភូមិ" + ddlVillageKh.SelectedItem.Text.Trim()) + " " + (ddlCommuneKh.SelectedValue.Trim() == "" ? "" : "ឃុំ" + ddlCommuneKh.SelectedItem.Text.Trim()) + " " +
        (ddlDistrictKh.SelectedValue.Trim() == "" ? "" : "ស្រុក" + ddlDistrictKh.SelectedItem.Text.Trim()) + " " + (ddlProvinceKh.SelectedValue.Trim() == "" ? "" : "ខេត្ត" + ddlProvinceKh.SelectedItem.Text);

        }
    }

    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string logDes = "";

            ValidateForm();
            string message = "";
            bool isValid = false;
            message = Message;// ViewState["MESSAGE"] + "";
            isValid = IsValide;// (bool)ViewState["IS_VALID"];
            if (isValid)
            {
                string app_number = "";
                #region Save Application
                bool is_save_app = false;
                bool is_save_customer = false;
                bool is_save_insurance = false;
                bool is_save_insurance_rider = false;
                bool is_save_beneficiary = false;
                bool is_save_question = false;

                string provinceCode = "";
                string districtCode = "";
                string communeCode = "";
                string villageCode = "";

                string channel_id = "0152DF80-BA95-46A9-BB7A-E71966A34089";//coporate


                string agentName = ViewState["AGENT_NAME"] + "";
                string leadID = LeadId;


                provinceCode = GetProvinceCode();
                districtCode = GetDisctrictCode();
                communeCode = GetCommuneCode();
                villageCode = GetVillageCode();

                #region customer information
                DateTime tran_date = DateTime.Now;
                bl_micro_application_customer app_cus = new bl_micro_application_customer();

                app_cus.ID_TYPE = ddlIDType.SelectedValue;
                app_cus.ID_NUMBER = txtIDNumber.Text.Trim();
                app_cus.FIRST_NAME_IN_ENGLISH = txtFirstNameEng.Text.Trim();
                app_cus.LAST_NAME_IN_ENGLISH = txtSurnameEng.Text.Trim();
                app_cus.FIRST_NAME_IN_KHMER = txtFirstNameKh.Text.Trim();
                app_cus.LAST_NAME_IN_KHMER = txtSurnameKh.Text.Trim();
                app_cus.GENDER = ddlGender.SelectedValue;
                app_cus.DATE_OF_BIRTH = Helper.FormatDateTime(txtDateOfBirth.Text.Trim());
                app_cus.NATIONALITY = txtNationality.Text.Trim();
                app_cus.MARITAL_STATUS = ddlMaritalStatus.SelectedValue;
                app_cus.OCCUPATION = ddlOccupation.SelectedValue;// txtOccupation.Text.Trim();

                app_cus.HOUSE_NO_KH = txtHouseNoKh.Text.Trim();
                app_cus.STREET_NO_KH = txtStreetKh.Text.Trim();
                app_cus.VILLAGE_KH = villageCode;
                app_cus.COMMUNE_KH = communeCode;
                app_cus.DISTRICT_KH = districtCode;
                //app_cus.PROVINCE_KH = txtProvinceKh.Text.Trim();
                app_cus.PROVINCE_KH = provinceCode;
                app_cus.HOUSE_NO_EN = txtHouseNoEn.Text.Trim();
                app_cus.STREET_NO_EN = txtStreetEn.Text.Trim();
                app_cus.VILLAGE_EN = villageCode;
                app_cus.COMMUNE_EN = communeCode;
                app_cus.DISTRICT_EN = districtCode;
                app_cus.PROVINCE_EN = provinceCode;
                app_cus.PHONE_NUMBER1 = txtPhoneNumber.Text.Trim();
                app_cus.EMAIL1 = txtEmail.Text.Trim();
                app_cus.CREATED_BY = userName;
                app_cus.CREATED_ON = tran_date;
                app_cus.STATUS = 1;


                #endregion

                #region application information

                bl_micro_application app = new bl_micro_application();

                app.APPLICATION_DATE = Helper.FormatDateTime(txtApplicationDate.Text);
                app.CHANNEL_ID = channel_id;
                app.CHANNEL_ITEM_ID = hdfChannelItemID.Value;
                app.CHANNEL_LOCATION_ID = hdfChannelLocationID.Value;
                app.SALE_AGENT_ID = txtSaleAgentID.Text;
                //app.APPLICATION_CUSTOMER_ID= "cusotmer id";
                app.CREATED_BY = userName;
                app.CREATED_ON = DateTime.Now;
                app.REMARKS = "";
                app.REFERRER_ID = txtReferrerId.Text.Trim();
                app.RENEW_FROM_POLICY = MYPARA.OldPolicyNumber == null ? "" : MYPARA.OldPolicyNumber;
                app.CLIENT_TYPE = ddlClientType.SelectedValue;
                app.LoanNumber = txtLoanNumber.Text.Trim();
                if (ddlClientType.SelectedValue == Helper.ClientTYpe.CLIENT_FAMILY.ToString() || ddlClientType.SelectedValue == Helper.ClientTYpe.BANK_STAFF_FAMILY.ToString())
                {
                    app.CLIENT_TYPE_REMARKS = txtBankStaffName.Text.Trim();
                    app.CLIENT_TYPE_RELATION = ddlClientTypeRelation.SelectedValue;
                }
                else
                {
                    app.CLIENT_TYPE_REMARKS = "";
                    app.CLIENT_TYPE_RELATION = "";
                }
                #endregion

                #region Insurance product
                bl_micro_application_insurance app_insurance = new bl_micro_application_insurance();
                // app_insurance.APPLICATION_NUMBER = app.APPLICATION_NUMBER;
                app_insurance.PRODUCT_ID = hdfProductID.Value;
                app_insurance.TERME_OF_COVER = Convert.ToInt32(txtTermOfCover.Text.Trim());
                app_insurance.PAYMENT_PERIOD = Convert.ToInt32(txtPremiumPaymentPeriod.Text.Trim());
                app_insurance.SUM_ASSURE = Convert.ToDouble(ddlBasicSa.SelectedValue);
                app_insurance.PAY_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);

                app_insurance.PREMIUM = Convert.ToDouble(txtPremium.Text.Trim());
                app_insurance.ANNUAL_PREMIUM = Convert.ToDouble(txtAnnualPremium.Text.Trim());
                app_insurance.USER_PREMIUM = 0;
                app_insurance.DISCOUNT_AMOUNT = Convert.ToDouble(txtBasicDiscount.Text.Trim() == "" ? "0" : txtBasicDiscount.Text.Trim());
                app_insurance.PACKAGE = ddlPackage.SelectedItem.Text;
                app_insurance.TOTAL_AMOUNT = Convert.ToDouble(txtBasicAfterDiscount.Text.Trim());
                app_insurance.CREATED_BY = userName;
                app_insurance.CREATED_ON = tran_date;


                #endregion
                #region Insurance rider
                bl_micro_application_insurance_rider app_insurance_rider = new bl_micro_application_insurance_rider();
                if (!string.IsNullOrWhiteSpace(ProductConfig.RiderProductID))
                {
                    //app_insurance_rider.APPLICATION_NUMBER = app.APPLICATION_NUMBER;
                    app_insurance_rider.PRODUCT_ID = hdfRiderProductID.Value;
                    app_insurance_rider.SUM_ASSURE = Convert.ToDouble(ddlRiderSa.SelectedValue);
                    app_insurance_rider.PREMIUM = Convert.ToDouble(txtRiderPremium.Text.Trim());
                    app_insurance_rider.ANNUAL_PREMIUM = Convert.ToDouble(txtRiderAnnualPremium.Text.Trim());
                    app_insurance_rider.DISCOUNT_AMOUNT = Convert.ToDouble(txtRiderDiscount.Text.Trim() == "" ? "0" : txtRiderDiscount.Text.Trim());
                    app_insurance_rider.TOTAL_AMOUNT = Convert.ToDouble(txtRiderAfterDiscount.Text.Trim());
                    app_insurance_rider.CREATED_BY = userName;
                    app_insurance_rider.CREATED_ON = tran_date;
                }
                #endregion

                #region Beneficiary
                bl_micro_application_beneficiary beneficiary = new bl_micro_application_beneficiary();
                // beneficiary.APPLICATION_NUMBER = app.APPLICATION_NUMBER;
                beneficiary.FULL_NAME = txtFullName.Text.Trim();
                beneficiary.AGE = txtAge.Text.Trim();
                beneficiary.RELATION = ddlRelation.SelectedValue;// txtRelation.Text.Trim();
                beneficiary.PERCENTAGE_OF_SHARE = Convert.ToDouble(txtPercentageOfShare.Text.Trim());
                beneficiary.ADDRESS = txtBenAddress.Text.Trim();
                beneficiary.CREATED_BY = userName;
                beneficiary.CREATED_ON = tran_date;
                #endregion

                if (hdfApplicationNumber.Value == "")
                {
                    is_save_customer = da_micro_application_customer.SaveApplicationCustomer(app_cus);

                    if (is_save_customer)
                    {
                    
                        app.APPLICATION_CUSTOMER_ID = app_cus.CUSTOMER_ID;
                        // hdfCustomerID.Value = app_cus.CUSTOMER_ID;
                        hdfApplicationCustomerID.Value = app_cus.CUSTOMER_ID;
                        bl_micro_application_prefix app_prefix = new bl_micro_application_prefix();
                        app_prefix = da_micro_application_prefix.GetLastApplicationPrefix();

                        app.SEQ = app.LAST_SEQ + 1;

                        if (app.LAST_PREFIX == app_prefix.PREFIX2)// in same year
                        {
                            app_number = app_prefix.PREFIX1 + "" + app_prefix.PREFIX2 + "" + (app.SEQ).ToString(app_prefix.DIGITS);
                        }
                        else
                        {
                            int newNumber = 1;
                            app.SEQ = newNumber;
                            app_number = app_prefix.PREFIX1 + "" + app_prefix.PREFIX2 + "" + newNumber.ToString(app_prefix.DIGITS);
                        }


                        app.APPLICATION_NUMBER = app_number;
                        is_save_app = da_micro_application.SaveApplication(app);
                        if (is_save_app)
                        {
                           
                            hdfApplicationNumber.Value = app.APPLICATION_NUMBER;// store to update
                            hdfApplicationID.Value = app.APPLICATION_ID;

                            app_insurance.APPLICATION_NUMBER = app.APPLICATION_NUMBER;
                            app_insurance_rider.APPLICATION_NUMBER = app.APPLICATION_NUMBER;
                            beneficiary.APPLICATION_NUMBER = app.APPLICATION_NUMBER;
                            app_insurance.PAYMENT_CODE = app.APPLICATION_NUMBER;

                            //save insurance
                            is_save_insurance = da_micro_application_insurance.SaveApplicationInsurance(app_insurance);

                            if (is_save_insurance)
                            {
                                // save insurance rider
                                if (!string.IsNullOrWhiteSpace(ProductConfig.RiderProductID))
                                {
                                    is_save_insurance_rider = da_micro_application_insurance_rider.SaveApplicationInsuranceRider(app_insurance_rider);
                                }
                                else // no product rider attched 
                                {
                                    is_save_insurance_rider = true;
                                }

                                if (is_save_insurance_rider)
                                {
                                    //save beneficiary
                                    is_save_beneficiary = da_micro_application_beneficiary.SaveApplicationBeneficiary(beneficiary);
                                    if (!is_save_beneficiary)
                                    {
                                        ViewState["MESSAGE"] = "Save Beneficiary fail.";
                                        RoleBack();
                                    }
                                    else
                                    {
                                        //save qeustion
                                        is_save_question = da_micro_application_questionaire.SaveQuestionaire(new bl_micro_application_questionaire()
                                        {
                                            QUESTION_ID = hdfQuestionID.Value,// "6D44FA76-6B00-42D6-A4D7-ACD85986DC7C",
                                            APPLICATION_NUMBER = hdfApplicationNumber.Value,
                                            ANSWER = Convert.ToInt32(ddlAnswer.SelectedValue),
                                            ANSWER_REMARKS = txtAnswerRemarks.Text.Trim(),
                                            CREATED_ON = tran_date,
                                            CREATED_BY = userName// my_session.USER_NAME
                                        });
                                        if (is_save_question)
                                        {
                                            txtApplicationNumber.Text = hdfApplicationNumber.Value;
                                            //da_customer_lead.UpdateCustomerLeadInsuranceApplicationNumber(hdfApplicationNumber.Value, my_session.LEAD_ID, my_session.USER_NAME, DateTime.Now);

                                            string leadType = "";

                                            if (this.IsUpgratePackage)
                                            {
                                                leadType = "New";
                                            }
                                            else
                                            {
                                                leadType = MYPARA.ApplicationType;
                                            }


                                            da_customer_lead.UpdateCustomerLeadInsuranceApplicationNumber(hdfApplicationNumber.Value, leadID, userName, DateTime.Now, leadType);

                                            da_customer_lead_app_temp.DeleteCustomerLeadAppTempByID(leadID);

                                            logDes = string.Concat("User creates new application [App No.", app.APPLICATION_NUMBER, "].");
                                            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, logDes);
                                            //load existing
                                            BindExisting();
                                            // Alert("Saved Success.");
                                            Helper.Alert(false, "Application is saved successfully. Application Number is <b>" + app.APPLICATION_NUMBER + "</b>", lblError);

                                        }
                                        else
                                        {
                                            ViewState["MESSAGE"] = "Save Questionaire fail.";
                                            RoleBack();
                                        }
                                    }

                                }
                                else
                                {
                                    ViewState["MESSAGE"] = "Saved Insurance rider fial.";
                                    RoleBack();
                                }
                            }
                            else
                            {
                                ViewState["MESSAGE"] = "Saved Insurance Product and detail fial.";
                                RoleBack();
                            }

                        }
                        else
                        {
                            logDes = "User creates new application fail.";
                            ViewState["MESSAGE"] = "Save Application Information fail.";
                            RoleBack();

                        }
                    }
                    else
                    {

                        Helper.Alert(true, "Save Customer Information fail.", lblError);
                    }
                }
                else
                {
                    string errMessage = "";
                    //update 
                    DateTime updated_date = DateTime.Now;
                    app_cus.CUSTOMER_ID = hdfApplicationCustomerID.Value;// hdfCustomerID.Value;
                    app_cus.UPDATED_BY = userName;
                    app_cus.UPDATED_ON = updated_date;

                    is_save_customer = da_micro_application_customer.UpdateApplicationCustomer(app_cus);

                    app.UPDATED_BY = userName;
                    app.UPDATED_ON = updated_date;
                    app.APPLICATION_NUMBER = hdfApplicationNumber.Value;
                    app.APPLICATION_CUSTOMER_ID = hdfApplicationCustomerID.Value;// hdfCustomerID.Value;
                    app.REFERRER_ID = txtReferrerId.Text.Trim();
                    app.CLIENT_TYPE = ddlClientType.SelectedValue;

                    is_save_app = da_micro_application.UpdateApplication(app);
                    if (is_save_app)
                    {
                        app_insurance.UPDATED_BY = userName;
                        app_insurance.UPDATED_ON = updated_date;
                        app_insurance.APPLICATION_NUMBER = hdfApplicationNumber.Value;
                        app_insurance.PAYMENT_CODE = hdfApplicationNumber.Value;
                        is_save_insurance = da_micro_application_insurance.UpdateApplicationInsurance(app_insurance);

                        if (is_save_insurance)
                        {
                            app_insurance_rider.UPDATED_BY = userName;
                            app_insurance_rider.UPDATED_ON = updated_date;
                            app_insurance_rider.APPLICATION_NUMBER = hdfApplicationNumber.Value;
                            if (!string.IsNullOrWhiteSpace(ProductConfig.RiderProductID))
                            {
                                is_save_insurance_rider = da_micro_application_insurance_rider.UpdateApplicationInsuranceRider(app_insurance_rider);
                            }
                            else//no rider attached
                            {
                                is_save_insurance_rider = true;
                            }
                            if (is_save_insurance_rider)
                            {
                                beneficiary.UPDATED_BY = userName;
                                beneficiary.UPDATED_ON = updated_date;
                                beneficiary.APPLICATION_NUMBER = hdfApplicationNumber.Value;
                                is_save_beneficiary = da_micro_application_beneficiary.UpdateApplicationBeneficiary(beneficiary);
                                if (is_save_beneficiary)
                                {
                                    is_save_question = da_micro_application_questionaire.UpdateQuestionaire(new bl_micro_application_questionaire()
                                    {
                                        QUESTION_ID = hdfQuestionID.Value,
                                        APPLICATION_NUMBER = hdfApplicationNumber.Value,
                                        ANSWER = Convert.ToInt32(ddlAnswer.SelectedValue),
                                        ANSWER_REMARKS = txtAnswerRemarks.Text.Trim(),
                                        UPDATED_BY = userName,
                                        UPDATED_ON = tran_date

                                    });
                                    if (is_save_question)
                                    {
                                        string leadType = "";

                                        if (this.IsUpgratePackage)
                                        {
                                            leadType = "New";
                                        }
                                        else
                                        {
                                            leadType = MYPARA.ApplicationType;
                                        }


                                        da_customer_lead.UpdateCustomerLeadInsuranceApplicationNumber(hdfApplicationNumber.Value, leadID, userName, DateTime.Now, leadType);
                                    }
                                    errMessage = is_save_question == true ? "" : "Update Application Questionaire:" + da_micro_application_questionaire.MESSAGE;
                                }

                            }
                            else//update rider fail
                            {
                                is_save_insurance_rider = false;
                                errMessage = "Update Application Insurance Rider:" + da_micro_application_insurance_rider.MESSAGE;
                            }
                        }
                        else//update insurance product fail
                        {
                            is_save_insurance = false;
                            errMessage = "Update Application Insurance:" + da_micro_application_insurance.MESSAGE;
                        }

                    }
                    else//update customer information fail
                    {
                        is_save_customer = false;
                        errMessage = "Update Application Customer:" + da_micro_application_customer.MESSAGE;
                    }
                    if (is_save_customer && is_save_app && is_save_insurance && is_save_insurance_rider && is_save_beneficiary && is_save_question)
                    {
                        //load existing
                        BindExisting();
                        // Alert("Update successfully.");
                        Helper.Alert(false, "Application is updated successfully.", lblError);
                    }
                    else
                    {
                        // Alert("Update fail.");
                        //Helper.Alert(true, "Application is updated fial.", lblError);

                        if (da_micro_application.RestoreApplication(hdfApplicationNumber.Value, userName, Convert.ToDateTime(ViewState["VS_BAK_DATE"] + "")))
                        {

                            BindExisting();
                            Helper.Alert(true, "Application is updated fial, detail [" + errMessage + "]. System is roleback successfully.", lblError);
                        }
                        else
                        {
                            Helper.Alert(true, "Application is updated fial. System is roleback fail.", lblError);
                        }
                    }
                }
                #endregion
            }
            else// invalid
            {
                //Alert(my_session.MESSAGE);
                Helper.Alert(true, message, lblError);
            }
        }
        catch (Exception ex)
        {
            // Alert("Saved fail, place contact your system administrator.");
            Helper.Alert(false, "Application is saved fail, place contact your system administrator.", lblError);
            Log.AddExceptionToLog("Error function [ibtnSave_Click(object sender, ImageClickEventArgs e)] class [banca_micro_application_aspx.cs], detail: ", ex.Message + "==>" + ex.StackTrace);
        }

    }
    void RoleBack()
    {

        da_micro_application_customer.DeleteApplicationCustomer(hdfApplicationCustomerID.Value);
        da_micro_application.DeleteApplication(hdfApplicationNumber.Value);
        da_micro_application_insurance.DeleteApplicationInsurance(hdfApplicationNumber.Value);
        da_micro_application_insurance_rider.DeleteApplicationInsuranceRider(hdfApplicationNumber.Value);
        da_micro_application_beneficiary.DeleteApplicationBeneficiary(hdfApplicationNumber.Value);
        da_micro_application_questionaire.DeleteQuestionaire(hdfApplicationNumber.Value);

        hdfApplicationCustomerID.Value = "";
        hdfApplicationNumber.Value = "";
        hdfApplicationID.Value = "";


        string message = ViewState["MESSAGE"] + "";
        Helper.Alert(false, message, lblError);

    }
    protected void bntIssue_Click(object sender, EventArgs e)
    {
        DateTime tran_date = DateTime.Now;
        bool is_save_cus = false;
        bool is_save_pol = false;
        bool is_save_pol_de = false;
        bool is_save_pol_pay = false;
        bool is_save_pol_ben = false;
        bool is_save_pol_rider = false;
        bool is_save_pol_address = false;

        bool isMoveNext = false;

        string provinceCode = "";
        string districtCode = "";
        string communeCode = "";
        string villageCode = "";

        string channel_id = "0152DF80-BA95-46A9-BB7A-E71966A34089";//corporate

        string leadID = LeadId;
        string agentName = ViewState["AGENT_NAME"] + "";


        double referralFee = 0.0;
        double incentive = 0.0;
        

        provinceCode = GetProvinceCode();
        districtCode = GetDisctrictCode();
        communeCode = GetCommuneCode();
        villageCode = GetVillageCode();

        try
        {
            string messageUpdate = "";
            if (UpdatedAppliction(out messageUpdate))
            {
                Helper.Alert(false, messageUpdate, lblError);
            }
            else
            {
                bl_micro_policy polExist = da_micro_policy.GetPolicyByApplicationID(hdfApplicationID.Value);

                #region calcuslate commission
                da_micro_production_commission_config commConfig = new da_micro_production_commission_config();
                bl_micro_product_commission_config commObj = commConfig.GetProductionCommConfig(ProductConfig.ChannelItemId, ddlPackage.SelectedValue, bl_micro_product_commission_config.CommissionTypeOption.ReferralFee, ddlClientType.SelectedValue);
                if (!String.IsNullOrWhiteSpace(commObj.ProductId))
                {
                    if (commObj.Status == 1 && Helper.FormatDateTime(txtApplicationDate.Text) <= commObj.EffectiveTo)
                    {
                        if (commObj.ValueType == bl_micro_product_commission_config.ValueTypeOption.Fix)
                        {
                            referralFee = commObj.Value;
                        }
                        else if (commObj.ValueType == bl_micro_product_commission_config.ValueTypeOption.Percentage)
                        {
                            referralFee = (Convert.ToDouble(txtTotalPremiumAfterDiscount.Text) * commObj.Value) / 100;
                        }

                    }
                }
                commObj = new bl_micro_product_commission_config();
                commObj = commConfig.GetProductionCommConfig(ProductConfig.ChannelItemId, ddlPackage.SelectedValue, bl_micro_product_commission_config.CommissionTypeOption.Incentive, ddlClientType.SelectedValue);
                if (!String.IsNullOrWhiteSpace(commObj.ProductId))
                {
                    if (commObj.ValueType == bl_micro_product_commission_config.ValueTypeOption.Fix)
                    {
                        incentive = commObj.Value;
                    }
                    else if (commObj.ValueType == bl_micro_product_commission_config.ValueTypeOption.Percentage)
                    {
                        incentive = (Convert.ToDouble(txtTotalPremiumAfterDiscount.Text) * commObj.Value) / 100;
                    }
                }
                #endregion calcuslate commission

                // check existing policy by application id
                if (polExist.APPLICATION_ID == "" || polExist.APPLICATION_ID == null)
                {
                    #region Issued
                    ValidateIssuePolicy();
                    string message = "";
                    bool isValid = false;
                    message = Message;// ViewState["MESSAGE"] + "";
                    isValid = IsValide;// (bool)ViewState["IS_VALID"];
                    if (isValid)
                    {

                        #region save customer
                        bl_micro_customer1 cus = new bl_micro_customer1();
                        bl_micro_customer_prefix pre = new bl_micro_customer_prefix();

                        #region check exist customer
                        cus = new bl_micro_customer1();
                        if (MYPARA.CustomerId != null)
                        {
                            cus.ID = MYPARA.CustomerId;
                        }
                        else
                        {
                            cus = da_micro_customer.GetExistCustomer(txtFirstNameEng.Text.Trim(), txtSurnameEng.Text.Trim(), Helper.FormatDateTime(txtDateOfBirth.Text.Trim()));
                        }


                        #endregion check exist customer
                        if (cus.CUSTOMER_NUMBER != null)
                        {
                            hdfExistCustomerID.Value = cus.ID; ;
                            is_save_cus = true;
                        }
                        else//not exist/ new customer
                        {

                            #region New customer
                            hdfExistCustomerID.Value = "";

                            pre = da_micro_customer_prefix.GetLastCustomerPrefix();
                            // string format = "";
                            string cus_number = "";

                            cus = new bl_micro_customer1();
                            cus.SEQ = cus.LAST_SEQ + 1;
                            if (pre.PREFIX2 == cus.LAST_PREFIX)//in the same year
                            {
                                cus_number = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                            }
                            else
                            {

                                cus.SEQ = 1;
                                cus_number = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                            }

                            cus.CUSTOMER_NUMBER = cus_number;
                            cus.CUSTOMER_TYPE = "INDIVIDUAL";
                            cus.ID_TYPE = ddlIDType.SelectedValue;
                            cus.ID_NUMBER = txtIDNumber.Text.Trim();

                            cus.FIRST_NAME_IN_ENGLISH = txtFirstNameEng.Text.Trim();
                            cus.LAST_NAME_IN_ENGLISH = txtSurnameEng.Text.Trim();
                            cus.FIRST_NAME_IN_KHMER = txtFirstNameKh.Text.Trim();
                            cus.LAST_NAME_IN_KHMER = txtSurnameKh.Text.Trim();
                            cus.GENDER = ddlGender.SelectedValue;
                            cus.DATE_OF_BIRTH = Helper.FormatDateTime(txtDateOfBirth.Text.Trim());
                            cus.NATIONALITY = txtNationality.Text.Trim();
                            cus.MARITAL_STATUS = ddlMaritalStatus.SelectedValue;
                            cus.OCCUPATION = ddlOccupation.SelectedValue;// txtOccupation.Text.Trim();

                            cus.HOUSE_NO_KH = txtHouseNoKh.Text.Trim();
                            cus.STREET_NO_KH = txtStreetKh.Text.Trim();
                            cus.VILLAGE_KH = villageCode;
                            cus.COMMUNE_KH = communeCode;
                            cus.DISTRICT_KH = districtCode;
                            // cus.PROVINCE_KH = txtProvinceKh.Text.Trim();
                            cus.PROVINCE_KH = provinceCode;
                            cus.HOUSE_NO_EN = txtHouseNoEn.Text.Trim();
                            cus.STREET_NO_EN = txtStreetEn.Text.Trim();
                            cus.VILLAGE_EN = villageCode;
                            cus.COMMUNE_EN = communeCode;
                            cus.DISTRICT_EN = districtCode;
                            cus.PROVINCE_EN = provinceCode;
                            cus.PHONE_NUMBER1 = txtPhoneNumber.Text.Trim();
                            cus.EMAIL1 = txtEmail.Text.Trim();
                            cus.CREATED_BY = userName;
                            cus.CREATED_ON = tran_date;
                            cus.STATUS = 1;

                            hdfCustomerID.Value = cus.ID;

                            is_save_cus = da_micro_customer.SaveCustomer(cus);
                            #endregion new customer

                        }
                        #endregion save customer

                        #region save policy
                        bl_micro_policy pol = new bl_micro_policy();
                        bl_micro_policy_prefix pol_pre = new bl_micro_policy_prefix();

                        if (is_save_cus)
                        {

                            pol_pre = da_micro_policy_prefix.GetLastPolicyPrefix();

                            // string pol_format = "";
                            string pol_number = "";

                            pol.SEQ = pol.LAST_SEQ + 1;
                            if (pol.LAST_PREFIX == pol_pre.PREFIX2)//in the same year
                            {
                                pol_number = pol_pre.PREFIX1 + pol_pre.PREFIX2 + pol.SEQ.ToString(pol_pre.DIGITS);
                            }
                            else
                            {
                                pol.SEQ = 1;
                                pol_number = pol_pre.PREFIX1 + pol_pre.PREFIX2 + pol.SEQ.ToString(pol_pre.DIGITS);
                            }

                            pol.POLICY_NUMBER = pol_number;
                            pol.POLICY_TYPE = "COR";
                            pol.APPLICATION_ID = hdfApplicationID.Value;
                            pol.CUSTOMER_ID = cus.ID;
                            pol.PRODUCT_ID = hdfProductID.Value;
                            pol.CHANNEL_ID = channel_id;
                            pol.CHANNEL_ITEM_ID = hdfChannelItemID.Value;
                            pol.CHANNEL_LOCATION_ID = hdfChannelLocationID.Value;
                            pol.AGENT_CODE = hdfSaleAgentID.Value;
                            pol.CREATED_ON = tran_date;
                            pol.CREATED_BY = userName;
                            pol.POLICY_STATUS = "IF";
                            pol.RenewFromPolicy = MYPARA.OldPolicyNumber != null ? MYPARA.OldPolicyNumber : "";//old policy number which do repayment
                            hdfPolicyID.Value = pol.POLICY_ID;

                            is_save_pol = da_micro_policy.SavePolicy(pol);
                        #endregion Save Policy
                            #region save policy detail
                            if (is_save_pol)
                            {

                                bl_micro_policy_detail p_detail = new bl_micro_policy_detail();
                                p_detail.POLICY_ID = pol.POLICY_ID;
                                p_detail.EFFECTIVE_DATE = Helper.FormatDateTime(txtEffectiveDate.Text.Trim());
                                p_detail.ISSUED_DATE = Helper.FormatDateTime(txtIssueDate.Text.Trim());
                                p_detail.MATURITY_DATE = p_detail.EFFECTIVE_DATE.AddYears(1);
                                p_detail.EXPIRY_DATE = p_detail.MATURITY_DATE.AddDays(-1);// p_detail.MATURITY_DATE.AddDays(-1).AddHours(23).AddMinutes(59);
                                p_detail.AGE = Convert.ToInt32(txtCustomerAge.Text.Trim());
                                p_detail.CURRANCY = "USD";
                                p_detail.SUM_ASSURE = Convert.ToDouble(ddlBasicSa.SelectedValue);
                                p_detail.PAY_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                                p_detail.PAYMENT_CODE = hdfApplicationNumber.Value;
                                p_detail.PREMIUM = Convert.ToDouble(txtPremium.Text.Trim());
                                p_detail.ANNUAL_PREMIUM = Convert.ToDouble(txtAnnualPremium.Text.Trim());
                                p_detail.DISCOUNT_AMOUNT = Convert.ToDouble(txtBasicDiscount.Text.Trim() == "" ? "0" : txtBasicDiscount.Text.Trim());
                                p_detail.TOTAL_AMOUNT = Convert.ToDouble(txtBasicAfterDiscount.Text.Trim());/// p_detail.PREMIUM - p_detail.DISCOUNT_AMOUNT;
                                p_detail.REFERRAL_FEE = 0;// p_detail.PREMIUM * 0.25;
                                p_detail.REFERRAL_INCENTIVE = 0;
                                p_detail.COVER_YEAR = Convert.ToInt32(txtTermOfCover.Text.Trim());
                                p_detail.PAY_YEAR = Convert.ToInt32(txtPremiumPaymentPeriod.Text.Trim());
                                p_detail.COVER_UP_TO_AGE = p_detail.AGE + p_detail.COVER_YEAR;
                                p_detail.PAY_UP_TO_AGE = p_detail.AGE + p_detail.PAY_YEAR;
                                p_detail.POLICY_STATUS_REMARKS = ddlClientType.SelectedValue == Helper.ClientTYpe.REPAYMENT.ToString() ? "REPAYMENT" : "NEW";// this.IsUpgratePackage == true ? "New" : this.MYPARA.ApplicationType;
                                p_detail.CREATED_BY = userName;
                                p_detail.CREATED_ON = tran_date;
                                hdfPolicyDetailID.Value = p_detail.POLICY_DETAIL_ID;
                                is_save_pol_de = da_micro_policy_detail.SavePolicyDetail(p_detail);
                            #endregion Save policy detail
                                #region save rider
                                if (is_save_pol_de)
                                {

                                    bl_micro_policy_rider rider = new bl_micro_policy_rider();
                                    if (!string.IsNullOrWhiteSpace(ProductConfig.RiderProductID))
                                    {
                                        rider.POLICY_ID = pol.POLICY_ID;
                                        rider.PRODUCT_ID = hdfRiderProductID.Value;
                                        rider.SUM_ASSURE = Convert.ToDouble(ddlRiderSa.SelectedValue);
                                        rider.PREMIUM = Convert.ToDouble(txtRiderPremium.Text.Trim());
                                        rider.ANNUAL_PREMIUM = Convert.ToDouble(txtRiderAnnualPremium.Text.Trim());
                                        rider.DISCOUNT_AMOUNT = Convert.ToDouble(txtRiderDiscount.Text.Trim() == "" ? "0" : txtRiderDiscount.Text.Trim());
                                        rider.TOTAL_AMOUNT = Convert.ToDouble(txtRiderAfterDiscount.Text.Trim());
                                        rider.CREATED_BY = userName;
                                        rider.CREATED_ON = tran_date;
                                        hdfPolicyRiderID.Value = rider.ID;
                                        is_save_pol_rider = da_micro_policy_rider.SaveRider(rider);
                                    }
                                    else//no rider attached
                                    {
                                        is_save_pol_rider = true;
                                    }
                                #endregion save rider
                                    #region save payment
                                    if (is_save_pol_rider)
                                    {

                                        bl_micro_policy_payment pay = new bl_micro_policy_payment();

                                        pay.POLICY_DETAIL_ID = p_detail.POLICY_DETAIL_ID;
                                        pay.DUE_DATE = p_detail.EFFECTIVE_DATE;
                                        pay.PAY_DATE = p_detail.EFFECTIVE_DATE;
                                        pay.NEXT_DUE = Calculation.GetNext_Due(pay.DUE_DATE.AddYears(1), pay.DUE_DATE, p_detail.EFFECTIVE_DATE);
                                        pay.PREMIUM_YEAR = 1;
                                        pay.PREMIUM_LOT = 1;
                                        pay.USER_PREMIUM = Convert.ToDouble(txtUserPremium.Text.Trim());
                                        pay.AMOUNT = Convert.ToDouble(txtTotalPremium.Text);
                                        pay.DISCOUNT_AMOUNT = Convert.ToDouble(txtTotalDiscountAmount.Text.Trim() == "" ? "0" : txtTotalDiscountAmount.Text.Trim());
                                        pay.TOTAL_AMOUNT = Convert.ToDouble(txtTotalPremiumAfterDiscount.Text);
                                        pay.POLICY_STATUS = "IF";
                                        pay.OFFICE_ID = "Head Office";
                                        pay.PAY_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                                        pay.REFERRAL_FEE = referralFee;// pay.TOTAL_AMOUNT * 0.25;// pay.AMOUNT * 0.25;
                                        pay.REFERRAL_INCENTIVE = incentive;// p_detail.SUM_ASSURE == 5000 ? 2.30 : 2;// new referral fee for package 2 is 2.30 effective from 01 jan 23
                                        pay.TRANSACTION_TYPE = "";
                                        pay.REFERANCE_TRANSACTION_CODE = txtPaymentReferenceNo.Text.Trim();
                                        pay.CREATED_BY = userName;
                                        pay.CREATED_ON = tran_date;
                                        hdfPolicyPaymentID.Value = pay.POLICY_PAYMENT_ID;
                                        is_save_pol_pay = da_micro_policy_payment.SavePayment(pay);
                                    #endregion Save payment
                                        #region save beneficiary
                                        if (is_save_pol_pay)
                                        {

                                            bl_micro_policy_beneficiary ben = new bl_micro_policy_beneficiary();
                                            ben.POLICY_ID = pol.POLICY_ID;
                                            ben.FULL_NAME = txtFullName.Text.Trim();
                                            ben.AGE = txtAge.Text.Trim();
                                            ben.RELATION = ddlRelation.SelectedValue;// txtRelation.Text.Trim();
                                            ben.PERCENTAGE_OF_SHARE = Convert.ToDouble(txtPercentageOfShare.Text.Trim());
                                            ben.ADDRESS = txtBenAddress.Text.Trim();
                                            ben.CREATED_BY = userName;
                                            ben.CREATED_ON = tran_date;
                                            hdfPolicyBenID.Value = ben.ID;
                                            is_save_pol_ben = da_micro_policy_beneficiary.SaveBeneficiary(ben);
                                        #endregion save beneficiary
                                            #region Save address
                                            if (!is_save_pol_ben)//save policy beneficiary fail
                                            {
                                                message = da_micro_policy_beneficiary.MESSAGE;
                                                RoleBackIssuePolicy();
                                            }
                                            else
                                            {
                                                //Alert("Issued successfully.");


                                                bl_micro_policy_address add = new bl_micro_policy_address();
                                                add.HouseNoKh = txtHouseNoKh.Text;
                                                add.HouseNoEn = txtHouseNoEn.Text;
                                                add.StreetNoEn = txtStreetEn.Text;
                                                add.StreetNoKh = txtStreetKh.Text;
                                                add.PolicyID = pol.POLICY_ID;
                                                add.ProvinceCode = GetProvinceCode();
                                                add.DistrictCode = GetDisctrictCode();
                                                add.CommuneCode = GetCommuneCode();
                                                add.VillageCode = GetVillageCode();
                                                add.CreatedOn = tran_date;
                                                add.CreatedBy = userName;// my_session.USER_NAME;
                                                hdfPolicyAddressID.Value = add.ID;
                                                is_save_pol_address = da_micro_policy_address.SaveAddress(add);
                                            #endregion Save address
                                                #region save approver
                                                if (is_save_pol_address)
                                                {
                                                    //save approver
                                                    List<da_report_approver.bl_report_approver> approver = new List<da_report_approver.bl_report_approver>();
                                                    approver = da_report_approver.GetApproverList();
                                                    bool is_save_approver = false;
                                                    foreach (da_report_approver.bl_report_approver ap in approver.Where(_ => _.NameEn == "Prim Somony"))
                                                    {

                                                        is_save_approver = da_report_approver.InsertApproverPolicy(new da_report_approver.bl_report_approver_policy()
                                                        {
                                                            Approver_ID = ap.ID,
                                                            Policy_ID = hdfPolicyID.Value,
                                                            Created_By = userName,//my_session.USER_NAME,
                                                            Created_On = tran_date
                                                        });


                                                    }
                                                #endregion save approver
                                                    #region update lead
                                                    if (is_save_approver)
                                                    {
                                                        //da_customer_lead.UpdateCustomerLeadStatus("Approved", "Issued", my_session.LEAD_ID, my_session.USER_NAME, DateTime.Now);
                                                        da_customer_lead.UpdateCustomerLeadStatus("Approved", "Issued", leadID, userName, DateTime.Now);
                                                        da_customer_lead_status_history.InsertLeadStatus(new bl_customer_lead_status_history()
                                                        {
                                                            LeadID = leadID,
                                                            Status = "Approved",
                                                            StatusRemarks = "Issued",
                                                            CreatedBy = userName,
                                                            CreatedOn = DateTime.Now
                                                        });
                                                    #endregion update lead
                                                        #region send mail to uw
                                                        if (da_customer_lead.SUCCESS)
                                                        {
                                                            #region V4 added
                                                            if (isMobileAppPayment)
                                                            {
                                                                da_PostPayment postPayment = new da_PostPayment();
                                                                postPayment.UpdateAmountUsed(txtApplicationNumber.Text, 1, userName, DateTime.Now);
                                                                if (postPayment.SUCCESS)
                                                                {
                                                                    isMoveNext = true;
                                                                    //  postPayment.DeletePostpayment(postPaymentId);// delete record in post payment realtime table.
                                                                }
                                                                else
                                                                {
                                                                    isMoveNext = false;
                                                                    ViewState["MESSAGE"] = "Issued Policy Fail, becasuse system cannot update post payment status.";
                                                                    RoleBackIssuePolicy();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                isMoveNext = true;
                                                            }
                                                            #endregion V4 added
                                                            if (isMoveNext)//V4 added
                                                            {
                                                                if (AppConfiguration.GetSendEmailOption())
                                                                {
                                                                    #region V1
                                                                    //string mail = "";
                                                                    //mail = "There is a new policy issued  </br>";
                                                                    //mail += "<table border='1'><th>Cusotmer Name</th><th>Gender</th><th>Phone Number</th><th>Customer No.</th><th>Certificate No.</th><th>Product</th><th>Issued Date</th><th>Effectived Date</th><th>SA (USD)</th><th>Premium (USD)</th><th>IA Name</th><th>Branch Name</td>";
                                                                    //mail += "<tr><td>" + cus.LAST_NAME_IN_ENGLISH + " " + cus.FIRST_NAME_IN_ENGLISH + "</td><td>" + Helper.GetGenderText(Convert.ToInt32(cus.GENDER), true) + "</td><td>" + cus.PHONE_NUMBER1 + "</td><td>" + cus.CUSTOMER_NUMBER + "</td><td>" + pol.POLICY_NUMBER + "</td><td>" + pol.PRODUCT_ID + "</td><td>" + p_detail.ISSUED_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.EFFECTIVE_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.SUM_ASSURE + "</td><td>" + pay.TOTAL_AMOUNT + "</td><td>" + agentName + "</td><td>" + txtBranchName.Text + "</td></tr></table>";
                                                                    //if (SendEmail(mail))
                                                                    //{
                                                                    //    Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b>", lblError);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b>. Note: System cannot send email notification to UW department.", lblError);
                                                                    //}
                                                                    #endregion V1

                                                                    #region V2
                                                                    string mail = "";
                                                                    bl_customer_lead lead = da_customer_lead.GetCustomerLeadByApplicationNumber(hdfApplicationNumber.Value);
                                                                    mail = "There is a new policy issued  </br>";
                                                                    mail += "<table border='1'><th>Branch Code</th><th>Brand Name</th><th>Referral Staff ID</th><th>Referral Staff Name</th><th>Application Number</th><th>Certificate No.</th><th>Customer No.</th><th>Payment Reference No.</th><th>Client Name (English)</th><th>Client Name (Khmer)</th><th>Date of Birth</th><th>Phone Number</th><th>Occupation</th><th>Married Status</th><th>Gender</th><th>ID Type</th><th>ID Number</th><th>Village</th><th>Commune</th><th>District</th><th>Province</th><th>Issued Date</th><th>Effective Date</th><th>Maturity Date</th><th>SA</th><th>Permium</th><th>Package</th><th>Agent code</th><th>IA Name</th><th>Benificery Name</th><th>Benificery Age</th><th>Benificery Relation</th>";
                                                                    mail += "<tr><td>" + lead.BranchCode + " </td><td> " + lead.BranchName + "</td><td>" + lead.ReferralStaffId + "</td><td>" + lead.ReferralStaffName + "</td><td>" + hdfApplicationNumber.Value + "</td><td>" + pol.POLICY_NUMBER + "</td><td>" + cus.CUSTOMER_NUMBER + "</td><td>" + pay.REFERANCE_TRANSACTION_CODE + "</td><td>" + cus.LAST_NAME_IN_ENGLISH + " " + cus.FIRST_NAME_IN_ENGLISH + "</td><td>" + cus.LAST_NAME_IN_KHMER + " " + cus.FIRST_NAME_IN_KHMER + "</td><td>" + cus.DATE_OF_BIRTH.ToString("dd-MM-yyyy") + "</td><td>" + cus.PHONE_NUMBER1 + "</td><td>" + cus.OCCUPATION + "</td><td>" + cus.MARITAL_STATUS + "</td><td>" + Helper.GetGenderText(Convert.ToInt32(cus.GENDER), true, true) + "</td><td>" + Helper.GetIDCardTypeText(Convert.ToInt32(cus.ID_TYPE)) + "</td><td>" + cus.ID_NUMBER + "</td><td>" +
                                                                        ddlVillageKh.SelectedValue + "/" + ddlVillageKh.SelectedItem.Text + "</td><td>" + ddlCommuneKh.SelectedValue + "/" + ddlCommuneKh.SelectedItem.Text + "</td><td>" + ddlDistrictKh.SelectedValue + "/" + ddlDistrictKh.SelectedItem.Text + "</td><td>" + ddlProvinceKh.SelectedValue + "/" + ddlProvinceKh.SelectedItem.Text + "</td><td>" + p_detail.ISSUED_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.EFFECTIVE_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.MATURITY_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.SUM_ASSURE + "</td><td>" + pay.TOTAL_AMOUNT + "</td><td>" + ddlPackage.SelectedItem.Text + "</td><td>" + pol.AGENT_CODE + "</td><td>" + da_sale_agent.GetSaleAgentName(pol.AGENT_CODE) + "</td><td>" + ben.FULL_NAME + "</td><td>" + ben.AGE + "</td><td>" + ben.RELATION + "</td></tr></table>";

                                                                    if (SendEmail(mail))
                                                                    {
                                                                        Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b>", lblError);
                                                                    }
                                                                    else
                                                                    {
                                                                        Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b> </br> Note: System cannot send email notification to UW department.", lblError);
                                                                    }
                                                                    #endregion V2
                                                                }
                                                                #region V4: added new
                                                                else
                                                                {
                                                                    Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b> </br> Note: Sending email option is disabled.", lblError);
                                                                }
                                                                #endregion  #region V4: added new

                                                                da_micro_application.DeleteBackupApplication(hdfApplicationNumber.Value, userName, Convert.ToDateTime(ViewState["VS_BAK_DATE"] + ""));
                                                                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User issues new policy [Pol No.", pol.POLICY_NUMBER, ", from App No.", hdfApplicationNumber.Value , "]. "));
                                                                BindExisting();
                                                                return;

                                                            }
                                                        #endregion send email

                                                        }
                                                        else
                                                        {
                                                            ViewState["MESSAGE"] = "Issued Policy Fail, becasuse system cannot update client lead status.";
                                                            RoleBackIssuePolicy();
                                                        }


                                                    }
                                                    else
                                                    {
                                                        ViewState["MESSAGE"] = "Save Approver Information fail.";
                                                        RoleBackIssuePolicy();
                                                    }
                                                }
                                                else
                                                {
                                                    ViewState["MESSAGE"] = da_micro_policy_address.MESSAGE;
                                                    RoleBackIssuePolicy();
                                                }



                                            }



                                        }
                                        else//save policy payment fail
                                        {
                                            ViewState["MESSAGE"] = da_micro_policy_payment.MESSAGE;
                                            RoleBackIssuePolicy();
                                        }


                                    }
                                    else//save policy rider fail
                                    {
                                        ViewState["MESSAGE"] = da_micro_policy_rider.MESSAGE;
                                        RoleBackIssuePolicy();
                                    }



                                }
                                else//save policy detail fail
                                {
                                    ViewState["MESSAGE"] = da_micro_policy_detail.MESSAGE;
                                    RoleBackIssuePolicy();
                                }

                            }
                            else//save policy fail
                            {
                                ViewState["MESSAGE"] = da_micro_policy.MESSAGE;
                                RoleBackIssuePolicy();
                            }
                        }
                        else //save customer error
                        {
                            Helper.Alert(true, da_micro_customer.MESSAGE, lblError);

                        }


                    }
                    else
                    {
                        //  Alert(my_session.MESSAGE);

                        Helper.Alert(true, message, lblError);
                    }
                    #endregion Issued
                }
                else
                {
                    Helper.Alert(true, "Policy number [" + polExist.POLICY_NUMBER + "] is already exist.", lblError);
                }
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [bntIssue_Click(object sender, EventArgs e)] in class [banca_micro_application.aspx.cs], detail: " + ex.Message + "=>" + ex.StackTrace);
            RoleBackIssuePolicy();
            Helper.Alert(true, ex.StackTrace, lblError);
        }
    }

    void RoleBackIssuePolicy(string INPUT_MESSAGE = "")
    {
        string cus_id = "";
        string pol_id = "";
        string pol_de_id = "";
        string pol_ben_id = "";
        string pol_rider_id = "";
        string pol_pay_id = "";
        string pol_address_id = "";
        bool is_roleback = false;
        if (hdfExistCustomerID.Value == "")
        {
            cus_id = hdfCustomerID.Value;// new customer id when can be deleted.
        }
        pol_id = hdfPolicyID.Value;
        pol_de_id = hdfPolicyDetailID.Value;
        pol_pay_id = hdfPolicyPaymentID.Value;
        pol_ben_id = hdfPolicyBenID.Value;
        pol_rider_id = hdfPolicyBenID.Value;
        pol_address_id = hdfPolicyAddressID.Value;

        is_roleback = da_banca.RoleBackIssuePolicy(cus_id, pol_id, pol_de_id, pol_rider_id, pol_pay_id, pol_ben_id, pol_address_id);
        if (is_roleback)
        {
            hdfPolicyID.Value = "";
            hdfPolicyBenID.Value = "";
            hdfPolicyDetailID.Value = "";
            hdfPolicyPaymentID.Value = "";
            hdfPolicyRiderID.Value = "";
            hdfCustomerID.Value = "";
            hdfPolicyAddressID.Value = "";
            Helper.Alert(true, "Issued policy fail. " + ViewState["MESSAGE"] + " [System roleback successfully.]", lblError);
        }
        else
        {
            Helper.Alert(true, "Issued policy  fail. " + ViewState["MESSAGE"] + " [System roleback fail.]", lblError);
        }
    }

    protected void ibtnPrintApplication_Click(object sender, ImageClickEventArgs e)
    {
        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat( "User views application form [App No.", txtApplicationNumber.Text,"]."));
        string url = "banca_micro_application_print.aspx?APP_ID=" + hdfApplicationID.Value + "&A_TYPE=IND"; 
        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);

    }
    protected void ibtnPrintCertificate_Click(object sender, ImageClickEventArgs e)
    {
        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views certificate [Cert No.", txtPolicyNumber.Text, "]."));
        string url = "banca_micro_cert.aspx?P_ID=" + hdfPolicyID.Value + "&P_TYPE=IND"; ;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
    }
    protected void ddlProvinceKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProvinceKh.SelectedIndex > 0)
        {
            BindDistrict();
            //clear dropdownlist
            ddlCommuneKh.Items.Clear();
            ddlCommuneKh.Items.Add(new ListItem("--ឃុំ/សង្កាត់--", ""));
            ddlVillageKh.Items.Clear();
            ddlVillageKh.Items.Add(new ListItem("--ភូមិ--", ""));



            txtProvinceEn.Text = ddlProvinceKh.SelectedValue;
            txtCurrentAddress_TextChanged(null, null);
        }
        else
        {
            ddlDistrictKh.Items.Clear();
            ddlDistrictKh.Items.Add(new ListItem("--ស្រុក/ខណ្ឌ--", ""));
        }
    }


    private bool SendEmail(string mailBody)
    {
        bool result = false;
        try
        {
            #region V1
            //  EmailSender mail;
            //  mail = new EmailSender();
            //  mail.From = "";
            //  mail.From = "autosend@camlife.com.kh";
            //// mail.To = "uw.support@camlife.com.kh";
            //mail.To = "maneth.som@camlife.com.kh";
            // mail.BCC = "maneth.som@camlife.com.kh";
            //  mail.Subject = "New Policy Issued";
            //  mail.Message = mailBody;
            //  mail.Host = "mail.camlife.com.kh";

            //  mail.Port = 587;
            //  mail.Password = "admin1!2cdef";
            #endregion V1
            #region V2
            EmailSender mail;
            mail = new EmailSender();
            mail.From = AppConfiguration.GetEmailFrom();

            mail.To = AppConfiguration.GetEmailTo();
            //mail.BCC = "maneth.som@camlife.com.kh";
            mail.Subject = "New Policy Issued";
            mail.Message = mailBody;
            mail.Host = AppConfiguration.GetEmailHost();

            mail.Port = AppConfiguration.GetEmailPort();
            mail.Password = AppConfiguration.GetEmailPassword();

            #endregion V2

            if (mail.SendMail(mail))
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [SendEmail(string mailBody)] in class [banca_micro_application.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
        return result;
    }


    void BindProvince()
    {
        List<bl_address.province> ProList = da_address.province.GetProvince();
        ddlProvinceKh.Items.Clear();
        ddlProvinceKh.Items.Add(new ListItem("--ខេត្ត/ក្រុង--", ""));
        if (ProList.Count > 0)
        {

            foreach (bl_address.province pro in ProList)
            {
                ddlProvinceKh.Items.Add(new ListItem(pro.Khmer, pro.Code + "/" + pro.English));
            }

        }
        
    }

    void SelectProvince(string value)
    {
        string val = "";
        for (int i = 0; i < ddlProvinceKh.Items.Count; i++)
        {

            val = ddlProvinceKh.Items[i].Value;
            string code = "";
            string[] split = val.Split('/');
            if (split.Length > 0)
            {
                code = split[0].ToString();
                if (code == value)
                {
                    ddlProvinceKh.SelectedIndex = i;
                    ddlProvinceKh_SelectedIndexChanged(null, null);
                    break;
                }
            }
           
        }
    }
    void selectDistrict(string value)
    {
        string val = "";
        for (int i = 0; i < ddlDistrictKh.Items.Count; i++)
        {

            val = ddlDistrictKh.Items[i].Value;
            string code = "";
            string[] split = val.Split('/');
            if (split.Length > 0)
            {
                code = split[0].ToString();
                if (code == value)
                {
                    ddlDistrictKh.SelectedIndex = i;
                    ddlDistrictKh_SelectedIndexChanged(null, null);
                    break;
                }
            }



        }
    }
    void selectCommune(string value)
    {
        string val = "";
        for (int i = 0; i < ddlCommuneKh.Items.Count; i++)
        {

            val = ddlCommuneKh.Items[i].Value;
            string code = "";
            string[] split = val.Split('/');
            if (split.Length > 0)
            {
                code = split[0].ToString();
                if (code == value)
                {
                    ddlCommuneKh.SelectedIndex = i;
                    ddlCommuneKh_SelectedIndexChanged(null, null);
                    break;
                }
            }



        }
    }
    void selectVillage(string value)
    {
        string val = "";
        for (int i = 0; i < ddlVillageKh.Items.Count; i++)
        {

            val = ddlVillageKh.Items[i].Value;
            string code = "";
            string[] split = val.Split('/');
            if (split.Length > 0)
            {
                code = split[0].ToString();
                if (code == value)
                {
                    ddlVillageKh.SelectedIndex = i;
                    ddlVillageKh_SelectedIndexChanged(null, null);
                    break;
                }
            }



        }
    }
    void BindDistrict()
    {
        if (ddlProvinceKh.SelectedValue != "")
        {

            List<bl_address.district> disList = da_address.district.GetDistrict(GetProvinceCode());
            ddlDistrictKh.Items.Clear();
            ddlDistrictKh.Items.Add(new ListItem("--ស្រុក/ខណ្ឌ--", ""));
            if (disList.Count > 0)
            {

                foreach (bl_address.district dis in disList)
                {
                    ddlDistrictKh.Items.Add(new ListItem(dis.Khmer, dis.Code + "/" + dis.English));
                }
            }
        }

    }
    void BindCommune()
    {
        if (ddlDistrictKh.SelectedValue != "")
        {

            List<bl_address.commune> comList = da_address.commune.GetCommune(GetDisctrictCode());
            ddlCommuneKh.Items.Clear();
            ddlCommuneKh.Items.Add(new ListItem("--ឃុំ/សង្កាត់--", ""));
            if (comList.Count > 0)
            {

                foreach (bl_address.commune com in comList)
                {
                    ddlCommuneKh.Items.Add(new ListItem(com.Khmer, com.Code + "/" + com.English));
                }
            }
        }

    }
    void BindVillage()
    {
        if (ddlCommuneKh.SelectedValue != "")
        {

            List<bl_address.village> viList = da_address.village.GetVillage(GetCommuneCode());
            ddlVillageKh.Items.Clear();
            ddlVillageKh.Items.Add(new ListItem("--ភូមិ--", ""));
            if (viList.Count > 0)
            {

                foreach (bl_address.village vi in viList)
                {
                    ddlVillageKh.Items.Add(new ListItem(vi.Khmer, vi.Code + "/" + vi.English));
                }
            }
        }

    }
    string GetProvinceCode()
    {
        string code = "";
        string[] split = ddlProvinceKh.SelectedValue.Split('/');
        if (split.Length > 0)
        {
            code = split[0].ToString();
        }
        return code;
    }
    string GetDisctrictCode()
    {
        string code = "";
        string[] split = ddlDistrictKh.SelectedValue.Split('/');
        if (split.Length > 0)
        {
            code = split[0].ToString();
        }
        return code;
    }
    string GetCommuneCode()
    {
        string code = "";
        string[] split = ddlCommuneKh.SelectedValue.Split('/');
        if (split.Length > 0)
        {
            code = split[0].ToString();
        }
        return code;
    }
    string GetVillageCode()
    {
        string code = "";
        string[] split = ddlVillageKh.SelectedValue.Split('/');
        if (split.Length > 0)
        {
            code = split[0].ToString();
        }
        return code;
    }
    protected void ddlCommuneKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVillage();
        txtCommuneEn.Text = ddlCommuneKh.SelectedValue;
        txtCurrentAddress_TextChanged(null, null);
    }
    protected void ddlDistrictKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCommune();
        ddlVillageKh.Items.Clear();
        ddlVillageKh.Items.Add(new ListItem("--ភូមិ--", ""));
        txtDistrictEn.Text = ddlDistrictKh.SelectedValue;
        txtCurrentAddress_TextChanged(null, null);
    }
    protected void ddlVillageKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtVillageEn.Text = ddlVillageKh.SelectedValue;
        txtCurrentAddress_TextChanged(null, null);
    }

    bool UpdatedAppliction(out string messageUpdate)
    {
        bool result = false;
        messageUpdate = "";
        if (hdfOldClientType.Value != ddlClientType.SelectedValue)
        {
            messageUpdate = "You have changed client type [" + hdfOldClientType.Value + "] To [" + ddlClientType.SelectedValue + "].";
            result = true;
        }
        else if(hdfOldBankStaffName.Value!=txtBankStaffName.Text.Trim())
        {
            if (ddlClientType.SelectedValue == Helper.ClientTYpe.CLIENT_FAMILY.ToString())
            {
                messageUpdate = "You have changed client name [" + hdfOldBankStaffName.Value + "] To [" + txtBankStaffName.Text + "].";
                result = true;
            }
            else if (ddlClientType.SelectedValue == Helper.ClientTYpe.BANK_STAFF_FAMILY.ToString())
            {
                messageUpdate = "You have changed bank staff name [" + hdfOldBankStaffName.Value + "] To [" + txtBankStaffName.Text + "].";
                result = true;
            }
        }


        else if (hdfOldClientTypeRelation.Value != ddlClientTypeRelation.SelectedValue)
        {
            messageUpdate = "You have changed ralation  [" + hdfOldClientTypeRelation.Value + "] To [" + ddlClientTypeRelation.SelectedValue + "].";
            result = true;
        }
        if (hdfOldApplicationDate.Value != txtApplicationDate.Text.Trim())
        {
            messageUpdate = "You have changed Application Date [" + hdfOldApplicationDate.Value + "] To [" + txtApplicationDate.Text + "].";
            result = true;
        }
        else if (hdfOldReferrerId.Value != txtReferrerId.Text.Trim())
        {
            messageUpdate = "You have changed Referral Id [" + hdfOldReferrerId.Value + "] To [" + txtReferrerId.Text + "].";
            result = true;
        }
        else if (hdfOldIdType.Value != ddlIDType.SelectedItem.Text)
        {
            messageUpdate = "You have changed ID Type [" + hdfOldIdType.Value + "] To [" + ddlIDType.SelectedItem.Text + "].";
            result = true;
        }
        else if (hdfOldIdNumber.Value.Trim() != txtIDNumber.Text.Trim())
        {
            messageUpdate = "You have changed ID Number [" + hdfOldIdNumber.Value + "] To [" + txtIDNumber.Text.Trim() + "].";
            result = true;
        }
        else if (hdfSurnameKh.Value.Trim() != txtSurnameKh.Text.Trim())
        {
            messageUpdate = "You have changed Surname In Khmer [" + hdfSurnameKh.Value + "] To [" + txtSurnameKh.Text.Trim() + "].";
            result = true;
        }
        else if (hdfFirstnameKh.Value.Trim() != txtFirstNameKh.Text.Trim())
        {
            messageUpdate = "You have changed First Name In Khmer [" + hdfFirstnameKh.Value + "] To [" + txtFirstNameKh.Text.Trim() + "].";
            result = true;
        }
        else if (hdfSurnameEng.Value.Trim() != txtSurnameEng.Text.Trim())
        {
            messageUpdate = "You have changed Surname In English [" + hdfSurnameEng.Value + "] To [" + txtSurnameEng.Text.Trim() + "].";
            result = true;
        }
        else if (hdfFirstNameEng.Value.Trim() != txtFirstNameEng.Text.Trim())
        {
            messageUpdate = "You have changed First Name In English [" + hdfFirstNameEng.Value + "] To [" + txtFirstNameEng.Text.Trim() + "].";
            result = true;
        }
        else if (hdfNationlity.Value.Trim() != txtNationality.Text.Trim())
        {
            messageUpdate = "You have changed Nationality [" + hdfNationlity.Value + "] To [" + txtNationality.Text.Trim() + "].";
            result = true;
        }
        else if (hdfGender.Value != ddlGender.SelectedItem.Text)
        {
            messageUpdate = "You have changed Gender [" + hdfGender.Value + "] To [" + ddlGender.SelectedItem.Text + "].";
            result = true;
        }
        else if (hdfDateOfBirth.Value != txtDateOfBirth.Text)
        {
            messageUpdate = "You have changed Date of Birth [" + hdfDateOfBirth.Value + "] To [" + txtDateOfBirth.Text + "].";
            result = true;
        }
        else if (hdfMaritalStatus.Value != ddlMaritalStatus.SelectedItem.Text)
        {
            messageUpdate = "You have changed Marital Status [" + hdfMaritalStatus.Value + "] To [" + ddlMaritalStatus.SelectedItem.Text + "].";
            result = true;
        }
        else if (hdfOccupation.Value != ddlOccupation.SelectedItem.Text)
        {
            messageUpdate = "You have changed Occupation [" + hdfOccupation.Value + "] To [" + ddlOccupation.SelectedItem.Text + "].";
            result = true;
        }
        else if (hdfPhoneNumber.Value.Trim() != txtPhoneNumber.Text.Trim())
        {
            messageUpdate = "You have changed Phone Number [" + hdfPhoneNumber.Value + "] To [" + txtPhoneNumber.Text + "].";
            result = true;
        }
        else if (hdfEmail.Value.Trim() != txtEmail.Text.Trim())
        {
            messageUpdate = "You have changed Email [" + hdfEmail.Value + "] To [" + txtEmail.Text + "].";
            result = true;
        }
        else if (hdfHouseNoKh.Value.Trim() != txtHouseNoKh.Text.Trim())
        {
            messageUpdate = "You have changed House No. [" + hdfHouseNoKh.Value + "] To [" + txtHouseNoKh.Text + "].";
            result = true;
        }
        else if (hdfHouseNoEn.Value.Trim() != txtHouseNoEn.Text.Trim())
        {
            messageUpdate = "You have changed House No. [" + hdfHouseNoEn.Value + "] To [" + txtHouseNoEn.Text + "].";
            result = true;
        }
        else if (hdfStreetKh.Value.Trim() != txtStreetKh.Text.Trim())
        {
            messageUpdate = "You have changed Street No. [" + hdfStreetKh.Value + "] To [" + txtStreetKh.Text + "].";
            result = true;
        }
        else if (hdfStreetEn.Value.Trim() != txtStreetEn.Text.Trim())
        {
            messageUpdate = "You have changed Street No. [" + hdfStreetEn.Value + "] To [" + txtStreetEn.Text + "].";
            result = true;
        }
        else if (hdfVillagekh.Value.Trim() != ddlVillageKh.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed Village [" + hdfVillagekh.Value + "] To [" + ddlVillageKh.SelectedItem.Text.Trim() + "].";
            result = true;
        }
        else if (hdfVillagekh.Value.Trim() != ddlVillageKh.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed Village [" + hdfVillagekh.Value + "] To [" + ddlVillageKh.SelectedItem.Text.Trim() + "].";
            result = true;
        }
        else if (hdfVillageEn.Value.Trim() != txtVillageEn.Text.Trim())
        {
            messageUpdate = "You have changed Village [" + hdfVillageEn.Value + "] To [" + txtVillageEn.Text.Trim() + "].";
            result = true;
        }
        else if (hdfCommuneKh.Value.Trim() != ddlCommuneKh.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed Commune [" + hdfCommuneKh.Value + "] To [" + ddlCommuneKh.SelectedItem.Text.Trim() + "].";
            result = true;
        }
        else if (hdfCommuneEn.Value.Trim() != txtCommuneEn.Text.Trim())
        {
            messageUpdate = "You have changed Commune [" + hdfCommuneEn.Value + "] To [" + txtCommuneEn.Text.Trim() + "].";
            result = true;
        }
        else if (hdfDistrictKh.Value.Trim() != ddlDistrictKh.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed District [" + hdfDistrictKh.Value + "] To [" + ddlDistrictKh.SelectedItem.Text.Trim() + "].";
            result = true;
        }
        else if (hdfDistrictEn.Value.Trim() != txtDistrictEn.Text.Trim())
        {
            messageUpdate = "You have changed District [" + hdfDistrictEn.Value + "] To [" + txtDistrictEn.Text.Trim() + "].";
            result = true;
        }
        else if (hdfProvinceKh.Value.Trim() != ddlProvinceKh.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed Province [" + hdfProvinceKh.Value + "] To [" + ddlProvinceKh.SelectedItem.Text.Trim() + "].";
            result = true;
        }
        else if (hdfProvinceEn.Value.Trim() != txtProvinceEn.Text.Trim())
        {
            messageUpdate = "You have changed Province [" + hdfProvinceEn.Value + "] To [" + txtProvinceEn.Text.Trim() + "].";
            result = true;
        }
        else if (hdfPackage.Value.Trim() != ddlBasicSa.SelectedValue)
        {
            messageUpdate = "You have changed Package [" + hdfPackage.Value + "] To [" + ddlPackage.SelectedItem.Text + "].";
            result = true;
        }
        else if (hdfFullName.Value.Trim() != txtFullName.Text.Trim())
        {
            messageUpdate = "You have changed Beneficiary Full Name [" + hdfFullName.Value + "] To [" + txtFullName.Text + "].";
            result = true;
        }
        else if (hdfAge.Value.Trim() != txtAge.Text.Trim())
        {
            messageUpdate = "You have changed Beneficiary Age [" + hdfAge.Value + "] To [" + txtAge.Text + "].";
            result = true;
        }
        else if (hdfRelation.Value.Trim() != ddlRelation.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed Beneficiary Relation [" + hdfRelation.Value + "] To [" + ddlRelation.SelectedItem.Text + "].";
            result = true;
        }
        else if (hdfAnswer.Value.Trim() != ddlAnswer.SelectedItem.Text.Trim())
        {
            messageUpdate = "You have changed Answer [" + hdfAnswer.Value + "] To [" + ddlAnswer.SelectedItem.Text + "].";
            result = true;

        }
        else if (hdfAnswerRemarks.Value.Trim() != txtAnswerRemarks.Text.Trim())
        {
            messageUpdate = "You have changed Answer Detail [" + hdfAnswerRemarks.Value + "] To [" + txtAnswerRemarks.Text + "].";
            result = true;
        }

        //if (ddlAnswer.SelectedValue == "1")
        //{

        //    messageUpdate = "Pre-existing is detected, system is not alowed to save.";
        //    result = true;
        //}

        messageUpdate += " Please save application before processing issue policy.";
        return result;
    }
    protected void btnGetPayment_Click(object sender, EventArgs e)
    {
        bl_PostPayment postPayment = new bl_PostPayment();

        da_PostPayment tranPost = new da_PostPayment();
        postPayment = tranPost.GetPostPaymentAmountNotUse(txtApplicationNumber.Text);
        if (tranPost.SUCCESS)
        {
            if (postPayment != null)// has record in post payment
            {
                isMobileAppPayment = true;
                postPaymentId = postPayment.Id;
                txtUserPremium.Text = postPayment.BillAmount + "";
                txtPaymentReferenceNo.Text = postPayment.TransactionReferenceNumber;
                txtUserPremium.Attributes.Add("disabled", "disabled");
                txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");

            }
            else// no record in post payment
            {
                isMobileAppPayment = false;
                postPaymentId = "";
                txtPaymentReferenceNo.Attributes.Remove("disabled");
                txtUserPremium.Attributes.Remove("disabled");
                Helper.Alert(false, "[Payment] information is not found, please manually input.", lblError);
            }
            btnIssue.Attributes.Remove("disabled");
        }
        else
        {
            isMobileAppPayment = false;
            postPaymentId = "";
            btnIssue.Attributes.Add("disabled", "disabled");
            // system get error while get post payment
            Helper.Alert(true, tranPost.MESSAGE, lblError);
        }


    }
    protected void ddlBasicSa_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculatePremium();
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {

        CalculatePremium();


    }
    protected void ddlRiderSa_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculatePremium();
    }
    protected void ddlClientType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlClientType.SelectedValue == Helper.ClientTYpe.BANK_STAFF_FAMILY.ToString())
        {
            lblBankStaffName.Text = "Bank Staff Name";
            ShowClientTypeRemarks(true);
        }
        else if (ddlClientType.SelectedValue == Helper.ClientTYpe.CLIENT_FAMILY.ToString())
        {
            lblBankStaffName.Text = "Client Name";
            ShowClientTypeRemarks(true);
        }
        //else if (ddlClientType.SelectedValue == Helper.ClientTYpe.REPAYMENT.ToString())
        //{
        //    ShowClientTypeRemarks(false);

        //    txtReletedCertificate.Attributes.Remove("disabled");
        //    txtReletedCertificate.Enabled = true;
        //}
        else
        {
            ShowClientTypeRemarks(false);

           txtReletedCertificate.Enabled = false;
        }

        ddlPackage.SelectedIndex = 0;
        ddlPackage_SelectedIndexChanged(null, null);
    }

    void ShowClientTypeRemarks(bool t) {
        lblBankStaffName.Visible = t;
        txtBankStaffName.Visible = t;
        lblClientTypeRelation.Visible = t;
        ddlClientTypeRelation.Visible = t;

        if (t)
        {
            txtBankStaffName.Text = "";
            ddlClientTypeRelation.SelectedIndex = 0;
        }
    }
}