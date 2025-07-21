using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Pages_Channel_04frmChannelLocation : System.Web.UI.Page
{
    private string ChannelLocationID { get { return ViewState["V_CHANNEL_LOCATION_ID"] + ""; } set { ViewState["V_CHANNEL_LOCATION_ID"] = value; } }
    private string UserName { get { return ViewState["V_USER_NAME"] + ""; } set { ViewState["V_USER_NAME"] = value; } }
    private bool PerView { get { return (bool)ViewState["V_PERM_VIEW"]; } set { ViewState["V_PERM_VIEW"] = value; } }
    private bool PerAdd { get { return (bool)ViewState["V_PERM_ADD"]; } set { ViewState["V_PERM_ADD"] = value; } }
    private bool PerUpdate { get { return (bool)ViewState["V_PERM_UPDATE"]; } set { ViewState["V_PERM_UPDATE"] = value; } }
    private bool PerAdmin { get { return (bool)ViewState["V_PERM_ADM"]; } set { ViewState["V_PERM_ADM"] = value; } }
    private string ConfigId { get { return ViewState["V_CONFIG_ID"] + ""; } set { ViewState["V_CONFIG_ID"] = value; } }
    private bool IsFormValid { get { return (bool)ViewState["V_VALID_F"]; } set { ViewState["V_VALID_F"] = value; } }
    private string ErrorMessage { get { return ViewState["V_ERROR_SMS"] + ""; } set { ViewState["V_ERROR_SMS"] = value; } }
    void Transaction(Helper.FormTransactionType type)
    {
        if (type.ToString() == Helper.FormTransactionType.FIRST_LOAD.ToString())
        {
            LoadChannel();
            divAdd.Visible = false;
            EnableControl(false);
        }
        else if (type.ToString() == Helper.FormTransactionType.SEARCH.ToString())
        {
            LoadData();
        }
        else if (type.ToString()==Helper.FormTransactionType.ADD_NEW.ToString())
        {
            ClearText();
            EnableControl();
            divAdd.Visible = true;
        }
        else if (type.ToString() == Helper.FormTransactionType.EDIT.ToString())
        {
            ClearText();
            divAdd.Visible = true;
            EnableControl();
            if (PerAdmin)
            {
                btnDelete.Enabled = true;
            }
            else if (PerAdd)
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
            }
            else if (PerView)
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
            }
           
        }
        else if (type.ToString() == Helper.FormTransactionType.CANCEL.ToString())
        {
            ClearText();
            divAdd.Visible = false;
        }
        else if (type.ToString() == Helper.FormTransactionType.SAVE.ToString())
        {
            ClearText();
            LoadData();
        }
        else if (type.ToString() == Helper.FormTransactionType.DELETE.ToString())
        {
            ClearText();
            LoadData();
            btnDelete.Enabled = false;
        }
    }

    bool ValidateForm()
    {
        if (ddlChannelName.SelectedValue == "")
        {
            ErrorMessage = "Channel Name is required.";
            return false;
        }
        else if (txtBranchCode.Text.Trim() == "")
        {
            ErrorMessage = "Branch code is required.";
            return false;
        }
        else if (txtBranchName.Text.Trim() == "")
        {
            ErrorMessage = "Branch Name is required.";
            return false;
        }
        else if (txtAddressEn.Text.Trim() == "")
        {
            ErrorMessage = "Address EN is required.";
            return false;
        }
        else if (ddlStatus.SelectedValue == "")
        {
            ErrorMessage = "Status is required.";
            return false;
        }
        else
        {
            ErrorMessage = "";
            return true;
        }

    }
    void ClearText()
    {
        ddlChannelName.SelectedIndex = 0;
        txtBranchCode.Text = "";
        txtBranchName.Text = "";
        txtPhoneNumber.Text = "";
        txtAddressEn.Text = "";
        txtAddressKh.Text = "";
        ddlStatus.SelectedIndex = 0;
        ChannelLocationID = string.Empty;
    }

    void LoadChannel()
    {
        List<bl_channel_item> chList = da_channel.GetChannelItemList();
        Options.Bind(ddlChannelName, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");
        Options.Bind(ddlChannelNameSearch, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");
    }

    void LoadData()
    {
        try
        {
            List<bl_channel_location> chList = da_channel.GetChannelLocationListByChannelItemID(ddlChannelNameSearch.SelectedValue);
            gvParam.DataSource = chList;
            gvParam.DataBind();
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error LoadData()",lblError);
        }
    }
    void EnableControl(bool t =true)
    {
        ddlChannelName.Enabled = t;
        txtBranchCode.Enabled = t;
        txtBranchName.Enabled = t;
        txtPhoneNumber.Enabled = t;
        txtAddressEn.Enabled = t;
        txtAddressKh.Enabled = t;
        ddlStatus.Enabled = t;
        btnSave.Enabled = t;
        btnDelete.Enabled = false;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        UserName = System.Web.Security.Membership.GetUser().UserName;
        string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
        List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
        bl_sys_user_role ur = new bl_sys_user_role();
        bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);

        PerView = u.IsView == 1 ? true : false;
        PerAdd = u.IsAdd == 1 ? true : false;
        PerAdmin = u.IsAdmin == 1 ? true : false;
        PerUpdate = u.IsUpdate == 1 ? true : false;
        if (!Page.IsPostBack)
        {
            Transaction(Helper.FormTransactionType.FIRST_LOAD);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlChannelNameSearch.SelectedValue != "")
        {
            Transaction(Helper.FormTransactionType.SEARCH);
        }
        else
        {
            Helper.Alert(false, "Please select channel name.", lblError);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            string channelItemId = ddlChannelName.SelectedValue;
            string officeCode = txtBranchCode.Text.Trim();
            string officeName = txtBranchName.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();
            string addressEn = txtAddressEn.Text.Trim();
            string addressKh = txtAddressKh.Text.Trim();
            int status = Convert.ToInt32(ddlStatus.SelectedValue);
            string remarks = txtRemarks.Text.Trim();
            bl_channel_location obj = new bl_channel_location();
            obj = new bl_channel_location()
            {

                Channel_Item_ID = channelItemId,
                Office_Code = officeCode,
                Office_Name = officeName,
                PhoneNumber = phone,
                Address = addressEn,
                AddressKh = addressKh,
                Status = status,
                Created_By = UserName,
                Created_On = DateTime.Now,
                CreatedNote = remarks
            };
            if (string.IsNullOrWhiteSpace(ChannelLocationID))
            {
                obj.Channel_Location_ID = da_channel.GetChannelLocationId();

                if (da_channel.AddChannelLocation(obj))
                {
                    Helper.Alert(false, "Added New Channel Location successfully.", lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, "Added New Channel Location fail.", lblError);
                }
            }
            else
            {
                obj.Channel_Location_ID = ChannelLocationID;
                if (da_channel.UpdateChannelLocation(obj))
                {
                    Helper.Alert(false, "Updated Channel Location successfully.", lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, "Updated Channel Location fail.", lblError);
                }
            }
        }
        else
        {
            Helper.Alert(true, ErrorMessage, lblError);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ChannelLocationID))
        {
            if (da_channel.DeleteChannelLocation(ChannelLocationID))
            {
                Helper.Alert(false, "Deleted successfully.", lblError);
                Transaction(Helper.FormTransactionType.DELETE);
            }
            else
            {
                Helper.Alert(true, "Deleted fail.", lblError);
            }
        }
        else
        {
            Helper.Alert(true, "", lblError);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.CANCEL);
    }
    protected void gvParam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvParam.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Transaction(Helper.FormTransactionType.EDIT);

            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lChannelLocationId = (Label)r.FindControl("lblChannelLocationId");
            Label lChannelName = (Label)r.FindControl("lblChannelName");
            Label lOfficeCode = (Label)r.FindControl("lblBranchCode");
            Label lOfficeName = (Label)r.FindControl("lblBranchName");
            Label lPhone = (Label)r.FindControl("lblPhone");
            Label lAddressEn = (Label)r.FindControl("lblAddressEn");
            Label lAddressKh = (Label)r.FindControl("lblAddressKh");
          
            Label lStatus = (Label)r.FindControl("lblStatus");
            ChannelLocationID = lChannelLocationId.Text;
         
            Helper.SelectedDropDownListIndex("TEXT", ddlChannelName, lChannelName.Text);
            txtBranchCode.Text = lOfficeCode.Text;
            txtBranchName.Text = lOfficeName.Text;
            txtPhoneNumber.Text = lPhone.Text;
            txtAddressEn.Text = lAddressEn.Text;
            txtAddressKh.Text = lAddressKh.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
           
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.ADD_NEW);
    }
    protected void ddlChannelName_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
}