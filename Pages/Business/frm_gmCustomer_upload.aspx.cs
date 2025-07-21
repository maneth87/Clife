using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.Security;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;

public partial class Pages_Business_frm_gmCustomer_upload : System.Web.UI.Page
{
    string userID = "";
    string userName = "";

    private bl_group_master_product PrivateGroupMasterProduct { get { return (bl_group_master_product)ViewState["VS_GROUP_MASTER_PROUCT"]; } set { ViewState["VS_GROUP_MASTER_PROUCT"] = value; } }
    private string MyMessage { get { return ViewState["VS_MY_MESSAGE"] + ""; } set { ViewState["VS_MY_MESSAGE"] = value; } }
    private bl_micro_product_config MyProductConfig { get { return (bl_micro_product_config)ViewState["VS_PRODUCT_CONF"]; } set { ViewState["VS_PRODUCT_CONF"] = value; } }
    /// <summary>
    /// Full file path
    /// </summary>
    private string MyFilePath { get { return ViewState["VS_MY_FILE_PATH"] + ""; } set { ViewState["VS_MY_FILE_PATH"] = value; } }
    /// <summary>
    /// Path sotre temporation file
    /// </summary>
    private string MyTempFilePath { get { return ViewState["VS_MY_TEMP_FILE_PATH"] + ""; } set { ViewState["VS_MY_TEMP_FILE_PATH"] = value; } }
    /// <summary>
    /// only file extention
    /// </summary>
    private string MyFileExtention { get { return ViewState["VS_MY_FILE_EXT"] + ""; } set { ViewState["VS_MY_FILE_EXT"] = value; } }
    /// <summary>
    /// Sub folder and file 
    /// </summary>
    private string MySubFolderFilePath { get { return ViewState["VS_MY_SUB_FILE_PATH"] + ""; } set { ViewState["VS_MY_SUB_FILE_PATH"] = value; } }

    /// <summary>
    /// Total imported records
    /// </summary>
    public int MyTotalImportRecord { get { return Convert.ToInt32(ViewState["VS_MY_TOTAL_REC_IMPORT"] + ""); } set { ViewState["VS_MY_TOTAL_REC_IMPORT"] = value; } }
    /// <summary>
    /// Total imported valide records 
    /// </summary>
    public int MyValideRecord { get { return Convert.ToInt32(ViewState["VS_MY_TOTAL_REC_IMPORT_VALIDE"] + ""); } set { ViewState["VS_MY_TOTAL_REC_IMPORT_VALIDE"] = value; } }
    /// <summary>
    /// Total imported invalide records 
    /// </summary>
    public int MyInvalideRecord { get { return Convert.ToInt32(ViewState["VS_MY_TOTAL_REC_IMPORT_INVALIDE"] + ""); } set { ViewState["VS_MY_TOTAL_REC_IMPORT_INVALIDE"] = value; } }

    /// <summary>
    /// Sale Agent Information
    /// </summary>
    private bl_sale_agent_micro AgentInfo { get { return (bl_sale_agent_micro)ViewState["VS_MY_AGENT"]; } set { ViewState["VS_MY_AGENT"] = value; } }

    /// <summary>
    /// Get existing customer after upload record
    /// </summary>
    private List<bl_micro_group_customer> ExistingCustomerList { get { return (List<bl_micro_group_customer>)ViewState["VS_MY_EXISTING_CUS"]; } set { ViewState["VS_MY_EXISTING_CUS"] = value; } }

    private List<da_micro_group_loan_upload.FirstPolicyList> FirstPolicyList { get { return (List<da_micro_group_loan_upload.FirstPolicyList>)ViewState["VS_FIRST_POL_LIST"]; } set { ViewState["VS_FIRST_POL_LIST"] = value; } }

    private DataTable MyPolicyInforce { get { return (DataTable)ViewState["VS_MY_POLICY_INFORCE"]; } set { ViewState["VS_MY_POLICY_INFORCE"] = value; } }

    private DataTable MyDigitalLoanInvalidTable { get { return (DataTable)ViewState["VS_MY_DIGI_LOAN_INVALID_TBL"]; } set { ViewState["VS_MY_DIGI_LOAN_INVALID_TBL"] = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        userID = Membership.GetUser().ProviderUserKey.ToString();
        userName = Membership.GetUser().UserName;

        lblError.Text = "";


        if (!Page.IsPostBack)
        {

            AgentInfo = null;

            ddlPaymentMode.Enabled = false;
            ddlProduct.Enabled = false;
            ddlChannelLocation.Enabled = false;
            ddlBasicSumAssure.Enabled = false;
            ddlRiderSumAssure.Enabled = false;
            ddlProductRider.Enabled = false;

            Session["SORTEX"] = "ASC";
            Session["SORTCOL"] = "ClientName";
            btnSave.Enabled = false;

            //LoadData(userName, DateTime.Now);
            ddlChannel.SelectedIndex = 2;
            ddlChannel_SelectedIndexChanged(null, null);
            ddlChannel.Enabled = false;

            /*bind project type*/
            Options.Bind(ddlProjectType, da_master_list.GetMasterList("PROJECT_TYPE"), "DescEn", "Code", -1);


            #region Table to store invalid digital loan records
            DataTable tblDigInvalid = new DataTable();
            var col = tblDigInvalid.Columns;
            col.Add("No");
            col.Add("Account Number");
            col.Add("Client Name");
            col.Add("Gender");
            col.Add("ID Type");
            col.Add("ID Number");
            col.Add("DOB");
            col.Add("Phone Number");
            col.Add("Province");
            col.Add("District");
            col.Add("Commune");
            col.Add("Village");
            col.Add("Applied Date");
            col.Add("Issue Date");
            col.Add("Effective Date");
            col.Add("Expiry Date");
            col.Add("Maturity Date");
            col.Add("Cover Period (M)");
            col.Add("First Policy?");
            col.Add("Policy Status Remarks");
            col.Add("Premium");
            col.Add("SumAssure");
            col.Add("SystemRemarks");

            MyDigitalLoanInvalidTable = tblDigInvalid;
            #endregion

        }
    }


    List<Int32[]> countPolicyList = new List<Int32[]>();
    List<Int32> countYear = new List<Int32>();
    //private List<Int32> CountPolicies(Int32 months)
    private List<Int32[]> CountPolicies(Int32 period, Helper.LoanPeriodType type)
    {
        Int32 polCount = 0;
    START:
        if (type == Helper.LoanPeriodType.M)
        {
            polCount = period - 12;
            if (polCount <= 0)
            {
                countYear.Add(period);

            }
            else if (polCount >= 1)
            {

                countYear.Add(12);
                period -= 12;
                if (period > 0)
                {
                    goto START;
                    // CountPolicies(months);
                }
            }
        }
        else if (type == Helper.LoanPeriodType.D)
        {
            polCount = period - 365;
            if (polCount <= 0)
            {
                countYear.Add(period);

            }
            else if (polCount >= 1)
            {

                countYear.Add(365);
                period -= 365;
                if (period > 0)
                {
                    goto START;
                   
                }
            }
        }
       
        Int32[] result = countYear.ToArray();
        countPolicyList.Add(result);
        return countPolicyList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idType"></param>
    /// <param name="idNumber"></param>
    /// <param name="year">Effective Year</param>
    /// <returns></returns>
    private double[] GetTotalNumberPolicyInforce(string idType, string idNumber, int year)
    {
        double[] result = new double[] { 0, 0 };
        double totalPolicy = 0;
        double totalSumAssure = 0;
        foreach (DataRow r in MyPolicyInforce.Select("ID_TYPE='" + idType + "' AND ID_NUMBER='" + idNumber + "' AND YEAR =" + year))
        {
            totalSumAssure += Convert.ToDouble(r["TOTAL_SUM_ASSURE"].ToString());
            totalPolicy += Convert.ToDouble(r["POLICY_COUNT"].ToString());

        }
        result = new double[] { totalPolicy, totalSumAssure };
        return result;
    }

    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }

    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChannelLocation.Items.Clear();
        Helper.BindChanneLocation(ddlChannelLocation, ddlChannelItem.SelectedValue);
        //PrivateGroupMasterProduct = da_group_master_product.GetGroupMasterProductByChannelItem(ddlChannelItem.SelectedValue);


        //Auto select channel location while has only one location
        if (ddlChannelLocation.Items.Count == 2)
        {
            ddlChannelLocation.SelectedIndex = 1;
            ddlChannelLocation.Enabled = false;
            ddlChannelLocation_SelectedIndexChanged(null, null);
        }
        else if (ddlChannelLocation.Items.Count > 2)
        {
            ddlChannelLocation.Enabled = true;
        }
    }

    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["SS_DATA"] != null)
        {
            gv_valid.PageIndex = e.NewPageIndex;
            DataTable tbl = (DataTable)Session["SS_DATA"];
            DataView dv = new DataView(tbl);

            gv_valid.DataSource = dv;
            gv_valid.DataBind();
            int countRow = 0;

            if (gv_valid.PageCount == e.NewPageIndex + 1)//last page
            {
                countRow = gv_valid.PageSize * (e.NewPageIndex) + gv_valid.Rows.Count;
            }
            else
            {
                countRow = gv_valid.PageSize * (e.NewPageIndex + 1);
            }
            lblRecords.Text = "Record(s): " + countRow + " of " + tbl.Rows.Count;
        }
    }

    private void EnabledControls(bool t = true)
    {
        ddlChannelItem.Enabled = t;
        ddlProduct.Enabled = t;
        ddlPayPeriodType.Enabled = t;
        ddlPayPeriod.Enabled = t;
        ddlCoverPeriodType.Enabled = t;
        ddlCoverPeriod.Enabled = t;
        ddlProjectType.Enabled = t;
        txtSaleAgentID.Enabled = t;
        txtPaymentReportDate.Enabled = t;
        fUpload.Enabled = t;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //Clear Invalid digital loan record
        MyDigitalLoanInvalidTable.Clear();
        #region Button is upload
        if (btnUpload.Text == "Upload")
        {
            if (ValidateForm())
            {
                validateFile();

                DataTable tblSuccess = new DataTable();
                DataTable tblFail = new DataTable();
                DataTable dataUpload = (DataTable)Session["SS_DATA_UPLOAD"];
                DataTable dataExistCustomer = (DataTable)Session["SS_DATA_EXISTING_CUSTOMER"];
                DataTable dataValid = (DataTable)Session["SS_DATA_VALID"];
                DataTable dataInvalid = (DataTable)Session["SS_DATA_INVALID"];
                bool isValid = (bool)ViewState["VS_VAL_ISVALID"];
                string validMessage = ViewState["VS_VAL_MESSAGE"] + "";
                DateTime tranDate = DateTime.Now;

                if (isValid)
                {
                    if (dataInvalid.Rows.Count == 0)// if (isValid)
                    {

                        bool save = false;
                        if (dataUpload.Rows.Count > 0)
                        {
                            try
                            {
                                ckbShow.Checked = true;
                                if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString())
                                {
                                    save = da_micro_group_loan_upload.UploadLoan(dataValid, userName, tranDate, ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, ddlChannelLocation.SelectedValue, "", ddlProduct.SelectedItem.Text, MySubFolderFilePath);
                                }
                                else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString())
                                {
                                    save = da_micro_group_loan_upload.UploadMoneyTransfer(dataValid, userName, tranDate, ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, ddlChannelLocation.SelectedValue, "", ddlProduct.SelectedItem.Text, MySubFolderFilePath);

                                }
                                else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString())
                                {
                                    save = da_micro_group_loan_upload.UploadPayroll(dataValid, userName, tranDate, ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, ddlChannelLocation.SelectedValue, "", ddlProduct.SelectedItem.Text, MySubFolderFilePath);
                                }
                                else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString())
                                {
                                    //save = da_micro_group_loan_upload.UploadDigitalLoan(dataValid, userName, tranDate, ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, ddlChannelLocation.SelectedValue, ddlCoverPeriodType.SelectedItem.Text, ddlProduct.SelectedItem.Text, MySubFolderFilePath);
                                    save = da_micro_group_loan_upload.UploadDigitalLoan(dataValid, userName, tranDate, ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, ddlChannelLocation.SelectedValue, "D", ddlProduct.SelectedItem.Text, MySubFolderFilePath);
                                }
                                if (save)
                                {
                                    EnabledControls(false);
                                    btnUpload.Text = "Re-Upload";

                                    try
                                    {
                                        LoadData(userName, DateTime.Now);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.AddExceptionToLog("Error function [btnUpload_Click(object sender, EventArgs e)] in class [frm_gmCustomer_upload.aspx.cs], detail:" + ex.Message+"=>", ex.StackTrace);
                                        Helper.Alert(true, "Get Uploaded data error.", lblError);
                                    }
                                }
                                else
                                {
                                    Helper.Alert(true, da_micro_group_loan_upload.MESSAGE, lblError);
                                }

                            }
                            catch (Exception ex)
                            {
                                Helper.Alert(true, "Imported fail, detail error:" + ex.Message, lblError);
                            }
                        }
                        else
                        {
                            ckbShow.Checked = false;
                            Helper.Alert(true, "No record upload.", lblError);
                        }
                    }
                    else
                    {
                        btnSave.Enabled = false;
                        //Helper.Alert(true, validMessage, lblError);
                        Helper.Alert(true, "System is not allow to proceed convert to policy due to invalid data is detected. Please view invalid data in TAB Invalid.", lblError);
                        //gv_digital_loan.DataSource = dataValid;
                        //gv_digital_loan.DataBind();

                        gvInvalid.DataSource = dataInvalid;
                        gvInvalid.DataBind();
                    }
                }
                else
                {
                    Helper.Alert(true, validMessage, lblError);
                }
            }
            else
            {
                btnSave.Enabled = false;
                Helper.Alert(true, MyMessage, lblError);
            }
        }
        #endregion
        #region Button is reupload
        else if (btnUpload.Text == "Re-Upload")
        {
            EnabledControls();
            gvMoneyTransfer.DataSource = null;
            gvMoneyTransfer.DataBind();

            gv_valid.DataSource = null;
            gv_valid.DataBind();

            gv_digital_loan.DataSource = null;
            gv_digital_loan.DataBind();

            gvInvalid.DataSource = null;
            gvInvalid.DataBind();

            lblRecords.Text = "";
            btnUpload.Text = "Upload";

            //disable button convert to policy
            btnSave.Enabled = false;

            Session["SS_DATA_EXISTING_CUSTOMER"] = null;
            
        }
        #endregion
    }

    //void AddInvalidRecord(string accountNumber, string clientName, string gender, string idType, string idNumber, string dob, string phoneNumber, string province, string district, string commune, string village, string appliedDate, string maturityDate, string remarks ,string occupation, string benName, string benRelation, string systemRemarks)
    void AddInvalidRecord(bl_wing_digital_loan_upload digitalLoanObject, string systemRemarks)
    {

        DataRow r = MyDigitalLoanInvalidTable.NewRow();
        var obj = digitalLoanObject;
        int rowcount = MyDigitalLoanInvalidTable.Rows.Count + 1;
        r["No"] = rowcount;
        r["Account Number"] = obj.AccountNumber;
        r["Client Name"] = obj.ClientName;
        r["Gender"] = obj.GenderText;
        r["ID Type"] = obj.IdTypeText;
        r["ID Number"] = obj.IdNumber;
        r["DOB"] = obj.DOB.ToString("dd-MMM-yyyy");
        r["Phone Number"] = obj.PhoneNumber;
        r["Province"] = obj.Province;
        r["District"] = obj.District;
        r["Commune"] = obj.Commune;
        r["Village"] = obj.Village;
        r["Applied Date"] = obj.AppliedDate.ToString("dd-MMM-yyyy");
        r["Issue Date"] = obj.IssueDate.ToString("dd-MMM-yyyy");
        r["Effective Date"] = obj.EffectiveDate.ToString("dd-MMM-yyyy");
        r["Expiry Date"] = obj.ExpiryDate.ToString("dd-MMM-yyyy");
        r["Maturity Date"] = obj.MaturityDate.ToString("dd-MMM-yyyy");
        r["Cover Period (M)"] = obj.LoanPeriod;
        r["First Policy?"] = obj.IsFirstPolicy == true ? "Y" : "N";
        r["Policy Status Remarks"] = obj.PolicyStatusRemarks;
        r["Premium"] = obj.Premium;
        r["SumAssure"] = obj.SumAssure;
        r["SystemRemarks"] = systemRemarks;
        MyDigitalLoanInvalidTable.Rows.Add(r);
    }

    void LoadData(string createdBy, DateTime createdOn)
    {
        List<bl_loan_upload> loanList = new List<bl_loan_upload>();
        List<bl_loan_upload> loanListCoverable = new List<bl_loan_upload>();
        List<bl_money_transfer_upload> tranList = new List<bl_money_transfer_upload>();
        List<bl_wing_payroll_upload> wingList = new List<bl_wing_payroll_upload>();
        List<bl_wing_digital_loan_upload> digitalLoanList = new List<bl_wing_digital_loan_upload>();

        DataTable tblValid = new DataTable();
        DataTable tblInvalid = new DataTable();

        Channel_Item_Config chObj = new Channel_Item_Config();
        Channel_Item_Config chConfig = chObj.GetChannelItemConfig(ddlChannelItem.SelectedValue, MyProductConfig.Product_ID);

        if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString())
        {
            loanList = da_micro_group_loan_upload.GetLoanList(createdBy, createdOn);
            #region Calculation coverable year
            int custAge = 0;
            int lastAge = 0;// customer current age + loan period
            int coverableYear = 0;
            int unCoverYear = 0;


            DateTime effectiveDate;


            foreach (bl_loan_upload loan in loanList)
            {
                coverableYear = 0;

                for (int i = 0; i < loan.LoanPeriod; i++)
                {
                    effectiveDate = loan.DisbursementDate.AddYears(i);
                    custAge = Calculation.Culculate_Customer_Age(loan.DOB.ToString("dd/MM/yyyy"), effectiveDate);


                    if (custAge >= MyProductConfig.Age_Min && custAge <= MyProductConfig.Age_Max)
                    {
                        coverableYear += 1;
                    }
                    //if (unCoverYear <= 0) // full cover
                    //{
                    //    coverableYear = loan.LoanPeriod;
                    //}
                    //else
                    //{
                    //    coverableYear = loan.LoanPeriod - unCoverYear;
                    //}


                }

                loan.CoverableYear = coverableYear;
                loanListCoverable.Add(loan);


            }
            Session["SS_LOAN_LIST_OBJECT"] = loanListCoverable;
            #endregion Calculation coverable year
            tblValid = Convertor.ToDataTable(loanListCoverable);
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString())
        {
            tranList = da_micro_group_loan_upload.GetMoneyTransferList(createdBy, createdOn);
            Session["SS_LOAN_LIST_OBJECT"] = tranList;
            tblValid = Convertor.ToDataTable(tranList);
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString())
        {
            wingList = da_micro_group_loan_upload.GetWingPayrollList(createdBy, createdOn);
            Session["SS_LOAN_LIST_OBJECT"] = wingList;
            tblValid = Convertor.ToDataTable(wingList);
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString())
        {

            digitalLoanList = da_micro_group_loan_upload.GetWingDigitalLoanlList(createdBy, createdOn);

            List<bl_wing_digital_loan_upload> myLoanList = new List<bl_wing_digital_loan_upload>();
            List<bl_wing_digital_loan_upload> valideList = new List<bl_wing_digital_loan_upload>();
            List<bl_wing_digital_loan_upload> inValideList = new List<bl_wing_digital_loan_upload>();

            DateTime effectiveDate = new DateTime(1900, 1, 1);
            DateTime maturityDate = new DateTime(1900, 1, 1);
            DateTime expiryDate = new DateTime(1900, 1, 1);
            int polCountLoopStep = 1;

            FirstPolicyList = null;
            FirstPolicyList = da_micro_group_loan_upload.GroupFirstPolicyList;

            ///Get count inforce policy
            MyPolicyInforce = da_micro_group_loan_upload.PolicyInforce;

            /*existing customer*/
            ExistingCustomerList=null;
            ExistingCustomerList = da_micro_group_loan_upload.ExistingCustomerList;

            int policyYear = 0;
            double totalInforcePolicy = 0;
            DataRow rowPolicyInforce;

            #region Loop through digital loan list
            foreach (var obj in digitalLoanList)
            {

                /*add new first policy*/
                if (!IsFirstPolicy(obj.IdType, obj.IdNumber, PrivateGroupMasterProduct.GroupCode))
                {
                    FirstPolicyList.Add(new da_micro_group_loan_upload.FirstPolicyList()
                    {
                        IdNumber = obj.IdNumber,
                        IdType = obj.IdType,
                        GroupCode = PrivateGroupMasterProduct.GroupCode,
                        IsFirstPolicy = true
                    });
                    obj.IsFirstPolicy = true;
                }
                else
                {
                    obj.IsFirstPolicy = false;
                }

                #region Loop count policy

                foreach (Int32[] polArry in CountPolicies(obj.LoanPeriod,Helper.LoanPeriodType.D))
                {
                    foreach (Int32 pol in polArry)
                    {

                        policyYear += 1;

                        /*assign new value*/
                        
                        //effectiveDate = policyYear == 1 ? obj.AppliedDate : maturityDate.AddMonths(pol);
                        //maturityDate = effectiveDate.AddMonths(pol);
                        //expiryDate = maturityDate.AddDays(-1);
                        if (policyYear == 1)
                        {
                            effectiveDate = obj.AppliedDate;
                        }
                        else
                        {
                            if (obj.LoanPeriodType == Helper.LoanPeriodType.D.ToString())
                            {
                                effectiveDate = maturityDate.AddDays(pol);
                            }
                            else if (obj.LoanPeriodType == Helper.LoanPeriodType.M.ToString())
                            {
                                effectiveDate = maturityDate.AddMonths(pol);
                            }
                        }

                        if (obj.LoanPeriodType == Helper.LoanPeriodType.D.ToString())
                        {
                            maturityDate = effectiveDate.AddDays(pol) ;
                        }
                        else if (obj.LoanPeriodType == Helper.LoanPeriodType.M.ToString())
                        {
                            maturityDate = effectiveDate.AddMonths(pol);
                           
                        }
                        maturityDate = maturityDate.AddDays(1);
                        expiryDate = maturityDate.AddDays(-1);

                        var obj1 = new bl_wing_digital_loan_upload()
                        {
                             Seq= obj.Seq,
                            AccountNumber = obj.AccountNumber,
                            ClientName = obj.ClientName,
                            Gender = obj.Gender,
                            DOB = obj.DOB,
                            AppliedDate = obj.AppliedDate,
                            MaturityDate = maturityDate,
                            ChannelId = obj.ChannelId,
                            ChannelItemId = obj.ChannelItemId,
                            ChannelLocationId = obj.ChannelLocationId,
                            ID = obj.ID,
                            IdType = obj.IdType,
                            Province = obj.Province,
                            District = obj.District,
                            Commune = obj.Commune,
                            Village = obj.Village,
                            Address = obj.Address,
                            PhoneNumber = obj.PhoneNumber,
                            BundleName = obj.BundleName,
                            Occupation = obj.Occupation,
                            CreatedBy = obj.CreatedBy,
                            CreatedOn = obj.CreatedOn,
                            BeneficiaryName = obj.BeneficiaryName,
                            Relation = obj.Relation,
                            Remarks = obj.Remarks,
                            LoanPeriodType = obj.LoanPeriodType,
                            IdNumber = obj.IdNumber,
                            LoanPeriod = pol,
                            EffectiveDate = effectiveDate,
                            ExpiryDate = expiryDate,
                            IssueDate = obj.AppliedDate,
                            IsFirstPolicy = obj.IsFirstPolicy
                        };

                        /*calculate premium*/
                        int first3Month = 3 ;
                        int remainMonth = 0;
                        
                        if (obj.LoanPeriodType == Helper.LoanPeriodType.D.ToString())
                        {
                         
                            remainMonth = (obj1.LoanPeriod/30) - first3Month;
                        }
                        else if (obj.LoanPeriodType == Helper.LoanPeriodType.M.ToString())
                        {
                          
                            remainMonth = obj1.LoanPeriod - first3Month;
                        }

                        if (policyYear == 1)/*is the first year policy*/
                        {
                            obj1.PolicyStatusRemarks = Helper.PolicyStatusRemarks.New.ToString();
                            if (obj1.IsFirstPolicy)
                            {
                                obj1.Premium = remainMonth <= 0 ? 0.1 : 0.1 + (0.1 * remainMonth);
                                obj1.SumAssure = Convert.ToDouble(ddlBasicSumAssure.SelectedValue); //2000;
                            }
                            else
                            {
                                obj1.Premium = remainMonth <= 0 ? 0.08 : 0.08 + (0.08 * remainMonth);
                                obj1.SumAssure = Convert.ToDouble(ddlBasicSumAssure.SelectedValue);
                            }
                        }
                        else
                        {
                            obj1.PolicyStatusRemarks = Helper.PolicyStatusRemarks.Repayment.ToString();
                            if (obj1.IsFirstPolicy)
                            {
                                obj1.Premium = remainMonth <= 0 ? 0.1 : 0.1 + (0.1 * remainMonth);
                                obj1.SumAssure = Convert.ToDouble(ddlBasicSumAssure.SelectedValue); //2000;
                            }
                            else
                            {

                                obj1.Premium = 0.08 * obj1.LoanPeriod;
                                obj1.SumAssure = Convert.ToDouble(ddlBasicSumAssure.SelectedValue);
                            }
                        }

                        #region count total inforce policy   /*save policy inforce*/

                        rowPolicyInforce = MyPolicyInforce.NewRow();
                        rowPolicyInforce["year"] = obj1.EffectiveDate.Year;
                        rowPolicyInforce["id_type"] = obj1.IdType;
                        rowPolicyInforce["id_number"] = obj1.IdNumber;
                        rowPolicyInforce["full_name_en"] = obj1.LastName + " " + obj1.FirstName;
                        rowPolicyInforce["policy_count"] = 1;
                        rowPolicyInforce["total_sum_assure"] = obj1.SumAssure;
                        MyPolicyInforce.Rows.Add(rowPolicyInforce);

                        /*count policies inforce*/
                        totalInforcePolicy = GetTotalNumberPolicyInforce(obj.IdType + "", obj.IdNumber, obj1.EffectiveDate.Year)[0];

                        if (totalInforcePolicy <= chConfig.MaxPolicyPerLife)
                        {
                            valideList.Add(obj1);
                        }
                        else // max policy is reach
                        {
                            /*invalid data*/
                            //  AddInvalidRecord(obj.AccountNumber, obj.LastName, obj.FirstName, obj.GenderText, obj.IdTypeText, obj.IdNumber, obj.DOB.ToString("dd-MM-yyyy"), obj.PhoneNumber, obj.Province, obj.District, obj.Commune, obj.Village, obj.AppliedDate.ToString("dd-MM-yyyy"), obj1.MaturityDate.ToString("dd-MM-yyyy"), obj.Remarks, obj.Occupation, obj.BeneficiaryName, obj.Relation, "Max policies Inforce [" + totalInforcePolicy + "] is reach.");
                            AddInvalidRecord(obj1, "Max policies Inforce [ " + totalInforcePolicy + " ] is exceed [ " + chConfig.MaxPolicyPerLife + " ].");

                        }

                        #endregion count total inforce policy

                    }/*loop new and repayment policy*/

                    /*reset variable*/
                    policyYear = 0;
                    countPolicyList = new List<Int32[]>();
                    countYear = new List<Int32>();
                    polCountLoopStep += 1;
                }/*loop count policy*/

                /*reset count policy*/
                polCountLoopStep = 1;
                effectiveDate = new DateTime(1900, 1, 1);
                maturityDate = new DateTime(1900, 1, 1);
                #endregion Loop count policy
            }

            #endregion Loop through digital loan list

            /*clear first policy list*/
            FirstPolicyList = new List<da_micro_group_loan_upload.FirstPolicyList>();

            Session["SS_LOAN_LIST_OBJECT"] = valideList;
            tblValid = Convertor.ToDataTable(valideList);

            Session["SS_DATA_INVALID"] = MyDigitalLoanInvalidTable;
        }

        if (Session["SS_DATA_INVALID"] != null)
        {
            tblInvalid = (DataTable)Session["SS_DATA_INVALID"];
            MyInvalideRecord = tblInvalid.Rows.Count;
        }

        MyValideRecord = tblValid.Rows.Count;
        Session["SS_DATA"] = tblValid;
        string sortEx = "";
        string sortCol = "";
        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tblValid);

        #region display data in gridview
        if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString())
        {
            gv_valid.Visible = true;
            gv_valid.DataSource = dview;
            gv_valid.DataBind();

            gvMoneyTransfer.DataSource = null;
            gvMoneyTransfer.DataBind();
            gvMoneyTransfer.Visible = false;

            gv_digital_loan.Visible = false;
            gv_digital_loan.DataSource = null;
            gv_digital_loan.DataBind();

            gvPayroll.DataSource = null;
            gvPayroll.DataBind();
            gvPayroll.Visible = false;

            lblRecords.Text = "Uploaded Record(s): " + gv_valid.Rows.Count + " of " + tblValid.Rows.Count;
            gv_valid.Visible = ckbShow.Checked;
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString())
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            gv_valid.Visible = false;

            gvPayroll.DataSource = null;
            gvPayroll.DataBind();
            gvPayroll.Visible = false;

            gv_digital_loan.Visible = false;
            gv_digital_loan.DataSource = null;
            gv_digital_loan.DataBind();

            gvMoneyTransfer.Visible = true;
            gvMoneyTransfer.DataSource = dview;
            gvMoneyTransfer.DataBind();
            lblRecords.Text = "Uploaded Record(s): " + gvMoneyTransfer.Rows.Count + " of " + tblValid.Rows.Count;
            gvMoneyTransfer.Visible = ckbShow.Checked;
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString())
        {
            gvMoneyTransfer.DataSource = null;
            gvMoneyTransfer.DataBind();
            gvMoneyTransfer.Visible = false;

            gv_valid.DataSource = null;
            gv_valid.DataBind();
            gv_valid.Visible = false;

            gv_digital_loan.Visible = false;
            gv_digital_loan.DataSource = null;
            gv_digital_loan.DataBind();

            gvPayroll.Visible = true;
            gvPayroll.DataSource = dview;
            gvPayroll.DataBind();
            lblRecords.Text = "Uploaded Record(s): " + gvPayroll.Rows.Count + " of " + tblValid.Rows.Count;
            gvPayroll.Visible = ckbShow.Checked;

        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString())
        {
            gvMoneyTransfer.DataSource = null;
            gvMoneyTransfer.DataBind();
            gvMoneyTransfer.Visible = false;

            gv_valid.DataSource = null;
            gv_valid.DataBind();
            gv_valid.Visible = false;

            gvPayroll.Visible = false;
            gvPayroll.DataSource = null;
            gvPayroll.DataBind();

            gv_digital_loan.DataSource = dview;
            gv_digital_loan.DataBind();
            lblRecords.Text = "Uploaded Record(s): " + gv_digital_loan.Rows.Count + " of " + tblValid.Rows.Count;
            gv_digital_loan.Visible = ckbShow.Checked;
        }

        #endregion display data in gridview

        Session["SS_DATA"] = dview.ToTable();

        if (tblValid.Rows.Count > 0)
        {
            btnSave.Enabled = true;
            // btnUpload.Enabled = false;
            ckbShow.Enabled = true;

        }
        else
        {
            btnSave.Enabled = false;
            // btnUpload.Enabled = true;
            ckbShow.Enabled = false;
        }


        gvInvalid.DataSource = (DataTable)Session["SS_DATA_INVALID"];
        gvInvalid.DataBind();

        if (MyInvalideRecord > 0 && MyValideRecord == 0)
        {
            Helper.Alert(true, "Imported fail, please check data in [Invalide TAB].", lblError);
        }
        else if (MyValideRecord > 0 && MyInvalideRecord == 0)
        {
            Helper.Alert(false, "Imported successfully.", lblError);
        }
        else if (MyValideRecord == 0 && MyInvalideRecord == 0)
        {
            Helper.Alert(true, "Imported Data Not Found.", lblError);
        }
        else
        {
            Helper.Alert(true, "Imported some data successfully and other fail. please check data in [Valide TAB or Invalide TAB].", lblError);
        }

        btnExportInvalide.Visible = MyInvalideRecord > 0 ? true : false;
        lblInvalideText.Visible = MyInvalideRecord > 0 ? false : true;
    }

    bool ValidateForm()
    {
        bool check = true;
        if (ddlChannel.SelectedValue == "")
        {
            MyMessage = "Channel is required.";
            check = false;
        }
        else if (ddlChannelItem.SelectedValue == "")
        {
            MyMessage = "Company is required.";
            return false;
        }
        else if (ddlChannelLocation.SelectedValue == "")
        {
            MyMessage = "Branch is required.";
            check = false;
        }
        else if (ddlProduct.SelectedValue == "")
        {
            MyMessage = "Product is required.";
            check = false;
        }
        else if (ddlPaymentMode.SelectedValue == "")
        {
            MyMessage = "Payment Code is required.";
            check = false;
        }
        else if (ddlBasicSumAssure.SelectedValue == "" || ddlBasicSumAssure.SelectedValue == "0")
        {
            MyMessage = "Sum Assure is required";
            check = false;
        }
        else if (!string.IsNullOrWhiteSpace(MyProductConfig.RiderProductID))
        {
            if (MyProductConfig.IsRequiredRider)
            {
                if (ddlProductRider.SelectedValue == "")
                {
                    MyMessage = "Rider product is required.";
                    check = false;
                }
                else if (ddlRiderSumAssure.SelectedValue == "" || ddlRiderSumAssure.SelectedValue == "0")
                {
                    MyMessage = "Rider Sum Assure is required.";
                    check = false;
                }
                else
                {
                    MyMessage = "";
                    check = true;
                }
            }
            else
            {
                MyMessage = "";
                check = true;
            }
        }
        else
        {
            MyMessage = "";
            check = true;
        }

        if (check)//if all above validate checking  is true
        {
            if (txtSaleAgentID.Text.Trim() == "")
            {
                MyMessage = "Sale Agent Code is required.";
                check = false;
            }
            else if (AgentInfo == null)
            {
                MyMessage = "Sale Agent Code <strong>" + txtSaleAgentID.Text + "</strong> is not exist in system.";
                check = false;
            }
            else if (!Helper.IsDate(txtPaymentReportDate.Text))
            {
                MyMessage = "Payment Report Date is required with format [DD/MM/YYYY].";
                check = false;
            }
            else
            {
                check = true;
            }
        }
        return check;
    }

    void validateFile()
    {
        ViewState["VS_VAL_MESSAGE"] = "";
        ViewState["VS_VAL_ISVALID"] = true;
        Session["SS_DATA_UPLOAD"] = null;
        Session["SS_DATA_INVALID"] = null;

        bool isValid = true;
        string message = "";

        if (ddlChannel.SelectedIndex == 0)
        {
            isValid = false;
            message = "Please select Channel.";
        }
        else if (ddlChannelItem.SelectedIndex == 0)
        {

            isValid = false;
            message = "Please select Company.";
        }
        else if ((fUpload.PostedFile != null) && !string.IsNullOrEmpty(fUpload.PostedFile.FileName))
        {

            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(fUpload.PostedFile.FileName);
            //string extension = Path.GetExtension(file_name);
            MyFileExtention = Path.GetExtension(file_name);
            file_name = file_name.Replace(MyFileExtention, DateTime.Now.ToString("yyyymmddhhmmss") + MyFileExtention);
            string file_path = "";

            Channel_Item_Config chObj = new Channel_Item_Config();
            Channel_Item_Config chConfig = chObj.GetChannelItemConfig(ddlChannelItem.SelectedValue, MyProductConfig.Product_ID);
            double[] ifPolicy = new double[] { 0, 0 };

            if (MyFileExtention.Trim().ToLower() == ".xls" || MyFileExtention.Trim().ToLower() == ".xlsx")
            {
                file_path = save_path + file_name;
                fUpload.SaveAs(Server.MapPath(file_path));//save file 

                MyTempFilePath = Server.MapPath(file_path);

                //save file in server
                string subFolder = "BundleFiles";
                string newPath = documents.TrascationFiles.CreateSubFolder(subFolder);
                string newFileName = (PrivateGroupMasterProduct.GroupCode + "_" + PrivateGroupMasterProduct.ProductID).Replace("-", "_") + "_" + DateTime.Now.ToString("yyMMddHHmm") + MyFileExtention;
                newPath += newFileName;
                MySubFolderFilePath = subFolder + "/" + newFileName;
                MyFilePath = newPath;//save file path

                DataTable myData = new DataTable();
                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "select * from [Data$]";

                if (my_excel.GetSheetName().ToLower() == "data$")
                {
                    myData = my_excel.GetData();
                    if (my_excel.Status)
                    {
                        Session["SS_DATA_UPLOAD"] = myData;
                        int seq= 0;
                        string submmitedDate = "";
                        string branch = "";
                        string clientName = "";
                        string clientGender = "";
                        string dob = "";
                        string idType = "";
                        string idNumber = "";
                        string occupation = "";
                        string address = "";
                        string contactNumber = "";
                        string currency = "";
                        string exchangeRate = "";
                        string loanAmount = "";
                        string DisburseDate = "";
                        string insuranceCost = "";
                        string loanPeriod = "";
                        string benName = "";
                        string relation = "";
                        int index = -1;
                        int indexInvalid = -1;

                        string accountNumber = "";
                        string province = "";
                        string district = "";
                        string commune = "";
                        string village = "";
                        string maturityDate = "";

                        /*check upload project type*/
                        #region LOAN
                        if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString())
                        {
                            if (myData.Columns.Count == 19)
                            {
                                if (myData.Rows.Count > 0)
                                {
                                    DataTable tblValid = myData.Clone();
                                    DataTable tblInvalid = myData.Clone();
                                    tblInvalid.Columns.Add("System_Remarks");

                                    string err = "";
                                    try
                                    {
                                        foreach (DataRow r in myData.Rows)
                                        {
                                            submmitedDate = r[0].ToString().Trim();
                                            branch = r[1].ToString();
                                            clientName = r[2].ToString().Trim();
                                            clientGender = r[3].ToString().Trim();
                                            dob = r[4].ToString().Trim();
                                            idType = r[5].ToString().Trim();
                                            idNumber = r[6].ToString().Trim();
                                            occupation = r[8].ToString().Trim();
                                            address = r[9].ToString().Trim();
                                            contactNumber = r[10].ToString().Trim();
                                            currency = r[11].ToString().Trim();
                                            exchangeRate = r[12].ToString().Trim();
                                            loanAmount = r[13].ToString().Trim();
                                            DisburseDate = r[14].ToString().Trim();
                                            loanPeriod = r[15].ToString().Trim();
                                            insuranceCost = r[16].ToString().Trim();
                                            benName = r[17].ToString().Trim();
                                            relation = r[18].ToString().Trim();

                                            index = myData.Rows.IndexOf(r);// +1;
                                            if (branch == "")
                                            {
                                                err = "[Branch can not be blank.]";
                                            }
                                            else if (clientName == "")
                                            {
                                                err = "[Client Name can not be blank.]";
                                            }
                                            else if (clientGender == "")
                                            {
                                                err = "[Gender cannot be blank.]";
                                            }
                                            else if (!Helper.IsDate(dob))
                                            {
                                                err = "[DOB must be in format DD-MM-YYYY]";
                                            }
                                            else if (idType == "")
                                            {
                                                err = "[ID Type cannot be blank]";
                                            }
                                            else if (idNumber == "")
                                            {
                                                err = "[ID Number cannot be blank]";
                                            }
                                            else if (occupation == "")
                                            {
                                                err = "[Occupation cannot be blank]";
                                            }
                                            else if (address == "")
                                            {
                                                err = "[Address cannot be blank]";
                                            }
                                            else if (contactNumber == "")
                                            {
                                                err = "[Contact Number cannot be blank]";

                                            }
                                            else if (currency.ToUpper() != "KHR" && currency.ToUpper() != "USD")
                                            {
                                                err = "[Currency must be KHR or USD]";
                                            }
                                            else if (!Helper.IsAmount(exchangeRate))
                                            {
                                                err = "[Exchange Rate must be number]";
                                            }
                                            else if (!Helper.IsAmount(loanAmount))
                                            {
                                                err = "[Loan Amount must be number]";
                                            }
                                            else if (!Helper.IsDate(DisburseDate))
                                            {
                                                err = "[Disbursement Date must be in format DD-MM-YYYY]";
                                            }
                                            else if (!Helper.IsNumber(loanPeriod))
                                            {
                                                err = "[Loan peroid is required as number.]";
                                            }
                                            else if (loanPeriod == "0")
                                            {
                                                err = "[Loan peroid is required.]";
                                            }
                                            else
                                            {
                                                int age = Calculation.Culculate_Customer_Age_Micro(Helper.FormatDateTime(dob), Helper.FormatDateTime(submmitedDate));
                                                if (age > MyProductConfig.Age_Max || age < MyProductConfig.Age_Min)
                                                {
                                                    err = "Age [" + age + "] is not in range.";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }

                                            }

                                            //CHECK MAX EFFECTIVE POLICY;
                                            int countPolicy = 0;
                                            ifPolicy = da_micro_group_policy.GetPolicyEffective(Helper.GetIDCardTypeID(idType.Replace(" ", "_")), idNumber, Helper.FormatDateTime(DisburseDate).Year);
                                            countPolicy = CountPolicy(myData, idType, idNumber, Helper.FormatDateTime(DisburseDate).Year, da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN);

                                            if (err == "")/* nothing error checking above*/
                                            {
                                                if (ifPolicy[0] + countPolicy > chConfig.MaxPolicyPerLife)
                                                {
                                                    err = "Maximum In-force policy is exceed. [issued policies count =" + ifPolicy[0] + "] [new policies =" + countPolicy + "]";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }
                                            }

                                            if (err != "")
                                            {
                                                indexInvalid += 1;
                                                tblInvalid.ImportRow(r);
                                                tblInvalid.Rows[indexInvalid]["System_Remarks"] = err;

                                            }
                                            else
                                            {
                                                tblValid.ImportRow(r);
                                            }
                                            err = "";
                                        }

                                        Session["SS_DATA_INVALID"] = tblInvalid;
                                        Session["SS_DATA_VALID"] = tblValid;
                                    }
                                    catch (Exception ex)
                                    {
                                        isValid = false;
                                        message = "[validateFile()], Unexpected Error: " + ex.Message;
                                    }
                                }
                                else// no row 
                                {
                                    isValid = false;
                                    message = "No record to upload.";
                                }
                            }
                            else//invalid column numbers
                            {

                                isValid = false;
                                message = "Please check your file,[" + da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString() + "] make sure total columns number = 19.";

                            }
                        }
                        #endregion
                        #region MONEY TRANSFER
                        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString())
                        {
                            if (myData.Columns.Count == 14)
                            {
                                if (myData.Rows.Count > 0)
                                {
                                    DataTable tblValid = myData.Clone();
                                    DataTable tblInvalid = myData.Clone();
                                    tblInvalid.Columns.Add("System_Remarks");

                                    string err = "";
                                    try
                                    {
                                        foreach (DataRow r in myData.Rows)
                                        {
                                            submmitedDate = r[0].ToString().Trim();
                                            //branch = r[1].ToString();
                                            clientName = r[1].ToString().Trim();
                                            clientGender = r[2].ToString().Trim();
                                            dob = r[3].ToString().Trim();
                                            idType = r[4].ToString().Trim();
                                            idNumber = r[5].ToString().Trim();
                                            accountNumber = r[6].ToString().Trim();
                                            //address = r[9].ToString().Trim();
                                            contactNumber = r[7].ToString().Trim();
                                            currency = r[8].ToString().Trim();
                                            exchangeRate = r[9].ToString().Trim();
                                            loanAmount = r[10].ToString().Trim();/*sum assured*/
                                            DisburseDate = r[11].ToString().Trim();/*effective date*/
                                            //loanPeriod = r[15].ToString().Trim();
                                            insuranceCost = r[12].ToString().Trim();/*premium*/
                                            //benName = r[17].ToString().Trim();
                                            //relation = r[18].ToString().Trim();

                                            index = myData.Rows.IndexOf(r);
                                            if (clientName == "")
                                            {
                                                err = "[Client Name can not be blank.]";
                                            }
                                            else if (clientGender == "")
                                            {
                                                err = "[Gender cannot be blank.]";
                                            }
                                            else if (!Helper.IsDate(dob))
                                            {
                                                err = "[DOB must be in format DD-MM-YYYY]";
                                            }
                                            else if (idType == "")
                                            {
                                                err = "[ID Type cannot be blank]";
                                            }
                                            else if (idNumber == "")
                                            {
                                                err = "[ID Number cannot be blank]";
                                            }
                                            //else if (accountNumber == "")
                                            //{
                                            //    err = "[Account Number cannot be blank]";
                                            //}
                                            else if (contactNumber == "")
                                            {
                                                err = "[Contact Number cannot be blank]";

                                            }
                                            else if (currency.ToUpper() != "KHR" && currency.ToUpper() != "USD")
                                            {
                                                err = "[Currency must be KHR or USD]";
                                            }
                                            else if (!Helper.IsAmount(exchangeRate))
                                            {
                                                err = "[Exchange Rate must be number]";
                                            }
                                            else if (!Helper.IsAmount(loanAmount))
                                            {
                                                err = "[Sum Assued must be number]";
                                            }
                                            else if (!Helper.IsDate(DisburseDate))
                                            {
                                                err = "[Effective Date must be in format DD-MM-YYYY]";
                                            }
                                            else if (!Helper.IsAmount(insuranceCost))
                                            {
                                                err = "[Premium must be number]";
                                            }
                                            else
                                            {
                                                int age = Calculation.Culculate_Customer_Age_Micro(Helper.FormatDateTime(dob), Helper.FormatDateTime(submmitedDate));
                                                if (age > MyProductConfig.Age_Max || age < MyProductConfig.Age_Min)
                                                {
                                                    err = "Age [" + age + "] is not in range.";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }

                                            }

                                            //CHECK MAX EFFECTIVE POLICY;
                                            int countPolicy = 0;
                                            ifPolicy = da_micro_group_policy.GetPolicyEffective(Helper.GetIDCardTypeID(idType.Replace(" ", "_")), idNumber, Helper.FormatDateTime(DisburseDate).Year);
                                            countPolicy = CountPolicy(myData, idType, idNumber, Helper.FormatDateTime(DisburseDate).Year, da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER);

                                            if (err == "")/* nothing error checking above*/
                                            {
                                                if (ifPolicy[0] + countPolicy > chConfig.MaxPolicyPerLife)
                                                {
                                                    err = "Maximum In-force policy is exceed. [issued policies count =" + ifPolicy[0] + "] [new policies =" + countPolicy + "]";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }
                                            }

                                            if (err != "")
                                            {
                                                indexInvalid += 1;
                                                tblInvalid.ImportRow(r);
                                                tblInvalid.Rows[indexInvalid]["System_Remarks"] = err;

                                            }
                                            else
                                            {
                                                tblValid.ImportRow(r);
                                            }
                                            err = "";
                                        }

                                        Session["SS_DATA_INVALID"] = tblInvalid;
                                        Session["SS_DATA_VALID"] = tblValid;
                                    }
                                    catch (Exception ex)
                                    {
                                        isValid = false;
                                        message = "[validateFile()], Unexpected Error: " + ex.Message;
                                    }
                                }
                                else// no row 
                                {
                                    isValid = false;
                                    message = "No record to upload.";
                                }
                            }
                            else//invalid column numbers
                            {

                                isValid = false;
                                message = "Please check your file,[" + da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString() + "] make sure total columns number = 14.";

                            }
                        }
                        #endregion Money Transfer
                        #region wing payroll
                        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString())
                        {
                            string firstName = "";
                            string lastName = "";
                            string subCompanyName = "";

                            if (myData.Columns.Count == 12)
                            {
                                if (myData.Rows.Count > 0)
                                {
                                    DataTable tblValid = myData.Clone();
                                    DataTable tblInvalid = myData.Clone();
                                    tblInvalid.Columns.Add("System_Remarks");

                                    string err = "";
                                    try
                                    {
                                        foreach (DataRow r in myData.Rows)
                                        {

                                            submmitedDate = r[0].ToString().Trim(); /*issue date*/
                                            lastName = r[1].ToString().Trim();
                                            firstName = r[2].ToString().Trim();
                                            // clientName = string.Concat(lastName, " ", firstName);
                                            clientGender = r[3].ToString().Trim();
                                            idType = r[4].ToString().Trim();
                                            idNumber = r[5].ToString().Trim();
                                            dob = r[6].ToString().Trim();
                                            contactNumber = r[7].ToString().Trim();
                                            loanAmount = r[8].ToString().Trim();/*sum assured*/
                                            insuranceCost = r[9].ToString().Trim();/*premium*/
                                            DisburseDate = r[10].ToString().Trim();/*effective date*/
                                            subCompanyName = r[11].ToString();

                                            index = myData.Rows.IndexOf(r);
                                            if (firstName == "")
                                            {
                                                err = "[First Name can not be blank.]";
                                            }
                                            else if (lastName == "")
                                            {
                                                err = "[Last Name can not be blank.]";
                                            }
                                            else if (clientGender == "")
                                            {
                                                err = "[Gender cannot be blank.]";
                                            }
                                            else if (!Helper.IsDate(dob))
                                            {
                                                err = "[DOB must be in format DD-MM-YYYY]";
                                            }
                                            else if (idType == "")
                                            {
                                                err = "[ID Type cannot be blank]";
                                            }
                                            else if (idNumber == "")
                                            {
                                                err = "[ID Number cannot be blank]";
                                            }

                                            else if (contactNumber == "")
                                            {
                                                err = "[Contact Number cannot be blank]";

                                            }
                                            //else if (currency.ToUpper() != "KHR" && currency.ToUpper() != "USD")
                                            //{
                                            //    err = "[Currency must be KHR or USD]";
                                            //}
                                            //else if (!Helper.IsAmount(exchangeRate))
                                            //{
                                            //    err = "[Exchange Rate must be number]";
                                            //}
                                            else if (!Helper.IsAmount(loanAmount))
                                            {
                                                err = "[Sum Assued must be number]";
                                            }
                                            else if (!Helper.IsDate(DisburseDate))
                                            {
                                                err = "[Effective Date must be in format DD-MM-YYYY]";
                                            }
                                            else if (!Helper.IsAmount(insuranceCost))
                                            {
                                                err = "[Premium must be number]";
                                            }
                                            else
                                            {
                                                int age = Calculation.Culculate_Customer_Age_Micro(Helper.FormatDateTime(dob), Helper.FormatDateTime(submmitedDate));
                                                if (age > MyProductConfig.Age_Max || age < MyProductConfig.Age_Min)
                                                {
                                                    err = "Age [" + age + "] is not in range.";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }

                                            }

                                            //CHECK MAX EFFECTIVE POLICY;
                                            int countPolicy = 0;
                                            ifPolicy = da_micro_group_policy.GetPolicyEffective(Helper.GetIDCardTypeID(idType.Replace(" ", "_")), idNumber, Helper.FormatDateTime(DisburseDate).Year);
                                            countPolicy = CountPolicy(myData, idType, idNumber, Helper.FormatDateTime(DisburseDate).Year, da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL);

                                            if (err == "")/* nothing error checking above*/
                                            {
                                                if (ifPolicy[0] + countPolicy > chConfig.MaxPolicyPerLife)
                                                {
                                                    err = "Maximum In-force policy is exceed. [issued policies count =" + ifPolicy[0] + "] [new policies =" + countPolicy + "]";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }
                                            }

                                            if (err != "")
                                            {
                                                indexInvalid += 1;
                                                tblInvalid.ImportRow(r);
                                                tblInvalid.Rows[indexInvalid]["System_Remarks"] = err;

                                            }
                                            else
                                            {
                                                tblValid.ImportRow(r);
                                            }
                                            err = "";
                                        }

                                        Session["SS_DATA_INVALID"] = tblInvalid;
                                        Session["SS_DATA_VALID"] = tblValid;
                                    }
                                    catch (Exception ex)
                                    {
                                        isValid = false;
                                        message = "[validateFile()], Unexpected Error: " + ex.Message;
                                    }
                                }
                                else// no row 
                                {
                                    isValid = false;
                                    message = "No record to upload.";
                                }
                            }
                            else//invalid column numbers
                            {

                                isValid = false;
                                message = "Please check your file,[" + da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString() + "] make sure total columns number = 12.";

                            }
                        }
                        #endregion wing payroll
                        #region wing digital loan
                        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString())
                        {
                            if (myData.Columns.Count == 19)
                            {
                                if (myData.Rows.Count > 0)
                                {
                                    DataTable tblValid = myData.Clone();
                                    DataTable tblInvalid = myData.Clone();
                                    tblInvalid.Columns.Add("System_Remarks");

                                    string err = "";
                                    try
                                    {
                                        foreach (DataRow r in myData.Rows)
                                        {
                                            seq   = Convert.ToInt32( r[0].ToString());
                                            accountNumber = r[1].ToString();
                                            clientName = string.Concat(r[2].ToString().Trim(), " ", r[3].ToString().Trim());
                                            clientGender = r[4].ToString().Trim();
                                            idType = r[5].ToString().Trim();
                                            idNumber = r[6].ToString().Trim();
                                            dob = r[7].ToString().Trim();
                                            contactNumber = r[8].ToString().Trim();
                                            province = r[9].ToString().Trim();
                                            district = r[10].ToString().Trim();
                                            commune = r[11].ToString().Trim();
                                            village = r[12].ToString().Trim();
                                            submmitedDate = r[13].ToString().Trim();
                                            maturityDate = r[14].ToString().Trim();
                                            occupation = r[16].ToString().Trim();
                                            benName = r[17].ToString().Trim();
                                            relation = r[18].ToString().Trim();
                                            address = string.Concat(village, " ", commune, " ", district, " ", province);

                                            index = myData.Rows.IndexOf(r);// +1;
                                            if (clientName == "")
                                            {
                                                err = "[" + seq + "] [Client Name can not be blank.]";
                                            }
                                            else if (clientGender == "")
                                            {
                                                err = "[" + clientGender + "] [Gender cannot be blank.]";
                                            }
                                            else if (!Helper.IsDate(dob))
                                            {
                                                err = "[" + dob + "] [DOB must be in format DD-MM-YYYY]";
                                            }
                                            else if (idType == "")
                                            {
                                                err = "[" + seq + "] [ID Type cannot be blank]";
                                            }
                                            else if (idNumber == "")
                                            {
                                                err = "[" + seq + "] [ID Number cannot be blank]";
                                            }
                                            //else if (occupation == "")
                                            //{
                                            //    err = "[Occupation cannot be blank]";
                                            //}
                                            else if (address == "")
                                            {
                                                err = "[" + seq + "] [Address cannot be blank]";
                                            }
                                            else if (contactNumber == "")
                                            {
                                                err = "[" + seq + "] [Contact Number cannot be blank]";

                                            }
                                            else
                                            {
                                                int age = Calculation.Culculate_Customer_Age_Micro(Helper.FormatDateTime(dob), Helper.FormatDateTime(submmitedDate));
                                                if (age > MyProductConfig.Age_Max || age < MyProductConfig.Age_Min)
                                                {
                                                    err = "[" + seq + "] Age [" + age + "] is not in range.";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }

                                            }

                                            //CHECK MAX EFFECTIVE POLICY;
                                            int countPolicy = 0;
                                            //   ifPolicy = da_micro_group_policy.GetPolicyEffective(Helper.GetIDCardTypeID(idType.Replace(" ", "_")), idNumber, Helper.FormatDateTime(submmitedDate).Year);
                                            //  countPolicy = CountPolicy(myData, idType, idNumber, Helper.FormatDateTime(submmitedDate).Year, da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN);

                                            if (err == "")/* nothing error checking above*/
                                            {
                                                if (ifPolicy[0] + countPolicy > chConfig.MaxPolicyPerLife)
                                                {
                                                    err = "[" + seq + "] Maximum In-force policy is exceed. [issued policies count =" + ifPolicy[0] + "] [new policies =" + countPolicy + "]";
                                                }
                                                else
                                                {
                                                    err = "";
                                                }
                                            }

                                            if (err != "")
                                            {
                                                indexInvalid += 1;
                                                tblInvalid.ImportRow(r);
                                                tblInvalid.Rows[indexInvalid]["System_Remarks"] = err;

                                            }
                                            else
                                            {
                                                tblValid.ImportRow(r);
                                            }
                                            err = "";
                                        }

                                        Session["SS_DATA_INVALID"] = tblInvalid;
                                        Session["SS_DATA_VALID"] = tblValid;
                                    }
                                    catch (Exception ex)
                                    {
                                        isValid = false;
                                        message = "[validateFile()], Unexpected Error: " + ex.Message;
                                        Log.AddExceptionToLog("Errof function [frm_gmCustomer_upload.aspx.cs=> validateFile()], detail: " + ex.StackTrace);
                                    }
                                }
                                else// no row 
                                {
                                    isValid = false;
                                    message = "No record to upload.";
                                }
                            }
                            else//invalid column numbers
                            {

                                isValid = false;
                                message = "Please check your file,[" + da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString() + "] make sure total columns number =19.";

                            }
                        }
                        #endregion wing degital loan

                    }
                    else //get data from excel error
                    {
                        message = my_excel.Message;
                        isValid = false;
                    }
                }
                else//invalid sheet name
                {
                    message = "Invalid sheet name, please download file template from system.";
                    isValid = false;
                }
            }//invalid file 
            else
            {
                message = "[" + MyFileExtention + "] is not supported.";
                isValid = false;
            }
            //delete file
            //if (saved_file)
            //{
            //    File.Delete(Server.MapPath(file_path));
            //}
        }
        else
        {
            message = "Please select an excel file.";
            isValid = false;
        }
        ViewState["VS_VAL_MESSAGE"] = message;
        ViewState["VS_VAL_ISVALID"] = isValid;

    }

    protected void gv_valid_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable tbl = (DataTable)Session["SS_DATA"];
        if (Session["SORTEX"] + "" == "ASC")
        {
            Session["SORTEX"] = "DESC";
        }
        else
        {
            Session["SORTEX"] = "ASC";
        }
        Session["SORTCOL"] = e.SortExpression;


        string sortEx = "";
        string sortCol = "";
        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_valid.DataSource = dview;
        gv_valid.DataBind();
        lblRecords.Text = "Record(s): " + gv_valid.Rows.Count + " of " + tbl.Rows.Count;
        //Data table after sorting
        Session["SS_DATA"] = dview.ToTable();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString())
        {
            SaveLoan();
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString())
        {
            SaveMoneyTransfer();
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString())
        {
            SaveWingPayroll();
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString())
        {
            SaveWingDigitalLoan();
        }
    }

    protected void SaveLoan()
    {
        bl_micro_customer_prefix pre = new bl_micro_customer_prefix();
        bl_micro_group_policy_prefix polPre = new bl_micro_group_policy_prefix();
        bl_micro_product_config proConf = MyProductConfig;// (bl_micro_product_config)Session["SS_PRODUCT_CONF"];
        bl_micro_product_rate proRate;
        bl_micro_product_discount_config disConf = new bl_micro_product_discount_config();
        disConf = da_micro_product_config.DiscountConfig.GetProductDiscount(proConf.Product_ID, proConf.RiderProductID, 0, 0, "SELF");
        bl_micro_product_rider_rate riderRate = new bl_micro_product_rider_rate();
        bl_product proInfo = new bl_product();
        proInfo = da_product.GetProductByProductID(proConf.Product_ID);
        bl_micro_product_rider proRiderInfo = new bl_micro_product_rider();
        proRiderInfo = da_micro_product_rider.GetMicroProductByProductID(proConf.RiderProductID);
        DateTime tranDate = DateTime.Now;

        bl_sale_agent_micro agent = new bl_sale_agent_micro();
        agent = da_sale_agent.GetSaleAgentMicro(txtSaleAgentID.Text);

        int payMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
        bool isSaveCus = false;
        bool isSaveAddress = false;
        bool isSaveContact = false;
        bool isSavePolicy = false;
        bool isSaveBen = false;
        bool isSavePolicyDetail = false;
        bool isSavePayment = false;
        bool isSavePaymentBunchSummary = false;
        bool isSavePaymentBunchDetail = false;
        double totalAmount = 0;
        double totalDiscount = 0;
        double grandTotalAmount = 0;
        double totalReturnAmount = 0;
        double totalAnnualPremium = 0;
        double totalPremium = 0;
        double totalSA = 0;
        int loopStep = 0;
        int countPolicy = 0;
        DateTime reportDate;
        bool isSaveSuccess = true;
        string message = "";
        string firstPolicyNo = "";
        string lastPolicyNo = "";
        double riderSA = ddlRiderSumAssure.Items.Count > 0 ? Convert.ToDouble(ddlRiderSumAssure.SelectedValue) : 0;
        int coverPeriod = 0;
        int payPeriod = 0;
        try
        {
            bl_micro_group_policy pol;
            bl_micro_group_policy_detail polDe;
            bl_micro_group_policy_payment pay = new bl_micro_group_policy_payment();
            bl_micro_group_policy_payment_bunch.summary bunchSummary = new bl_micro_group_policy_payment_bunch.summary();
            bl_micro_group_policy_payment_bunch.detail bunchDetail;
            bl_micro_group_invoice.summary invoiceSummary = new bl_micro_group_invoice.summary();
            bl_micro_policy_beneficiary beneficiary;

            #region save customer
            bl_micro_group_customer cus;//= new bl_micro_group_customer();
            pre = da_micro_group_customer_prefix.GetLastCustomerPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_group_master_product> gMasterList = new List<bl_group_master_product>();
            gMasterList = da_group_master_product.GetGroupMasterProductList(PrivateGroupMasterProduct.ProductID, ddlChannelItem.SelectedValue);
            var gMaster = gMasterList[0];

            polPre = da_micro_group_policy_prefix.GetLastPolicyPrefix(gMaster.GroupCode);

            string cusNo = "";
            reportDate = Helper.FormatDateTime(txtPaymentReportDate.Text.Trim());

            bl_group_micro_application app = new bl_group_micro_application();
            bl_group_micro_application_number appNumberPrefix = new bl_group_micro_application_number();
            bl_group_micro_application_prefix appPrefix = new bl_group_micro_application_prefix();
            appPrefix = da_group_micro_application_prefix.GetLastPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_loan_upload> loan = (List<bl_loan_upload>)Session["SS_LOAN_LIST_OBJECT"];

            bool isExistCust = false;
            tranDate = DateTime.Now;

            string coverPeriodType = ddlCoverPeriodType.SelectedItem.Text;
            string payPeriodType = ddlPayPeriodType.SelectedItem.Text;
            coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
            payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

            string paymentModeEn = Helper.GetPaymentModeEnglish(payMode);
            string paymentModeKh = Helper.GetPaymentModeInKhmer(payMode);

            if (da_micro_group_loan_upload.SaveLoan(userName, tranDate, ddlChannelItem.SelectedValue))
            {
                //copy file
                File.Copy(MyTempFilePath, MyFilePath, true);

                foreach (bl_loan_upload l in loan) //loop loan records
                {
                    tranDate = DateTime.Now;
                    loopStep += 1;

                    cus = new bl_micro_group_customer();
                    //check existing customer
                    //cus = da_micro_group_customer.Get(l.ClientName, l.IdType, l.IdNumber);
                    //  cus = da_micro_group_customer.Get(l.ClientName, l.IdType, l.IdNumber, PrivateGroupMasterProduct.GroupCode);
                    cus = da_micro_group_customer.Get(l.IdType, l.IdNumber, PrivateGroupMasterProduct.GroupCode);
                    if (cus.CUSTOMER_NUMBER != null)
                    {
                        isSaveCus = true;
                        isExistCust = true;
                    }
                    else//new customer
                    {
                        isExistCust = false;
                        bl_micro_group_customer.LastSequence objCustLastSeq = da_micro_group_customer.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);
                        cus = new bl_micro_group_customer(PrivateGroupMasterProduct.GroupCode);
                        cus.SEQ = Convert.ToInt32(objCustLastSeq.SequenceNumber + 1);// cus.LAST_SEQ + 1;

                        if (pre.PREFIX2 == objCustLastSeq.Prefix)// cus.LAST_PREFIX)//in the same year
                        {
                            cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        }
                        else
                        {
                            cus.SEQ = 1;
                            cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        }
                        cus.CUSTOMER_NUMBER = cusNo;

                        string[] breakName = Helper.BreakFullName(l.ClientName, ' ');

                        if (Helper.CheckContentUnicode(l.ClientName))
                        {
                            cus.FULL_NAME_KH = l.ClientName;
                            cus.FIRST_NAME_KH = breakName[1];// Helper.GetBreakName(cus.FULL_NAME_EN, 1);
                            cus.LAST_NAME_KH = breakName[0];// Helper.GetBreakName(cus.FULL_NAME_EN, 0);
                            cus.FULL_NAME_EN = "";
                            cus.FIRST_NAME_EN = "";
                            cus.LAST_NAME_EN = "";
                        }
                        else
                        {
                            cus.FULL_NAME_EN = l.ClientName;
                            cus.FIRST_NAME_EN = breakName[1];// Helper.GetBreakName(cus.FULL_NAME_EN, 1);
                            cus.LAST_NAME_EN = breakName[0];//Helper.GetBreakName(cus.FULL_NAME_EN, 0);
                            cus.FULL_NAME_KH = "";
                            cus.FIRST_NAME_KH = "";
                            cus.LAST_NAME_KH = "";
                        }


                        cus.GENDER = l.Gender;
                        cus.DOB = l.DOB;
                        cus.ID_TYPE = l.IdType;
                        cus.ID_NUMBER = l.IdNumber;
                        cus.OCCUPATION = l.Occupation;
                        cus.STATUS = 1;
                        cus.CREATED_BY = userName;
                        cus.CREATED_ON = tranDate;
                        cus.MARITAL_STATUS = "";
                        isSaveCus = da_micro_group_customer.Save(cus);
                    }

                    if (isSaveCus)
                    {
                        //loop loan period
                        for (int i = 0; i < l.CoverableYear; i++)
                        {
                            appNumberPrefix = da_group_micro_application_number.GetLastSeq(PrivateGroupMasterProduct.GroupCode);
                            bl_group_micro_application_number appNumber = new bl_group_micro_application_number()
                            {
                                Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION_NUMBER" }, { "FIELD", "ID" } }),
                                ChannelId = ddlChannel.SelectedValue,
                                ChannelItemId = ddlChannelItem.SelectedValue,
                                ChannelLocationId = ddlChannelLocation.SelectedValue,
                                GroupCode = PrivateGroupMasterProduct.GroupCode,
                                CreatedBy = userName,
                                CreatedOn = DateTime.Now
                            };

                            if (appNumberPrefix.PrefixYear == appPrefix.Prefix2)//in same year
                            {
                                appNumber.Seq = appNumberPrefix.Seq + 1;
                            }
                            else
                            {
                                appNumber.Seq = 1;
                            }
                            appNumber.ApplicationNumber = appPrefix.Prefix1 + appPrefix.Prefix2 + appNumber.Seq.ToString(appPrefix.Digits);

                            if (da_group_micro_application_number.Save(appNumber))
                            {

                                bool saveAppTran = Transaction.GroupMirco.ApplicationNumber.BackUp(new Transaction.GroupMirco.ApplicationNumber.Tran()
                                {
                                    ApplicationNumber = appNumber.ApplicationNumber,
                                    TranBy = userName,
                                    TranDate = tranDate,
                                    TranType = "INSERT"
                                });
                                if (saveAppTran)
                                {
                                    isSaveSuccess = true;
                                }
                                else
                                {
                                    isSaveSuccess = false;
                                    message = "Backup application fail.";
                                    break;
                                }

                                #region rate and discount
                                double basicDiscount = 0.0;
                                double riderDiscount = 0.0;
                                double basicPremiumByMode = 0.0;
                                double basicAnnualPremium = 0.0;
                                double totalBisicAmount = 0;
                                double totalRiderAmount = 0;
                                double riderPremiumByMode = 0.0;
                                double riderAnnualPremium = 0.0;
                                double[] riderPremium = new double[] { 0, 0 };

                                DateTime issueDate = l.DisbursementDate;
                                DateTime effectiveDate = l.DisbursementDate;
                                //if (i > 1)
                                //{
                                //    effectiveDate = effectiveDate.AddYears(i-1);
                                //}
                                effectiveDate = effectiveDate.AddYears(i);
                                //  int custAge = Calculation.Culculate_Customer_Age(cus.DOB.ToString("dd/MM/yyyy"), l.DisbursementDate);
                                int custAge = Calculation.Culculate_Customer_Age(cus.DOB.ToString("dd/MM/yyyy"), effectiveDate);
                                //get rate
                                proRate = new bl_micro_product_rate();
                                proRate = da_micro_product_rate.GetProductRate(proConf.Product_ID, cus.GENDER, custAge, l.LoanAmount, payMode);
                                if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                                {
                                    riderRate = da_micro_product_rider_rate.GetProductRate(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                    riderPremium = Calculation.GetMicroProductRiderPremium(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                    riderPremiumByMode = riderPremium[1];
                                    riderAnnualPremium = riderPremium[0];
                                }
                                double[] premium = Calculation.GetMicroProducPremium(proConf.Product_ID, cus.GENDER, custAge, l.LoanAmount, payMode);
                                basicPremiumByMode = premium[1];
                                basicAnnualPremium = premium[0];

                                if (!string.IsNullOrWhiteSpace(disConf.ProductID))
                                {
                                    basicDiscount = disConf.BasicDiscountAmount;
                                    riderDiscount = disConf.RiderDiscountAmount;
                                }
                                totalBisicAmount = basicPremiumByMode - basicDiscount;
                                totalRiderAmount = riderPremiumByMode - riderDiscount;
                                #endregion rate and discount

                                #region save application
                                string appAddressEn = "";
                                string appAddressKh = "";
                                if (Helper.CheckContentUnicode(l.Address))
                                {
                                    appAddressKh = l.Address;
                                    appAddressEn = "";
                                }
                                else
                                {
                                    appAddressEn = l.Address;
                                    appAddressKh = "";
                                }
                                app = new bl_group_micro_application()
                                {
                                    Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION" }, { "FIELD", "ID" } }),
                                    ApplicationNumber = appNumber.ApplicationNumber,
                                    ApplicationDate = l.DisbursementDate,
                                    AgentCode = agent.SaleAgentId,
                                    AgentNameEn = agent.FullNameEn,
                                    AgentNameKh = agent.FullNameKh,
                                    IdType = l.IdType,
                                    IdEn = l.IdTypeText,
                                    IdKh = Helper.GetIDCardTypeTextKh(cus.ID_TYPE),//(l.IdType),
                                    IdNo = l.IdNumber,
                                    FirstNameInEnglish = cus.FIRST_NAME_EN,// Helper.GetBreakName(l.ClientName, 1),
                                    LastNameInEnglish = cus.LAST_NAME_EN,//Helper.GetBreakName(l.ClientName, 0),
                                    FirstNameInKhmer = cus.FIRST_NAME_KH,
                                    LastNameInKhmer = cus.LAST_NAME_KH,
                                    Gender = l.Gender,
                                    GenderEn = l.GenderText,
                                    GenderKh = Helper.GetGenderText(l.Gender, false, true),
                                    BirthOfDate = l.DOB,
                                    Nationality = "",
                                    MaritalStatus = "",
                                    MaritalStatusKh = "",
                                    Occupation = l.Occupation,
                                    OccupationKh = "",
                                    PhoneNumber = l.ContactNumber,
                                    Email = "",
                                    Address = appAddressEn,
                                    AddressKh = appAddressKh,
                                    Province = "",
                                    ProvinceKh = "",
                                    ProductId = proConf.Product_ID,
                                    ProductName = proInfo.En_Title,
                                    ProductNameKh = proInfo.Kh_Title,
                                    SumAssure = l.LoanAmount,
                                    TermOfCover = coverPeriod,
                                    PaymentPeriod = payPeriod,
                                    PayPeriodType = payPeriodType,
                                    CoverPeriodType = coverPeriodType,
                                    PayMode = payMode,
                                    PayModeEn = paymentModeEn,
                                    PayModeKh = paymentModeKh,
                                    Premium = basicPremiumByMode,
                                    AnnualPremium = basicAnnualPremium,
                                    UserPremium = 0,
                                    DiscountAmount = basicDiscount,
                                    TotalAmount = totalBisicAmount,
                                    RiderProductId = proConf.RiderProductID,
                                    RiderProductName = proRiderInfo.EN_TITLE != null ? proRiderInfo.EN_TITLE : "",
                                    RiderProductNameKh = proRiderInfo.KH_TITLE != null ? proRiderInfo.KH_TITLE : "",
                                    RiderSumAssure = riderSA,
                                    RiderPremium = riderPremiumByMode,
                                    RiderAnnualPremium = riderAnnualPremium,
                                    RiderDiscountAmount = riderDiscount,
                                    RiderTotalAmount = totalRiderAmount,
                                    BenFullName = l.BeneficiaryName,
                                    BenAge = "-",
                                    BenAddress = l.Address,
                                    PercentageShared = 100,
                                    Relation = l.Relation,
                                    RelationKh = "",
                                    QuestionId = "6D44FA76-6B00-42D6-A4D7-ACD85986DC7C",
                                    Answer = -1,
                                    AnswerRemarks = "",
                                    PaymentCode = "",
                                    Referrer = "",
                                    ReferrerId = ""

                                };

                                isSaveSuccess = da_group_micro_application.Save(app);
                                if (isSaveSuccess)
                                {
                                    bool saveTranApp = Transaction.GroupMirco.Application.BackUp(new Transaction.GroupMirco.Application.Tran()
                                    {
                                        ApplicationNumber = appNumber.ApplicationNumber,
                                        TranBy = userName,
                                        TranDate = tranDate,
                                        TranType = "INSERT"
                                    });
                                    if (!saveTranApp)
                                    {
                                        isSaveSuccess = false;
                                        message = "Save application fail.";
                                        break;
                                    }
                                }
                                else
                                {
                                    isSaveSuccess = false;
                                    message = "Save application fail.";
                                    break;
                                }
                                #endregion save applicaiton
                                #region Save Contact

                                if (!isExistCust)
                                {
                                    if (i == 0)//save only first time cuz cutomer is the same
                                    {
                                        isSaveContact = da_micro_group_customer.Contact.Save(new bl_micro_group_customer_contact()
                                        {
                                            CUSTOMER_ID = cus.ID,
                                            PHONE_NUMBER1 = l.ContactNumber,
                                            CREATED_BY = userName,
                                            CREATED_ON = tranDate
                                        });
                                    }
                                    else // same customer is detected so need to save
                                    {
                                        isSaveContact = true;
                                    }
                                }
                                else
                                {
                                    isSaveContact = true;
                                }

                                if (!isSaveContact)
                                {
                                    isSaveSuccess = false;
                                    message = da_micro_group_customer.MESSAGE;
                                    break;
                                }
                                else
                                {
                                    #region Save Address
                                    if (!isExistCust)
                                    {
                                        if (i == 0)//save only first time cuz cutomer is the same
                                        {
                                            isSaveAddress = da_micro_group_customer.Address.Save(
                                                new bl_address()
                                                {
                                                    CUSTOMER_ID = cus.ID,
                                                    ADDRESS_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CUSTOMER_ADDRESS" }, { "FIELD", "ADDRESS_ID" } }),
                                                    ADDRESS1 = l.Address,
                                                    CREATED_BY = userName,
                                                    CREATED_ON = tranDate
                                                }
                                                );
                                        }
                                        else// same customer is detected so need to save
                                        {
                                            isSaveAddress = true;
                                        }
                                    }
                                    else
                                    {
                                        isSaveAddress = true;
                                    }
                                    if (!isSaveAddress)
                                    {
                                        isSaveSuccess = false;
                                        message = da_micro_group_customer.MESSAGE;
                                        break;
                                    }
                                    else
                                    {
                                        #region policy
                                        string pol_number = "";

                                        pol = new bl_micro_group_policy(PrivateGroupMasterProduct.GroupCode);

                                        //pol.SEQ = pol.LAST_SEQ + 1;
                                        //if (pol.LAST_PREFIX == polPre.PREFIX2)//in the same year
                                        //{
                                        //    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);
                                        //}
                                        //else
                                        //{
                                        //    pol.SEQ = 1;
                                        //    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);
                                        //}
                                        bl_micro_group_policy.LastSequence objLastSeq = da_micro_group_policy.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);

                                        if (objLastSeq.Prefix == polPre.PREFIX2)//in the same year
                                        {
                                            pol.SEQ = objLastSeq.LastSequenceNumber + 1;
                                        }
                                        else
                                        {
                                            pol.SEQ = 1;

                                        }
                                        pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);

                                        //if (loopStep == 1)
                                        //{
                                        //    firstPolicyNo = pol_number;
                                        //}
                                        //else if (loopStep == loan.Count)
                                        //{
                                        //    lastPolicyNo = pol_number;
                                        //}

                                        if (i == 0 && loopStep == 1)
                                        {
                                            firstPolicyNo = pol_number;
                                        }
                                        else if (i == l.LoanPeriod - 1)
                                        {
                                            lastPolicyNo = pol_number;
                                        }

                                        pol.ApplicationId = app.Id;
                                        pol.PolicyNumber = pol_number;
                                        pol.CustomerId = cus.ID;
                                        pol.GroupMasterId = gMaster.GroupMasterID;
                                        pol.LoanId = l.LoanID;
                                        pol.ProductId = proConf.Product_ID;
                                        pol.Currency = l.Currency;
                                        pol.ExchangeRate = l.ExchangeRate;
                                        pol.PolicyStatus = "IF";
                                        pol.AgentCode = txtSaleAgentID.Text.Trim();
                                        pol.ChannelId = ddlChannel.SelectedValue;
                                        pol.ChannelItemId = ddlChannelItem.SelectedValue;
                                        pol.ChannelLocationId = ddlChannelLocation.SelectedValue;
                                        pol.createdBy = userName;
                                        pol.CreatedOn = tranDate;
                                        isSavePolicy = da_micro_group_policy.Save(pol);
                                        if (!isSavePolicy)
                                        {
                                            isSaveSuccess = false;
                                            message = da_micro_group_policy.MESSAGE;
                                            break;
                                        }
                                        else
                                        {
                                            //backup policy
                                            Transaction.GroupMirco.Policy.Backup(new Transaction.GroupMirco.Policy.Tran() { PolicyId = pol.PolicyId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                            //count success policy
                                            countPolicy += 1;

                                            #region save approver

                                            //save approver
                                            List<da_report_approver.bl_report_approver> approver = new List<da_report_approver.bl_report_approver>();
                                            approver = da_report_approver.GetApproverList();

                                            foreach (da_report_approver.bl_report_approver ap in approver.Where(_ => _.NameEn.ToUpper() == AppConfiguration.GetApplicationApprover().ToUpper()))
                                            {
                                                isSaveSuccess = da_report_approver.InsertApproverPolicy(new da_report_approver.bl_report_approver_policy()
                                                {
                                                    Approver_ID = ap.ID,
                                                    Policy_ID = pol.PolicyId,
                                                    Created_By = userName,//my_session.USER_NAME,
                                                    Created_On = tranDate
                                                });
                                                break;

                                            }

                                            if (!isSaveSuccess)
                                            {
                                                message = "Saved Approver fail.";
                                                break;
                                            }
                                            #endregion save approver

                                            #region Beneficiary
                                            beneficiary = new bl_micro_policy_beneficiary()
                                            {
                                                ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_BENEFICIARY" }, { "FIELD", "BENEFICIARY_ID" } }),
                                                POLICY_ID = pol.PolicyId,
                                                FULL_NAME = app.BenFullName,
                                                Gender = "",
                                                AGE = app.BenAge,
                                                ADDRESS = app.BenAddress,
                                                PERCENTAGE_OF_SHARE = 100,
                                                RELATION = l.Relation,
                                                CREATED_BY = userName,
                                                CREATED_ON = tranDate,
                                                REMARKS = ""

                                            };
                                            isSaveBen = da_micro_group_policy_beneficiary.Save(beneficiary);
                                            if (!isSaveBen)
                                            {
                                                isSaveSuccess = false;
                                                message = da_micro_group_policy_beneficiary.MESSAGE;
                                                break;
                                            }
                                            else
                                            {
                                                isSaveSuccess = true;
                                                //backup bendeficiary
                                                Transaction.GroupMirco.Beneficiary.Backup(new Transaction.GroupMirco.Beneficiary.Tran() { BeneficiaryId = beneficiary.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                                #region Policy Rider
                                                string policyRiderid = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_RIDER" }, { "FIELD", "POLICY_RIDER_ID" } });
                                                if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                                                {
                                                    double premRielRider = pol.ExchangeRate > 0 ? Math.Round(riderPremiumByMode * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0;
                                                    if (premRielRider > 0)
                                                    {
                                                        int leR = premRielRider.ToString().Length;
                                                        string bR = premRielRider.ToString().Substring(leR - 2, 2);

                                                        if (Convert.ToDouble(bR) < 50)
                                                        {
                                                            //round down
                                                            premRielRider = premRielRider - Convert.ToDouble(bR);
                                                        }
                                                        else if (Convert.ToDouble(bR) >= 50)
                                                        {
                                                            premRielRider = (premRielRider - Convert.ToDouble(bR)) + 100;
                                                        }
                                                    }

                                                    isSaveSuccess = da_micro_group_policy_rider.Save(new bl_micro_group_policy_rider()
                                                    {

                                                        PolicyRiderId = policyRiderid,
                                                        PolicyId = pol.PolicyId,
                                                        ProductId = proConf.RiderProductID,
                                                        SumAssured = riderSA,
                                                        PremiumRate = riderRate.RATE,
                                                        Premium = riderPremiumByMode,
                                                        PremiumRiel = premRielRider,
                                                        AnnualPremium = riderAnnualPremium,
                                                        DiscountAmount = riderDiscount,
                                                        RiderStatus = "IF",
                                                        TotalAmount = riderPremiumByMode - riderDiscount,
                                                        CreatedBy = userName,
                                                        CreatedOn = tranDate,
                                                        Remarks = ""
                                                    });

                                                    if (isSaveSuccess)
                                                    {
                                                        isSaveSuccess = Transaction.GroupMirco.PolicyRider.BackUp(new Transaction.GroupMirco.PolicyRider.Tran()
                                                        {
                                                            PolicyRiderId = policyRiderid,
                                                            TranBy = userName,
                                                            TranDate = tranDate,
                                                            TranType = "INSERT"
                                                        });
                                                        if (!isSaveSuccess)
                                                        {
                                                            message = "Backup Policy Rider fail.";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        message = da_micro_group_policy_rider.MESSAGE;

                                                    }
                                                    if (!isSaveSuccess)
                                                    {
                                                        message = "Backup Policy Rider fail.";

                                                    }
                                                }
                                                #endregion Policy Rider

                                                if (!isSaveSuccess)
                                                {

                                                    break;
                                                }


                                                polDe = new bl_micro_group_policy_detail();
                                                polDe.PolicyID = pol.PolicyId;
                                                polDe.IssuedDate = l.DisbursementDate;
                                                polDe.EffectivedDate = effectiveDate;// l.DisbursementDate;

                                                //polDe.CoverYear = int.Parse(ddlCoverPeriod.SelectedValue);// 1;
                                                //polDe.PayYear = int.Parse(ddlPayPeriod.SelectedValue);// 1;

                                                //polDe.MaturityDate = polDe.EffectivedDate.AddYears(polDe.CoverYear);
                                                //polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);

                                                polDe.CoverPeriodType = ddlCoverPeriodType.SelectedItem.Text;
                                                polDe.PayPeriodType = ddlPayPeriodType.SelectedItem.Text;

                                                coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
                                                payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

                                                polDe.CoverYear = coverPeriod;
                                                polDe.PayYear = payPeriod;

                                                if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                                                {
                                                    polDe.MaturityDate = polDe.EffectivedDate.AddYears(polDe.CoverYear);
                                                    polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                                }
                                                else if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                                                {
                                                    polDe.MaturityDate = polDe.EffectivedDate.AddMonths(polDe.CoverYear);
                                                    polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                                }
                                                else if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.D.ToString())
                                                {
                                                    polDe.MaturityDate = polDe.EffectivedDate.AddDays(polDe.CoverYear);
                                                    polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                                }

                                                polDe.Age = custAge;// Calculation.Culculate_Customer_Age(cus.DOB.ToString("dd/MM/yyyy"), polDe.IssuedDate);

                                                polDe.SumAssured = pol.Currency.ToUpper() == "USD" ? l.LoanAmount : Math.Round(l.LoanAmount / pol.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                                                polDe.PayMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                                                polDe.PaymentCode = "";

                                                polDe.PremiumRate = proRate.RATE;
                                                polDe.Premium = Math.Round(premium[1], 2, MidpointRounding.AwayFromZero);

                                                double premRiel = pol.ExchangeRate > 0 ? Math.Round(polDe.Premium * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0;
                                                if (premRiel > 0)
                                                {
                                                    int le = premRiel.ToString().Length;
                                                    string b = premRiel.ToString().Substring(le - 2, 2);
                                                    if (Convert.ToDouble(b) < 50)
                                                    {
                                                        //round down
                                                        premRiel = premRiel - Convert.ToDouble(b);
                                                    }
                                                    else if (Convert.ToDouble(b) >= 50)
                                                    {
                                                        premRiel = (premRiel - Convert.ToDouble(b)) + 100;
                                                    }
                                                }
                                                //check discount 

                                                if (disConf.ProductID != null)
                                                {
                                                    polDe.DiscountAmount = disConf.BasicDiscountAmount;

                                                }
                                                else
                                                {
                                                    polDe.DiscountAmount = 0;
                                                }

                                                polDe.TotalAmount = polDe.Premium - polDe.DiscountAmount;
                                                polDe.PremiumRiel = premRiel;// pol.ExchangeRate > 0 ? Math.Round(polDe.Premium * pol.ExchangeRate, 0, MidpointRounding.ToEven) : 0;
                                                polDe.AnnualPremium = Math.Round(premium[0], 2, MidpointRounding.AwayFromZero); ;

                                                polDe.PolicyStatusRemarks = "NEW";
                                                polDe.RenewalFrom = "";
                                                polDe.FrequencyReduceYear = 0;
                                                polDe.ReduceRate = 0;
                                                polDe.CreatedBy = userName;
                                                polDe.CreatedOn = tranDate;

                                                isSavePolicyDetail = da_micro_group_policy_detail.Save(polDe);
                                                if (!isSavePolicyDetail)
                                                {
                                                    isSaveSuccess = false;
                                                    message = da_micro_group_policy_detail.MESSAGE;
                                                    break;
                                                }
                                                else
                                                {
                                                    //backup policy detail
                                                    Transaction.GroupMirco.PolicyDetail.Backup(new Transaction.GroupMirco.PolicyDetail.Tran() { PolicyDetailId = polDe.PolicyDetailId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                                    #region payment
                                                    pay = new bl_micro_group_policy_payment()
                                                    {
                                                        PolicyDetailId = polDe.PolicyDetailId,
                                                        PayMode = payMode,
                                                        UserPremium = polDe.Premium,
                                                        Amount = polDe.Premium,
                                                        DiscountAmount = polDe.DiscountAmount,
                                                        TotalAmount = polDe.TotalAmount,
                                                        AmountRiel = polDe.PremiumRiel,
                                                        DueDate = polDe.EffectivedDate,
                                                        NextDueDate = payMode == 1 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddYears(1), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                        payMode == 2 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(6), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                        payMode == 3 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(3), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                        payMode == 4 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(1), polDe.EffectivedDate, polDe.EffectivedDate) : new DateTime(1900, 1, 1),
                                                        PayDate = new DateTime(1900, 1, 1),// polDe.EffectivedDate,
                                                        ReturnAmount = (polDe.Premium - polDe.Premium),
                                                        PolicyStatus = "IF",
                                                        PremiumYear = 1,
                                                        PremiumLot = 1,
                                                        OfficeId = "HQ",
                                                        TransactionType = "",
                                                        TransactionRef = "",
                                                        CreatedBy = userName,
                                                        CreatedOn = tranDate,
                                                        ReportDate = reportDate
                                                    };

                                                    totalAmount += pay.Amount;
                                                    totalDiscount += pay.DiscountAmount;
                                                    grandTotalAmount += pay.TotalAmount;
                                                    totalReturnAmount += pay.ReturnAmount;

                                                    totalPremium += polDe.Premium;
                                                    totalAnnualPremium += polDe.AnnualPremium;
                                                    totalSA += polDe.SumAssured;


                                                    isSavePayment = da_micro_group_policy_payment.Save(pay);
                                                    if (!isSavePayment)
                                                    {
                                                        isSaveSuccess = false;
                                                        message = da_micro_group_policy_payment.MESSAGE;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        #region save certificate
                                                        isSaveSuccess = da_group_micro_certificate.Save(new bl_group_micro_certificate()
                                                        {
                                                            Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CERTIFICATE" }, { "FIELD", "ID" } }),

                                                            CustomerNo = cus.CUSTOMER_NUMBER,
                                                            PolicyNumber = pol.PolicyNumber,
                                                            AgentCode = app.AgentCode,
                                                            AgentNameEn = app.AgentNameEn,
                                                            AgentNameKh = app.AgentNameKh,
                                                            IdType = app.IdType,
                                                            IdEn = app.IdEn,
                                                            IdKh = app.IdKh,
                                                            IdNo = app.IdNo,
                                                            FullName = l.ClientName,
                                                            Gender = app.Gender,
                                                            GenderEn = app.GenderEn,
                                                            GenderKh = app.GenderKh,
                                                            DateOfBirth = app.BirthOfDate,
                                                            Age = polDe.Age,
                                                            Nationality = app.Nationality,
                                                            Address = l.Address,
                                                            Province = app.Province,
                                                            ProductId = app.ProductId,
                                                            ProductName = app.ProductName,
                                                            ProductNameKh = app.ProductNameKh,
                                                            SumAssure = app.SumAssure,
                                                            CoverPeriodType = app.CoverPeriodType,
                                                            PayPeriodType = app.PayPeriodType,
                                                            TermOfCover = app.TermOfCover,
                                                            PaymentPeriod = app.PaymentPeriod,
                                                            PayMode = app.PayMode,
                                                            PayModeEn = app.PayModeEn,
                                                            PayModeKh = app.PayModeKh,
                                                            Premium = app.Premium,
                                                            AnnualPremium = app.AnnualPremium,
                                                            UserPremium = app.UserPremium,
                                                            DiscountAmount = app.DiscountAmount,
                                                            TotalAmount = app.TotalAmount,
                                                            RiderProductId = app.RiderProductId,
                                                            RiderProductName = app.RiderProductName,
                                                            RiderProductNameKh = app.RiderProductNameKh,
                                                            RiderSumAssure = app.RiderSumAssure,
                                                            RiderPremium = app.RiderPremium,
                                                            RiderAnnualPremium = app.RiderAnnualPremium,
                                                            RiderDiscountAmount = app.RiderDiscountAmount,
                                                            RiderTotalAmount = app.RiderTotalAmount,
                                                            EffectiveDate = polDe.EffectivedDate,
                                                            ExpiryDate = polDe.ExpiryDate,
                                                            NextDueDate = pay.NextDueDate
                                                        });
                                                        if (isSaveSuccess)
                                                        {
                                                            bool saveCert = Transaction.GroupMirco.Certificate.BackUp(new Transaction.GroupMirco.Certificate.Tran() { PolicyNumber = pol.PolicyNumber, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                                            if (!saveCert)
                                                            {
                                                                isSaveSuccess = false;
                                                                message = "Backup certificate fail.";
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            isSaveSuccess = false;
                                                            message = "Save certificate fail.";
                                                            break;
                                                        }
                                                        #endregion save certificate

                                                        //save policy payment transaction 
                                                        Transaction.GroupMirco.PolicyPayment.Backup(new Transaction.GroupMirco.PolicyPayment.Tran()
                                                        {
                                                            PolicyPaymentId = pay.PolicyPaymentId,
                                                            TranBy = userName,
                                                            TranDate = tranDate,
                                                            TranType = "INSERT"

                                                        });

                                                        #region bunch payment summary
                                                        if (i == 0 && loopStep == 1) // save only first loop
                                                        {
                                                            bunchSummary = new bl_micro_group_policy_payment_bunch.summary()
                                                            {
                                                                GroupMasterCode = gMaster.GroupCode,
                                                                Amount = totalAmount,
                                                                DisountAmount = totalDiscount,
                                                                TotalAmount = grandTotalAmount,
                                                                ReturnAmount = totalReturnAmount,
                                                                Status = 0,//pending to pay 1 is paid,
                                                                PaymentType = "NEW",//new policy
                                                                CreatedBy = userName,
                                                                CreatedOn = tranDate
                                                            };
                                                            isSavePaymentBunchSummary = da_micro_group_policy_payment_bunch.summary.Save(bunchSummary);
                                                        }

                                                        #endregion bunch payment summary
                                                        if (!isSavePaymentBunchSummary)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            //BACKUP payment summary
                                                            Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran()
                                                            {
                                                                BunchId = bunchSummary.BunchId,
                                                                TranBy = userName,
                                                                TranDate = tranDate,
                                                                TranType = "INSERT"
                                                            });

                                                            #region bunch payment detail
                                                            bunchDetail = new bl_micro_group_policy_payment_bunch.detail()
                                                            {
                                                                BunchId = bunchSummary.BunchId,
                                                                PolicyPaymentId = pay.PolicyPaymentId
                                                            };
                                                            isSavePaymentBunchDetail = da_micro_group_policy_payment_bunch.detail.Save(bunchDetail);
                                                            if (!isSavePaymentBunchDetail)
                                                            {
                                                                isSaveSuccess = false;
                                                                message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                //save transaction payment bunch detail
                                                                Transaction.GroupMirco.PaymentBunchDetail.Backup(new Transaction.GroupMirco.PaymentBunchDetail.Tran()
                                                                {
                                                                    BunchDetailId = bunchDetail.BunchDetailId,
                                                                    TranBy = userName,
                                                                    TranDate = tranDate,
                                                                    TranType = "INSERT"
                                                                });

                                                            }
                                                            #endregion bunch payment detail
                                                        }
                                                    }
                                                    #endregion payment
                                                }
                                            }
                                            #endregion Beneficiary
                                        }
                                        #endregion policy
                                    }
                                    #endregion Save Address
                                }
                                #endregion Save Conctact
                            }
                            else//save application fail
                            {
                                isSaveSuccess = false;
                                message = "Saved application number is fail.";
                                break;
                            }

                        }//END LOOP LOAN PERIOD
                    }
                    else
                    {
                        isSaveSuccess = false;
                        message = "Saved Customer fail.";
                        break;
                    }
                }//END LOOP LOAN LIST
            #endregion save customer

                if (isSaveSuccess)
                {
                    var a = bunchSummary;
                    a.Amount = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                    a.DisountAmount = Math.Round(totalDiscount, 2, MidpointRounding.AwayFromZero); ;
                    a.TotalAmount = Math.Round(grandTotalAmount, 2, MidpointRounding.AwayFromZero); ;
                    a.ReturnAmount = Math.Round(totalReturnAmount, 2, MidpointRounding.AwayFromZero);
                    a.NumberPolicy = countPolicy;
                    a.ReportDate = reportDate;
                    a.UpdatedBy = userName;
                    a.UpdatedOn = tranDate;

                    if (!da_micro_group_policy_payment_bunch.summary.Update(a))
                    {
                        isSaveSuccess = false;
                    }
                    else
                    {
                        //back up payment buch summary
                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran() { BunchId = a.BunchId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                        //if (!da_pma_report.Save(new bl_pma_report()
                        //{
                        //    PolicyNumber = gMaster.GroupCode,
                        //    EffectiveDate = gMaster.EffectiveDate,
                        //    PayDate = a.ReportDate,
                        //    Amount = a.Amount,
                        //    DiscountAmount = a.DisountAmount,
                        //    EM = 0,
                        //    Premium = totalPremium,
                        //    AnnualPremium = totalAnnualPremium,
                        //    PayYear = pay.PremiumYear,
                        //    PayLot = pay.PremiumLot,
                        //    PayMode = pay.PayModeText,
                        //    ProductCode = proConf.Plan_Code,
                        //    ProductId = proConf.Product_ID,
                        //    ProductName = proConf.En_Title,
                        //    ReportDate = a.ReportDate,
                        //    CreatedBy = userName,
                        //    CreatedOn = tranDate,
                        //    SA = totalSA,
                        //    NumberPolicy = a.NumberPolicy,
                        //    PolicyRange = firstPolicyNo + "-" + lastPolicyNo
                        //}))
                        //{
                        //    isSaveSuccess = false;
                        //}

                        bl_pma_report pma = new bl_pma_report()
                        {
                            PolicyNumber = gMaster.GroupCode,
                            EffectiveDate = gMaster.EffectiveDate,
                            PayDate = a.ReportDate,
                            Amount = a.Amount,
                            DiscountAmount = a.DisountAmount,
                            EM = 0,
                            Premium = totalPremium,
                            AnnualPremium = totalAnnualPremium,
                            PayYear = pay.PremiumYear,
                            PayLot = pay.PremiumLot,
                            PayMode = pay.PayModeText,
                            ProductCode = proConf.Plan_Code,
                            ProductId = proConf.Product_ID,
                            ProductName = proConf.En_Title,
                            ReportDate = a.ReportDate,
                            CreatedBy = userName,
                            CreatedOn = tranDate,
                            SA = totalSA,
                            NumberPolicy = a.NumberPolicy,
                            PolicyRange = firstPolicyNo + "-" + lastPolicyNo
                        };

                        if (da_pma_report.Save(pma))
                        {
                            isSaveSuccess = Transaction.GroupMirco.PMA.BackUp(new Transaction.GroupMirco.PMA.Tran() { ID = pma.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                        }
                        else
                        {
                            isSaveSuccess = false;
                        }

                    }
                    btnSave.Enabled = false;
                }

                //popup message
                if (isSaveSuccess)
                {

                    // Transaction.GroupMirco.ClearBackupTransactionIssuePolicyRecords(userName, tranDate, "INSERT");

                    //url
                    string url = string.Format("../Reports/group_micro_policy_detail_req.aspx?CHID={0}&FDATE={1}&TDATE={2}&PROID={3} ", ddlChannelItem.SelectedValue, txtPaymentReportDate.Text, txtPaymentReportDate.Text, ddlProduct.SelectedValue);
                    url = "<a href='" + url + " ' target='_blank'><span>Click here to show report</span></a>";

                    Helper.Alert(false, "Saved successfully.<br/>" + url, lblError);
                    btnUpload.Enabled = true;
                    da_micro_group_loan_upload.DeleteTempRecords(userName, tranDate);
                }
                else
                {
                    if (File.Exists(MyFilePath))
                    {
                        File.Delete(MyFilePath);

                    }
                    if (RoleBack(userName, tranDate))
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
                    }
                    btnSave.Enabled = false;
                }


            }
            else
            { //save loan fail
                Helper.Alert(true, "Saved fail. Error:" + da_micro_group_loan_upload.MESSAGE, lblError);

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnSave_Click()] in page [frm_gmCustomer_upload], detail:" + ex.Message + "=>" + ex.StackTrace);
            if (RoleBack(userName, tranDate))
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
            }
            else
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
            }
            btnSave.Enabled = false;
        }

        //Delete temp file
        if (File.Exists(MyTempFilePath))
        {
            File.Delete(MyTempFilePath);
        }
    }
    protected void SaveMoneyTransfer()
    {
        bl_micro_customer_prefix pre = new bl_micro_customer_prefix();
        bl_micro_group_policy_prefix polPre = new bl_micro_group_policy_prefix();
        bl_micro_product_config proConf = MyProductConfig;
        bl_micro_product_rate proRate;
        bl_micro_product_discount_config disConf = new bl_micro_product_discount_config();
        disConf = da_micro_product_config.DiscountConfig.GetProductDiscount(proConf.Product_ID, proConf.RiderProductID, 0, 0, "SELF");
        bl_micro_product_rider_rate riderRate = new bl_micro_product_rider_rate();
        bl_product proInfo = new bl_product();
        proInfo = da_product.GetProductByProductID(proConf.Product_ID);
        bl_micro_product_rider proRiderInfo = new bl_micro_product_rider();
        proRiderInfo = da_micro_product_rider.GetMicroProductByProductID(proConf.RiderProductID);
        DateTime tranDate = DateTime.Now;

        bl_sale_agent_micro agent = new bl_sale_agent_micro();
        agent = da_sale_agent.GetSaleAgentMicro(txtSaleAgentID.Text);

        int payMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
        bool isSaveCus = false;
        bool isSaveAddress = false;
        bool isSaveContact = false;
        bool isSavePolicy = false;
        bool isSaveBen = false;
        bool isSavePolicyDetail = false;
        bool isSavePayment = false;
        bool isSavePaymentBunchSummary = false;
        bool isSavePaymentBunchDetail = false;
        double totalAmount = 0;
        double totalDiscount = 0;
        double grandTotalAmount = 0;
        double totalReturnAmount = 0;
        double totalAnnualPremium = 0;
        double totalPremium = 0;
        double totalSA = 0;
        int loopStep = 0;
        int countPolicy = 0;
        DateTime reportDate;
        bool isSaveSuccess = true;
        string message = "";
        string firstPolicyNo = "";
        string lastPolicyNo = "";
        double riderSA = ddlRiderSumAssure.Items.Count > 0 ? Convert.ToDouble(ddlRiderSumAssure.SelectedValue) : 0;

        int coverPeriod = 0;
        int payPeriod = 0;

        try
        {
            bl_micro_group_policy pol;
            bl_micro_group_policy_detail polDe;
            bl_micro_group_policy_payment pay = new bl_micro_group_policy_payment();
            bl_micro_group_policy_payment_bunch.summary bunchSummary = new bl_micro_group_policy_payment_bunch.summary();
            bl_micro_group_policy_payment_bunch.detail bunchDetail;
            bl_micro_group_invoice.summary invoiceSummary = new bl_micro_group_invoice.summary();
            bl_micro_policy_beneficiary beneficiary;

            #region save customer
            bl_micro_group_customer cus;
            pre = da_micro_group_customer_prefix.GetLastCustomerPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_group_master_product> gMasterList = new List<bl_group_master_product>();
            gMasterList = da_group_master_product.GetGroupMasterProductList(PrivateGroupMasterProduct.ProductID, ddlChannelItem.SelectedValue);
            var gMaster = gMasterList[0];

            polPre = da_micro_group_policy_prefix.GetLastPolicyPrefix(gMaster.GroupCode);

            string cusNo = "";
            reportDate = Helper.FormatDateTime(txtPaymentReportDate.Text.Trim());

            bl_group_micro_application app = new bl_group_micro_application();
            bl_group_micro_application_number appNumberPrefix = new bl_group_micro_application_number();
            bl_group_micro_application_prefix appPrefix = new bl_group_micro_application_prefix();
            appPrefix = da_group_micro_application_prefix.GetLastPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_money_transfer_upload> tranMoney = (List<bl_money_transfer_upload>)Session["SS_LOAN_LIST_OBJECT"];

            bool isExistCust = false;
            tranDate = DateTime.Now;
            string coverPeriodType = ddlCoverPeriodType.SelectedItem.Text;
            string payPeriodType = ddlPayPeriodType.SelectedItem.Text;
            coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
            payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

            string paymentModeEn = Helper.GetPaymentModeEnglish(payMode);
            string paymentModeKh = Helper.GetPaymentModeInKhmer(payMode);

            if (da_micro_group_loan_upload.SaveMoneyTransfer(userName, tranDate, ddlChannelItem.SelectedValue))
            {
                //copy file
                File.Copy(MyTempFilePath, MyFilePath, true);

                foreach (bl_money_transfer_upload l in tranMoney) //loop loan records
                {
                    tranDate = DateTime.Now;
                    loopStep += 1;

                    cus = new bl_micro_group_customer();
                    //check existing customer

                    cus = da_micro_group_customer.Get(l.IdType, l.IdNumber, PrivateGroupMasterProduct.GroupCode);
                    if (cus.CUSTOMER_NUMBER != null)
                    {
                        isSaveCus = true;
                        isExistCust = true;
                    }
                    else//new customer
                    {
                        isExistCust = false;
                        bl_micro_group_customer.LastSequence objCustLastSeq = da_micro_group_customer.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);
                        cus = new bl_micro_group_customer(PrivateGroupMasterProduct.GroupCode);
                        cus.SEQ = Convert.ToInt32(objCustLastSeq.SequenceNumber+1);// cus.LAST_SEQ + 1;
                        if (pre.PREFIX2 == objCustLastSeq.Prefix)// cus.LAST_PREFIX)//in the same year
                        {
                            cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        }
                        else
                        {
                            cus.SEQ = 1;
                            cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        }
                        cus.CUSTOMER_NUMBER = cusNo;

                        string[] breakName = Helper.BreakFullName(l.ClientName, ' ');

                        if (Helper.CheckContentUnicode(l.ClientName))
                        {
                            cus.FULL_NAME_KH = l.ClientName;
                            cus.FIRST_NAME_KH = breakName[1];
                            cus.LAST_NAME_KH = breakName[0];
                            cus.FULL_NAME_EN = "";
                            cus.FIRST_NAME_EN = "";
                            cus.LAST_NAME_EN = "";
                        }
                        else
                        {
                            cus.FULL_NAME_EN = l.ClientName;
                            cus.FIRST_NAME_EN = breakName[1];
                            cus.LAST_NAME_EN = breakName[0];
                            cus.FULL_NAME_KH = "";
                            cus.FIRST_NAME_KH = "";
                            cus.LAST_NAME_KH = "";
                        }


                        cus.GENDER = l.Gender;
                        cus.DOB = l.DOB;
                        cus.ID_TYPE = l.IdType;
                        cus.ID_NUMBER = l.IdNumber;
                        cus.OCCUPATION = "NA";
                        cus.STATUS = 1;
                        cus.CREATED_BY = userName;
                        cus.CREATED_ON = tranDate;
                        cus.MARITAL_STATUS = "";
                        isSaveCus = da_micro_group_customer.Save(cus);
                    }

                    if (isSaveCus)
                    {

                        appNumberPrefix = da_group_micro_application_number.GetLastSeq(PrivateGroupMasterProduct.GroupCode);
                        bl_group_micro_application_number appNumber = new bl_group_micro_application_number()
                        {
                            Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION_NUMBER" }, { "FIELD", "ID" } }),
                            ChannelId = ddlChannel.SelectedValue,
                            ChannelItemId = ddlChannelItem.SelectedValue,
                            ChannelLocationId = ddlChannelLocation.SelectedValue,
                            GroupCode = PrivateGroupMasterProduct.GroupCode,
                            CreatedBy = userName,
                            CreatedOn = DateTime.Now
                        };

                        if (appNumberPrefix.PrefixYear == appPrefix.Prefix2)//in same year
                        {
                            appNumber.Seq = appNumberPrefix.Seq + 1;
                        }
                        else
                        {
                            appNumber.Seq = 1;
                        }
                        appNumber.ApplicationNumber = appPrefix.Prefix1 + appPrefix.Prefix2 + appNumber.Seq.ToString(appPrefix.Digits);

                        if (da_group_micro_application_number.Save(appNumber))
                        {

                            bool saveAppTran = Transaction.GroupMirco.ApplicationNumber.BackUp(new Transaction.GroupMirco.ApplicationNumber.Tran()
                            {
                                ApplicationNumber = appNumber.ApplicationNumber,
                                TranBy = userName,
                                TranDate = tranDate,
                                TranType = "INSERT"
                            });
                            if (saveAppTran)
                            {
                                isSaveSuccess = true;
                            }
                            else
                            {
                                isSaveSuccess = false;
                                message = "Backup application fail.";
                                break;
                            }

                            #region rate and discount
                            double basicDiscount = 0.0;
                            double riderDiscount = 0.0;
                            double basicPremiumByMode = 0.0;
                            double basicAnnualPremium = 0.0;
                            double totalBisicAmount = 0;
                            double totalRiderAmount = 0;
                            double riderPremiumByMode = 0.0;
                            double riderAnnualPremium = 0.0;
                            double[] riderPremium = new double[] { 0, 0 };

                            int custAge = Calculation.Culculate_Customer_Age(cus.DOB.ToString("dd/MM/yyyy"), l.EffectiveDate);
                            //get rate
                            proRate = new bl_micro_product_rate();
                            proRate = da_micro_product_rate.GetProductRate(proConf.Product_ID, cus.GENDER, custAge, l.SumAssured, payMode);

                            if (proRate.PRODUCT_ID == null)
                            {
                                isSaveSuccess = false;
                                message = "Product rate setup is not found.";
                                break;
                            }

                            if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                            {
                                riderRate = da_micro_product_rider_rate.GetProductRate(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                if (riderRate.PRODUCT_ID == null)
                                {
                                    isSaveSuccess = false;
                                    message = "Rider product rate setup is not found.";
                                    break;
                                }
                                riderPremium = Calculation.GetMicroProductRiderPremium(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                riderPremiumByMode = riderPremium[1];
                                riderAnnualPremium = riderPremium[0];
                            }
                            double[] premium = Calculation.GetMicroProducPremium(proConf.Product_ID, cus.GENDER, custAge, l.SumAssured, payMode);
                            basicPremiumByMode = premium[1];
                            basicAnnualPremium = premium[0];

                            if (!string.IsNullOrWhiteSpace(disConf.ProductID))
                            {
                                basicDiscount = disConf.BasicDiscountAmount;
                                riderDiscount = disConf.RiderDiscountAmount;
                            }
                            totalBisicAmount = basicPremiumByMode - basicDiscount;
                            totalRiderAmount = riderPremiumByMode - riderDiscount;
                            #endregion rate and discount

                            #region save application
                            string appAddressEn = "";
                            string appAddressKh = "";

                            app = new bl_group_micro_application()
                            {
                                Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION" }, { "FIELD", "ID" } }),
                                ApplicationNumber = appNumber.ApplicationNumber,
                                ApplicationDate = l.EffectiveDate,
                                AgentCode = agent.SaleAgentId,
                                AgentNameEn = agent.FullNameEn,
                                AgentNameKh = agent.FullNameKh,
                                IdType = l.IdType,
                                IdEn = l.IdTypeText,
                                IdKh = Helper.GetIDCardTypeTextKh(cus.ID_TYPE),
                                IdNo = l.IdNumber,
                                FirstNameInEnglish = cus.FIRST_NAME_EN,
                                LastNameInEnglish = cus.LAST_NAME_EN,
                                FirstNameInKhmer = cus.FIRST_NAME_KH,
                                LastNameInKhmer = cus.LAST_NAME_KH,
                                Gender = l.Gender,
                                GenderEn = l.GenderText,
                                GenderKh = Helper.GetGenderText(l.Gender, false, true),
                                BirthOfDate = l.DOB,
                                Nationality = "",
                                MaritalStatus = "",
                                MaritalStatusKh = "",
                                Occupation = "NA",
                                OccupationKh = "",
                                PhoneNumber = l.ContactNumber,
                                Email = "",
                                Address = appAddressEn,
                                AddressKh = appAddressKh,
                                Province = "",
                                ProvinceKh = "",
                                ProductId = proConf.Product_ID,
                                ProductName = proInfo.En_Title,
                                ProductNameKh = proInfo.Kh_Title,
                                SumAssure = l.SumAssured,
                                CoverPeriodType = coverPeriodType,
                                PayPeriodType = payPeriodType,
                                TermOfCover = coverPeriod,
                                PaymentPeriod = payPeriod,
                                PayMode = payMode,
                                PayModeEn = paymentModeEn,
                                PayModeKh = paymentModeKh,
                                Premium = basicPremiumByMode,
                                AnnualPremium = basicAnnualPremium,
                                UserPremium = 0,
                                DiscountAmount = basicDiscount,
                                TotalAmount = totalBisicAmount,
                                RiderProductId = proConf.RiderProductID,
                                RiderProductName = proRiderInfo.EN_TITLE != null ? proRiderInfo.EN_TITLE : "",
                                RiderProductNameKh = proRiderInfo.KH_TITLE != null ? proRiderInfo.KH_TITLE : "",
                                RiderSumAssure = riderSA,
                                RiderPremium = riderPremiumByMode,
                                RiderAnnualPremium = riderAnnualPremium,
                                RiderDiscountAmount = riderDiscount,
                                RiderTotalAmount = totalRiderAmount,
                                BenFullName = "NA",
                                BenAge = "-",
                                BenAddress = "NA",
                                PercentageShared = 0,
                                Relation = "NA",
                                RelationKh = "",
                                QuestionId = "6D44FA76-6B00-42D6-A4D7-ACD85986DC7C",
                                Answer = -1,
                                AnswerRemarks = "",
                                PaymentCode = "",
                                Referrer = "",
                                ReferrerId = ""

                            };

                            isSaveSuccess = da_group_micro_application.Save(app);
                            if (isSaveSuccess)
                            {
                                bool saveTranApp = Transaction.GroupMirco.Application.BackUp(new Transaction.GroupMirco.Application.Tran()
                                {
                                    ApplicationNumber = appNumber.ApplicationNumber,
                                    TranBy = userName,
                                    TranDate = tranDate,
                                    TranType = "INSERT"
                                });
                                if (!saveTranApp)
                                {
                                    isSaveSuccess = false;
                                    message = "Save application fail.";
                                    break;
                                }
                            }
                            else
                            {
                                isSaveSuccess = false;
                                message = "Save application fail.";
                                break;
                            }
                            #endregion save applicaiton
                            #region Save Contact

                            if (!isExistCust)
                            {
                                isSaveContact = da_micro_group_customer.Contact.Save(new bl_micro_group_customer_contact()
                                {
                                    CUSTOMER_ID = cus.ID,
                                    PHONE_NUMBER1 = l.ContactNumber,
                                    CREATED_BY = userName,
                                    CREATED_ON = tranDate
                                });
                            }
                            else
                            {
                                isSaveContact = true;
                            }

                            if (!isSaveContact)
                            {
                                isSaveSuccess = false;
                                message = da_micro_group_customer.MESSAGE;
                                break;
                            }
                            else
                            {
                                #region Save Address
                                if (!isExistCust)
                                {
                                    isSaveAddress = da_micro_group_customer.Address.Save(
                                            new bl_address()
                                            {
                                                CUSTOMER_ID = cus.ID,
                                                ADDRESS_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CUSTOMER_ADDRESS" }, { "FIELD", "ADDRESS_ID" } }),
                                                ADDRESS1 = "NA",
                                                CREATED_BY = userName,
                                                CREATED_ON = tranDate
                                            }
                                            );
                                }
                                else
                                {
                                    isSaveAddress = true;
                                }
                                if (!isSaveAddress)
                                {
                                    isSaveSuccess = false;
                                    message = da_micro_group_customer.MESSAGE;
                                    break;
                                }
                                else
                                {
                                    #region policy
                                    string pol_number = "";
                                    pol = new bl_micro_group_policy(PrivateGroupMasterProduct.GroupCode);

                                    //pol.SEQ = pol.LAST_SEQ + 1;
                                    //if (pol.LAST_PREFIX == polPre.PREFIX2)//in the same year
                                    //{
                                    //    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);
                                    //}
                                    //else
                                    //{
                                    //    pol.SEQ = 1;
                                    //    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);
                                    //}

                                    bl_micro_group_policy.LastSequence objLastSeq = da_micro_group_policy.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);

                                    if (objLastSeq.Prefix == polPre.PREFIX2)//in the same year
                                    {
                                        pol.SEQ = objLastSeq.LastSequenceNumber + 1;

                                    }
                                    else
                                    {
                                        pol.SEQ = 1;
                                    }
                                    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);


                                    if (loopStep == 1)
                                    {
                                        firstPolicyNo = pol_number;
                                    }
                                    else if (loopStep == tranMoney.Count)
                                    {
                                        lastPolicyNo = pol_number;
                                    }

                                    pol.ApplicationId = app.Id;
                                    pol.PolicyNumber = pol_number;
                                    pol.CustomerId = cus.ID;
                                    pol.GroupMasterId = gMaster.GroupMasterID;
                                    pol.LoanId = l.ID;
                                    pol.ProductId = proConf.Product_ID;
                                    pol.Currency = l.Currency;
                                    pol.ExchangeRate = l.ExchangeRate;
                                    pol.PolicyStatus = "IF";
                                    pol.AgentCode = txtSaleAgentID.Text.Trim();
                                    pol.ChannelId = ddlChannel.SelectedValue;
                                    pol.ChannelItemId = ddlChannelItem.SelectedValue;
                                    pol.ChannelLocationId = ddlChannelLocation.SelectedValue;
                                    pol.createdBy = userName;
                                    pol.CreatedOn = tranDate;
                                    isSavePolicy = da_micro_group_policy.Save(pol);
                                    if (!isSavePolicy)
                                    {
                                        isSaveSuccess = false;
                                        message = da_micro_group_policy.MESSAGE;
                                        break;
                                    }
                                    else
                                    {
                                        //backup policy
                                        Transaction.GroupMirco.Policy.Backup(new Transaction.GroupMirco.Policy.Tran() { PolicyId = pol.PolicyId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                        //count success policy
                                        countPolicy += 1;

                                        #region save approver

                                        //save approver
                                        List<da_report_approver.bl_report_approver> approver = new List<da_report_approver.bl_report_approver>();
                                        approver = da_report_approver.GetApproverList();

                                        foreach (da_report_approver.bl_report_approver ap in approver.Where(_ => _.NameEn.ToUpper() == AppConfiguration.GetApplicationApprover().ToUpper()))
                                        {
                                            isSaveSuccess = da_report_approver.InsertApproverPolicy(new da_report_approver.bl_report_approver_policy()
                                            {
                                                Approver_ID = ap.ID,
                                                Policy_ID = pol.PolicyId,
                                                Created_By = userName,
                                                Created_On = tranDate
                                            });
                                            break;

                                        }

                                        if (!isSaveSuccess)
                                        {
                                            message = "Saved Approver fail.";
                                            break;
                                        }
                                        #endregion save approver

                                        #region Beneficiary
                                        beneficiary = new bl_micro_policy_beneficiary()
                                        {
                                            ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_BENEFICIARY" }, { "FIELD", "BENEFICIARY_ID" } }),
                                            POLICY_ID = pol.PolicyId,
                                            FULL_NAME = "NA",
                                            Gender = "-1",
                                            AGE = "-",
                                            ADDRESS = "NA",
                                            PERCENTAGE_OF_SHARE = 0,
                                            RELATION = "NA",
                                            CREATED_BY = userName,
                                            CREATED_ON = tranDate,
                                            REMARKS = ""

                                        };
                                        isSaveBen = da_micro_group_policy_beneficiary.Save(beneficiary);
                                        if (!isSaveBen)
                                        {
                                            isSaveSuccess = false;
                                            message = da_micro_group_policy_beneficiary.MESSAGE;
                                            break;
                                        }
                                        else
                                        {
                                            isSaveSuccess = true;
                                            //backup bendeficiary
                                            Transaction.GroupMirco.Beneficiary.Backup(new Transaction.GroupMirco.Beneficiary.Tran() { BeneficiaryId = beneficiary.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                            #region Policy Rider
                                            string policyRiderid = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_RIDER" }, { "FIELD", "POLICY_RIDER_ID" } });
                                            if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                                            {
                                                double premRielRider = pol.ExchangeRate > 0 ? Math.Round(riderPremiumByMode * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0;
                                                if (premRielRider > 0)
                                                {
                                                    int leR = premRielRider.ToString().Length;
                                                    string bR = premRielRider.ToString().Substring(leR - 2, 2);

                                                    if (Convert.ToDouble(bR) < 50)
                                                    {
                                                        //round down
                                                        premRielRider = premRielRider - Convert.ToDouble(bR);
                                                    }
                                                    else if (Convert.ToDouble(bR) >= 50)
                                                    {
                                                        premRielRider = (premRielRider - Convert.ToDouble(bR)) + 100;
                                                    }
                                                }

                                                isSaveSuccess = da_micro_group_policy_rider.Save(new bl_micro_group_policy_rider()
                                                {

                                                    PolicyRiderId = policyRiderid,
                                                    PolicyId = pol.PolicyId,
                                                    ProductId = proConf.RiderProductID,
                                                    SumAssured = riderSA,
                                                    PremiumRate = riderRate.RATE,
                                                    Premium = riderPremiumByMode,
                                                    PremiumRiel = premRielRider,
                                                    AnnualPremium = riderAnnualPremium,
                                                    DiscountAmount = riderDiscount,
                                                    RiderStatus = "IF",
                                                    TotalAmount = riderPremiumByMode - riderDiscount,
                                                    CreatedBy = userName,
                                                    CreatedOn = tranDate,
                                                    Remarks = ""
                                                });

                                                if (isSaveSuccess)
                                                {
                                                    isSaveSuccess = Transaction.GroupMirco.PolicyRider.BackUp(new Transaction.GroupMirco.PolicyRider.Tran()
                                                    {
                                                        PolicyRiderId = policyRiderid,
                                                        TranBy = userName,
                                                        TranDate = tranDate,
                                                        TranType = "INSERT"
                                                    });
                                                    if (!isSaveSuccess)
                                                    {
                                                        message = "Backup Policy Rider fail.";
                                                    }

                                                }
                                                else
                                                {
                                                    message = da_micro_group_policy_rider.MESSAGE;

                                                }
                                                if (!isSaveSuccess)
                                                {
                                                    message = "Backup Policy Rider fail.";

                                                }
                                            }
                                            #endregion Policy Rider

                                            if (!isSaveSuccess)
                                            {

                                                break;
                                            }


                                            polDe = new bl_micro_group_policy_detail();
                                            polDe.PolicyID = pol.PolicyId;
                                            polDe.IssuedDate = l.IssuedDate;
                                            polDe.EffectivedDate = l.EffectiveDate;

                                            polDe.CoverPeriodType = ddlCoverPeriodType.SelectedItem.Text;
                                            polDe.PayPeriodType = ddlPayPeriodType.SelectedItem.Text;

                                            coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
                                            payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

                                            polDe.CoverYear = coverPeriod;
                                            polDe.PayYear = payPeriod;

                                            if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddYears(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }
                                            else if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddMonths(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }
                                            else if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.D.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddDays(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }


                                            polDe.Age = custAge;

                                            polDe.SumAssured = pol.Currency.ToUpper() == "USD" ? l.SumAssured : Math.Round(l.SumAssured / pol.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                                            polDe.PayMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                                            polDe.PaymentCode = "";

                                            polDe.PremiumRate = proRate.RATE;
                                            polDe.Premium = Math.Round(premium[1], 2, MidpointRounding.AwayFromZero);

                                            double premRiel = pol.ExchangeRate > 0 ? Math.Round(polDe.Premium * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0;

                                            if (premRiel > 0)
                                            {
                                                int le = premRiel.ToString().Length;
                                                string b = premRiel.ToString().Substring(le - 2, 2);
                                                if (Convert.ToDouble(b) < 50)
                                                {
                                                    //round down
                                                    premRiel = premRiel - Convert.ToDouble(b);
                                                }
                                                else if (Convert.ToDouble(b) >= 50)
                                                {
                                                    premRiel = (premRiel - Convert.ToDouble(b)) + 100;
                                                }
                                            }
                                            //check discount 

                                            if (disConf.ProductID != null)
                                            {
                                                polDe.DiscountAmount = disConf.BasicDiscountAmount;

                                            }
                                            else
                                            {
                                                polDe.DiscountAmount = 0;
                                            }

                                            polDe.TotalAmount = polDe.Premium - polDe.DiscountAmount;
                                            polDe.PremiumRiel = premRiel;
                                            polDe.AnnualPremium = Math.Round(premium[0], 2, MidpointRounding.AwayFromZero); ;

                                            polDe.PolicyStatusRemarks = "NEW";
                                            polDe.RenewalFrom = "";
                                            polDe.FrequencyReduceYear = 0;
                                            polDe.ReduceRate = 0;
                                            polDe.CreatedBy = userName;
                                            polDe.CreatedOn = tranDate;

                                            isSavePolicyDetail = da_micro_group_policy_detail.Save(polDe);
                                            if (!isSavePolicyDetail)
                                            {
                                                isSaveSuccess = false;
                                                message = da_micro_group_policy_detail.MESSAGE;
                                                break;
                                            }
                                            else
                                            {
                                                //backup policy detail
                                                Transaction.GroupMirco.PolicyDetail.Backup(new Transaction.GroupMirco.PolicyDetail.Tran() { PolicyDetailId = polDe.PolicyDetailId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                                #region payment
                                                pay = new bl_micro_group_policy_payment()
                                                {
                                                    PolicyDetailId = polDe.PolicyDetailId,
                                                    PayMode = payMode,
                                                    UserPremium = polDe.Premium,
                                                    Amount = polDe.Premium,
                                                    DiscountAmount = polDe.DiscountAmount,
                                                    TotalAmount = polDe.TotalAmount,
                                                    AmountRiel = polDe.PremiumRiel,
                                                    DueDate = polDe.EffectivedDate,
                                                    NextDueDate = payMode == 1 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddYears(1), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 2 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(6), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 3 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(3), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 4 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(1), polDe.EffectivedDate, polDe.EffectivedDate) : new DateTime(1900, 1, 1),
                                                    PayDate = new DateTime(1900, 1, 1),
                                                    ReturnAmount = (polDe.Premium - polDe.Premium),
                                                    PolicyStatus = "IF",
                                                    PremiumYear = 1,
                                                    PremiumLot = 1,
                                                    OfficeId = "HQ",
                                                    TransactionType = "",
                                                    TransactionRef = "",
                                                    CreatedBy = userName,
                                                    CreatedOn = tranDate,
                                                    ReportDate = reportDate
                                                };

                                                totalAmount += pay.Amount;
                                                totalDiscount += pay.DiscountAmount;
                                                grandTotalAmount += pay.TotalAmount;
                                                totalReturnAmount += pay.ReturnAmount;

                                                totalPremium += polDe.Premium;
                                                totalAnnualPremium += polDe.AnnualPremium;
                                                totalSA += polDe.SumAssured;


                                                isSavePayment = da_micro_group_policy_payment.Save(pay);
                                                if (!isSavePayment)
                                                {
                                                    isSaveSuccess = false;
                                                    message = da_micro_group_policy_payment.MESSAGE;
                                                    break;
                                                }
                                                else
                                                {
                                                    #region save certificate
                                                    isSaveSuccess = da_group_micro_certificate.Save(new bl_group_micro_certificate()
                                                    {
                                                        Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CERTIFICATE" }, { "FIELD", "ID" } }),

                                                        CustomerNo = cus.CUSTOMER_NUMBER,
                                                        PolicyNumber = pol.PolicyNumber,
                                                        AgentCode = app.AgentCode,
                                                        AgentNameEn = app.AgentNameEn,
                                                        AgentNameKh = app.AgentNameKh,
                                                        IdType = app.IdType,
                                                        IdEn = app.IdEn,
                                                        IdKh = app.IdKh,
                                                        IdNo = app.IdNo,
                                                        FullName = l.ClientName,
                                                        Gender = app.Gender,
                                                        GenderEn = app.GenderEn,
                                                        GenderKh = app.GenderKh,
                                                        DateOfBirth = app.BirthOfDate,
                                                        Age = polDe.Age,
                                                        Nationality = app.Nationality,
                                                        Address = "NA",
                                                        Province = app.Province,
                                                        ProductId = app.ProductId,
                                                        ProductName = app.ProductName,
                                                        ProductNameKh = app.ProductNameKh,
                                                        SumAssure = app.SumAssure,
                                                        CoverPeriodType = app.CoverPeriodType,
                                                        PayPeriodType = app.PayPeriodType,
                                                        TermOfCover = app.TermOfCover,
                                                        PaymentPeriod = app.PaymentPeriod,
                                                        PayMode = app.PayMode,
                                                        PayModeEn = app.PayModeEn,
                                                        PayModeKh = app.PayModeKh,
                                                        Premium = app.Premium,
                                                        AnnualPremium = app.AnnualPremium,
                                                        UserPremium = app.UserPremium,
                                                        DiscountAmount = app.DiscountAmount,
                                                        TotalAmount = app.TotalAmount,
                                                        RiderProductId = app.RiderProductId,
                                                        RiderProductName = app.RiderProductName,
                                                        RiderProductNameKh = app.RiderProductNameKh,
                                                        RiderSumAssure = app.RiderSumAssure,
                                                        RiderPremium = app.RiderPremium,
                                                        RiderAnnualPremium = app.RiderAnnualPremium,
                                                        RiderDiscountAmount = app.RiderDiscountAmount,
                                                        RiderTotalAmount = app.RiderTotalAmount,
                                                        EffectiveDate = polDe.EffectivedDate,
                                                        ExpiryDate = polDe.ExpiryDate,
                                                        NextDueDate = pay.NextDueDate
                                                    });
                                                    if (isSaveSuccess)
                                                    {
                                                        bool saveCert = Transaction.GroupMirco.Certificate.BackUp(new Transaction.GroupMirco.Certificate.Tran() { PolicyNumber = pol.PolicyNumber, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                                        if (!saveCert)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = "Backup certificate fail.";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSaveSuccess = false;
                                                        message = "Save certificate fail.";
                                                        break;
                                                    }
                                                    #endregion save certificate

                                                    //save policy payment transaction 
                                                    Transaction.GroupMirco.PolicyPayment.Backup(new Transaction.GroupMirco.PolicyPayment.Tran()
                                                    {
                                                        PolicyPaymentId = pay.PolicyPaymentId,
                                                        TranBy = userName,
                                                        TranDate = tranDate,
                                                        TranType = "INSERT"

                                                    });

                                                    #region bunch payment summary
                                                    if (loopStep == 1) // save only first loop
                                                    {
                                                        bunchSummary = new bl_micro_group_policy_payment_bunch.summary()
                                                        {
                                                            GroupMasterCode = gMaster.GroupCode,
                                                            Amount = totalAmount,
                                                            DisountAmount = totalDiscount,
                                                            TotalAmount = grandTotalAmount,
                                                            ReturnAmount = totalReturnAmount,
                                                            Status = 0,//pending to pay 1 is paid,
                                                            PaymentType = "NEW",
                                                            CreatedBy = userName,
                                                            CreatedOn = tranDate
                                                        };
                                                        isSavePaymentBunchSummary = da_micro_group_policy_payment_bunch.summary.Save(bunchSummary);
                                                    }

                                                    #endregion bunch payment summary
                                                    if (!isSavePaymentBunchSummary)
                                                    {
                                                        isSaveSuccess = false;
                                                        message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //BACKUP payment summary
                                                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran()
                                                        {
                                                            BunchId = bunchSummary.BunchId,
                                                            TranBy = userName,
                                                            TranDate = tranDate,
                                                            TranType = "INSERT"
                                                        });

                                                        #region bunch payment detail
                                                        bunchDetail = new bl_micro_group_policy_payment_bunch.detail()
                                                        {
                                                            BunchId = bunchSummary.BunchId,
                                                            PolicyPaymentId = pay.PolicyPaymentId
                                                        };
                                                        isSavePaymentBunchDetail = da_micro_group_policy_payment_bunch.detail.Save(bunchDetail);
                                                        if (!isSavePaymentBunchDetail)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            //save transaction payment bunch detail
                                                            Transaction.GroupMirco.PaymentBunchDetail.Backup(new Transaction.GroupMirco.PaymentBunchDetail.Tran()
                                                            {
                                                                BunchDetailId = bunchDetail.BunchDetailId,
                                                                TranBy = userName,
                                                                TranDate = tranDate,
                                                                TranType = "INSERT"
                                                            });

                                                        }
                                                        #endregion bunch payment detail
                                                    }
                                                }
                                                #endregion payment
                                            }
                                        }
                                        #endregion Beneficiary
                                    }
                                    #endregion policy
                                }
                                #endregion Save Address
                            }
                            #endregion Save Conctact
                        }
                        else//save application fail
                        {
                            isSaveSuccess = false;
                            message = "Saved application number is fail.";
                            break;
                        }


                    }
                    else
                    {
                        isSaveSuccess = false;
                        message = "Saved Customer fail.";
                        break;
                    }
                }//END LOOP LOAN LIST
            #endregion save customer

                if (isSaveSuccess)
                {
                    var a = bunchSummary;
                    a.Amount = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                    a.DisountAmount = Math.Round(totalDiscount, 2, MidpointRounding.AwayFromZero); ;
                    a.TotalAmount = Math.Round(grandTotalAmount, 2, MidpointRounding.AwayFromZero); ;
                    a.ReturnAmount = Math.Round(totalReturnAmount, 2, MidpointRounding.AwayFromZero);
                    a.NumberPolicy = countPolicy;
                    a.ReportDate = reportDate;
                    a.UpdatedBy = userName;
                    a.UpdatedOn = tranDate;

                    if (!da_micro_group_policy_payment_bunch.summary.Update(a))
                    {
                        isSaveSuccess = false;
                    }
                    else
                    {
                        //back up payment buch summary
                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran() { BunchId = a.BunchId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });



                        //if (da_pma_report.Save(new bl_pma_report()
                        //{
                        //    PolicyNumber = gMaster.GroupCode,
                        //    EffectiveDate = gMaster.EffectiveDate,
                        //    PayDate = a.ReportDate,
                        //    Amount = a.Amount,
                        //    DiscountAmount = a.DisountAmount,
                        //    EM = 0,
                        //    Premium = totalPremium,
                        //    AnnualPremium = totalAnnualPremium,
                        //    PayYear = pay.PremiumYear,
                        //    PayLot = pay.PremiumLot,
                        //    PayMode = pay.PayModeText,
                        //    ProductCode = proConf.Plan_Code,
                        //    ProductId = proConf.Product_ID,
                        //    ProductName = proConf.En_Title,
                        //    ReportDate = a.ReportDate,
                        //    CreatedBy = userName,
                        //    CreatedOn = tranDate,
                        //    SA = totalSA,
                        //    NumberPolicy = a.NumberPolicy,
                        //    PolicyRange = firstPolicyNo + "-" + lastPolicyNo
                        //}))
                        //{
                        //    Transaction.GroupMirco.PMA.BackUp(new Transaction.GroupMirco.PMA.Tran() { ID = a.BunchId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                        //}
                        //else
                        //{
                        //    isSaveSuccess = false;
                        //}

                        bl_pma_report pma = new bl_pma_report()
                        {
                            PolicyNumber = gMaster.GroupCode,
                            EffectiveDate = gMaster.EffectiveDate,
                            PayDate = a.ReportDate,
                            Amount = a.Amount,
                            DiscountAmount = a.DisountAmount,
                            EM = 0,
                            Premium = totalPremium,
                            AnnualPremium = totalAnnualPremium,
                            PayYear = pay.PremiumYear,
                            PayLot = pay.PremiumLot,
                            PayMode = pay.PayModeText,
                            ProductCode = proConf.Plan_Code,
                            ProductId = proConf.Product_ID,
                            ProductName = proConf.En_Title,
                            ReportDate = a.ReportDate,
                            CreatedBy = userName,
                            CreatedOn = tranDate,
                            SA = totalSA,
                            NumberPolicy = a.NumberPolicy,
                            PolicyRange = firstPolicyNo + "-" + lastPolicyNo
                        };

                        if (da_pma_report.Save(pma))
                        {
                            isSaveSuccess = Transaction.GroupMirco.PMA.BackUp(new Transaction.GroupMirco.PMA.Tran() { ID = pma.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                        }
                        else
                        {
                            isSaveSuccess = false;
                        }


                    }
                    btnSave.Enabled = false;
                }

                //popup message
                if (isSaveSuccess)
                {
                    da_micro_group_loan_upload.DeleteTempRecords(userName, tranDate);

                    //url
                    string url = string.Format("../Reports/group_micro_policy_detail_req.aspx?CHID={0}&FDATE={1}&TDATE={2}&PROID={3} ", ddlChannelItem.SelectedValue, txtPaymentReportDate.Text, txtPaymentReportDate.Text, ddlProduct.SelectedValue);
                    url = "<a href='" + url + " ' target='_blank'><span>Click here to show report</span></a>";

                    Helper.Alert(false, "Saved successfully.<br/>" + url, lblError);
                    btnUpload.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                    if (File.Exists(MyFilePath))
                    {
                        File.Delete(MyFilePath);

                    }
                    if (RoleBack(userName, tranDate))
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
                    }
                }


            }
            else
            { //save loan fail
                Helper.Alert(true, "Saved fail. Error:" + da_micro_group_loan_upload.MESSAGE, lblError);

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnSave_Click()] in page [frm_gmCustomer_upload], detail:" + ex.Message + "=>" + ex.StackTrace);
            if (RoleBack(userName, tranDate))
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
            }
            else
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
            }
            btnSave.Enabled = false;
        }

        //Delete temp file
        if (File.Exists(MyTempFilePath))
        {
            File.Delete(MyTempFilePath);
        }
    }
    protected void SaveWingPayroll()
    {
        bl_micro_customer_prefix pre = new bl_micro_customer_prefix();
        bl_micro_group_policy_prefix polPre = new bl_micro_group_policy_prefix();
        bl_micro_product_config proConf = MyProductConfig;
        bl_micro_product_rate proRate;
        bl_micro_product_discount_config disConf = new bl_micro_product_discount_config();
        disConf = da_micro_product_config.DiscountConfig.GetProductDiscount(proConf.Product_ID, proConf.RiderProductID, 0, 0, "SELF");
        bl_micro_product_rider_rate riderRate = new bl_micro_product_rider_rate();
        bl_product proInfo = new bl_product();
        proInfo = da_product.GetProductByProductID(proConf.Product_ID);
        bl_micro_product_rider proRiderInfo = new bl_micro_product_rider();
        proRiderInfo = da_micro_product_rider.GetMicroProductByProductID(proConf.RiderProductID);
        DateTime tranDate = DateTime.Now;

        int payMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
        bool isSaveCus = false;
        bool isSaveAddress = false;
        bool isSaveContact = false;
        bool isSavePolicy = false;
        bool isSaveBen = false;
        bool isSavePolicyDetail = false;
        bool isSavePayment = false;
        bool isSavePaymentBunchSummary = false;
        bool isSavePaymentBunchDetail = false;
        double totalAmount = 0;
        double totalDiscount = 0;
        double grandTotalAmount = 0;
        double totalReturnAmount = 0;
        double totalAnnualPremium = 0;
        double totalPremium = 0;
        double totalSA = 0;
        int loopStep = 0;
        int countPolicy = 0;
        DateTime reportDate;
        bool isSaveSuccess = true;
        string message = "";
        string firstPolicyNo = "";
        string lastPolicyNo = "";
        double riderSA = ddlRiderSumAssure.Items.Count > 0 ? Convert.ToDouble(ddlRiderSumAssure.SelectedValue) : 0;

        int coverPeriod = 0;
        int payPeriod = 0;

        try
        {
            bl_micro_group_policy pol;
            bl_micro_group_policy_detail polDe;
            bl_micro_group_policy_payment pay = new bl_micro_group_policy_payment();
            bl_micro_group_policy_payment_bunch.summary bunchSummary = new bl_micro_group_policy_payment_bunch.summary();
            bl_micro_group_policy_payment_bunch.detail bunchDetail;
            bl_micro_group_invoice.summary invoiceSummary = new bl_micro_group_invoice.summary();
            bl_micro_policy_beneficiary beneficiary;

            #region save customer
            bl_micro_group_customer cus;
            pre = da_micro_group_customer_prefix.GetLastCustomerPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_group_master_product> gMasterList = new List<bl_group_master_product>();
            gMasterList = da_group_master_product.GetGroupMasterProductList(PrivateGroupMasterProduct.ProductID, ddlChannelItem.SelectedValue);
            var gMaster = gMasterList[0];

            polPre = da_micro_group_policy_prefix.GetLastPolicyPrefix(gMaster.GroupCode);

            string cusNo = "";
            reportDate = Helper.FormatDateTime(txtPaymentReportDate.Text.Trim());

            bl_group_micro_application app = new bl_group_micro_application();
            bl_group_micro_application_number appNumberPrefix = new bl_group_micro_application_number();
            bl_group_micro_application_prefix appPrefix = new bl_group_micro_application_prefix();
            appPrefix = da_group_micro_application_prefix.GetLastPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_wing_payroll_upload> wingList = (List<bl_wing_payroll_upload>)Session["SS_LOAN_LIST_OBJECT"];

            bool isExistCust = false;
            tranDate = DateTime.Now;

            string coverPeriodType = ddlCoverPeriodType.SelectedItem.Text;
            string payPeriodType = ddlPayPeriodType.SelectedItem.Text;
            coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
            payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

            string paymentModeEn = Helper.GetPaymentModeEnglish(payMode);
            string paymentModeKh = Helper.GetPaymentModeInKhmer(payMode);

            if (da_micro_group_loan_upload.SaveWingPayroll(userName, tranDate, ddlChannelItem.SelectedValue))
            {
                //copy file
                File.Copy(MyTempFilePath, MyFilePath, true);

                foreach (bl_wing_payroll_upload l in wingList) //loop wing payroll records
                {
                    tranDate = DateTime.Now;
                    loopStep += 1;

                    cus = new bl_micro_group_customer();
                    //check existing customer

                    cus = da_micro_group_customer.Get(l.IdType, l.IdNumber, PrivateGroupMasterProduct.GroupCode);
                    if (cus.CUSTOMER_NUMBER != null)
                    {
                        isSaveCus = true;
                        isExistCust = true;
                    }
                    else//new customer
                    {
                        isExistCust = false;
                        bl_micro_group_customer.LastSequence objCustLastSeq = da_micro_group_customer.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);
                        cus = new bl_micro_group_customer(PrivateGroupMasterProduct.GroupCode);
                        cus.SEQ = Convert.ToInt32(objCustLastSeq.SequenceNumber + 1);// cus.LAST_SEQ + 1;
                        if (pre.PREFIX2 == objCustLastSeq.Prefix)// cus.LAST_PREFIX)//in the same year
                        {
                            cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        }
                        else
                        {
                            cus.SEQ = 1;
                            cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        }
                        cus.CUSTOMER_NUMBER = cusNo;

                        //string[] breakName = Helper.BreakFullName(l.ClientName, ' ');

                        if (Helper.CheckContentUnicode(l.ClientName))
                        {
                            cus.FULL_NAME_KH = l.ClientName;
                            cus.FIRST_NAME_KH = l.FirstName;// breakName[1];
                            cus.LAST_NAME_KH = l.LastName;// breakName[0];
                            cus.FULL_NAME_EN = "";
                            cus.FIRST_NAME_EN = "";
                            cus.LAST_NAME_EN = "";
                        }
                        else
                        {
                            cus.FULL_NAME_EN = l.ClientName;
                            cus.FIRST_NAME_EN = l.FirstName;// breakName[1];
                            cus.LAST_NAME_EN = l.LastName;// breakName[0];
                            cus.FULL_NAME_KH = "";
                            cus.FIRST_NAME_KH = "";
                            cus.LAST_NAME_KH = "";
                        }


                        cus.GENDER = l.Gender;
                        cus.DOB = l.DOB;
                        cus.ID_TYPE = l.IdType;
                        cus.ID_NUMBER = l.IdNumber;
                        cus.OCCUPATION = "NA";
                        cus.STATUS = 1;
                        cus.CREATED_BY = userName;
                        cus.CREATED_ON = tranDate;
                        cus.MARITAL_STATUS = "";
                        isSaveCus = da_micro_group_customer.Save(cus);
                    }

            #endregion save customer

                    if (isSaveCus)
                    {

                        appNumberPrefix = da_group_micro_application_number.GetLastSeq(PrivateGroupMasterProduct.GroupCode);
                        bl_group_micro_application_number appNumber = new bl_group_micro_application_number()
                        {
                            Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION_NUMBER" }, { "FIELD", "ID" } }),
                            ChannelId = ddlChannel.SelectedValue,
                            ChannelItemId = ddlChannelItem.SelectedValue,
                            ChannelLocationId = ddlChannelLocation.SelectedValue,
                            GroupCode = PrivateGroupMasterProduct.GroupCode,
                            CreatedBy = userName,
                            CreatedOn = DateTime.Now
                        };

                        if (appNumberPrefix.PrefixYear == appPrefix.Prefix2)//in same year
                        {
                            appNumber.Seq = appNumberPrefix.Seq + 1;
                        }
                        else
                        {
                            appNumber.Seq = 1;
                        }
                        appNumber.ApplicationNumber = appPrefix.Prefix1 + appPrefix.Prefix2 + appNumber.Seq.ToString(appPrefix.Digits);

                        if (da_group_micro_application_number.Save(appNumber))
                        {

                            bool saveAppTran = Transaction.GroupMirco.ApplicationNumber.BackUp(new Transaction.GroupMirco.ApplicationNumber.Tran()
                            {
                                ApplicationNumber = appNumber.ApplicationNumber,
                                TranBy = userName,
                                TranDate = tranDate,
                                TranType = "INSERT"
                            });
                            if (saveAppTran)
                            {
                                isSaveSuccess = true;
                            }
                            else
                            {
                                isSaveSuccess = false;
                                message = "Backup application fail.";
                                break;
                            }

                            #region rate and discount
                            double basicDiscount = 0.0;
                            double riderDiscount = 0.0;
                            double basicPremiumByMode = 0.0;
                            double basicAnnualPremium = 0.0;
                            double totalBisicAmount = 0;
                            double totalRiderAmount = 0;
                            double riderPremiumByMode = 0.0;
                            double riderAnnualPremium = 0.0;
                            double[] riderPremium = new double[] { 0, 0 };

                            int custAge = Calculation.Culculate_Customer_Age(cus.DOB.ToString("dd/MM/yyyy"), l.EffectiveDate);
                            //get rate
                            proRate = new bl_micro_product_rate();
                            proRate = da_micro_product_rate.GetProductRate(proConf.Product_ID, cus.GENDER, custAge, l.SumAssured, payMode);

                            if (proRate.PRODUCT_ID == null)
                            {
                                isSaveSuccess = false;
                                message = "Product rate setup is not found.";
                                break;
                            }

                            if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                            {
                                riderRate = da_micro_product_rider_rate.GetProductRate(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                if (riderRate.PRODUCT_ID == null)
                                {
                                    isSaveSuccess = false;
                                    message = "Rider product rate setup is not found.";
                                    break;
                                }
                                riderPremium = Calculation.GetMicroProductRiderPremium(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                riderPremiumByMode = riderPremium[1];
                                riderAnnualPremium = riderPremium[0];
                            }
                            double[] premium = Calculation.GetMicroProducPremium(proConf.Product_ID, cus.GENDER, custAge, l.SumAssured, payMode);
                            basicPremiumByMode = premium[1];
                            basicAnnualPremium = premium[0];

                            if (!string.IsNullOrWhiteSpace(disConf.ProductID))
                            {
                                basicDiscount = disConf.BasicDiscountAmount;
                                riderDiscount = disConf.RiderDiscountAmount;
                            }
                            totalBisicAmount = basicPremiumByMode - basicDiscount;
                            totalRiderAmount = riderPremiumByMode - riderDiscount;
                            #endregion rate and discount

                            #region save application
                            string appAddressEn = "";
                            string appAddressKh = "";

                            app = new bl_group_micro_application()
                            {
                                Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION" }, { "FIELD", "ID" } }),
                                ApplicationNumber = appNumber.ApplicationNumber,
                                ApplicationDate = l.EffectiveDate,
                                AgentCode = AgentInfo.SaleAgentId,
                                AgentNameEn = AgentInfo.FullNameEn,
                                AgentNameKh = AgentInfo.FullNameKh,
                                IdType = l.IdType,
                                IdEn = l.IdTypeText,
                                IdKh = Helper.GetIDCardTypeTextKh(cus.ID_TYPE),
                                IdNo = l.IdNumber,
                                FirstNameInEnglish = cus.FIRST_NAME_EN,
                                LastNameInEnglish = cus.LAST_NAME_EN,
                                FirstNameInKhmer = cus.FIRST_NAME_KH,
                                LastNameInKhmer = cus.LAST_NAME_KH,
                                Gender = l.Gender,
                                GenderEn = l.GenderText,
                                GenderKh = Helper.GetGenderText(l.Gender, false, true),
                                BirthOfDate = l.DOB,
                                Nationality = "",
                                MaritalStatus = "",
                                MaritalStatusKh = "",
                                Occupation = "NA",
                                OccupationKh = "",
                                PhoneNumber = l.PhoneNumber,
                                Email = "",
                                Address = appAddressEn,
                                AddressKh = appAddressKh,
                                Province = "",
                                ProvinceKh = "",
                                ProductId = proConf.Product_ID,
                                ProductName = proInfo.En_Title,
                                ProductNameKh = proInfo.Kh_Title,
                                SumAssure = l.SumAssured,
                                CoverPeriodType = coverPeriodType,
                                PayPeriodType = payPeriodType,
                                TermOfCover = coverPeriod,
                                PaymentPeriod = payPeriod,
                                PayMode = payMode,
                                PayModeEn = paymentModeEn,// Helper.GetPaymentModeEnglish(payMode),
                                PayModeKh = paymentModeKh,// Helper.GetPaymentModeInKhmer(payMode),
                                Premium = basicPremiumByMode,
                                AnnualPremium = basicAnnualPremium,
                                UserPremium = 0,
                                DiscountAmount = basicDiscount,
                                TotalAmount = totalBisicAmount,
                                RiderProductId = proConf.RiderProductID,
                                RiderProductName = proRiderInfo.EN_TITLE != null ? proRiderInfo.EN_TITLE : "",
                                RiderProductNameKh = proRiderInfo.KH_TITLE != null ? proRiderInfo.KH_TITLE : "",
                                RiderSumAssure = riderSA,
                                RiderPremium = riderPremiumByMode,
                                RiderAnnualPremium = riderAnnualPremium,
                                RiderDiscountAmount = riderDiscount,
                                RiderTotalAmount = totalRiderAmount,
                                BenFullName = "NA",
                                BenAge = "-",
                                BenAddress = "NA",
                                PercentageShared = 0,
                                Relation = "NA",
                                RelationKh = "",
                                QuestionId = "6D44FA76-6B00-42D6-A4D7-ACD85986DC7C",
                                Answer = -1,
                                AnswerRemarks = "",
                                PaymentCode = "",
                                Referrer = "",
                                ReferrerId = ""

                            };

                            isSaveSuccess = da_group_micro_application.Save(app);
                            if (isSaveSuccess)
                            {
                                bool saveTranApp = Transaction.GroupMirco.Application.BackUp(new Transaction.GroupMirco.Application.Tran()
                                {
                                    ApplicationNumber = appNumber.ApplicationNumber,
                                    TranBy = userName,
                                    TranDate = tranDate,
                                    TranType = "INSERT"
                                });
                                if (!saveTranApp)
                                {
                                    isSaveSuccess = false;
                                    message = "Save application fail.";
                                    break;
                                }
                            }
                            else
                            {
                                isSaveSuccess = false;
                                message = "Save application fail.";
                                break;
                            }
                            #endregion save applicaiton
                            #region Save Contact

                            if (!isExistCust)
                            {
                                isSaveContact = da_micro_group_customer.Contact.Save(new bl_micro_group_customer_contact()
                                {
                                    CUSTOMER_ID = cus.ID,
                                    PHONE_NUMBER1 = l.PhoneNumber,
                                    CREATED_BY = userName,
                                    CREATED_ON = tranDate
                                });
                            }
                            else
                            {
                                isSaveContact = true;
                            }

                            if (!isSaveContact)
                            {
                                isSaveSuccess = false;
                                message = da_micro_group_customer.MESSAGE;
                                break;
                            }
                            else
                            {
                                #region Save Address
                                if (!isExistCust)
                                {
                                    isSaveAddress = da_micro_group_customer.Address.Save(
                                            new bl_address()
                                            {
                                                CUSTOMER_ID = cus.ID,
                                                ADDRESS_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CUSTOMER_ADDRESS" }, { "FIELD", "ADDRESS_ID" } }),
                                                ADDRESS1 = "NA",
                                                CREATED_BY = userName,
                                                CREATED_ON = tranDate
                                            }
                                            );
                                }
                                else
                                {
                                    isSaveAddress = true;
                                }
                                #endregion Save Address

                                if (!isSaveAddress)
                                {
                                    isSaveSuccess = false;
                                    message = da_micro_group_customer.MESSAGE;
                                    break;
                                }
                                else
                                {
                                    #region policy
                                    string pol_number = "";
                                    pol = new bl_micro_group_policy(PrivateGroupMasterProduct.GroupCode);
                                    //pol.SEQ = pol.LAST_SEQ + 1;
                                    //if (pol.LAST_PREFIX == polPre.PREFIX2)//in the same year
                                    //{
                                    //    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);
                                    //}
                                    //else
                                    //{
                                    //    pol.SEQ = 1;
                                    //    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);
                                    //}

                                    bl_micro_group_policy.LastSequence objLastSeq = da_micro_group_policy.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);

                                    if (objLastSeq.Prefix == polPre.PREFIX2)//in the same year
                                    {
                                        pol.SEQ = objLastSeq.LastSequenceNumber + 1;
                                    }
                                    else
                                    {
                                        pol.SEQ = 1;
                                    }
                                    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);


                                    if (loopStep == 1)
                                    {
                                        firstPolicyNo = pol_number;
                                    }
                                    else if (loopStep == wingList.Count)
                                    {
                                        lastPolicyNo = pol_number;
                                    }

                                    pol.ApplicationId = app.Id;
                                    pol.PolicyNumber = pol_number;
                                    pol.CustomerId = cus.ID;
                                    pol.GroupMasterId = gMaster.GroupMasterID;
                                    pol.LoanId = l.ID;
                                    pol.ProductId = proConf.Product_ID;
                                    pol.Currency = l.Currency;
                                    pol.ExchangeRate = l.ExchangeRate;
                                    pol.PolicyStatus = "IF";
                                    pol.AgentCode = txtSaleAgentID.Text.Trim();
                                    pol.ChannelId = ddlChannel.SelectedValue;
                                    pol.ChannelItemId = ddlChannelItem.SelectedValue;
                                    pol.ChannelLocationId = ddlChannelLocation.SelectedValue;
                                    pol.createdBy = userName;
                                    pol.CreatedOn = tranDate;
                                    isSavePolicy = da_micro_group_policy.Save(pol);
                                    #endregion policy

                                    if (!isSavePolicy)
                                    {
                                        isSaveSuccess = false;
                                        message = da_micro_group_policy.MESSAGE;
                                        break;
                                    }
                                    else
                                    {
                                        //backup policy
                                        Transaction.GroupMirco.Policy.Backup(new Transaction.GroupMirco.Policy.Tran() { PolicyId = pol.PolicyId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                        //count success policy
                                        countPolicy += 1;

                                        #region save approver

                                        //save approver
                                        List<da_report_approver.bl_report_approver> approver = new List<da_report_approver.bl_report_approver>();
                                        approver = da_report_approver.GetApproverList();

                                        foreach (da_report_approver.bl_report_approver ap in approver.Where(_ => _.NameEn.ToUpper() == AppConfiguration.GetApplicationApprover().ToUpper()))
                                        {
                                            isSaveSuccess = da_report_approver.InsertApproverPolicy(new da_report_approver.bl_report_approver_policy()
                                            {
                                                Approver_ID = ap.ID,
                                                Policy_ID = pol.PolicyId,
                                                Created_By = userName,
                                                Created_On = tranDate
                                            });
                                            break;

                                        }

                                        if (!isSaveSuccess)
                                        {
                                            message = "Saved Approver fail.";
                                            break;
                                        }
                                        #endregion save approver

                                        #region Beneficiary
                                        beneficiary = new bl_micro_policy_beneficiary()
                                        {
                                            ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_BENEFICIARY" }, { "FIELD", "BENEFICIARY_ID" } }),
                                            POLICY_ID = pol.PolicyId,
                                            FULL_NAME = "NA",
                                            Gender = "-1",
                                            AGE = "-",
                                            ADDRESS = "NA",
                                            PERCENTAGE_OF_SHARE = 0,
                                            RELATION = "NA",
                                            CREATED_BY = userName,
                                            CREATED_ON = tranDate,
                                            REMARKS = ""

                                        };
                                        isSaveBen = da_micro_group_policy_beneficiary.Save(beneficiary);
                                        if (!isSaveBen)
                                        {
                                            isSaveSuccess = false;
                                            message = da_micro_group_policy_beneficiary.MESSAGE;
                                            break;
                                        }
                                        else
                                        {
                                            isSaveSuccess = true;
                                            //backup bendeficiary
                                            Transaction.GroupMirco.Beneficiary.Backup(new Transaction.GroupMirco.Beneficiary.Tran() { BeneficiaryId = beneficiary.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                            #region Policy Rider
                                            string policyRiderid = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_RIDER" }, { "FIELD", "POLICY_RIDER_ID" } });
                                            if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                                            {
                                                double premRielRider = pol.ExchangeRate > 0 ? Math.Round(riderPremiumByMode * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0;
                                                if (premRielRider > 0)
                                                {
                                                    int leR = premRielRider.ToString().Length;
                                                    string bR = premRielRider.ToString().Substring(leR - 2, 2);

                                                    if (Convert.ToDouble(bR) < 50)
                                                    {
                                                        //round down
                                                        premRielRider = premRielRider - Convert.ToDouble(bR);
                                                    }
                                                    else if (Convert.ToDouble(bR) >= 50)
                                                    {
                                                        premRielRider = (premRielRider - Convert.ToDouble(bR)) + 100;
                                                    }
                                                }
                                                isSaveSuccess = da_micro_group_policy_rider.Save(new bl_micro_group_policy_rider()
                                                {

                                                    PolicyRiderId = policyRiderid,
                                                    PolicyId = pol.PolicyId,
                                                    ProductId = proConf.RiderProductID,
                                                    SumAssured = riderSA,
                                                    PremiumRate = riderRate.RATE,
                                                    Premium = riderPremiumByMode,
                                                    PremiumRiel = premRielRider,
                                                    AnnualPremium = riderAnnualPremium,
                                                    DiscountAmount = riderDiscount,
                                                    RiderStatus = "IF",
                                                    TotalAmount = riderPremiumByMode - riderDiscount,
                                                    CreatedBy = userName,
                                                    CreatedOn = tranDate,
                                                    Remarks = ""
                                                });

                                                if (isSaveSuccess)
                                                {
                                                    isSaveSuccess = Transaction.GroupMirco.PolicyRider.BackUp(new Transaction.GroupMirco.PolicyRider.Tran()
                                                    {
                                                        PolicyRiderId = policyRiderid,
                                                        TranBy = userName,
                                                        TranDate = tranDate,
                                                        TranType = "INSERT"
                                                    });
                                                    if (!isSaveSuccess)
                                                    {
                                                        message = "Backup Policy Rider fail.";
                                                    }

                                                }
                                                else
                                                {
                                                    message = da_micro_group_policy_rider.MESSAGE;

                                                }
                                                if (!isSaveSuccess)
                                                {
                                                    message = "Backup Policy Rider fail.";

                                                }
                                            }
                                            #endregion Policy Rider

                                            if (!isSaveSuccess)
                                            {

                                                break;
                                            }


                                            polDe = new bl_micro_group_policy_detail();
                                            polDe.PolicyID = pol.PolicyId;
                                            polDe.IssuedDate = l.IssuedDate;
                                            polDe.EffectivedDate = l.EffectiveDate;

                                            polDe.CoverPeriodType = ddlCoverPeriodType.SelectedItem.Text;
                                            polDe.PayPeriodType = ddlPayPeriodType.SelectedItem.Text;

                                            // coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
                                            //  payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

                                            polDe.CoverYear = coverPeriod;
                                            polDe.PayYear = payPeriod;

                                            //if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                                            //{
                                            //    polDe.MaturityDate = polDe.EffectivedDate.AddYears(polDe.CoverYear);
                                            //    polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            //}
                                            //else if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                                            //{
                                            //    polDe.MaturityDate = polDe.EffectivedDate.AddMonths(polDe.CoverYear);
                                            //    polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            //}
                                            //else if (ddlCoverPeriodType.SelectedItem.Text == bl_micro_product_config.PERIOD_TYPE.D.ToString())
                                            //{
                                            //    polDe.MaturityDate = polDe.EffectivedDate.AddDays(polDe.CoverYear);
                                            //    polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            //}
                                            if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddYears(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }
                                            else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddMonths(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }
                                            else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.D.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddDays(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }

                                            polDe.Age = custAge;

                                            polDe.SumAssured = pol.Currency.ToUpper() == "USD" ? l.SumAssured : Math.Round(l.SumAssured / pol.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                                            polDe.PayMode = payMode;// Convert.ToInt32(ddlPaymentMode.SelectedValue);
                                            polDe.PaymentCode = "";

                                            polDe.PremiumRate = proRate.RATE;
                                            polDe.Premium = Math.Round(premium[1], 2, MidpointRounding.AwayFromZero);

                                            double premRiel = pol.ExchangeRate > 0 ? Math.Round(polDe.Premium * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0;

                                            if (premRiel > 0)
                                            {
                                                int le = premRiel.ToString().Length;
                                                string b = premRiel.ToString().Substring(le - 2, 2);
                                                if (Convert.ToDouble(b) < 50)
                                                {
                                                    //round down
                                                    premRiel = premRiel - Convert.ToDouble(b);
                                                }
                                                else if (Convert.ToDouble(b) >= 50)
                                                {
                                                    premRiel = (premRiel - Convert.ToDouble(b)) + 100;
                                                }
                                            }
                                            //check discount 

                                            if (disConf.ProductID != null)
                                            {
                                                polDe.DiscountAmount = disConf.BasicDiscountAmount;

                                            }
                                            else
                                            {
                                                polDe.DiscountAmount = 0;
                                            }

                                            polDe.TotalAmount = polDe.Premium - polDe.DiscountAmount;
                                            polDe.PremiumRiel = premRiel;
                                            polDe.AnnualPremium = Math.Round(premium[0], 2, MidpointRounding.AwayFromZero); ;

                                            polDe.PolicyStatusRemarks = "NEW";
                                            polDe.RenewalFrom = "";
                                            polDe.FrequencyReduceYear = 0;
                                            polDe.ReduceRate = 0;
                                            polDe.CreatedBy = userName;
                                            polDe.CreatedOn = tranDate;

                                            isSavePolicyDetail = da_micro_group_policy_detail.Save(polDe);
                                            if (!isSavePolicyDetail)
                                            {
                                                isSaveSuccess = false;
                                                message = da_micro_group_policy_detail.MESSAGE;
                                                break;
                                            }
                                            else
                                            {
                                                //backup policy detail
                                                Transaction.GroupMirco.PolicyDetail.Backup(new Transaction.GroupMirco.PolicyDetail.Tran() { PolicyDetailId = polDe.PolicyDetailId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                                #region payment
                                                pay = new bl_micro_group_policy_payment()
                                                {
                                                    PolicyDetailId = polDe.PolicyDetailId,
                                                    PayMode = payMode,
                                                    UserPremium = polDe.Premium,
                                                    Amount = polDe.Premium,
                                                    DiscountAmount = polDe.DiscountAmount,
                                                    TotalAmount = polDe.TotalAmount,
                                                    AmountRiel = polDe.PremiumRiel,
                                                    DueDate = polDe.EffectivedDate,
                                                    NextDueDate = payMode == 1 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddYears(1), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 2 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(6), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 3 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(3), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 4 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(1), polDe.EffectivedDate, polDe.EffectivedDate) : new DateTime(1900, 1, 1),
                                                    PayDate = new DateTime(1900, 1, 1),
                                                    ReturnAmount = (polDe.Premium - polDe.Premium),
                                                    PolicyStatus = "IF",
                                                    PremiumYear = 1,
                                                    PremiumLot = 1,
                                                    OfficeId = "HQ",
                                                    TransactionType = "",
                                                    TransactionRef = "",
                                                    CreatedBy = userName,
                                                    CreatedOn = tranDate,
                                                    ReportDate = reportDate
                                                };

                                                totalAmount += pay.Amount;
                                                totalDiscount += pay.DiscountAmount;
                                                grandTotalAmount += pay.TotalAmount;
                                                totalReturnAmount += pay.ReturnAmount;

                                                totalPremium += polDe.Premium;
                                                totalAnnualPremium += polDe.AnnualPremium;
                                                totalSA += polDe.SumAssured;


                                                isSavePayment = da_micro_group_policy_payment.Save(pay);
                                                if (!isSavePayment)
                                                {
                                                    isSaveSuccess = false;
                                                    message = da_micro_group_policy_payment.MESSAGE;
                                                    break;
                                                }
                                                else
                                                {
                                                    #region save certificate
                                                    isSaveSuccess = da_group_micro_certificate.Save(new bl_group_micro_certificate()
                                                    {
                                                        Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CERTIFICATE" }, { "FIELD", "ID" } }),

                                                        CustomerNo = cus.CUSTOMER_NUMBER,
                                                        PolicyNumber = pol.PolicyNumber,
                                                        AgentCode = app.AgentCode,
                                                        AgentNameEn = app.AgentNameEn,
                                                        AgentNameKh = app.AgentNameKh,
                                                        IdType = app.IdType,
                                                        IdEn = app.IdEn,
                                                        IdKh = app.IdKh,
                                                        IdNo = app.IdNo,
                                                        FullName = l.ClientName,
                                                        Gender = app.Gender,
                                                        GenderEn = app.GenderEn,
                                                        GenderKh = app.GenderKh,
                                                        DateOfBirth = app.BirthOfDate,
                                                        Age = polDe.Age,
                                                        Nationality = app.Nationality,
                                                        Address = "NA",
                                                        Province = app.Province,
                                                        ProductId = app.ProductId,
                                                        ProductName = app.ProductName,
                                                        ProductNameKh = app.ProductNameKh,
                                                        SumAssure = app.SumAssure,
                                                        CoverPeriodType = app.CoverPeriodType,
                                                        PayPeriodType = app.PayPeriodType,
                                                        TermOfCover = app.TermOfCover,
                                                        PaymentPeriod = app.PaymentPeriod,
                                                        PayMode = app.PayMode,
                                                        PayModeEn = app.PayModeEn,
                                                        PayModeKh = app.PayModeKh,
                                                        Premium = app.Premium,
                                                        AnnualPremium = app.AnnualPremium,
                                                        UserPremium = app.UserPremium,
                                                        DiscountAmount = app.DiscountAmount,
                                                        TotalAmount = app.TotalAmount,
                                                        RiderProductId = app.RiderProductId,
                                                        RiderProductName = app.RiderProductName,
                                                        RiderProductNameKh = app.RiderProductNameKh,
                                                        RiderSumAssure = app.RiderSumAssure,
                                                        RiderPremium = app.RiderPremium,
                                                        RiderAnnualPremium = app.RiderAnnualPremium,
                                                        RiderDiscountAmount = app.RiderDiscountAmount,
                                                        RiderTotalAmount = app.RiderTotalAmount,
                                                        EffectiveDate = polDe.EffectivedDate,
                                                        ExpiryDate = polDe.ExpiryDate,
                                                        NextDueDate = pay.NextDueDate
                                                    });
                                                    if (isSaveSuccess)
                                                    {
                                                        bool saveCert = Transaction.GroupMirco.Certificate.BackUp(new Transaction.GroupMirco.Certificate.Tran() { PolicyNumber = pol.PolicyNumber, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                                        if (!saveCert)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = "Backup certificate fail.";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSaveSuccess = false;
                                                        message = "Save certificate fail.";
                                                        break;
                                                    }
                                                    #endregion save certificate

                                                    //save policy payment transaction 
                                                    Transaction.GroupMirco.PolicyPayment.Backup(new Transaction.GroupMirco.PolicyPayment.Tran()
                                                    {
                                                        PolicyPaymentId = pay.PolicyPaymentId,
                                                        TranBy = userName,
                                                        TranDate = tranDate,
                                                        TranType = "INSERT"

                                                    });

                                                    #region bunch payment summary
                                                    if (loopStep == 1) // save only first loop
                                                    {
                                                        bunchSummary = new bl_micro_group_policy_payment_bunch.summary()
                                                        {
                                                            GroupMasterCode = gMaster.GroupCode,
                                                            Amount = totalAmount,
                                                            DisountAmount = totalDiscount,
                                                            TotalAmount = grandTotalAmount,
                                                            ReturnAmount = totalReturnAmount,
                                                            Status = 0,//pending to pay 1 is paid,
                                                            PaymentType = "NEW",
                                                            CreatedBy = userName,
                                                            CreatedOn = tranDate
                                                        };
                                                        isSavePaymentBunchSummary = da_micro_group_policy_payment_bunch.summary.Save(bunchSummary);
                                                    }

                                                    #endregion bunch payment summary
                                                    if (!isSavePaymentBunchSummary)
                                                    {
                                                        isSaveSuccess = false;
                                                        message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //BACKUP payment summary
                                                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran()
                                                        {
                                                            BunchId = bunchSummary.BunchId,
                                                            TranBy = userName,
                                                            TranDate = tranDate,
                                                            TranType = "INSERT"
                                                        });

                                                        #region bunch payment detail
                                                        bunchDetail = new bl_micro_group_policy_payment_bunch.detail()
                                                        {
                                                            BunchId = bunchSummary.BunchId,
                                                            PolicyPaymentId = pay.PolicyPaymentId
                                                        };
                                                        isSavePaymentBunchDetail = da_micro_group_policy_payment_bunch.detail.Save(bunchDetail);
                                                        if (!isSavePaymentBunchDetail)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            //save transaction payment bunch detail
                                                            Transaction.GroupMirco.PaymentBunchDetail.Backup(new Transaction.GroupMirco.PaymentBunchDetail.Tran()
                                                            {
                                                                BunchDetailId = bunchDetail.BunchDetailId,
                                                                TranBy = userName,
                                                                TranDate = tranDate,
                                                                TranType = "INSERT"
                                                            });

                                                        }
                                                        #endregion bunch payment detail
                                                    }
                                                }
                                                #endregion payment
                                            }
                                        }
                                        #endregion Beneficiary
                                    }

                                }

                            }
                            #endregion Save Conctact
                        }
                        else//save application fail
                        {
                            isSaveSuccess = false;
                            message = "Saved application number is fail.";
                            break;
                        }


                    }
                    else
                    {
                        isSaveSuccess = false;
                        message = "Saved Customer fail.";
                        break;
                    }
                }//END LOOP LOAN LIST


                if (isSaveSuccess)
                {
                    var a = bunchSummary;
                    a.Amount = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                    a.DisountAmount = Math.Round(totalDiscount, 2, MidpointRounding.AwayFromZero); ;
                    a.TotalAmount = Math.Round(grandTotalAmount, 2, MidpointRounding.AwayFromZero); ;
                    a.ReturnAmount = Math.Round(totalReturnAmount, 2, MidpointRounding.AwayFromZero);
                    a.NumberPolicy = countPolicy;
                    a.ReportDate = reportDate;
                    a.UpdatedBy = userName;
                    a.UpdatedOn = tranDate;

                    if (!da_micro_group_policy_payment_bunch.summary.Update(a))
                    {
                        isSaveSuccess = false;
                    }
                    else
                    {
                        //back up payment buch summary
                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran() { BunchId = a.BunchId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                        bl_pma_report pma = new bl_pma_report()
                        {
                            PolicyNumber = gMaster.GroupCode,
                            EffectiveDate = gMaster.EffectiveDate,
                            PayDate = a.ReportDate,
                            Amount = a.Amount,
                            DiscountAmount = a.DisountAmount,
                            EM = 0,
                            Premium = totalPremium,
                            AnnualPremium = totalAnnualPremium,
                            PayYear = pay.PremiumYear,
                            PayLot = pay.PremiumLot,
                            PayMode = pay.PayModeText,
                            ProductCode = proConf.Plan_Code,
                            ProductId = proConf.Product_ID,
                            ProductName = proConf.En_Title,
                            ReportDate = a.ReportDate,
                            CreatedBy = userName,
                            CreatedOn = tranDate,
                            SA = totalSA,
                            NumberPolicy = a.NumberPolicy,
                            PolicyRange = firstPolicyNo + "-" + lastPolicyNo
                        };

                        if (da_pma_report.Save(pma))
                        {
                            isSaveSuccess = Transaction.GroupMirco.PMA.BackUp(new Transaction.GroupMirco.PMA.Tran() { ID = pma.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                        }
                        else
                        {
                            isSaveSuccess = false;
                        }


                    }
                    btnSave.Enabled = false;
                }

                //popup message
                if (isSaveSuccess)
                {
                    da_micro_group_loan_upload.DeleteTempRecords(userName, tranDate);

                    //url
                    string url = string.Format("../Reports/group_micro_policy_detail_req.aspx?CHID={0}&FDATE={1}&TDATE={2}&PROID={3} ", ddlChannelItem.SelectedValue, txtPaymentReportDate.Text, txtPaymentReportDate.Text, ddlProduct.SelectedValue);
                    url = "<a href='" + url + " ' target='_blank'><span>Click here to show report</span></a>";

                    Helper.Alert(false, "Saved successfully.<br/>" + url, lblError);
                    btnUpload.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                    if (File.Exists(MyFilePath))
                    {
                        File.Delete(MyFilePath);

                    }
                    if (RoleBack(userName, tranDate))
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
                    }
                }


            }
            else
            { //save loan fail
                Helper.Alert(true, "Saved fail. Error:" + da_micro_group_loan_upload.MESSAGE, lblError);

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnSave_Click()] in page [frm_gmCustomer_upload], detail:" + ex.Message + "=>" + ex.StackTrace);
            if (RoleBack(userName, tranDate))
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
            }
            else
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
            }
            btnSave.Enabled = false;
        }

        //Delete temp file
        if (File.Exists(MyTempFilePath))
        {
            File.Delete(MyTempFilePath);
        }


    }

    private bl_micro_group_customer GetExistingCustomer(int idType, string idNumber, string groupMasterCode)
    {
        bl_micro_group_customer obj = new bl_micro_group_customer();

        foreach (var cus in ExistingCustomerList.Where(_ => _.ID_TYPE == idType && _.ID_NUMBER == idNumber && _.GroupCode == groupMasterCode))
        {
            obj = cus;
            break;
        }
        return obj;

    }

    private bool IsFirstPolicy(int idType, string idNumber, string groupMasterCode)
    {
        bool result = false;
        foreach (var obj in FirstPolicyList.Where(_ => _.IdType == idType && _.IdNumber == idNumber && _.GroupCode == groupMasterCode && _.IsFirstPolicy == true))
        {
            result = true;
            break;
        }
        return result;

    }

    protected void SaveWingDigitalLoan()
    {
        bl_micro_customer_prefix pre = new bl_micro_customer_prefix();
        bl_micro_group_policy_prefix polPre = new bl_micro_group_policy_prefix();
        bl_micro_product_config proConf = MyProductConfig;
        bl_micro_product_rate proRate;
        bl_micro_product_discount_config disConf = new bl_micro_product_discount_config();
        disConf = da_micro_product_config.DiscountConfig.GetProductDiscount(proConf.Product_ID, proConf.RiderProductID, 0, 0, "SELF");
        bl_micro_product_rider_rate riderRate = new bl_micro_product_rider_rate();
        bl_product proInfo = new bl_product();
        proInfo = da_product.GetProductByProductID(proConf.Product_ID);
        bl_micro_product_rider proRiderInfo = new bl_micro_product_rider();
        proRiderInfo = da_micro_product_rider.GetMicroProductByProductID(proConf.RiderProductID);
        DateTime tranDate = DateTime.Now;

        int payMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
        bool isSaveCus = false;
        bool isSaveAddress = false;
        bool isSaveContact = false;
        bool isSavePolicy = false;
        bool isSaveBen = false;
        bool isSavePolicyDetail = false;
        bool isSavePayment = false;
        bool isSavePaymentBunchSummary = false;
        bool isSavePaymentBunchDetail = false;
        double totalAmount = 0;
        double totalDiscount = 0;
        double grandTotalAmount = 0;
        double totalReturnAmount = 0;
        double totalAnnualPremium = 0;
        double totalPremium = 0;
        double totalSA = 0;
        int loopStep = 0;
        int countPolicy = 0;
        DateTime reportDate;
        bool isSaveSuccess = true;
        string message = "";
        string firstPolicyNo = "";
        string lastPolicyNo = "";
        double riderSA = ddlRiderSumAssure.Items.Count > 0 ? Convert.ToDouble(ddlRiderSumAssure.SelectedValue) : 0;

        int coverPeriod = 0;
        int payPeriod = 0;

        try
        {
            bl_micro_group_policy pol;
            bl_micro_group_policy_detail polDe;
            bl_micro_group_policy_payment pay = new bl_micro_group_policy_payment();
            bl_micro_group_policy_payment_bunch.summary bunchSummary = new bl_micro_group_policy_payment_bunch.summary();
            bl_micro_group_policy_payment_bunch.detail bunchDetail;
            bl_micro_group_invoice.summary invoiceSummary = new bl_micro_group_invoice.summary();
            bl_micro_policy_beneficiary beneficiary;

            #region save customer
            bl_micro_group_customer cus;
            pre = da_micro_group_customer_prefix.GetLastCustomerPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_group_master_product> gMasterList = new List<bl_group_master_product>();
            gMasterList = da_group_master_product.GetGroupMasterProductList(PrivateGroupMasterProduct.ProductID, ddlChannelItem.SelectedValue);
            var gMaster = gMasterList[0];

            polPre = da_micro_group_policy_prefix.GetLastPolicyPrefix(gMaster.GroupCode);

            string cusNo = "";
            reportDate = Helper.FormatDateTime(txtPaymentReportDate.Text.Trim());

            bl_group_micro_application app = new bl_group_micro_application();
            bl_group_micro_application_number appNumberPrefix = new bl_group_micro_application_number();
            bl_group_micro_application_prefix appPrefix = new bl_group_micro_application_prefix();
            appPrefix = da_group_micro_application_prefix.GetLastPrefix(PrivateGroupMasterProduct.GroupCode);

            List<bl_wing_digital_loan_upload> wingList = (List<bl_wing_digital_loan_upload>)Session["SS_LOAN_LIST_OBJECT"];

            bool isExistCust = false;
            tranDate = DateTime.Now;

            string coverPeriodType =  ddlCoverPeriodType.SelectedItem.Text;
            string payPeriodType = ddlPayPeriodType.SelectedItem.Text;
          //  coverPeriod = Convert.ToInt32(ddlCoverPeriod.SelectedValue);
          //  payPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue);

            string paymentModeEn = Helper.GetPaymentModeEnglish(payMode);
            string paymentModeKh = Helper.GetPaymentModeInKhmer(payMode);

            if (da_micro_group_loan_upload.SaveWingDigitalLoan(userName, tranDate, ddlChannelItem.SelectedValue))
            {
                bl_micro_group_customer.LastSequence objCustLastSeq = da_micro_group_customer.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);
                cus = new bl_micro_group_customer(PrivateGroupMasterProduct.GroupCode);
                int customerSeqNo = 0;
                double applicationSeqNo = 0;
                int policySeqNo = 0;

                if (pre.PREFIX2 == objCustLastSeq.Prefix)//in the same year
                {
                    customerSeqNo = Convert.ToInt32(objCustLastSeq.SequenceNumber + 1);// cus.LAST_SEQ + 1;
                    //  cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                }
                else
                {
                    customerSeqNo = 1;
                    // cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                }

                bl_micro_group_policy.LastSequence objLastSeq = da_micro_group_policy.GetLastSEQ(PrivateGroupMasterProduct.GroupCode);
                if (objLastSeq.Prefix == polPre.PREFIX2)//in the same year
                {
                    policySeqNo = objLastSeq.LastSequenceNumber + 1;

                }
                else
                {
                    policySeqNo = 1;

                }


                appNumberPrefix = da_group_micro_application_number.GetLastSeq(PrivateGroupMasterProduct.GroupCode);

                if (appNumberPrefix.PrefixYear == appPrefix.Prefix2)//in same year
                {
                    applicationSeqNo = appNumberPrefix.Seq + 1;
                }
                else
                {
                    applicationSeqNo = 1;
                }

                //save approver
                List<da_report_approver.bl_report_approver> approver = new List<da_report_approver.bl_report_approver>();
                approver = da_report_approver.GetApproverList();

                da_report_approver.bl_report_approver_policy appApprover = new da_report_approver.bl_report_approver_policy();

                foreach (da_report_approver.bl_report_approver ap in approver.Where(_ => _.NameEn.ToUpper() == AppConfiguration.GetApplicationApprover().ToUpper()))
                {
                    appApprover.Approver_ID = ap.ID;
                    appApprover.Created_By = userName;
                    appApprover.Created_On = tranDate;
                    break;
                }


                //get rate
                proRate = new bl_micro_product_rate();


                if (proRate.PRODUCT_ID == null)
                {
                    isSaveSuccess = false;
                    message = "Product rate setup is not found.";

                }

                //copy file
                File.Copy(MyTempFilePath, MyFilePath, true);

                foreach (bl_wing_digital_loan_upload l in wingList) //loop wing digital loan records
                {
                    coverPeriodType = l.LoanPeriodType;
                    coverPeriod = l.LoanPeriod;
                    payPeriod = l.LoanPeriod;
                    payPeriodType = l.LoanPeriodType;
                    tranDate = DateTime.Now;
                    loopStep += 1;

                    if (loopStep == 1)
                    {
                        proRate = da_micro_product_rate.GetProductRate(proConf.Product_ID, 1, 18, l.SumAssure, payMode);
                    }

                    cus = new bl_micro_group_customer();
                    //check existing customer

                    // cus = da_micro_group_customer.Get(l.IdType, l.IdNumber, PrivateGroupMasterProduct.GroupCode);
                    cus = GetExistingCustomer(l.IdType, l.IdNumber, PrivateGroupMasterProduct.GroupCode);

                    if (cus.CUSTOMER_NUMBER != null)
                    {
                        isSaveCus = true;
                        isExistCust = true;
                    }
                    else//new customer
                    {
                        isExistCust = false;
                        cus = new bl_micro_group_customer(PrivateGroupMasterProduct.GroupCode);
                        cus.SEQ = customerSeqNo;// cus.LAST_SEQ + 1;

                        cusNo = pre.PREFIX1 + pre.PREFIX2 + cus.SEQ.ToString(pre.DIGITS);
                        cus.CUSTOMER_NUMBER = cusNo;

                        if (Helper.CheckContentUnicode(l.ClientName))
                        {
                            cus.FULL_NAME_KH = l.ClientName;
                            cus.FIRST_NAME_KH = l.FirstName;
                            cus.LAST_NAME_KH = l.LastName;
                            cus.FULL_NAME_EN = "";
                            cus.FIRST_NAME_EN = "";
                            cus.LAST_NAME_EN = "";
                        }
                        else
                        {
                            cus.FULL_NAME_EN = l.ClientName;
                            cus.FIRST_NAME_EN = l.FirstName;
                            cus.LAST_NAME_EN = l.LastName;
                            cus.FULL_NAME_KH = "";
                            cus.FIRST_NAME_KH = "";
                            cus.LAST_NAME_KH = "";
                        }


                        cus.GENDER = l.Gender;
                        cus.DOB = l.DOB;
                        cus.ID_TYPE = l.IdType;
                        cus.ID_NUMBER = l.IdNumber;
                        cus.OCCUPATION = l.Occupation;
                        cus.STATUS = 1;
                        cus.CREATED_BY = userName;
                        cus.CREATED_ON = tranDate;
                        cus.MARITAL_STATUS = "";

                        /*save new customer*/
                        isSaveCus = da_micro_group_customer.Save(cus);
                        if (isSaveCus)
                        {
                            /*add new customer into existing customer list for checking exsting customer of next records*/
                            ExistingCustomerList.Add(cus);
                        }

                        customerSeqNo += 1;//increase 1 for next new customer
                    }

                    if (isSaveCus)
                    {
                        // appNumberPrefix = da_group_micro_application_number.GetLastSeq(PrivateGroupMasterProduct.GroupCode);
                        bl_group_micro_application_number appNumber = new bl_group_micro_application_number()
                        {
                            Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION_NUMBER" }, { "FIELD", "ID" } }),
                            ChannelId = ddlChannel.SelectedValue,
                            ChannelItemId = ddlChannelItem.SelectedValue,
                            ChannelLocationId = ddlChannelLocation.SelectedValue,
                            GroupCode = PrivateGroupMasterProduct.GroupCode,
                            CreatedBy = userName,
                            CreatedOn = DateTime.Now,
                            Seq = applicationSeqNo,
                            ApplicationNumber = appPrefix.Prefix1 + appPrefix.Prefix2 + applicationSeqNo.ToString(appPrefix.Digits)
                        };

                        if (da_group_micro_application_number.Save(appNumber))
                        {

                            bool saveAppTran = Transaction.GroupMirco.ApplicationNumber.BackUp(new Transaction.GroupMirco.ApplicationNumber.Tran()
                            {
                                ApplicationNumber = appNumber.ApplicationNumber,
                                TranBy = userName,
                                TranDate = tranDate,
                                TranType = "INSERT"
                            });
                            if (saveAppTran)
                            {
                                isSaveSuccess = true;
                            }
                            else
                            {
                                isSaveSuccess = false;
                                message = "Backup application fail.";
                                break;
                            }

                            #region rate and discount
                            
                            double basicDiscount = 0.0;
                            double riderDiscount = 0.0;
                            double basicPremiumByMode = 0.0;
                            double basicAnnualPremium = 0.0;
                            double totalBisicAmount = 0;
                            double totalRiderAmount = 0;
                            double riderPremiumByMode = 0.0;
                            double riderAnnualPremium = 0.0;

                            double[] riderPremium = new double[] { 0, 0 };
                            double sumAssure = l.SumAssure;// Convert.ToDouble(ddlBasicSumAssure.SelectedValue);

                            int custAge = Calculation.Culculate_Customer_Age(cus.DOB.ToString("dd/MM/yyyy"), l.AppliedDate);


                            if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                            {
                                riderRate = da_micro_product_rider_rate.GetProductRate(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                if (riderRate.PRODUCT_ID == null)
                                {
                                    isSaveSuccess = false;
                                    message = "Rider product rate setup is not found.";
                                    break;
                                }
                                riderPremium = Calculation.GetMicroProductRiderPremium(proConf.RiderProductID, cus.GENDER, custAge, riderSA, payMode);
                                riderPremiumByMode = riderPremium[1];
                                riderAnnualPremium = riderPremium[0];
                            }
                            //   double[] premium = Calculation.GetMicroProducPremium(proConf.Product_ID, cus.GENDER, custAge, sumAssure, payMode);
                            basicPremiumByMode = l.Premium;// premium[1];
                            basicAnnualPremium = 0;// premium[0];

                            /*CHECK POLICY COVER PERIOD MORE THAN 12 MONTHS*/
                            //    int policyCount = l.LoanPeriod / 12;


                            if (!string.IsNullOrWhiteSpace(disConf.ProductID))
                            {
                                basicDiscount = disConf.BasicDiscountAmount;
                                riderDiscount = disConf.RiderDiscountAmount;
                            }
                            totalBisicAmount = basicPremiumByMode - basicDiscount;
                            totalRiderAmount = riderPremiumByMode - riderDiscount;
                            #endregion rate and discount

                            #region save application
                            string appAddressEn = "";
                            string appAddressKh = "";

                            app = new bl_group_micro_application()
                            {
                                Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_APPLICATION" }, { "FIELD", "ID" } }),
                                ApplicationNumber = appNumber.ApplicationNumber,
                                ApplicationDate = l.AppliedDate,
                                AgentCode = AgentInfo.SaleAgentId,
                                AgentNameEn = AgentInfo.FullNameEn,
                                AgentNameKh = AgentInfo.FullNameKh,
                                IdType = l.IdType,
                                IdEn = l.IdTypeText,
                                IdKh = Helper.GetIDCardTypeTextKh(cus.ID_TYPE),
                                IdNo = l.IdNumber,
                                FirstNameInEnglish = cus.FIRST_NAME_EN,
                                LastNameInEnglish = cus.LAST_NAME_EN,
                                FirstNameInKhmer = cus.FIRST_NAME_KH,
                                LastNameInKhmer = cus.LAST_NAME_KH,
                                Gender = l.Gender,
                                GenderEn = l.GenderText,
                                GenderKh = Helper.GetGenderText(l.Gender, false, true),
                                BirthOfDate = l.DOB,
                                Nationality = "",
                                MaritalStatus = "",
                                MaritalStatusKh = "",
                                Occupation = "NA",
                                OccupationKh = "",
                                PhoneNumber = l.PhoneNumber,
                                Email = "",
                                Address = appAddressEn,
                                AddressKh = appAddressKh,
                                Province = "",
                                ProvinceKh = "",
                                ProductId = proConf.Product_ID,
                                ProductName = proInfo.En_Title,
                                ProductNameKh = proInfo.Kh_Title,
                                SumAssure = sumAssure,
                                CoverPeriodType = coverPeriodType,
                                PayPeriodType = payPeriodType,
                                TermOfCover = coverPeriod,
                                PaymentPeriod = payPeriod,
                                PayMode = payMode,
                                PayModeEn = paymentModeEn,
                                PayModeKh = paymentModeKh,
                                Premium = basicPremiumByMode,
                                AnnualPremium = basicAnnualPremium,
                                UserPremium = 0,
                                DiscountAmount = basicDiscount,
                                TotalAmount = totalBisicAmount,
                                RiderProductId = proConf.RiderProductID,
                                RiderProductName = proRiderInfo.EN_TITLE != null ? proRiderInfo.EN_TITLE : "",
                                RiderProductNameKh = proRiderInfo.KH_TITLE != null ? proRiderInfo.KH_TITLE : "",
                                RiderSumAssure = riderSA,
                                RiderPremium = riderPremiumByMode,
                                RiderAnnualPremium = riderAnnualPremium,
                                RiderDiscountAmount = riderDiscount,
                                RiderTotalAmount = totalRiderAmount,
                                BenFullName = "NA",
                                BenAge = "-",
                                BenAddress = "NA",
                                PercentageShared = 0,
                                Relation = "NA",
                                RelationKh = "",
                                QuestionId = "6D44FA76-6B00-42D6-A4D7-ACD85986DC7C",
                                Answer = -1,
                                AnswerRemarks = "",
                                PaymentCode = "",
                                Referrer = "",
                                ReferrerId = ""

                            };

                            isSaveSuccess = da_group_micro_application.Save(app);
                            if (isSaveSuccess)
                            {
                                bool saveTranApp = Transaction.GroupMirco.Application.BackUp(new Transaction.GroupMirco.Application.Tran()
                                {
                                    ApplicationNumber = appNumber.ApplicationNumber,
                                    TranBy = userName,
                                    TranDate = tranDate,
                                    TranType = "INSERT"
                                });
                                if (!saveTranApp)
                                {
                                    isSaveSuccess = false;
                                    message = "Save application fail.";
                                    break;
                                }
                            }
                            else
                            {
                                isSaveSuccess = false;
                                message = "Save application fail.";
                                break;
                            }
                            #endregion save applicaiton
                            #region Save Contact

                            if (!isExistCust)
                            {
                                isSaveContact = da_micro_group_customer.Contact.Save(new bl_micro_group_customer_contact()
                                {
                                    CUSTOMER_ID = cus.ID,
                                    PHONE_NUMBER1 = l.PhoneNumber,
                                    CREATED_BY = userName,
                                    CREATED_ON = tranDate
                                });
                            }
                            else
                            {
                                isSaveContact = true;
                            }
                            #endregion Save Conctact

                            if (!isSaveContact)
                            {
                                isSaveSuccess = false;
                                message = da_micro_group_customer.MESSAGE;
                                break;
                            }
                            else
                            {
                                #region Save Address
                                if (!isExistCust)
                                {
                                    isSaveAddress = da_micro_group_customer.Address.Save(
                                            new bl_address()
                                            {
                                                CUSTOMER_ID = cus.ID,
                                                ADDRESS_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CUSTOMER_ADDRESS" }, { "FIELD", "ADDRESS_ID" } }),
                                                ADDRESS1 = "NA",
                                                CREATED_BY = userName,
                                                CREATED_ON = tranDate
                                            }
                                            );
                                }
                                else
                                {
                                    isSaveAddress = true;
                                }
                                if (!isSaveAddress)
                                {
                                    isSaveSuccess = false;
                                    message = da_micro_group_customer.MESSAGE;
                                    break;
                                }
                                else
                                {
                                    #region policy
                                    string pol_number = "";
                                    pol = new bl_micro_group_policy(PrivateGroupMasterProduct.GroupCode);

                                    pol.SEQ = policySeqNo;
                                    pol_number = polPre.PREFIX1 + polPre.PREFIX2 + pol.SEQ.ToString(polPre.DIGITS);

                                    if (loopStep == 1)
                                    {
                                        firstPolicyNo = pol_number;
                                    }
                                    else if (loopStep == wingList.Count)
                                    {
                                        lastPolicyNo = pol_number;
                                    }

                                    pol.ApplicationId = app.Id;
                                    pol.PolicyNumber = pol_number;
                                    pol.CustomerId = cus.ID;
                                    pol.GroupMasterId = gMaster.GroupMasterID;
                                    pol.LoanId = l.ID;
                                    pol.ProductId = proConf.Product_ID;
                                    pol.Currency = "USD";
                                    pol.ExchangeRate = 0;
                                    pol.PolicyStatus = "IF";
                                    pol.AgentCode = txtSaleAgentID.Text.Trim();
                                    pol.ChannelId = ddlChannel.SelectedValue;
                                    pol.ChannelItemId = ddlChannelItem.SelectedValue;
                                    pol.ChannelLocationId = ddlChannelLocation.SelectedValue;
                                    pol.createdBy = userName;
                                    pol.CreatedOn = tranDate;
                                    pol.IsFirstPolicy = l.IsFirstPolicy;
                                    pol.PolicyStatusDate = new DateTime(1900, 1, 1);
                                    isSavePolicy = da_micro_group_policy.Save(pol);
                                    #endregion policy

                                    if (!isSavePolicy)
                                    {
                                        isSaveSuccess = false;
                                        message = da_micro_group_policy.MESSAGE;
                                        break;
                                    }
                                    else
                                    {
                                        //backup policy
                                        Transaction.GroupMirco.Policy.Backup(new Transaction.GroupMirco.Policy.Tran() { PolicyId = pol.PolicyId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                        //count success policy
                                        countPolicy += 1;

                                        #region save approver
                                        
                                        appApprover.Policy_ID = pol.PolicyId;
                                        isSaveSuccess = da_report_approver.InsertApproverPolicy(appApprover);

                                        if (!isSaveSuccess)
                                        {
                                            message = "Saved Approver fail.";
                                            break;
                                        }
                                        #endregion save approver

                                        #region Beneficiary
                                        beneficiary = new bl_micro_policy_beneficiary()
                                        {
                                            ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_BENEFICIARY" }, { "FIELD", "BENEFICIARY_ID" } }),
                                            POLICY_ID = pol.PolicyId,
                                            FULL_NAME = "NA",
                                            Gender = "-1",
                                            AGE = "-",
                                            ADDRESS = "NA",
                                            PERCENTAGE_OF_SHARE = 0,
                                            RELATION = "NA",
                                            CREATED_BY = userName,
                                            CREATED_ON = tranDate,
                                            REMARKS = ""

                                        };
                                        isSaveBen = da_micro_group_policy_beneficiary.Save(beneficiary);
                                        #endregion Beneficiary
                                        if (!isSaveBen)
                                        {
                                            isSaveSuccess = false;
                                            message = da_micro_group_policy_beneficiary.MESSAGE;
                                            break;
                                        }
                                        else
                                        {
                                            isSaveSuccess = true;
                                            //backup bendeficiary
                                            Transaction.GroupMirco.Beneficiary.Backup(new Transaction.GroupMirco.Beneficiary.Tran() { BeneficiaryId = beneficiary.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                            #region Policy Rider
                                            string policyRiderid = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_RIDER" }, { "FIELD", "POLICY_RIDER_ID" } });
                                            if (!string.IsNullOrWhiteSpace(proConf.RiderProductID))
                                            {
                                                double premRielRider = pol.ExchangeRate > 0 ? Math.Round(riderPremiumByMode * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0;
                                                if (premRielRider > 0)
                                                {
                                                    int leR = premRielRider.ToString().Length;
                                                    string bR = premRielRider.ToString().Substring(leR - 2, 2);

                                                    if (Convert.ToDouble(bR) < 50)
                                                    {
                                                        //round down
                                                        premRielRider = premRielRider - Convert.ToDouble(bR);
                                                    }
                                                    else if (Convert.ToDouble(bR) >= 50)
                                                    {
                                                        premRielRider = (premRielRider - Convert.ToDouble(bR)) + 100;
                                                    }
                                                }
                                                isSaveSuccess = da_micro_group_policy_rider.Save(new bl_micro_group_policy_rider()
                                                {

                                                    PolicyRiderId = policyRiderid,
                                                    PolicyId = pol.PolicyId,
                                                    ProductId = proConf.RiderProductID,
                                                    SumAssured = riderSA,
                                                    PremiumRate = riderRate.RATE,
                                                    Premium = riderPremiumByMode,
                                                    PremiumRiel = premRielRider,
                                                    AnnualPremium = riderAnnualPremium,
                                                    DiscountAmount = riderDiscount,
                                                    RiderStatus = "IF",
                                                    TotalAmount = riderPremiumByMode - riderDiscount,
                                                    CreatedBy = userName,
                                                    CreatedOn = tranDate,
                                                    Remarks = ""
                                                });

                                                if (isSaveSuccess)
                                                {
                                                    isSaveSuccess = Transaction.GroupMirco.PolicyRider.BackUp(new Transaction.GroupMirco.PolicyRider.Tran()
                                                    {
                                                        PolicyRiderId = policyRiderid,
                                                        TranBy = userName,
                                                        TranDate = tranDate,
                                                        TranType = "INSERT"
                                                    });
                                                    if (!isSaveSuccess)
                                                    {
                                                        message = "Backup Policy Rider fail.";
                                                    }

                                                }
                                                else
                                                {
                                                    message = da_micro_group_policy_rider.MESSAGE;

                                                }
                                                if (!isSaveSuccess)
                                                {
                                                    message = "Backup Policy Rider fail.";

                                                }
                                            }
                                            #endregion Policy Rider

                                            if (!isSaveSuccess)
                                            {

                                                break;
                                            }

                                            #region save policy detail

                                            polDe = new bl_micro_group_policy_detail();
                                            polDe.PolicyID = pol.PolicyId;
                                            polDe.IssuedDate = l.AppliedDate;
                                            polDe.EffectivedDate = l.AppliedDate;

                                            polDe.CoverPeriodType = coverPeriodType;// ddlCoverPeriodType.SelectedItem.Text;
                                            polDe.PayPeriodType = payPeriodType;// ddlPayPeriodType.SelectedItem.Text;
                                            polDe.CoverYear =coverPeriod;
                                            polDe.PayYear = payPeriod;

                                            if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddYears(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }
                                            else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddMonths(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }
                                            else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.D.ToString())
                                            {
                                                polDe.MaturityDate = polDe.EffectivedDate.AddDays(polDe.CoverYear);
                                                polDe.ExpiryDate = polDe.MaturityDate.AddDays(-1);
                                            }

                                            polDe.Age = custAge;

                                            polDe.SumAssured = l.SumAssure;//.Currency.ToUpper() == "USD" ? sumAssure : Math.Round(sumAssure / pol.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                                            polDe.PayMode = payMode;// Convert.ToInt32(ddlPaymentMode.SelectedValue);
                                            polDe.PaymentCode = "";

                                            polDe.PremiumRate = proRate.RATE;
                                            polDe.Premium = l.Premium;// Math.Round(premium[1], 2, MidpointRounding.AwayFromZero);

                                            double premRiel = pol.ExchangeRate > 0 ? Math.Round(polDe.Premium * pol.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0;

                                            if (premRiel > 0)
                                            {
                                                int le = premRiel.ToString().Length;
                                                string b = premRiel.ToString().Substring(le - 2, 2);
                                                if (Convert.ToDouble(b) < 50)
                                                {
                                                    //round down
                                                    premRiel = premRiel - Convert.ToDouble(b);
                                                }
                                                else if (Convert.ToDouble(b) >= 50)
                                                {
                                                    premRiel = (premRiel - Convert.ToDouble(b)) + 100;
                                                }
                                            }
                                            //check discount 

                                            if (disConf.ProductID != null)
                                            {
                                                polDe.DiscountAmount = disConf.BasicDiscountAmount;

                                            }
                                            else
                                            {
                                                polDe.DiscountAmount = 0;
                                            }

                                            polDe.TotalAmount = polDe.Premium - polDe.DiscountAmount;
                                            polDe.PremiumRiel = premRiel;
                                            polDe.AnnualPremium = 0;// Math.Round(premium[0], 2, MidpointRounding.AwayFromZero); ;

                                            polDe.PolicyStatusRemarks = "NEW";
                                            polDe.RenewalFrom = "";
                                            polDe.FrequencyReduceYear = 0;
                                            polDe.ReduceRate = 0;
                                            polDe.CreatedBy = userName;
                                            polDe.CreatedOn = tranDate;

                                            isSavePolicyDetail = da_micro_group_policy_detail.Save(polDe);
                                            #endregion save policy detail

                                            if (!isSavePolicyDetail)
                                            {
                                                isSaveSuccess = false;
                                                message = da_micro_group_policy_detail.MESSAGE;
                                                break;
                                            }
                                            else
                                            {
                                                //backup policy detail
                                                Transaction.GroupMirco.PolicyDetail.Backup(new Transaction.GroupMirco.PolicyDetail.Tran() { PolicyDetailId = polDe.PolicyDetailId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                                                #region payment
                                                pay = new bl_micro_group_policy_payment()
                                                {
                                                    PolicyDetailId = polDe.PolicyDetailId,
                                                    PayMode = payMode,
                                                    UserPremium = polDe.Premium,
                                                    Amount = polDe.Premium,
                                                    DiscountAmount = polDe.DiscountAmount,
                                                    TotalAmount = polDe.TotalAmount,
                                                    AmountRiel = polDe.PremiumRiel,
                                                    DueDate = polDe.EffectivedDate,
                                                    NextDueDate = payMode == 1 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddYears(1), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 2 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(6), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 3 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(3), polDe.EffectivedDate, polDe.EffectivedDate) :
                                                    payMode == 4 ? Calculation.GetNext_Due(polDe.EffectivedDate.AddMonths(1), polDe.EffectivedDate, polDe.EffectivedDate) : new DateTime(1900, 1, 1),
                                                    PayDate = new DateTime(1900, 1, 1),
                                                    ReturnAmount = (polDe.Premium - polDe.Premium),
                                                    PolicyStatus = "IF",
                                                    PremiumYear = 1,
                                                    PremiumLot = 1,
                                                    OfficeId = "HQ",
                                                    TransactionType = "",
                                                    TransactionRef = "",
                                                    CreatedBy = userName,
                                                    CreatedOn = tranDate,
                                                    ReportDate = reportDate
                                                };

                                                totalAmount += pay.Amount;
                                                totalDiscount += pay.DiscountAmount;
                                                grandTotalAmount += pay.TotalAmount;
                                                totalReturnAmount += pay.ReturnAmount;

                                                totalPremium += polDe.Premium;
                                                totalAnnualPremium += polDe.AnnualPremium;
                                                totalSA += polDe.SumAssured;


                                                isSavePayment = da_micro_group_policy_payment.Save(pay);
                                                #endregion payment

                                                if (!isSavePayment)
                                                {
                                                    isSaveSuccess = false;
                                                    message = da_micro_group_policy_payment.MESSAGE;
                                                    break;
                                                }
                                                else
                                                {
                                                    #region save certificate
                                                    isSaveSuccess = da_group_micro_certificate.Save(new bl_group_micro_certificate()
                                                    {
                                                        Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CERTIFICATE" }, { "FIELD", "ID" } }),

                                                        CustomerNo = cus.CUSTOMER_NUMBER,
                                                        PolicyNumber = pol.PolicyNumber,
                                                        AgentCode = app.AgentCode,
                                                        AgentNameEn = app.AgentNameEn,
                                                        AgentNameKh = app.AgentNameKh,
                                                        IdType = app.IdType,
                                                        IdEn = app.IdEn,
                                                        IdKh = app.IdKh,
                                                        IdNo = app.IdNo,
                                                        FullName = l.ClientName,
                                                        Gender = app.Gender,
                                                        GenderEn = app.GenderEn,
                                                        GenderKh = app.GenderKh,
                                                        DateOfBirth = app.BirthOfDate,
                                                        Age = polDe.Age,
                                                        Nationality = app.Nationality,
                                                        Address = "NA",
                                                        Province = app.Province,
                                                        ProductId = app.ProductId,
                                                        ProductName = app.ProductName,
                                                        ProductNameKh = app.ProductNameKh,
                                                        SumAssure = app.SumAssure,
                                                        CoverPeriodType = app.CoverPeriodType,
                                                        PayPeriodType = app.PayPeriodType,
                                                        TermOfCover = app.TermOfCover,
                                                        PaymentPeriod = app.PaymentPeriod,
                                                        PayMode = app.PayMode,
                                                        PayModeEn = app.PayModeEn,
                                                        PayModeKh = app.PayModeKh,
                                                        Premium = app.Premium,
                                                        AnnualPremium = app.AnnualPremium,
                                                        UserPremium = app.UserPremium,
                                                        DiscountAmount = app.DiscountAmount,
                                                        TotalAmount = app.TotalAmount,
                                                        RiderProductId = app.RiderProductId,
                                                        RiderProductName = app.RiderProductName,
                                                        RiderProductNameKh = app.RiderProductNameKh,
                                                        RiderSumAssure = app.RiderSumAssure,
                                                        RiderPremium = app.RiderPremium,
                                                        RiderAnnualPremium = app.RiderAnnualPremium,
                                                        RiderDiscountAmount = app.RiderDiscountAmount,
                                                        RiderTotalAmount = app.RiderTotalAmount,
                                                        EffectiveDate = polDe.EffectivedDate,
                                                        ExpiryDate = polDe.ExpiryDate,
                                                        NextDueDate = pay.NextDueDate
                                                    });
                                                    if (isSaveSuccess)
                                                    {
                                                        bool saveCert = Transaction.GroupMirco.Certificate.BackUp(new Transaction.GroupMirco.Certificate.Tran() { PolicyNumber = pol.PolicyNumber, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                                                        if (!saveCert)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = "Backup certificate fail.";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSaveSuccess = false;
                                                        message = "Save certificate fail.";
                                                        break;
                                                    }
                                                    #endregion save certificate

                                                    //save policy payment transaction 
                                                    Transaction.GroupMirco.PolicyPayment.Backup(new Transaction.GroupMirco.PolicyPayment.Tran()
                                                    {
                                                        PolicyPaymentId = pay.PolicyPaymentId,
                                                        TranBy = userName,
                                                        TranDate = tranDate,
                                                        TranType = "INSERT"

                                                    });

                                                    #region bunch payment summary
                                                    if (loopStep == 1) // save only first loop
                                                    {
                                                        bunchSummary = new bl_micro_group_policy_payment_bunch.summary()
                                                        {
                                                            GroupMasterCode = gMaster.GroupCode,
                                                            Amount = totalAmount,
                                                            DisountAmount = totalDiscount,
                                                            TotalAmount = grandTotalAmount,
                                                            ReturnAmount = totalReturnAmount,
                                                            Status = 0,//pending to pay 1 is paid,
                                                            PaymentType = "NEW",
                                                            CreatedBy = userName,
                                                            CreatedOn = tranDate
                                                        };
                                                        isSavePaymentBunchSummary = da_micro_group_policy_payment_bunch.summary.Save(bunchSummary);
                                                    }

                                                    #endregion bunch payment summary
                                                    if (!isSavePaymentBunchSummary)
                                                    {
                                                        isSaveSuccess = false;
                                                        message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //BACKUP payment summary
                                                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran()
                                                        {
                                                            BunchId = bunchSummary.BunchId,
                                                            TranBy = userName,
                                                            TranDate = tranDate,
                                                            TranType = "INSERT"
                                                        });

                                                        #region bunch payment detail
                                                        bunchDetail = new bl_micro_group_policy_payment_bunch.detail()
                                                        {
                                                            BunchId = bunchSummary.BunchId,
                                                            PolicyPaymentId = pay.PolicyPaymentId
                                                        };
                                                        isSavePaymentBunchDetail = da_micro_group_policy_payment_bunch.detail.Save(bunchDetail);
                                                        if (!isSavePaymentBunchDetail)
                                                        {
                                                            isSaveSuccess = false;
                                                            message = da_micro_group_policy_payment_bunch.MESSAGE;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            //save transaction payment bunch detail
                                                            Transaction.GroupMirco.PaymentBunchDetail.Backup(new Transaction.GroupMirco.PaymentBunchDetail.Tran()
                                                            {
                                                                BunchDetailId = bunchDetail.BunchDetailId,
                                                                TranBy = userName,
                                                                TranDate = tranDate,
                                                                TranType = "INSERT"
                                                            });

                                                        }
                                                        #endregion bunch payment detail
                                                    }
                                                }
                                               
                                            }
                                        }

                                    }

                                }
                                #endregion Save Address
                            }

                        }
                        else//save application fail
                        {
                            isSaveSuccess = false;
                            message = "Saved application number is fail.";
                            break;
                        }
                    }
                    else
                    {
                        isSaveSuccess = false;
                        message = "Saved Customer fail.";
                        break;
                    }

                    applicationSeqNo += 1; //increate application sequence number 1 for next application 
                    policySeqNo += 1;
                }//END LOOP LOAN LIST
            #endregion save customer

                if (isSaveSuccess)
                {
                    var a = bunchSummary;
                    a.Amount = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                    a.DisountAmount = Math.Round(totalDiscount, 2, MidpointRounding.AwayFromZero); ;
                    a.TotalAmount = Math.Round(grandTotalAmount, 2, MidpointRounding.AwayFromZero); ;
                    a.ReturnAmount = Math.Round(totalReturnAmount, 2, MidpointRounding.AwayFromZero);
                    a.NumberPolicy = countPolicy;
                    a.ReportDate = reportDate;
                    a.UpdatedBy = userName;
                    a.UpdatedOn = tranDate;

                    if (!da_micro_group_policy_payment_bunch.summary.Update(a))
                    {
                        isSaveSuccess = false;
                    }
                    else
                    {
                        //back up payment buch summary
                        Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran() { BunchId = a.BunchId, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });

                        bl_pma_report pma = new bl_pma_report()
                        {
                            PolicyNumber = gMaster.GroupCode,
                            EffectiveDate = gMaster.EffectiveDate,
                            PayDate = a.ReportDate,
                            Amount = a.Amount,
                            DiscountAmount = a.DisountAmount,
                            EM = 0,
                            Premium = totalPremium,
                            AnnualPremium = totalAnnualPremium,
                            PayYear = pay.PremiumYear,
                            PayLot = pay.PremiumLot,
                            PayMode = pay.PayModeText,
                            ProductCode = proConf.Plan_Code,
                            ProductId = proConf.Product_ID,
                            ProductName = proConf.En_Title,
                            ReportDate = a.ReportDate,
                            CreatedBy = userName,
                            CreatedOn = tranDate,
                            SA = totalSA,
                            NumberPolicy = a.NumberPolicy,
                            PolicyRange = firstPolicyNo + "-" + lastPolicyNo
                        };

                        if (da_pma_report.Save(pma))
                        {
                            isSaveSuccess = Transaction.GroupMirco.PMA.BackUp(new Transaction.GroupMirco.PMA.Tran() { ID = pma.ID, TranBy = userName, TranDate = tranDate, TranType = "INSERT" });
                        }
                        else
                        {
                            isSaveSuccess = false;
                        }

                    }
                    btnSave.Enabled = false;
                }

                //popup message
                if (isSaveSuccess)
                {
                    da_micro_group_loan_upload.DeleteTempRecords(userName, tranDate);
                    //clear backupdata
           Transaction.GroupMirco.ClearBackupTransactionIssuePolicyRecords(userName, tranDate, "INSERT");
                 

                    //url
                    string url = string.Format("../Reports/group_micro_policy_detail_req.aspx?CHID={0}&FDATE={1}&TDATE={2}&PROID={3} ", ddlChannelItem.SelectedValue, txtPaymentReportDate.Text, txtPaymentReportDate.Text, ddlProduct.SelectedValue);
                    url = "<a href='" + url + " ' target='_blank'><span>Click here to show report</span></a>";

                    Helper.Alert(false, "Saved successfully.<br/>" + url, lblError);
                    btnUpload.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                    if (File.Exists(MyFilePath))
                    {
                        File.Delete(MyFilePath);

                    }
                    if (RoleBack(userName, tranDate))
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
                    }
                }


            }
            else
            { //save loan fail
                Helper.Alert(true, "Saved fail. Error:" + da_micro_group_loan_upload.MESSAGE, lblError);

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnSave_Click()] in page [frm_gmCustomer_upload], detail:" + ex.Message + "=>" + ex.StackTrace);
            if (RoleBack(userName, tranDate))
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction successfully.", lblError);
            }
            else
            {
                Helper.Alert(true, "Saved fail. Error:" + message + ", System RoleBack Transaction fail. Please cotact your system adminitrator.", lblError);
            }
            btnSave.Enabled = false;
        }

        //Delete temp file
        if (File.Exists(MyTempFilePath))
        {
            File.Delete(MyTempFilePath);
        }

        ExistingCustomerList = null;
        FirstPolicyList = null;
        MyPolicyInforce = null;
    }

    bool RoleBack(string userName, DateTime tranDate)
    {
        bool result = Transaction.GroupMirco.RollBackUpload(userName, tranDate);
        if (result)
        {
            //clear backupdata
            Transaction.GroupMirco.ClearBackupTransactionIssuePolicyRecords(userName, tranDate, "INSERT");
        }

        return result;

    }

    protected void ddlChannelLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<bl_group_master_product> lGroup = da_group_master_product.GetGroupMasterProductList("", ddlChannelItem.SelectedValue);
        List<bl_micro_product_config> listPro = new List<bl_micro_product_config>();
        listPro = da_micro_product_config.ProductConfig.GetProductMicroProductSO();
        Session["PRODUCT_LIST"] = listPro;// store in session for other filter

        /*bind saleable product list */
        ddlProduct.Items.Clear();
        List<bl_micro_product_config> proConList = da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(ddlChannelItem.SelectedValue, true);
        Options.Bind(ddlProduct, proConList, bl_micro_product_config.NAME.MarketingName, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");

        if (ddlProduct.Items.Count == 2)
        {
            ddlProduct.SelectedIndex = 1;
            ddlProduct.Enabled = false;
            ddlProduct_SelectedIndexChanged(null, null);
        }
        else
        {
            ddlProduct.Enabled = true;
            ddlProduct.SelectedIndex = 0;
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


    void BindBaicSA(double[] sa)
    {
        ddlBasicSumAssure.Items.Clear();
        ddlBasicSumAssure.Items.Add(new ListItem("--- Select ---", "0"));
        foreach (double d in sa)
        {
            ddlBasicSumAssure.Items.Add(new ListItem(d + "", d + ""));
        }

        if (sa.Count() == 1)
        {
            ddlBasicSumAssure.SelectedIndex = 1;
            ddlBasicSumAssure.Enabled = false;
        }
        else
        {
            ddlBasicSumAssure.Enabled = true;
        }
    }
    void BindRiderSA(double[] sa)
    {
        ddlRiderSumAssure.Items.Clear();
        ddlRiderSumAssure.Items.Add(new ListItem("--- Select ---", "0"));
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
            ddlRiderSumAssure.Enabled = true;
        }
    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {

        bl_micro_product_config proConf = new bl_micro_product_config();

        proConf = da_micro_product_config.ProductConfig.GetProductMicroProduct(ddlProduct.SelectedValue);
        //Session["SS_PRODUCT_CONF"] = proConf;

        MyProductConfig = proConf;

        try
        {
            List<bl_group_master_product> mList = da_group_master_product.GetGroupMasterProductList(MyProductConfig.Product_ID, ddlChannelItem.SelectedValue);
            PrivateGroupMasterProduct = mList[0];

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [ddlProduct_SelectedIndexChanged(object sender, EventArgs e)] in class [frm_gm_Customer_upload.aspx.cs], detail:" + ex.Message);
            Helper.Alert(true, "Event [ddlProduct_SelectedIndexChanged], Get Master Product Error.", lblError);
            return;
        }
        BindPayMode(proConf.PayMode);

        BindBaicSA(proConf.BasicSumAssuredRange);

        ddlPayPeriodType.Items.Clear();
        foreach (string myType in proConf.PayPeriodType)
        {
            var str = myType.Split(':');
            ddlPayPeriodType.Items.Add(new ListItem(str[0].ToString().Substring(0, 1), str[1].ToString()));
        }

        ddlCoverPeriodType.Items.Clear();
        foreach (string myType in proConf.CoverPeriodType)
        {
            var str = myType.Split(':');
            ddlCoverPeriodType.Items.Add(new ListItem(str[0].ToString().Substring(0, 1), str[1].ToString()));
        }

        ddlPayPeriodType_SelectedIndexChanged(null, null);
        ddlCoverPeriodType_SelectedIndexChanged(null, null);

        /*bind rider product*/

        if (!string.IsNullOrWhiteSpace(proConf.RiderProductID)) // has rider
        {

            ddlProductRider.Items.Add(new ListItem(proConf.RiderProductID, proConf.RiderProductID));
            BindRiderSA(proConf.RiderSumAssuredRange);
        }
        else // no rider configured
        {
            ddlProductRider.Items.Clear();
            ddlProductRider.Enabled = false;
            ddlRiderSumAssure.Items.Clear();
            ddlRiderSumAssure.Enabled = false;
        }

    }
    protected void ckbShow_CheckedChanged(object sender, EventArgs e)
    {
        //if (ckbShow.Checked)
        //{
        //    gv_valid.Visible = true;
        //}
        //else
        //{
        //    gv_valid.Visible = false;
        //}

        if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN.ToString())
        {
            gv_valid.Visible = ckbShow.Checked;
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER.ToString())
        {
            gvMoneyTransfer.Visible = ckbShow.Checked;
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL.ToString())
        {
            gvPayroll.Visible = ckbShow.Checked;
        }
        else if (ddlProjectType.SelectedValue == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN.ToString())
        {
            gv_digital_loan.Visible = ckbShow.Checked;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        List<bl_sale_agent_micro> agentList = da_sale_agent.GetSaleAgentMicroList(txtAgentName.Text);
        GridView1.DataSource = agentList;
        GridView1.DataBind();

    }
    protected void txtSaleAgentID_TextChanged(object sender, EventArgs e)
    {
        txtSaleAgentName.Text = "";
        List<bl_sale_agent_micro> agentList = da_sale_agent.GetSaleAgentMicroList(txtAgentName.Text);
        foreach (bl_sale_agent_micro agent in agentList.Where(_ => _.SaleAgentId == txtSaleAgentID.Text.Trim()))
        {
            txtSaleAgentName.Text = agent.FullNameEn;
            AgentInfo = agent;
            break;
        }
        if (AgentInfo == null)
        {
            Helper.Alert(true, "Agent code <strong>" + txtSaleAgentID.Text + "</strong> is not exist in system.", lblError);
        }

    }
    protected void btnExportInvalide_Click(object sender, EventArgs e)
    {
        if (Session["SS_DATA_INVALID"] != null)
        {
            DataTable tbl = (DataTable)Session["SS_DATA_INVALID"];
            List<string> colName = new List<string>();

            foreach (DataColumn col in tbl.Columns)
            {
                colName.Add(col.ColumnName);
            }

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
            Helper.excel.Sheet = sheet1;
            Helper.excel.Title = new string[] { "Invalide Record" };
            Helper.excel.HeaderText = colName.ToArray();
            Helper.excel.generateHeader();

            int row_no = 0;
            row_no = Helper.excel.NewRowIndex - 1;
            int colIndex = -1;
            foreach (DataRow r in tbl.Rows)
            {

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);
                foreach (DataColumn col in tbl.Columns)
                {
                    colIndex += 1;
                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(colIndex);
                    Cell1.SetCellValue(r[colIndex].ToString());
                }
                colIndex = -1;

            }
            string filename = "Invalide_Loan_Record_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);


            Response.BinaryWrite(file.GetBuffer());

            Response.End();
        }
    }

    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            var obj = e.Row;
            Label lbl1 = (Label)obj.FindControl("lblLoanPeriod");
            Label lbl2 = (Label)obj.FindControl("lblCoverableYear");

            if (Convert.ToInt32(lbl1.Text) != Convert.ToInt32(lbl2.Text))
            {

                obj.Cells[16].BackColor = System.Drawing.Color.Red;
            }

        }
    }

    private int CountPolicy(DataTable tbl, string idType, string IdNo, int year, da_micro_group_loan_upload.POJECT_TYPE_OPTIONS projectType)
    {
        int numberOfRecords = 0;
        if (tbl.Rows.Count > 0)
        {
            var r = tbl.Rows[0];
            if (projectType == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.LOAN)
            {
                string str = r["Disbursement Date(DD-MM-YYYY)"].ToString();
                numberOfRecords = tbl.Select("[id type] = '" + idType + "' and [id number] ='" + IdNo + "' AND  SUBSTRING([Disbursement Date(DD-MM-YYYY)],7,4)=" + year).Length;

            }
            else if (projectType == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.INTERNATIONAL_MONEY_TRANSFER)
            {
                string str = r["Effective_Date"].ToString();
                numberOfRecords = tbl.Select("[id_type] = '" + idType + "' and [id_number] ='" + IdNo + "' AND  SUBSTRING([Effective_Date],7,4)=" + year).Length;

            }
            else if (projectType == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.PAYROLL)
            {
                string str = r["Effective_Date(DD/MM/YYYY)"].ToString();
                numberOfRecords = tbl.Select("[ID_TYPE] = '" + idType + "' and [ID_No] ='" + IdNo + "' AND  SUBSTRING([Effective_Date(DD/MM/YYYY)],7,4)=" + year).Length;

            }
            else if (projectType == da_micro_group_loan_upload.POJECT_TYPE_OPTIONS.DIGITAL_LOAN)
            {
                string str = r["APPLIED_DATE(DD-MM-YYYY)"].ToString();
                numberOfRecords = tbl.Select("[ID_TYPE] = '" + idType + "' and [NID] ='" + IdNo + "' AND  SUBSTRING([APPLIED_DATE(DD-MM-YYYY)],7,4)=" + year).Length;

            }
        }
        return numberOfRecords;
    }

    protected void ddlPayPeriodType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayPeriodType.SelectedValue != "")
        {
            ddlPayPeriod.DataSource = Array.ConvertAll(ddlPayPeriodType.SelectedValue.Split(','), new Converter<string, Int32>(Int32.Parse)).ToList();
            ddlPayPeriod.DataBind();
        }
        else
        {
            ddlPayPeriod.Items.Clear();
        }
    }
    protected void ddlCoverPeriodType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCoverPeriodType.SelectedValue != "")
        {
            ddlCoverPeriod.DataSource = Array.ConvertAll(ddlCoverPeriodType.SelectedValue.Split(','), new Converter<string, Int32>(Int32.Parse)).ToList();
            ddlCoverPeriod.DataBind();
        }
        else
        {
            ddlCoverPeriod.Items.Clear();
        }
    }
    protected void gvMoneyTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["SS_DATA"] != null)
        {
            gvMoneyTransfer.PageIndex = e.NewPageIndex;
            DataTable tbl = (DataTable)Session["SS_DATA"];
            DataView dv = new DataView(tbl);

            gvMoneyTransfer.DataSource = dv;
            gvMoneyTransfer.DataBind();
            int countRow = 0;

            if (gvMoneyTransfer.PageCount == e.NewPageIndex + 1)//last page
            {
                countRow = gvMoneyTransfer.PageSize * (e.NewPageIndex) + gvMoneyTransfer.Rows.Count;
            }
            else
            {
                countRow = gvMoneyTransfer.PageSize * (e.NewPageIndex + 1);
            }
            lblRecords.Text = "Record(s): " + countRow + " of " + tbl.Rows.Count;
        }
    }
    protected void gvMoneyTransfer_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void gvMoneyTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvPayroll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gv_digital_loan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["SS_DATA"] != null)
        {
            gv_digital_loan.PageIndex = e.NewPageIndex;
            DataTable tbl = (DataTable)Session["SS_DATA"];
            DataView dv = new DataView(tbl);

            gv_digital_loan.DataSource = dv;
            gv_digital_loan.DataBind();
            int countRow = 0;

            if (gv_digital_loan.PageCount == e.NewPageIndex + 1)//last page
            {
                countRow = gv_digital_loan.PageSize * (e.NewPageIndex) + gv_digital_loan.Rows.Count;
            }
            else
            {
                countRow = gv_digital_loan.PageSize * (e.NewPageIndex + 1);
            }
            lblRecords.Text = "Record(s): " + countRow + " of " + tbl.Rows.Count;
        }
    }
}