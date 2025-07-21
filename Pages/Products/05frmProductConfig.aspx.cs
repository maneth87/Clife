using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Products_05frmProductConfig : System.Web.UI.Page
{
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ProductConfigID { get { return ViewState["_PRODUCT_CONFIG_ID"] + ""; } set { ViewState["_PRODUCT_CONFIG_ID"] = value; } }

    void LoadData()
    {
        try
        {
            string productName = txtProductNameSearch.Text.Trim();

            List<bl_micro_product_config> proList = new List<bl_micro_product_config>();

            if (productName != "")
            {

                proList = da_micro_product_config.ProductConfig.GetProductMicroProductSO("%" + productName + "%");
            }
            else
            {
                proList = da_micro_product_config.ProductConfig.GetProductMicroProductSO();
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
        txtPayMode.Enabled = yes;
        txtBasicSaRange.Enabled = yes;
        txtRiderSaRange.Enabled = yes;
        ddlStatus.Enabled = yes;
        ddlAllowRefer.Enabled = yes;
        ddlBusinessType.Enabled = yes;
        txtMarketingName.Enabled = yes;
        txtRemarks.Enabled = yes;
        ddlRequiredRider.Enabled = yes;
        btnSave.Enabled = yes;
        btnDelete.Enabled = yes;
        btnCancel.Enabled = yes;
        ddlRequiredReferralId.Enabled = yes;
        txtCoverPeriod.Enabled = yes;
        txtPayPeriod.Enabled = yes;
    }
    void ClearText()
    {
        ddlChannelItem.SelectedIndex = 0;
        ddlProduct.SelectedIndex = 0;
        ddlRiderId.SelectedIndex = 0;
        txtPayMode.Text = "";
        txtBasicSaRange.Text = "";
        txtRiderSaRange.Text = "";
        txtMarketingName.Text = "";
        txtRemarks.Text = "";
        ddlAllowRefer.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlBusinessType.SelectedIndex = 0;
        ddlRequiredRider.SelectedIndex = 0;
        ProductConfigID = string.Empty;
        ddlRequiredReferralId.SelectedIndex = 0;
        txtCoverPeriod.Text = "";
        txtPayPeriod.Text = "";
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
        else if (!IsSumAssuredRange(txtBasicSaRange.Text))
        {
            ErrorMessage = "Basic SA Range required as number. Format [1000,2000]";
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
        else if (string.IsNullOrWhiteSpace(txtPayMode.Text))
        {
            ErrorMessage = "Paymode is required.";
            return false;
        }

        else if (string.IsNullOrWhiteSpace(ddlBusinessType.SelectedValue))
        {
            ErrorMessage = "Business Type is required as number.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlAllowRefer.SelectedValue))
        {
            ErrorMessage = "Allow Refer is required as number.";
            return false;
        }
        else if(string.IsNullOrWhiteSpace(txtCoverPeriod.Text.Trim()))
        {
            ErrorMessage = "Cover Period is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(txtPayPeriod.Text.Trim()))
        {
            ErrorMessage = "Pay Period is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
        {
            ErrorMessage = "Status is required as number.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(txtMarketingName.Text))
        {
            ErrorMessage = "Marketing Name is required.";
            return false;
        }
        else
        {
            ErrorMessage = string.Empty;
            return true;
        }
    }
    bool IsSumAssuredRange(string SaRange)
    {
        bool result = false;
        try
        {
          double[] d=  Array.ConvertAll(SaRange.Split(','), new Converter<string, double>(Double.Parse));
            result= true;
        }
        catch (Exception ex)
        {
            result = false;
            throw ex;
          
        }
        return result;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            /*bind payment mode*/
            List<bl_payment_mode> modeList = da_payment_mode.GetPaymentModeList();
            foreach (bl_payment_mode mode in modeList)
            {
                lblPayModeTip.Text += lblPayModeTip.Text == "" ? (mode.Pay_Mode_ID + " : " + mode.Mode ): " , " + (mode.Pay_Mode_ID + " : " + mode.Mode );
            }
            lblPayModeTip.Text = "{ " + lblPayModeTip.Text + " } Input format: 0,1,2";
           
            lblBasicSaRangeTip.Text = "Input format: 1000,2000,3000";
            lblRiderSaRangeTip.Text = "Input format: 10,20,30";

            lblCoverPeriodTip.Text = "{M:Month, Y:Year} [Single Type & Period = M:1] [Single Type & Multi-Period = M:1,2], [Mulit Type & Single Period = M:1;Y:1] [Mulit Type & Multi Period = M:1,2;Y:1,2]";
            lblPayPeriodTip.Text = "{M:Month, Y:Year} [Single Type & Period = M:1] [Single Type & Multi-Period = M:1,2], [Mulit Type & Single Period = M:1;Y:1] [Mulit Type & Multi Period = M:1,2;Y:1,2]";
            /*Bind channel item*/
            List<bl_channel_item> chList = da_channel.GetChannelItemList();
            Options.Bind(ddlChannelItem, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");

            /*Bind product baisc*/
            List<bl_product> proList = da_product.GetProductList();
            Options.Bind(ddlProduct, proList, bl_product.NAME.ProductRemarks, bl_product.NAME.Product_ID, 0, "--- Select ---");

            /*Bind rider product*/
            List<bl_micro_product_rider> riderList = da_micro_product_rider.GetMicroProductList();
            Options.Bind(ddlRiderId, riderList, bl_micro_product_rider.Name.PRODUCT_ID_REMARKS, bl_micro_product_rider.Name.PRODUCT_ID, 0, "--- Select ---");

            Options.Bind(ddlBusinessType, da_master_list.GetMasterList("BUSINESS_TYPE"), "DescEn", "Code", -1);

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
                bl_micro_product_config pro = new bl_micro_product_config();
                pro.ChannelItemId = ddlChannelItem.SelectedValue;
                pro.Product_ID = ddlProduct.SelectedValue;
                pro.RiderProductID = ddlRiderId.SelectedValue;
                pro.BasicSumAssuredRange = Array.ConvertAll(txtBasicSaRange.Text.Split(','), new Converter<string, double>(Double.Parse));
                pro.RiderSumAssuredRange = txtRiderSaRange.Text.Trim()=="" ? new double[]{0}:  Array.ConvertAll(txtRiderSaRange.Text.Split(','), new Converter<string, double>(Double.Parse));
                pro.PayMode = Array.ConvertAll(txtPayMode.Text.Split(','), new Converter<string, Int32>(Int32.Parse));
                pro.BusinessType = ddlBusinessType.SelectedValue;
                pro.AllowRefer = ddlAllowRefer.SelectedValue == "1" ? true : false;
                pro.Status = ddlStatus.SelectedValue == "1" ? true : false;
                pro.MarketingName = txtMarketingName.Text;
                pro.Remarks = txtRemarks.Text;
                pro.IsRequiredRider = ddlRequiredRider.SelectedValue == "1" ? true : false;
                pro.IsValidateReferralId = ddlRequiredReferralId.SelectedValue == "1" ? true : false;
                pro.CoverPeriodType = txtCoverPeriod.Text.Trim().Split(';').ToArray();
                pro.PayPeriodType = txtPayPeriod.Text.Trim().Split(';').ToArray();

                if (string.IsNullOrEmpty(ProductConfigID))//save
                {
                    pro.Id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PRODUCT_CONFIG" }, { "FIELD", "ID" } });
                    pro.CreatedBy = LoginUserName;
                    pro.CreatedOn = DateTime.Now;
                    if (da_micro_product_config.ProductConfig .Save(pro))
                    {
                        Helper.Alert(false, "Saved successfully.", lblError);
                        Transaction(3);
                    }
                    else
                    {
                        Helper.Alert(true, "Saved fail.", lblError);
                    }
                }
                else//update
                {
                    pro.Id = ProductConfigID;
                    pro.UpdatedBy = LoginUserName;
                    pro.UpdatedOn = DateTime.Now;
                    if (da_micro_product_config.ProductConfig.Update(pro))
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
                Helper.Alert(true, ex.Message, lblError);
                Log.AddExceptionToLog("Error function [btnSave_Click(object sender, EventArgs e)] in page [frmProductConfig], detail: " + ex.Message + "=>" + ex.StackTrace);
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
            Label lBasicSaRang=(Label)r.FindControl("lblBasicSaRange");
            Label lRiderId = (Label)r.FindControl("lblRiderId");
            Label lRiderSaRange = (Label)r.FindControl("lblRiderSaRange");
            Label lPayMode = (Label)r.FindControl("lblPayMode");
            Label lBusinessType = (Label)r.FindControl("lblBusinessType");
            Label lAllowRefer = (Label)r.FindControl("lblAllowRefer");
            Label lStatus = (Label)r.FindControl("lblStatus");
            Label lMarketingName = (Label)r.FindControl("lblMarketingName");
            Label lRemarks = (Label)r.FindControl("lblRemarks");
            Label lRequiredRider = (Label)r.FindControl("lblRequiredRider");
            Label lValidateReferralId = (Label)r.FindControl("lblValidateReferralId");
            Label lCoverPeroid = (Label)r.FindControl("lblCoverPeriod");
            Label lPayPeriod = (Label)r.FindControl("lblPayPeriod");

            ProductConfigID = lId.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlChannelItem, lChannelName.Text);
            Helper.SelectedDropDownListIndex("VALUE", ddlProduct, lProductId.Text);
            txtBasicSaRange.Text = lBasicSaRang.Text;
            Helper.SelectedDropDownListIndex("VALUE", ddlRiderId, lRiderId.Text);
            txtRiderSaRange.Text = lRiderSaRange.Text;
            txtPayMode.Text = lPayMode.Text;
            Helper.SelectedDropDownListIndex("VALUE", ddlBusinessType, lBusinessType.Text);
            Helper.SelectedDropDownListIndex("TEXT", ddlAllowRefer, lAllowRefer.Text);
            Helper.SelectedDropDownListIndex("TEXT", ddlStatus, lStatus.Text);
            Helper.SelectedDropDownListIndex("TEXT", ddlRequiredRider, lRequiredRider.Text);
            txtMarketingName.Text = lMarketingName.Text;
            txtRemarks.Text = lRemarks.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlRequiredReferralId, lValidateReferralId.Text);
            txtCoverPeriod.Text = lCoverPeroid.Text;
            txtPayPeriod.Text = lPayPeriod.Text;
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
    protected void ibtnAdd_Click1(object sender, EventArgs e)
    {
        Transaction(2);
    }
}