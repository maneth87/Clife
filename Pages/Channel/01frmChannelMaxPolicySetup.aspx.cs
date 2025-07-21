using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
public partial class Pages_Channel_01frmChannelMaxPolicySetup : System.Web.UI.Page
{

    private string UserName { get { return ViewState["V_USER_NAME"] + ""; } set { ViewState["V_USER_NAME"] = value; } }
    private bool PerView { get { return (bool)ViewState["V_PERM_VIEW"]; } set { ViewState["V_PERM_VIEW"] = value; } }
    private bool PerAdd { get { return (bool)ViewState["V_PERM_ADD"]; } set { ViewState["V_PERM_ADD"] = value; } }
    private bool PerUpdate { get { return (bool)ViewState["V_PERM_UPDATE"]; } set { ViewState["V_PERM_UPDATE"] = value; } }
    private bool PerAdmin { get { return (bool)ViewState["V_PERM_ADM"]; } set { ViewState["V_PERM_ADM"] = value; } }
    private string ConfigId { get { return ViewState["V_CONFIG_ID"] + ""; } set { ViewState["V_CONFIG_ID"] = value; } }
    private bool IsFormValid { get { return (bool)ViewState["V_VALID_F"]; } set { ViewState["V_VALID_F"] = value; } }
    private string ErrorMessage { get { return ViewState["V_ERROR_SMS"] + ""; } set { ViewState["V_ERROR_SMS"] = value; } }

    void ValidForm()
    {
        if (ddlChannelName.SelectedValue == "")
        {
            ErrorMessage = "Channel Name is required.";
            IsFormValid = false;
        }
        else if (ddlProduct.SelectedValue == "")
        {
            ErrorMessage = "Product ID is required.";
            IsFormValid = false;
        }
        else if (!Helper.IsNumber(txtMaxPolNo.Text))
        {
            ErrorMessage = "Max Policy Per Life is required as Number.";
            IsFormValid = false;
        }
        else if (ddlStatus.SelectedValue == "")
        {
            ErrorMessage = "Status is required.";
            IsFormValid = false;
        }
        else
        {
            ErrorMessage = "";
            IsFormValid = true;
        }
    }

    bool LoadData()
    {
        try
        {
            Channel_Item_Config chObj = new Channel_Item_Config();
            List<Channel_Item_Config> chConfigList;
            if (ddlChannelNameSearch.SelectedValue != "")
            {
                chConfigList = chObj.GetChannelItemConfig(ddlChannelNameSearch.SelectedValue, -1);
            }
            else
            {
                chConfigList = chObj.GetChannelItemConfig();
            }
            if (Channel_Item_Config.Transection)
            {
                gvParam.DataSource = chConfigList;
                gvParam.DataBind();
            }
            else
            {
                gvParam.DataSource = null;
                gvParam.DataBind();
            }

            return Channel_Item_Config.Transection;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    void ClearText()
    {
        ddlChannelName.SelectedIndex = 0;
        ddlProduct.Items.Clear();
        txtMaxPolNo.Text = "";
        ddlStatus.SelectedIndex = 0;
        txtRemarks.Text = "";
        ConfigId = string.Empty;

    }
    void EnableControl(bool t = true)
    {
        divAdd.Visible = t;
    }
    void Transaction(Helper.FormTransactionType type)
    {

        if (type.ToString() == Helper.FormTransactionType.FIRST_LOAD.ToString())
        {
            Helper.BindChannelItem(ddlChannelName, "0152DF80-BA95-46A9-BB7A-E71966A34089");
            Helper.BindChannelItem(ddlChannelNameSearch, "0152DF80-BA95-46A9-BB7A-E71966A34089");
            if (PerView)
            {
                EnableControl(false);
            }
            if (PerAdd)
            {
                EnableControl(false);
            }
            if (PerAdmin)
            {
                EnableControl(false);
            }
        }
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
        else if (type.ToString() == Helper.FormTransactionType.SAVE.ToString())
        {

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;

            ClearText();
            LoadData();
        }
        else if (type.ToString() == Helper.FormTransactionType.UPDATE.ToString())
        {


            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;

            ClearText();

            LoadData();
        }
        else if (type.ToString() == Helper.FormTransactionType.CANCEL.ToString())
        {
            ClearText();
            EnableControl(false);
            divAdd.Visible = false;

        }
        else if (type.ToString() == Helper.FormTransactionType.EDIT.ToString())
        {
            ClearText();


            if (PerAdmin)
            {
                EnableControl();
                btnDelete.Enabled = true;
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
                btnDelete.Enabled = false;
            }


            divAdd.Visible = true;


        }
        else if (type.ToString() == Helper.FormTransactionType.SEARCH.ToString())
        {
            if (!LoadData())
            {
                Helper.Alert(true, "LoadData() error.", lblError);
            }

        }
        else if (type.ToString() == Helper.FormTransactionType.DELETE.ToString())
        {

            ClearText();
            LoadData();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        UserName = Membership.GetUser().UserName;

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
            IsFormValid = false;
            Transaction(Helper.FormTransactionType.FIRST_LOAD);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ValidForm();
        if (IsFormValid)
        {

            string channelId = ddlChannelName.SelectedValue;
            string productId = ddlProduct.SelectedValue;
            int maxPolNo = Convert.ToInt32(txtMaxPolNo.Text.Trim() == "" ? "0" : txtMaxPolNo.Text.Trim());
            int status = Convert.ToInt32(ddlStatus.SelectedValue);
            string remarks = txtRemarks.Text.Trim();

            Channel_Item_Config chObj = new Channel_Item_Config();
            string id = chObj.GetNewId();
            if (string.IsNullOrWhiteSpace(ConfigId))
            {
                if (chObj.Save(new Channel_Item_Config()
                {
                    ID = id,
                    ProductId = productId,
                    ChannelItemId = channelId,
                    MaxPolicyPerLife = maxPolNo,
                    Status = status,
                    Remarks = remarks,
                    CreatedBy = UserName,
                    CreatedOn = DateTime.Now,
                    CreatedRemarks = ""

                }))
                {
                    Helper.Alert(false, "Added new record successfully.", lblError);
                }
                else
                {
                    Helper.Alert(true, "Added new record fail.", lblError);
                }
            }
            else
            {
                if (chObj.Update(new Channel_Item_Config()
                    {
                        ID = ConfigId,
                        ProductId = productId,
                        ChannelItemId = channelId,
                        MaxPolicyPerLife = maxPolNo,
                        Status = status,
                        Remarks = remarks,
                        UpdatedBy = UserName,
                        UpdatedOn = DateTime.Now,
                        UpdatedRemarks = ""

                    }))
                {
                    Helper.Alert(false, "Updated record successfully.", lblError);
                }
                else
                {
                    Helper.Alert(true, "Updated record fail.", lblError);
                }
            }
            Transaction(Helper.FormTransactionType.SAVE);
        }
        else
        {
            Helper.Alert(true, ErrorMessage, lblError);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Channel_Item_Config chObj = new Channel_Item_Config();
        if (!string.IsNullOrWhiteSpace(ConfigId))
        {
            if (chObj.Delete(ConfigId))
            {
                Helper.Alert(false, "Deleted record successfully.", lblError);
            }
            else
            {
                Helper.Alert(true, "Deleted record fail.", lblError);
            }
        }
        else
        {
            Helper.Alert(true, "System cannot found record for deleting.", lblError);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.CANCEL);
    }
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Transaction(Helper.FormTransactionType.EDIT);

            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lId = (Label)r.FindControl("lblId");
            Label lChannelName = (Label)r.FindControl("lblChannelName");
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lMaxPolNo = (Label)r.FindControl("lblMaxPolNo");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lRemarks = (Label)r.FindControl("lblRemarks");

            ConfigId = lId.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlChannelName, lChannelName.Text);
            ddlChannelName_SelectedIndexChanged(null, null);
            Helper.SelectedDropDownListIndex("VALUE", ddlProduct, lProductId.Text);
            txtMaxPolNo.Text = lMaxPolNo.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
            txtRemarks.Text = lRemarks.Text;

        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void gvParam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvParam.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.SEARCH);
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.ADD_NEW);
    }
    protected void ddlChannelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProduct.Items.Clear();
        List<bl_micro_product_config> proList = da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(ddlChannelName.SelectedValue);
        Options.Bind(ddlProduct, proList, bl_micro_product_config.NAME.ProductRemarks, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");
    }
}