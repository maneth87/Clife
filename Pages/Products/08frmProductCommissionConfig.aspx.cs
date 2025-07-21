using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Products_08frmProductCommissionConfig : System.Web.UI.Page
{
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ProductDiscountConfigID { get { return ViewState["_PRODUCT_DIS_CONFIG_ID"] + ""; } set { ViewState["_PRODUCT_DIS_CONFIG_ID"] = value; } }
    private List<bl_micro_product_config> ProductConfig { get { return (List<bl_micro_product_config>)ViewState["_PRODUCT_CONFIG_OBJ"]; } set { ViewState["_PRODUCT_CONFIG_OBJ"] = value; } }

    void LoadData()
    {
        try
        {
            string strVal = txtSearchValue.Text.Trim();
            string strCol = ddlSeachBy.SelectedValue;

            List<bl_micro_product_commission_config> comList = new List<bl_micro_product_commission_config>();
            da_micro_production_commission_config comObj = new da_micro_production_commission_config();

            if (strCol != "" && strVal!="")
            {

                comList = comObj.GetProductionCommConfigList(strCol, "%"+strVal+"%");
            }
            else
            {
                comList = comObj.GetProductionCommConfigList();
            }

            DataTable tbl = Convertor.ToDataTable(comList);
            gvParam.DataSource = tbl;
            gvParam.DataBind();
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Load data error, please contact your system administrator.", lblError);
            Log.AddExceptionToLog("Error function [LoadData()] in page [frmProductSetup], detail: " + ex.Message);
        }
    }

    /// <summary>
    /// Type: {1: First Load, 2: add new , 3: update, 4: cancel, 5: edit, 6: search, 7:relead}
    /// </summary>
    /// <param name="type"></param>
    void Transaction(int type)
    {
        if (type == 1)//firstload
        {
            EnableControl(false);
            LoadData();

        }
        else if (type == 2)//add new
        {
            ClearText();
            EnableControl();
            btnDelete.Enabled = false;


        }
        else if (type == 3)//update
        {
            LoadData();
            ClearText();
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;

        }
        else if (type == 4)//cancel
        {
            ClearText();
            EnableControl(false);
        }
        else if (type == 5)//edit
        {
            ClearText();
            EnableControl();

            btnDelete.Enabled = false;
        }
        else if (type == 6)//search
        {
            LoadData();
        }
        else if (type == 7)//reload
        {


        }
    }
    void EnableControl(bool yes = true)
    {
        ddlChannelItem.Enabled = yes;
        ddlProduct.Enabled = yes;
        txtProductName.Enabled = false;
        ddlCommissionType.Enabled = yes;
        ddlStatus.Enabled = yes;
        ddlValueType.Enabled = yes;
        txtValue.Enabled = yes;
        txtRemarks.Enabled = yes;
        txtEffectiveFrom.Enabled = yes;
        txtEffectiveTo.Enabled = yes;
        btnSave.Enabled = yes;
        btnDelete.Enabled = yes;
        btnCancel.Enabled = yes;
        ddlClientType.Enabled = yes;
    }
    void ClearText()
    {
        ddlChannelItem.SelectedIndex = 0;
        ddlProduct.Items.Clear();
        txtProductName.Text = "";
        ddlCommissionType.SelectedIndex = 0;
        ddlValueType.SelectedIndex = 0;
        txtValue.Text = "";
        txtEffectiveFrom.Text = "";
        txtEffectiveTo.Text = "";
        txtRemarks.Text = "";
        ddlStatus.SelectedIndex = 0;
        ProductDiscountConfigID = string.Empty;
        ddlClientType.SelectedIndex = 0;
    }

    bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(ddlChannelItem.SelectedValue))
        {
            ErrorMessage = "Channel Item Name is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlProduct.SelectedValue))
        {
            ErrorMessage = "Product ID is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlCommissionType.SelectedValue))
        {
            ErrorMessage = "Commission Type is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlValueType.SelectedValue))
        {
            ErrorMessage = "Value Type is required.";
            return false;
        }
        else if (!Helper.IsAmount(txtValue.Text))
        {
            ErrorMessage = "Value is required.";
            return false;
        }
        else if (!Helper.IsDate(txtEffectiveFrom.Text))
        {
            ErrorMessage = "Effective From is required.";
            return false;
        }
        else if (!Helper.IsDate(txtEffectiveTo.Text))
        {
            ErrorMessage = "Effective To is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
        {
            ErrorMessage = "Status is required as number.";
            return false;
        }

        else
        {
            ErrorMessage = string.Empty;
            return true;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            ddlSeachBy.Items.Add(new ListItem("--- Select ---", ""));
            ddlSeachBy.Items.Add(new ListItem(bl_micro_product_commission_config.Table.Columns.ProductId, bl_micro_product_commission_config.Table.Columns.ProductId));
            ddlSeachBy.Items.Add(new ListItem(bl_micro_product_commission_config.Table.Columns.ChannelName, bl_micro_product_commission_config.Table.Columns.ChannelName));
            /*bing product id from product config*/
            List<bl_micro_product_config> proList = da_micro_product_config.ProductConfig.GetProductMicroProductSO();
            ProductConfig = proList;

            List<bl_channel_item> chList = da_micro_product_config.ProductConfig.GetChannelItemList();
            Options.Bind(ddlChannelItem, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");


            ddlCommissionType.Items.Add(new ListItem("--- Select ---", ""));
            ddlCommissionType.Items.Add(new ListItem(bl_micro_product_commission_config.CommissionTypeOption.Incentive, bl_micro_product_commission_config.CommissionTypeOption.Incentive));
            ddlCommissionType.Items.Add(new ListItem(bl_micro_product_commission_config.CommissionTypeOption.ReferralFee, bl_micro_product_commission_config.CommissionTypeOption.ReferralFee));
            ddlValueType.Items.Add(new ListItem("--- Select ---", ""));
            ddlValueType.Items.Add(new ListItem(bl_micro_product_commission_config.ValueTypeOption.Fix, bl_micro_product_commission_config.ValueTypeOption.Fix));
            ddlValueType.Items.Add(new ListItem(bl_micro_product_commission_config.ValueTypeOption.Percentage, bl_micro_product_commission_config.ValueTypeOption.Percentage));
            Transaction(1);


        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Transaction(6);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bl_micro_product_commission_config comObj = new bl_micro_product_commission_config();
        da_micro_production_commission_config comTran = new da_micro_production_commission_config();
        comObj.ProductId = ddlProduct.SelectedValue;
        comObj.ChannelItemId = ddlChannelItem.SelectedValue;
        comObj.CommissionType = ddlCommissionType.SelectedValue;
        comObj.ValueType = ddlValueType.SelectedValue;
        comObj.Value = Convert.ToDouble(txtValue.Text);
        comObj.EffectiveFrom = Helper.FormatDateTime(txtEffectiveFrom.Text);
        comObj.EffectiveTo = Helper.FormatDateTime(txtEffectiveTo.Text);
        comObj.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        comObj.CreatedRemarks = txtRemarks.Text;
        comObj.ClientType = ddlClientType.SelectedValue;
        if (string.IsNullOrWhiteSpace(ProductDiscountConfigID)) //Save
        {
            comObj.ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PRODUCT_COMMISSION_CONFIG" }, { "FIELD", "ID" } });
            comObj.CreatedBy = LoginUserName;
            comObj.CreatedOn = DateTime.Now;
            if (comTran.Save(comObj))
            {
                Helper.Alert(false, "Saved successfully.", lblError);
                Transaction(3);
            }
            else
            {
                Helper.Alert(true, "Saved fail.", lblError);
            }
        }
        else //update
        {
            comObj.ID = ProductDiscountConfigID;
            comObj.UpdatedBy = LoginUserName;
            comObj.UpdatedOn = DateTime.Now;
            comObj.UpdatedRemarks = txtRemarks.Text;
            if(comTran.Update(comObj))
            {
             
                Helper.Alert(false, "Updated successfully.", lblError);
                Transaction(3);
            }
            else
            {
                Helper.Alert(true, "Updated fail.", lblError);
            }


        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(4);
    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProduct.Items.Clear();
        ddlProduct.Items.Add(new ListItem("--- Select ---", ""));
        foreach (bl_micro_product_config pro in ProductConfig.Where(_ => _.ChannelItemId == ddlChannelItem.SelectedValue))
        {
            ddlProduct.Items.Add(new ListItem(pro.ProductIDRemarks, pro.Product_ID));

        }

    }

    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Transaction(5);
            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lId = (Label)r.FindControl("lblId");
            Label lChannelName = (Label)r.FindControl("lblChannelName");
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lCommissionType = (Label)r.FindControl("lblCommissionType");
            Label lValueType = (Label)r.FindControl("lblValueType");
            Label lValue = (Label)r.FindControl("lblValue");
            Label lEffectiveFrom = (Label)r.FindControl("lblEffectiveFrom");
            Label lEffectiveTo = (Label)r.FindControl("lblEffectiveTo");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lRemarks = (Label)r.FindControl("lblRemarks");
            Label lClientType = (Label)r.FindControl("lblClientType");

            ProductDiscountConfigID = lId.Text;

            Helper.SelectedDropDownListIndex("TEXT", ddlChannelItem, lChannelName.Text);
            ddlChannelItem_SelectedIndexChanged(null, null);

            Helper.SelectedDropDownListIndex("VALUE", ddlProduct, lProductId.Text);
            ddlProduct_SelectedIndexChanged1(null, null);

            Helper.SelectedDropDownListIndex("VALUE", ddlCommissionType, lCommissionType.Text);
            Helper.SelectedDropDownListIndex("VALUE", ddlValueType, lValueType.Text);
            txtValue.Text = lValue.Text;
            txtEffectiveFrom.Text = lEffectiveFrom.Text;
            txtEffectiveTo.Text = lEffectiveTo.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
            txtRemarks.Text = lRemarks.Text;

            Helper.SelectedDropDownListIndex("VALUE", ddlClientType, lClientType.Text);

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
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(2);
    }
    protected void ddlProduct_SelectedIndexChanged1(object sender, EventArgs e)
    {
        txtProductName.Text = da_product.GetProductByProductID(ddlProduct.SelectedValue).En_Title;
    }

}