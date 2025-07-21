using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Pages_CoreData_sale_agent_new : System.Web.UI.Page
{
    private string SaleAgentId { get { return ViewState["V_SALE_AGENT_ID"] + ""; } set { ViewState["V_SALE_AGENT_ID"] = value; } }
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
        List<bl_master_list> agentTypeList=    da_master_list.GetMasterList("SALE_AGENT_TYPE");

        Options.Bind(ddlSaleAgentType, agentTypeList, "DescEn", "Code", -1);

            LoadData();
            divAdd.Visible = false;
            EnableControl(false);
        }
        else if (type.ToString() == Helper.FormTransactionType.SEARCH.ToString())
        {
            LoadData();
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
                txtSaleAgentId.Enabled = false;
                txtRemarks.Enabled = false;
                divList.Visible = false;
            }
            else if (PerAdd)
            {
                ClearText();
                divAdd.Visible = true;
                EnableControl();
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                txtSaleAgentId.Enabled = false;
                txtRemarks.Enabled = false;
                divList.Visible = false;
            }
            else if (PerView)
            {
                ClearText();
                divAdd.Visible = true;
                EnableControl(false);
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                txtSaleAgentId.Enabled = false;
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
            btnDelete.Enabled = false;
        }
    }
    void EnableControl(bool t = true)
    {
        txtSaleAgentId.Enabled = t;
        txtFullNameEn.Enabled = t;
        txtFullNameKh.Enabled = t;
        txtPosition.Enabled = t;
        ddlStatus.Enabled = t;
        txtRemarks.Enabled = t;
        ddlStatus.Enabled = t;
        btnSave.Enabled = t;
        btnDelete.Enabled = false;

    }
    bool ValidateForm()
    {
        if (txtSaleAgentId.Text.Trim() == "")
        {
            ErrorMessage = "Sale Agent Id is required.";
            return false;
        }
        else if (txtFullNameEn.Text.Trim() == "")
        {
            ErrorMessage = "Full Name EN is required.";
            return false;
        }
        else if (ddlSaleAgentType.SelectedValue == "")
        {
            ErrorMessage = "Sale Agent Type is required.";
            return false;
        }
        else if (!Helper.IsDate(txtValidFrom.Text.Trim()))
        {
            ErrorMessage = "Valid From is required with format [DD/MM/YYYY].";
            return false;
        }
        else if (!Helper.IsDate(txtValidTo.Text.Trim()))
        {

            ErrorMessage = "Valid To is required with format [DD/MM/YYYY].";
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
        txtSaleAgentId.Text = "";
        txtFullNameEn.Text = "";
        txtFullNameKh.Text = "";
        txtPosition.Text = "";
        ddlStatus.SelectedIndex = 0;
        txtRemarks.Text = "";
        txtEmail.Text = "";
        SaleAgentId = string.Empty;
        txtValidFrom.Text = "";
        txtValidTo.Text = "";
        ddlSaleAgentType.SelectedIndex = 0;
    }
    void LoadData()
    {
        try
        {
            List<bl_sale_agent_micro> objList = new List<bl_sale_agent_micro>();
            if (txtSaleAgentSearch.Text.Trim() == "")
            {
                objList = da_sale_agent.GetSaleAgentMicroList();
            }
            else
            {
                objList = da_sale_agent.GetSaleAgentMicroList(txtSaleAgentSearch.Text.Trim());
            }
            if (objList.Count > 0)
            {
                gvParam.DataSource = objList;
                gvParam.DataBind();
            }
            else
            {
                Helper.Alert(false, "No data found.", lblError);
                gvParam.DataSource = null;
                gvParam.DataBind();

            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    void ReLoadSave()
    {
        try
        {
            List<bl_sale_agent_micro> objList = new List<bl_sale_agent_micro>();
            bl_sale_agent_micro obj = da_sale_agent.GetSaleAgentMicro(SaleAgentId);
            objList.Add(obj);
            gvParam.DataSource = objList;
            gvParam.DataBind();
           
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error ReloadSave(): " + ex.Message, lblError);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        UserName = System.Web.Security.Membership.GetUser().UserName;
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
            string fullNameEn = txtFullNameEn.Text.Trim();
            string fullNameKh = txtFullNameKh.Text.Trim();
            string position = txtPosition.Text.Trim();
            int status = Convert.ToInt32(ddlStatus.SelectedValue);
            string remarks = txtRemarks.Text.Trim();
            string saleAgentId=txtSaleAgentId.Text.Trim();
            string email = txtEmail.Text.Trim();

            DateTime validFrom = Helper.FormatDateTime(txtValidFrom.Text.Trim());
            DateTime validTo = Helper.FormatDateTime(txtValidTo.Text.Trim());

            int agentType = Convert.ToInt32(ddlSaleAgentType.SelectedValue);
            bl_sale_agent_micro obj = new bl_sale_agent_micro()
            {
                FullNameEn = fullNameEn,
                FullNameKh = fullNameKh,
                Position = position,
                SaleAgentType = agentType,
                Status = status,
                CreatedBy = UserName,
                CreatedOn = DateTime.Now,
                CreatedNote = remarks,
                Email = email,
                ValidFrom = validFrom,
                ValidTo=validTo
            };

            if (string.IsNullOrWhiteSpace(SaleAgentId))
            {
                obj.SaleAgentId = saleAgentId;
                if (da_sale_agent.AddSaleAgentMicro(obj))
                {
                    SaleAgentId = obj.SaleAgentId;
                    Helper.Alert(false, "Added new agent successfully.", lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, "Added agent fail.", lblError);
                }


            }
            else
            {
                obj.SaleAgentId = SaleAgentId;
                if (da_sale_agent.UpdateSaleAgentMicro(obj))
                {
                    Helper.Alert(false, "Updated  agent successfully.", lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, "Updated agent fail.", lblError);
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
        Helper.Alert(false, "Delete function is not implemented.", lblError);
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
            Label lSaleAgentId = (Label)r.FindControl("lblSaleAgentId");
            Label lFullNameEn = (Label)r.FindControl("lblFullNameEn");
            Label lFullNameKh = (Label)r.FindControl("lblFullNameKh");
            Label lPosition = (Label)r.FindControl("lblPosition");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lRemarks = (Label)r.FindControl("lblRemarks");
            Label lEmail = (Label)r.FindControl("lblEmail");
            Label lAgentType = (Label)r.FindControl("lblSaleAgentType");
            Label lValidFrom = (Label)r.FindControl("lblValidFrom");
            Label lValidTo = (Label)r.FindControl("lblValidTo");
            SaleAgentId = lSaleAgentId.Text;
            txtSaleAgentId.Text = lSaleAgentId.Text;
            txtFullNameEn.Text = lFullNameEn.Text;
            txtFullNameKh.Text = lFullNameKh.Text;
            txtPosition.Text = lPosition.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
            txtRemarks.Text = lRemarks.Text;
            txtEmail.Text = lEmail.Text;

            Helper.SelectedDropDownListIndex("VALUE", ddlSaleAgentType, lAgentType.Text);
            txtValidFrom.Text = lValidFrom.Text;
            txtValidTo.Text = lValidTo.Text;

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