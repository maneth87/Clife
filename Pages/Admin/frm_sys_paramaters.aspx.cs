using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Admin_frm_sys_paramaters : System.Web.UI.Page
{

    private string ParamaterName { get { return ViewState["_PARAM_NAME"] + ""; } set {  ViewState["_PARAM_NAME"] =value; } }
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set {  ViewState["_ERROR_MESSAGE"] =value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ApplicationId { get { return da_user.GetApplicationID(System.Web.Security.Membership.ApplicationName); } }
    protected void Page_Load(object sender, EventArgs e)
    {

        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            Transaction(1);
           
        }
    }
    /// <summary>
    /// [TYPE]:[1: first load, 2: Add new, 3: Update, 4: reload, 5 SEARCH]
    /// </summary>
    /// <param name="type"></param>
    void Transaction(int type)
    {
        if (type == 1)//first load
        {
            ParamaterName = string.Empty;
            ClearText();
            txtIdAdd.Enabled = false;
            txtParamaterNameAdd.Enabled = false;
            ddlActive.Enabled = false;
            txtParamaterValueAdd.Enabled = false;
            txtParamaterDesc.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            LoadData();
        }
        else if (type == 2)//add new
        {
            txtIdAdd.Enabled = true;
            txtParamaterNameAdd.Enabled = true;
            ddlActive.Enabled = true;
            txtParamaterValueAdd.Enabled = true;
            txtParamaterDesc.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            ClearText();
            txtIdAdd.Focus();
        }
        else if (type == 3)//update
        {
            txtIdAdd.Enabled = false;
            txtParamaterNameAdd.Enabled = false;
            ddlActive.Enabled = true;
            txtParamaterValueAdd.Enabled = true;
            txtParamaterDesc.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
        }
        else if (type == 4)//reload
        {
            LoadData();
            txtIdAdd.Enabled = true;
            txtParamaterNameAdd.Enabled = true;
            ddlActive.Enabled = true;
            txtParamaterValueAdd.Enabled = true;
            txtParamaterDesc.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            ClearText();

        }
        else if (type == 5)//SEARCH
        {
         
            txtIdAdd.Enabled = false;
            txtParamaterNameAdd.Enabled = false;
            ddlActive.Enabled = false;
            txtParamaterValueAdd.Enabled = false;
            txtParamaterDesc.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            ClearText();
            LoadData("%"+txtParamaterNameSearch.Text+ "%");
        }
    }
    void ClearText()
    {
        ParamaterName = string.Empty;
        txtIdAdd.Text = "";
        txtParamaterNameAdd.Text = "";
        txtParamaterValueAdd.Text = "";
        txtParamaterDesc.Text = "";
        ddlActive.SelectedIndex = 0;
    }
    void LoadData(string paramatername="")
    {
        List<bl_system.SYSTEM_PARAMATER> listParam = new List<bl_system.SYSTEM_PARAMATER>();
        bl_system.SYSTEM_PARAMATER objParam =new bl_system.SYSTEM_PARAMATER();
        listParam = objParam.GetParamaterList(ApplicationId, paramatername);

        DataTable tbl = Convertor.ToDataTable(listParam);

        gvParam.DataSource = tbl;
        gvParam.DataBind();
    }
     bool ValidateForm()
    {
        bool isValide=false;
        if (string.IsNullOrWhiteSpace(txtIdAdd.Text))
        {
            ErrorMessage = "ID is required.";
        }
        else if (string.IsNullOrWhiteSpace(txtParamaterNameAdd.Text))
        {
            ErrorMessage = "Paramater Name is required.";
        }
        else if (string.IsNullOrWhiteSpace(txtParamaterValueAdd.Text))
        {
            ErrorMessage = "Paramater Value is required.";
        }else if(string.IsNullOrWhiteSpace(txtParamaterDesc.Text))
        {
            ErrorMessage="Paramater Description is required.";
        }
        else
        {
            isValide =true;
            ErrorMessage=string.Empty;
        }
        return isValide;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            if (string.IsNullOrEmpty(ParamaterName))
            {
                //save new
               
                if (sysObj.Save(ApplicationId, Convert.ToInt32(txtIdAdd.Text.Trim()), txtParamaterNameAdd.Text.Trim().ToUpper(), txtParamaterValueAdd.Text.Trim(), txtParamaterDesc.Text.Trim(), ddlActive.SelectedValue == "1" ? true : false, LoginUserName, DateTime.Now))
                {
                    Helper.Alert(false, "Saved successfully.", lblError);
                    Transaction(4);
                }
                else
                {
                    Helper.Alert(true, sysObj.ErrorMessage, lblError);
                }
            }
            else
            {
                //update existing 

                if (sysObj.Update( ApplicationId, Convert.ToInt32(txtIdAdd.Text.Trim()), ParamaterName, txtParamaterValueAdd.Text.Trim(), txtParamaterDesc.Text.Trim(), ddlActive.SelectedValue == "1" ? true : false, LoginUserName, DateTime.Now))
                {
                    Helper.Alert(false, "Updated successfully.", lblError);
                    Transaction(4);
                }
                else {
                    Helper.Alert(true, sysObj.ErrorMessage, lblError);
                }
            }
        }
        else
        {
            Helper.Alert(true, ErrorMessage, lblError);
        }
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(2);
    }
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lId = (Label)r.FindControl("lblId");
            Label lName = (Label)r.FindControl("lblParamaterName");
            Label lActive = (Label)r.FindControl("lblIsActive");
            Label lValue = (Label)r.FindControl("lblParamaterVal");
            Label lDescription = (Label)r.FindControl("lblParamaterDesc");
            txtIdAdd.Text = lId.Text;
            txtParamaterNameAdd.Text = lName.Text;
            Helper.SelectedDropDownListIndex("VALUE", ddlActive, lActive.Text);
            txtParamaterValueAdd.Text = lValue.Text;
            txtParamaterDesc.Text = lDescription.Text;
            ParamaterName = lName.Text;
          
            Transaction(3);
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(1);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Transaction(5);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        if (!string.IsNullOrEmpty(ParamaterName))
        {
            if (sysObj.Delete(ApplicationId, ParamaterName))
            {
                Helper.Alert(false, "Deleted successfully.", lblError);
                Transaction(4);
            }
            else
            {
                Helper.Alert(true,sysObj.ErrorMessage, lblError);
            }

        }
        else
        {
            Helper.Alert(true,"No record for delete.", lblError);
        }
    }
}