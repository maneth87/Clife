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
public partial class Pages_Business_micro_application_form_edit : System.Web.UI.Page
{
    string userName = "";
    string userId = "";

    private bl_micro_product_config _ProductConfig
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

    private bl_micro_product_rider _ProductRider
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

    private bool _IsMainPolicy
    {
        get { return (bool)this.ViewState["V_MAIN_POLICY"]; }
        set
        {
            this.ViewState["V_MAIN_POLICY"] =
                value;
        }
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

            this.Initialize();
            this.BeneficiaryList = new List<bl_micro_application_beneficiary>();
            this._BenPercentage = 0.0;
            this._MaxPolicyPerLife = 0;
            this._IsMainPolicy = true;
            if (this.Request.QueryString.Count == 0)
            {
                Helper.Alert(true, "Invalid URL", this.lblError);
            }
            else
            {
                this.hdfApplicationID.Value = this.Request.QueryString["APP_ID"].ToString();
                this.BindExisting();
            }
        }
        else
            this.Response.Redirect("../../unauthorize.aspx");
    }

    private void BindProvince()
    {
        List<bl_address.province> province1 = da_address.province.GetProvince();
        this.ddlProvinceKh.Items.Clear();
        this.ddlProvinceKh.Items.Add(new ListItem("--ខេត្ត/ក្រុង--", ""));
        if (province1.Count <= 0)
            return;
        foreach (bl_address.province province2 in province1)
            this.ddlProvinceKh.Items.Add(new ListItem(province2.Khmer, province2.Code + "/" + province2.English));
    }

    private void BindDistrict()
    {
        if (!(this.ddlProvinceKh.SelectedValue != ""))
            return;
        List<bl_address.district> districtList = da_address.district.GetDistrict(this.GetProvinceCode());
        this.ddlDistrictKh.Items.Clear();
        this.ddlDistrictKh.Items.Add(new ListItem("--ស្រុក/ខណ្ឌ--", ""));
        if (districtList.Count <= 0)
            return;
        foreach (bl_address.district district in districtList)
            this.ddlDistrictKh.Items.Add(new ListItem(district.Khmer, district.Code + "/" + district.English));
    }

    private void BindCommune()
    {
        if (!(this.ddlDistrictKh.SelectedValue != ""))
            return;
        List<bl_address.commune> communeList = da_address.commune.GetCommune(this.GetDisctrictCode());
        this.ddlCommuneKh.Items.Clear();
        this.ddlCommuneKh.Items.Add(new ListItem("--ឃុំ/សង្កាត់--", ""));
        if (communeList.Count <= 0)
            return;
        foreach (bl_address.commune commune in communeList)
            this.ddlCommuneKh.Items.Add(new ListItem(commune.Khmer, commune.Code + "/" + commune.English));
    }

    private void BindVillage()
    {
        if (!(this.ddlCommuneKh.SelectedValue != ""))
            return;
        List<bl_address.village> villageList = da_address.village.GetVillage(this.GetCommuneCode());
        this.ddlVillageKh.Items.Clear();
        this.ddlVillageKh.Items.Add(new ListItem("--ភូមិ--", ""));
        if (villageList.Count <= 0)
            return;
        foreach (bl_address.village village in villageList)
            this.ddlVillageKh.Items.Add(new ListItem(village.Khmer, village.Code + "/" + village.English));
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

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
                this.txtProductName.Text = this._ProductConfig.En_Title;
                this.txtSumAssure.Text = this._ProductConfig.BasicSumAssured ?? "";
                this.txtTotalSumAssure.Text = this._ProductConfig.BasicSumAssured ?? "";
                this.ddlCoverType.Items.Clear();
                this.ddlTermOfCover.Items.Clear();
                foreach (string coverTypes in this._ProductConfig.CoverPeriodType)
                {
                    string[] arrType = coverTypes.Split(':');
                    string[] arrCoverType = arrType[0].Split(',');
                    string[] arrCoverPeriod = arrType[1].Split(',');
                    foreach (string val in arrCoverType)
                        this.ddlCoverType.Items.Add(new ListItem(val, val));
                    foreach (string val in arrCoverPeriod)
                        this.ddlTermOfCover.Items.Add(new ListItem(val, val));
                }
                this.txtTotalSumAssure_TextChanged(null, null);/*call event total sum assure changed*/
                this.BindPaymode();
                if (!string.IsNullOrWhiteSpace(this._ProductConfig.RiderProductID))
                {
                    this.BindProductRider();
                    this.BindDHCSA();
                    this.dvRider.Attributes.CssStyle.Add("display", "block");
                }
                else
                    this.dvRider.Attributes.CssStyle.Add("display", "none");

                if (this._ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
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
        catch (Exception ex)
        {
            var str = string.Format("{0}{2}", "");
            Log.AddExceptionToLog(string.Format("Error function [ddlProduct_SelectedIndexChanged] in class [micro_application_form_edit.aspx.cs], Error Lin:{0} datail:{1}==>{2}", Log.GetLineNumber(ex), ex.Message, ex.StackTrace));
        }
    }

    private bool GetProductInfo(string productID)
    {
        try
        {
            List<bl_micro_product_config> proList = (List<bl_micro_product_config>)this.Session["PRODUCT_LIST"];
            if (proList.Count > 0)
            {
                foreach (var config in proList.Where(_ => _.Product_ID == productID))
                    this._ProductConfig = config;

                try
                {
                    Channel_Item_Config channelItemConfig = new Channel_Item_Config().GetChannelItemConfig(this.ddlCompany.SelectedValue, this._ProductConfig.Product_ID);
                    for (int index = 1; index <= channelItemConfig.MaxPolicyPerLife; ++index) /*add number application in dropdown list*/
                        this.ddlNumberofApplication.Items.Add(new ListItem(index + "", index + ""));
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
        List<bl_micro_product_rider> proRiderList = da_micro_product_config.ProductConfig.GetProductMicroProductRider(this.ddlProduct.SelectedValue);
        this.ddlRiderProduct.Items.Clear();
        this.ddlRiderProduct.Items.Add(new ListItem("---Select---", ""));
        foreach (bl_micro_product_rider rider in proRiderList)
            this.ddlRiderProduct.Items.Add(new ListItem(rider.PRODUCT_ID + "/" + rider.REMARKS, rider.PRODUCT_ID));
    }

    private void BindDHCSA()
    {
        this.ddlRiderSumAssure.Items.Clear();
        this.ddlRiderSumAssure.Items.Add(new ListItem("---Select---", ""));
        if (_ProductConfig.Product_ID == null)
            return;
        foreach (double sa in _ProductConfig.RiderSumAssuredRange)
            this.ddlRiderSumAssure.Items.Add(new ListItem(sa + "", sa + ""));
    }

    protected void ibtnPrintApplication_Click(object sender, ImageClickEventArgs e)
    {
        this.SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, "User views application form. [App No:" + this.hdfApplicationNumber.Value + "].");
        if (this._ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || this._ProductConfig.CreatedOn.Year >= 2025)
        {
            this.Session["APP_ID_PRINT"] = new List<string>() /*session for printing application from*/
      {
        this.hdfApplicationID.Value
      };
            string url = "load_new_application_form.aspx";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
        }
        else
        {
            string url = "banca_micro_application_print.aspx?APP_ID=" + this.hdfApplicationID.Value + "&A_TYPE=IND";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", string.Format("<script>window.open('{0}');</script>", url), false);
        }
    }

    protected void ibtnPrintCertificate_Click(object sender, ImageClickEventArgs e)
    {
        this.SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, "User views certificate. [Pol No:" + this.hdfPolicyNumber.Value + "].");
        if (this._ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || this._ProductConfig.CreatedOn.Year >= 2025)
        {
            this.Session["POL_ID_PRINT"] = new List<string>() /*session for print certificate*/
      {
        this.hdfPolicyID.Value
      };
            string url = "load_new_certificate.aspx?policyType=IND&printPolInsurance=Y";
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", string.Format("<script>window.open('{0}');</script>", url), false);
        }
        else
        {
            string url = string.Format("banca_micro_cert.aspx?P_ID={0}&P_TYPE=IND", this.hdfPolicyID.Value);
            System.Web.UI.ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "none", string.Format("<script>window.open('{0}');</script>", url), false);
        }
    }

    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.ValidateForm();
            string message = string.Concat(this.ViewState["MESSAGE"]);
            if ((bool)this.ViewState["IS_VALID"])
            {
                string channelItemId = this.ddlCompany.SelectedValue;
                string channelId = this.ddlChannel.SelectedValue;
                string channelLocationId = this.hdfChannelLocationID.Value;
                string provinceCode = this.GetProvinceCode();
                string disctrictCode = this.GetDisctrictCode();
                string communeCode = this.GetCommuneCode();
                string villageCode = this.GetVillageCode();
                string applicationID = this.hdfApplicationID.Value;
                string applicationNumber = this.hdfApplicationNumber.Value;
                DateTime tranDate = DateTime.Now;
                List<string> stringList = new List<string>();
                List<string> existAppList = new List<string>();
                bool flag = false;


                string applicationCustomerId = this.hdfApplicationCustomerID.Value;
                bl_micro_application app = new bl_micro_application();
                bl_micro_application_customer appCust = new bl_micro_application_customer();
                appCust.ID_TYPE = this.ddlIDType.SelectedValue;
                appCust.ID_NUMBER = this.txtIDNumber.Text.Trim();
                appCust.FIRST_NAME_IN_ENGLISH = this.txtFirstNameEng.Text.Trim();
                appCust.LAST_NAME_IN_ENGLISH = this.txtSurnameEng.Text.Trim();
                appCust.FIRST_NAME_IN_KHMER = this.txtFirstNameKh.Text.Trim();
                appCust.LAST_NAME_IN_KHMER = this.txtSurnameKh.Text.Trim();
                appCust.GENDER = this.ddlGender.SelectedValue;
                appCust.DATE_OF_BIRTH = Helper.FormatDateTime(this.txtDateOfBirth.Text.Trim());
                appCust.NATIONALITY = this.ddlNationality.SelectedItem.Text;
                appCust.MARITAL_STATUS = this.ddlMaritalStatus.SelectedValue;
                appCust.OCCUPATION = this.ddlOccupation.SelectedValue;
                appCust.HOUSE_NO_KH = this.txtHouseNoKh.Text.Trim();
                appCust.STREET_NO_KH = this.txtStreetKh.Text.Trim();
                appCust.VILLAGE_KH = villageCode;
                appCust.COMMUNE_KH = communeCode;
                appCust.DISTRICT_KH = disctrictCode;
                appCust.PROVINCE_KH = provinceCode;
                appCust.HOUSE_NO_EN = this.txtHouseNoEn.Text.Trim();
                appCust.STREET_NO_EN = this.txtStreetEn.Text.Trim();
                appCust.VILLAGE_EN = villageCode;
                appCust.COMMUNE_EN = communeCode;
                appCust.DISTRICT_EN = disctrictCode;
                appCust.PROVINCE_EN = provinceCode;
                appCust.PHONE_NUMBER1 = this.txtPhoneNumber.Text.Trim();
                appCust.EMAIL1 = this.txtEmail.Text.Trim();
                appCust.CREATED_BY = this.userName;
                appCust.CREATED_ON = tranDate;
                appCust.STATUS = 1;
                app.APPLICATION_DATE = Helper.FormatDateTime(this.txtApplicationDate.Text);
                app.CHANNEL_ITEM_ID = channelItemId;
                app.CHANNEL_ID = channelId;
                app.CHANNEL_LOCATION_ID = channelLocationId;
                app.SALE_AGENT_ID = this.txtSaleAgentID.Text;
                app.CREATED_BY = this.userName;
                app.CREATED_ON = tranDate;
                app.REMARKS = "";
                app.CLIENT_TYPE = this.ddlClientType.SelectedValue;
                app.CLIENT_TYPE_RELATION = "";
                app.CLIENT_TYPE_REMARKS = "";
                if (_ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
                {
                    app.LoanNumber = this.txtLoanNumber.Text.Trim();
                    app.PolicyholderName = this.txtHolderName.Text.Trim();
                    app.PolicyholderDOB = this.txtHolderDOB.Text.Trim() == "" ? new DateTime(1900, 1, 10) : Helper.FormatDateTime(this.txtHolderDOB.Text.Trim());
                    app.PolicyholderGender = Convert.ToInt32(this.ddlHolderGender.SelectedValue);
                    app.PolicyholderIDType = Convert.ToInt32(this.ddlHolderIdType.SelectedValue);
                    app.PolicyholderIDNo = this.txtHolderIdNo.Text.Trim();
                    app.PolicyholderAddress = this.txtHolderAddress.Text.Trim();
                }
                else
                {
                    app.LoanNumber = "";
                    app.PolicyholderName = "";
                    app.PolicyholderDOB = new DateTime(1900, 1, 10);
                    app.PolicyholderGender = -1;
                    app.PolicyholderIDType = -1;
                    app.PolicyholderIDNo = "";
                    app.PolicyholderAddress = "";
                }
                bl_micro_application_insurance appInsurance = new bl_micro_application_insurance();
                appInsurance.PRODUCT_ID = this.hdfProductID.Value;
                appInsurance.TERME_OF_COVER = Convert.ToInt32(this.ddlTermOfCover.SelectedValue);
                appInsurance.PAYMENT_PERIOD = Convert.ToInt32(this.txtPremiumPaymentPeriod.Text.Trim());
                appInsurance.SUM_ASSURE = Convert.ToDouble(this.txtSumAssure.Text.Trim());
                appInsurance.PAY_MODE = Convert.ToInt32(this.ddlPaymentMode.SelectedValue);
                appInsurance.PREMIUM = Convert.ToDouble(this.txtPremium.Text.Trim());
                appInsurance.ANNUAL_PREMIUM = Convert.ToDouble(this.txtAnnualPremium.Text.Trim());
                appInsurance.USER_PREMIUM = 0.0;
                appInsurance.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
                appInsurance.PACKAGE = "";
                appInsurance.TOTAL_AMOUNT = Convert.ToDouble(this.txtBasicAfterDiscount.Text.Trim());
                appInsurance.CREATED_BY = this.userName;
                appInsurance.CREATED_ON = tranDate;
                appInsurance.PAYMENT_CODE = this.txtPaymentCode.Text;
                bl_micro_application_insurance_rider appRider = new bl_micro_application_insurance_rider();
                if (this.hdfRiderProductID.Value != "")
                {
                    appRider.PRODUCT_ID = this.hdfRiderProductID.Value;
                    appRider.SUM_ASSURE = Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue);
                    appRider.PREMIUM = Convert.ToDouble(this.txtRiderPremium.Text.Trim());
                    appRider.ANNUAL_PREMIUM = Convert.ToDouble(this.txtRiderAnnualPremium.Text.Trim());
                    appRider.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
                    appRider.TOTAL_AMOUNT = Convert.ToDouble(this.txtRiderAfterDiscount.Text.Trim());
                    appRider.CREATED_BY = this.userName;
                    appRider.CREATED_ON = tranDate;
                }
                bl_micro_application_beneficiary.PrimaryBeneciary primaryBen;
                if (_ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
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

                if (da_micro_application.BackupApplication(applicationID, "UPDATE", this.userName, tranDate))
                {
                    DateTime now3 = DateTime.Now;
                    appCust.CUSTOMER_ID = applicationCustomerId;
                    appCust.UPDATED_BY = this.userName;
                    appCust.UPDATED_ON = tranDate;
                    if (da_micro_application_customer.UpdateApplicationCustomer(appCust))
                    {
                        existAppList.Add(applicationNumber);
                        app.UPDATED_BY = this.userName;
                        app.UPDATED_ON = tranDate;
                        app.APPLICATION_NUMBER = applicationNumber;
                        app.APPLICATION_CUSTOMER_ID = applicationCustomerId;
                        flag = da_micro_application.UpdateApplication(app);
                        if (flag)
                        {
                            appInsurance.UPDATED_BY = this.userName;
                            appInsurance.UPDATED_ON = tranDate;
                            appInsurance.APPLICATION_NUMBER = applicationNumber;
                            appInsurance.PAYMENT_CODE = this.txtPaymentCode.Text;
                            appInsurance.COVER_TYPE = (bl_micro_product_config.PERIOD_TYPE)Enum.Parse(typeof(bl_micro_product_config.PERIOD_TYPE), this.ddlCoverType.SelectedValue);
                            flag = da_micro_application_insurance.UpdateApplicationInsurance(appInsurance);
                            if (flag)
                            {
                                bl_micro_application_insurance_rider existingRider = da_micro_application_insurance_rider.GetApplicationRider(applicationNumber);
                                if (!string.IsNullOrWhiteSpace(appRider.PRODUCT_ID)) /*rider is attached*/
                                {
                                    appRider.APPLICATION_NUMBER = applicationNumber;
                                    if (!string.IsNullOrWhiteSpace(existingRider.PRODUCT_ID))/*existing rider found, do update*/
                                    {
                                        appRider.UPDATED_BY = this.userName;
                                        appRider.UPDATED_ON = tranDate;
                                        flag = da_micro_application_insurance_rider.UpdateApplicationInsuranceRider(appRider);
                                    }
                                    else/*existing rider not found, do save new*/
                                    {
                                        appRider.CREATED_BY = this.userName;
                                        appRider.CREATED_ON = tranDate;
                                        flag = da_micro_application_insurance_rider.SaveApplicationInsuranceRider(appRider);
                                    }
                                }
                                else if (string.IsNullOrWhiteSpace(appRider.PRODUCT_ID))/*rider is not attached*/
                                {
                                    if (!string.IsNullOrWhiteSpace(existingRider.PRODUCT_ID))
                                    {
                                        da_micro_application_insurance_rider.DeleteApplicationInsuranceRider(applicationNumber);/*delete existing rider*/
                                    }

                                }
                                if (flag)
                                {
                                    if (this.BeneficiaryList != null)
                                    {
                                        int benLoopStep = 1;
                                        foreach (bl_micro_application_beneficiary beneficiary in this.BeneficiaryList)
                                        {
                                            beneficiary.APPLICATION_NUMBER = applicationNumber;
                                            if (beneficiary.ID.Length <= 2)/*new beneficairy*/
                                            {
                                                beneficiary.ID = Helper.GetNewGuid(new string[2, 2] {
                          { "TABEL","CT_MICRO_APPLICATION_BENEFICIARY"}, {"FIELD","ID"}
                        });

                                                beneficiary.CREATED_BY = this.userName;
                                                beneficiary.CREATED_ON = benLoopStep == 1 ? tranDate : tranDate.AddSeconds(1.0);
                                                flag = da_micro_application_beneficiary.SaveApplicationBeneficiary(beneficiary);
                                            }/*existing bendeficary*/
                                            else
                                            {
                                                beneficiary.UPDATED_BY = this.userName;
                                                beneficiary.UPDATED_ON = benLoopStep == 1 ? tranDate : tranDate.AddSeconds(1.0);
                                                flag = da_micro_application_beneficiary.UpdateApplicationBeneficiary(beneficiary, da_micro_application_beneficiary.DeleteBeneficiaryOption.ID);
                                            }
                                            if (!flag)
                                                break;
                                            benLoopStep++;
                                        }
                                    }/*beneficiary is empty*/
                                    else
                                        flag = false;

                                    if (flag)
                                    {
                                        bl_micro_application_beneficiary.PrimaryBeneciary existingprimaryBeneciary = da_micro_application_beneficiary.PremaryBeneficiary.Get(applicationNumber);
                                        if (primaryBen != null)/*attached primary beneficiary*/
                                        {
                                            primaryBen.ApplicationNumber = applicationNumber;
                                            if (existingprimaryBeneciary == null)
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
                                        else if (existingprimaryBeneciary != null)/*attached primary beneficiary, then remove existing primary beneficiary*/
                                            da_micro_application_beneficiary.PremaryBeneficiary.Delete(existingprimaryBeneciary.ApplicationNumber);

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
                                                message = "Health Question is updated fial.";
                                        }
                                        else
                                            message = "Primary Beneficiary is updated fial.";

                                    }/*update beneficiary fail      */
                                    else
                                        message = "Beneficiary is updated fial.";
                                }/*update rider fail*/
                                else
                                    message = "Application Rider is updated fial.";
                            }/*update application insurance fail*/
                            else
                                message = "Application Insurance is updated fial.";
                        } /*update application*/
                        else
                            message = "Application is updated fial.";

                        /*Start issue policy*/
                        if (flag)
                        {
                            if (this.hdfPolicyID.Value != "")
                            {
                                if (da_micro_policy.BackupPolicy(this.hdfPolicyNumber.Value, "UPDATED", this.userName, tranDate))
                                {
                                    bl_micro_customer1 CUSTOMER = new bl_micro_customer1();
                                    CUSTOMER.CUSTOMER_TYPE = "INDIVIDUAL";
                                    CUSTOMER.ID_TYPE = this.ddlIDType.SelectedValue;
                                    CUSTOMER.ID_NUMBER = this.txtIDNumber.Text.Trim();
                                    CUSTOMER.FIRST_NAME_IN_ENGLISH = this.txtFirstNameEng.Text.Trim();
                                    CUSTOMER.LAST_NAME_IN_ENGLISH = this.txtSurnameEng.Text.Trim();
                                    CUSTOMER.FIRST_NAME_IN_KHMER = this.txtFirstNameKh.Text.Trim();
                                    CUSTOMER.LAST_NAME_IN_KHMER = this.txtSurnameKh.Text.Trim();
                                    CUSTOMER.GENDER = this.ddlGender.SelectedValue;
                                    CUSTOMER.DATE_OF_BIRTH = Helper.FormatDateTime(this.txtDateOfBirth.Text.Trim());
                                    CUSTOMER.NATIONALITY = this.ddlNationality.SelectedItem.Text;
                                    CUSTOMER.MARITAL_STATUS = this.ddlMaritalStatus.SelectedValue;
                                    CUSTOMER.OCCUPATION = this.ddlOccupation.SelectedValue;
                                    CUSTOMER.HOUSE_NO_KH = this.txtHouseNoKh.Text.Trim();
                                    CUSTOMER.STREET_NO_KH = this.txtStreetKh.Text.Trim();
                                    CUSTOMER.VILLAGE_KH = villageCode;
                                    CUSTOMER.COMMUNE_KH = communeCode;
                                    CUSTOMER.DISTRICT_KH = disctrictCode;
                                    CUSTOMER.PROVINCE_KH = provinceCode;
                                    CUSTOMER.HOUSE_NO_EN = this.txtHouseNoEn.Text.Trim();
                                    CUSTOMER.STREET_NO_EN = this.txtStreetEn.Text.Trim();
                                    CUSTOMER.VILLAGE_EN = villageCode;
                                    CUSTOMER.COMMUNE_EN = communeCode;
                                    CUSTOMER.DISTRICT_EN = disctrictCode;
                                    CUSTOMER.PROVINCE_EN = provinceCode;
                                    CUSTOMER.PHONE_NUMBER1 = this.txtPhoneNumber.Text.Trim();
                                    CUSTOMER.EMAIL1 = this.txtEmail.Text.Trim();
                                    CUSTOMER.UPDATED_BY = this.userName;
                                    CUSTOMER.UPDATED_ON = tranDate;
                                    CUSTOMER.STATUS = 1;
                                    CUSTOMER.ID = this.hdfCustomerID.Value;
                                    flag = da_micro_customer.UpdateCustomer(CUSTOMER);
                                    if (flag)
                                    {
                                        bl_micro_policy POLICY = new bl_micro_policy();
                                        POLICY.APPLICATION_ID = this.hdfApplicationID.Value;
                                        POLICY.CUSTOMER_ID = CUSTOMER.ID;
                                        POLICY.PRODUCT_ID = this.hdfProductID.Value;
                                        POLICY.CHANNEL_ID = this.hdfChannelID.Value;
                                        POLICY.CHANNEL_ITEM_ID = this.hdfChannelItemID.Value;
                                        POLICY.CHANNEL_LOCATION_ID = this.hdfChannelLocationID.Value;
                                        POLICY.AGENT_CODE = this.hdfSaleAgentID.Value;
                                        POLICY.UPDATED_ON = tranDate;
                                        POLICY.UPDATED_BY = this.userName;
                                        POLICY.POLICY_STATUS = "IF";
                                        POLICY.POLICY_ID = this.hdfPolicyID.Value;
                                        flag = da_micro_policy.UpdatePolicy(POLICY);
                                        if (flag)
                                        {
                                            bl_micro_policy_detail POLICY_DETAIL = new bl_micro_policy_detail();
                                            POLICY_DETAIL.POLICY_ID = this.hdfPolicyID.Value;
                                            POLICY_DETAIL.EFFECTIVE_DATE = Helper.FormatDateTime(this.txtEffectiveDate.Text.Trim());
                                            POLICY_DETAIL.ISSUED_DATE = Helper.FormatDateTime(this.txtIssueDate.Text.Trim());
                                            POLICY_DETAIL.COVER_TYPE = (bl_micro_product_config.PERIOD_TYPE)Enum.Parse(typeof(bl_micro_product_config.PERIOD_TYPE), this.ddlCoverType.SelectedValue);
                                            POLICY_DETAIL.COVER_YEAR = Convert.ToInt32(this.ddlTermOfCover.SelectedValue);
                                            if (POLICY_DETAIL.COVER_TYPE == bl_micro_product_config.PERIOD_TYPE.Y)
                                                POLICY_DETAIL.MATURITY_DATE = POLICY_DETAIL.EFFECTIVE_DATE.AddYears(1);
                                            else if (POLICY_DETAIL.COVER_TYPE == bl_micro_product_config.PERIOD_TYPE.M)
                                                POLICY_DETAIL.MATURITY_DATE = POLICY_DETAIL.EFFECTIVE_DATE.AddMonths(POLICY_DETAIL.COVER_YEAR);
                                            POLICY_DETAIL.EXPIRY_DATE = POLICY_DETAIL.MATURITY_DATE.AddDays(-1.0);
                                            POLICY_DETAIL.AGE = Convert.ToInt32(this.txtCustomerAge.Text.Trim());
                                            POLICY_DETAIL.CURRANCY = "USD";
                                            POLICY_DETAIL.SUM_ASSURE = Convert.ToDouble(this.txtSumAssure.Text.Trim());
                                            POLICY_DETAIL.PAY_MODE = Convert.ToInt32(this.ddlPaymentMode.SelectedValue);
                                            POLICY_DETAIL.PAYMENT_CODE = this.hdfApplicationNumber.Value;
                                            POLICY_DETAIL.PREMIUM = Convert.ToDouble(this.txtPremium.Text.Trim());
                                            POLICY_DETAIL.ANNUAL_PREMIUM = Convert.ToDouble(this.txtAnnualPremium.Text.Trim());
                                            POLICY_DETAIL.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
                                            POLICY_DETAIL.TOTAL_AMOUNT = Convert.ToDouble(this.txtBasicAfterDiscount.Text.Trim());
                                            POLICY_DETAIL.REFERRAL_FEE = 0.0;
                                            POLICY_DETAIL.REFERRAL_INCENTIVE = 0.0;
                                            POLICY_DETAIL.PAY_YEAR = Convert.ToInt32(this.txtPremiumPaymentPeriod.Text.Trim());
                                            POLICY_DETAIL.COVER_UP_TO_AGE = POLICY_DETAIL.AGE + POLICY_DETAIL.COVER_YEAR;
                                            POLICY_DETAIL.PAY_UP_TO_AGE = POLICY_DETAIL.AGE + POLICY_DETAIL.PAY_YEAR;
                                            POLICY_DETAIL.POLICY_STATUS_REMARKS = this.ddlClientType.SelectedValue == Helper.ClientTYpe.REPAYMENT.ToString() ? "REPAYMENT" : "NEW";
                                            POLICY_DETAIL.UPDATED_BY = this.userName;
                                            POLICY_DETAIL.UPDATED_ON = tranDate;
                                            POLICY_DETAIL.POLICY_DETAIL_ID = this.hdfPolicyDetailID.Value;
                                            flag = da_micro_policy_detail.UpdatePolicyDetail(POLICY_DETAIL);
                                            if (flag)
                                            {
                                                bl_micro_policy_rider RIDER = new bl_micro_policy_rider();
                                                if (this.hdfRiderProductIDOld.Value == "")
                                                {
                                                    if (this.hdfRiderProductID.Value != "")/*save new rider*/
                                                    {
                                                        RIDER.POLICY_ID = POLICY_DETAIL.POLICY_ID;
                                                        RIDER.PRODUCT_ID = this.hdfRiderProductID.Value;
                                                        RIDER.SUM_ASSURE = Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue);
                                                        RIDER.PREMIUM = Convert.ToDouble(this.txtRiderPremium.Text.Trim());
                                                        RIDER.ANNUAL_PREMIUM = Convert.ToDouble(this.txtRiderAnnualPremium.Text.Trim());
                                                        RIDER.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
                                                        RIDER.TOTAL_AMOUNT = Convert.ToDouble(this.txtRiderAfterDiscount.Text.Trim());
                                                        RIDER.CREATED_BY = this.userName;
                                                        RIDER.CREATED_ON = tranDate;
                                                        flag = da_micro_policy_rider.SaveRider(RIDER);
                                                    }
                                                }
                                                else if (this.hdfRiderProductID.Value == "")/*delete existing rider*/
                                                {
                                                    flag = da_micro_policy_rider.DeleteRiderByPolicyId(POLICY.POLICY_ID);
                                                }
                                                else/*update rider*/
                                                {
                                                    RIDER.POLICY_ID = POLICY_DETAIL.POLICY_ID;
                                                    RIDER.PRODUCT_ID = this.hdfRiderProductID.Value;
                                                    RIDER.SUM_ASSURE = Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue);
                                                    RIDER.PREMIUM = Convert.ToDouble(this.txtRiderPremium.Text.Trim());
                                                    RIDER.ANNUAL_PREMIUM = Convert.ToDouble(this.txtRiderAnnualPremium.Text.Trim());
                                                    RIDER.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
                                                    RIDER.TOTAL_AMOUNT = Convert.ToDouble(this.txtRiderAfterDiscount.Text.Trim());
                                                    RIDER.UPDATED_BY = this.userName;
                                                    RIDER.UPDATED_ON = tranDate;
                                                    flag = da_micro_policy_rider.UpdateRider(RIDER);
                                                }
                                                if (flag)
                                                {
                                                    bl_micro_policy_payment PAYMENT = new bl_micro_policy_payment()
                                                    {
                                                        POLICY_DETAIL_ID = POLICY_DETAIL.POLICY_DETAIL_ID,
                                                        DUE_DATE = POLICY_DETAIL.EFFECTIVE_DATE,
                                                        PAY_DATE = Helper.FormatDateTime(this.txtPaydate.Text.Trim())
                                                    };
                                                    PAYMENT.NEXT_DUE = POLICY_DETAIL.COVER_TYPE == bl_micro_product_config.PERIOD_TYPE.Y ? PAYMENT.DUE_DATE.AddYears(1) : (POLICY_DETAIL.COVER_TYPE == bl_micro_product_config.PERIOD_TYPE.M ? PAYMENT.DUE_DATE.AddMonths(POLICY_DETAIL.COVER_YEAR) : PAYMENT.DUE_DATE);
                                                    PAYMENT.PREMIUM_YEAR = 1;
                                                    PAYMENT.PREMIUM_LOT = 1;
                                                    PAYMENT.USER_PREMIUM = this._IsMainPolicy ? Convert.ToDouble(this.txtUserPremium.Text.Trim()) : 0.0;
                                                    PAYMENT.AMOUNT = Convert.ToDouble(this.txtTotalPremium.Text);
                                                    PAYMENT.DISCOUNT_AMOUNT = Convert.ToDouble(this.txtTotalDiscountAmount.Text.Trim() == "" ? "0" : this.txtTotalDiscountAmount.Text.Trim());
                                                    PAYMENT.TOTAL_AMOUNT = Convert.ToDouble(this.txtTotalPremiumAfterDiscount.Text);
                                                    PAYMENT.POLICY_STATUS = "IF";
                                                    PAYMENT.OFFICE_ID = "Head Office";
                                                    PAYMENT.PAY_MODE = Convert.ToInt32(this.ddlPaymentMode.SelectedValue);
                                                    PAYMENT.TRANSACTION_TYPE = "";
                                                    PAYMENT.REFERANCE_TRANSACTION_CODE = this.txtPaymentRefNo.Text.Trim();
                                                    PAYMENT.UPDATED_BY = this.userName;
                                                    PAYMENT.UPDATED_ON = tranDate;
                                                    PAYMENT.POLICY_PAYMENT_ID = this.hdfPolicyPaymentID.Value;
                                                    PAYMENT.REFERRAL_FEE = 0.0;
                                                    PAYMENT.REFERRAL_INCENTIVE = 0.0;
                                                    flag = da_micro_policy_payment.UpdatePayment(PAYMENT);
                                                    if (flag)
                                                    {
                                                        if (this.BeneficiaryList != null)
                                                        {
                                                            foreach (bl_micro_policy_beneficiary beneficiary in da_micro_policy_beneficiary.GetBeneficiaryList(POLICY.POLICY_ID))
                                                                da_micro_policy_beneficiary.DeleteBeneficiary(beneficiary.ID);
                                                            foreach (bl_micro_application_beneficiary beneficiary in this.BeneficiaryList)
                                                            {
                                                                bl_micro_policy_beneficiary BENEFICIARY = new bl_micro_policy_beneficiary();
                                                                beneficiary.ID = Helper.GetNewGuid(new string[2, 2]
                                {
                                  {"TABEL","CT_MICRO_POLICY_BENEFICIARY"},{"FIELD","ID"}
                                });
                                                                BENEFICIARY.ADDRESS = beneficiary.ADDRESS;
                                                                BENEFICIARY.AGE = beneficiary.AGE;
                                                                BENEFICIARY.IdType = beneficiary.IdType;
                                                                BENEFICIARY.IdNo = beneficiary.IdNo;
                                                                BENEFICIARY.BirthDate = beneficiary.DOB;
                                                                BENEFICIARY.FULL_NAME = beneficiary.FULL_NAME;
                                                                BENEFICIARY.RELATION = beneficiary.RELATION;
                                                                BENEFICIARY.PERCENTAGE_OF_SHARE = beneficiary.PERCENTAGE_OF_SHARE;
                                                                BENEFICIARY.POLICY_ID = this.hdfPolicyID.Value;
                                                                BENEFICIARY.CREATED_BY = this.userName;
                                                                BENEFICIARY.CREATED_ON = tranDate.AddSeconds(1.0);
                                                                flag = da_micro_policy_beneficiary.SaveBeneficiary(BENEFICIARY);
                                                                if (!flag)
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                        message = da_micro_policy_payment.MESSAGE;
                                                }
                                                else
                                                    message = da_micro_policy_rider.MESSAGE;
                                            }
                                            else
                                                message = da_micro_policy_detail.MESSAGE;
                                        }
                                        else
                                            message = da_micro_policy.MESSAGE;
                                    }
                                    else
                                        message = da_micro_customer.MESSAGE;
                                }
                                else
                                    message = da_micro_policy.MESSAGE;
                            }
                            string smsUpdate = "<strong>UPDATED APPLICATION:</strong> <br />";
                            foreach (string appNo in existAppList)
                                smsUpdate += string.Format("{0} --> Successfully.<br />", appNo);

                            smsUpdate += "<strong>UPDATED POLICY:</strong> <br />";
                            if (flag)
                            {
                                Helper.Alert(false, string.Format(smsUpdate + "{0} --> Successfully. <br />", this.hdfPolicyNumber.Value), this.lblError);
                                this.DeleteBackupApp();
                                this.DeleteBackupPolicy();
                                this.BindExisting(false);
                            }
                            else
                            {
                                smsUpdate += string.Format("{0} --> Fail. <br />", this.hdfPolicyNumber.Value);
                                string roleBakSms = "";
                                this._RoleBack(existAppList, this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"])), out roleBakSms);
                                bool flagRoleBack = this.RollBackPolicy();
                                Helper.Alert(true, string.Format(smsUpdate + "{0}<br /> Rollback policy {1}", roleBakSms, (flagRoleBack ? "successfully" : "fail")), this.lblError);
                            }
                        }
                        else
                        {
                            string roleBakSms;
                            this._RoleBack(existAppList, this.userName, tranDate, out roleBakSms);
                            Helper.Alert(true, string.Concat(message, "<br />", roleBakSms), this.lblError);
                        }
                    }
                    else
                        Helper.Alert(true, "Customer is updated fial.", this.lblError);
                }
                else
                    Helper.Alert(true, "Backup application fail", this.lblError);
            }
            else
                Helper.Alert(true, message, this.lblError);
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Application is saved fail, place contact your system administrator.", this.lblError);
            Log.AddExceptionToLog(string.Format("Error function [ibtnSave_Click(object sender, ImageClickEventArgs e)] class [individule_micro_application_form_aspx.cs], detail: ", "{0}==>{1}", ex.Message, ex.StackTrace));
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
        string str = DateTime.Now.ToString("dd-MM-yyyy");
        this.ibtnPrintApplication.Attributes.Add("disabled", "disabled");
        this.ibtnPrintCertificate.Attributes.Add("disabled", "disabled");
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
            this.ddlProduct.Items.Add(new ListItem(string.Concat(microProductConfig.Product_ID, " / ", microProductConfig.MarketingName), microProductConfig.Product_ID));
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
        this.ddlNumberofYear.Attributes.Add("disabled", "disabled");
        this.txtTotalSumAssure.Attributes.Add("disabled", "disabled");
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
    }

    private void BindPaymode()
    {

        DataTable data = new DB().GetData(AppConfiguration.GetConnectionString(), "SELECT Pay_Mode_ID, MODE FROM Ct_Payment_Mode;");
        this.ddlPaymentMode.Items.Clear();
        this.ddlPaymentMode.Items.Add(new ListItem("---Select---", ""));
        if (_ProductConfig.Product_ID == null)
            return;

        string str = "";
        //foreach (int num in _ProductConfig.PayMode)
        //  str = str == "" ? string.Concat((object) num) : $"{str},{(object) num}";

        str = string.Join(",", _ProductConfig.PayMode);
        str = string.Format("pay_mode_id in ({0})", str);
        foreach (DataRow dataRow in data.Select(str))
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
            else if (Convert.ToDouble(this.txtSumAssure.Text.Trim()) < _ProductConfig.Sum_Min && Convert.ToDouble(this.txtSumAssure.Text.Trim()) > _ProductConfig.Sum_Max)
            {
                flag = false;
                str = string.Format("Sum Assured is allowed [{0} - {1}]", _ProductConfig.Sum_Min, _ProductConfig.Sum_Max);
            }
            else
            {
                int cusAge = Convert.ToInt32(this.txtCustomerAge.Text);
                if (cusAge < _ProductConfig.Age_Min || cusAge > _ProductConfig.Age_Max)
                {
                    flag = false;
                    str = string.Format("Age [{0}] is not allow.", cusAge);
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
        bl_micro_product_config productConfig = this._ProductConfig;
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
        else if (this.ddlNumberofApplication.SelectedValue == "0" && _IsMainPolicy)
        {
            flag = false;
            str = "Number of application is required.";
        }
        else if (this.ddlNumberofYear.SelectedValue == "0" && _IsMainPolicy)
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
        if (flag && _IsMainPolicy && Convert.ToDecimal(txtUserPremium.Text.Trim()) < Convert.ToDecimal(txtTotalPremiumAfterDiscount.Text.Trim()))
        {

            flag = false;
            str = "Collected premium is lower than Total premium after discount is not allowed.";

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
        if (this.ddlProduct.SelectedIndex <= 0)
            return;
        double SumAssured = this.txtSumAssure.Text.Trim() == "" ? 0.0 : Convert.ToDouble(this.txtSumAssure.Text.Trim());
        int gender = this.ddlGender.SelectedIndex == 0 ? -1 : Convert.ToInt32(this.ddlGender.SelectedValue);
        string selectedValue = this.ddlProduct.SelectedValue;
        int cusAge = this.txtCustomerAge.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtCustomerAge.Text.Trim());
        int payMode = this.ddlPaymentMode.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlPaymentMode.SelectedValue);
        double[] premium = Calculation.GetMicroProducPremium(selectedValue, gender, cusAge, SumAssured, payMode);
        this.txtPremium.Text = premium[1].ToString();
        this.txtAnnualPremium.Text = premium[0].ToString();
        this.CalculateDiscount();
    }

    private void CalculateRiderPremium()
    {
        if (this.ddlProduct.SelectedIndex <= 0 || this.ddlPaymentMode.SelectedIndex <= 0 || this.ddlRiderSumAssure.SelectedIndex <= 0)
            return;
        double sa = this.ddlRiderSumAssure.Items.Count == 0 ? 0.0 : Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue);
        int gender = this.ddlGender.SelectedIndex == 0 ? -1 : Convert.ToInt32(this.ddlGender.SelectedValue);
        string prodId = this.ddlRiderProduct.SelectedValue;
        int cusAge = this.txtCustomerAge.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtCustomerAge.Text.Trim());
        int payMode = this.ddlPaymentMode.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlPaymentMode.SelectedValue);

        double[] productRiderPremium = Calculation.GetMicroProductRiderPremium(prodId, gender, cusAge, sa, payMode);
        double premium = productRiderPremium[1];
        double annualPremium = productRiderPremium[0];
        //if (this.txtPremium.Text.Trim() != "")
        //  Convert.ToDouble(this.txtPremium.Text.Trim());
        this.txtRiderPremium.Text = premium + "";
        this.txtRiderAnnualPremium.Text = annualPremium + "";
        this.CalculateDiscount();
    }

    private void CalculateDiscount()
    {
        double basicDis = 0.0;
        double riderDis = 0.0;
        bl_micro_product_discount_config productDiscountConfig = new bl_micro_product_discount_config();
        string productID = this.hdfProductID.Value;
        string productRiderID = this.hdfRiderProductID.Value;
        double basicSumAssured = Convert.ToDouble(this.txtSumAssure.Text.Trim() == "" ? "0" : this.txtSumAssure.Text.Trim());
        double riderSumAssured = Convert.ToDouble(this.ddlRiderSumAssure.SelectedValue == "" ? "0" : this.ddlRiderSumAssure.SelectedValue);
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
        double totalPremium = Convert.ToDouble(this.txtPremium.Text.Trim() == "" ? "0" : this.txtPremium.Text.Trim()) + Convert.ToDouble(this.txtRiderPremium.Text.Trim() == "" ? "0" : this.txtRiderPremium.Text.Trim());
        double basicAfterDis = Convert.ToDouble(this.txtPremium.Text.Trim() == "" ? "0" : this.txtPremium.Text.Trim()) - basicDis;
        double riderAfterDis = Convert.ToDouble(this.txtRiderPremium.Text.Trim() == "" ? "0" : this.txtRiderPremium.Text.Trim()) - basicDis;
        double totalAfterDis = basicAfterDis + riderAfterDis;
        double totalDis = basicDis + riderDis;
        this.txtBasicDiscount.Text = basicDis.ToString();
        this.txtBasicAfterDiscount.Text = basicAfterDis.ToString();
        this.txtRiderDiscount.Text = riderDis.ToString();
        this.txtRiderAfterDiscount.Text = riderAfterDis.ToString();
        this.txtTotalDiscountAmount.Text = totalDis.ToString();
        this.txtTotalPremium.Text = totalPremium.ToString();
        this.txtTotalPremiumAfterDiscount.Text = totalAfterDis.ToString();
    }

    protected void ddlRiderProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        this._ProductRider = da_micro_product_rider.GetMicroProductByProductID(this.ddlRiderProduct.SelectedValue);
        if (!string.IsNullOrWhiteSpace(this._ProductRider.PRODUCT_ID))
        {
            this.txtRiderProductName.Text = this._ProductRider.EN_TITLE;
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

        if (_ProductConfig == null)
            return;
        if (this.txtSumAssure.Text.Trim() == "")
        {
            this.txtAnnualPremium.Text = "";
            Helper.Alert(true, "Sum Assured is required.", lblError);
        }
        else if (!Helper.IsNumber(this.txtSumAssure.Text.Trim()))
            Helper.Alert(true, "Sum Assured is allowed only number.", lblError);
        else if (Convert.ToDouble(this.txtSumAssure.Text.Trim()) < _ProductConfig.Sum_Min || Convert.ToDouble(this.txtSumAssure.Text.Trim()) > _ProductConfig.Sum_Max)
        {
            Helper.Alert(true, string.Concat("Sum Assured is allowed [", _ProductConfig.Sum_Min, " - ", _ProductConfig.Sum_Max, "]."), lblError);
            this.txtSumAssure.Text = "";
            this.txtPremium.Text = "";
        }
        else
            this.CalculatePremium();
    }

    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!(this.ddlPaymentMode.SelectedValue != ""))
            return;
        this.CalculatePremium();
        if (string.IsNullOrWhiteSpace(this._ProductConfig.RiderProductID))
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
        if (this.ddlProvinceKh.SelectedValue.ToUpper() == "12/PHNOM PENH" || this.ddlProvinceKh.SelectedValue.ToUpper() == "PHNOM PENH")
        {
            this.txtBenAddress.Text = (this.txtHouseNoKh.Text.Trim() == "" ? "" : "ផ្ទះលេខ" + this.txtHouseNoKh.Text) + " " + (this.txtStreetKh.Text.Trim() == "" ? "" : "ផ្លូវលេខ" + this.txtStreetKh.Text.Trim()) + " " + (this.ddlVillageKh.SelectedValue.Trim() == "" ? "" : "ភូមិ" + this.ddlVillageKh.SelectedItem.Text.Trim()) + " " + (this.ddlCommuneKh.SelectedValue.Trim() == "" ? "" : "សង្កាត់" + this.ddlCommuneKh.SelectedItem.Text.Trim()) + " " + (this.ddlDistrictKh.SelectedValue.Trim() == "" ? "" : "ខណ្ឌ" + this.ddlDistrictKh.SelectedItem.Text.Trim()) + " " + (this.ddlProvinceKh.SelectedValue.Trim() == "" ? "" : "ក្រុង" + this.ddlProvinceKh.SelectedItem.Text);
        }
        else
        {
            if (!(this.ddlProvinceKh.SelectedValue != ""))
                return;
            this.txtBenAddress.Text = (this.txtHouseNoKh.Text.Trim() == "" ? "" : "ផ្ទះលេខ" + this.txtHouseNoKh.Text) + " " + (this.txtStreetKh.Text.Trim() == "" ? "" : "ផ្លូវលេខ" + this.txtStreetKh.Text.Trim()) + " " + (this.ddlVillageKh.SelectedValue.Trim() == "" ? "" : "ភូមិ" + this.ddlVillageKh.SelectedItem.Text.Trim()) + " " + (this.ddlCommuneKh.SelectedValue.Trim() == "" ? "" : "ឃុំ" + this.ddlCommuneKh.SelectedItem.Text.Trim()) + " " + (this.ddlDistrictKh.SelectedValue.Trim() == "" ? "" : "ស្រុក" + this.ddlDistrictKh.SelectedItem.Text.Trim()) + " " + (this.ddlProvinceKh.SelectedValue.Trim() == "" ? "" : "ខេត្ត" + this.ddlProvinceKh.SelectedItem.Text);
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

    private void BindExisting(bool firstLoad = true)
    {
        try
        {
            bl_micro_application_details applicationDetail = da_micro_application.GetApplicationDetail(da_micro_application.GetApplicationByApplicationID(this.hdfApplicationID.Value).APPLICATION_NUMBER);

            decimal totalPremiumCollection = 0;
            if (applicationDetail != null)
            {
                this.ViewState["VS_BAK_DATE"] = (object)DateTime.Now;
                if (da_micro_application.BackupApplication(this.hdfApplicationID.Value, "UPDATE", this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"]))))
                {
                    Helper.BindChannel(this.ddlChannel);
                    Helper.BindChannelItem(this.ddlCompany, applicationDetail.Application.CHANNEL_ID);
                    Helper.BindChanneLocation(this.ddlBranch, applicationDetail.Application.CHANNEL_ITEM_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlChannel, applicationDetail.Application.CHANNEL_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlCompany, applicationDetail.Application.CHANNEL_ITEM_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlBranch, applicationDetail.Application.CHANNEL_LOCATION_ID);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlClientType, applicationDetail.Application.CLIENT_TYPE);
                    this.ddlClientType_SelectedIndexChanged((object)null, (EventArgs)null);
                    this.txtBankStaffName.Text = applicationDetail.Application.CLIENT_TYPE_REMARKS;
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlClientTypeRelation, applicationDetail.Application.CLIENT_TYPE_RELATION);
                    this.hdfProductID.Value = applicationDetail.Insurance.PRODUCT_ID;
                    this.txtApplicationNumber.Text = applicationDetail.Application.APPLICATION_NUMBER;
                    this.txtApplicationDate.Text = applicationDetail.Application.APPLICATION_DATE.ToString("dd-MM-yyyy");
                    this._IsMainPolicy = applicationDetail.Application.MainApplicationNumber == "";
                    if (!this._IsMainPolicy && firstLoad)
                        Helper.Alert(false, "Sub Application is selected.", this.lblError);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlBranch, applicationDetail.OfficeCode);
                    this.txtSaleAgentID.Text = applicationDetail.Application.SALE_AGENT_ID;
                    this.txtSaleAgentName.Text = applicationDetail.Application.SALE_AGENT_NAME;
                    this.hdfApplicationNumber.Value = applicationDetail.Application.APPLICATION_NUMBER;
                    this.hdfChannelID.Value = applicationDetail.Application.CHANNEL_ID;
                    this.hdfChannelItemID.Value = applicationDetail.Application.CHANNEL_ITEM_ID;
                    this.hdfChannelLocationID.Value = applicationDetail.Application.CHANNEL_LOCATION_ID;
                    this.hdfSaleAgentID.Value = applicationDetail.Application.SALE_AGENT_ID;
                    this.hdfApplicationID.Value = applicationDetail.Application.APPLICATION_ID;
                    this.txtReferrerId.Text = applicationDetail.Application.REFERRER_ID;
                    this.txtReferrerName.Text = applicationDetail.Application.REFERRER;
                    this.hdfApplicationCustomerID.Value = applicationDetail.Customer.CUSTOMER_ID;
                    this.hdfApplicationNumber.Value = applicationDetail.Application.APPLICATION_NUMBER;
                    this.txtLoanNumber.Text = applicationDetail.Application.LoanNumber;
                    if (!string.IsNullOrWhiteSpace(applicationDetail.PolicyId))
                    {
                        this.hdfPolicyID.Value = applicationDetail.PolicyId;
                        this.hdfPolicyNumber.Value = applicationDetail.PolicyNumber;
                        this.hdfCustomerID.Value = da_micro_customer.GetCustomer(da_micro_policy.GetPolicyByID(applicationDetail.PolicyId).CUSTOMER_ID).ID;
                        bl_micro_policy_detail detailByPolicyId = da_micro_policy_detail.GetPolicyDetailByPolicyID(applicationDetail.PolicyId);
                        this.hdfPolicyDetailID.Value = detailByPolicyId.POLICY_DETAIL_ID;
                        DataRow[] rowPayment = da_micro_policy_payment.GetPolicyPaymentDetail(applicationDetail.PolicyId).Select(string.Concat("policy_detail_id='", detailByPolicyId.POLICY_DETAIL_ID, "'"));
                        int index = 0;
                        if (index < rowPayment.Length)
                        {
                            DataRow dataRow = rowPayment[index];
                            this.hdfPolicyPaymentID.Value = dataRow["policy_payment_id"].ToString();
                            this.txtPaymentRefNo.Text = dataRow["TRANSACTION_REFERRENCE_NO"].ToString();
                            this.txtPaydate.Text = Convert.ToDateTime(dataRow["pay_date"].ToString()).ToString("dd-MM-yyyy");
                            totalPremiumCollection = Convert.ToDecimal(dataRow["USER_PREMIUM"].ToString());
                        }

                        this.hdfPolicyBenID.Value = da_micro_policy_beneficiary.GetBeneficiary(applicationDetail.PolicyId).ID;
                        bl_micro_policy_rider rider = da_micro_policy_rider.GetRider(applicationDetail.PolicyId);
                        if (!string.IsNullOrWhiteSpace(rider.PRODUCT_ID))
                            this.hdfRiderProductIDOld.Value = rider.PRODUCT_ID;
                        this.txtPolicyNumber.Text = applicationDetail.PolicyNumber;
                        this.txtPolicyStatus.Text = applicationDetail.PolicyStatus;
                        this.ibtnPrintCertificate.Attributes.Remove("disabled");
                        this.txtIssueDate.Attributes.Remove("disabled");
                        this.txtEffectiveDate.Attributes.Remove("disabled");
                        this.txtPaydate.Attributes.Remove("disabled");
                        this.txtPaymentRefNo.Attributes.Remove("disabled");
                        this.txtUserPremium.Attributes.Remove("disabled");
                        this.txtIssueDate.Text = detailByPolicyId.ISSUED_DATE.ToString("dd-MM-yyyy");
                        this.txtEffectiveDate.Text = detailByPolicyId.EFFECTIVE_DATE.ToString("dd-MM-yyyy");
                    }
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
                    string village = applicationDetail.Customer.VILLAGE_CODE == "" ? applicationDetail.Customer.VILLAGE_EN : string.Concat(applicationDetail.Customer.VILLAGE_CODE, "/", applicationDetail.Customer.VILLAGE_EN);
                    string commune = applicationDetail.Customer.COMMUNE_CODE == "" ? applicationDetail.Customer.COMMUNE_EN : string.Concat(applicationDetail.Customer.COMMUNE_CODE, "/", applicationDetail.Customer.COMMUNE_EN);
                    string distict = applicationDetail.Customer.DISTRICT_CODE == "" ? applicationDetail.Customer.DISTRICT_EN : string.Concat(applicationDetail.Customer.DISTRICT_CODE, "/", applicationDetail.Customer.DISTRICT_EN);
                    string province = applicationDetail.Customer.PROVINCE_CODE == "" ? applicationDetail.Customer.PROVINCE_EN : string.Concat(applicationDetail.Customer.PROVINCE_CODE, "/", applicationDetail.Customer.PROVINCE_EN);
                    this.txtVillageEn.Text = village;
                    this.txtCommuneEn.Text = commune;
                    this.txtDistrictEn.Text = distict;
                    this.txtProvinceEn.Text = province;
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
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlTermOfCover, applicationDetail.Insurance.TERME_OF_COVER.ToString());
                    Helper.SelectedDropDownListIndex("VALUE", ddlCoverType, applicationDetail.Insurance.COVER_TYPE.ToString());
                    this.txtPremiumPaymentPeriod.Text = applicationDetail.Insurance.PAYMENT_PERIOD.ToString();
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofApplication, applicationDetail.Application.NumbersOfApplicationFirstYear.ToString());
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofYear, applicationDetail.Application.NumbersOfPurchasingYear.ToString());
                    this.txtTotalSumAssure.Text = (applicationDetail.Insurance.SUM_ASSURE * (double)applicationDetail.Application.NumbersOfApplicationFirstYear).ToString();
                    this.BindPayMode(this._ProductConfig.PayMode);
                    this.txtSumAssure.Text = string.Concat((object)applicationDetail.Insurance.SUM_ASSURE);
                    Helper.SelectedDropDownListIndex("VALUE", this.ddlPaymentMode, applicationDetail.Insurance.PAY_MODE.ToString());
                    this.txtPremium.Text = applicationDetail.Insurance.PREMIUM.ToString();
                    this.txtAnnualPremium.Text = applicationDetail.Insurance.ANNUAL_PREMIUM.ToString();
                    this.txtBasicDiscount.Text = applicationDetail.Insurance.DISCOUNT_AMOUNT.ToString();
                    this.txtBasicAfterDiscount.Text = applicationDetail.Insurance.TOTAL_AMOUNT.ToString();
                    if (this._ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
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
                        this.BindRiderSA(this._ProductConfig.RiderSumAssuredRange);
                        this.txtRiderProductName.Text = applicationDetail.Rider.PRODUCT_NAME;
                        this.txtRiderPremium.Text = string.Concat((object)applicationDetail.Rider.PREMIUM);
                        this.txtRiderAnnualPremium.Text = string.Concat((object)applicationDetail.Rider.ANNUAL_PREMIUM);
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlRiderSumAssure, string.Concat((object)applicationDetail.Rider.SUM_ASSURE));
                        Helper.SelectedDropDownListIndex("VALUE", this.ddlRiderProduct, applicationDetail.Rider.PRODUCT_ID);
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
                        this.ddlRiderSumAssure.Items.Clear();
                        this.txtRiderProductName.Text = "";
                        this.txtRiderPremium.Text = "";
                        this.txtRiderAnnualPremium.Text = "";
                        this.txtRiderDiscount.Text = "";
                        this.txtRiderAfterDiscount.Text = "";
                        if (!string.IsNullOrEmpty(this.dvRider.Attributes["style"]))
                        {
                            AttributeCollection attributes;
                            (attributes = this.dvRider.Attributes)["style"] = attributes["style"] + " display:none;";
                        }
                        else
                            this.dvRider.Attributes.Add("style", "display:none;");
                    }
                    double basic = this.txtPremium.Text.Trim() != "" ? Convert.ToDouble(this.txtPremium.Text) : 0.0;
                    double riderPrem = this.txtRiderPremium.Text.Trim() != "" ? Convert.ToDouble(this.txtRiderPremium.Text) : 0.0;
                    double basicDis = this.txtBasicDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtBasicDiscount.Text) : 0.0;
                    double riderDis = this.txtRiderDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtRiderDiscount.Text) : 0.0;
                    double basicAfterDis = this.txtBasicAfterDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtBasicAfterDiscount.Text) : 0.0;
                    double riderAfterDis = this.txtRiderAfterDiscount.Text.Trim() != "" ? Convert.ToDouble(this.txtRiderAfterDiscount.Text) : 0.0;
                    this.txtTotalPremium.Text = (basic + riderPrem).ToString();
                    this.txtTotalDiscountAmount.Text = (basicDis + riderDis).ToString();
                    this.txtTotalPremiumAfterDiscount.Text = (basicAfterDis + riderAfterDis).ToString();
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
                    this.txtUserPremium.Text = (_IsMainPolicy ? totalPremiumCollection : 0) + "";
                    this.ibtnPrintApplication.Attributes.Remove("disabled");
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
            Log.AddExceptionToLog(string.Format("Error function [BindExsting] in class [micro_application_form_edit.aspx.cs], error lin:{0} detail: {1} ==> {2}", Log.GetLineNumber(ex), ex.Message, ex.Source));
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
        this.Page_Load((object)null, (EventArgs)null);
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
            Helper.Alert(true, "Beneficiary Address is required as number.", this.lblError);
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
                    applicationBeneficiary.ID = string.Concat((object)(this.BeneficiaryList.Count + 1));
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
        this.gvBen.DataSource = (object)this.BeneficiaryList;
        this.gvBen.DataBind();
        this.hdfBeneficiaryId.Value = "";
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
        int int32 = Convert.ToInt32(Convert.ToDouble(this.txtTotalSumAssure.Text.Trim()) / 5000.0);
        int num = int32 == 0 || int32 < 1 ? 1 : int32;
        if (num > this._MaxPolicyPerLife)
        {
            Helper.Alert(true, "Policy per life is over limit.", this.lblError);
            this.ddlNumberofApplication.SelectedIndex = 0;
        }
        else
            Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofApplication, string.Concat((object)num));
    }

    private void _RoleBack(
      List<string> existAppList,
      string userName,
      DateTime tranDate,
      out string message)
    {
        string str = "";
        foreach (string existApp in existAppList)
        {
            str += !da_micro_application.RestoreApplication(existApp, userName, tranDate) ? string.Concat(existApp, " fail <br />") : string.Concat(existApp, " success <br />");

        }
        str = str != "" ? "<strong>RESTORED APPLICATION:</strong> <br />" + str : "";
        message = str;
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
        Options.Bind(this.ddlRelation, (object)da_master_list.da_beneficiary_relation.GetBeneficiaryRelationList(Convert.ToInt32(this.ddlBenGender.SelectedValue)), "RelationKh", "RelationKh", -1);
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
            this.ddlNumberofYear.SelectedValue = "1";
            ddlTermOfCover.Attributes.Add("disabled", "disabled");
            //ddlNumberofYear.Attributes.Add("disabled", "disabled");
        }
        else
        {
            ddlTermOfCover.Attributes.Remove("disabled");
            Helper.SelectedDropDownListIndex("VALUE", this.ddlNumberofYear, "1");
        }
    }

    protected void txtLoanNumber_TextChanged(object sender, EventArgs e)
    {
        if (this._ProductConfig == null)
            return;
        if (this._ProductConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString())
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
        }
        else
            this.ddlTermOfCover.Attributes.Remove("disabled");
    }

    private bool RollBackApp()
    {
        return da_micro_application.RestoreApplication(this.hdfApplicationNumber.Value, this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"])));
    }

    private bool DeleteBackupApp()
    {
        return da_micro_application.DeleteBackupApplication(this.hdfApplicationNumber.Value, this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"])));
    }

    private bool RollBackPolicy()
    {
        return da_micro_policy.RestorePolicy(this.hdfPolicyNumber.Value, this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"])));
    }

    private bool DeleteBackupPolicy()
    {
        return da_micro_policy.DeleteBackupPolicy(this.hdfPolicyNumber.Value, this.userName, Convert.ToDateTime(string.Concat(this.ViewState["VS_BAK_DATE"])));
    }

    protected void txtBasicDiscount_TextChanged(object sender, EventArgs e)
    {
        if (Helper.IsAmount(this.txtBasicDiscount.Text.Trim()))
        {
            double basicDis = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
            double premium = Convert.ToDouble(this.txtPremium.Text.Trim() == "" ? "0" : this.txtPremium.Text.Trim());
            if (basicDis > premium)
            {
                Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
                this.txtBasicDiscount.Text = "0";
                basicDis = 0.0;
            }
            this.txtBasicAfterDiscount.Text = (premium - basicDis).ToString();
            this.ComputeTotalDiscount();
        }
        else
            Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
    }

    protected void txtRiderDiscount_TextChanged(object sender, EventArgs e)
    {
        if (Helper.IsAmount(this.txtRiderDiscount.Text.Trim()))
        {
            double dis = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
            double premium = Convert.ToDouble(this.txtRiderPremium.Text.Trim() == "" ? "0" : this.txtRiderPremium.Text.Trim());
            if (dis > premium)
            {
                Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
                this.txtRiderDiscount.Text = "0";
                dis = 0.0;
            }
            this.txtRiderAfterDiscount.Text = (premium - dis).ToString();
            this.ComputeTotalDiscount();
        }
        else
            Helper.Alert(true, "Discount amount is greater than premium is not allow.", this.lblError);
    }

    private void ComputeTotalDiscount()
    {
        double basicDis = Convert.ToDouble(this.txtBasicDiscount.Text.Trim() == "" ? "0" : this.txtBasicDiscount.Text.Trim());
        double riderDis = Convert.ToDouble(this.txtRiderDiscount.Text.Trim() == "" ? "0" : this.txtRiderDiscount.Text.Trim());
        double basicAfterDis = Convert.ToDouble(this.txtBasicAfterDiscount.Text.Trim() == "" ? "0" : this.txtBasicAfterDiscount.Text.Trim());
        double riderAfterDis = Convert.ToDouble(this.txtRiderAfterDiscount.Text.Trim() == "" ? "0" : this.txtRiderAfterDiscount.Text.Trim());
        this.txtTotalDiscountAmount.Text = (basicDis + riderDis).ToString();
        this.txtTotalPremiumAfterDiscount.Text = (basicAfterDis + riderAfterDis).ToString();
    }


    public class ResponeApplicationForm
    {
        public int StatusCode { get; set; }

        public string Status { get; set; }

        public byte[] ApplicationForm { get; set; }

        public string Message { get; set; }
    }


    [WebMethod(EnableSession = true)]
    public  static  string ConfirmAction(string policyId, string attachedPolicyInsurance)
    {
      
       var username = HttpContext.Current.User.Identity.Name;
     
       var pol = da_micro_policy.GetPolicyByID(policyId, username);
       var pro = da_micro_product_config.ProductConfig.GetProductMicroProduct(pol.PRODUCT_ID);

        da_sys_activity_log.Save(new bl_sys_activity_log(username, "", bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, DateTime.Now, "User views certificate. [Pol No:" + pol.POLICY_NUMBER +"]", Membership.ApplicationName));
        if (pro.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || pro.CreatedOn.Year >= 2025)
        {
            HttpContext.Current.Session["POL_ID_PRINT"] = new List<string>() 
      {
        pol.POLICY_ID
      };
          
            return "load_new_certificate.aspx?policyType=IND&printPolInsurance="+ (attachedPolicyInsurance);
        }
        else
        {
          
            return string.Format("banca_micro_cert.aspx?P_ID={0}&P_TYPE=IND", pol.POLICY_ID);
        }
       
    }
}
