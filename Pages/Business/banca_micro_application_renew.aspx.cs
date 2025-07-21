
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Data;
public partial class Pages_Business_banca_micro_application_renew : System.Web.UI.Page
{

    string userName = "";
    private bl_micro_product_config ProductConfig { get { return (bl_micro_product_config)ViewState["V_PRODUCT_CONFIG"]; } set { ViewState["V_PRODUCT_CONFIG"] = value; } }
    private bl_micro_policy Policy { get { return (bl_micro_policy)ViewState["V_POLICY"]; } set { ViewState["V_POLICY"] = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";

        userName = Membership.GetUser().UserName;

        if (!Page.IsPostBack)
        {
            SESSION_PARA para = new SESSION_PARA();
            para = (SESSION_PARA)Session["SS_SESSION_PARA"];
            if (Request.QueryString.Count == 0 || para == null)
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
                DisabledControls();
                //DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SELECT Pay_Mode_ID, MODE FROM Ct_Payment_Mode;");
                //Options.Bind(ddlPaymentMode, tbl, "MODE", "Pay_Mode_ID", 1);

                string polId = Request.QueryString["p_id"].ToString();

                BindProvince();

                Helper.BindOccupation(ddlOccupation, "KH");
                Helper.BindRelation(ddlRelation, "KH");

                #region get policy information
                bl_micro_policy pol = da_micro_policy.GetPolicyByID(polId, userName);
                Policy = pol;
                //  bl_micro_policy_rider polR = da_micro_policy_rider.GetRider(polId,userName);
                #endregion get policy information

                hdfProductID.Value = pol.POLICY_ID;
                // hdfRiderProductID.Value = String.IsNullOrWhiteSpace(polR.PRODUCT_ID) == true ? "" : polR.PRODUCT_ID;
                hdfOldPolicyId.Value = polId;


                bl_customer_lead_app_temp cus = new bl_customer_lead_app_temp();
                bl_micro_product_config proConf = new bl_micro_product_config();
                proConf = da_micro_product_config.ProductConfig.GetProductMicroProduct(hdfProductID.Value);
                ProductConfig = proConf;

                ddlPackage.Items.Clear();
                List<bl_micro_product_config> proConList = da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(para.ChannelItemId, true);
                Options.Bind(ddlPackage, proConList, bl_micro_product_config.NAME.MarketingName, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");
                BindBasicSA(ProductConfig.BasicSumAssuredRange);
                BindRiderSA(ProductConfig.RiderSumAssuredRange);
                BindPayMode(ProductConfig.PayMode);

                #region //check existing
                if (hdfOldPolicyId.Value != "")
                {
                    bl_micro_policy_expiring objPol = da_micro_policy_expiring.GetPolicyExpiringObject(hdfOldPolicyId.Value);
                    if (objPol.PolicyNumber != null && objPol.PolicyNumber != "")//check existing record in ct_micro_policy_expiring
                    {
                        if (objPol.NewApplicationNumber != null && objPol.NewApplicationNumber != "")//policy is already convert to application
                        {
                            hdfApplicationNumber.Value = objPol.NewApplicationNumber;
                            bl_micro_application appExist = da_micro_application.GetApplication(hdfApplicationNumber.Value);
                            hdfApplicationID.Value = appExist.APPLICATION_ID;
                            BindExistingApp();
                        }
                        else
                        {
                            BindExistingPolicy();
                        }
                    }
                    else
                    {
                        DisabledAllControls();
                        btnIssue.Attributes.Add("disabled", "disabled");
                        txtIssueDate.Attributes.Add("disabled", "disabled");
                        txtUserPremium.Attributes.Add("disabled", "disabled");
                        txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
                        ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                        ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                        ibtnSave.Attributes.Add("disabled", "disabled");
                        Helper.Alert(true, "Record is not found or it was converted to policy already, please go to page Policy Expiring then refresh the page.", lblError);
                    }


                }
                #endregion //check existing
                #region
                else
                {
                    DisabledAllControls();

                }
                #endregion

            }


        }
    }
    void BindRiderSA(double[] sa)
    {
        ddlRiderSumAssure.Items.Clear();
        ddlRiderSumAssure.Items.Add(new ListItem("---Select---", "0"));
        foreach (double d in sa)
        {
            ddlRiderSumAssure.Items.Add(new ListItem(d + "", d + ""));
        }
        if (sa.Count() == 1)
        {
            ddlRiderSumAssure.SelectedIndex = 1;
            ddlRiderSumAssure.Enabled = false;
        }
        else
        {
            ddlRiderSumAssure.SelectedIndex = 0;
            ddlRiderSumAssure.Enabled = true;
        }
    }
    void BindBasicSA(double[] sa)
    {
        ddlBasicSA.Items.Clear();
        ddlBasicSA.Items.Add(new ListItem("---Select---", "0"));
        foreach (double d in sa)
        {
            ddlBasicSA.Items.Add(new ListItem(d + "", d + ""));
        }
        if (sa.Count() == 1)
        {
            ddlBasicSA.SelectedIndex = 1;
            ddlBasicSA.Enabled = false;
        }
        else
        {
            ddlBasicSA.SelectedIndex = 0;
            ddlBasicSA.Enabled = true;
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

    /// <summary>
    /// is the expiring policy
    /// </summary>
    void BindExistingPolicy()
    {

        bl_micro_policy objPol = Policy;// da_micro_policy.GetPolicyByID(hdfOldPolicyId.Value);

        txtPolicyStatus.Text = "";
        txtReletedCertificate.Text = objPol.POLICY_NUMBER;

        if (objPol.POLICY_ID != null && objPol.POLICY_ID != "")
        {
            SESSION_PARA para = new SESSION_PARA();
            para = (SESSION_PARA)Session["SS_SESSION_PARA"];

            bl_micro_application app = da_micro_application.GetApplicationByApplicationID(objPol.APPLICATION_ID);
            bl_micro_application_insurance appin = da_micro_application_insurance.GetApplicationInsurance(app.APPLICATION_NUMBER, userName);
            bl_micro_application_insurance_rider appRider = da_micro_application_insurance_rider.GetApplicationRider(app.APPLICATION_NUMBER);
            //  bl_micro_policy_detail polD = da_micro_policy_detail.GetPolicyDetailByPolicyID(objPol.POLICY_ID);
      

            hdfChannelItemID.Value = para.ChannelItemId;
            hdfChannelLocationID.Value = para.ChannelLocationId;
            hdfSaleAgentID.Value = para.AgentCode;
            hdfSaleAgentName.Value = para.AgentName;
            txtBranchCode.Text = para.BranchCode;
            txtBranchName.Text = para.BranchName;
            txtSaleAgentID.Text = para.AgentCode;
            txtSaleAgentName.Text = para.AgentName;
            txtApplicationDate.Text = DateTime.Now.ToString("dd-MM-yyyy");


            txtReferrerId.Text = app.REFERRER_ID;
            txtReferrerName.Text = app.REFERRER;
            #region customer information

            bl_micro_customer1 cus = da_micro_customer.GetCustomer(objPol.CUSTOMER_ID);

            Helper.SelectedDropDownListIndex("VALUE", ddlIDType, cus.ID_TYPE);
            txtIDNumber.Text = cus.ID_NUMBER;
            txtSurnameEng.Text = cus.LAST_NAME_IN_ENGLISH;
            txtFirstNameEng.Text = cus.FIRST_NAME_IN_ENGLISH;
            txtSurnameKh.Text = cus.LAST_NAME_IN_KHMER;
            txtFirstNameKh.Text = cus.FIRST_NAME_IN_KHMER;
            txtNationality.Text = cus.NATIONALITY;
            Helper.SelectedDropDownListIndex("VALUE", ddlGender, cus.GENDER);
            txtDateOfBirth.Text = cus.DATE_OF_BIRTH.ToString("dd-MM-yyyy");
            Helper.SelectedDropDownListIndex("VALUE", ddlMaritalStatus, cus.MARITAL_STATUS);

            Helper.SelectedDropDownListIndex("TEXT", ddlOccupation, cus.OCCUPATION);
            txtPhoneNumber.Text = cus.PHONE_NUMBER1;
            txtEmail.Text = cus.EMAIL1;
            int age = CalculateAge(txtDateOfBirth.Text, txtApplicationDate.Text);
            txtCustomerAge.Text = age + "";
            txtHouseNoEn.Text = cus.HOUSE_NO_EN;
            txtStreetEn.Text = cus.STREET_NO_EN;

            txtVillageEn.Text = cus.VILLAGE_EN;
            txtCommuneEn.Text = cus.COMMUNE_EN;
            txtDistrictEn.Text = cus.DISTRICT_EN;
            txtProvinceEn.Text = cus.PROVINCE_EN;
            txtHouseNoKh.Text = cus.HOUSE_NO_KH;
            txtStreetKh.Text = cus.STREET_NO_KH;

            Helper.SelectedDropDownListIndex("VALUE", ddlProvinceKh, cus.PROVINCE_EN);

            //bind district
            BindDistrict();
            Helper.SelectedDropDownListIndex("VALUE", ddlDistrictKh, cus.DISTRICT_EN);

            //bind commune
            BindCommune();
            Helper.SelectedDropDownListIndex("VALUE", ddlCommuneKh, cus.COMMUNE_EN);

            //bind village
            BindVillage();
            Helper.SelectedDropDownListIndex("VALUE", ddlVillageKh, cus.VILLAGE_EN);

            #endregion customer information

            #region insurance product

            Helper.SelectedDropDownListIndex("TEXT", ddlPackage, appin.PACKAGE);
            Helper.SelectedDropDownListIndex("VALUE", ddlBasicSA, appin.SUM_ASSURE + "");
            Helper.SelectedDropDownListIndex("VALUE", ddlRiderSumAssure, appRider.SUM_ASSURE + "");

            ddlPackage_SelectedIndexChanged(null, null);


            #endregion insurance product

            #region rider
            //bl_micro_policy_rider rider = da_micro_policy_rider.GetRider(objPol.POLICY_ID, userName);


            //txtRiderProduct.Text = rider.PRODUCT_ID;
            //txtRiderPremium.Text = rider.PREMIUM + "";
            //txtRiderAnnualPremium.Text = rider.ANNUAL_PREMIUM + "";
            //txtRiderSumAssure.Text = rider.SUM_ASSURE + "";
            //txtRiderDiscount.Text = rider.DISCOUNT_AMOUNT + "";
            //txtRiderAfterDiscount.Text = rider.TOTAL_AMOUNT + "";

            #endregion rider

            //txtTotalPremium.Text = (Convert.ToDouble(txtPremium.Text) + Convert.ToDouble(txtRiderPremium.Text)) + "";
            //txtTotalDiscountAmount.Text = (Convert.ToDouble(txtBasicDiscount.Text) + Convert.ToDouble(txtRiderDiscount.Text)) + "";
            //txtTotalPremiumAfterDiscount.Text = (Convert.ToDouble(txtBasicAfterDiscount.Text) + Convert.ToDouble(txtRiderAfterDiscount.Text)) + "";

            #region issue policy
            txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            #endregion issue policy

            #region beneficiary
            bl_micro_policy_beneficiary ben = da_micro_policy_beneficiary.GetBeneficiary(objPol.POLICY_ID);
            txtBenAddress.Text = ben.ADDRESS;

            txtFullName.Text = ben.FULL_NAME;

            Helper.SelectedDropDownListIndex("TEXT", ddlRelation, ben.RELATION);

            txtPercentageOfShare.Text = ben.PERCENTAGE_OF_SHARE + "";
            txtAge.Text = ben.AGE;

            #endregion beneficiary
            #region question
            bl_micro_application_questionaire que = da_micro_application_questionaire.GetQuestionaire(app.APPLICATION_NUMBER);

            Helper.SelectedDropDownListIndex("Value", ddlAnswer, que.ANSWER + "");

            txtAnswerRemarks.Text = que.REMARKS;

            #endregion
            btnIssue.Attributes.Add("disabled", "disabled");
            txtIssueDate.Attributes.Add("disabled", "disabled");
            txtUserPremium.Attributes.Add("disabled", "disabled");
            txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
            ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
            ibtnPrintApplication.Attributes.Add("disabled", "disabled");

        }
        else
        {

        }

    }

    void DisabledAllControls()
    {
        DisabledControls();

        txtApplicationDate.Attributes.Add("disabled", "disabled");

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
        txtApplicationDate.Attributes.Add("disabled", "disabled");
        txtReferrerId.Attributes.Add("disabled", "disabled");
        txtReferrerName.Attributes.Add("disabled", "disabled");
        txtBranchName.Attributes.Add("disabled", "disabled");
        txtBranchCode.Attributes.Add("disabled", "disabled");
        txtSaleAgentID.Attributes.Add("disabled", "disabled");
        txtSaleAgentName.Attributes.Add("disabled", "disabled");
        txtPolicyNumber.Attributes.Add("disabled", "disabled");
        txtPolicyStatus.Attributes.Add("disabled", "disabled");
        txtReletedCertificate.Attributes.Add("disabled", "disabled");

        //customer information
        txtCustomerAge.Attributes.Add("disabled", "disabled");
        //Address
        txtProvinceEn.Visible = false;
        txtDistrictEn.Visible = false;
        txtCommuneEn.Visible = false;
        txtVillageEn.Visible = false;
        //Insurance product and detail
        txtProductName.Attributes.Add("disabled", "disabled");
        txtTermOfCover.Attributes.Add("disabled", "disabled");
        txtPremiumPaymentPeriod.Attributes.Add("disabled", "disabled");
        ddlBasicSA.Attributes.Add("disabled", "disabled");
        ddlPaymentMode.Attributes.Add("disabled", "disabled");
        txtPremium.Attributes.Add("disabled", "disabled");
        txtAnnualPremium.Attributes.Add("disabled", "disabled");
        txtBasicDiscount.Attributes.Add("disabled", "disabled");
        txtBasicAfterDiscount.Attributes.Add("disabled", "disabled");
        txtRiderProduct.Attributes.Add("disabled", "disabled");
        txtRiderPremium.Attributes.Add("disabled", "disabled");
        txtRiderAnnualPremium.Attributes.Add("disabled", "disabled");
        ddlRiderSumAssure.Attributes.Add("disabled", "disabled");
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


    void CalculatePremium()
    {
        bl_micro_product_rate basic;
        bl_micro_product_rider_rate rider;
        string productID = hdfProductID.Value;
        string productRiderID = hdfRiderProductID.Value;
        int gender = Convert.ToInt32(ddlGender.SelectedValue);
        int age = Convert.ToInt32(txtCustomerAge.Text == "" ? "0" : txtCustomerAge.Text.Trim());
        double sumAssure = Convert.ToDouble(ddlBasicSA.SelectedValue);
        double sumAssureRider = Convert.ToDouble(ddlRiderSumAssure.SelectedValue);
        bool valid = false;
        string message = "";
        int paymentMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
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

        valid = (bool)ViewState["IS_VALID"];
        message = ViewState["MESSAGE"] + "";
        if (valid && ddlPackage.SelectedIndex > 0 && gender > -1 && age > 0 && sumAssure > 0 && sumAssureRider > 0)
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

            basic = da_micro_product_rate.GetProductRate(productID, gender, age, sumAssure, paymentMode);
            DateTime appDate = Helper.FormatDateTime(txtApplicationDate.Text.Trim());
            if (basic.PRODUCT_ID != null)
            {
                txtPremium.Text = basic.RATE + "";
                txtAnnualPremium.Text = basic.RATE + "";



                rider = da_micro_product_rider_rate.GetProductRate(productRiderID, gender, age, sumAssureRider, paymentMode);
                if (rider.PRODUCT_ID != null)
                {
                    txtRiderPremium.Text = rider.RATE + "";
                    txtRiderAnnualPremium.Text = rider.RATE + "";
                }
                else
                {
                    txtRiderPremium.Text = "";
                    txtRiderAnnualPremium.Text = "";
                }
                #region//check discount

                bl_micro_product_discount_config proConf = new bl_micro_product_discount_config();

                proConf = da_micro_product_config.DiscountConfig.GetProductDiscount(productID, productRiderID, sumAssure, sumAssureRider,ddlClientType.SelectedValue);
                //check expiry date of discount
                // DateTime appDate = Helper.FormatDateTime(txtApplicationDate.Text.Trim());
                if (proConf.ProductID != null)
                {
                    if (proConf.Status && appDate <= proConf.ExpiryDate)
                    {
                        basicDiscount = proConf.BasicDiscountAmount;
                        riderDiscount = proConf.RiderDiscountAmount;
                    }
                }
                #endregion
            }
            else
            {
                txtPremium.Text = "";
                txtAnnualPremium.Text = "";

            }

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

    protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
    {

        bool valid = false;
        string message = "";

        
        if (ddlPackage.SelectedIndex > 0)
        {

            ProductConfig = da_micro_product_config.ProductConfig.GetProductMicroProduct(ddlPackage.SelectedValue);
            BindBasicSA(ProductConfig.BasicSumAssuredRange);
            BindRiderSA(ProductConfig.RiderSumAssuredRange);
            BindPayMode(ProductConfig.PayMode);

            ValidateCalculateAge();
            valid = (bool)ViewState["IS_VALID"];
            message = ViewState["MESSAGE"] + "";

            if (valid)
            {
                CalculatePremium();
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
        valid = (bool)ViewState["IS_VALID"];
        message = ViewState["MESSAGE"] + "";
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
        ddlBasicSA.SelectedIndex=0;
        txtPremium.Text = "";
        txtAnnualPremium.Text = "";
        txtBasicDiscount.Text = "";
        txtBasicAfterDiscount.Text = "";
        ddlPaymentMode.SelectedIndex = 0;

        //Rider
        txtRiderProduct.Text = "";
        txtRiderPremium.Text = "";
        txtRiderAnnualPremium.Text = "";
        ddlRiderSumAssure.SelectedIndex = 0;
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
       
        if (ProductConfig.AllowRefer)
        {
            if (txtReferrerId.Text.Trim() == "")
            {
                isValid = false;
                message = "Referral Id is required.";
            }
            else
            {
                isValid = true;
                message = "";
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
        //else if (txtCurrentAddress.Text.Trim() == "")
        //{
        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "Current Residential Address is required.";
        //}
        //else if (txtVillage.Text.Trim() == "")
        //{
        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "Village is required.";
        //}
        //else if (txtCommune.Text.Trim() == "")
        //{
        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "Commune is required.";
        //}
        //else if (txtDistrict.Text.Trim() == "")
        //{
        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "District is required.";
        //}
        //else if (txtProvince.Text.Trim() == "")
        //{
        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "Provice is required.";
        //}
        //else if (txtProvinceEn.Text.Trim() == "")
        //{
        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "Provice In English is required.";
        //}
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
        //else if (txtPhoneNumber.Text.Trim() != "" && !Helper.IsNumber(txtPhoneNumber.Text.Trim()))
        //{

        //    my_session.IS_VALID = false;
        //    my_session.MESSAGE = "Phone number is allowed only number.";

        //}

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
        else if (ddlPackage.SelectedIndex == 0)
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
        else if (ddlBasicSA.SelectedIndex==0)
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

        if (ProductConfig.IsRequiredRider)
        {
            if (txtRiderProduct.Text.Trim() == "")
            {
                isValid = false;
                message = "Rider Product is required.";
            }
            else if (ddlRiderSumAssure.SelectedIndex==0)
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
        
        #endregion
        if (txtBenAddress.Text.Trim() == "")
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
        ViewState["IS_VALID"] = isValid;
        ViewState["MESSAGE"] = message;

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
                bl_product product = new bl_product();
                product = (bl_product)Session["PRODUCT"];
                if (product.Product_ID != null)
                {
                    if (age >= product.Age_Min && age <= product.Age_Max)
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

        ViewState["IS_VALID"] = isValid;
        ViewState["MESSAGE"] = message;

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
        ViewState["IS_VALID"] = isValid;
        ViewState["MESSAGE"] = message;

    }
    void ValidateIssuePolicy()
    {
        ValidateForm();
        bool isValid = true;
        string message = "";
        isValid = (bool)ViewState["IS_VALID"];
        if (isValid)
        {

            if (txtIssueDate.Text.Trim() == "")
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
            ViewState["IS_VALID"] = isValid;
            ViewState["MESSAGE"] = message;
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
            System.Threading.Thread.Sleep(1000);

            ValidateForm();
            string message = "";
            bool isValid = false;
            message = ViewState["MESSAGE"] + "";
            isValid = (bool)ViewState["IS_VALID"];
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

                //string agentName = ViewState["AGENT_NAME"] + "";

                provinceCode = ddlProvinceKh.SelectedValue;
                districtCode = ddlDistrictKh.SelectedValue;
                communeCode = ddlCommuneKh.SelectedValue;
                villageCode = ddlVillageKh.SelectedValue;

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
                app_cus.OCCUPATION = ddlOccupation.SelectedValue;

                app_cus.HOUSE_NO_KH = txtHouseNoKh.Text.Trim();
                app_cus.STREET_NO_KH = txtStreetKh.Text.Trim();
                app_cus.VILLAGE_KH = villageCode;
                app_cus.COMMUNE_KH = communeCode;
                app_cus.DISTRICT_KH = districtCode;

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
                #region check and count policy
                bl_micro_customer1 cus = da_micro_customer.GetCustomerByIdNumber(Convert.ToInt32(app_cus.ID_TYPE), app_cus.ID_NUMBER, userName);
                if (cus.ID_NUMBER != null && cus.ID_NUMBER != "")
                {
                    if (!AppConfiguration.AllowMultiRepaymentPolicyPerLife())
                    {
                        if (da_micro_policy.CountPolicy(cus.ID, true, userName) > 0)
                        {
                            Helper.Alert(true, "System is not allowed to issue multi policies per life.", lblError);
                            return;
                        }
                    }
                }
                #endregion check and count policy

                #region application information

                bl_micro_application app = new bl_micro_application();

                app.APPLICATION_DATE = Helper.FormatDateTime(txtApplicationDate.Text);
                app.CHANNEL_ID = channel_id;
                app.CHANNEL_ITEM_ID = hdfChannelItemID.Value;
                app.CHANNEL_LOCATION_ID = hdfChannelLocationID.Value;
                app.SALE_AGENT_ID = txtSaleAgentID.Text;

                app.CREATED_BY = userName;
                app.CREATED_ON = DateTime.Now;
                app.REMARKS = "";

                #endregion

                #region Insurance product
                bl_micro_application_insurance app_insurance = new bl_micro_application_insurance();
                app_insurance.PRODUCT_ID = hdfProductID.Value;
                app_insurance.TERME_OF_COVER = Convert.ToInt32(txtTermOfCover.Text.Trim());
                app_insurance.PAYMENT_PERIOD = Convert.ToInt32(txtPremiumPaymentPeriod.Text.Trim());
                app_insurance.SUM_ASSURE = Convert.ToDouble(ddlBasicSA.SelectedValue);
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
                app_insurance_rider.PRODUCT_ID = hdfRiderProductID.Value;
                app_insurance_rider.SUM_ASSURE = Convert.ToDouble(ddlRiderSumAssure.SelectedValue);
                app_insurance_rider.PREMIUM = Convert.ToDouble(txtRiderPremium.Text.Trim());
                app_insurance_rider.ANNUAL_PREMIUM = Convert.ToDouble(txtRiderAnnualPremium.Text.Trim());
                app_insurance_rider.DISCOUNT_AMOUNT = Convert.ToDouble(txtRiderDiscount.Text.Trim() == "" ? "0" : txtRiderDiscount.Text.Trim());
                app_insurance_rider.TOTAL_AMOUNT = Convert.ToDouble(txtRiderAfterDiscount.Text.Trim());
                app_insurance_rider.CREATED_BY = userName;
                app_insurance_rider.CREATED_ON = tran_date;
                #endregion

                #region Beneficiary
                bl_micro_application_beneficiary beneficiary = new bl_micro_application_beneficiary();

                beneficiary.FULL_NAME = txtFullName.Text.Trim();
                beneficiary.AGE = txtAge.Text.Trim();
                beneficiary.RELATION = ddlRelation.SelectedValue;// txtRelation.Text.Trim();
                beneficiary.PERCENTAGE_OF_SHARE = Convert.ToDouble(txtPercentageOfShare.Text.Trim());
                beneficiary.ADDRESS = txtBenAddress.Text.Trim();
                beneficiary.CREATED_BY = userName;
                beneficiary.CREATED_ON = tran_date;
                #endregion

                #region new application
                if (hdfApplicationNumber.Value == "")
                {
                    is_save_customer = da_micro_application_customer.SaveApplicationCustomer(app_cus);

                    if (is_save_customer)
                    {
                        app.APPLICATION_CUSTOMER_ID = app_cus.CUSTOMER_ID;
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
                        app.REFERRER_ID = txtReferrerId.Text;
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
                                is_save_insurance_rider = da_micro_application_insurance_rider.SaveApplicationInsuranceRider(app_insurance_rider);

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
                                            QUESTION_ID = hdfQuestionID.Value,
                                            APPLICATION_NUMBER = hdfApplicationNumber.Value,
                                            ANSWER = Convert.ToInt32(ddlAnswer.SelectedValue),
                                            ANSWER_REMARKS = txtAnswerRemarks.Text.Trim(),
                                            CREATED_ON = tran_date,
                                            CREATED_BY = userName// my_session.USER_NAME
                                        });
                                        if (is_save_question)
                                        {

                                            if (da_micro_policy_expiring.Update(app.APPLICATION_NUMBER, hdfOldPolicyId.Value, "", userName, tran_date, "", userName))
                                            {
                                                SESSION_PARA para = (SESSION_PARA)Session["SS_SESSION_PARA"];
                                                para.OldApplicationNumber = app.APPLICATION_NUMBER;
                                                Session["SS_SESSION_PARA"] = para;
                                                BindExistingApp();

                                                Helper.Alert(false, "Application is saved successfully. Application Number is <b>" + app.APPLICATION_NUMBER + "</b>", lblError);
                                            }
                                            else
                                            {
                                                Helper.Alert(true, "Application is saved fail.", lblError);
                                                RoleBack();
                                            }

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
                            ViewState["MESSAGE"] = "Save Application Information fail.";
                            RoleBack();

                        }
                    }
                    else
                    {

                        Helper.Alert(true, "Save Customer Information fail.", lblError);
                    }
                }
                #endregion new application
                else
                {
                    //update 
                    string errMessage = "";
                    DateTime updated_date = DateTime.Now;
                    app_cus.CUSTOMER_ID = hdfApplicationCustomerID.Value;// hdfCustomerID.Value;
                    app_cus.UPDATED_BY = userName;
                    app_cus.UPDATED_ON = updated_date;

                    is_save_customer = da_micro_application_customer.UpdateApplicationCustomer(app_cus);

                    app.UPDATED_BY = userName;
                    app.UPDATED_ON = updated_date;
                    app.APPLICATION_NUMBER = hdfApplicationNumber.Value;
                    app.APPLICATION_CUSTOMER_ID = hdfApplicationCustomerID.Value;// hdfCustomerID.Value;
                    app.REFERRER_ID = txtReferrerId.Text;
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
                            is_save_insurance_rider = da_micro_application_insurance_rider.UpdateApplicationInsuranceRider(app_insurance_rider);
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
                                    if (!is_save_question)
                                    {
                                        errMessage = "Update Application Questionaire: " + da_micro_application_questionaire.MESSAGE;
                                    }
                                }
                                else
                                {
                                    errMessage = "Update Application Beneficiary: " + da_micro_application_beneficiary.MESSAGE;
                                }

                            }
                            else//update rider fail
                            {
                                is_save_insurance_rider = false;
                                errMessage = " Update Application Rider: " + da_micro_application_insurance_rider.MESSAGE;
                            }
                        }
                        else//update insurance product fail
                        {
                            is_save_insurance = false;
                            errMessage = " Update Application Insurance: " + da_micro_application_insurance.MESSAGE;
                        }

                    }
                    else//update customer information fail
                    {
                        is_save_customer = false;
                        errMessage = " Update Application Customer: " + da_micro_application.MESSAGE;
                    }

                    if (is_save_customer && is_save_app && is_save_insurance && is_save_insurance_rider && is_save_beneficiary && is_save_question)
                    {
                        //load existing
                        BindExistingApp();
                        // Alert("Update successfully.");
                        Helper.Alert(false, "Application is updated successfully.", lblError);
                    }
                    else
                    {
                        // Alert("Update fail.");

                        if (da_micro_application.RestoreApplication(hdfApplicationNumber.Value, userName, Convert.ToDateTime(ViewState["VS_BAK_DATE"] + "")))
                        {

                            BindExistingApp();
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

        string provinceCode = "";
        string districtCode = "";
        string communeCode = "";
        string villageCode = "";

        string channel_id = "0152DF80-BA95-46A9-BB7A-E71966A34089";//corporate


        provinceCode = GetProvinceCode();
        districtCode = GetDisctrictCode();
        communeCode = GetCommuneCode();
        villageCode = GetVillageCode();

        try
        {
            System.Threading.Thread.Sleep(1000);
            string messageUpdate = "";
            if (UpdatedAppliction(out messageUpdate))
            {
                Helper.Alert(false, messageUpdate, lblError);
            }
            else
            {

                bl_micro_policy polExist = da_micro_policy.GetPolicyByApplicationID(hdfApplicationID.Value);
                // check existing policy why application id
                if (polExist.APPLICATION_ID == "" || polExist.APPLICATION_ID == null)
                {
                    #region Issued
                    ValidateIssuePolicy();
                    string message = "";
                    bool isValid = false;
                    message = ViewState["MESSAGE"] + "";
                    isValid = (bool)ViewState["IS_VALID"];
                    if (isValid)
                    {

                        #region save customer
                        bl_micro_customer1 cus = new bl_micro_customer1();
                        bl_micro_customer_prefix pre = new bl_micro_customer_prefix();

                        #region check exist customer
                        cus = new bl_micro_customer1();
                        cus = da_micro_customer.GetExistCustomer(txtFirstNameEng.Text.Trim(), txtSurnameEng.Text.Trim(), Helper.FormatDateTime(txtDateOfBirth.Text.Trim()));

                        #endregion check exist customer
                        if (cus.CUSTOMER_NUMBER != null)
                        {
                            hdfExistCustomerID.Value = cus.ID; ;
                            //check and count policy by customer id
                            if (AppConfiguration.AllowMultiRepaymentPolicyPerLife())
                            {
                                is_save_cus = true;
                            }
                            else//not allow multi policy per life
                            {
                                if (da_micro_policy.CountPolicy(cus.ID, true, userName) > 0)
                                {
                                    is_save_cus = false;
                                    Helper.Alert(true, "System is not allowed to issue multi policies per life.", lblError);
                                    return;
                                }
                                else
                                {
                                    is_save_cus = true;
                                }
                            }
                        }
                        else
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
                            pol.RenewFromPolicy = txtReletedCertificate.Text;
                            hdfPolicyID.Value = pol.POLICY_ID;

                            is_save_pol = da_micro_policy.SavePolicy(pol);
                        #endregion Save Policy
                            #region save policy detail
                            if (is_save_pol)
                            {
                                SESSION_PARA para = new SESSION_PARA();
                                para = (SESSION_PARA)Session["SS_SESSION_PARA"];
                                bl_micro_policy_detail p_detail = new bl_micro_policy_detail();
                                p_detail.POLICY_ID = pol.POLICY_ID;
                                p_detail.EFFECTIVE_DATE = para.OldPolicyStatus == "IF" ? para.PolicyNewEffectiveDate : Helper.FormatDateTime(txtIssueDate.Text.Trim());
                                p_detail.ISSUED_DATE = Helper.FormatDateTime(txtIssueDate.Text.Trim());
                                p_detail.MATURITY_DATE = p_detail.EFFECTIVE_DATE.AddYears(1);
                                p_detail.EXPIRY_DATE = p_detail.MATURITY_DATE.AddDays(-1);// p_detail.MATURITY_DATE.AddDays(-1).AddHours(23).AddMinutes(59);
                                p_detail.AGE = Convert.ToInt32(txtCustomerAge.Text.Trim());
                                p_detail.CURRANCY = "USD";
                                p_detail.SUM_ASSURE = Convert.ToDouble(ddlBasicSA.SelectedValue);
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
                                p_detail.POLICY_STATUS_REMARKS = "Repayment";
                                p_detail.CREATED_BY = userName;
                                p_detail.CREATED_ON = tran_date;

                                hdfPolicyDetailID.Value = p_detail.POLICY_DETAIL_ID;
                                is_save_pol_de = da_micro_policy_detail.SavePolicyDetail(p_detail);
                            #endregion Save policy detail
                                #region save rider
                                if (is_save_pol_de)
                                {

                                    bl_micro_policy_rider rider = new bl_micro_policy_rider();
                                    rider.POLICY_ID = pol.POLICY_ID;
                                    rider.PRODUCT_ID = hdfRiderProductID.Value;
                                    rider.SUM_ASSURE = Convert.ToDouble(ddlRiderSumAssure.SelectedValue);
                                    rider.PREMIUM = Convert.ToDouble(txtRiderPremium.Text.Trim());
                                    rider.ANNUAL_PREMIUM = Convert.ToDouble(txtRiderAnnualPremium.Text.Trim());
                                    rider.DISCOUNT_AMOUNT = Convert.ToDouble(txtRiderDiscount.Text.Trim() == "" ? "0" : txtRiderDiscount.Text.Trim());
                                    rider.TOTAL_AMOUNT = Convert.ToDouble(txtRiderAfterDiscount.Text.Trim());
                                    rider.CREATED_BY = userName;
                                    rider.CREATED_ON = tran_date;
                                    hdfPolicyRiderID.Value = rider.ID;
                                    is_save_pol_rider = da_micro_policy_rider.SaveRider(rider);
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
                                        pay.REFERRAL_FEE = pay.TOTAL_AMOUNT * 0.25;// pay.AMOUNT * 0.25;
                                        pay.REFERRAL_INCENTIVE = p_detail.SUM_ASSURE == 5000 ? 2.30 : 2;// new referral fee for package 2 is 2.30 effective from 01 jan 23
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
                                                        //delete record from expiring list
                                                        da_micro_policy_expiring.Delete(hdfOldPolicyId.Value);

                                                    #endregion update lead
                                                        #region send mail to uw
                                                        //if (da_micro_policy_expiring_status.SUCCESS)
                                                        //{



                                                        if (AppConfiguration.GetSendEmailOption())
                                                        {

                                                            #region V1
                                                            string mail = "";
                                                            // bl_customer_lead lead = da_customer_lead.GetCustomerLeadByApplicationNumber(hdfApplicationNumber.Value);
                                                            mail = "There is a new policy issued  </br>";
                                                            mail += "<table border='1'><th>Branch Code</th><th>Brand Name</th><th>Referral Staff ID</th><th>Referral Staff Name</th><th>Application Number</th><th>Certificate No.</th><th>Customer No.</th><th>Payment Reference No.</th><th>Client Name (English)</th><th>Client Name (Khmer)</th><th>Date of Birth</th><th>Phone Number</th><th>Occupation</th><th>Married Status</th><th>Gender</th><th>ID Type</th><th>ID Number</th><th>Village</th><th>Commune</th><th>District</th><th>Province</th><th>Issued Date</th><th>Effective Date</th><th>Maturity Date</th><th>SA</th><th>Permium</th><th>Package</th><th>Agent code</th><th>IA Name</th><th>Benificery Name</th><th>Benificery Age</th><th>Benificery Relation</th>";
                                                            mail += "<tr><td>" + txtBranchCode.Text + " </td><td> " + txtBranchName.Text + "</td><td>" + txtReferrerId.Text + "</td><td>" + txtReferrerName.Text + "</td><td>" + hdfApplicationNumber.Value + "</td><td>" + pol.POLICY_NUMBER + "</td><td>" + cus.CUSTOMER_NUMBER + "</td><td>" + pay.REFERANCE_TRANSACTION_CODE + "</td><td>" + cus.LAST_NAME_IN_ENGLISH + " " + cus.FIRST_NAME_IN_ENGLISH + "</td><td>" + cus.LAST_NAME_IN_KHMER + " " + cus.FIRST_NAME_IN_KHMER + "</td><td>" + cus.DATE_OF_BIRTH.ToString("dd-MM-yyyy") + "</td><td>" + cus.PHONE_NUMBER1 + "</td><td>" + cus.OCCUPATION + "</td><td>" + cus.MARITAL_STATUS + "</td><td>" + Helper.GetGenderText(Convert.ToInt32(cus.GENDER), true, true) + "</td><td>" + Helper.GetIDCardTypeText(Convert.ToInt32(cus.ID_TYPE)) + "</td><td>" + cus.ID_NUMBER + "</td><td>" +
                                                                ddlVillageKh.SelectedValue + "/" + ddlVillageKh.SelectedItem.Text + "</td><td>" + ddlCommuneKh.SelectedValue + "/" + ddlCommuneKh.SelectedItem.Text + "</td><td>" + ddlDistrictKh.SelectedValue + "/" + ddlDistrictKh.SelectedItem.Text + "</td><td>" + ddlProvinceKh.SelectedValue + "/" + ddlProvinceKh.SelectedItem.Text + "</td><td>" + p_detail.ISSUED_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.EFFECTIVE_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.MATURITY_DATE.ToString("dd-MM-yyyy") + "</td><td>" + p_detail.SUM_ASSURE + "</td><td>" + pay.TOTAL_AMOUNT + "</td><td>" + ddlPackage.SelectedItem.Text + "</td><td>" + pol.AGENT_CODE + "</td><td>" + da_sale_agent.GetSaleAgentName(pol.AGENT_CODE) + "</td><td>" + ben.FULL_NAME + "</td><td>" + ben.AGE + "</td><td>" + ben.RELATION + "</td></tr></table>";



                                                            if (SendEmail(mail))
                                                            {
                                                                Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b>", lblError);
                                                            }
                                                            else
                                                            {
                                                                Helper.Alert(false, "Issued Successfully. Certificate No. is <b>" + pol.POLICY_NUMBER + "</b>. Note: System cannot send email notification to UW department.", lblError);
                                                            }
                                                            #endregion V1
                                                        }


                                                        #endregion send email

                                                        da_micro_application.DeleteBackupApplication(hdfApplicationNumber.Value, userName, Convert.ToDateTime(ViewState["VS_BAK_DATE"] + ""));

                                                        BindExistingApp();
                                                        return;

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
            RoleBackIssuePolicy();
            Log.AddExceptionToLog("Error function [bntIssue_Click(object sender, EventArgs e)] in class [banca_micro_application.aspx.cs], detail: " + ex.Message + "=>" + ex.StackTrace);
            // RoleBackIssuePolicy();
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
        string url = "banca_micro_application_print.aspx?APP_ID=" + hdfApplicationID.Value;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);

    }
    protected void ibtnPrintCertificate_Click(object sender, ImageClickEventArgs e)
    {
        string url = "banca_micro_cert.aspx?P_ID=" + hdfPolicyID.Value;
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
                ddlProvinceKh.Items.Add(new ListItem(pro.Khmer, pro.Code));
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
                    ddlDistrictKh.Items.Add(new ListItem(dis.Khmer, dis.Code));
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
                    ddlCommuneKh.Items.Add(new ListItem(com.Khmer, com.Code));
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
                    ddlVillageKh.Items.Add(new ListItem(vi.Khmer, vi.Code));
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
        if (hdfOldApplicationDate.Value != txtApplicationDate.Text.Trim())
        {
            messageUpdate = "You have changed Application Date [" + hdfOldApplicationDate.Value + "] To [" + txtApplicationDate.Text + "].";
            result = true;
        }
        else if (hdfOldReferrerId.Value != txtReferrerId.Text.Trim())
        {
            messageUpdate = "You have changed Referral ID  [" + hdfOldReferrerId.Value + "] To [" + txtReferrerId.Text + "].";
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
        else if (hdfPackage.Value.Trim() != ddlPackage.SelectedItem.Text.Trim())
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
        messageUpdate += " Please save application before processing issue policy.";
        return result;
    }

    #region Existing Application
    void BindExistingApp()
    {

        //da_micro_application_customer.
        DataTable tbl_app = da_micro_application.GetApplicationDetailByApplicationID(hdfApplicationID.Value);
        bool isBackupApp = true;
        if (tbl_app.Rows.Count > 0)
        {

            #region check policy status

            bl_micro_policy relatedPol = da_micro_policy.GetPolicyByID(hdfOldPolicyId.Value);
            txtReletedCertificate.Text = relatedPol.POLICY_NUMBER;

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
                    break;
                }

                DisabledAllControls();
                #endregion issue policy

                btnIssue.Attributes.Add("disabled", "disabled");
                txtIssueDate.Attributes.Add("disabled", "disabled");
                txtUserPremium.Attributes.Add("disabled", "disabled");
                txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
                ibtnSave.Attributes.Add("disabled", "disabled");
                ibtnPrintCertificate.Attributes.Remove("disabled");
            }
            else
            {
                ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                //txtIssueDate.Attributes.Remove("disabled");
                txtUserPremium.Attributes.Remove("disabled");
                txtPaymentReferenceNo.Attributes.Remove("disabled");
                btnIssue.Attributes.Remove("disabled");
                ibtnSave.Attributes.Remove("disabled");
                txtPolicyNumber.Text = "";
                txtPolicyStatus.Text = "";
                //Backup applicaiton
                ViewState["VS_BAK_DATE"] = DateTime.Now;
                isBackupApp = da_micro_application.BackupApplication(hdfApplicationID.Value, "UPDATE", userName, Convert.ToDateTime(ViewState["VS_BAK_DATE"] + ""));
            }
            #endregion check policy status
            if (isBackupApp)
            {

                var app_row = tbl_app.Rows[0];

                txtApplicationNumber.Text = app_row["application_number"].ToString();
                txtApplicationDate.Text = Convert.ToDateTime(app_row["application_date"].ToString()).ToString("dd-MM-yyyy");
                hdfOldApplicationDate.Value = Convert.ToDateTime(app_row["application_date"].ToString()).ToString("dd-MM-yyyy");
                txtBranchCode.Text = app_row["office_code"].ToString();
                txtBranchName.Text = app_row["office_name"].ToString();
                txtSaleAgentID.Text = app_row["sale_agent_id"].ToString();
                txtSaleAgentName.Text = app_row["full_name"].ToString();


                ViewState["AGENT_CODE"] = txtSaleAgentID.Text;
                ViewState["AGENT_NAME"] = txtSaleAgentName.Text;

                hdfApplicationNumber.Value = app_row["application_number"].ToString();
                hdfChannelItemID.Value = app_row["channel_item_id"].ToString();
                hdfChannelLocationID.Value = app_row["channel_location_id"].ToString();
                hdfSaleAgentID.Value = app_row["sale_agent_id"].ToString();
                hdfApplicationID.Value = app_row["application_id"].ToString();

                // hdfCustomerID.Value = app_row["application_customer_id"].ToString();
                hdfApplicationCustomerID.Value = app_row["application_customer_id"].ToString();
                hdfApplicationNumber.Value = app_row["application_number"].ToString();


                txtReferrerId.Text = app_row["referrer_id"].ToString();
                hdfOldReferrerId.Value = app_row["referrer_id"].ToString();
                txtReferrerName.Text = app_row["referrer"].ToString();
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


                #endregion customer information

                #region insurance product

                Helper.SelectedDropDownListIndex("TEXT", ddlPackage, app_row["package"].ToString());
                hdfPackage.Value = ddlPackage.SelectedItem.Text;
                txtProductName.Text = app_row["product_name"].ToString();
                txtTermOfCover.Text = app_row["term_of_cover"].ToString();
                txtPremiumPaymentPeriod.Text = app_row["payment_period"].ToString();
                Helper.SelectedDropDownListIndex("VALUE",ddlBasicSA,  app_row["sum_assure"].ToString());
                Helper.SelectedDropDownListIndex("VALUE", ddlPaymentMode, app_row["pay_mode"].ToString());
                txtPremium.Text = app_row["premium"].ToString();
                txtAnnualPremium.Text = app_row["annual_premium"].ToString();

                txtBasicDiscount.Text = app_row["discount_amount"].ToString();
                txtBasicAfterDiscount.Text = app_row["total_amount"].ToString();


                hdfProductID.Value = app_row["product_id"].ToString();
                #endregion insurance product

                #region rider
                hdfRiderProductID.Value = app_row["rider_product_id"].ToString();
                txtRiderProduct.Text = app_row["rider_product_name"].ToString();
                txtRiderPremium.Text = app_row["rider_premium"].ToString();
                txtRiderAnnualPremium.Text = app_row["rider_annual_premium"].ToString();
                Helper.SelectedDropDownListIndex("VALUE", ddlRiderSumAssure, app_row["rider_sum_assure"].ToString());

                txtRiderDiscount.Text = app_row["rider_discount_amount"].ToString();
                txtRiderAfterDiscount.Text = app_row["rider_total_amount"].ToString();

                #endregion rider

                txtTotalPremium.Text = (Convert.ToDouble(txtPremium.Text) + Convert.ToDouble(txtRiderPremium.Text)) + "";
                txtTotalDiscountAmount.Text = (Convert.ToDouble(txtBasicDiscount.Text) + Convert.ToDouble(txtRiderDiscount.Text)) + "";
                txtTotalPremiumAfterDiscount.Text = (Convert.ToDouble(txtBasicAfterDiscount.Text) + Convert.ToDouble(txtRiderAfterDiscount.Text)) + "";

                #region issue policy
                txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
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
                //txtIssueDate.Attributes.Remove("disabled");
                //txtUserPremium.Attributes.Remove("disabled");
                //txtPaymentReferenceNo.Attributes.Remove("disabled");
                //btnIssue.Attributes.Remove("disabled");

                // txtIssueDate.Text = txtApplicationDate.Text.Trim();

                ibtnPrintApplication.Attributes.Remove("disabled");

            }
            else
            {
                ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                DisabledAllControls();
                Helper.Alert(true, "System is error while try to backup application", lblError);
            }
        }
        else
        {
            btnIssue.Attributes.Add("disabled", "disabled");
            txtIssueDate.Attributes.Add("disabled", "disabled");
            txtUserPremium.Attributes.Add("disabled", "disabled");
            txtPaymentReferenceNo.Attributes.Add("disabled", "disabled");
            ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
            ibtnPrintApplication.Attributes.Add("disabled", "disabled");

        }

    }
    #endregion Existing Application
    protected void ddlBasicSA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlRiderSumAssure_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
