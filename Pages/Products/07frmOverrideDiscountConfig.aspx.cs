using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Products_07frmOverrideDiscountConfig : System.Web.UI.Page
{
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string OverridDiscountConfigID { get { return ViewState["_OVER_DISCOUNT_CONFIG_ID"] + ""; } set { ViewState["_OVER_DISCOUNT_CONFIG_ID"] = value; } }
    private List<bl_micro_product_config> ProductConfig { get { return (List<bl_micro_product_config>)ViewState["_PRODUCT_CONFIG_OBJ"]; } set { ViewState["_PRODUCT_CONFIG_OBJ"] = value; } }
    private string CustomerId { get { return ViewState["_CUSTOMER_ID"] + ""; } set { ViewState["_CUSTOMER_ID"] = value; } }
    private string PolicyId { get { return ViewState["_POLICY_ID"] + ""; } set { ViewState["_POLICY_ID"] = value; } }

    void LoadData()
    {
        try
        {
            string val = txtValue.Text.Trim();
            
            List<bl_override_discount_config> overList = new List<bl_override_discount_config>();
            if (val != "")
            {
                overList = da_micro_product_config.OverrideDiscountConfig.GetOverriderDiscountList(ddlSearchBy.SelectedValue, "%" + val + "%");
               
            }
            else
            {
                overList = da_micro_product_config.OverrideDiscountConfig.GetOverriderDiscountList();
            }

            DataTable tbl = Convertor.ToDataTable(overList);
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
        ddlRiderId.Enabled = yes;
        txtBasicSa.Enabled = yes;
        txtBasicDisAmount.Enabled = yes;
        txtRiderSa.Enabled = yes;
        txtRiderDisAmount.Enabled = yes;
        ddlStatus.Enabled = yes;
        txtRemarks.Enabled = yes;
        txtEffectiveDate.Enabled = yes;
        txtExpiryDate.Enabled = yes;
        btnSave.Enabled = yes;
        btnDelete.Enabled = yes;
        btnCancel.Enabled = yes;
        txtCustomerName.Enabled = false;
        
    }
    void ClearText()
    {
        ddlChannelItem.SelectedIndex = 0;

        //if (ddlProduct.Items.Count > 0)
        //    ddlProduct.SelectedIndex = 0;
        //if(ddlRiderId.Items.Count>0)
        //    ddlRiderId.SelectedIndex = 0;
        ddlProduct.Items.Clear();
        ddlRiderId.Items.Clear();

        txtBasicSa.Text = "";
        txtBasicDisAmount.Text = "";
        txtRiderSa.Text = "";
        txtRiderDisAmount.Text = "";
        txtEffectiveDate.Text = "";
        txtExpiryDate.Text = "";
        txtRemarks.Text = "";
        txtCustomerName.Text = "";
        txtCustomerNumber.Text = "";
        txtPolicyNumber.Text = "";
        ddlStatus.SelectedIndex = 0;
        OverridDiscountConfigID = string.Empty;
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
        else if (!Helper.IsNumber(txtBasicSa.Text))
        {
            ErrorMessage = "Basic SA Range required as number.";
            return false;
        }
        else if (!Helper.IsNumber(txtBasicDisAmount.Text))
        {
            ErrorMessage = "Basic Discount Amount required as number.";
            return false;
        }
        //else if (string.IsNullOrWhiteSpace(ddlRiderId.SelectedValue))
        //{
        //    ErrorMessage = "Rider ID is required.";
        //    return false;
        //}
        //else if (!Helper.IsNumber(txtRiderSaRange.Text))
        //{
        //    ErrorMessage = "Rider SA Range required as number.";
        //    return false;
        //}
        else if (!Helper.IsDate(txtEffectiveDate.Text))
        {
            ErrorMessage = "Effective Date is required with format {DD/MM/YYYY}.";
            return false;
        }

        else if (!Helper.IsDate(txtExpiryDate.Text))
        {
            ErrorMessage = "Expiry Date is required with format {DD/MM/YYYY}.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
        {
            ErrorMessage = "Status is required as number.";
            return false;
        }

        else
        {
            bl_micro_policy pol = da_micro_policy.GetPolicyByNumber(txtPolicyNumber.Text);
            bl_micro_customer1 cus = da_micro_customer.GetCustomerByCustomerNo(txtCustomerNumber.Text);
            if (string.IsNullOrWhiteSpace(pol.POLICY_NUMBER))
            {
                ErrorMessage = "Policy number is not found.";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cus.CUSTOMER_NUMBER))
            {
                ErrorMessage = "Customer number is not found.";
                return false;
            }
            else
            {
                //check policy is own of customer 
                if (pol.CUSTOMER_ID != cus.ID)
                {
                    ErrorMessage = "Policy number ["+ pol.POLICY_NUMBER +"] is not under customer number ["+ cus.CUSTOMER_NUMBER+"].";
                    return false;
                }
                else
                {

                    PolicyId = pol.POLICY_ID;
                    CustomerId = cus.ID;
                    ErrorMessage = string.Empty;
                    return true;
                }
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            /*bing product id from product config*/
            List<bl_micro_product_config> proList = da_micro_product_config.ProductConfig.GetProductMicroProductSO();
            ProductConfig = proList;
            List<bl_channel_item> chList = da_micro_product_config.ProductConfig.GetChannelItemList();
            Options.Bind(ddlChannelItem, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");

            ddlSearchBy.Items.Add(new ListItem("--- Select ---", ""));
            ddlSearchBy.Items.Add(new ListItem(bl_override_discount_config.Table.Columns.CustomerNumber, bl_override_discount_config.Table.Columns.CustomerNumber));
            ddlSearchBy.Items.Add(new ListItem(bl_override_discount_config.Table.Columns.CustomerLastName, bl_override_discount_config.Table.Columns.CustomerLastName));
            ddlSearchBy.Items.Add(new ListItem(bl_override_discount_config.Table.Columns.CustomerFirstName, bl_override_discount_config.Table.Columns.CustomerFirstName));
            ddlSearchBy.Items.Add(new ListItem(bl_override_discount_config.Table.Columns.PolicyNumber, bl_override_discount_config.Table.Columns.PolicyNumber));
    
            Transaction(1);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Transaction(6);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            try
            {
                bl_override_discount_config over = new bl_override_discount_config();
                over.ProductId = ddlProduct.SelectedValue;
                over.ProductRiderId = ddlRiderId.SelectedValue;
                over.BasicDiscountAmount = Convert.ToDouble(txtBasicDisAmount.Text);
                over.BasicSumAssured = Convert.ToDouble(txtBasicSa.Text);
                over.RiderDiscountAmount = Convert.ToDouble(txtRiderDisAmount.Text);
                over.RiderSumAssured = Convert.ToDouble(txtRiderSa.Text);
                over.ChannelItemId = ddlChannelItem.SelectedValue;
                over.EffectiveDate = Helper.FormatDateTime(txtEffectiveDate.Text);
                over.ExpiryDate = Helper.FormatDateTime(txtExpiryDate.Text);
                over.Remarks = txtRemarks.Text;
                over.Status = ddlStatus.SelectedValue == "1" ? true : false;
                over.PolicyId = PolicyId;
                over.CustomerId = CustomerId;
                if (string.IsNullOrEmpty(OverridDiscountConfigID))
                {
                    over.Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PRODUCT_OVERRIDE__DISCOUNT_CONFIG" }, { "FIELD", "ID" } });
                    over.CreatedBy = LoginUserName;
                    over.CreatedOn = DateTime.Now;
                    
                    if (da_micro_product_config.OverrideDiscountConfig.Save(over))
                    {
                        Helper.Alert(false, "Saved successfully.", lblError);
                        Transaction(3);
                    }
                    else
                    {
                        Helper.Alert(true, "Saved fail.", lblError);
                    }
                }
                else
                {
                    over.Id = OverridDiscountConfigID;
                    over.UpdatedBy = LoginUserName;
                    over.UpdatedOn = DateTime.Now;
                    if (da_micro_product_config.OverrideDiscountConfig.Update(over))
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
            catch (Exception ex)
            {
                Helper.Alert(true, "SAVE / UPDATE transaction error. please contract your system administrator.", lblError);
                Log.AddExceptionToLog("Error function [btnSave_Click(object sender, EventArgs e)] in page [07frmOverrideDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
        }
        else
        {
            Helper.Alert(true, ErrorMessage, lblError);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Transaction(4);
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(2);
    }
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Transaction(5);
            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lId = (Label)r.FindControl("lblId");
            Label lChannelName = (Label)r.FindControl("lblChannelName");
            Label lCustomerId=(Label)r.FindControl("lblCustomerId");
            Label lCustomerNumber=(Label)r.FindControl("lblCustomerNumber");
            Label lCustomerName = (Label)r.FindControl("lblCustomerName");
            Label lPolicyNumber=(Label)r.FindControl("lblPolicyNumber");
            Label lPolicyId=(Label)r.FindControl("lblPolicyId");
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lRiderId = (Label)r.FindControl("lblRiderId");
            Label lBasicSa = (Label)r.FindControl("lblBasicSa");
            Label lRiderSa = (Label)r.FindControl("lblRiderSa");
            Label lBasicDisAdmount = (Label)r.FindControl("lblBasicDisAmount");
            Label lRiderDisAmount = (Label)r.FindControl("lblRiderDisAmount");
            Label lEffectiveDate = (Label)r.FindControl("lblEffectiveDate");
            Label lExpiryDate = (Label)r.FindControl("lblExpiryDate");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lRemarks = (Label)r.FindControl("lblRemarks");

            OverridDiscountConfigID = lId.Text;
            
            Helper.SelectedDropDownListIndex("TEXT", ddlChannelItem, lChannelName.Text);
            ddlChannelItem_SelectedIndexChanged(null, null);
            txtCustomerName.Text = lCustomerName.Text;
            txtCustomerNumber.Text = lCustomerNumber.Text;
            CustomerId = lCustomerId.Text;
            txtPolicyNumber.Text = lPolicyNumber.Text;
            PolicyId = lPolicyId.Text;
            Helper.SelectedDropDownListIndex("VALUE", ddlProduct, lProductId.Text);
            ddlProduct_SelectedIndexChanged(null, null);
            Helper.SelectedDropDownListIndex("VALUE", ddlRiderId, lRiderId.Text);
            txtBasicSa.Text = lBasicSa.Text;
            txtRiderSa.Text = lRiderSa.Text;
            txtBasicDisAmount.Text = lBasicDisAdmount.Text;
            txtRiderDisAmount.Text = lRiderDisAmount.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
            txtRemarks.Text = lRemarks.Text;
            txtEffectiveDate.Text = lEffectiveDate.Text;
            txtExpiryDate.Text = lExpiryDate.Text;
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
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProduct.Items.Clear();
        ddlRiderId.Items.Clear();
        // List<bl_micro_product_config> proList = da_micro_product_config.GetMicroProductConfigListByChannelItemId(ddlChannelItem.SelectedValue);
        // Options.Bind(ddlProduct, ProductConfig, bl_micro_product_config.NAME.Product_ID, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");

        foreach (bl_micro_product_config pro in ProductConfig.Where(_ => _.ChannelItemId == ddlChannelItem.SelectedValue))
        {
            ddlProduct.Items.Add(new ListItem(pro.ProductIDRemarks, pro.Product_ID));
        }
        ddlProduct_SelectedIndexChanged(null, null);
    }
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRiderId.Items.Clear();
        foreach (bl_micro_product_config pro in ProductConfig.Where(_ => _.Product_ID == ddlProduct.SelectedValue && _.ChannelItemId == ddlChannelItem.SelectedValue))
        {
            ddlRiderId.Items.Add(new ListItem(pro.ProductIDRemarks, pro.RiderProductID));
        }

    }
}