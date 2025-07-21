using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Pages_Channel_03frmChannelLinkUser : System.Web.UI.Page
{
    private string ChannelLinkUserId { get { return ViewState["V_CHANNEL_U_LINK"] + ""; } set { ViewState["V_CHANNEL_U_LINK"] = value; } }
    private string UserName { get { return ViewState["V_USER_NAME"] + ""; } set { ViewState["V_USER_NAME"] = value; } }
    private bool PerView { get { return (bool)ViewState["V_PERM_VIEW"]; } set { ViewState["V_PERM_VIEW"] = value; } }
    private bool PerAdd { get { return (bool)ViewState["V_PERM_ADD"]; } set { ViewState["V_PERM_ADD"] = value; } }
    private bool PerUpdate { get { return (bool)ViewState["V_PERM_UPDATE"]; } set { ViewState["V_PERM_UPDATE"] = value; } }
    private bool PerAdmin { get { return (bool)ViewState["V_PERM_ADM"]; } set { ViewState["V_PERM_ADM"] = value; } }

    private string ErrorMessage { get { return ViewState["V_ERROR_SMS"] + ""; } set { ViewState["V_ERROR_SMS"] = value; } }

    void Transaction(Helper.FormTransactionType type)
    {
        if (type.ToString() == Helper.FormTransactionType.FIRST_LOAD.ToString())
        {
            BindChannel();
            if (!LoadData())
            {
                Helper.Alert(true, ErrorMessage, lblError);
            }
            divAdd.Visible = false;
            EnableControl(false);
        }
        else if (type.ToString() == Helper.FormTransactionType.SEARCH.ToString())
        {
            LoadData();
            divList.Visible = true;
            divAdd.Visible = false;
        }
        else if (type.ToString() == Helper.FormTransactionType.ADD_NEW.ToString())
        {
            if (PerAdd)
            {
                ClearText();
                EnableControl();
                divList.Visible = false;
                divAdd.Visible = true;
                btnDelete.Enabled = false;

            }
            else if (PerAdmin)
            {
                ClearText();
                EnableControl();
                divAdd.Visible = true;
                divList.Visible = false;
                btnDelete.Enabled = true;
            }
            else if (PerView)
            {
                EnableControl(false);
                divList.Visible = false;
                divAdd.Visible = true;

            }
            else
            {
                Helper.Alert(false, "Permission is required.", lblError);
            }


        }
        else if (type.ToString() == Helper.FormTransactionType.EDIT.ToString())
        {

            if (PerAdmin)
            {
                ClearText();
                divAdd.Visible = true;
                EnableControl();
                btnDelete.Enabled = true;
                divList.Visible = false;
            }
            else if (PerAdd)
            {
                ClearText();
                divAdd.Visible = true;
                EnableControl();
                btnSave.Enabled = true;
                btnDelete.Enabled = false;

                divList.Visible = false;
            }
            else if (PerView)
            {
                ClearText();
                divAdd.Visible = true;
                EnableControl(false);
                btnSave.Enabled = false;
                btnDelete.Enabled = false;

                divList.Visible = false;
            }
            else
            {
                Helper.Alert(false, "Permission is required.", lblError);
            }

        }
        else if (type.ToString() == Helper.FormTransactionType.CANCEL.ToString())
        {

            ClearText();
            divAdd.Visible = false;
            divList.Visible = true;
            LoadData();
        }
        else if (type.ToString() == Helper.FormTransactionType.SAVE.ToString())
        {
            ReLoadSave();
            divList.Visible = true;
            btnDelete.Enabled = false;
            ClearText();

        }
        else if (type.ToString() == Helper.FormTransactionType.DELETE.ToString())
        {

            ClearText();
            LoadData();
            divAdd.Visible = false;
            divList.Visible = true;
            btnDelete.Enabled = false;
        }
    }
    void EnableControl(bool t = true)
    {
        ddlChannelName.Enabled = t;
       
        txtUserName.Enabled = t;
       
        txtRemarks.Enabled = t;
      
        btnSave.Enabled = t;
        btnDelete.Enabled = false;

    }
    bool ValidateForm()
    {
        if (ddlChannelName.SelectedValue == "")
        {
            ErrorMessage = "Channel Name is required.";
            return false;
        }
       
        else if (txtUserName.Text.Trim() == "")
        {
            ErrorMessage = "User Name is required.";
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
        ChannelLinkUserId = string.Empty;
        ddlChannelName.SelectedIndex = 0;
        txtUserName.Text = "";
        txtRemarks.Text = "";
    }
    void BindChannel()
    {
        Helper.BindChannelItem(ddlChannelName, "0152DF80-BA95-46A9-BB7A-E71966A34089");
    }
    bool LoadData()
    {
        try
        {
            ChannelItemUserConfig chConfig = new ChannelItemUserConfig();
            List<ChannelItemUserConfig> chList = new List<ChannelItemUserConfig>();
            List<ChannelItemUserConfig> chListFilter = new List<ChannelItemUserConfig>();
            chList = chConfig.GetList();
            if (txtSearchInfo.Text.Trim() != "")
            {
              chListFilter=  chList.Where(_ => _.UserName.Contains(txtSearchInfo.Text.Trim())).ToList();

            }
            else
            {
                chListFilter = chList;
            }


            gvParam.DataSource = chListFilter;
            gvParam.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error LoadData(), " + ex.Message;
            return false;
        }
    }
    bool ReLoadSave()
    {
        try
        {
            ChannelItemUserConfig chConfig = new ChannelItemUserConfig();
            List<ChannelItemUserConfig> chList = new List<ChannelItemUserConfig>();
          
            chList.Add(chConfig.GetByUserName(txtUserName.Text.Trim()));
           
            gvParam.DataSource = chList;
            gvParam.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error ReloadSave(), "+ ex.Message;
            return false;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
           
            ChannelItemUserConfig obj = new ChannelItemUserConfig() { 
             UserName = txtUserName.Text.Trim(), ChannelItemId=ddlChannelName.SelectedValue, Remarks=txtRemarks.Text.Trim(), CreatedRemarks="", UpdatedRemarks="", CreatedOn=DateTime.Now, CreatedBy=UserName
            };



            if (string.IsNullOrWhiteSpace(ChannelLinkUserId))
            {
                obj.Id = obj.GenerateId();
                if (obj.Save())
                {
                    Helper.Alert(false, obj.Message, lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, obj.Message, lblError);
                }
            }
            else
            {
                obj.Id = ChannelLinkUserId;
                obj.UpdatedBy = UserName;
                obj.UpdatedOn = DateTime.Now;
                if (obj.Update())
                {
                    Helper.Alert(false, obj.Message, lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, obj.Message, lblError);
                }
            }
        }
        else {
            Helper.Alert(true, ErrorMessage, lblError);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ChannelLinkUserId))
        {
              ChannelItemUserConfig obj = new ChannelItemUserConfig();
              if (obj.Delete(ChannelLinkUserId))
              {
                  Helper.Alert(false, obj.Message, lblError);
                  Transaction(Helper.FormTransactionType.DELETE);
              }
              else
              {
                  Helper.Alert(true, obj.Message, lblError);
              }
        }
        else
        {
            Helper.Alert(true, "No record found for deleting.", lblError);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.CANCEL);
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(Helper.FormTransactionType.ADD_NEW);
    }
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Transaction(Helper.FormTransactionType.EDIT);

            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lId = (Label)r.FindControl("lblId");
            Label lChannelItemId = (Label)r.FindControl("lblChannelItemId");
            Label lRemarks = (Label)r.FindControl("lblRemarks");
            Label lUserName = (Label)r.FindControl("lblUserName");
            Helper.SelectedDropDownListIndex("VALUE", ddlChannelName, lChannelItemId.Text);
          
            ChannelLinkUserId = lId.Text;
            txtUserName.Text = lUserName.Text;
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
}