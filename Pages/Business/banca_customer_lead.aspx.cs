using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.IO;
public partial class Pages_Business_banca_customer_lead : System.Web.UI.Page
{

    private string UserName { get { return ViewState["V_USER_NAME"] + ""; } set { ViewState["V_USER_NAME"] = value; } }
    private bool RoleIsIA { get { return (bool)ViewState["V_ROLE_IA"]; } set { ViewState["V_ROLE_IA"] = value; } }
    // string[] userRole = new string[] { };
    private string ApiUserId { get { return ViewState["V_API_USER_ID"] + ""; } set { ViewState["V_API_USER_ID"] = value; } }
    private string AgentName { get { return ViewState["V_AGENT_NAME"] + ""; } set { ViewState["V_AGENT_NAME"] = value; } }
    private string AgentId { get { return ViewState["V_AGETN_ID"] + ""; } set { ViewState["V_AGETN_ID"] = value; } }
    private string LeadId { get { return ViewState["V_LEAD_ID"] + ""; } set { ViewState["V_LEAD_ID"] = value; } }
    private string ChannelItemId { get { return ViewState["V_CHANNEL_ITEM_ID"] + ""; } set { ViewState["V_CHANNEL_ITEM_ID"] = value; } }
    private string ChannelLocationId { get { return ViewState["V_CHANNEL_LOCATION_ID"] + ""; } set { ViewState["V_CHANNEL_LOCATION_ID"] = value; } }
    private bool ValidateForm { get { return (bool)ViewState["V_VALIDATE_FORM"]; } set { ViewState["V_VALIDATE_FORM"] = value; } }
    private string ErrorMessage { get { return ViewState["V_ERROR_SMS"] + ""; } set { ViewState["V_ERROR_SMS"] = value; } }
    private string LeadStatus { get { return ViewState["V_LEAD_STATUS"] + ""; } set { ViewState["V_LEAD_STATUS"] = value; } }
    private bool PerView { get { return (bool)ViewState["V_PERM_VIEW"]; } set { ViewState["V_PERM_VIEW"] = value; } }
    private bool PerAdd { get { return (bool)ViewState["V_PERM_ADD"]; } set { ViewState["V_PERM_ADD"] = value; } }
    private bool PerUpdate { get { return (bool)ViewState["V_PERM_UPDATE"]; } set { ViewState["V_PERM_UPDATE"] = value; } }
    private bool PerAdmin { get { return (bool)ViewState["V_PERM_ADM"]; } set { ViewState["V_PERM_ADM"] = value; } }
    private string InsureApplicationNo { get { return ViewState["V_INSURE_NO"] + ""; } set { ViewState["V_INSURE_NO"] = value; } }
    private bl_sys_user_role UserRole { get { return (bl_sys_user_role)ViewState["V_USER_ROLE"]; } set { ViewState["V_USER_ROLE"] = value; } }
    
    /// <summary>
    /// Return Total channel
    /// </summary>
    /// <param name="ddl">channel dropdownlist name</param>
    /// <returns></returns>
    int LoadChannel(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<bl_channel_item> chList = new List<bl_channel_item>();
        if (RoleIsIA)
        {
            /*bind channel by user name*/
            chList = da_channel.GetChannelItemByUserName(UserName);
            //channel search
            Options.Bind(ddl, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");
            if (chList.Count == 1)
            {
                ddl.SelectedIndex = 1;
                ddl.Enabled = false;
                if (ddl == ddlChannel)
                {
                    //call channel search event to bind branch name
                    ddlChannel_SelectedIndexChanged(null, null);
                }
                else if (ddl == ddlChannelNameAdd)
                {
                    //call channel add event to bind branch name
                    ddlChannelNameAdd_SelectedIndexChanged(null, null);
                }
            }
        }
        else
        {
            chList = da_channel.GetChannelItemList();
            Options.Bind(ddl, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select --");

        }
        return chList.Count;
    }
    /// <summary>
    /// Return total branch
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="channelId"></param>
    /// <returns></returns>
    int LoadBranch(DropDownList ddl, string channelId)
    {
        ddl.Items.Clear();
        /*GET API USER ID*/
        ChannelItemUserConfig objconfig = new ChannelItemUserConfig();
        ChannelItemUserConfig uConfig = objconfig.GetByChannelItemId(channelId);
        ApiUserId = uConfig.UserId;
      
        List<bl_channel_location> locList=new List<bl_channel_location>();
        if (RoleIsIA)
        {
            locList = da_channel.GetChannelLocationByChannelItemIdUser(channelId, UserName);
            Options.Bind(ddl, locList, bl_channel_location.NAME.CombineName, bl_channel_location.NAME.OfficeCode, 0, "--- Select ---");
            if (locList.Count() == 1)
            {
                ddl.SelectedIndex = 1;
                ddl.Enabled = false;
                if (ddl == ddlBranchName)
                {
                    /*call event select change on branch name*/
                    ddlBranchName_SelectedIndexChanged(null, null);
                }
                else if (ddl == ddlBranchNameAdd)
                {
                    ddlBranchNameAdd_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                txtBranchCode.Text = "";
                ddl.Enabled = true;
            }
        }
        else
        {
            locList = da_channel.GetChannelLocationListByChannelItemID(channelId);
            Options.Bind(ddl, locList, bl_channel_location.NAME.OfficeName, bl_channel_location.NAME.OfficeCode, 0, "--- Select ---");
        }

        return locList.Count;
    }
    void Transaction(Helper.FormTransactionType type)
    {
        #region First Load
        if (type.ToString() == Helper.FormTransactionType.FIRST_LOAD.ToString())
        {
            List<bl_channel_item> chList = new List<bl_channel_item>();
            if (RoleIsIA)// AI and Team leader
            {
                //bind channel in dropdownlist search
                LoadChannel(ddlChannel);
              
                bl_channel_sale_agent chAgent = da_channel.GetChannelSaleAgentByUserName(UserName);
                //get agent information 
                AgentId = chAgent.SALE_AGENT_ID;
                AgentName = da_sale_agent.GetSaleAgentNameByID(chAgent.SALE_AGENT_ID);
            }
      
            else
            {
                chList = da_channel.GetChannelItemList();
                Options.Bind(ddlChannel, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select --");
                AgentId = string.Empty;
                AgentName = string.Empty;
            }
            /*Bind province*/
            BindProvince();
            BindNationality();
           

            txtReferredDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            EnableControl(false);
            divAdd.Visible =false;
            txtTopRow.Text = "50";
            txtBranchCode.Attributes.Add("disabled", "disabled");
            ckbShowAddress.Checked = false;
            LoadDefault();
        }
        #endregion
        #region Add New lead
        else if (type.ToString() == Helper.FormTransactionType.ADD_NEW.ToString())
        {
            try
            {
                if (PerAdd || PerAdmin)
                {
                    ClearText();
                    EnableControl();
                    btnDelete.Enabled = false;
                    divAdd.Visible = true;
                    divList.Visible = false;
                    //Bind channel name in dropdownlist ADD
                    LoadChannel(ddlChannelNameAdd);
                    // SET CAMBODIAN DEFAULT
                    Helper.SelectedDropDownListIndex("TEXT", ddlNationality, "Cambodian");
                   
                }
                else
                {
                    Helper.Alert(false, "Permission is required.", lblError);
                }
            }
            catch (Exception ex)
            {
                Helper.Alert(true, "Error function Transaction(ADD)", lblError);
                
            }
        }
        #endregion
        #region Save
        else if (type.ToString() == Helper.FormTransactionType.SAVE.ToString())
        {
            divList.Visible = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            ReleadUpdate();
            ClearText();
            //Bind channel name in dropdownlist ADD
            LoadChannel(ddlChannelNameAdd);
        }
        #endregion
        #region Update
        else if (type.ToString() == Helper.FormTransactionType.UPDATE.ToString())
        {

            divList.Visible = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            ReleadUpdate();
            ClearText();
            //Bind channel name in dropdownlist ADD
            LoadChannel(ddlChannelNameAdd);
        }
        #endregion
        #region Cancel
        else if (type.ToString() == Helper.FormTransactionType.CANCEL.ToString())
        {
            ClearText();
            EnableControl(false);
            divAdd.Visible = false;
            divList.Visible = true;
        }
        #endregion
        #region Edit
        else if (type.ToString() == Helper.FormTransactionType.EDIT.ToString())
        {
            ClearText();

            if (string.IsNullOrWhiteSpace(LeadStatus))
            {
                LoadChannel(ddlChannelNameAdd);
                if (String.IsNullOrWhiteSpace(InsureApplicationNo))
                {
                    if (PerAdmin)
                    {
                        EnableControl();
                       // LoadChannel(ddlChannelNameAdd);
                    }
                    else if (PerUpdate)
                    {
                        EnableControl();
                      //  LoadChannel(ddlChannelNameAdd);
                    }
                    else
                    {
                        Helper.Alert(false, "READ ONLY !!! Update lead is disable.", lblError);
                        EnableControl(false);
                        btnCancel.Enabled = true;
                    }
                }
                else
                {
                    Helper.Alert(false, "READ ONLY !!! Lead was already converted to Application.", lblError);
                    EnableControl(false);
                    btnCancel.Enabled = true;
                }
               
            }
            else if (LeadStatus == bl_customer_lead.LeadStaus.Approved.ToString() || LeadStatus == bl_customer_lead.LeadStaus.Reject.ToString() || LeadStatus == bl_customer_lead.LeadStaus.Delete.ToString())
                {
                    Helper.Alert(false, "Lead status [ " + LeadStatus + " ]. Update lead is disable.", lblError);
                    EnableControl(false);
                    btnCancel.Enabled = true;
                
            }
            divAdd.Visible = true;
            divList.Visible = false;
            btnDelete.Enabled = false;
        }
        #endregion
        #region Search
        else if (type.ToString() == Helper.FormTransactionType.SEARCH.ToString())
        {
            divList.Visible = true;
            SearchData();

            string logDesc = "User inquiries Lead with criteria [Channel:" + ddlChannel.SelectedValue + ", Branch Name:" + ddlBranchName.SelectedValue + (txtCustomerNameEnglish.Text.Trim()!="" ? ", Customer Name:" + txtCustomerNameEnglish.Text:"") + 
                (txtIDNumber.Text.Trim()!="" ? ", ID Number:" + txtIDNumber.Text :"")+ (ddlGender.SelectedIndex>0 ? ", Gender:" + ddlGender.SelectedValue :"")+ (txtDateOfBirth.Text.Trim()!="" ?", Dob:" + txtDateOfBirth.Text:"")+ "]";

            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, logDesc);

           // da_sys_activity_log.Save(new bl_sys_activity_log( UserName,UserRole.ObjectId, bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY,DateTime.Now,logDesc,Membership.ApplicationName ));

        }
        #endregion
        #region Reload
        else if (type.ToString() == Helper.FormTransactionType.RELOAD.ToString())
        {


        }
        #endregion
    }
    void EnableControl(bool t = true)
    {
        txtBranchCode.Enabled = t;
        txtApplicationId.Enabled = false;//always disable
        txtReferralStaffId.Enabled = t;
        txtReferralStaffName.Enabled = t;
        txtReferralStaffPosition.Enabled = t;
        txtReferredDate.Enabled = t;
        ddlClientType.Enabled = t;
        txtCif.Enabled = t;
        txtClientLastNameEN.Enabled = t;
        txtClientFirstNameEn.Enabled = t;
        txtClientLastNameKh.Enabled = t;
        txtClientFirstNameKh.Enabled = t;
        ddlChannelNameAdd.Enabled = t;
        ddlBranchNameAdd.Enabled = t;
        ddlGenderAdd.Enabled = t;
        ddlNationality.Enabled = t;
        txtIdNo.Enabled = t;
        txtDateOfBirthAdd.Enabled = t;

        txtPhoneNumber.Enabled = t;
        ddlProvince.Enabled = t;
        ddlDistrict.Enabled = t;
        ddlCommune.Enabled = t;
        ddlVillage.Enabled = t;
        txtRemarks.Enabled = t;

        btnSave.Enabled = t;
        btnDelete.Enabled = t;
        btnCancel.Enabled = t;
    }
    void ClearText()
    {
        try
        {
            ddlBranchNameAdd.SelectedIndex = ddlBranchNameAdd.Items.Count > 0 ? 0 : -1 ;
            txtReferralStaffId.Text = "";
            txtReferralStaffName.Text = "";
            txtReferralStaffPosition.Text = "";
            txtReferredDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtApplicationId.Text = "";
            ddlClientType.SelectedIndex = 0;
            txtCif.Text = "";
            txtClientLastNameEN.Text = "";
            txtClientFirstNameEn.Text = "";
            txtClientLastNameKh.Text = "";
            txtClientFirstNameKh.Text = "";
            ddlGenderAdd.SelectedIndex = 0;
            ddlNationality.SelectedIndex = 0;
            ddlIdType.SelectedIndex = 0;
            txtIdNo.Text = "";
            txtPhoneNumber.Text = "";
            txtDateOfBirthAdd.Text = "";
            ddlProvince.SelectedIndex = 0;
            ddlDistrict.Items.Clear();
            ddlCommune.Items.Clear();
            ddlVillage.Items.Clear();
            LeadId = string.Empty;
            txtRemarks.Text = "";
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error function ClearText()", lblError);
            
        }
    }
    bool FormIsValide()
    {
        if (ddlChannelNameAdd.SelectedValue == "")
        {
            ErrorMessage = "Channel is required.";
            return false;
        }
        else if (ddlBranchNameAdd.SelectedValue == "")
        {
            ErrorMessage = "Branch Name is required.";
            return false;
        }
        else if (txtReferralStaffId.Text.Trim() == "")
        {
            ErrorMessage = "Referral Staff ID is required.";
            return false;
        }
        else if (txtReferralStaffName.Text.Trim() == "")
        {
            ErrorMessage = "Referral Staff Name is required.";
            return false;
        }
        else if (!Helper.IsDate(txtReferredDate.Text.Trim()))
        {
            ErrorMessage = "Referred Date is required. Format {DD/MM/YYYY}";
            return false;
        }
        else if (ddlClientType.SelectedValue == "")
        {
            ErrorMessage = "Client Type is required.";
            return false;
        }
        else if (txtClientLastNameEN.Text.Trim() == "")
        {
            ErrorMessage = "Last Name (EN) is required.";
            return false;
        }
        else if (txtClientFirstNameEn.Text.Trim() == "")
        {
            ErrorMessage = "First Name (EN) is required.";
            return false;
        }
        else if (txtClientLastNameKh.Text.Trim() == "")
        {
            ErrorMessage = "Last Name (KH) is required.";
            return false;
        }
        else if (txtClientFirstNameKh.Text.Trim() == "")
        {
            ErrorMessage = "First Name (KH) is required.";
            return false;
        }
        else if (ddlGenderAdd.SelectedValue == "")
        {
            ErrorMessage = "Gender is required.";
            return false;
        }
        else if (ddlNationality.SelectedValue == "")
        {
            ErrorMessage = "Nationality is required.";
            return false;
        }
        else if (!Helper.IsDate(txtDateOfBirthAdd.Text.Trim()))
        {
            ErrorMessage = "Date Of Birth is required. Format {DD/MM/YYYY}";
            return false;
        }
        else if (ddlIdType.SelectedValue == "")
        {
            ErrorMessage = "ID Type is required.";
            return false;
        }
        else if (txtIdNo.Text.Trim() == "")
        {
            ErrorMessage = "ID No. is required.";
            return false;
        }
        else if (ddlProvince.SelectedValue == "")
        {
            ErrorMessage = "Province is required.";
            return false;
        }
        else
        {
            ErrorMessage = string.Empty;
            return true;
        }
    }
    void BindNationality()
    {
        DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SELECT nationality from ct_country order by  nationality;");
        Options.Bind(ddlNationality, tbl, "nationality", "nationality", 0, "--- Select ---");
    }

    void BindProvince()
    {
        List<bl_address.province> ProList = da_address.province.GetProvince();
        ddlProvince.Items.Clear();
        ddlProvince.Items.Add(new ListItem("--ខេត្ត/ក្រុង--", ""));
        if (ProList.Count > 0)
        {
            foreach (bl_address.province pro in ProList)
            {
                ddlProvince.Items.Add(new ListItem(pro.Khmer, pro.Code));
            }

        }
    }
    void BindDistrict()
    {
        if (ddlProvince.SelectedValue != "")
        {

            List<bl_address.district> disList = da_address.district.GetDistrict(ddlProvince.SelectedValue);// (GetProvinceValue(ValueOption.CODE));
            ddlDistrict.Items.Clear();
            ddlDistrict.Items.Add(new ListItem("--ស្រុក/ខណ្ឌ--", ""));
            if (disList.Count > 0)
            {

                foreach (bl_address.district dis in disList)
                {
                    ddlDistrict.Items.Add(new ListItem(dis.Khmer, dis.Code));
                }
            }
        }


    }
    void BindCommune()
    {
        if (ddlDistrict.SelectedValue != "")
        {

            List<bl_address.commune> comList = da_address.commune.GetCommune(ddlDistrict.SelectedValue);// (GetDisctrictValue(ValueOption.CODE));
            ddlCommune.Items.Clear();
            ddlCommune.Items.Add(new ListItem("--ឃុំ/សង្កាត់--", ""));
            if (comList.Count > 0)
            {

                foreach (bl_address.commune com in comList)
                {
                    ddlCommune.Items.Add(new ListItem(com.Khmer, com.Code));
                }
            }
        }


    }
    void BindVillage()
    {
        if (ddlCommune.SelectedValue != "")
        {

            List<bl_address.village> viList = da_address.village.GetVillage(ddlCommune.SelectedValue);// (GetCommuneValue(ValueOption.CODE));
            ddlVillage.Items.Clear();
            ddlVillage.Items.Add(new ListItem("--ភូមិ--", ""));
            if (viList.Count > 0)
            {

                foreach (bl_address.village vi in viList)
                {
                    ddlVillage.Items.Add(new ListItem(vi.Khmer, vi.Code));
                }
            }
        }


    }
    enum ValueOption { CODE, NAME }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";

        UserName = Membership.GetUser().UserName;

        string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
        List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
        bl_sys_user_role ur = new bl_sys_user_role();
        bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);

        UserRole = u;

        if (u.ObjectCode == "" || u.ObjectCode == null)
        {
            Response.Redirect("../../unauthorize.aspx");
        }
        else
        {
            PerView = u.IsView == 1 ? true : false;
            PerAdd = u.IsAdd == 1 ? true : false;
            PerAdmin = u.IsAdmin == 1 ? true : false;
            PerUpdate = u.IsUpdate == 1 ? true : false;
            List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];
            if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
            {
                RoleIsIA = true;
            }
            else
            {
                RoleIsIA = false;
            }
            if (!Page.IsPostBack)
            {
                Transaction(Helper.FormTransactionType.FIRST_LOAD);

            }
        }
    }
    void ShowAddress(bool show = true)
    {
        gv_valid.Columns[15].Visible = show;
        gv_valid.Columns[16].Visible = show;
        gv_valid.Columns[17].Visible = show;
        gv_valid.Columns[18].Visible = show;
    }

    void LoadDefault()
    {
        LoadData("");
    }

    void SearchData()
    {

        string[, ,] condition = new string[,,] { };
        List<string[, ,]> list_cond = new List<string[, ,]>();
        string str_formated_condition = "";

        if (txtCustomerNameEnglish.Text.Trim() != "")
        {
            condition = new string[,,] { { { "CLIENT_NAME_IN_ENGLISH", "LIKE", txtCustomerNameEnglish.Text.Trim() } } };
            list_cond.Add(condition);
        }
        if (txtDateOfBirth.Text.Trim() != "")
        {
            if (Helper.IsDate(txtDateOfBirth.Text))
            {
                list_cond.Add(new string[,,] { { { "DATE_OF_BIRTH", "=", Helper.FormatDateTime(txtDateOfBirth.Text.Trim()) + "" } } });
            }
            else
            {
                Helper.Alert(true, "Date of Birth is invalid format.", lblError);

            }
        }
        if (ddlGender.SelectedIndex > 0)
        {
            list_cond.Add(new string[,,] { { { "GENDER", "=", ddlGender.SelectedValue } } });
        }
        if (txtIDNumber.Text.Trim() != "")
        {
            list_cond.Add(new string[,,] { { { "ID_NUMBER", "=", txtIDNumber.Text.Trim() } } });
        }
        str_formated_condition = new DB().FormatSQLConditionFilds(list_cond);

        LoadData(str_formated_condition);
    }
    void ReleadSave()
    {
        string[, ,] condition = new string[,,] { };
        List<string[, ,]> list_cond = new List<string[, ,]>();
        string str_formated_condition = "";
        list_cond.Add(new string[,,] { { { "ID", "=", LeadId } } });

        str_formated_condition = new DB().FormatSQLConditionFilds(list_cond);

        LoadData(str_formated_condition);
    }
    void ReleadUpdate()
    {
        List<bl_customer_lead> cusList = new List<bl_customer_lead>();
        string[, ,] condition = new string[,,] { };
        List<string[, ,]> list_cond = new List<string[, ,]>();
        string str_formated_condition = "";
        list_cond.Add(new string[,,] { { { "ID", "=", LeadId } } });

        str_formated_condition = new DB().FormatSQLConditionFilds(list_cond);

     
            cusList = da_customer_lead.GetCustomerLead(ddlBranchNameAdd.SelectedValue, str_formated_condition, txtTopRow.Text.Trim() == "" ? 0 : Convert.ToInt32(txtTopRow.Text.Trim()));
        

        if (cusList.Count > 0)
        {
            gv_valid.DataSource = cusList;
            Session["CUSLIST"] = cusList;
            gv_valid.DataBind();
            ShowAddress(ckbShowAddress.Checked == true ? true : false);
            lblRecords.Text = gv_valid.Rows.Count + " Of " + cusList.Count + " Records";
        }
        else
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            lblRecords.Text = "";
        }
    }
    void LoadData(string CONDITION)
    {

        List<bl_customer_lead> cusList = new List<bl_customer_lead>();
       

        if (txtBranchCode.Text.Trim() != "")
        {
            cusList = da_customer_lead.GetCustomerLead(txtBranchCode.Text.Trim(), CONDITION, txtTopRow.Text.Trim() == "" ? 0 : Convert.ToInt32(txtTopRow.Text.Trim()));
        }

        if (cusList.Count > 0)
        {
            gv_valid.DataSource = cusList;
            Session["CUSLIST"] = cusList;
            gv_valid.DataBind();
            ShowAddress(ckbShowAddress.Checked == true ? true : false);
            lblRecords.Text = gv_valid.Rows.Count + " Of " + cusList.Count + " Records";
        }
        else
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            lblRecords.Text = "";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtBranchCode.Text.Trim() != "")
        {
            Transaction(Helper.FormTransactionType.SEARCH);
        }
        else
        {
            Helper.Alert(false, "Please select branch name before seaching lead.", lblError);
        }
    }

    bool ConvertToApp(string APPLICATION_ID, string CREATED_BY, DateTime CREATED_ON, string CHANNEL_ITEM_ID, string CHANNEL_LOCATION_ID, string SALE_AGENT_ID, string SALE_AGENT_NAME, out string MESSAGE)
    {
        bool result = false;
        try
        {
            result = da_customer_lead.CopyToApplication(APPLICATION_ID, CREATED_BY, CREATED_ON, CHANNEL_ITEM_ID, CHANNEL_LOCATION_ID, SALE_AGENT_ID, SALE_AGENT_NAME);
            MESSAGE = da_customer_lead.MESSAGE;
        }
        catch (Exception ex)
        {
            MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error: [" + UserName + "], function [ConvertToApp(string APPLICATION_ID, string CREATED_BY, DateTime CREATED_ON, string CHANNEL_ITEM_ID , string CHANNEL_LOCATION_ID , string SALE_AGENT_ID , string SALE_AGENT_NAME, out string MESSAGE)] in class [banca_customer_lead.aspx.cs], detail: " + ex.Message + "=>" + ex.StackTrace);
        }
        return result;
    }
    protected void gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        #region Label
        Label app_id;
        Label id;
        Label app_no;
        Label lCIF;
        Label lLeadType;
        Label lClientType;
        Label lClientName;
        Label lClientDob;
        Label lClientGender;
        Label lClientIdType;
        Label lClientIdNumber;
        Label lClientNameKh;
        Label lNationality;
        Label lPhoneNumber;
        Label lProvince;
        Label lDistrict;
        Label lCommune;
        Label lVillage;
        Label lReferralId;
        Label lReferralName;
        Label lReferredDate;
        Label lReferralPosition;
        Label lBranchName;
        Label lRemarks;
        Label lApiUser;
        Label lBranchCode;
        #endregion
        string application_id = "";
        string lead_id = "";
        string insurace_app_no = ""; string MESSAGE = "";
        string cif = "";
        string leadType = "";
        string clientType = "";
        string status = "";

        string clientName = "";
        DateTime clientDob = new DateTime(1900, 1, 1);
        string clientGender = "";
        string clientIdType = "";
        string clientIdNumber = "";
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow g_row;
            GridView g = sender as GridView;
            g_row = g.Rows[index];
            app_id = (Label)g_row.FindControl("lblApplicationID");
            id = (Label)g_row.FindControl("lblID");
            app_no = (Label)g_row.FindControl("lblInsuranceApplicationNumber");
            application_id = app_id.Text;
            lead_id = id.Text;
            LeadId = id.Text;


            lCIF = (Label)g_row.FindControl("lblCIF");
            cif = lCIF.Text;

            lLeadType = (Label)g_row.FindControl("lblLeadType");
            leadType = lLeadType.Text;

            lClientType = (Label)g_row.FindControl("lblClientType");
            clientType = lClientType.Text;

            leadType = leadType == "" || leadType == "New" ? "New" : leadType;

            lClientName = (Label)g_row.FindControl("lblClientNameEnglish");
            clientName = lClientName.Text;
            lClientGender = (Label)g_row.FindControl("lblGender");
            clientGender = lClientGender.Text;
            lClientDob = (Label)g_row.FindControl("lblDateOfBirth");
            clientDob = Helper.FormatDateTime(lClientDob.Text);
            lClientIdType = (Label)g_row.FindControl("lblIDType");
            clientIdType = lClientIdType.Text;
            lClientIdNumber = (Label)g_row.FindControl("lblIDNumber");
            clientIdNumber = lClientIdNumber.Text;
            lPhoneNumber = (Label)g_row.FindControl("lblPhoneNumber");
            lNationality = (Label)g_row.FindControl("lblNationality");
            lProvince = (Label)g_row.FindControl("lblProvince");
            lDistrict = (Label)g_row.FindControl("lblDistrict");
            lCommune = (Label)g_row.FindControl("lblCommune");
            lVillage = (Label)g_row.FindControl("lblVillage");
            lClientNameKh = (Label)g_row.FindControl("lblClientNameKhmer");

            lReferralName = (Label)g_row.FindControl("lblReferralStaffName");
            lReferralId = (Label)g_row.FindControl("lblReferralStaffID");
            lReferralPosition = (Label)g_row.FindControl("lblReferralStaffPosition");
            lReferredDate = (Label)g_row.FindControl("lblReferredDate");

            lBranchName = (Label)g_row.FindControl("lblBranchName");
            lRemarks = (Label)g_row.FindControl("lblRemarks");
            lApiUser = (Label)g_row.FindControl("lblApiUserId");
            lBranchCode = (Label)g_row.FindControl("lblBranchCode");

            insurace_app_no = ((Label)g_row.FindControl("lblInsuranceApplicationNumber")).Text;
            //purpose for using in add new lead form
            InsureApplicationNo = insurace_app_no;

            status = ((Label)g_row.FindControl("lblStatus")).Text;
            LeadStatus = status;
            #region COPY
            if (e.CommandName == "CMD_COPY")
            {

                if (string.IsNullOrWhiteSpace(ChannelItemId) == true )
                {
                    Helper.Alert(true, "Channel CODE/NAME is not found.", lblError);
                }
                else if (string.IsNullOrWhiteSpace(ChannelLocationId) == true)
                {
                    Helper.Alert(true, "Branch CODE/NAME is not found.", lblError);
                }
                else if (string.IsNullOrWhiteSpace(AgentId) == true || string.IsNullOrWhiteSpace(AgentName) == true)
                {
                    Helper.Alert(true, "Sale agent CODE/NAME is not found.", lblError);
                }
                else if (status.ToUpper() == "DELETE")
                {
                    Helper.Alert(true, "Lead status is 'DELETE', System not allow to convert.", lblError);
                }
                else if (status.ToUpper() == "REJECT")
                {
                    Helper.Alert(true, "Lead status is 'REJECT', System not allow to convert.", lblError);
                }
                else
                {
                    #region backup V1
                    /* string url = "banca_micro_application.aspx";
                    SESSION_PARA para = new SESSION_PARA();
                    para.ChannelItemId = hdfChannelItemID.Value;
                    para.ChannelLocationId = hdfChannelLocationID.Value;
                    para.AgentCode = hdfSaleAgentID.Value;
                    para.AgentName = hdfSaleAgentName.Value;
                    para.BranchCode = txtBranchCode.Text;
                    para.BranchName = ddlBranchName.SelectedItem.Text;
                    para.ApplicationType = leadType;
                    para.NewApplicationNumber = insurace_app_no;

                    bool processNext = true;
                    string sms = "";

                    double remainExpiryDays = 0;
                    int policyExpirDays = 0;
                    int allowBeforeExpireDays = AppConfiguration.AllowMultiPolicyBeforeExpireDays();
                    int allowAfterExpireDays = AppConfiguration.AllowMRepaymentPolicyAfterExpireDays();

                    DataTable tbl = new DataTable();
                    bl_micro_application app = new bl_micro_application();

                    bl_micro_policy pol = new bl_micro_policy();
                    bl_micro_policy_detail polD = new bl_micro_policy_detail();
                    bl_customer_lead objLead = da_customer_lead.GetCustomerLeadByLeadID(lead_id);
                    if (objLead.ID != null && objLead.ID != "")
                    {

                        app = da_micro_application.GetApplication(objLead.InsuranceApplicationId);
                        para.OldPolicyNumber = app.RENEW_FROM_POLICY;

                        if (objLead.InsuranceApplicationId != "" && (objLead.Status != "" || objLead.Status == ""))//current selected row has application number and remarks
                        {
                            //get policy status

                            pol = da_micro_policy.GetPolicyByNumber(app.RENEW_FROM_POLICY);
                            if (da_micro_policy.SUCCESS)
                            {
                                polD = da_micro_policy_detail.GetPolicyDetailByPolicyID(pol.POLICY_ID);
                                if (da_micro_policy_detail.SUCCESS)
                                {
                                    //old lead already convert to application / issue policy
                                    //user able to view application or issue policy
                                    para.NewApplicationNumber = objLead.InsuranceApplicationId;
                                    para.OldPolicyNumber = pol.POLICY_NUMBER;
                                    para.OldPolicyStatus = pol.POLICY_STATUS;
                                    para.OldPolicyExpiryDate = pol.POLICY_NUMBER != "" ? polD.EXPIRY_DATE : new DateTime(1900, 1, 1);
                                    para.ApplicationType = objLead.LeadType;
                                    para.CustomerId = null;
                                    para.OldPolicyId = null;
                                    para.OldApplicationNumber = null;
                                    processNext = true;
                                }
                                else
                                {
                                    processNext = false;
                                    sms = da_micro_policy_detail.MESSAGE;
                                }


                            }
                            else
                            {
                                processNext = false;
                                sms = da_micro_policy.MESSAGE;
                            }

                        }

                        else // current selected row has no application number & remarks
                        {
                            //get count policy to check the policy status
                            if (clientType != "" && clientType.ToUpper() != "FAMILY")
                            {
                                tbl = da_banca.GetLeadApplicationPolicy(cif);//count policies 
                            }
                            else
                            {
                                tbl = da_banca.GetLeadApplicationPolicy(clientName, clientGender, clientDob, clientIdType, clientIdNumber);
                            }
                            if (da_banca.SUCCESS)//get policy not error
                            {
                                if (tbl.Rows.Count > 0)// has policy
                                {
                                    var r = tbl.Rows[0];
                                    remainExpiryDays = Convert.ToDateTime(r["expiry_date"].ToString()).Date.Subtract(DateTime.Now.Date).Days;//calculate remain expire days
                                    policyExpirDays = DateTime.Now.Date.Subtract(Convert.ToDateTime(r["expiry_date"].ToString()).Date).Days;
                                    para.OldPolicyStatus = r["policy_status"].ToString();
                                    para.OldPolicyExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString());
                                    para.OldPolicyNumber = r["policy_number"].ToString();
                                    para.CustomerId = r["customer_id"].ToString();
                                    para.OldPolicyId = r["policy_id"].ToString();
                                    para.OldApplicationNumber = r["application_number"].ToString();
                                    //check policy status
                                    if (para.OldPolicyStatus == "IF") // previous policy status is In-force
                                    {
                                        //check multi policy is allow or not
                                        if (AppConfiguration.AllowMultiPolicyPerLife())//  system is allow multi policy 
                                        {
                                            if (remainExpiryDays <= allowBeforeExpireDays)
                                            {
                                                processNext = true;

                                            }
                                            else
                                            {
                                                processNext = false;
                                                sms = clientName + " has one policy [" + para.OldPolicyNumber + "] is still In-force which will be expiried in " + remainExpiryDays + " days. System is not allowed to proceed.";
                                            }
                                        }
                                        else//  system is not allow multi policy 
                                        {
                                            processNext = false;
                                            sms = "System is not allow issue multi policies.";
                                        }
                                    }
                                    else if (para.OldPolicyStatus == "EX")// previous policy status is expired
                                    {
                                        processNext = true;
                                        if (policyExpirDays <= allowAfterExpireDays)
                                        {
                                            //policy expired in 365 days system counts as repayment, this value can be changed in web config "ALLOW-REPAYMENT-POLICY-AFTER-EXPIRE-DAYS"
                                            para.ApplicationType = "Repayment";
                                        }
                                        else
                                        {
                                            //policy expired more than 365 days system counts as new policy
                                            para.ApplicationType = "New";

                                        }
                                    }
                                }
                                else// no policy found
                                {
                                    //new lead is detected 
                                    para.OldPolicyStatus = null;
                                    para.OldPolicyExpiryDate = new DateTime(1900, 1, 1);
                                    para.OldPolicyNumber = null;
                                    para.CustomerId = null;
                                    para.OldPolicyId = null;
                                    para.OldApplicationNumber = null;
                                    processNext = true;
                                }
                            }
                            else //get policy error
                            {
                                processNext = false;
                                sms = da_banca.MESSAGE;
                            }
                        }
                    }

                    if (processNext)
                    {

                        if (para.NewApplicationNumber != "")//exist
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?ID=" + id.Text + "&APP_ID=" + app.APPLICATION_ID + "');</script>", false);
                        }
                        else //not exist
                        {
                            //new application
                            bool result = ConvertToApp(app_id.Text, userName, DateTime.Now, hdfChannelItemID.Value, hdfChannelLocationID.Value, hdfSaleAgentID.Value, hdfSaleAgentName.Value, out MESSAGE);

                            if (result)
                            {

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?ID=" + id.Text + "&APP_ID=" + "');</script>", false);
                            }
                            else
                            {
                                Helper.Alert(true, "Ooop! system cannot convert lead to application, please contact your system administrator.", lblError);

                            }

                        }
                        //store session
                        Session["SS_SESSION_LEAD"] = para;


                    }
                    else
                    {

                        // Helper.Alert(true, "Policy [" + tbl.Rows[0]["policy_number"] + "] is still in-force." + remainExpiryDays, lblError);
                        Helper.Alert(true, sms, lblError);
                    }
                    */
                    #endregion backup V1

                    #region v2
                    /*updated by maneth @26 oct 2023
                     add condition to check multi policy type [NEW, REPAYMENT]
                     */

                    string url = "banca_micro_application.aspx";
                    SESSION_PARA para = new SESSION_PARA();
                    para.ChannelItemId = ChannelItemId;
                    para.ChannelLocationId = ChannelLocationId;
                    para.AgentCode = AgentId;
                    para.AgentName = AgentName;
                    para.BranchCode = txtBranchCode.Text;
                    para.BranchName = ddlBranchName.SelectedItem.Text;
                    para.ApplicationType = leadType;
                    para.NewApplicationNumber = insurace_app_no;

                    bool processNext = true;
                    string sms = "";

                    double remainExpiryDays = 0;
                    int policyExpirDays = 0;
                    int allowBeforeExpireDays = AppConfiguration.AllowMultiRepaymentPolicyBeforeExpireDays();
                    int allowAfterExpireDays = AppConfiguration.AllowMRepaymentPolicyAfterExpireDays();

                    DataTable tbl = new DataTable();
                    bl_micro_application app = new bl_micro_application();

                    bl_micro_policy pol = new bl_micro_policy();
                    bl_micro_policy_detail polD = new bl_micro_policy_detail();
                    bl_customer_lead objLead = da_customer_lead.GetCustomerLeadByLeadID(lead_id);
                    if (objLead.ID != null && objLead.ID != "")
                    {

                        app = da_micro_application.GetApplication(objLead.InsuranceApplicationId);
                        para.OldPolicyNumber = app.RENEW_FROM_POLICY;

                        if (objLead.InsuranceApplicationId != "" && (objLead.Status != "" || objLead.Status == ""))//current selected row has application number and remarks
                        {
                            #region Existing application
                            //get policy status

                            pol = da_micro_policy.GetPolicyByNumber(app.RENEW_FROM_POLICY);
                            if (da_micro_policy.SUCCESS)
                            {
                                polD = da_micro_policy_detail.GetPolicyDetailByPolicyID(pol.POLICY_ID);
                                if (da_micro_policy_detail.SUCCESS)
                                {
                                    //old lead already convert to application / issue policy
                                    //user able to view application or issue policy
                                    para.NewApplicationNumber = objLead.InsuranceApplicationId;
                                    para.OldPolicyNumber = pol.POLICY_NUMBER;
                                    para.OldPolicyStatus = pol.POLICY_STATUS;
                                    para.OldPolicyExpiryDate = pol.POLICY_NUMBER != "" ? polD.EXPIRY_DATE : new DateTime(1900, 1, 1);
                                    para.ApplicationType = objLead.LeadType;
                                    para.CustomerId = null;
                                    para.OldPolicyId = null;
                                    para.OldApplicationNumber = null;
                                    processNext = true;
                                }
                                else
                                {
                                    processNext = false;
                                    sms = da_micro_policy_detail.MESSAGE;
                                }

                            }
                            else
                            {
                                processNext = false;
                                sms = da_micro_policy.MESSAGE;
                            }
                            #endregion Existing application
                        }

                        else // current selected row has no application number & remarks
                        {
                            #region check on channel user config
                            ChannelItemUserConfig chUserConf = new ChannelItemUserConfig();
                            ChannelItemUserConfig chUser = chUserConf.GetByUserId(objLead.ApiUser);
                            if (chUserConf.Transection)//get channel user config success
                            {
                                if (string.IsNullOrEmpty(chUser.ChannelItemId)) //channel user config is not exist
                                {
                                    //show error message
                                    processNext = false;
                                    sms = "The specific user in lead posting is not yet configure in channel user configuration.";

                                }
                                else //channel user config is  exist
                                {
                                    #region check on channel item config
                                    Channel_Item_Config ch = new Channel_Item_Config();
                                    List<Channel_Item_Config> lCh = ch.GetChannelItemConfig(chUser.ChannelItemId, 1);
                                    if (Channel_Item_Config.Transection && lCh.Count>0)
                                    {
                                        var chConfig = lCh[0];

                                        //get count policy to check the policy status
                                        if (clientType != "" && clientType.ToUpper() != "FAMILY")
                                        {
                                            if (cif != "")
                                            {
                                                tbl = da_banca.GetLeadApplicationPolicy(cif);//count policies 
                                            }
                                            else // purpose for jtrust
                                            {
                                                tbl = da_banca.GetLeadApplicationPolicy(clientName, clientGender, clientDob, clientIdType, clientIdNumber);
                                            }
                                        }
                                        else
                                        {
                                            tbl = da_banca.GetLeadApplicationPolicy(clientName, clientGender, clientDob, clientIdType, clientIdNumber);
                                        }
                                        if (da_banca.SUCCESS)//get policy not error
                                        {
                                            //if (AppConfiguration.CheckMultiPolicy() == AppConfiguration.MultiPolicyType.REPAYMENT.ToString())
                                            if (AppConfiguration.CheckMultiPolicy() == bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.CHECK_MULTI_POLICY_OPTION.OPTION.REPAYMENT)
                                            {

                                                if (tbl.Rows.Count > 0)// has policy
                                                {
                                                    var r = tbl.Rows[0];
                                                    remainExpiryDays = Convert.ToDateTime(r["expiry_date"].ToString()).Date.Subtract(DateTime.Now.Date).Days;//calculate remain expire days
                                                    policyExpirDays = DateTime.Now.Date.Subtract(Convert.ToDateTime(r["expiry_date"].ToString()).Date).Days;
                                                    para.OldPolicyStatus = r["policy_status"].ToString();
                                                    para.OldPolicyExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString());
                                                    para.OldPolicyNumber = r["policy_number"].ToString();
                                                    para.CustomerId = r["customer_id"].ToString();
                                                    para.OldPolicyId = r["policy_id"].ToString();
                                                    para.OldApplicationNumber = r["application_number"].ToString();
                                                    //check policy status
                                                    if (para.OldPolicyStatus == "IF") // previous policy status is In-force
                                                    {
                                                        //check multi policy is allow or not
                                                        if (AppConfiguration.AllowMultiRepaymentPolicyPerLife())//  system is allow multi repayment policy 
                                                        {
                                                            if (remainExpiryDays <= allowBeforeExpireDays)
                                                            {
                                                                processNext = true;

                                                            }
                                                            else
                                                            {
                                                                processNext = false;
                                                                sms = clientName + " has one policy [" + para.OldPolicyNumber + "] is still In-force which will be expiried in " + remainExpiryDays + " days. System is not allowed to proceed.";
                                                            }
                                                        }
                                                        else//  system is not allow multi policy 
                                                        {
                                                            processNext = false;
                                                            sms = "System is not allow issue multi policies.";
                                                        }
                                                    }
                                                    else if (para.OldPolicyStatus == "EX")// previous policy status is expired
                                                    {
                                                        processNext = true;
                                                        if (policyExpirDays <= allowAfterExpireDays)
                                                        {
                                                            //policy expired in 365 days system counts as repayment, this value can be changed in web config "ALLOW-REPAYMENT-POLICY-AFTER-EXPIRE-DAYS"
                                                            para.ApplicationType = "Repayment";
                                                        }
                                                        else
                                                        {
                                                            //policy expired more than 365 days system counts as new policy
                                                            para.ApplicationType = "New";

                                                        }
                                                    }
                                                }
                                                else// no policy found
                                                {
                                                    //new lead is detected 
                                                    para.OldPolicyStatus = null;
                                                    para.OldPolicyExpiryDate = new DateTime(1900, 1, 1);
                                                    para.OldPolicyNumber = null;
                                                    para.CustomerId = null;
                                                    para.OldPolicyId = null;
                                                    para.OldApplicationNumber = null;
                                                    processNext = true;
                                                }


                                            }
                                            //else if (AppConfiguration.CheckMultiPolicy() == AppConfiguration.MultiPolicyType.NEW.ToString())
                                            else if (AppConfiguration.CheckMultiPolicy() == bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.CHECK_MULTI_POLICY_OPTION.OPTION.NEW)
                                            {
                                                if (tbl.Rows.Count > 0)//existing policy found
                                                {
                                                    List<DataRow> dataInforce = tbl.AsEnumerable().Where(_ => _["policy_status"].ToString() == "IF").ToList();
                                                    int policyInf = dataInforce.Count();// count inforce policy
                                                    string exPolicy = "";
                                                    if (policyInf > 0)
                                                    {
                                                        foreach (DataRow row in dataInforce)
                                                        {
                                                            exPolicy += exPolicy == "" ? row["policy_number"].ToString() : "," + row["policy_number"].ToString();
                                                        }

                                                        if (AppConfiguration.AllowMultiNewPolicyPerLife())
                                                        {
                                                            if (policyInf >= chConfig.MaxPolicyPerLife)
                                                            {
                                                                processNext = false;
                                                                sms = "Number of policy is reach to limited number [" + policyInf + "], policy number [" + exPolicy + "].";

                                                            }
                                                            else
                                                            {
                                                                para.OldPolicyStatus = null;
                                                                para.OldPolicyExpiryDate = new DateTime(1900, 1, 1);
                                                                para.OldPolicyNumber = null;
                                                                para.CustomerId = null;
                                                                para.OldPolicyId = null;
                                                                para.OldApplicationNumber = null;
                                                                processNext = true;
                                                                para.ApplicationType = "New";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            processNext = false;
                                                            sms = "Multi policy is detected, system is not allowed to proceed, Exist policy number [" + exPolicy + "].";
                                                        }
                                                    }
                                                }
                                                else // existing policy not found
                                                {

                                                    para.OldPolicyStatus = null;
                                                    para.OldPolicyExpiryDate = new DateTime(1900, 1, 1);
                                                    para.OldPolicyNumber = null;
                                                    para.CustomerId = null;
                                                    para.OldPolicyId = null;
                                                    para.OldApplicationNumber = null;
                                                    processNext = true;
                                                    para.ApplicationType = "New";
                                                }
                                            }
                                            else// system config error
                                            {
                                                processNext = false;
                                                sms = "System configuration file is getting error, please contact your system administrator.";

                                            }
                                        }
                                        else //get policy error
                                        {
                                            processNext = false;
                                            sms = da_banca.MESSAGE;
                                        }


                                    }
                                    else //get channel configuration error
                                    {
                                        processNext = false;
                                        sms = Channel_Item_Config.Message;
                                    }
                                    #endregion check on channel item config

                                }
                            }
                            else//get channel user config error
                            {
                                //show error message
                                processNext = false;
                                sms = chUserConf.Message;
                            }

                            #endregion check on channel user config
                        }
                    }

                    if (processNext)
                    {

                        if (para.NewApplicationNumber != "")//exist
                        {
                            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views application detail [App No.", para.NewApplicationNumber, "]."));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?ID=" + id.Text + "&APP_ID=" + app.APPLICATION_ID + "');</script>", false);
                        }
                        else //not exist
                        {
                            //new application
                            bool result = ConvertToApp(app_id.Text, UserName, DateTime.Now, ChannelItemId, ChannelLocationId, AgentId, AgentName, out MESSAGE);

                            if (result)
                            {
                                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views lead in application detail [Lead Id.", LeadId, "]."));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?ID=" + id.Text + "&APP_ID=" + "');</script>", false);
                            }
                            else
                            {
                                Helper.Alert(true, "Ooop! system cannot convert lead to application, please contact your system administrator.", lblError);

                            }

                        }
                        //store session
                        Session["SS_SESSION_LEAD"] = para;


                    }
                    else
                    {

                        // Helper.Alert(true, "Policy [" + tbl.Rows[0]["policy_number"] + "] is still in-force." + remainExpiryDays, lblError);
                        Helper.Alert(true, sms, lblError);
                    }
                    #endregion V2
                }
            }
            #endregion COPY
            #region UPDATE STATUS
            else if (e.CommandName == "CMD_UPDATE")
            {
                string url = "banca_customer_lead_update.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?ID=" + id.Text + "&APP_NO=" + app_no.Text + "');</script>", false);
                bool result = ConvertToApp(app_id.Text, UserName, DateTime.Now, ChannelItemId, ChannelLocationId, AgentId, AgentName, out MESSAGE);

                // result = da_customer_lead.CopyToApplication(app_id.Text, user_name, DateTime.Now);
                // MESSAGE = da_customer_lead.MESSAGE;

                if (result)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?ID=" + id.Text + "&APP_ID=" + "');</script>", false);
                }
                else
                {
                    Helper.Alert(true, "Ooop! system cannot convert lead to application, please contact your system administrator.", lblError);

                }
            }
            #endregion UPDATE STATUS
            #region EDIT
            else if (e.CommandName == "CMD_EDIT")
            {
                Transaction(Helper.FormTransactionType.EDIT);
                LeadId = id.Text;
                string[] strNameEn = Helper.BreakFullName(lClientName.Text, ' ');
                string[] strNameKh = Helper.BreakFullName(lClientNameKh.Text, ' ');

                /*Get channel location id from channel item user config*/
                ChannelItemUserConfig chUser = new ChannelItemUserConfig();
                chUser= chUser.GetByUserId(lApiUser.Text);

                Helper.SelectedDropDownListIndex("VALUE", ddlChannelNameAdd, chUser.ChannelItemId);
                ddlChannelNameAdd_SelectedIndexChanged(null, null);
                Helper.SelectedDropDownListIndex("VALUE", ddlBranchNameAdd, lBranchCode.Text);
                ddlBranchNameAdd_SelectedIndexChanged(null, null);
                txtReferralStaffId.Text = lReferralId.Text;
                txtReferralStaffName.Text = lReferralName.Text;
                txtReferralStaffPosition.Text = lReferralPosition.Text;
                txtReferredDate.Text = lReferredDate.Text;
                Helper.SelectedDropDownListIndex("VALUE", ddlClientType, lClientType.Text);
                txtCif.Text = lCIF.Text;
                txtApplicationId.Text = application_id;
                txtClientLastNameEN.Text = strNameEn[0];
                txtClientFirstNameEn.Text = strNameEn[1];
                txtClientLastNameKh.Text = strNameKh[0];
                txtClientFirstNameKh.Text = strNameKh[1];
                Helper.SelectedDropDownListIndex("TEXT", ddlIdType, lClientIdType.Text);
                txtIdNo.Text = lClientIdNumber.Text;
                txtDateOfBirthAdd.Text = lClientDob.Text;
                Helper.SelectedDropDownListIndex("VALUE", ddlGenderAdd, lClientGender.Text);
                Helper.SelectedDropDownListIndex("VALUE", ddlNationality, lNationality.Text);
                txtPhoneNumber.Text = lPhoneNumber.Text;
                Helper.SelectedDropDownListIndex("VALUE", ddlProvince, lProvince.Text);
                txtRemarks.Text = lRemarks.Text;
                //call this event to bind distict
                ddlProvince_SelectedIndexChanged(null, null);

                Helper.SelectedDropDownListIndex("VALUE", ddlDistrict, lDistrict.Text);
                ddlDistrict_SelectedIndexChanged(null, null);
                Helper.SelectedDropDownListIndex("VALUE", ddlCommune, lCommune.Text);
                ddlCommune_SelectedIndexChanged(null, null);
                Helper.SelectedDropDownListIndex("VALUE", ddlVillage, lVillage.Text);

                

            }
            #endregion EDIT
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)] in class [banca_customer_lead.aspx.cs], detail: " + ex.Message + " => " + ex.StackTrace);
            Helper.Alert(true, "Ooop! system cannot convert lead to application, please contact your system administrator.", lblError);

            da_customer_lead_app_temp.DeleteCustomerLeadAppTempByID(lead_id);
        }
    }

    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;

        List<bl_customer_lead> cusList = new List<bl_customer_lead>();
        cusList = (List<bl_customer_lead>)Session["CUSLIST"];
        gv_valid.DataSource = cusList;
        gv_valid.DataBind();
        /*show record count*/
        lblRecords.Text = gv_valid.Rows.Count + " Of " + cusList.Count + " Records";

    }
    protected void ddlBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region V2
        txtBranchCode.Text = ddlBranchName.SelectedValue;

        List<bl_channel_location> listLocation = new List<bl_channel_location>();
        List<bl_channel_location> chList = new List<bl_channel_location>();
        chList = da_channel.GetChannelLocationByUser(UserName);

        foreach (bl_channel_location ch in chList.Where(_ => _.Office_Code == ddlBranchName.SelectedValue))
        {
            if (!string.IsNullOrWhiteSpace(ch.Office_Code))
            {
                ChannelLocationId = ch.Channel_Location_ID;
                break;
            }
        }

        Transaction(Helper.FormTransactionType.SEARCH);
        #endregion V2

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.ADD_NEW);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (FormIsValide())
            {
                string branchCode = ddlBranchNameAdd.SelectedValue;
                string branchName = ddlBranchNameAdd.SelectedItem.Text;
                string applicationId ="";// DateTime.Now.ToString("hhmmss");// txtApplicationId.Text.Trim();
              
                bl_customer_lead_prefix leadPrefix = new bl_customer_lead_prefix();
                leadPrefix = da_customer_lead_prefix.GetLastCustomerLeadPrefix();
                bl_customer_lead_prefix.bl_customer_lead_number leadNo = new bl_customer_lead_prefix.bl_customer_lead_number();
                bl_customer_lead_prefix.bl_customer_lead_number lastLead = new bl_customer_lead_prefix.bl_customer_lead_number();
                lastLead = da_customer_lead_number.GetLastCustomerLeadNumber();
                leadNo.LeadNumberId = Helper.GetNewGuid(new string[,]{{"TABLE","CT_CUSTOMER_LEAD_NUMBER"},{"FIELD","LEAD_NUMBER_ID"}});

                if (string.IsNullOrWhiteSpace(lastLead.LeadNumberId))
                {
                    leadNo.LeadNumber = 1;
                }
                else
                {
                    if (lastLead.PrefixYear == leadPrefix.PREFIX2)//in the same year
                    {
                        leadNo.LeadNumber = lastLead.LeadNumber + 1;
                       
                    }
                    else
                    {
                        leadNo.LeadNumber = 1;
                       
                    }
                }
                applicationId = leadPrefix.PREFIX1 + leadPrefix.PREFIX2 + leadNo.LeadNumber.ToString(leadPrefix.DIGITS);
                leadNo.LeadNumberVar = applicationId;
                leadNo.CreatedBy=UserName;
                leadNo.CreatedOn=DateTime.Now;
                leadNo.Remarks="";

                string clientType = ddlClientType.SelectedValue;
                string cif = txtCif.Text.Trim();
                string lastNameEn = txtClientLastNameEN.Text.Trim();
                string firstNameEn = txtClientFirstNameEn.Text.Trim();
                string lastNameKh = txtClientLastNameKh.Text.Trim();
                string firstNameKh = txtClientFirstNameKh.Text.Trim();
                string gender = ddlGenderAdd.SelectedValue;
                string nationality = ddlNationality.SelectedValue;
                DateTime dob = Helper.FormatDateTime(txtDateOfBirthAdd.Text.Trim());
                string idType = ddlIdType.SelectedItem.Text;
                string idNo = txtIdNo.Text.Trim();
                string phoneNumber = txtPhoneNumber.Text.Trim();
                string province = ddlProvince.SelectedValue;
                string commune = ddlCommune.SelectedValue;
                string district = ddlDistrict.SelectedValue;
                string village = ddlVillage.SelectedValue;

                string referralId = txtReferralStaffId.Text.Trim();
                string referralName = txtReferralStaffName.Text.Trim();
                string referralPosition = txtReferralStaffPosition.Text.Trim();
                DateTime referDate = Helper.FormatDateTime(txtReferredDate.Text.Trim());
                string remarks = txtRemarks.Text;
                bl_customer_lead lead = new bl_customer_lead()
                {
                    ApiUser = ApiUserId,
                    ApplicationID = applicationId,
                    BranchCode = branchCode,
                    BranchName = branchName,
                    ClientCIF = cif,
                    ClientType = clientType,
                    ClientNameENG = lastNameEn + " " + firstNameEn,
                    ClientNameKHM = lastNameKh + " " + firstNameKh,
                    ClientGender = gender,
                    ClientDoB = dob,
                    ClientNationality = nationality,
                    DocumentId = idNo,
                    DocumentType = idType,
                    ClientPhoneNumber = phoneNumber,
                    ClientProvince = province,
                    ClientDistrict = district,
                    ClientCommune = commune,
                    ClientVillage = village,
                    LeadType = "New",
                    Interest = "Micro Insurance",
                    ReferralStaffId = referralId,
                    ReferralStaffName = referralName,
                    ReferralStaffPosition = referralPosition,
                    ReferredDate = referDate,
                    CreatedBy = UserName,
                    CreatedOn = DateTime.Now,
                    Remarks = remarks
                };
                if (string.IsNullOrWhiteSpace(LeadId))//save
                {
                    //save lead number

                    if (da_customer_lead_number.Save(leadNo))
                    {
                        if (da_customer_lead.InsertCustomerLead(lead) != "")
                        {
                            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat( "User was added new Lead [lead id:", lead.ID,"]."));
                            LeadId = lead.ID;//store for show in gridview after save success
                            Helper.Alert(false, "Add New Lead successully.", lblError);
                            Transaction(Helper.FormTransactionType.SAVE);
                        }
                        else
                        {
                            Helper.Alert(true, "Add New Lead fail.", lblError);
                            da_customer_lead_number.Delete(leadNo.LeadNumberId);
                        }
                    }
                    else
                    {
                        Helper.Alert(true, "Add New Lead fail.", lblError);
                    }
                    
                }
                else//update
                {
                    lead.UpdatedBy = UserName;
                    lead.UpdatedOn = DateTime.Now;
                    lead.ApplicationID = txtApplicationId.Text;
                    lead.ID = LeadId;
                    if (da_customer_lead.UpdateCustomerLead(lead) != "")
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE, string.Concat("User was updated Lead Information [lead id:", lead.ID, "]."));
                        Helper.Alert(false, "Updated Lead successully.", lblError);
                        Transaction(Helper.FormTransactionType.UPDATE);
                    }
                    else
                    {
                        Helper.Alert(true, "Updated Lead fail.", lblError);
                    }
                }
            }
            else// form is not valide
            {
                Helper.Alert(true, ErrorMessage, lblError);
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.DELETE, string.Concat("User was deleted Lead [lead id:",LeadId, "]."));
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.CANCEL);
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChannelItemId = ddlChannel.SelectedValue;
        LoadBranch(ddlBranchName, ddlChannel.SelectedValue);
    }

    protected void ddlChannelNameAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
       // ChannelItemId = ddlChannelNameAdd.SelectedValue;
        LoadBranch(ddlBranchNameAdd, ddlChannelNameAdd.SelectedValue);

    }
    protected void ddlBranchNameAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  ChannelLocationId = ddlBranchNameAdd.SelectedValue;
    }
    protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProvince.SelectedIndex > 0)
        {
            BindDistrict();
            //clear dropdownlist
            ddlCommune.Items.Clear();
            ddlCommune.Items.Add(new ListItem("--ឃុំ/សង្កាត់--", ""));
            ddlVillage.Items.Clear();
            ddlVillage.Items.Add(new ListItem("--ភូមិ--", ""));

        }
        else
        {
            ddlDistrict.Items.Clear();
            ddlDistrict.Items.Add(new ListItem("--ស្រុក/ខណ្ឌ--", ""));
        }
    }
    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCommune();
        ddlVillage.Items.Clear();
        ddlVillage.Items.Add(new ListItem("--ភូមិ--", ""));

    }
    protected void ddlCommune_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVillage();

    }
    protected void ddlVillage_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gv_valid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
       // string logDesc = "User inquiries Lead with criteria [Channel:" + ddlChannel.SelectedValue + ", Branch Name:" + ddlBranchName.SelectedValue + ", Customer Name:" + txtCustomerNameEnglish.Text + ", ID Number:" + txtIDNumber.Text + ", Gender:" + ddlGender.SelectedValue + ", Dob:" + txtDateOfBirth.Text + "]";
        da_sys_activity_log.Save(new bl_sys_activity_log(UserName, UserRole.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
}