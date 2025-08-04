using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.Linq.SqlClient;
public partial class Pages_Channel_05frmChannelLocationAgentMapping : System.Web.UI.Page
{

    private string AgentMappingId { get { return ViewState["V_AGENT_MAPPIING_ID"] + ""; } set { ViewState["V_AGENT_MAPPIING_ID"] = value; } }
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
            LoadData();
            divAdd.Visible = false;
            EnableControl(false);
        }
        else if (type.ToString() == Helper.FormTransactionType.SEARCH.ToString())
        {
            LoadData(txtSearchInfo.Text.Trim());
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
        cblBranch.Enabled = t;
        cblProduct.Enabled = t;
        txtUserName.Enabled = t;
        txtAgentCode.Enabled = t;
        ddlStatus.Enabled = t;
        txtRemarks.Enabled = t;
        ddlStatus.Enabled = t;
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
        else if (cblBranch.Items.Count==0)
        {
            ErrorMessage = "Branch is required.";
            return false;
        }
        else if (cblProduct.Items.Count == 0)
        {
            ErrorMessage = "Product code is required.";
            return false;
        }
        else if (txtUserName.Text.Trim() == "")
        {
            ErrorMessage = "User Name is required.";
            return false;
        }
        else if (txtAgentCode.Text.Trim() == "")
        {
            ErrorMessage = "Agent Code is required.";
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
        AgentMappingId = string.Empty;
        ddlChannelName.SelectedIndex = 0;
        cblBranch.Items.Clear();
        txtUserName.Text = "";
        txtAgentCode.Text = "";
        ddlStatus.SelectedIndex = 0;
        txtRemarks.Text = "";
        cblProduct.Items.Clear();
    }
    void BindChannel()
    {
        Helper.BindChannelItem(ddlChannelName, "0152DF80-BA95-46A9-BB7A-E71966A34089");
    }
    void BindChannelLocation()
    {
        Helper.BindChanneLocation(cblBranch, ddlChannelName.SelectedValue);
    }
    private void BindProduct()
    {
        Options.Bind(cblProduct, da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(ddlChannelName.SelectedValue), bl_micro_product_config.NAME.ProductRemarks, bl_micro_product_config.NAME.Product_ID, -1);
    }
    void LoadData(string filter = "")
    {
        try
        {
            filter = txtSearchInfo.Text.Trim();
            List<bl_agent_mapping> objList = da_agent_mapping.GetAgentMappingList(false);

            if (filter != "")
            {
                List<bl_agent_mapping> filterResult = objList.Where(x => x.UserName.Contains(filter)).ToList();
                gvParam.DataSource = filterResult;
                gvParam.DataBind();
            }
            else
            {
                gvParam.DataSource = objList;
                gvParam.DataBind();
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "LoadData() error." + ex.Message, lblError);

        }
    }
    void ReLoadSave()
    {
        try
        {
            List<bl_agent_mapping> objList = new List<bl_agent_mapping>();
            objList.Add(da_agent_mapping.GetAgentMapping(AgentMappingId));
            gvParam.DataSource = objList;
            gvParam.DataBind();
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "ReloadSave() error.", lblError);
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
        if (this.ValidateForm())
        {
            bl_agent_mapping blAgentMapping = new bl_agent_mapping()
            {
                ChannelLocationId = "",
                UserName = this.txtUserName.Text.Trim(),
                SaleAgentId = this.txtAgentCode.Text.Trim(),
                Status = Convert.ToInt32(this.ddlStatus.SelectedValue),
                CreatedBy = this.UserName,
                CreatedOn = DateTime.Now,
                Remarks = this.txtRemarks.Text.Trim(),
                UpdatedBy = this.UserName,
                UpdatedOn = DateTime.Now
            };
            string str = "";
            for (int index = 0; index < this.cblProduct.Items.Count; ++index)
            {
                if (this.cblProduct.Items[index].Selected)
                    str += str != "" ? "," + this.cblProduct.Items[index].Value : this.cblProduct.Items[index].Value;
            }
            int num = 0;
            if (string.IsNullOrWhiteSpace(this.AgentMappingId))
            {
                for (int index = 0; index <= this.cblBranch.Items.Count - 1; ++index)
                {
                    if (this.cblBranch.Items[index].Selected)
                    {
                        blAgentMapping.ChannelLocationId = this.cblBranch.Items[index].Value;
                        blAgentMapping.Id = da_agent_mapping.GetId();
                        blAgentMapping.ProductId = str;
                        num = da_agent_mapping.Save(blAgentMapping) ? 1 : 0;
                    }
                }
                if (num > 0)
                {
                    Helper.Alert(false, da_agent_mapping.Message, this.lblError);
                    this.AgentMappingId = blAgentMapping.Id;
                    this.Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                    Helper.Alert(true, da_agent_mapping.Message, this.lblError);
            }
            else
            {
                blAgentMapping.Id = this.AgentMappingId;
                blAgentMapping.ProductId = str;
                for (int index = 0; index <= this.cblBranch.Items.Count - 1; ++index)
                {
                    if (this.cblBranch.Items[index].Selected)
                    {
                        blAgentMapping.ChannelLocationId = this.cblBranch.Items[index].Value;
                        break;
                    }
                }
                if (da_agent_mapping.Update(blAgentMapping))
                {
                    Helper.Alert(false, da_agent_mapping.Message, this.lblError);
                    this.Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                    Helper.Alert(true, da_agent_mapping.Message, this.lblError);
            }
        }
        else
            Helper.Alert(true, this.ErrorMessage, this.lblError);
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            bl_agent_mapping obj = new bl_agent_mapping()
            {
               // ChannelLocationId = ddlBranch.SelectedValue,
                UserName = txtUserName.Text.Trim(),
                SaleAgentId = txtAgentCode.Text.Trim(),
                Status = Convert.ToInt32(ddlStatus.SelectedValue),
                CreatedBy = UserName,
                CreatedOn = DateTime.Now,
                Remarks = txtRemarks.Text.Trim(),
                UpdatedBy = UserName,
                UpdatedOn = DateTime.Now
            };
            if (string.IsNullOrWhiteSpace(AgentMappingId))
            {
                obj.Id = da_agent_mapping.GetId();
                if (da_agent_mapping.Save(obj))
                {
                    Helper.Alert(false, da_agent_mapping.Message, lblError);
                    AgentMappingId = obj.Id;
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, da_agent_mapping.Message, lblError);
                }
            }
            else
            {
                obj.Id = AgentMappingId;
                if (da_agent_mapping.Update(obj))
                {
                    Helper.Alert(false, da_agent_mapping.Message, lblError);
                    Transaction(Helper.FormTransactionType.SAVE);
                }
                else
                {
                    Helper.Alert(true, da_agent_mapping.Message, lblError);
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
        if (!string.IsNullOrWhiteSpace(AgentMappingId))
        {
            if (da_agent_mapping.Delete(AgentMappingId))
            {
                Helper.Alert(false, da_agent_mapping.Message, lblError);
                Transaction(Helper.FormTransactionType.DELETE);
            }
            else
            {
                Helper.Alert(true, da_agent_mapping.Message, lblError);
            }
        }
        else {
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
            Label lChannelLocationId = (Label)r.FindControl("lblChannelLocationId");
            Label lAgentName = (Label)r.FindControl("lblAgentName");
            Label lAgentCode = (Label)r.FindControl("lblAgentCode");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lRemarks = (Label)r.FindControl("lblRemarks");
            Label lUserName = (Label)r.FindControl("lblUserName");
            Label lProudctId = (Label)r.FindControl("lblProductId");
            Helper.SelectedDropDownListIndex("VALUE", ddlChannelName, lChannelItemId.Text);
            ddlChannelName_SelectedIndexChanged(null, null);
            AgentMappingId = lId.Text;
           Helper.SelectedCheckBoxListIndex("VALUE", cblBranch, lChannelLocationId.Text,false);
            txtUserName.Text = lUserName.Text;
            txtAgentCode.Text = lAgentCode.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
            txtRemarks.Text = lRemarks.Text;

            List<string> list = lProudctId.Text.Split(',').ToList<string>();
            for (int index = 0; index < this.cblProduct.Items.Count; ++index)
            {
                string str1 = this.cblProduct.Items[index].Value;
                foreach (string str2 in list)
                {
                    if (str2.ToLower() == str1.ToLower())
                    {
                        this.cblProduct.Items[index].Selected = true;
                        break;
                    }
                }
            }

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
    protected void ddlChannelName_SelectedIndexChanged(object sender, EventArgs e)
    {
       cblBranch.Items.Clear();
        BindChannelLocation();
        BindProduct();
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int num = this.cblBranch.Items.Cast<ListItem>().Count<ListItem>((Func<ListItem, bool>)(li => li.Selected));
        int count = this.cblBranch.Items.Count;
        string[] strArray = this.Request.Form["__EVENTTARGET"].Split('$');
        if (int.Parse(strArray[strArray.Length - 1]) == 0)
        {
            if (this.cblBranch.Items[0].Selected)
            {
                for (int index = 0; index <= this.cblBranch.Items.Count - 1; ++index)
                    this.cblBranch.Items[index].Selected = true;
            }
            else
                this.cblBranch.ClearSelection();
        }
        else if (num < count && this.cblBranch.Items[0].Selected)
        {
            this.cblBranch.Items[0].Selected = false;
        }
        else
        {
            if (num != count - 1)
                return;
            this.cblBranch.Items[0].Selected = true;
        }
    }
}