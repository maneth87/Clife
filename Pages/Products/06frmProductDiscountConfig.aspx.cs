using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Products_06frmProductDiscountConfig : System.Web.UI.Page
{
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ProductDiscountConfigID { get { return ViewState["_PRODUCT_DISCOUNT_CONFIG_ID"] + ""; } set { ViewState["_PRODUCT_DISCOUNT_CONFIG_ID"] = value; } }
    private List<bl_micro_product_config> ProductConfig { get { return (List<bl_micro_product_config>)ViewState["_PRODUCT_CONFIG_OBJ"]; } set { ViewState["_PRODUCT_CONFIG_OBJ"] = value; } }
    void LoadData()
    {
        try
        {
            string productName = txtProductNameSearch.Text.Trim();

            List<bl_micro_product_discount_config> proList = new List<bl_micro_product_discount_config>();

            if (productName != "")
            {

                proList = da_micro_product_config.DiscountConfig.GetDiscountConfigList("%" + productName + "%");
            }
            else
            {
                proList = da_micro_product_config.DiscountConfig.GetDiscountConfigList();
            }

            DataTable tbl = Convertor.ToDataTable(proList);
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
        ddlClientType.Enabled = yes;
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
        else if (!Helper.IsAmount(txtBasicSa.Text))
        {
            ErrorMessage = "Basic SA Range required as number.";
            return false;
        }
        else if (!Helper.IsAmount(txtBasicDisAmount.Text))
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
            ErrorMessage = string.Empty;
            return true;
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
                bl_micro_product_discount_config pro = new bl_micro_product_discount_config();
                pro.ProductID = ddlProduct.SelectedValue;
                pro.ProductRiderID = ddlRiderId.Items.Count > 0 ? ddlRiderId.SelectedValue : "";
                pro.BasicDiscountAmount = Convert.ToDouble(txtBasicDisAmount.Text);
                pro.BasicSumAssured = Convert.ToDouble(txtBasicSa.Text);
                pro.RiderDiscountAmount = txtRiderDisAmount.Text.Trim() != "" ? Convert.ToDouble(txtRiderDisAmount.Text) : 0;
                pro.RiderSumAssured = txtRiderSa.Text.Trim() != "" ? Convert.ToDouble(txtRiderSa.Text) : 0;
                pro.ChannelItemId = ddlChannelItem.SelectedValue;
                pro.EffectiveDate = Helper.FormatDateTime(txtEffectiveDate.Text);
                pro.ExpiryDate = Helper.FormatDateTime(txtExpiryDate.Text);
                pro.Remarks = txtRemarks.Text;
                pro.Status = ddlStatus.SelectedValue == "1" ? true : false;
                pro.ClientType = ddlClientType.SelectedValue;
                if (string.IsNullOrEmpty(ProductDiscountConfigID))
                {
                    pro.ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PRODUCT_DISCOUNT_CONFIG" }, { "FIELD", "ID" } });
                    pro.CreatedBy = LoginUserName;
                    pro.CreatedOn = DateTime.Now;
                    if (da_micro_product_config.DiscountConfig.Save(pro))
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
                    pro.ID = ProductDiscountConfigID;
                    pro.UpdatedBy = LoginUserName;
                    pro.UpdatedOn = DateTime.Now;
                    if (da_micro_product_config.DiscountConfig.Update(pro))
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
                Log.AddExceptionToLog("Error function [btnSave_Click(object sender, EventArgs e)] in page [frmProductDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
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
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lRiderId = (Label)r.FindControl("lblRiderId");
            Label lBasicSa = (Label)r.FindControl("lblBasicSa");
            Label lRiderSa=(Label)r.FindControl("lblRiderSa");
            Label lBasicDisAdmount = (Label)r.FindControl("lblBasicDisAmount");
            Label lRiderDisAmount = (Label)r.FindControl("lblRiderDisAmount");
            Label lEffectiveDate = (Label)r.FindControl("lblEffectiveDate");
            Label lExpiryDate = (Label)r.FindControl("lblExpiryDate");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lRemarks = (Label)r.FindControl("lblRemarks");
            Label lClientType = (Label)r.FindControl("lblClientType");

            ProductDiscountConfigID = lId.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlChannelItem, lChannelName.Text);
            ddlChannelItem_SelectedIndexChanged(null, null);
            Helper.SelectedDropDownListIndex("VALUE", ddlProduct, lProductId.Text);
            ddlProduct_SelectedIndexChanged(null, null);
            Helper.SelectedDropDownListIndex("VALUE", ddlRiderId, lRiderId.Text);
            Helper.SelectedDropDownListIndex("VALUE", ddlClientType, lClientType.Text);
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

        foreach (bl_micro_product_config pro in ProductConfig.Where(_ =>  _.ChannelItemId == ddlChannelItem.SelectedValue))
        {
            ddlProduct.Items.Add(new ListItem(pro.ProductIDRemarks, pro.Product_ID));
        }
        ddlProduct_SelectedIndexChanged(null, null);
    }
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRiderId.Items.Clear();
        foreach (bl_micro_product_config pro in ProductConfig.Where( _ => _.Product_ID == ddlProduct.SelectedValue && _.ChannelItemId == ddlChannelItem.SelectedValue))
        {
            ddlRiderId.Items.Add(new ListItem( da_micro_product_rider.GetMicroProductByProductID(pro.RiderProductID).PRODUCT_ID_REMARKS,  pro.RiderProductID));
        }

    }
}