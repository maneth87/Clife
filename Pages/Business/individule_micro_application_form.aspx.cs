using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.IO;
using System.Web.Services;

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public partial class Pages_Business_MiroIndviduleApp_individule_micro_application_form : System.Web.UI.Page
{
   
    string userName = "";

    private bl_micro_product_config ProductConfig
    {
        get { return (bl_micro_product_config)this.ViewState["PRO_CONIF"]; }
        set { this.ViewState["PRO_CONIF"] = value; }
    }

    private bl_sys_user_role UserRole
    {
        get { return (bl_sys_user_role)this.ViewState["V_USER_ROLE"]; }
        set { this.ViewState["V_USER_ROLE"] = value; }
    }

    private int _MaxPolicyPerLife
    {
        get { return (int)this.ViewState["V_MAX_POLICY_PER_LIFE"]; }
        set { this.ViewState["V_MAX_POLICY_PER_LIFE"] = value; }
    }

    private bl_micro_product_rider ProductRider
    {
        get { return (bl_micro_product_rider)this.ViewState["PRO_RIDER"]; }
        set { this.ViewState["PRO_RIDER"] = value; }
    }

    private List<bl_micro_application_beneficiary> BeneficiaryList
    {
        get { return (List<bl_micro_application_beneficiary>)this.ViewState["BEN_LIST"]; }
        set { this.ViewState["BEN_LIST"] = value; }
    }

    private int _BenRowIndex
    {
        get { return (int)this.ViewState["V_BEN_INDEX"]; }
        set { this.ViewState["V_BEN_INDEX"] = value; }
    }

    private double _BenPercentage
    {
        get { return (double)this.ViewState["V_BEN_PER"]; }
        set { this.ViewState["V_BEN_PER"] = value; }
    }

    private List<Pages_Business_MiroIndviduleApp_individule_micro_application_form.SumAssurePremiums> _SumAssurePremium
    {
        get
        {
            return (List<Pages_Business_MiroIndviduleApp_individule_micro_application_form.SumAssurePremiums>)this.ViewState["V_SA_PREM"];
        }
        set { this.ViewState["V_SA_PREM"] = value; }
    }
    private List<bl_micro_policy.SavedIssuePolicy> _SavedIssuePolicyList
    {
        get
        {
            return (List<bl_micro_policy.SavedIssuePolicy>)this.ViewState["V_POL_SAVE_LIST"];
        }
        set { this.ViewState["V_POL_SAVE_LIST"] = value; }
    }

    [WebMethod]
    public void SaveActivity(
      bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity,
      string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(this.UserRole.UserName, this.UserRole.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblError.Text = "";
        this.userName = Membership.GetUser().UserName;
        string fileName = Path.GetFileName(this.Request.Url.AbsolutePath);
        List<bl_sys_user_role> UserRole = (List<bl_sys_user_role>)this.Session["SS_UR_LIST"];
        if (UserRole != null)
        {
            bl_sys_user_role sysUserRole = new bl_sys_user_role().GetSysUserRole(UserRole, fileName);
            if (sysUserRole.UserName != null)
                this.UserRole = sysUserRole;
            if (this.Page.IsPostBack)
                return;

            this.BeneficiaryList = new List<bl_micro_application_beneficiary>();
            this._BenPercentage = 0.0;
            this._MaxPolicyPerLife = 0;
            this._SumAssurePremium = new List<Pages_Business_MiroIndviduleApp_individule_micro_application_form.SumAssurePremiums>();
            this._SavedIssuePolicyList = new List<bl_micro_policy.SavedIssuePolicy>();
            this.Initialize();
        }
        else
            this.Response.Redirect("../../unauthorize.aspx");
    }

    private void BindProvince()
    {
        List<bl_address.province> provinces = da_address.province.GetProvince();
        this.ddlProvinceKh.Items.Clear();
        this.ddlProvinceKh.Items.Add(new ListItem("--ខេត្ត/ក្រុង--", ""));
        if (provinces.Count <= 0)
            return;

        foreach (var province in provinces)
            this.ddlProvinceKh.Items.Add(new ListItem(province.Khmer, province.Code + "/" + province.English));
    }

    private void BindDistrict()
    {
        if (!(this.ddlProvinceKh.SelectedValue != ""))
            return;
        List<bl_address.district> districts = da_address.district.GetDistrict(this.GetProvinceCode());
        this.ddlDistrictKh.Items.Clear();
        this.ddlDistrictKh.Items.Add(new ListItem("--ស្រុក/ខណ្ឌ--", ""));
        if (districts.Count <= 0)
            return;
        foreach (var district in districts)
            this.ddlDistrictKh.Items.Add(new ListItem(district.Khmer, district.Code + "/" + district.English));
    }

    private void BindCommune()
    {
        if (!(this.ddlDistrictKh.SelectedValue != ""))
            return;
        List<bl_address.commune> commune1 = da_address.commune.GetCommune(this.GetDisctrictCode());
        this.ddlCommuneKh.Items.Clear();
        this.ddlCommuneKh.Items.Add(new ListItem("--ឃុំ/សង្កាត់--", ""));
        if (commune1.Count <= 0)
            return;
        foreach (bl_address.commune commune2 in commune1)
            this.ddlCommuneKh.Items.Add(new ListItem(commune2.Khmer, commune2.Code + "/" + commune2.English));
    }

    private void BindVillage()
    {
        if (!(this.ddlCommuneKh.SelectedValue != ""))
            return;
        List<bl_address.village> village1 = da_address.village.GetVillage(this.GetCommuneCode());
        this.ddlVillageKh.Items.Clear();
        this.ddlVillageKh.Items.Add(new ListItem("--ភូមិ--", ""));
        if (village1.Count <= 0)
            return;
        foreach (bl_address.village village2 in village1)
            this.ddlVillageKh.Items.Add(new ListItem(village2.Khmer, village2.Code + "/" + village2.English));
    }

    private string GetProvinceCode()
    {
        string provinceCode = "";
        string[] strArray = this.ddlProvinceKh.SelectedValue.Split('/');
        if (strArray.Length > 0)
            provinceCode = strArray[0].ToString();
        return provinceCode;
    }

    private string GetDisctrictCode()
    {
        string disctrictCode = "";
        string[] strArray = this.ddlDistrictKh.SelectedValue.Split('/');
        if (strArray.Length > 0)
            disctrictCode = strArray[0].ToString();
        return disctrictCode;
    }

    private string GetCommuneCode()
    {
        string communeCode = "";
        string[] strArray = this.ddlCommuneKh.SelectedValue.Split('/');
        if (strArray.Length > 0)
            communeCode = strArray[0].ToString();
        return communeCode;
    }

    private string GetVillageCode()
    {
        string villageCode = "";
        string[] strArray = this.ddlVillageKh.SelectedValue.Split('/');
        if (strArray.Length > 0)
            villageCode = strArray[0].ToString();
        return villageCode;
    }


    protected void btnIssue_Click(object sender, EventArgs e)
    {
        DateTime tranDate = DateTime.Now;
        try
        {
            this.ValidateIssuePolicy();
            bool flag = (bool)this.ViewState["IS_VALID"];
            string message = string.Concat(this.ViewState["MESSAGE"]);
            if (flag)
            {
                //bl_micro_application.bl_application_for_issue applicationForIssue = new bl_micro_application.bl_application_for_issue();
               // object obj1 = new object();
                int loopStep = 0;
                DateTime newEffectiveDate = new DateTime(1900, 1, 1);
                DateTime effectiveDate = Helper.FormatDateTime(this.txtEffectiveDate.Text.Trim());
                DateTime issueDate = Helper.FormatDateTime(this.txtIssueDate.Text.Trim());
                DateTime maturityDate = new DateTime(1900, 1, 1);
                DateTime expiryDate = new DateTime(1900, 1, 1);
                double collectedPremium = Convert.ToDouble(this.txtUserPremium.Text.Trim());
                string paymentRefNo = this.txtPaymentRefNo.Text.Trim();
                bl_micro_customer1 CUSTOMER = new bl_micro_customer1();
                //List<bl_micro_policy.SavedIssuePolicy> savedIssuePolicyList = new List<bl_micro_policy.SavedIssuePolicy>();
                bl_micro_product_config proConfig = new bl_micro_product_config();
                bool flagSubmit = false;
                bool isExistCust = false;
                bool isBreakAll = false;    
                string cusNoFormat = "";
                string cusType = "";
                string polNoFormat = "";

                int cusAge = 0;
                 
                foreach (bl_micro_application.ApplicationFilter applicationFilter in da_micro_application.GetApplicationNumberMainSub(this.hdfApplicationNumber.Value))
                {
                    bl_micro_application.bl_application_for_issue applicationForIssuePolicy = da_micro_application.GetApplicationForIssuePolicy(applicationFilter.ApplicationNumber);
                   // object obj2 = new object();
                    if (applicationForIssuePolicy != null)
                    {
                        if (loopStep == 0)/*flag to save customer only one time since it's same customer*/
                        {
                            proConfig = da_micro_product_config.ProductConfig.GetProductMicroProduct(applicationForIssuePolicy.Insurance.PRODUCT_ID);
                            bl_micro_customer1 existCusObj = da_micro_customer.GetCustomerByIdNumber(Convert.ToInt32(applicationForIssuePolicy.Customer.ID_TYPE), applicationForIssuePolicy.Customer.ID_NUMBER);
                            if (existCusObj.CUSTOMER_NUMBER != null) /*skip saving existing customer*/
                            {
                                
                                CUSTOMER = existCusObj;
                                isExistCust = true;
                                flagSubmit = true;
                            }
                            else
                            {
                                #region Customer
                                isExistCust = false;
                                bl_micro_customer_prefix lastCustomerPrefix = da_micro_customer_prefix.GetLastCustomerPrefix();
                                CUSTOMER = new bl_micro_customer1();
                                CUSTOMER.SEQ = !(lastCustomerPrefix.PREFIX2 == CUSTOMER.LAST_PREFIX) ? 1 : CUSTOMER.LAST_SEQ + 1;

                                if (this.ddlChannel.SelectedItem.Text.Trim().ToUpper() == bl_channel.CHANNEL_NAME.INDIVIDUAL.ToString())
                                    cusType = bl_channel.CHANNEL_NAME.INDIVIDUAL.ToString();
                                else if (this.ddlChannel.SelectedItem.Text.Trim().ToUpper() == bl_channel.CHANNEL_NAME.CORPORATE.ToString())
                                    cusType = bl_channel.CHANNEL_NAME.CORPORATE.ToString();
                                cusNoFormat = lastCustomerPrefix.PREFIX1 + lastCustomerPrefix.PREFIX2 + CUSTOMER.SEQ.ToString(lastCustomerPrefix.DIGITS);
                                CUSTOMER.CUSTOMER_NUMBER = cusNoFormat;
                                CUSTOMER.CUSTOMER_TYPE = cusType;
                                CUSTOMER.ID_TYPE = string.IsNullOrWhiteSpace(applicationForIssuePolicy.Customer.ID_TYPE) ? "" : applicationForIssuePolicy.Customer.ID_TYPE;
                                CUSTOMER.ID_NUMBER = applicationForIssuePolicy.Customer.ID_NUMBER;
                                CUSTOMER.FIRST_NAME_IN_ENGLISH = applicationForIssuePolicy.Customer.FIRST_NAME_IN_ENGLISH;
                                CUSTOMER.LAST_NAME_IN_ENGLISH = applicationForIssuePolicy.Customer.LAST_NAME_IN_ENGLISH;
                                CUSTOMER.FIRST_NAME_IN_KHMER = applicationForIssuePolicy.Customer.FIRST_NAME_IN_KHMER;
                                CUSTOMER.LAST_NAME_IN_KHMER = applicationForIssuePolicy.Customer.LAST_NAME_IN_KHMER;
                                CUSTOMER.GENDER = string.IsNullOrWhiteSpace(applicationForIssuePolicy.Customer.GENDER) ? "" : applicationForIssuePolicy.Customer.GENDER;
                                CUSTOMER.DATE_OF_BIRTH = applicationForIssuePolicy.Customer.DATE_OF_BIRTH;
                                CUSTOMER.NATIONALITY = applicationForIssuePolicy.Customer.NATIONALITY;
                                CUSTOMER.MARITAL_STATUS = applicationForIssuePolicy.Customer.MARITAL_STATUS;
                                CUSTOMER.OCCUPATION = applicationForIssuePolicy.Customer.OCCUPATION;
                                CUSTOMER.HOUSE_NO_KH = applicationForIssuePolicy.Customer.HOUSE_NO_EN;
                                CUSTOMER.STREET_NO_KH = applicationForIssuePolicy.Customer.STREET_NO_EN;
                                CUSTOMER.VILLAGE_KH = applicationForIssuePolicy.Customer.VILLAGE_EN;
                                CUSTOMER.COMMUNE_KH = applicationForIssuePolicy.Customer.COMMUNE_EN;
                                CUSTOMER.DISTRICT_KH = applicationForIssuePolicy.Customer.DISTRICT_EN;
                                CUSTOMER.PROVINCE_KH = applicationForIssuePolicy.Customer.PROVINCE_EN;
                                CUSTOMER.HOUSE_NO_EN = applicationForIssuePolicy.Customer.HOUSE_NO_EN;
                                CUSTOMER.STREET_NO_EN = applicationForIssuePolicy.Customer.STREET_NO_EN;
                                CUSTOMER.VILLAGE_EN = applicationForIssuePolicy.Customer.VILLAGE_EN;
                                CUSTOMER.COMMUNE_EN = applicationForIssuePolicy.Customer.COMMUNE_EN;
                                CUSTOMER.DISTRICT_EN = applicationForIssuePolicy.Customer.DISTRICT_EN;
                                CUSTOMER.PROVINCE_EN = applicationForIssuePolicy.Customer.PROVINCE_EN;
                                CUSTOMER.PHONE_NUMBER1 = applicationForIssuePolicy.Customer.PHONE_NUMBER1;
                                CUSTOMER.EMAIL1 = applicationForIssuePolicy.Customer.EMAIL1;
                                CUSTOMER.CREATED_BY = this.userName;
                                CUSTOMER.CREATED_ON = tranDate;
                                CUSTOMER.STATUS = 1;
                                flagSubmit = da_micro_customer.SaveCustomer(CUSTOMER);
                                #endregion Customer
                            }
                        }
                        else /*skip saving same customer*/
                            flagSubmit = true;

                        if (string.IsNullOrWhiteSpace(applicationForIssuePolicy.PolicyNumber))
                        {
                            bl_micro_policy POLICY = new bl_micro_policy(applicationForIssuePolicy.Insurance.PRODUCT_ID);
                            bl_micro_policy_detail polDetail = new bl_micro_policy_detail();
                            bl_micro_policy_rider polRider = new bl_micro_policy_rider();
                            bl_micro_policy_payment polPayment = new bl_micro_policy_payment();
                            bl_micro_policy_address polAddress = new bl_micro_policy_address();
                            bl_micro_policy_prefix lastPolicyPrefix = da_micro_policy_prefix.GetLastPolicyPrefix(POLICY.PRODUCT_ID);
                            bl_micro_application_beneficiary.PrimaryBeneciary primaryBeneciary = applicationForIssuePolicy.PrimaryBeneciary;

                            bool isMatched = lastPolicyPrefix.PRODUCT_ID.Contains(POLICY.PRODUCT_ID);
                            if (!isMatched)
                            {
                                flagSubmit = false;
                                isBreakAll = true;
                                this.ViewState["MESSAGE"] = "Policy Prefix Number Is Not Matched.";
                            }

                            if (flagSubmit)
                            {
                                #region Policy
                               
                                POLICY.SEQ = !(POLICY.LAST_PREFIX == lastPolicyPrefix.PREFIX2) || !(POLICY.LAST_PREFIX1.ToUpper() == lastPolicyPrefix.PREFIX1) ? 1 : POLICY.LAST_SEQ + 1;

                                polNoFormat= lastPolicyPrefix.PREFIX1 + lastPolicyPrefix.PREFIX2 + POLICY.SEQ.ToString(lastPolicyPrefix.DIGITS);
                                POLICY.POLICY_NUMBER = polNoFormat;
                                POLICY.POLICY_TYPE = proConfig.BusinessType.ToUpper() == bl_micro_product_config.BusinussTypeOption.BANCA_COOPERATE.ToString() ? "COR" : "IND";
                                POLICY.APPLICATION_ID = applicationForIssuePolicy.Application.APPLICATION_ID;
                                POLICY.CUSTOMER_ID = CUSTOMER.ID;
                               // POLICY.PRODUCT_ID = applicationForIssuePolicy.Insurance.PRODUCT_ID;
                                POLICY.CHANNEL_ID = applicationForIssuePolicy.Application.CHANNEL_ID;
                                POLICY.CHANNEL_ITEM_ID = applicationForIssuePolicy.Application.CHANNEL_ITEM_ID;
                                POLICY.CHANNEL_LOCATION_ID = applicationForIssuePolicy.Application.CHANNEL_LOCATION_ID;
                                POLICY.AGENT_CODE = applicationForIssuePolicy.Application.SALE_AGENT_ID;
                                POLICY.CREATED_ON = tranDate;
                                POLICY.CREATED_BY = this.userName;
                                POLICY.POLICY_STATUS = "IF";
                                POLICY.RenewFromPolicy = applicationForIssuePolicy.Application.RENEW_FROM_POLICY;
                                flagSubmit = da_micro_policy.SavePolicy(POLICY);
                                #endregion policy

                                if (flagSubmit)
                                {
                                    bl_micro_policy.SavedIssuePolicy savedIssuePolicy = new bl_micro_policy.SavedIssuePolicy()
                                    {
                                        PolicyId = POLICY.POLICY_ID,
                                        PolicyNumber = POLICY.POLICY_NUMBER,
                                        ApplicationNumber = applicationForIssuePolicy.Application.APPLICATION_NUMBER,
                                        CustomerId = CUSTOMER.ID,
                                        IsExistingCustomer = isExistCust
                                    };
                                    _SavedIssuePolicyList.Add(savedIssuePolicy);

                                    #region policyDetail

                                    if (newEffectiveDate.Year == 1900)
                                    {
                                        if (applicationFilter.ApplicationType != Helper.ApplicationType.R)
                                        {
                                            newEffectiveDate = effectiveDate;
                                        }
                                        else
                                        {
                                            newEffectiveDate = effectiveDate.AddYears(1);

                                        }
                                    }
                                    else
                                    {
                                        newEffectiveDate = applicationFilter.ApplicationType == Helper.ApplicationType.R ? newEffectiveDate.AddYears(1) : effectiveDate;
                                    }

                                   // newEffectiveDate = newEffectiveDate.Year != 1900 ? (applicationFilter.ApplicationType == Helper.ApplicationType.R ? newEffectiveDate.AddYears(1) : effectiveDate) : (applicationFilter.ApplicationType == Helper.ApplicationType.R ? effectiveDate.AddYears(1) : effectiveDate);
                                    maturityDate = applicationForIssuePolicy.Insurance.COVER_TYPE != bl_micro_product_config.PERIOD_TYPE.Y ? (applicationForIssuePolicy.Insurance.COVER_TYPE != bl_micro_product_config.PERIOD_TYPE.M ? newEffectiveDate : newEffectiveDate.AddMonths(applicationForIssuePolicy.Insurance.TERME_OF_COVER)) : newEffectiveDate.AddYears(1);
                                    expiryDate = maturityDate.AddDays(-1.0);
                                   cusAge = Calculation.Culculate_Customer_Age(CUSTOMER.DATE_OF_BIRTH.ToString("dd/MM/yyyy"), applicationForIssuePolicy.Application.APPLICATION_DATE);
                                    polDetail = new bl_micro_policy_detail()
                                    {
                                        POLICY_ID = POLICY.POLICY_ID,
                                        EFFECTIVE_DATE = newEffectiveDate,
                                        ISSUED_DATE = issueDate,
                                        MATURITY_DATE = maturityDate,
                                        EXPIRY_DATE = expiryDate,
                                        PREMIUM = applicationForIssuePolicy.Insurance.PREMIUM,
                                        ANNUAL_PREMIUM = applicationForIssuePolicy.Insurance.ANNUAL_PREMIUM,
                                        DISCOUNT_AMOUNT = applicationForIssuePolicy.Insurance.DISCOUNT_AMOUNT,
                                        TOTAL_AMOUNT = applicationForIssuePolicy.Insurance.TOTAL_AMOUNT,
                                        COVER_YEAR = applicationForIssuePolicy.Insurance.TERME_OF_COVER,
                                        PAY_YEAR = applicationForIssuePolicy.Insurance.PAYMENT_PERIOD,
                                        PAYMENT_CODE = applicationForIssuePolicy.Insurance.PAYMENT_CODE,
                                        PAY_MODE = applicationForIssuePolicy.Insurance.PAY_MODE,
                                        AGE = cusAge,
                                        COVER_UP_TO_AGE = cusAge + applicationForIssuePolicy.Insurance.TERME_OF_COVER,
                                        PAY_UP_TO_AGE = cusAge + applicationForIssuePolicy.Insurance.PAYMENT_PERIOD,
                                        CURRANCY = "USD",
                                        REFERRAL_FEE = 0.0,
                                        REFERRAL_INCENTIVE = 0.0,
                                        SUM_ASSURE = applicationForIssuePolicy.Insurance.SUM_ASSURE,
                                        POLICY_STATUS_REMARKS = applicationForIssuePolicy.Application.CLIENT_TYPE == Helper.ClientTYpe.REPAYMENT.ToString() ? "REPAYMENT" : "NEW",
                                        CREATED_BY = this.userName,
                                        CREATED_ON = tranDate,
                                        COVER_TYPE = applicationForIssuePolicy.Insurance.COVER_TYPE
                                    };
                                    flagSubmit = da_micro_policy_detail.SavePolicyDetail(polDetail);
                                    #endregion policydetail
                                    if (flagSubmit)
                                    {
                                       
                                        #region rider
                                        if (string.IsNullOrWhiteSpace(applicationForIssuePolicy.Rider.PRODUCT_ID))/*not attached rider*/
                                        {
                                            flagSubmit = true;
                                        }
                                        else /*rider is attached*/
                                        {
                                            polRider = new bl_micro_policy_rider()
                                            {
                                                POLICY_ID = POLICY.POLICY_ID,
                                                PRODUCT_ID = applicationForIssuePolicy.Rider.PRODUCT_ID,
                                                SUM_ASSURE = applicationForIssuePolicy.Rider.SUM_ASSURE,
                                                PREMIUM = applicationForIssuePolicy.Rider.PREMIUM,
                                                ANNUAL_PREMIUM = applicationForIssuePolicy.Rider.ANNUAL_PREMIUM,
                                                DISCOUNT_AMOUNT = applicationForIssuePolicy.Rider.DISCOUNT_AMOUNT,
                                                TOTAL_AMOUNT = applicationForIssuePolicy.Rider.TOTAL_AMOUNT,
                                                CREATED_BY = this.userName,
                                                CREATED_ON = tranDate    
                                            };
                                            flagSubmit = da_micro_policy_rider.SaveRider(polRider);
                                        }
                                        #endregion Rider

                                        if (flagSubmit) /*save rider success*/
                                        {
                                          
                                            #region Payment
                                            double referralFee = 0.0;
                                            double incentive = 0.0;
                                            da_micro_production_commission_config commObj = new da_micro_production_commission_config();
                                            bl_micro_product_commission_config proComm = commObj.GetProductionCommConfig(POLICY.CHANNEL_ITEM_ID, POLICY.PRODUCT_ID, bl_micro_product_commission_config.CommissionTypeOption.ReferralFee, applicationForIssuePolicy.Application.CLIENT_TYPE);
                                            if (!string.IsNullOrWhiteSpace(proComm.ProductId) && proComm.Status == 1 && applicationForIssuePolicy.Application.APPLICATION_DATE <= proComm.EffectiveTo)
                                            {
                                                if (proComm.ValueType == bl_micro_product_commission_config.ValueTypeOption.Fix)
                                                    referralFee = proComm.Value;
                                                else if (proComm.ValueType == bl_micro_product_commission_config.ValueTypeOption.Percentage)
                                                    referralFee = (polDetail.TOTAL_AMOUNT + polRider.TOTAL_AMOUNT) * proComm.Value / 100.0;
                                            }
                                            //bl_micro_product_commission_config commissionConfig2 = new bl_micro_product_commission_config();
                                            proComm = commObj.GetProductionCommConfig(POLICY.CHANNEL_ITEM_ID, POLICY.PRODUCT_ID, bl_micro_product_commission_config.CommissionTypeOption.Incentive, applicationForIssuePolicy.Application.CLIENT_TYPE);
                                            if (!string.IsNullOrWhiteSpace(proComm.ProductId))
                                            {
                                                if (proComm.ValueType == bl_micro_product_commission_config.ValueTypeOption.Fix)
                                                    incentive = proComm.Value;
                                                else if (proComm.ValueType == bl_micro_product_commission_config.ValueTypeOption.Percentage)
                                                    incentive = (polDetail.TOTAL_AMOUNT + polRider.TOTAL_AMOUNT) * proComm.Value / 100.0;
                                            }

                                            flagSubmit = da_micro_policy_payment.SavePayment(new bl_micro_policy_payment()
                                            {
                                                POLICY_DETAIL_ID = polDetail.POLICY_DETAIL_ID,
                                                USER_PREMIUM = applicationFilter.MainApplicationNumber == "" ? collectedPremium : 0.0, /*set collected premium to zero for all sub applications*/
                                                AMOUNT = polDetail.PREMIUM + polRider.PREMIUM,
                                                DISCOUNT_AMOUNT = polDetail.DISCOUNT_AMOUNT + polRider.DISCOUNT_AMOUNT,
                                                TOTAL_AMOUNT = polDetail.TOTAL_AMOUNT + polRider.TOTAL_AMOUNT,
                                                DUE_DATE = polDetail.EFFECTIVE_DATE,
                                                PAY_DATE = polDetail.ISSUED_DATE,
                                                NEXT_DUE = Calculation.GetNext_Due(polDetail.EFFECTIVE_DATE.AddYears(1), polDetail.EFFECTIVE_DATE, polDetail.EFFECTIVE_DATE),
                                                PREMIUM_YEAR = 1,
                                                PREMIUM_LOT = 1,
                                                OFFICE_ID = "Head Office",
                                                PAY_MODE = applicationForIssuePolicy.Insurance.PAY_MODE,
                                                POLICY_STATUS = "IF",
                                                REFERANCE_TRANSACTION_CODE = paymentRefNo,
                                                TRANSACTION_TYPE = "",
                                                REFERRAL_FEE = referralFee,
                                                REFERRAL_INCENTIVE = incentive,
                                                CREATED_BY = this.userName,
                                                CREATED_ON = tranDate
                                            });
                                            #endregion Payment

                                            if (flagSubmit)
                                            {
                                                
                                                #region Beneficiary
                                                List<string> benIdList = new List<string>();
                                                if (applicationForIssuePolicy.Beneficiaries.Count > 0)
                                                {
                                                    foreach (bl_micro_application_beneficiary beneficiary in applicationForIssuePolicy.Beneficiaries)
                                                    {
                                                        flagSubmit = da_micro_policy_beneficiary.SaveBeneficiary(new bl_micro_policy_beneficiary()
                                                        {
                                                            POLICY_ID = POLICY.POLICY_ID,
                                                            FULL_NAME = beneficiary.FULL_NAME,
                                                            AGE = beneficiary.AGE,
                                                            ADDRESS = beneficiary.ADDRESS,
                                                            RELATION = beneficiary.RELATION,
                                                            PERCENTAGE_OF_SHARE = beneficiary.PERCENTAGE_OF_SHARE,
                                                            CREATED_BY = this.userName,
                                                            CREATED_ON = tranDate    .AddSeconds(1.0),
                                                            BirthDate = beneficiary.DOB,
                                                            Gender = string.Concat((object)beneficiary.Gender),
                                                            IdType = beneficiary.IdType,
                                                            IdNo = beneficiary.IdNo
                                                        });
                                                        if (!flagSubmit)
                                                        {
                                                            isBreakAll = true;
                                                            break;
                                                        }
                                                        else
                                                            benIdList.Add(beneficiary.ID);
                                                            
                                                    }
                                                   
                                                }
#endregion Beneficiary

                                                #region Primary Beneficiary
                                                bl_micro_policy_beneficiary.beneficiary_primary primaryBen = new bl_micro_policy_beneficiary.beneficiary_primary();
                                                if (flagSubmit && proConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
                                                {
                                                    primaryBen = new bl_micro_policy_beneficiary.beneficiary_primary()
                                                      {
                                                          PolicyId = POLICY.POLICY_ID,
                                                          FullName = primaryBeneciary.FullName,
                                                          LoanNumber = primaryBeneciary.LoanNumber,
                                                          Address = primaryBeneciary.Address,
                                                          CreatedBy = this.userName,
                                                          CreatedOn = tranDate,
                                                          CreatedRemarks = ""
                                                      };

                                                    flagSubmit = da_micro_policy_beneficiary.beneficiary_primary.Save(primaryBen);
                                                }
                                                #endregion Primary Beneficiary

                                                if (flagSubmit)
                                                {
                                                   
                                                    #region Policy Address
                                                    polAddress=new bl_micro_policy_address()
                                                    {
                                                        HouseNoKh = CUSTOMER.HOUSE_NO_KH,
                                                        HouseNoEn = CUSTOMER.HOUSE_NO_EN,
                                                        StreetNoEn = CUSTOMER.STREET_NO_EN,
                                                        StreetNoKh = CUSTOMER.STREET_NO_KH,
                                                        PolicyID = POLICY.POLICY_ID,
                                                        ProvinceCode = CUSTOMER.PROVINCE_EN,
                                                        DistrictCode = CUSTOMER.DISTRICT_EN,
                                                        CommuneCode = CUSTOMER.COMMUNE_EN,
                                                        VillageCode = CUSTOMER.VILLAGE_EN,
                                                        CreatedOn = tranDate,
                                                        CreatedBy = this.userName
                                                    };

                                                    flagSubmit = da_micro_policy_address.SaveAddress(polAddress);
                                                    #endregion Policy Address

                                                    if (flagSubmit)
                                                    {
                                                       
                                                        string approver = AppConfiguration.GetApplicationApprover();
                                                        if (approver != "")
                                                        {
                                                            //List<da_report_approver.bl_report_approver> blReportApproverList = new List<da_report_approver.bl_report_approver>();
                                                            List<da_report_approver.bl_report_approver> approverList = da_report_approver.GetApproverList();
                                                            da_report_approver.bl_report_approver approverObj = new da_report_approver.bl_report_approver();
                                                            foreach (var auth in approverList.Where(_ => _.NameEn.ToUpper().Trim() == approver.ToUpper().Trim()))
                                                                approverObj = auth;

                                                            if (!string.IsNullOrWhiteSpace(approverObj.NameKh))
                                                            {
                                                                flagSubmit = da_report_approver.InsertApproverPolicy(new da_report_approver.bl_report_approver_policy()
                                                                {
                                                                    Approver_ID = approverObj.ID,
                                                                    Policy_ID = POLICY.POLICY_ID,
                                                                    Created_By = this.userName,
                                                                    Created_On = tranDate
                                                                });
                                                                if (!flagSubmit)
                                                                {
                                                                    this.ViewState["MESSAGE"] = (object)"Save policy approver fail";
                                                                    isBreakAll = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                this.ViewState["MESSAGE"] = (object)"Policy approver not found.";
                                                                isBreakAll = true;
                                                                flagSubmit = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            this.ViewState["MESSAGE"] = (object)"Get policy approver error.";
                                                            isBreakAll = true;
                                                            flagSubmit = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        this.ViewState["MESSAGE"] = (object)"Save address fail";
                                                        isBreakAll = true;
                                                    }
                                                }
                                                else
                                                {
                                                    this.ViewState["MESSAGE"] = (object)"Save beneficiary fail";
                                                    isBreakAll = true;
                                                }
                                            }
                                            else
                                            {
                                                this.ViewState["MESSAGE"] = (object)"Save policy payment fail";
                                                isBreakAll = true;
                                            }
                                        }
                                        else
                                        {
                                            this.ViewState["MESSAGE"] = (object)"Save rider fail.";
                                            isBreakAll = true;
                                        }
                                    }
                                    else
                                    {
                                        this.ViewState["MESSAGE"] = (object)"Save policy detail fail.";
                                        isBreakAll = true;
                                    }
                                }
                                else
                                {
                                    this.ViewState["MESSAGE"] = (object)"Save policy fail.";
                                    isBreakAll = true;
                                }
                            }
                            else
                            {
                                this.ViewState["MESSAGE"] = (object)"Save customer fail.";
                                isBreakAll = true;
                            }
                        }
                        else
                        {
                            this.ViewState["MESSAGE"] = "Application :" + applicationFilter.ApplicationNumber + " was already issued.";
                            isBreakAll = true;
                        }
                    }
                    else
                    {
                        this.ViewState["MESSAGE"] = (object)"Application is not found.";
                        isBreakAll = true;
                    }
                    ++loopStep;
                    if (isBreakAll)
                        break;
                }

                if (flagSubmit)
                {
                     string strSuccess = "<strong>Policy issue successfully</strong><br />";
                    foreach (bl_micro_policy.SavedIssuePolicy savedIssuePolicy in _SavedIssuePolicyList)
                        strSuccess +=  savedIssuePolicy.ApplicationNumber + " --> " + savedIssuePolicy.PolicyNumber + "<br />";
                    Helper.Alert(false, strSuccess, this.lblError);
                    this.BindExisting();
                }
                else
                {
                    string strFail = this.ViewState["MESSAGE"].ToString() + "<br />" + "<strong>Policy roleback: </strong><br />";
                    foreach (bl_micro_policy.SavedIssuePolicy savedIssuePolicy in _SavedIssuePolicyList)
                        strFail +=  savedIssuePolicy.PolicyNumber + " --> " + (da_banca.RoleBackIssuePolicy(savedIssuePolicy.IsExistingCustomer ? "" : savedIssuePolicy.CustomerId, savedIssuePolicy.PolicyId) ? "Successfully" : "Fail") + "<br />";
                    Helper.Alert(true, strFail, this.lblError);
                }
            }
            else
                Helper.Alert(true, message, this.lblError);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [bntIssue_Click(object sender, EventArgs e)] in class [banca_micro_application.aspx.cs], detail: " + ex.Message + "=>" + ex.StackTrace);
            this.RoleBackIssuePolicy();
            Helper.Alert(true, ex.StackTrace, this.lblError);
        }
    }

    private void RoleBackIssuePolicy(string INPUT_MESSAGE = "")
    {
        string roleBackSms="";
        foreach (var polObj in _SavedIssuePolicyList)
            roleBackSms += da_banca.RoleBackIssuePolicy(polObj.IsExistingCustomer ? "" : polObj.CustomerId, polObj.PolicyId) ? string.Concat(polObj.PolicyNumber, " --> Successfully<br />") : string.Concat(polObj.PolicyNumber, " --> Fail<br />");
       
        Helper.Alert(true, string.Concat( "Issue Policy fail. Rolebak Policy <br />", roleBackSms), this.lblError);
    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtProductName.Text = "";
        this.txtSumAssure.Text = "";
        this.txtPremium.Text = "";
        this.txtAnnualPremium.Text = "";
        this.ddlRiderProduct.Items.Clear();
        this.txtRiderAnnualPremium.Text = "";
        this.txtRiderPremium.Text = "";
        this.txtRiderProductName.Text = "";
        if (this.GetProductInfo(this.ddlProduct.SelectedValue))
        {
            this.hdfProductID.Value = this.ddlProduct.SelectedValue;
            this.txtProductName.Text = this.ProductConfig.En_Title;
            this.txtSumAssure.Text = this.ProductConfig.BasicSumAssured ?? "";
            this.txtTotalSumAssure.Text = this.ProductConfig.BasicSumAssured ?? "";
            this.ddlCoverType.Items.Clear();
            this.ddlTermOfCover.Items.Clear();
            foreach (string str in this.ProductConfig.CoverPeriodType)
            {
                char[] chArray = new char[1] { ':' };
                string[] strArray1 = str.Split(chArray);
                string[] strArray2 = strArray1[0].Split(',');
                string[] strArray3 = strArray1[1].Split(',');
                foreach (string text in strArray2)
                    this.ddlCoverType.Items.Add(new ListItem(text, text));
                foreach (string text in strArray3)
                    this.ddlTermOfCover.Items.Add(new ListItem(text, text));
            }
            this.txtTotalSumAssure_TextChanged((object)null, (EventArgs)null);
            this.BindPaymode();
            if (!string.IsNullOrWhiteSpace(this.ProductConfig.RiderProductID))
            {
                this.BindProductRider();
                this.BindDHCSA();
                this.dvRider.Attributes.CssStyle.Add("display", "block");
            }
            else
                this.dvRider.Attributes.CssStyle.Add("display", "none");
            if (this.ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
            {
                bl_channel_item channelChannelItem = da_channel.GetChannelChannelItem(this.ddlCompany.SelectedValue);
                if (!string.IsNullOrWhiteSpace(channelChannelItem.Channel_Name))
                {
                    this.txtHolderName.Text = channelChannelItem.Channel_Name;
                    this.txtHolderAddress.Text = channelChannelItem.Channel_HQ_Address_KH;
                }
                this.txtPrimaryBenName.Text = channelChannelItem.Channel_Name;
                this.txtPrimaryBenLoan.Text = this.txtLoanNumber.Text.Trim();
                this.txtPrimaryBenAddress.Text = channelChannelItem.Channel_HQ_Address_KH;
                this.benTitle.InnerHtml = "Contingent Beneficiary(s)";
                this.dvPrimaryBeneficiary.Attributes.CssStyle.Add("display", "block");
                this.dvPolicyHolder.Attributes.CssStyle.Add("display", "block");
            }
            else
            {
                this.txtHolderName.Text = "";
                this.txtHolderAddress.Text = "";
                this.txtPrimaryBenName.Text = "";
                this.txtPrimaryBenLoan.Text = "";
                this.txtPrimaryBenAddress.Text = "";
                this.benTitle.InnerHtml = "Beneficiary(s)";
                this.dvPrimaryBeneficiary.Attributes.CssStyle.Add("display", "none");
                this.dvPolicyHolder.Attributes.CssStyle.Add("display", "none");
            }
        }
        else
            Helper.Alert(true, string.Concat(this.ViewState["V_MESSAGE"]), this.lblError);
    }

    private bool GetProductInfo(string productID)
    {
        try
        {
            List<bl_micro_product_config> source = (List<bl_micro_product_config>)this.Session["PRODUCT_LIST"];
            if (source.Count > 0)
            {
                using (IEnumerator<bl_micro_product_config> enumerator = source.Where<bl_micro_product_config>((System.Func<bl_micro_product_config, bool>)(_ => _.Product_ID == productID)).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                        this.ProductConfig = enumerator.Current;
                }
                try
                {
                    Channel_Item_Config channelItemConfig = new Channel_Item_Config().GetChannelItemConfig(this.ddlCompany.SelectedValue, this.ProductConfig.Product_ID);
                    for (int index = 1; index <= channelItemConfig.MaxPolicyPerLife; ++index)
                        this.ddlNumberofApplication.Items.Add(new ListItem(string.Concat((object)index), string.Concat((object)index)));
                    this._MaxPolicyPerLife = channelItemConfig.MaxPolicyPerLife;
                }
                catch (Exception ex)
                {
                    this._MaxPolicyPerLife = 0;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            this.ViewState["V_MESSAGE"] = (object)("Get product informaton error, detail: <br />" + ex.Message);
            return false;
        }
    }

    private void BindProductRider()
    {
        List<bl_micro_product_rider> microProductRider1 = da_micro_product_config.ProductConfig.GetProductMicroProductRider(this.ddlProduct.SelectedValue);
        this.ddlRiderProduct.Items.Clear();
        this.ddlRiderProduct.Items.Add(new ListItem("---Select---", ""));
        foreach (bl_micro_product_rider microProductRider2 in microProductRider1)
            this.ddlRiderProduct.Items.Add(new ListItem(microProductRider2.PRODUCT_ID + "/" + microProductRider2.REMARKS, microProductRider2.PRODUCT_ID));
    }

    private void BindDHCSA()
    {
        bl_micro_product_config productConfig = this.ProductConfig;
        this.ddlRiderSumAssure.Items.Clear();
        this.ddlRiderSumAssure.Items.Add(new ListItem("---Select---", ""));
        if (productConfig.Product_ID == null)
            return;
        foreach (double num in productConfig.RiderSumAssuredRange)
            this.ddlRiderSumAssure.Items.Add(new ListItem(string.Concat((object)num), string.Concat((object)num)));
    }

    protected void ibtnPrintApplication_Click(object sender, ImageClickEventArgs e)
    {
        this.SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, "User views application form. [App No:" + this.hdfApplicationNumber.Value + "].");
        //  da_micro_application_insurance.GetApplicationInsurance(this.hdfApplicationNumber.Value);
        if (this.ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || this.ProductConfig.CreatedOn.Year >= 2025)
        {
            this.Session["APP_ID_PRINT"] = (object)da_micro_application.GetApplicationNumberMainSub(this.hdfApplicationNumber.Value).Select<bl_micro_application.ApplicationFilter, string>((System.Func<bl_micro_application.ApplicationFilter, string>)(x => x.ApplicationId)).ToList<string>();
            string url = "load_new_application_form.aspx";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
        }
        else
        {
            string url = "banca_micro_application_print.aspx?APP_ID=" + this.hdfApplicationID.Value + "&A_TYPE=IND";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
        }
    }

    protected void ibtnPrintCertificate_Click(object sender, ImageClickEventArgs e)
    {
        this.SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, "User views certificate. [Pol No:" + this.hdfPolicyNumber.Value + "].");
        if (this.ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || this.ProductConfig.CreatedOn.Year >= 2025)
        {
            List<bl_micro_application.ApplicationFilter> applicationNumberMainSub = da_micro_application.GetApplicationNumberMainSub(this.hdfApplicationNumber.Value);
            List<string> stringList = new List<string>();
            foreach (bl_micro_application.ApplicationFilter applicationFilter in applicationNumberMainSub)
            {
                bl_micro_policy policyByApplicationId = da_micro_policy.GetPolicyByApplicationID(applicationFilter.ApplicationId);
                stringList.Add(policyByApplicationId.POLICY_ID);
            }
            this.Session["POL_ID_PRINT"] = (object)stringList;
            string url = "load_new_certificate.aspx?policyType=IND&printPolInsurance=Y";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
        }
        else
        {
            string url = "banca_micro_cert.aspx?P_ID=" + this.hdfPolicyID.Value + "&P_TYPE=IND";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
        }
    }

    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        string message = "";
        DateTime tranDate = DateTime.Now;
        List<string> newAppList = new List<string>();
        List<string> existAppList = new List<string>();
        try
        {
            this.ValidateForm();
            message = string.Concat(this.ViewState["MESSAGE"]);
            if ((bool)this.ViewState["IS_VALID"])
            {
                string channelItemId = this.ddlCompany.SelectedValue;
                string channelId = this.ddlChannel.SelectedValue;
                string channelLocationId = this.hdfChannelLocationID.Value;
                string provinceCode = this.GetProvinceCode();
                string disctrictCode = this.GetDisctrictCode();
                string communeCode = this.GetCommuneCode();
                string villageCode = this.GetVillageCode();
                int appStep = 1;
                int totalAppStep = 0;
                int yearStep = 1;
                // int appStepInYear = 1;
                int numOfApp = Convert.ToInt32(this.ddlNumberofApplication.SelectedValue);
                int numOfYear = Convert.ToInt32(this.ddlNumberofYear.SelectedValue);
                int numOfAppOld = 0;
                int numOfYearOld = 0;

                string appId = this.hdfApplicationID.Value;
                string MainApplicationNumber = "";
                List<bl_micro_application> appExistingLists = da_micro_application.GetApplicationBatchByApplicationID(appId);

                bool flag = false;
                // bl_micro_application microApplication1 = new bl_micro_application();
                int newAppCount = numOfApp * numOfYear;
                string appCustomerId = "";
                bool isBreakAll = false;
                while (appStep <= numOfApp)/*loop through numbers of application count*/
                {
                    SumAssurePremiums sumAssurePremiums = this._SumAssurePremium[appStep - 1];
                    double sumAssure = sumAssurePremiums.SumAssure;
                    double premium = Convert.ToDouble(sumAssurePremiums.Premium + "");
                    double annualPremium = Convert.ToDouble("" + sumAssurePremiums.AnnualPremium);

                    while (yearStep <= numOfYear)/*loop purchasing years*/
                    {
                        bl_micro_application APPLICATION = new bl_micro_application();
                        bl_micro_application_customer APPLICATION_CUSTOMER = new bl_micro_application_customer();
                        APPLICATION_CUSTOMER.ID_TYPE = this.ddlIDType.SelectedValue;
                        APPLICATION_CUSTOMER.ID_NUMBER = this.txtIDNumber.Text.Trim();
                        APPLICATION_CUSTOMER.FIRST_NAME_IN_ENGLISH = this.txtFirstNameEng.Text.Trim();
                        APPLICATION_CUSTOMER.LAST_NAME_IN_ENGLISH = this.txtSurnameEng.Text.Trim();
                        APPLICATION_CUSTOMER.FIRST_NAME_IN_KHMER = this.txtFirstNameKh.Text.Trim();
                        APPLICATION_CUSTOMER.LAST_NAME_IN_KHMER = this.txtSurnameKh.Text.Trim();
                        APPLICATION_CUSTOMER.GENDER = this.ddlGender.SelectedValue;
                        APPLICATION_CUSTOMER.DATE_OF_BIRTH = Helper.FormatDateTime(this.txtDateOfBirth.Text.Trim());
                        APPLICATION_CUSTOMER.NATIONALITY = this.ddlNationality.SelectedItem.Text;
                        APPLICATION_CUSTOMER.MARITAL_STATUS = this.ddlMaritalStatus.SelectedValue;
                        APPLICATION_CUSTOMER.OCCUPATION = this.ddlOccupation.SelectedValue;
                        APPLICATION_CUSTOMER.HOUSE_NO_KH = this.txtHouseNoKh.Text.Trim();
                        APPLICATION_CUSTOMER.STREET_NO_KH = this.txtStreetKh.Text.Trim();
                        APPLICATION_CUSTOMER.VILLAGE_KH = villageCode;
                        APPLICATION_CUSTOMER.COMMUNE_KH = communeCode;
                        APPLICATION_CUSTOMER.DISTRICT_KH = disctrictCode;
                        APPLICATION_CUSTOMER.PROVINCE_KH = provinceCode;
                        APPLICATION_CUSTOMER.HOUSE_NO_EN = this.txtHouseNoEn.Text.Trim();
                        APPLICATION_CUSTOMER.STREET_NO_EN = this.txtStreetEn.Text.Trim();
                        APPLICATION_CUSTOMER.VILLAGE_EN = villageCode;
                        APPLICATION_CUSTOMER.COMMUNE_EN = communeCode;
                        APPLICATION_CUSTOMER.DISTRICT_EN = disctrictCode;
                        APPLICATION_CUSTOMER.PROVINCE_EN = provinceCode;
                        APPLICATION_CUSTOMER.PHONE_NUMBER1 = this.txtPhoneNumber.Text.Trim();
                        APPLICATION_CUSTOMER.EMAIL1 = this.txtEmail.Text.Trim();
                        APPLICATION_CUSTOMER.CREATED_BY = this.userName;
                        APPLICATION_CUSTOMER.CREATED_ON = tranDate;
                        APPLICATION_CUSTOMER.STATUS = 1;
                        APPLICATION.APPLICATION_DATE = Helper.FormatDateTime(this.txtApplicationDate.Text);
                        APPLICATION.CHANNEL_ITEM_ID = channelItemId;
                        APPLICATION.CHANNEL_ID = channelId;
                        APPLICATION.CHANNEL_LOCATION_ID = channelLocationId;
                        APPLICATION.SALE_AGENT_ID = this.txtSaleAgentID.Text;
                        APPLICATION.CREATED_BY = this.userName;
                        APPLICATION.CREATED_ON = DateTime.Now;
                        APPLICATION.REMARKS = "";
                        APPLICATION.CLIENT_TYPE = yearStep == 1 ? this.ddlClientType.SelectedValue : "REPAYMENT";
                        APPLICATION.CLIENT_TYPE_RELATION = "";
                        APPLICATION.CLIENT_TYPE_REMARKS = "";
                        if (ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
                        {
                            APPLICATION.LoanNumber = this.txtLoanNumber.Text.Trim();
                            APPLICATION.PolicyholderName = this.txtHolderName.Text.Trim();
                            APPLICATION.PolicyholderDOB = this.txtHolderDOB.Text.Trim() == "" ? new DateTime(1900, 1, 10) : Helper.FormatDateTime(this.txtHolderDOB.Text.Trim());
                            APPLICATION.PolicyholderGender = Convert.ToInt32(this.ddlHolderGender.SelectedValue);
                            APPLICATION.PolicyholderIDType = Convert.ToInt32(this.ddlHolderIdType.SelectedValue);
                            APPLICATION.PolicyholderIDNo = this.txtHolderIdNo.Text.Trim();
                            APPLICATION.PolicyholderAddress = this.txtHolderAddress.Text.Trim();
                        }
                        else
                        {
                            APPLICATION.LoanNumber = "";
                            APPLICATION.PolicyholderName = "";
                            APPLICATION.PolicyholderDOB = new DateTime(1900, 1, 10);
                            APPLICATION.PolicyholderGender = -1;
                            APPLICATION.PolicyholderIDType = -1;
                            APPLICATION.PolicyholderIDNo = "";
                            APPLICATION.PolicyholderAddress = "";
                        }
                        bl_micro_application_insurance APP_INSURANCE = new bl_micro_application_insurance()
                        {
                            PRODUCT_ID = this.hdfProductID.Value,
                            TERME_OF_COVER = Convert.ToInt32(this.ddlTermOfCover.SelectedValue),
                            PAYMENT_PERIOD = Convert.ToInt32(this.txtPremiumPaymentPeriod.Text.Trim()),
                            SUM_ASSURE = sumAssure,
                            PAY_MODE = Convert.ToInt32(this.ddlPaymentMode.SelectedValue),
                            PREMIUM = premium,
                            ANNUAL_PREMIUM = annualPremium,
                            USER_PREMIUM = 0.0,
                            DISCOUNT_AMOUNT = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim()),
                            PACKAGE = ""
                        };
                        APP_INSURANCE.TOTAL_AMOUNT = APP_INSURANCE.PREMIUM - APP_INSURANCE.DISCOUNT_AMOUNT;
                        APP_INSURANCE.CREATED_BY = this.userName;
                        APP_INSURANCE.CREATED_ON = tranDate;
                        APP_INSURANCE.PAYMENT_CODE = this.txtPaymentCode.Text;
                        bl_micro_application_insurance_rider APP_INSURANCE_RIDER = new bl_micro_application_insurance_rider();
                        if (this.hdfRiderProductID.Value != "")
                        {
                            APP_INSURANCE_RIDER.PRODUCT_ID = this.hdfRiderProductID.Value;
                            APP_INSURANCE_RIDER.SUM_ASSURE = Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue);
                            APP_INSURANCE_RIDER.PREMIUM = Convert.ToDouble(this.txtRiderPremium.Text.Trim());
                            APP_INSURANCE_RIDER.ANNUAL_PREMIUM = Convert.ToDouble(this.txtRiderAnnualPremium.Text.Trim());
                            APP_INSURANCE_RIDER.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
                            APP_INSURANCE_RIDER.TOTAL_AMOUNT = Convert.ToDouble(this.txtRiderAfterDiscount.Text.Trim());
                            APP_INSURANCE_RIDER.CREATED_BY = this.userName;
                            APP_INSURANCE_RIDER.CREATED_ON = tranDate;
                        }
                        bl_micro_application_beneficiary.PrimaryBeneciary primaryBen;
                        if (ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
                            primaryBen = new bl_micro_application_beneficiary.PrimaryBeneciary()
                            {
                                FullName = this.txtPrimaryBenName.Text.Trim(),
                                LoanNumber = this.txtPrimaryBenLoan.Text.Trim(),
                                Address = this.txtPrimaryBenAddress.Text.Trim(),
                                CreatedBy = this.userName,
                                CreatedOn = tranDate,
                                CreatedRemarks = ""
                            };
                        else
                            primaryBen = null;

                        if (totalAppStep < appExistingLists.Count) /*update*/
                        {
                            bl_micro_application existApp = appExistingLists[totalAppStep];
                            if (da_micro_application.BackupApplication(existApp.APPLICATION_ID, "UPDATE", this.userName, tranDate))
                            {
                                existAppList.Add(existApp.APPLICATION_NUMBER);
                                if (appStep == 1 && yearStep == 1)/*update customer information only first time since it's same information*/
                                {
                                    MainApplicationNumber = existApp.APPLICATION_NUMBER;
                                    appCustomerId = existApp.APPLICATION_CUSTOMER_ID;
                                    numOfAppOld = existApp.NumbersOfApplicationFirstYear;
                                    numOfYearOld = existApp.NumbersOfPurchasingYear;

                                    APPLICATION_CUSTOMER.CUSTOMER_ID = existApp.APPLICATION_CUSTOMER_ID;
                                    APPLICATION_CUSTOMER.UPDATED_BY = this.userName;
                                    APPLICATION_CUSTOMER.UPDATED_ON = tranDate;
                                    flag = da_micro_application_customer.UpdateApplicationCustomer(APPLICATION_CUSTOMER);
                                }
                                else
                                    flag = true;
                                if (flag)
                                {
                                    APPLICATION.UPDATED_BY = this.userName;
                                    APPLICATION.UPDATED_ON = tranDate;
                                    APPLICATION.APPLICATION_NUMBER = existApp.APPLICATION_NUMBER;
                                    APPLICATION.APPLICATION_CUSTOMER_ID = existApp.APPLICATION_CUSTOMER_ID;
                                    flag = da_micro_application.UpdateApplication(APPLICATION);
                                    if (flag)
                                    {
                                        APP_INSURANCE.UPDATED_BY = this.userName;
                                        APP_INSURANCE.UPDATED_ON = tranDate;
                                        APP_INSURANCE.APPLICATION_NUMBER = existApp.APPLICATION_NUMBER;
                                        APP_INSURANCE.PAYMENT_CODE = this.txtPaymentCode.Text;
                                        APP_INSURANCE.COVER_TYPE = (bl_micro_product_config.PERIOD_TYPE)Enum.Parse(typeof(bl_micro_product_config.PERIOD_TYPE), this.ddlCoverType.SelectedValue);
                                        flag = da_micro_application_insurance.UpdateApplicationInsurance(APP_INSURANCE);
                                        if (flag)
                                        {
                                            bl_micro_application_insurance_rider existRider = da_micro_application_insurance_rider.GetApplicationRider(existApp.APPLICATION_NUMBER);
                                            if (!string.IsNullOrWhiteSpace(APP_INSURANCE_RIDER.PRODUCT_ID))/*attached new rider*/
                                            {
                                                APP_INSURANCE_RIDER.APPLICATION_NUMBER = existApp.APPLICATION_NUMBER;
                                                if (!string.IsNullOrWhiteSpace(existRider.PRODUCT_ID))
                                                {
                                                    if (appStep == 1) /*first application each year*/
                                                    {
                                                        APP_INSURANCE_RIDER.UPDATED_BY = this.userName;
                                                        APP_INSURANCE_RIDER.UPDATED_ON = tranDate;
                                                        flag = da_micro_application_insurance_rider.UpdateApplicationInsuranceRider(APP_INSURANCE_RIDER);
                                                    }
                                                    else
                                                        flag = da_micro_application_insurance_rider.DeleteApplicationInsuranceRider(existApp.APPLICATION_NUMBER);
                                                }
                                                else if (appStep == 1)/*add rider first application each year*/
                                                {
                                                    APP_INSURANCE.CREATED_BY = this.userName;
                                                    APP_INSURANCE.CREATED_ON = tranDate;
                                                    flag = da_micro_application_insurance_rider.SaveApplicationInsuranceRider(APP_INSURANCE_RIDER);
                                                }
                                            }
                                            else /*not attached rider then remove*/
                                                da_micro_application_insurance_rider.DeleteApplicationInsuranceRider(existApp.APPLICATION_NUMBER);

                                            if (flag)
                                            {
                                                if (this.BeneficiaryList != null)
                                                {
                                                    foreach (bl_micro_application_beneficiary beneficiary in this.BeneficiaryList)
                                                    {
                                                        if (beneficiary.ID.Length <= 2)
                                                        {
                                                            beneficiary.ID = beneficiary.NewID();
                                                            beneficiary.APPLICATION_NUMBER = existApp.APPLICATION_NUMBER;
                                                            beneficiary.CREATED_BY = this.userName;
                                                            beneficiary.CREATED_ON = tranDate.AddSeconds(1.0);
                                                            flag = da_micro_application_beneficiary.SaveApplicationBeneficiary(beneficiary);
                                                        }
                                                        else
                                                        {
                                                            beneficiary.APPLICATION_NUMBER = existApp.APPLICATION_NUMBER;
                                                            beneficiary.UPDATED_BY = this.userName;
                                                            beneficiary.UPDATED_ON = tranDate;
                                                            flag = da_micro_application_beneficiary.UpdateApplicationBeneficiary(beneficiary);
                                                        }
                                                        if (!flag)
                                                        {
                                                            isBreakAll = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                    flag = false;

                                                if (flag)
                                                {
                                                    bl_micro_application_beneficiary.PrimaryBeneciary existingPrimaryBen = da_micro_application_beneficiary.PremaryBeneficiary.Get(existApp.APPLICATION_NUMBER);
                                                    if (primaryBen != null)
                                                    {
                                                        primaryBen.ApplicationNumber = existApp.APPLICATION_NUMBER;
                                                        if (existingPrimaryBen == null)
                                                        {
                                                            primaryBen.CreatedBy = this.userName;
                                                            primaryBen.CreatedOn = tranDate;
                                                            primaryBen.CreatedRemarks = "";
                                                            flag = da_micro_application_beneficiary.PremaryBeneficiary.Save(primaryBen);
                                                        }
                                                        else
                                                        {
                                                            primaryBen.UpdatedBy = this.userName;
                                                            primaryBen.UpdatedOn = tranDate;
                                                            primaryBen.UpdatedRemarks = "";
                                                            flag = da_micro_application_beneficiary.PremaryBeneficiary.Update(primaryBen);
                                                        }
                                                    }
                                                    else if (existingPrimaryBen != null)
                                                        da_micro_application_beneficiary.PremaryBeneficiary.Delete(existingPrimaryBen.ApplicationNumber);

                                                    if (flag)
                                                    {
                                                        flag = da_micro_application_questionaire.UpdateQuestionaire(new bl_micro_application_questionaire()
                                                        {
                                                            QUESTION_ID = this.hdfQuestionID.Value,
                                                            APPLICATION_NUMBER = this.hdfApplicationNumber.Value,
                                                            ANSWER = Convert.ToInt32(this.ddlAnswer.SelectedValue),
                                                            ANSWER_REMARKS = this.txtAnswerRemarks.Text.Trim(),
                                                            UPDATED_BY = this.userName,
                                                            UPDATED_ON = tranDate
                                                        });
                                                        if (!flag)
                                                        {
                                                            Helper.Alert(true, "Health Question is updated fial.", this.lblError);
                                                            isBreakAll = true;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Helper.Alert(true, "Primary Beneficiary is updated fial.", this.lblError);
                                                        isBreakAll = true;
                                                        break;
                                                    }

                                                }
                                                else
                                                {
                                                    Helper.Alert(true, "Beneficiary is updated fial.", this.lblError);
                                                    isBreakAll = true;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                Helper.Alert(true, "Application Rider is updated fial.", this.lblError);
                                                isBreakAll = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Helper.Alert(true, "Application Insurance is updated fial.", this.lblError);
                                            isBreakAll = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Helper.Alert(true, "Application is updated fial.", this.lblError);
                                        isBreakAll = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    Helper.Alert(true, "Customer is updated fial.", this.lblError);
                                    isBreakAll = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (appStep == 1 && yearStep == 1)/*first application and year*/
                            {
                                appCustomerId = appCustomerId == "" ? APPLICATION_CUSTOMER.CUSTOMER_ID : appCustomerId;
                                APPLICATION_CUSTOMER.CUSTOMER_ID = appCustomerId;
                                flag = da_micro_application_customer.SaveApplicationCustomer(APPLICATION_CUSTOMER);
                            }
                            else /*skip saving save customer information*/
                            {
                                flag = true;
                                APPLICATION_CUSTOMER.CUSTOMER_ID = appCustomerId;
                            }
                            if (flag)
                            {
                                APPLICATION.APPLICATION_CUSTOMER_ID = APPLICATION_CUSTOMER.CUSTOMER_ID;
                                this.hdfApplicationCustomerID.Value = APPLICATION_CUSTOMER.CUSTOMER_ID;
                                bl_micro_application_prefix applicationPrefix1 = new bl_micro_application_prefix();
                                bl_micro_application_prefix applicationPrefix2 = da_micro_application_prefix.GetLastApplicationPrefix();
                                APPLICATION.SEQ = APPLICATION.LAST_SEQ + 1;
                                string appNoFormat = "";
                                //if (APPLICATION.LAST_PREFIX == applicationPrefix2.PREFIX2)
                                //{
                                //    appNoFormat = applicationPrefix2.PREFIX1 + applicationPrefix2.PREFIX2 + APPLICATION.SEQ.ToString(applicationPrefix2.DIGITS);
                                //}
                                //else
                                //{

                                //    APPLICATION.SEQ = 1;
                                //    appNoFormat = applicationPrefix2.PREFIX1 + applicationPrefix2.PREFIX2 + APPLICATION.SEQ.ToString(applicationPrefix2.DIGITS);
                                //}
                                if (APPLICATION.LAST_PREFIX != applicationPrefix2.PREFIX2)
                                    APPLICATION.SEQ = 1;

                                appNoFormat = applicationPrefix2.PREFIX1 + applicationPrefix2.PREFIX2 + APPLICATION.SEQ.ToString(applicationPrefix2.DIGITS);
                                APPLICATION.APPLICATION_NUMBER = appNoFormat;
                                if (appStep == 1 && yearStep == 1)
                                {
                                    //if (appExistingLists.Count > 0)
                                    //{
                                    //    numOfAppOld = appExistingLists[appStep].NumbersOfApplicationFirstYear;
                                    //    numOfYearOld = appExistingLists[appStep].NumbersOfPurchasingYear;
                                    //}

                                    APPLICATION.NumbersOfApplicationFirstYear = numOfApp;
                                    APPLICATION.NumbersOfPurchasingYear = numOfYear;
                                    MainApplicationNumber = APPLICATION.APPLICATION_NUMBER;
                                }
                                else
                                {
                                    APPLICATION.MainApplicationNumber = MainApplicationNumber;
                                    APPLICATION.NumbersOfApplicationFirstYear = 0;
                                    APPLICATION.NumbersOfPurchasingYear = 0;
                                }
                                flag = da_micro_application.SaveApplication(APPLICATION);
                                if (flag)
                                {
                                    newAppList.Add(APPLICATION.APPLICATION_NUMBER);
                                    this.hdfApplicationNumber.Value = APPLICATION.APPLICATION_NUMBER;
                                    this.hdfApplicationID.Value = APPLICATION.APPLICATION_ID;
                                    APP_INSURANCE.APPLICATION_NUMBER = APPLICATION.APPLICATION_NUMBER;
                                    APP_INSURANCE_RIDER.APPLICATION_NUMBER = APPLICATION.APPLICATION_NUMBER;
                                    APP_INSURANCE.COVER_TYPE = (bl_micro_product_config.PERIOD_TYPE)Enum.Parse(typeof(bl_micro_product_config.PERIOD_TYPE), this.ddlCoverType.SelectedValue);
                                    flag = da_micro_application_insurance.SaveApplicationInsurance(APP_INSURANCE);
                                    if (flag)
                                    {
                                        if (appStep == 1)/*save rider one time only each year*/
                                        {
                                            if (hdfRiderProductID.Value != "")
                                                flag = da_micro_application_insurance_rider.SaveApplicationInsuranceRider(APP_INSURANCE_RIDER);
                                        }
                                        if (primaryBen != null)
                                        {
                                            primaryBen.ApplicationNumber = APPLICATION.APPLICATION_NUMBER;
                                            primaryBen.CreatedBy = this.userName;
                                            primaryBen.CreatedOn = tranDate;
                                            primaryBen.CreatedRemarks = "";
                                            flag = da_micro_application_beneficiary.PremaryBeneficiary.Save(primaryBen);
                                        }
                                        if (flag)
                                        {
                                            foreach (bl_micro_application_beneficiary beneficiary in this.BeneficiaryList)
                                            {
                                                beneficiary.ID = beneficiary.NewID();
                                                beneficiary.APPLICATION_NUMBER = APPLICATION.APPLICATION_NUMBER;
                                                beneficiary.CREATED_ON = tranDate.AddSeconds(1.0);
                                                beneficiary.CREATED_BY = this.userName;
                                                flag = da_micro_application_beneficiary.SaveApplicationBeneficiary(beneficiary);
                                                if (!flag)
                                                {
                                                    isBreakAll = true;
                                                    break;
                                                }
                                            }
                                            if (!flag)
                                            {
                                                this.ViewState["MESSAGE"] = "Save Beneficiary fail.";
                                                this.RoleBack();
                                                isBreakAll = true;
                                                break;
                                            }
                                            flag = da_micro_application_questionaire.SaveQuestionaire(new bl_micro_application_questionaire()
                                            {
                                                QUESTION_ID = this.hdfQuestionID.Value,
                                                APPLICATION_NUMBER = this.hdfApplicationNumber.Value,
                                                ANSWER = Convert.ToInt32(this.ddlAnswer.SelectedValue),
                                                ANSWER_REMARKS = this.txtAnswerRemarks.Text.Trim(),
                                                CREATED_ON = tranDate,
                                                CREATED_BY = this.userName
                                            });
                                            if (flag)
                                            {
                                                this.SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, "User saves new application. [App No:" + this.hdfApplicationNumber.Value + "].");
                                                this.txtApplicationNumber.Text = this.hdfApplicationNumber.Value;
                                            }
                                            else
                                            {
                                                this.ViewState["MESSAGE"] = (object)"Save Questionaire fail.";
                                                this.RoleBack();
                                                isBreakAll = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            this.ViewState["MESSAGE"] = (object)"Saved Insurance rider fial.";
                                            this.RoleBack();
                                            isBreakAll = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        this.ViewState["MESSAGE"] = (object)"Saved Insurance Product and detail fial.";
                                        this.RoleBack();
                                        isBreakAll = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    this.ViewState["MESSAGE"] = (object)"Save Application Information fail.";
                                    this.RoleBack();
                                    isBreakAll = true;
                                    break;
                                }
                            }
                            else
                            {
                                this.ViewState["MESSAGE"] = (object)"Save Customer Information fail.";

                                isBreakAll = true;
                                break;
                            }
                        }

                        // appStepInYear++;
                        totalAppStep++;
                        yearStep++;
                    } /*end loop year*/
                    // appStepInYear = 1;/*reset while end loop of year*/
                    yearStep = 1;
                    appStep++;
                    if (isBreakAll)
                        break;
                }/*end loop application*/

                if (flag)
                {
                    if (newAppCount < appExistingLists.Count)/*user decrease total number applications then system will remove them*/
                    {
                        int numRemove = appExistingLists.Count - newAppCount;/*define total records to remove*/
                        int indexRemove = appExistingLists.Count - numRemove; /*define index*/
                        while (indexRemove < appExistingLists.Count && da_micro_application.UpdateApplicationStatus(appExistingLists[indexRemove].APPLICATION_NUMBER, "DEL", this.userName, tranDate, "user decreased total applications"))
                            ++indexRemove;
                    }
                    string updatedRemarks = "";
                    if (appExistingLists.Count > 0)
                    {
                        if (numOfApp != numOfAppOld)
                            updatedRemarks += "Change Numbers of Application First Year [" + numOfAppOld + "] To [" + numOfApp + "]";
                        if (numOfYear != numOfYearOld)
                            updatedRemarks += (updatedRemarks == "" ? "Change Numbers of Purchasing Year " : "Numbers of Purchasing Year ") + "[" + numOfYearOld + "] To [" + numOfYear + "]";
                    }
                    if (da_micro_application.UpdateApplicationTotalNumbers(MainApplicationNumber, numOfApp, numOfYear, this.userName, tranDate, updatedRemarks))
                    {
                        foreach (string appNo in existAppList)
                            da_micro_application.DeleteBackupApplication(appNo, this.userName, tranDate);

                        string smsAdd = "";
                        int recordNo = 1;
                        foreach (string appUpdate in existAppList)
                        {
                            smsAdd += recordNo + "." + appUpdate + " - Updated <br />";
                            ++recordNo;
                        }

                        foreach (string appAdd in newAppList)
                        {
                            smsAdd += recordNo + "." + appAdd + " - Added <br />";
                            ++recordNo;
                        }
                        Helper.Alert(false, smsAdd, this.lblError);
                        this.BindExisting();
                    }
                    else
                    {
                        this._RoleBack(newAppList, existAppList, this.userName, tranDate, out message);
                        Helper.Alert(false, "SYSTEM ROLEBACK <br />" + message, this.lblError);
                    }
                }
                else
                {
                    this._RoleBack(newAppList, existAppList, this.userName, tranDate, out message);
                    Helper.Alert(false, "SYSTEM ROLEBACK <br />" + message, this.lblError);
                }
            }
            else

                Helper.Alert(true, message, this.lblError);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ibtnSave_Click(object sender, ImageClickEventArgs e)] class [individule_micro_application_form_aspx.cs], detail: " + ex.Message + "==>" + ex.StackTrace);
            this._RoleBack(newAppList, existAppList, this.userName, tranDate, out message);
            Helper.Alert(true, "Application is saved fail, SYSTEM ROLEBACK <br />" + message, this.lblError);
        }
    }

    protected void txtApplicationDate_TextChanged(object sender, EventArgs e)
    {
        this.ValidateCalculateAge();
        string message = string.Concat(this.ViewState["MESSAGE"]);
        if (!(bool)this.ViewState["IS_VALID"])
        {
            this.txtCustomerAge.Text = "";
            Helper.Alert(true, message, this.lblError);
        }
        else
        {
            this.CalculatePremium();
            this.CalculateRiderPremium();
        }
    }

    protected void ddlProvinceKh_SelectedIndexChanged1(object sender, EventArgs e)
    {
        this.txtProvinceEn.Text = this.ddlProvinceKh.SelectedValue;
        this.BindDistrict();
        this.MerchAddress();
    }

    protected void txtDateOfBirth_TextChanged(object sender, EventArgs e)
    {
        this.txtApplicationDate_TextChanged((object)null, (EventArgs)null);
    }

    protected void txtHouseNoKh_TextChanged(object sender, EventArgs e) { this.MerchAddress(); }

    protected void txtStreetKh_TextChanged(object sender, EventArgs e) { this.MerchAddress(); }

    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.hdfChannelItemID.Value = "";
        this.hdfChannelID.Value = this.ddlChannel.SelectedValue;
        Helper.BindChannelItem(this.ddlCompany, this.ddlChannel.SelectedValue);
        if (this.ddlChannel.SelectedIndex == 1)
        {
            this.ddlCompany.SelectedIndex = 1;
            this.ddlCompany.Attributes.Add("disabled", "disabled");
            this.hdfChannelItemID.Value = this.ddlCompany.SelectedValue;
            this.hdfChannelLocationID.Value = "0D696111-2590-4FA2-BCE6-C8B2D46648C9";
            this.ddlBranch.Items.Clear();
            this.ddlBranch.Attributes.Add("disabled", "disabled");
        }
        else if (this.ddlChannel.SelectedIndex == 0)
        {
            this.ddlCompany.Items.Clear();
            this.ddlBranch.Items.Clear();
        }
        else
            this.ddlCompany.Attributes.Remove("disabled");
    }

    private void Initialize()
    {
        this.dvAppList.InnerHtml = "<strong>Application To Be Issued:</strong><br />";
        string str = DateTime.Now.ToString("dd-MM-yyyy");
        this.ibtnPrintApplication.Attributes.Add("disabled", "disabled");
        this.ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
        btnPrintCert.Disabled = true;
       
        this.txtApplicationNumber.Attributes.Add("disabled", "disabled");
        this.txtSaleAgentID.Attributes.Add("disabled", "disabled");
        this.txtSaleAgentName.Attributes.Add("disabled", "disabled");
        this.txtPolicyNumber.Attributes.Add("disabled", "disabled");
        this.txtPolicyStatus.Attributes.Add("disabled", "disabled");
        this.txtApplicationDate.Text = str;
        this.txtCustomerAge.Attributes.Add("disabled", "disabled");
        Options.Bind(this.ddlNationality, (object)da_master_list.GetMasterList("NATIONALITY"), "DescKh", "Code", -1);
        List<bl_micro_product_config> microProductConfigList = new List<bl_micro_product_config>();
        List<bl_micro_product_config> productMicroProductSo = da_micro_product_config.ProductConfig.GetProductMicroProductSO();
        this.Session["PRODUCT_LIST"] = (object)productMicroProductSo;
        foreach (bl_micro_product_config microProductConfig in productMicroProductSo)
            this.ddlProduct.Items.Add(new ListItem(microProductConfig.Product_ID + " / " + microProductConfig.MarketingName, microProductConfig.Product_ID));
        this.txtProductName.Attributes.Add("disabled", "disabled");
        this.ddlTermOfCover.SelectedIndex = -1;
        this.ddlCoverType.SelectedIndex = -1;
        this.txtPremiumPaymentPeriod.Attributes.Add("disabled", "disabled");
        this.txtPremiumPaymentPeriod.Text = "1";
        this.txtReferrerId.Attributes.Add("disabled", "disabled");
        this.txtReferrerName.Attributes.Add("disabled", "disabled");
        this.lblBankStaffName.Attributes.CssStyle.Add("display", "none");
        this.lblClientTypeRelation.Attributes.CssStyle.Add("display", "none");
        this.txtBankStaffName.Attributes.CssStyle.Add("display", "none");
        this.ddlClientTypeRelation.Attributes.CssStyle.Add("display", "none");
        this.txtSumAssure.Attributes.Add("disabled", "disabled");
        this.txtBasicAfterDiscount.Attributes.Add("disabled", "disabled");
        this.txtPremium.Attributes.Add("disabled", "disabled");
        this.txtAnnualPremium.Attributes.Add("disabled", "disabled");
        this.txtTotalPremiumAfterDiscount.Attributes.Add("disabled", "disabled");
        this.txtTotalDiscountAmount.Attributes.Add("disabled", "disabled");
        this.txtTotalPremium.Attributes.Add("disabled", "disabled");
        this.txtRiderPremium.Attributes.Add("disabled", "disabled");
        this.txtRiderAnnualPremium.Attributes.Add("disabled", "disabled");
        this.txtRiderProductName.Attributes.Add("disabled", "disabled");
        this.lblAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.txtAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.lblRiderAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.txtRiderAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.txtRiderAfterDiscount.Attributes.Add("disabled", "disabled");
        this.ddlNumberofApplication.Attributes.Add("disabled", "disabled");
        this.txtAge.Text = "-";
        this.BindProvince();
        Helper.BindOccupation(this.ddlOccupation, "KH");
        Helper.BindRelation(this.ddlRelation, "KH");
        Helper.BindMasterRelation(this.ddlClientTypeRelation, "CLIENT_TYPE_RELATION");
        this.txtIssueDate.Attributes.Add("disabled", "disabled");
        this.txtIssueDate.Text = str;
        this.txtUserPremium.Attributes.Add("disabled", "disabled");
        this.txtEffectiveDate.Text = str;
        this.txtPaydate.Text = str;
        this.txtEffectiveDate.Attributes.Add("disabled", "disabled");
        this.txtPaydate.Attributes.Add("disabled", "disabled");
        this.txtPaymentRefNo.Attributes.Add("disabled", "disabled");
        this.btnIssue.Attributes.Add("disabled", "disabled");
    }

    private void BindPaymode()
    {
        bl_micro_product_config productConfig = this.ProductConfig;
        DataTable data = new DB().GetData(AppConfiguration.GetConnectionString(), "SELECT Pay_Mode_ID, MODE FROM Ct_Payment_Mode;");
        this.ddlPaymentMode.Items.Clear();
        this.ddlPaymentMode.Items.Add(new ListItem("---Select---", ""));
        if (productConfig.Product_ID == null)
            return;

        string payMode = string.Join(",", productConfig.PayMode);

        foreach (DataRow dataRow in data.Select("pay_mode_id in (" + payMode + ")"))
            this.ddlPaymentMode.Items.Add(new ListItem(dataRow["mode"].ToString(), dataRow["pay_mode_id"].ToString()));
    }

    public int CalculateAge(string DATE_OF_BIRTH, string COMPARE_DATE)
    {
        return Calculation.Culculate_Customer_Age(DATE_OF_BIRTH, Helper.FormatDateTime(COMPARE_DATE));
    }


    private void ValidateCalculateAge()
    {
        string str = "";
        bool flag = true;
        if (this.txtApplicationDate.Text.Trim() == "")
        {
            flag = false;
            str = "Application Date is required.";
        }
        else if (this.txtDateOfBirth.Text.Trim() == "")
        {
            flag = false;
            str = "Date of Birth is required.";
        }
        else if (this.txtDateOfBirth.Text.Trim() != "" && !Helper.IsDate(this.txtDateOfBirth.Text))
        {
            flag = false;
            str = "Date of Birth is invalid format.";
        }
        else if (this.txtApplicationDate.Text.Trim() != "" && !Helper.IsDate(this.txtApplicationDate.Text))
        {
            flag = false;
            str = "Application Date is invalid format.";
        }
        else
            this.txtCustomerAge.Text = string.Concat((object)this.CalculateAge(this.txtDateOfBirth.Text, this.txtApplicationDate.Text));
        this.ViewState["MESSAGE"] = (object)str;
        this.ViewState["IS_VALID"] = (object)flag;
    }

    private void ValidateCalculatePremium()
    {
        bool flag = true;
        string str = "";
        if (this.ddlProduct.SelectedIndex > 0)
        {
            bl_micro_product_config productConfig = this.ProductConfig;
            if (this.txtCustomerAge.Text.Trim() == "")
            {
                flag = false;
                str = "Customer Age is required.";
            }
            else if (this.txtCustomerAge.Text.Trim() != "" && !Helper.IsNumber(this.txtCustomerAge.Text.Trim()))
            {
                flag = false;
                str = "Customer Age is allowed only number.";
            }
            else if (this.ddlGender.SelectedIndex == 0)
            {
                flag = false;
                str = "Gender is required.";
            }
            else if (this.ddlProduct.SelectedIndex == 0)
            {
                flag = false;
                str = "Product is required.";
            }
            else if (this.txtSumAssure.Text.Trim() == "")
            {
                flag = false;
                str = "Sum Assured is required.";
            }
            else if (!Helper.IsNumber(this.txtSumAssure.Text.Trim()))
            {
                flag = false;
                str = "Sum Assured is allowed only number.";
            }
            else if (Convert.ToDouble(this.txtSumAssure.Text.Trim()) < productConfig.Sum_Min && Convert.ToDouble(this.txtSumAssure.Text.Trim()) > productConfig.Sum_Max)
            {
                flag = false;
                str = "Sum Assured is allowed [" + productConfig.Sum_Min + " - " + productConfig.Sum_Max + "]";
            }
            else if (this.ddlPaymentMode.SelectedValue.Trim() == "")
            {
                flag = false;
                str = "Payment mode is required.";
            }
            else
            {
                int age = Convert.ToInt32(this.txtCustomerAge.Text);
                if (age < productConfig.Age_Min || age > productConfig.Age_Max)
                {
                    flag = false;
                    str = "Age [" + age + "] is not allow.";
                }
            }
        }
        this.ViewState["MESSAGE"] = (object)str;
        this.ViewState["IS_VALID"] = (object)flag;
    }

    private void ValidateIssuePolicy()
    {
        this.ValidateForm();
        string str = string.Concat(this.ViewState["MESSAGE"]);
        bool flag = (bool)this.ViewState["IS_VALID"];
        if (flag)
        {
            if (this.txtIssueDate.Text.Trim() == "")
            {
                flag = false;
                str = "Issue Date is required.";
            }
            else if (this.txtIssueDate.Text.Trim() != "" && !Helper.IsDate(this.txtIssueDate.Text.Trim()))
            {
                flag = false;
                str = "Issue Date is invalid format.";
            }
            else if (!Helper.IsDate(this.txtEffectiveDate.Text.Trim()))
            {
                flag = false;
                str = "Effective Date is invalid format.";
            }
            else if (!Helper.IsDate(this.txtPaydate.Text.Trim()))
            {
                flag = false;
                str = "Pay Date is invalid format.";
            }
            else if (this.txtUserPremium.Text.Trim() == "")
            {
                flag = false;
                str = "Input Collected Premium is required.";
            }
            else if (this.txtUserPremium.Text.Trim() != "" && !Helper.IsAmount(this.txtUserPremium.Text.Trim()))
            {
                flag = false;
                str = "Input Collected Premium is allowed only number.";
            }
        }
        this.ViewState["MESSAGE"] = (object)str;
        this.ViewState["IS_VALID"] = (object)flag;
    }

    private void ValidateForm()
    {
        bool flag = true;
        string str = "";
        bl_micro_product_config productConfig = this.ProductConfig;
        if (this.userName == "")
        {
            flag = false;
            str = "Session was expired, please reload page.";
        }
        else if (this.ddlChannel.SelectedIndex == 0)
        {
            flag = false;
            str = "Channel is required.";
        }
        else if (this.ddlCompany.SelectedIndex == 0)
        {
            flag = false;
            str = "Company is required.";
        }
        else if (this.txtSaleAgentID.Text.Trim() == "")
        {
            flag = false;
            str = "Sale Agent ID is required.";
        }
        else if (this.txtSaleAgentName.Text.Trim() == "")
        {
            flag = false;
            str = "Sale Agent Name is required.";
        }
        else if (this.txtApplicationDate.Text.Trim() == "")
        {
            flag = false;
            str = "Application Date is required.";
        }
        else if (this.ddlIDType.SelectedIndex == 0)
        {
            flag = false;
            str = "ID Type is required.";
        }
        else if (this.txtIDNumber.Text.Trim() == "")
        {
            flag = false;
            str = "ID Number is required.";
        }
        else if (this.txtSurnameKh.Text.Trim() == "")
        {
            flag = false;
            str = "Surname In Khmer is required.";
        }
        else if (this.txtFirstNameKh.Text.Trim() == "")
        {
            flag = false;
            str = "First Name In Khmer is required.";
        }
        else if (this.txtSurnameEng.Text.Trim() == "")
        {
            flag = false;
            str = "Surname In English is required.";
        }
        else if (this.txtFirstNameEng.Text.Trim() == "")
        {
            flag = false;
            str = "First Name In English is required.";
        }
        else if (this.ddlNationality.SelectedIndex <= 0)
        {
            flag = false;
            str = "Nationality is required.";
        }
        else if (this.ddlGender.SelectedIndex == 0)
        {
            flag = false;
            str = "Gender is required.";
        }
        else if (this.txtDateOfBirth.Text.Trim() == "")
        {
            flag = false;
            str = "Date of Birth is required.";
        }
        else if (this.txtCustomerAge.Text.Trim() == "")
        {
            flag = false;
            str = "Age is required.";
        }
        else if (this.ddlMaritalStatus.SelectedIndex == 0)
        {
            flag = false;
            str = "Marital Status is required.";
        }
        else if (this.ddlOccupation.SelectedIndex == 0)
        {
            flag = false;
            str = "Occupation is required.";
        }
        else if (this.ddlProvinceKh.SelectedIndex == 0)
        {
            flag = false;
            str = "Provice In Khmer is required.";
        }
        else if (this.txtPhoneNumber.Text.Trim() == "")
        {
            flag = false;
            str = "Phone Number is required.";
        }
        else if (this.txtApplicationDate.Text.Trim() != "" && !Helper.IsDate(this.txtApplicationDate.Text.Trim()))
        {
            flag = false;
            str = "Application is invalid format.";
        }
        else if (this.txtDateOfBirth.Text.Trim() != "" && !Helper.IsDate(this.txtDateOfBirth.Text.Trim()))
        {
            flag = false;
            str = "Date of Birth is invalid format.";
        }
        else if (this.txtCustomerAge.Text.Trim() != "" && !Helper.IsNumber(this.txtCustomerAge.Text.Trim()))
        {
            flag = false;
            str = "Date of Birth is required.";
        }
        else if (this.txtProductName.Text.Trim() == "")
        {
            flag = false;
            str = "Product Name is required.";
        }
        else if (this.ddlTermOfCover.SelectedValue == "0")
        {
            flag = false;
            str = "Term of Cover is required.";
        }
        else if (this.ddlNumberofApplication.SelectedValue == "0")
        {
            flag = false;
            str = "Number of application is required.";
        }
        else if (this.ddlNumberofYear.SelectedValue == "0")
        {
            flag = false;
            str = "Number of year is required.";
        }
        else if (this.txtSumAssure.Text.Trim() == "")
        {
            flag = false;
            str = "Sum Assure is required.";
        }
        else if (this.ddlPaymentMode.SelectedValue.Trim() == "")
        {
            flag = false;
            str = "Payment Mode is required.";
        }
        else if (this.txtPremium.Text.Trim() == "")
        {
            flag = false;
            str = "Premium is required.";
        }
        else if (this.txtTotalPremium.Text.Trim() == "")
        {
            flag = false;
            str = "Total Amount is required.";
        }
        else if (this.txtTotalPremiumAfterDiscount.Text.Trim() == "")
        {
            flag = false;
            str = "Total Premium After discount is required.";
        }
        else if (productConfig.IsRequiredRider)
        {
            if (this.ddlRiderProduct.SelectedIndex == 0)
            {
                flag = false;
                str = "Rider Product is required.";
            }
            else if (this.ddlRiderSumAssure.SelectedIndex == 0)
            {
                flag = false;
                str = "Rider Sum Assure is required.";
            }
            else if (this.txtRiderPremium.Text.Trim() == "")
            {
                flag = false;
                str = "Rider Premium is required.";
            }
        }
        if (flag)
        {
            double num = 0.0;
            foreach (bl_micro_application_beneficiary beneficiary in this.BeneficiaryList)
                num += beneficiary.PERCENTAGE_OF_SHARE;
            if (this.BeneficiaryList.Count == 0)
            {
                flag = false;
                str = "Total Percentage of share must be equal to 100%.";
            }
            else if (this.ddlAnswer.SelectedIndex == 0)
            {
                flag = false;
                str = "Please answer a question.";
            }
            else if (this.ddlAnswer.SelectedValue == "1" && this.txtAnswerRemarks.Text.Trim() == "")
            {
                flag = false;
                str = "Please give detail of answer.";
            }
        }
        if (flag && productConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() && this.txtLoanNumber.Text.Trim() == "")
        {
            flag = false;
            str = "Loan number is required.";
        }
        this.ViewState["IS_VALID"] = (object)flag;
        this.ViewState["MESSAGE"] = (object)str;
    }

    private void CalculatePremium()
    {
        this.ValidateCalculatePremium();
        bool flag = (bool)this.ViewState["IS_VALID"];
        string.Concat(this.ViewState["MESSAGE"]);
        if (!flag)
            return;

        // double sumAssure = 0.0;
        if (this.ddlProduct.SelectedIndex <= 0)
            return;

        //sumAssure = this.txtSumAssure.Text.Trim() == "" ? 0.0 : Convert.ToDouble(this.txtSumAssure.Text.Trim());
        double totalSumAssure = this.txtTotalSumAssure.Text.Trim() == "" ? 0.0 : Convert.ToDouble(this.txtTotalSumAssure.Text.Trim());
        int gender = this.ddlGender.SelectedIndex == 0 ? -1 : Convert.ToInt32(this.ddlGender.SelectedValue);
        string productId = this.ddlProduct.SelectedValue;
        int cusAge = this.txtCustomerAge.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtCustomerAge.Text.Trim());
        int payMode = this.ddlPaymentMode.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlPaymentMode.SelectedValue);
        this._SumAssurePremium = new List<SumAssurePremiums>();
        double newSA = 0.0;
        int loopStep = 1;

        while (totalSumAssure > 0)
        {
            newSA = totalSumAssure > 5000 ? 5000 : totalSumAssure;
            double[] premium = Calculation.GetMicroProducPremium(productId, gender, cusAge, newSA, payMode);
            this._SumAssurePremium.Add(new SumAssurePremiums()
            {
                SumAssure = newSA,
                Premium = premium[1],
                AnnualPremium = premium[0]
            });
            if (loopStep == 1)/*flag main application*/
            {
                this.txtPremium.Text = premium[1].ToString();
                this.txtAnnualPremium.Text = premium[0].ToString();
            }
            totalSumAssure -= 5000;
            loopStep++;
        }

        this.CalculateDiscount();
    }

    private void CalculateRiderPremium()
    {
        if (this.ddlProduct.SelectedIndex <= 0 || this.ddlPaymentMode.SelectedIndex <= 0 || !(this.ddlRiderSumAssure.SelectedValue != ""))
            return;

        double sa = this.ddlRiderSumAssure.Items.Count == 0 ? 0.0 : Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue);
        int gender = this.ddlGender.SelectedIndex == 0 ? -1 : Convert.ToInt32(this.ddlGender.SelectedValue);
        string productId = this.ddlRiderProduct.SelectedValue;
        int cusAge = this.txtCustomerAge.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtCustomerAge.Text.Trim());
        int payMode = this.ddlPaymentMode.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlPaymentMode.SelectedValue);
        double[] riderPremium = Calculation.GetMicroProductRiderPremium(productId, gender, cusAge, sa, payMode);
        double premium = riderPremium[1];
        double annual = riderPremium[0];
        //if (this.txtPremium.Text.Trim() != "")
        //    Convert.ToDouble(this.txtPremium.Text.Trim());
        this.txtRiderPremium.Text = premium + "";
        this.txtRiderAnnualPremium.Text = annual + "";
        this.CalculateDiscount();
    }

    private void CalculateDiscount()
    {
        double basicDis = 0.0;
        double riderDis = 0.0;
        double basicSumAssured = 0.0;
        double riderSumAssured = 0.0;
        double totalPremium = 0.0;
        double basicAfterDis = 0.0;
        double riderAfterDis = 0.0;

        bl_micro_product_discount_config productDiscountConfig = new bl_micro_product_discount_config();
        string productID = this.hdfProductID.Value;
        string productRiderID = this.hdfRiderProductID.Value;
        basicSumAssured = Convert.ToDouble(this.txtSumAssure.Text.Trim() == "" ? "0" : this.txtSumAssure.Text.Trim());
        riderSumAssured = Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue == "" ? "0" : this.ddlRiderSumAssure.SelectedValue);
        if (productID != "" & productRiderID != "" && basicSumAssured > 0.0 && riderSumAssured >= 0.0)
        {
            bl_micro_product_discount_config productDiscount = da_micro_product_config.DiscountConfig.GetProductDiscount(productID, productRiderID, basicSumAssured, riderSumAssured, this.ddlClientType.SelectedValue);
            DateTime dateTime = Helper.FormatDateTime(this.txtApplicationDate.Text.Trim());
            if (productDiscount.ProductID != null && productDiscount.Status && dateTime < productDiscount.ExpiryDate)
            {
                basicDis = productDiscount.BasicDiscountAmount;
                riderDis = productDiscount.RiderDiscountAmount;
            }
        }
        totalPremium = Convert.ToDouble(this.txtPremium.Text.Trim() == "" ? "0" : this.txtPremium.Text.Trim()) + Convert.ToDouble(this.txtRiderPremium.Text.Trim() == "" ? "0" : this.txtRiderPremium.Text.Trim());
        basicDis = basicDis > 0.0 ? basicDis : Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
        riderDis = riderDis > 0.0 ? riderDis : Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
        basicAfterDis = Convert.ToDouble(this.txtPremium.Text.Trim() == "" ? "0" : this.txtPremium.Text.Trim()) - basicDis;
        riderAfterDis = Convert.ToDouble(this.txtRiderPremium.Text.Trim() == "" ? "0" : this.txtRiderPremium.Text.Trim()) - riderDis;

        this.txtBasicDiscount.Text = basicDis + "";
        this.txtBasicAfterDiscount.Text = basicAfterDis + "";
        this.txtRiderDiscount.Text = riderDis + "";
        this.txtRiderAfterDiscount.Text = riderAfterDis + "";
        this.txtTotalDiscountAmount.Text = (basicDis + riderDis) + "";
        this.txtTotalPremium.Text = totalPremium + "";
        this.txtTotalPremiumAfterDiscount.Text = (basicAfterDis + riderAfterDis) + "";
    }

    protected void ddlRiderProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ProductRider = da_micro_product_rider.GetMicroProductByProductID(this.ddlRiderProduct.SelectedValue);
        if (!string.IsNullOrWhiteSpace(this.ProductRider.PRODUCT_ID))
        {
            this.txtRiderProductName.Text = this.ProductRider.EN_TITLE;
            this.hdfRiderProductID.Value = this.ddlRiderProduct.SelectedValue;
        }
        else
        {
            this.txtRiderProductName.Text = "";
            this.hdfRiderProductID.Value = "";
        }
    }

    protected void ddlRiderSumAssure_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CalculateRiderPremium();
    }

    protected void txtSumAssure_TextChanged(object sender, EventArgs e)
    {
        bl_micro_product_config productConfig = this.ProductConfig;
        if (productConfig == null)
            return;
        if (this.txtSumAssure.Text.Trim() == "")
        {
            this.txtAnnualPremium.Text = "";
            Helper.Alert(true, "Sum Assured is required.", lblError);
        }
        else if (!Helper.IsNumber(this.txtSumAssure.Text.Trim()))
            Helper.Alert(true, "Sum Assured is allowed only number.", lblError);
        else if (Convert.ToDouble(this.txtSumAssure.Text.Trim()) < productConfig.Sum_Min || Convert.ToDouble(this.txtSumAssure.Text.Trim()) > productConfig.Sum_Max)
        {
            Helper.Alert(true, "Sum Assured is allowed [" + productConfig.Sum_Min + " - " + productConfig.Sum_Max + "].", lblError);
            this.txtSumAssure.Text = "";
            this.txtPremium.Text = "";
        }
        else
        {
            this.txtTotalSumAssure.Text = this.txtSumAssure.Text;
            this.txtTotalSumAssure_TextChanged((object)null, (EventArgs)null);
            this.CalculatePremium();
        }
    }

    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!(this.ddlPaymentMode.SelectedValue != ""))
            return;
        this.CalculatePremium();
        if (string.IsNullOrWhiteSpace(this.ProductConfig.RiderProductID))
            return;
        this.CalculateRiderPremium();
    }

    protected void ddlGender_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CalculatePremium();
    }

    protected void ddlDistrictKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindCommune();
        this.txtDistrictEn.Text = this.ddlDistrictKh.SelectedValue;
        this.MerchAddress();
    }

    protected void ddlCommuneKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindVillage();
        this.txtCommuneEn.Text = this.ddlCommuneKh.SelectedValue;
        this.MerchAddress();
    }

    protected void ddlVillageKh_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtVillageEn.Text = this.ddlVillageKh.SelectedValue;
        this.MerchAddress();
    }

    private void MerchAddress()
    {
        // Check if the selected province is Phnom Penh
        if (this.ddlProvinceKh.SelectedValue.ToUpper() == "12/PHNOM PENH" || this.ddlProvinceKh.SelectedValue.ToUpper() == "PHNOM PENH")
        {
            // If Phnom Penh, use specific terms for address
            this.txtBenAddress.Text =
                (string.IsNullOrWhiteSpace(this.txtHouseNoKh.Text) ? "" : "ផ្ទះលេខ " + this.txtHouseNoKh.Text) + " " +
                (string.IsNullOrWhiteSpace(this.txtStreetKh.Text) ? "" : "ផ្លូវលេខ " + this.txtStreetKh.Text.Trim()) + " " +
                (string.IsNullOrWhiteSpace(this.ddlVillageKh.SelectedValue) ? "" : "ភូមិ " + this.ddlVillageKh.SelectedItem.Text.Trim()) + " " +
                (string.IsNullOrWhiteSpace(this.ddlCommuneKh.SelectedValue) ? "" : "សង្កាត់ " + this.ddlCommuneKh.SelectedItem.Text.Trim()) + " " +
                (string.IsNullOrWhiteSpace(this.ddlDistrictKh.SelectedValue) ? "" : "ខណ្ឌ " + this.ddlDistrictKh.SelectedItem.Text.Trim()) + " " +
                (string.IsNullOrWhiteSpace(this.ddlProvinceKh.SelectedValue) ? "" : "ក្រុង " + this.ddlProvinceKh.SelectedItem.Text);
        }
        else
        {
            // If not Phnom Penh, use different terms for address
            if (!string.IsNullOrWhiteSpace(this.ddlProvinceKh.SelectedValue))
            {
                this.txtBenAddress.Text =
                    (string.IsNullOrWhiteSpace(this.txtHouseNoKh.Text) ? "" : "ផ្ទះលេខ " + this.txtHouseNoKh.Text) + " " +
                    (string.IsNullOrWhiteSpace(this.txtStreetKh.Text) ? "" : "ផ្លូវលេខ " + this.txtStreetKh.Text.Trim()) + " " +
                    (string.IsNullOrWhiteSpace(this.ddlVillageKh.SelectedValue) ? "" : "ភូមិ " + this.ddlVillageKh.SelectedItem.Text.Trim()) + " " +
                    (string.IsNullOrWhiteSpace(this.ddlCommuneKh.SelectedValue) ? "" : "ឃុំ " + this.ddlCommuneKh.SelectedItem.Text.Trim()) + " " +
                    (string.IsNullOrWhiteSpace(this.ddlDistrictKh.SelectedValue) ? "" : "ស្រុក " + this.ddlDistrictKh.SelectedItem.Text.Trim()) + " " +
                    (string.IsNullOrWhiteSpace(this.ddlProvinceKh.SelectedValue) ? "" : "ខេត្ត " + this.ddlProvinceKh.SelectedItem.Text.Trim());
            }
        }
    }

    private void RoleBack()
    {
        da_micro_application_customer.DeleteApplicationCustomer(this.hdfApplicationCustomerID.Value);
        da_micro_application.DeleteApplication(this.hdfApplicationNumber.Value);
        da_micro_application_insurance.DeleteApplicationInsurance(this.hdfApplicationNumber.Value);
        da_micro_application_insurance_rider.DeleteApplicationInsuranceRider(this.hdfApplicationNumber.Value);
        da_micro_application_beneficiary.DeleteApplicationBeneficiary(this.hdfApplicationNumber.Value);
        da_micro_application_questionaire.DeleteQuestionaire(this.hdfApplicationNumber.Value);
        this.hdfApplicationCustomerID.Value = "";
        this.hdfApplicationNumber.Value = "";
        this.hdfApplicationID.Value = "";
        Helper.Alert(false, string.Concat(this.ViewState["MESSAGE"]), this.lblError);
    }


    private void BindExisting()
    {
        try
        {
            bool flag = true;
            foreach (bl_micro_application.ApplicationFilter applicationFilter in da_micro_application.GetApplicationNumberMainSub(this.hdfApplicationNumber.Value))
            {
                if (applicationFilter.ApplicationNumber == this.hdfApplicationNumber.Value && !string.IsNullOrWhiteSpace(applicationFilter.MainApplicationNumber))
                {
                    this.hdfApplicationNumber.Value = applicationFilter.MainApplicationNumber;
                    break;
                }
            }
            bl_micro_application_details applicationDetail = da_micro_application.GetApplicationDetail(this.hdfApplicationNumber.Value);
            if (applicationDetail != null)
            {
                if (!string.IsNullOrWhiteSpace(applicationDetail.PolicyNumber))
                {
                    this.txtPolicyNumber.Text = applicationDetail.PolicyNumber;
                    this.txtPolicyStatus.Text = applicationDetail.PolicyStatus;
                    this.hdfPolicyID.Value = applicationDetail.PolicyId;
                    this.txtIssueDate.Text = applicationDetail.IssueDate.ToString("dd-MM-yyyy");
                    this.txtUserPremium.Text = string.Concat((object)applicationDetail.CollectedPremium);
                    this.txtPaymentRefNo.Text = applicationDetail.PaymentReferenceNo;
                    this.txtEffectiveDate.Text = applicationDetail.EffectiveDate.ToString("dd-MM-yyyy");
                    this.DisabledAllControls();
                    this.btnIssue.Attributes.Add("disabled", "disabled");
                    this.txtIssueDate.Attributes.Add("disabled", "disabled");
                    this.txtEffectiveDate.Attributes.Add("disabled", "disabled");
                    this.txtUserPremium.Attributes.Add("disabled", "disabled");
                    this.txtPaymentRefNo.Attributes.Add("disabled", "disabled");
                    this.ibtnSave.Attributes.Add("disabled", "disabled");
                    this.ibtnPrintCertificate.Attributes.Remove("disabled");
                    btnPrintCert.Disabled = false;
                }
                else
                {
                    
                    this.ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                    this.txtUserPremium.Attributes.Add("disabled", "disabled");
                    this.txtPaymentRefNo.Attributes.Remove("disabled");
                    this.btnIssue.Attributes.Remove("disabled");
                    this.txtIssueDate.Attributes.Remove("disabled");
                    this.txtEffectiveDate.Attributes.Remove("disabled");
                    this.txtPaydate.Attributes.Remove("disabled");
                    this.ibtnSave.Attributes.Remove("disabled");
                    this.txtPolicyNumber.Text = "";
                    this.txtPolicyStatus.Text = "";
                    this.txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    this.txtEffectiveDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    this.ViewState["VS_BAK_DATE"] = (object)DateTime.Now;
                    flag = da_micro_application.BackupApplication(this.hdfApplicationID.Value, "UPDATE", this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"])));
                }
                if (flag)
                {
                    Helper.BindChannel(this.ddlChannel);
                    Helper.BindChannelItem(this.ddlCompany, applicationDetail.Application.CHANNEL_ID);
                    Helper.BindChanneLocation(this.ddlBranch, applicationDetail.Application.CHANNEL_ITEM_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlChannel, applicationDetail.Application.CHANNEL_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlCompany, applicationDetail.Application.CHANNEL_ITEM_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlBranch, applicationDetail.Application.CHANNEL_LOCATION_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlClientType, applicationDetail.Application.CLIENT_TYPE);
                    this.ddlClientType_SelectedIndexChanged(null, null);
                    this.txtBankStaffName.Text = applicationDetail.Application.CLIENT_TYPE_REMARKS;
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlClientTypeRelation, applicationDetail.Application.CLIENT_TYPE_RELATION);
                    this.hdfProductID.Value = applicationDetail.Insurance.PRODUCT_ID;
                    this.txtApplicationNumber.Text = applicationDetail.Application.APPLICATION_NUMBER;
                    this.txtApplicationDate.Text = applicationDetail.Application.APPLICATION_DATE.ToString("dd-MM-yyyy");
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlBranch, applicationDetail.OfficeCode);
                    this.txtSaleAgentID.Text = applicationDetail.Application.SALE_AGENT_ID;
                    this.txtSaleAgentName.Text = applicationDetail.Application.SALE_AGENT_NAME;
                    this.hdfApplicationNumber.Value = applicationDetail.Application.APPLICATION_NUMBER;
                    this.hdfChannelItemID.Value = applicationDetail.Application.CHANNEL_ITEM_ID;
                    this.hdfChannelLocationID.Value = applicationDetail.Application.CHANNEL_LOCATION_ID;
                    this.hdfSaleAgentID.Value = applicationDetail.Application.SALE_AGENT_ID;
                    this.hdfApplicationID.Value = applicationDetail.Application.APPLICATION_ID;
                    this.txtReferrerId.Text = applicationDetail.Application.REFERRER_ID;
                    this.txtReferrerName.Text = applicationDetail.Application.REFERRER;
                    this.hdfApplicationCustomerID.Value = applicationDetail.Application.APPLICATION_CUSTOMER_ID;
                    this.hdfApplicationNumber.Value = applicationDetail.Application.APPLICATION_NUMBER;
                    this.txtLoanNumber.Text = applicationDetail.Application.LoanNumber;
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlIDType, applicationDetail.Customer.ID_TYPE);
                    this.txtIDNumber.Text = applicationDetail.Customer.ID_NUMBER;
                    this.txtSurnameEng.Text = applicationDetail.Customer.LAST_NAME_IN_ENGLISH;
                    this.txtFirstNameEng.Text = applicationDetail.Customer.FIRST_NAME_IN_ENGLISH;
                    this.txtSurnameKh.Text = applicationDetail.Customer.LAST_NAME_IN_KHMER;
                    this.txtFirstNameKh.Text = applicationDetail.Customer.FIRST_NAME_IN_KHMER;
                    Helper.SelectedDropDownListIndex("TEXT", this.ddlNationality, applicationDetail.Customer.NATIONALITY);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlGender, applicationDetail.Customer.GENDER);
                    this.txtDateOfBirth.Text = applicationDetail.Customer.DATE_OF_BIRTH.ToString("dd-MM-yyyy");
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlMaritalStatus, applicationDetail.Customer.MARITAL_STATUS);
                    Helper.SelectedDropDownListIndex("TEXT", this.ddlOccupation, applicationDetail.Customer.OCCUPATION);
                    this.txtPhoneNumber.Text = applicationDetail.Customer.PHONE_NUMBER1;
                    this.txtEmail.Text = applicationDetail.Customer.EMAIL1;
                    this.txtCustomerAge.Text = string.Concat((object)this.CalculateAge(this.txtDateOfBirth.Text, this.txtApplicationDate.Text));
                    this.txtHouseNoEn.Text = applicationDetail.Customer.HOUSE_NO_EN;
                    this.txtStreetEn.Text = applicationDetail.Customer.STREET_NO_EN;
                    string str1 = applicationDetail.Customer.VILLAGE_CODE == "" ? applicationDetail.Customer.VILLAGE_EN : applicationDetail.Customer.VILLAGE_CODE + "/" + applicationDetail.Customer.VILLAGE_EN;
                    string str2 = applicationDetail.Customer.COMMUNE_CODE == "" ? applicationDetail.Customer.COMMUNE_EN : applicationDetail.Customer.COMMUNE_CODE + "/" + applicationDetail.Customer.COMMUNE_EN;
                    string str3 = applicationDetail.Customer.DISTRICT_CODE == "" ? applicationDetail.Customer.DISTRICT_EN : applicationDetail.Customer.DISTRICT_CODE + "/" + applicationDetail.Customer.DISTRICT_EN;
                    string str4 = applicationDetail.Customer.PROVINCE_CODE == "" ? applicationDetail.Customer.PROVINCE_EN : applicationDetail.Customer.PROVINCE_CODE + "/" + applicationDetail.Customer.PROVINCE_EN;
                    this.txtVillageEn.Text = str1;
                    this.txtCommuneEn.Text = str2;
                    this.txtDistrictEn.Text = str3;
                    this.txtProvinceEn.Text = str4;
                    this.txtHouseNoKh.Text = applicationDetail.Customer.HOUSE_NO_KH;
                    this.txtStreetKh.Text = applicationDetail.Customer.STREET_NO_KH;
                    Helper.SelectedDropDownListIndex("TEXT", this.ddlProvinceKh, applicationDetail.Customer.PROVINCE_KH);
                    this.BindDistrict();
                    Helper.SelectedDropDownListIndex("TEXT", this.ddlDistrictKh, applicationDetail.Customer.DISTRICT_KH);
                    this.BindCommune();
                    Helper.SelectedDropDownListIndex("TEXT", this.ddlCommuneKh, applicationDetail.Customer.COMMUNE_KH);
                    this.BindVillage();
                    Helper.SelectedDropDownListIndex("TEXT", this.ddlVillageKh, applicationDetail.Customer.VILLAGE_KH);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlProduct, applicationDetail.Insurance.PRODUCT_ID);
                    this.ddlProduct_SelectedIndexChanged((object)null, (EventArgs)null);
                    this.txtProductName.Text = applicationDetail.Insurance.PRODUCT_NAME;
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlTermOfCover, string.Concat((object)applicationDetail.Insurance.TERME_OF_COVER));
                    this.txtPremiumPaymentPeriod.Text = string.Concat((object)applicationDetail.Insurance.PAYMENT_PERIOD);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofApplication, string.Concat((object)applicationDetail.Application.NumbersOfApplicationFirstYear));
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofYear, string.Concat((object)applicationDetail.Application.NumbersOfPurchasingYear));
                    this.BindPayMode(this.ProductConfig.PayMode);
                    this.txtSumAssure.Text = string.Concat((object)applicationDetail.Insurance.SUM_ASSURE);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlPaymentMode, string.Concat((object)applicationDetail.Insurance.PAY_MODE));
                    this.txtPremium.Text = string.Concat((object)applicationDetail.Insurance.PREMIUM);
                    this.txtAnnualPremium.Text = string.Concat((object)applicationDetail.Insurance.ANNUAL_PREMIUM);
                    this.txtBasicDiscount.Text = string.Concat((object)applicationDetail.Insurance.DISCOUNT_AMOUNT);
                    this.txtBasicAfterDiscount.Text = string.Concat((object)applicationDetail.Insurance.TOTAL_AMOUNT);
                    if (this.ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
                    {
                        bl_micro_application application = applicationDetail.Application;
                        this.txtHolderName.Text = application.PolicyholderName;
                        this.txtHolderAddress.Text = application.PolicyholderAddress;
                        this.txtHolderDOB.Text = application.PolicyholderDOB.Year == 1900 ? "" : application.PolicyholderDOB.ToString("dd-MM-yyyy");
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlHolderIdType, string.Concat((object)application.PolicyholderIDType));
                        this.txtHolderIdNo.Text = application.PolicyholderIDNo;
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlHolderGender, string.Concat((object)application.PolicyholderGender));
                        this.benTitle.InnerHtml = "Contingent Beneficiary(s)";
                    }
                    else
                    {
                        this.benTitle.InnerHtml = "Beneficiary(s)";
                        this.txtHolderName.Text = "";
                        this.txtHolderAddress.Text = "";
                        this.txtHolderDOB.Text = "";
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlHolderIdType, "-1");
                        this.txtHolderIdNo.Text = "";
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlHolderGender, "-1");
                    }
                    if (applicationDetail.Rider != null)
                    {
                        this.hdfRiderProductID.Value = applicationDetail.Rider.PRODUCT_ID;
                        this.BindRiderSA(this.ProductConfig.RiderSumAssuredRange);
                        this.txtRiderProductName.Text = applicationDetail.Rider.PRODUCT_NAME;
                        this.txtRiderPremium.Text = string.Concat((object)applicationDetail.Rider.PREMIUM);
                        this.txtRiderAnnualPremium.Text = string.Concat((object)applicationDetail.Rider.ANNUAL_PREMIUM);
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlRiderSumAssure, string.Concat((object)applicationDetail.Rider.SUM_ASSURE));
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlRiderProduct, applicationDetail.Rider.PRODUCT_ID ?? "");
                        this.txtRiderDiscount.Text = string.Concat((object)applicationDetail.Rider.DISCOUNT_AMOUNT);
                        this.txtRiderAfterDiscount.Text = string.Concat((object)applicationDetail.Rider.TOTAL_AMOUNT);
                        if (!string.IsNullOrEmpty(this.dvRider.Attributes["style"]))
                        {
                            AttributeCollection attributes;
                            (attributes = this.dvRider.Attributes)["style"] = attributes["style"] + " display:block;";
                        }
                        else
                            this.dvRider.Attributes.Add("style", "display:block;");
                    }
                    else
                    {
                        this.hdfRiderProductID.Value = "";
                        this.txtRiderProductName.Text = "";
                        this.txtRiderPremium.Text = "";
                        this.txtRiderAnnualPremium.Text = "";
                        this.txtRiderDiscount.Text = "";
                        this.txtRiderAfterDiscount.Text = "";
                        if (string.IsNullOrWhiteSpace(this.ProductConfig.RiderProductID))
                        {
                            if (!string.IsNullOrEmpty(this.dvRider.Attributes["style"]))
                            {
                                AttributeCollection attributes;
                                (attributes = this.dvRider.Attributes)["style"] = attributes["style"] + " display:none;";
                            }
                            else
                                this.dvRider.Attributes.Add("style", "display:none;");
                        }
                    }
                    double num1 = this.txtPremium.Text.Trim() != "" ? Convert.ToDouble(this.txtPremium.Text) : 0.0;
                    double num2 = this.txtRiderPremium.Text.Trim() != "" ? Convert.ToDouble(this.txtRiderPremium.Text) : 0.0;
                    double num3 = this.txtBasicDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtBasicDiscount.Text) : 0.0;
                    double num4 = this.txtRiderDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtRiderDiscount.Text) : 0.0;
                    double num5 = this.txtBasicAfterDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtBasicAfterDiscount.Text) : 0.0;
                    double num6 = this.txtRiderAfterDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtRiderAfterDiscount.Text) : 0.0;
                    this.txtTotalPremium.Text = string.Concat((object)(num1 + num2));
                    this.txtTotalDiscountAmount.Text = string.Concat((object)(num3 + num4));
                    this.txtTotalPremiumAfterDiscount.Text = string.Concat((object)(num5 + num6));
                    if (applicationDetail.PrimaryBeneficiary != null)
                    {
                        this.txtPrimaryBenName.Text = applicationDetail.PrimaryBeneficiary.FullName;
                        this.txtPrimaryBenLoan.Text = applicationDetail.PrimaryBeneficiary.LoanNumber;
                        this.txtPrimaryBenAddress.Text = applicationDetail.PrimaryBeneficiary.Address;
                        if (!string.IsNullOrEmpty(this.dvPrimaryBeneficiary.Attributes["style"]))
                        {
                            AttributeCollection attributes;
                            (attributes = this.dvPrimaryBeneficiary.Attributes)["style"] = attributes["style"] + " display:block;";
                        }
                        else
                            this.dvPrimaryBeneficiary.Attributes.Add("style", "display:block;");
                    }
                    else
                    {
                        this.txtPrimaryBenName.Text = "";
                        this.txtPrimaryBenLoan.Text = "";
                        this.txtPrimaryBenAddress.Text = "";
                        if (!string.IsNullOrEmpty(this.dvPrimaryBeneficiary.Attributes["style"]))
                        {
                            AttributeCollection attributes;
                            (attributes = this.dvPrimaryBeneficiary.Attributes)["style"] = attributes["style"] + " display:none;";
                        }
                        else
                            this.dvPrimaryBeneficiary.Attributes.Add("style", "display:none;");
                    }
                    this.BeneficiaryList = applicationDetail.Beneficiaries;
                    this.gvBen.DataSource = (object)this.BeneficiaryList;
                    this.gvBen.DataBind();
                    Helper.SelectedDropDownListIndex("Value", this.ddlAnswer, string.Concat((object)applicationDetail.Questionaire.ANSWER));
                    this.txtAnswerRemarks.Text = applicationDetail.Questionaire.ANSWER_REMARKS;
                    string strAlter = "<strong>APPLICATION(S) TO BE ISSUED:</strong><br />";
                    int rowSubApp = 2;
                    double totalPremiumCollection = applicationDetail.Insurance.TOTAL_AMOUNT + (applicationDetail.Rider != null ? applicationDetail.Rider.TOTAL_AMOUNT : 0.0);
                    double sumAssure = applicationDetail.Insurance.SUM_ASSURE;
                    strAlter += "1. " + applicationDetail.Application.APPLICATION_NUMBER + " - NEW (MAIN)<br />";
                    foreach (bl_micro_application_details.SubApplication subApplication in applicationDetail.SubApplications)
                    {
                        if (subApplication.ClientType.ToUpper() != "REPAYMENT")
                            sumAssure += subApplication.SumAssure;
                        totalPremiumCollection += subApplication.BasicAmount + subApplication.RiderAmount;
                        strAlter += rowSubApp + ". " + subApplication.ApplicationNumber + (subApplication.ClientType.ToUpper() == "SELF" ? " - NEW" : " - REPAYMENT") + "<br />";
                        ++rowSubApp;
                    }
                    this.dvAppList.InnerHtml = strAlter;
                    this.txtTotalSumAssure.Text = string.Concat((object)sumAssure);
                    this.txtUserPremium.Text = string.Concat((object)totalPremiumCollection);
                    this.ibtnPrintApplication.Attributes.Remove("disabled");
                    this.CalculatePremium();
                }
                else
                {
                    this.ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                    this.ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                    this.DisabledAllControls();
                    Helper.Alert(true, "System is error while try to backup application", this.lblError);
                }
            }
            else
            {
                this.btnIssue.Attributes.Add("disabled", "disabled");
                this.txtIssueDate.Attributes.Add("disabled", "disabled");
                this.txtUserPremium.Attributes.Add("disabled", "disabled");
                this.txtPaymentRefNo.Attributes.Add("disabled", "disabled");
                this.ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
                this.ibtnPrintApplication.Attributes.Add("disabled", "disabled");
                this.Initialize();
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, this.lblError);
        }
    }

    private void DisabledAllControls()
    {
        this.DisabledControls();
        this.txtApplicationDate.Attributes.Add("disabled", "disabled");
        this.ddlChannel.Attributes.Add("disabled", "disabled");
        this.ddlCompany.Attributes.Add("disabled", "disabled");
        this.ddlBranch.Attributes.Add("disabled", "disabled");
        this.ddlProduct.Attributes.Add("disabled", "disabled");
        this.ddlRiderProduct.Attributes.Add("disabled", "disabled");
        this.ddlRiderSumAssure.Attributes.Add("disabled", "disabled");
        this.txtLoanNumber.Attributes.Add("disabled", "disabled");
        this.ddlClientType.Attributes.Add("disabled", "disabled");
        this.ddlIDType.Attributes.Add("disabled", "disabled");
        this.txtIDNumber.Attributes.Add("disabled", "disabled");
        this.txtSurnameKh.Attributes.Add("disabled", "disabled");
        this.txtFirstNameKh.Attributes.Add("disabled", "disabled");
        this.txtSurnameEng.Attributes.Add("disabled", "disabled");
        this.txtFirstNameEng.Attributes.Add("disabled", "disabled");
        this.ddlNationality.Attributes.Add("disabled", "disabled");
        this.ddlGender.Attributes.Add("disabled", "disabled");
        this.txtDateOfBirth.Attributes.Add("disabled", "disabled");
        this.ddlMaritalStatus.Attributes.Add("disabled", "disabled");
        this.ddlOccupation.Attributes.Add("disabled", "disabled");
        this.txtPhoneNumber.Attributes.Add("disabled", "disabled");
        this.txtEmail.Attributes.Add("disabled", "disabled");
        this.txtStreetEn.Attributes.Add("disabled", "disabled");
        this.txtHouseNoEn.Attributes.Add("disabled", "disabled");
        this.txtVillageEn.Attributes.Add("disabled", "disabled");
        this.txtCommuneEn.Attributes.Add("disabled", "disabled");
        this.txtDistrictEn.Attributes.Add("disabled", "disabled");
        this.txtProvinceEn.Attributes.Add("disabled", "disabled");
        this.txtHouseNoKh.Attributes.Add("disabled", "disabled");
        this.txtStreetKh.Attributes.Add("disabled", "disabled");
        this.ddlVillageKh.Attributes.Add("disabled", "disabled");
        this.ddlCommuneKh.Attributes.Add("disabled", "disabled");
        this.ddlDistrictKh.Attributes.Add("disabled", "disabled");
        this.ddlProvinceKh.Attributes.Add("disabled", "disabled");
        this.txtBenAddress.Attributes.Add("disabled", "disabled");
        this.txtAge.Attributes.Add("disabled", "disabled");
        this.ddlRelation.Attributes.Add("disabled", "disabled");
        this.txtPercentageOfShare.Attributes.Add("disabled", "disabled");
        this.txtFullName.Attributes.Add("disabled", "disabled");
        this.ddlAnswer.Attributes.Add("disabled", "disabled");
        this.txtAnswerRemarks.Attributes.Add("disabled", "disabled");
        this.txtHolderName.Attributes.Add("disabled", "disabled");
        this.ddlHolderGender.Attributes.Add("disabled", "disabled");
        this.ddlHolderIdType.Attributes.Add("disabled", "disabled");
        this.txtHolderDOB.Attributes.Add("disabled", "disabled");
        this.txtHolderIdNo.Attributes.Add("disabled", "disabled");
        this.txtHolderAddress.Attributes.Add("disabled", "disabled");
    }

    private void DisabledControls()
    {
        this.txtApplicationNumber.Attributes.Add("disabled", "disabled");
        this.txtSaleAgentID.Attributes.Add("disabled", "disabled");
        this.txtSaleAgentName.Attributes.Add("disabled", "disabled");
        this.txtPolicyNumber.Attributes.Add("disabled", "disabled");
        this.txtPolicyStatus.Attributes.Add("disabled", "disabled");
        this.txtCustomerAge.Attributes.Add("disabled", "disabled");
        this.txtProductName.Attributes.Add("disabled", "disabled");
        this.ddlTermOfCover.Attributes.Add("disabled", "disabled");
        this.txtPremiumPaymentPeriod.Attributes.Add("disabled", "disabled");
        this.txtSumAssure.Attributes.Add("disabled", "disabled");
        this.ddlPaymentMode.Attributes.Add("disabled", "disabled");
        this.txtPremium.Attributes.Add("disabled", "disabled");
        this.txtAnnualPremium.Attributes.Add("disabled", "disabled");
        this.txtRiderPremium.Attributes.Add("disabled", "disabled");
        this.txtRiderAnnualPremium.Attributes.Add("disabled", "disabled");
        this.txtTotalPremium.Attributes.Add("disabled", "disabled");
        this.lblAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.txtAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.lblRiderAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.txtRiderAnnualPremium.Attributes.CssStyle.Add("display", "none");
        this.txtTotalPremiumAfterDiscount.Attributes.Add("disabled", "disabled");
        this.txtPercentageOfShare.Attributes.Add("disabled", "disabled");
    }

    protected void btnSearch_Click(object sender, EventArgs e) { this.BindExisting(); }

    protected void ibtnClear_Click(object sender, ImageClickEventArgs e)
    {
        this.Page_Load(null,null);
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ddlBranch.Items.Clear();
        Options.Bind(this.ddlBranch, (object)da_channel.GetChannelLocationListByChannelItemID(this.ddlCompany.SelectedValue), bl_channel_location.NAME.OfficeName, bl_channel_location.NAME.ChannelItemId, -1);
        this.ddlBranch.Attributes.Remove("disabled");
        this.hdfChannelItemID.Value = this.ddlCompany.SelectedValue;
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.hdfChannelLocationID.Value = this.ddlBranch.SelectedValue;
    }

    protected void tblAddBen_Click(object sender, EventArgs e)
    {
        if (this.txtFullName.Text.Trim() == "")
            Helper.Alert(true, "Beneficiary name is required", this.lblError);
        else if (this.txtBenIdNo.Text.Trim() != "" && this.ddlBenIdType.SelectedValue == "-1")
            Helper.Alert(true, "Beneficiary Id Type is required", this.lblError);
        else if (this.ddlBenGender.SelectedValue == "-1")
            Helper.Alert(true, "Beneficiary Gender is required", this.lblError);
        else if (this.ddlRelation.SelectedValue == "")
            Helper.Alert(true, "Beneficiary Relation is required", this.lblError);
        else if (!Helper.IsNumber(this.txtPercentageOfShare.Text.Trim()))
            Helper.Alert(true, "Percentage of share is required as number.", this.lblError);
        else if (this.txtBenAddress.Text.Trim() == "")
            Helper.Alert(true, "Beneficiary Address is required.", this.lblError);
        else if (this.txtBenDOB.Text.Trim() != "" && !Helper.IsDate(this.txtBenDOB.Text.Trim()))
        {
            Helper.Alert(true, "Beneficiary DOB is required as [DD-MM-YYYY].", this.lblError);
        }
        else
        {
            double num1 = 0.0;
            double num2 = Convert.ToDouble(this.txtPercentageOfShare.Text.Trim());
            foreach (bl_micro_application_beneficiary beneficiary in this.BeneficiaryList)
                num1 += beneficiary.PERCENTAGE_OF_SHARE;
            if (num1 + num2 - this._BenPercentage > 100.0)
            {
                Helper.Alert(true, "Total Percentage of share must be equal to 100%.", this.lblError);
            }
            else
            {
                bl_micro_application_beneficiary applicationBeneficiary = new bl_micro_application_beneficiary()
                {
                    FULL_NAME = this.txtFullName.Text.Trim(),
                    AGE = this.txtAge.Text.Trim(),
                    Gender = Convert.ToInt32(this.ddlBenGender.SelectedValue),
                    DOB = this.txtBenDOB.Text.Trim() == "" ? new DateTime(1900, 1, 1) : Helper.FormatDateTime(this.txtBenDOB.Text.Trim()),
                    IdType = Convert.ToInt32(this.ddlBenIdType.SelectedValue),
                    IdNo = this.txtBenIdNo.Text.Trim(),
                    ADDRESS = this.txtBenAddress.Text.Trim(),
                    RELATION = this.ddlRelation.SelectedValue,
                    PERCENTAGE_OF_SHARE = Convert.ToDouble(this.txtPercentageOfShare.Text.Trim())
                };
                if (this.hdfBeneficiaryId.Value == "")
                {
                    applicationBeneficiary.ID = (this.BeneficiaryList.Count + 1)+"";
                    this.BeneficiaryList.Add(applicationBeneficiary);
                }
                else
                {
                    bl_micro_application_beneficiary beneficiary = this.BeneficiaryList[this._BenRowIndex];
                    beneficiary.FULL_NAME = this.txtFullName.Text.Trim();
                    beneficiary.AGE = this.txtAge.Text.Trim();
                    beneficiary.Gender = Convert.ToInt32(this.ddlBenGender.SelectedValue);
                    beneficiary.DOB = this.txtBenDOB.Text.Trim() == "" ? new DateTime(1900, 1, 1) : Helper.FormatDateTime(this.txtBenDOB.Text.Trim());
                    beneficiary.IdType = Convert.ToInt32(this.ddlBenIdType.SelectedValue);
                    beneficiary.IdNo = this.txtBenIdNo.Text.Trim();
                    beneficiary.ADDRESS = this.txtBenAddress.Text.Trim();
                    beneficiary.RELATION = this.ddlRelation.SelectedValue;
                    beneficiary.PERCENTAGE_OF_SHARE = Convert.ToDouble(this.txtPercentageOfShare.Text.Trim());
                }

                
            }
        }
        this.gvBen.DataSource = this.BeneficiaryList;
        this.gvBen.DataBind();
        this.hdfBeneficiaryId.Value = "";
        /*Clear beneficiary information*/
        txtFullName.Text = "";
        txtBenDOB.Text = "";
        ddlBenGender.SelectedIndex = ddlBenGender.Items.Count > 0 ? 0 : -1;
        ddlBenIdType.SelectedIndex = ddlBenIdType.Items.Count > 0 ? 0 : -1;
        txtBenIdNo.Text = "";
        txtAge.Text = "-";
        ddlRelation.SelectedIndex = ddlRelation.Items.Count > 0 ? 0 : -1;
        txtPercentageOfShare.Text = "";
    }

    protected void ddlIDType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this._GetCustomer(this.ddlIDType.SelectedValue == "" ? 0 : Convert.ToInt32(this.ddlIDType.SelectedValue), this.txtIDNumber.Text);
    }

    protected void txtIDNumber_TextChanged(object sender, EventArgs e)
    {
        this._GetCustomer(this.ddlIDType.SelectedValue == "" ? 0 : Convert.ToInt32(this.ddlIDType.SelectedValue), this.txtIDNumber.Text);
    }

    private void _GetCustomer(int idType, string idNo)
    {
        if (!(this.ddlIDType.SelectedValue != "") || !(this.txtIDNumber.Text.Trim() != ""))
            return;
        bl_micro_customer1 customerByIdNumber = da_micro_customer.GetCustomerByIdNumber(idType, idNo);
        if (!string.IsNullOrWhiteSpace(customerByIdNumber.ID_NUMBER))
        {
            Helper.SelectedDropDownListIndex("VALUE", this.ddlIDType, customerByIdNumber.ID_TYPE);
            this.txtIDNumber.Text = customerByIdNumber.ID_NUMBER;
            this.txtSurnameEng.Text = customerByIdNumber.LAST_NAME_IN_ENGLISH;
            this.txtSurnameKh.Text = customerByIdNumber.LAST_NAME_IN_KHMER;
            this.txtFirstNameEng.Text = customerByIdNumber.FIRST_NAME_IN_ENGLISH;
            this.txtFirstNameKh.Text = customerByIdNumber.FIRST_NAME_IN_KHMER;
            Helper.SelectedDropDownListIndex("VALUE", this.ddlGender, customerByIdNumber.GENDER);
            this.txtDateOfBirth.Text = customerByIdNumber.DATE_OF_BIRTH.ToString("dd/MM/yyyy");
            this.txtCustomerAge.Text = string.Concat((object)this.CalculateAge(this.txtDateOfBirth.Text, this.txtApplicationDate.Text));
            Helper.SelectedDropDownListIndex("VALUE", this.ddlMaritalStatus, customerByIdNumber.MARITAL_STATUS);
            Helper.SelectedDropDownListIndex("VALUE", this.ddlOccupation, customerByIdNumber.OCCUPATION);
            this.txtPhoneNumber.Text = customerByIdNumber.PHONE_NUMBER1;
            this.txtEmail.Text = customerByIdNumber.EMAIL1;
            this._SelectProvince(customerByIdNumber.PROVINCE_EN);
            this.BindDistrict();
            this._SelectDistrict(customerByIdNumber.DISTRICT_EN);
            this.BindCommune();
            this._SelectCommune(customerByIdNumber.COMMUNE_EN);
            this.BindVillage();
            this._SelectVillage(customerByIdNumber.VILLAGE_EN);
            this.txtStreetEn.Text = customerByIdNumber.STREET_NO_EN;
            this.txtStreetKh.Text = customerByIdNumber.STREET_NO_KH;
            this.txtHouseNoEn.Text = customerByIdNumber.HOUSE_NO_EN;
            this.txtHouseNoKh.Text = customerByIdNumber.HOUSE_NO_KH;
            Helper.SelectedDropDownListIndex("TEXT", this.ddlNationality, customerByIdNumber.NATIONALITY);
            this._EnableCustomer(false);
            this.MerchAddress();
        }
        else
        {
            this._EnableCustomer();
            this._ClearCustomer();
        }
    }

    private void _SelectProvince(string value)
    {
        for (int index = 0; index < this.ddlProvinceKh.Items.Count; ++index)
        {
            if (this.ddlProvinceKh.Items[index].Value.Trim().Split('/')[0].ToString().ToLower() == value.Trim().ToLower())
            {
                this.ddlProvinceKh.SelectedIndex = index;
                break;
            }
        }
    }

    private void _SelectDistrict(string value)
    {
        for (int index = 0; index < this.ddlDistrictKh.Items.Count; ++index)
        {
            if (this.ddlDistrictKh.Items[index].Value.Trim().Split('/')[0].ToString().ToLower() == value.Trim().ToLower())
            {
                this.ddlDistrictKh.SelectedIndex = index;
                break;
            }
        }
    }

    private void _SelectCommune(string value)
    {
        for (int index = 0; index < this.ddlCommuneKh.Items.Count; ++index)
        {
            if (this.ddlCommuneKh.Items[index].Value.Trim().Split('/')[0].ToString().ToLower() == value.Trim().ToLower())
            {
                this.ddlCommuneKh.SelectedIndex = index;
                break;
            }
        }
    }

    private void _SelectVillage(string value)
    {
        for (int index = 0; index < this.ddlVillageKh.Items.Count; ++index)
        {
            if (this.ddlVillageKh.Items[index].Value.Trim().Split('/')[0].ToString().ToLower() == value.Trim().ToLower())
            {
                this.ddlVillageKh.SelectedIndex = index;
                break;
            }
        }
    }

    private void _EnableCustomer(bool t = true)
    {
        if (!t)
        {
            this.txtSurnameEng.Attributes.Add("disabled", "disabled");
            this.txtSurnameKh.Attributes.Add("disabled", "disabled");
            this.txtFirstNameEng.Attributes.Add("disabled", "disabled");
            this.txtFirstNameKh.Attributes.Add("disabled", "disabled");
            this.ddlGender.Attributes.Add("disabled", "disabled");
            this.txtDateOfBirth.Attributes.Add("disabled", "disabled");
            this.ddlMaritalStatus.Attributes.Add("disabled", "disabled");
            this.ddlOccupation.Attributes.Add("disabled", "disabled");
            this.txtPhoneNumber.Attributes.Add("disabled", "disabled");
            this.txtEmail.Attributes.Add("disabled", "disabled");
            this.ddlProvinceKh.Attributes.Add("disabled", "disabled");
            this.ddlCommuneKh.Attributes.Add("disabled", "disabled");
            this.ddlDistrictKh.Attributes.Add("disabled", "disabled");
            this.ddlVillageKh.Attributes.Add("disabled", "disabled");
            this.txtStreetEn.Attributes.Add("disabled", "disabled");
            this.txtStreetKh.Attributes.Add("disabled", "disabled");
            this.txtHouseNoEn.Attributes.Add("disabled", "disabled");
            this.txtHouseNoKh.Attributes.Add("disabled", "disabled");
            this.ddlNationality.Attributes.Add("disabled", "disabled");
        }
        else
        {
            this.txtSurnameEng.Attributes.Remove("disabled");
            this.txtSurnameKh.Attributes.Remove("disabled");
            this.txtFirstNameEng.Attributes.Remove("disabled");
            this.txtFirstNameKh.Attributes.Remove("disabled");
            this.ddlGender.Attributes.Remove("disabled");
            this.txtDateOfBirth.Attributes.Remove("disabled");
            this.ddlMaritalStatus.Attributes.Remove("disabled");
            this.ddlOccupation.Attributes.Remove("disabled");
            this.txtPhoneNumber.Attributes.Remove("disabled");
            this.txtEmail.Attributes.Remove("disabled");
            this.ddlProvinceKh.Attributes.Remove("disabled");
            this.ddlCommuneKh.Attributes.Remove("disabled");
            this.ddlDistrictKh.Attributes.Remove("disabled");
            this.ddlVillageKh.Attributes.Remove("disabled");
            this.txtStreetEn.Attributes.Remove("disabled");
            this.txtStreetKh.Attributes.Remove("disabled");
            this.txtHouseNoEn.Attributes.Remove("disabled");
            this.txtHouseNoKh.Attributes.Remove("disabled");
            this.ddlNationality.Attributes.Remove("disabled");
        }
    }

    private void _ClearCustomer()
    {
        this.txtSurnameEng.Text = "";
        this.txtSurnameKh.Text = "";
        this.txtFirstNameEng.Text = "";
        this.txtFirstNameKh.Text = "";
        this.ddlGender.Text = "";
        this.txtDateOfBirth.Text = "";
        this.txtCustomerAge.Text = "";
        this.ddlMaritalStatus.SelectedIndex = 0;
        this.ddlOccupation.SelectedIndex = 0;
        this.txtPhoneNumber.Text = "";
        this.txtEmail.Text = "";
        this.ddlProvinceKh.SelectedIndex = 0;
        this.ddlCommuneKh.SelectedIndex = 0;
        this.ddlDistrictKh.SelectedIndex = 0;
        this.ddlVillageKh.SelectedIndex = 0;
        this.ddlNationality.SelectedIndex = 0;
        this.txtStreetEn.Text = "";
        this.txtStreetKh.Text = "";
        this.txtHouseNoEn.Text = "";
        this.txtHouseNoKh.Text = "";
    }

    protected void gvBen_RowEditing(object sender, GridViewEditEventArgs e)
    {
        bl_micro_application_beneficiary applicationBeneficiary = new bl_micro_application_beneficiary();
        this._BenRowIndex = e.NewEditIndex;
        GridViewRow row = this.gvBen.Rows[this._BenRowIndex];
        Label control1 = (Label)row.FindControl("lblFullName");
        Label control2 = (Label)row.FindControl("lblDOB");
        HiddenField control3 = (HiddenField)row.FindControl("hdfGender");
        HiddenField control4 = (HiddenField)row.FindControl("hdfIdType");
        Label control5 = (Label)row.FindControl("lblIdNo");
        Label control6 = (Label)row.FindControl("lblAge");
        Label control7 = (Label)row.FindControl("lblRelation");
        Label control8 = (Label)row.FindControl("lblPercentage");
        Label control9 = (Label)row.FindControl("lblAddress");
        Label control10 = (Label)row.FindControl("lblBenId");
        this.txtFullName.Text = control1.Text;
        this.txtBenDOB.Text = control2.Text;
        Helper.SelectedDropDownListIndex("VALUE", this.ddlBenGender, control3.Value);
        Helper.SelectedDropDownListIndex("VALUE", this.ddlBenIdType, control4.Value);
        this.txtBenIdNo.Text = control5.Text;
        this.txtAge.Text = control6.Text;
        Helper.SelectedDropDownListIndex("VALUE", this.ddlRelation, control7.Text);
        this.txtPercentageOfShare.Text = control8.Text;
        this.txtBenAddress.Text = control9.Text;
        this.hdfBeneficiaryId.Value = control10.Text;
        this._BenPercentage = Convert.ToDouble(control8.Text);
    }

    protected void txtTotalSumAssure_TextChanged(object sender, EventArgs e)
    {
        if (!Helper.IsNumber(this.txtTotalSumAssure.Text.Trim()))
            return;

        double totalSA = Convert.ToDouble(this.txtTotalSumAssure.Text.Trim());
        int countApp = 0;
        if (totalSA > 5000.0)
        {
            countApp = Convert.ToInt32(totalSA / 5000.0);
            double remainSA = totalSA - (double)(countApp * 5000);
            countApp = countApp <= 1 ? 1 : countApp;
            countApp = remainSA > 0.0 ? countApp + 1 : countApp;
        }
        else
            countApp = 1;
        this.txtSumAssure.Text = totalSA >= 5000.0 ? "5000" : string.Concat((object)totalSA);
        if (this._MaxPolicyPerLife == 0)
            Helper.Alert(true, "System is missed MAX_POLICY_PER_LIFE parameter.", this.lblError);
        else if (countApp > this._MaxPolicyPerLife)
        {
            Helper.Alert(true, "Policy count [ " + countApp + " ] is over limit.", this.lblError);
            this.ddlNumberofApplication.SelectedIndex = 0;
        }
        else
        {
            Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofApplication, string.Concat(countApp));
            this.ValidateCalculatePremium();
            if (!(bool)this.ViewState["IS_VALID"])
                return;
            this.CalculatePremium();
        }
    }

    private void _RoleBack(
      List<string> newAppList,
      List<string> existAppList,
      string userName,
      DateTime tranDate,
      out string message)
    {
        string strDelete = "";
        string strRestore = "";
        bool flag = false;
        foreach (string newApp in newAppList)
        {
            flag = da_micro_application.DeleteApplication(newApp);
            flag = da_micro_application_insurance.DeleteApplicationInsurance(newApp);
            flag = da_micro_application_insurance_rider.DeleteApplicationInsuranceRider(newApp);
            flag = da_micro_application_beneficiary.DeleteApplicationBeneficiary(newApp);
            flag = da_micro_application_questionaire.DeleteQuestionaire(newApp);
            strDelete += !da_micro_application_beneficiary.PremaryBeneficiary.Delete(newApp) ? newApp + " fail <br />" : newApp + " success <br />";
        }
        strDelete = strDelete != "" ? "<strong>DELETED APPLICATION:</strong> <br />" + strDelete : "";
        foreach (string existApp in existAppList)
            strRestore += !da_micro_application.RestoreApplication(existApp, userName, tranDate) ? existApp + "fail <br />" : existApp + " success <br />";
        strRestore = strRestore != "" ? "<strong>RESTORED APPLICATION:</strong> <br />" + strRestore : "";
        message = strDelete + strRestore;
    }

    protected void ddlClientType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddlClientType.SelectedValue == Helper.ClientTYpe.BANK_STAFF_FAMILY.ToString())
        {
            this.lblBankStaffName.Text = "Bank Staff Name";
            this.ShowClientTypeRemarks(true);
        }
        else if (this.ddlClientType.SelectedValue == Helper.ClientTYpe.CLIENT_FAMILY.ToString())
        {
            this.lblBankStaffName.Text = "Client Name";
            this.ShowClientTypeRemarks(true);
        }
        else
            this.ShowClientTypeRemarks(false);
    }

    private void ShowClientTypeRemarks(bool t)
    {
        this.lblBankStaffName.Visible = t;
        this.txtBankStaffName.Visible = t;
        this.lblClientTypeRelation.Visible = t;
        this.ddlClientTypeRelation.Visible = t;
        if (!t)
            return;
        this.txtBankStaffName.Text = "";
        this.ddlClientTypeRelation.SelectedIndex = 0;
    }

    private void BindBaicSA(double[] sa)
    {
    }

    private void BindRiderSA(double[] sa)
    {
        this.ddlRiderSumAssure.Items.Clear();
        this.ddlRiderSumAssure.Items.Add(new ListItem("--- Select ---", "0"));
        foreach (double num in sa)
            this.ddlRiderSumAssure.Items.Add(new ListItem(string.Concat((object)num), string.Concat((object)num)));
        if (((IEnumerable<double>)sa).Count<double>() == 1)
        {
            this.ddlRiderSumAssure.SelectedIndex = 1;
            this.ddlRiderSumAssure.Enabled = false;
        }
        else
            this.ddlRiderSumAssure.Enabled = true;
    }

    private void BindPayMode(int[] mode)
    {
        this.ddlPaymentMode.Items.Clear();
        this.ddlPaymentMode.Items.Add(new ListItem("--- Select ---", "-1"));
        bl_payment_mode blPaymentMode = new bl_payment_mode();
        foreach (int pay_mode_ID in mode)
        {
            bl_payment_mode paymentModeByPayModeId = da_payment_mode.GetPaymentModeByPayModeID(pay_mode_ID);
            this.ddlPaymentMode.Items.Add(new ListItem(paymentModeByPayModeId.Mode, string.Concat((object)paymentModeByPayModeId.Pay_Mode_ID)));
        }
        if (((IEnumerable<int>)mode).Count<int>() == 1)
            this.ddlPaymentMode.SelectedIndex = 1;
        else
            this.ddlPaymentMode.Enabled = true;
    }

    protected void ddlBenGender_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ddlRelation.Items.Clear();
        Options.Bind(this.ddlRelation, da_master_list.da_beneficiary_relation.GetBeneficiaryRelationList(Convert.ToInt32(this.ddlBenGender.SelectedValue)), "RelationKh", "RelationKh", -1);
    }

    protected void gvBen_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        this._BenRowIndex = e.RowIndex;
        this.BeneficiaryList.RemoveAt(this._BenRowIndex);
        this.gvBen.DataSource = (object)this.BeneficiaryList;
        this.gvBen.DataBind();
    }

    protected void txtIssueDate_TextChanged(object sender, EventArgs e)
    {
        int days = DateTime.Now.Subtract(Helper.FormatDateTime(this.txtIssueDate.Text.Trim())).Days;
        if (days > 3)
        {
            Helper.Alert(false, "Back date is allow only 3 days.", this.lblError);
            this.txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        else
        {
            if (days >= 0)
                return;
            Helper.Alert(false, "Issue Date is greater than system date, is not allow.", this.lblError);
            this.txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
    }

    protected void txtEffectiveDate_TextChanged(object sender, EventArgs e)
    {
        int days = DateTime.Now.Subtract(Helper.FormatDateTime(this.txtEffectiveDate.Text.Trim())).Days;
        if (days > 3)
        {
            Helper.Alert(false, "Back date is allow only 3 days.", this.lblError);
            this.txtEffectiveDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        else
        {
            if (days >= 0)
                return;
            Helper.Alert(false, "Issue Date is greater than system date, is not allow.", this.lblError);
            this.txtEffectiveDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
    }

    protected void txtPaydate_TextChanged(object sender, EventArgs e)
    {
        int days = DateTime.Now.Subtract(Helper.FormatDateTime(this.txtPaydate.Text.Trim())).Days;
        if (days > 3)
        {
            Helper.Alert(false, "Back date is allow only 3 days.", this.lblError);
            this.txtPaydate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        else
        {
            if (days >= 0)
                return;
            Helper.Alert(false, "Issue Date is greater than system date, is not allow.", this.lblError);
            this.txtPaydate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
    }

    protected void ddlCoverType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddlCoverType.SelectedValue.ToUpper() == "Y")
        {
            this.ddlTermOfCover.SelectedValue = "1";
            this.ddlTermOfCover.Attributes.Add("disabled", "disabled");
            this.ddlNumberofYear.Attributes.Remove("disabled");
        }
        else
        {
            this.ddlTermOfCover.Attributes.Remove("disabled");
            Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofYear, "1");
            this.ddlNumberofYear.Attributes.Add("disabled", "disabled");
            this.CalculatePremium();
        }
    }

    protected void txtLoanNumber_TextChanged(object sender, EventArgs e)
    {
        if (this.ProductConfig == null)
            return;
        if (this.ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
            this.txtPrimaryBenLoan.Text = this.txtLoanNumber.Text.Trim();
        else
            this.txtPrimaryBenLoan.Text = "";
    }

    protected void ddlTermOfCover_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(this.ddlTermOfCover.SelectedValue) >= 12)
        {
            Helper.SelectedDropDownListIndex("VALUE", this.ddlCoverType, "Y");
            Helper.SelectedDropDownListIndex("VALUE", this.ddlTermOfCover, "1");
            this.ddlTermOfCover.Attributes.Add("disabled", "disabled");
            this.ddlNumberofYear.Attributes.Remove("disabled");
        }
        else
        {
            this.ddlTermOfCover.Attributes.Remove("disabled");
            this.ddlNumberofYear.Attributes.Add("disabled", "disabled");
            Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofYear, "1");
        }
        this.CalculatePremium();
    }

    protected void txtRiderDiscount_TextChanged(object sender, EventArgs e)
    {
        if (Helper.IsAmount(this.txtRiderDiscount.Text.Trim()))
        {
            double num1 = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
            double num2 = Convert.ToDouble(this.txtRiderPremium.Text.Trim() == "" ? "0" : this.txtRiderPremium.Text.Trim());
            if (num1 > num2)
            {
                Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
                this.txtRiderDiscount.Text = "0";
                num1 = 0.0;
            }
            this.txtRiderAfterDiscount.Text = string.Concat((object)(num2 - num1));
            this.ComputeTotalDiscount();
        }
        else
            Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
    }

    protected void txtBasicDiscount_TextChanged(object sender, EventArgs e)
    {
        if (Helper.IsAmount(this.txtBasicDiscount.Text.Trim()))
        {
            double num1 = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
            double num2 = Convert.ToDouble(this.txtPremium.Text.Trim() == "" ? "0" : this.txtPremium.Text.Trim());
            if (num1 > num2)
            {
                Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
                this.txtBasicDiscount.Text = "0";
                num1 = 0.0;
            }
            this.txtBasicAfterDiscount.Text = string.Concat((object)(num2 - num1));
            this.ComputeTotalDiscount();
        }
        else
            Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
    }

    private void ComputeTotalDiscount()
    {
        double num1 = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
        double num2 = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
        double num3 = Convert.ToDouble(this.txtBasicAfterDiscount.Text.Trim() == "" ? "0" : this.txtBasicAfterDiscount.Text.Trim());
        double num4 = Convert.ToDouble(this.txtRiderAfterDiscount.Text.Trim() == "" ? "0" : this.txtRiderAfterDiscount.Text.Trim());
        this.txtTotalDiscountAmount.Text = string.Concat((object)(num1 + num2));
        this.txtTotalPremiumAfterDiscount.Text = string.Concat((object)(num3 + num4));
    }

    public class ResponeApplicationForm
    {
        public int StatusCode { get; set; }

        public string Status { get; set; }

        public byte[] ApplicationForm { get; set; }

        public string Message { get; set; }
    }

    [Serializable]
    public class SumAssurePremiums
    {
        public double SumAssure { get; set; }

        public double Premium { get; set; }

        public double AnnualPremium { get; set; }
    }

    [WebMethod(EnableSession = true)]
    public static string ConfirmAction(string policyId, string attachedPolicyInsurance)
    {

        var username = HttpContext.Current.User.Identity.Name;

        var pol = da_micro_policy.GetPolicyByID(policyId, username);
        var pro = da_micro_product_config.ProductConfig.GetProductMicroProduct(pol.PRODUCT_ID);

        da_sys_activity_log.Save(new bl_sys_activity_log(username, "", bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, DateTime.Now, "User views certificate. [Pol No:" + pol.POLICY_NUMBER + "]", Membership.ApplicationName));
        if (pro.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || pro.CreatedOn.Year >= 2025)
        {
            HttpContext.Current.Session["POL_ID_PRINT"] = new List<string>() 
      {
        pol.POLICY_ID
      };

            return "load_new_certificate.aspx?policyType=IND&printPolInsurance=" + (attachedPolicyInsurance);
        }
        else
        {

            return string.Format("banca_micro_cert.aspx?P_ID={0}&P_TYPE=IND", pol.POLICY_ID);
        }

    }
}