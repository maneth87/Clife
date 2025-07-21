using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Products_04frmRiderRateSetup : System.Web.UI.Page
{
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ProductRateID { get { return ViewState["_PRODUCT_RATE_ID"] + ""; } set { ViewState["_PRODUCT_RATE_ID"] = value; } }

    void LoadData()
    {
        try
        {
            string productName = txtProductNameSearch.Text.Trim();

            List<bl_micro_product_rider_rate> rateList = new List<bl_micro_product_rider_rate>();

            if (productName != "")
            {

                rateList = da_micro_product_rider_rate.GetProductRate("%" + productName + "%");
            }
            else
            {
                rateList = da_micro_product_rider_rate.GetProductRate();
            }

            DataTable tbl = Convertor.ToDataTable(rateList);
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
        ddlProduct.Enabled = yes;
        ddlGender.Enabled = yes;
        ddlPaymode.Enabled = yes;
        txtAgeMin.Enabled = yes;
        txtAgeMax.Enabled = yes;
        txtSaMin.Enabled = yes;
        txtSaMax.Enabled = yes;
        txtRatePer.Enabled = yes;
        ddlRateType.Enabled = yes;
        txtRate.Enabled = yes;
        txtRemarks.Enabled = yes;
        btnSave.Enabled = yes;
        btnDelete.Enabled = yes;
        btnCancel.Enabled = yes;
    }
    void ClearText()
    {
        ddlProduct.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        ddlPaymode.SelectedIndex = 0;
        txtAgeMin.Text = "";
        txtAgeMax.Text = "";
        txtSaMin.Text = "";
        txtSaMax.Text = "";
        txtRemarks.Text = "";
        txtRatePer.Text = "";
        ddlRateType.SelectedIndex = 0;
        txtRate.Text = "";
        ProductRateID = string.Empty;
    }

    bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(ddlProduct.SelectedValue))
        {
            ErrorMessage = "Product ID is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlGender.SelectedValue))
        {
            ErrorMessage = "Gender is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlPaymode.SelectedValue))
        {
            ErrorMessage = "Paymode is required.";
            return false;
        }
        else if (!Helper.IsNumber(txtAgeMin.Text))
        {
            ErrorMessage = "Age Min is required as number.";
            return false;
        }
        else if (!Helper.IsNumber(txtAgeMax.Text))
        {
            ErrorMessage = "Age Max is required as number.";
            return false;
        }
        else if (!Helper.IsNumber(txtSaMin.Text))
        {
            ErrorMessage = "Sum Assure Min is required as number.";
            return false;
        }
        else if (!Helper.IsNumber(txtSaMax.Text))
        {
            ErrorMessage = "Sum Assure Max is required as number.";
            return false;
        }
        else if (!Helper.IsNumber(txtRatePer.Text))
        {
            ErrorMessage = "Rate Per is required as number.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ddlRateType.SelectedValue))
        {
            ErrorMessage = "Rate Type is required.";
            return false;
        }
        else if (!Helper.IsAmount(txtRate.Text))
        {
            ErrorMessage = "Rate is required.";
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
            /*bind payment mode*/
            List<bl_payment_mode> modeList = da_payment_mode.GetPaymentModeList();
            Options.Bind(ddlPaymode, modeList, bl_payment_mode.NAME.Mode, bl_payment_mode.NAME.Pay_Mode_ID, 0, "--- Select ---");
            /*Bind rider product*/
            List<bl_micro_product_rider> riderList = da_micro_product_rider.GetMicroProductList();
            Options.Bind(ddlProduct, riderList, bl_micro_product_rider.Name.PRODUCT_ID_REMARKS, bl_micro_product_rider.Name.PRODUCT_ID, 0, "--- Select ---");
            Transaction(1);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            bl_micro_product_rider_rate rate = new bl_micro_product_rider_rate();
            rate.PRODUCT_ID = ddlProduct.SelectedValue;
            rate.GENDER = Convert.ToInt32(ddlGender.SelectedValue);
            rate.PAY_MODE = Convert.ToInt32(ddlPaymode.SelectedValue);
            rate.AGE_MIN = Convert.ToInt32(txtAgeMin.Text);
            rate.AGE_MAX = Convert.ToInt32(txtAgeMax.Text);
            rate.SUM_ASSURE_START = Convert.ToDouble(txtSaMin.Text);
            rate.SUM_ASSURE_END = Convert.ToDouble(txtSaMax.Text);
            rate.RATE_PER = Convert.ToDouble(txtRatePer.Text);
            rate.RATE_TYPE = ddlRateType.SelectedValue;
            rate.RATE = Convert.ToDouble(txtRate.Text);
            rate.REMARKS = txtRemarks.Text;
            if (string.IsNullOrEmpty(ProductRateID))//save
            {
                
                rate.PRODUCT_RATE_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PRODUCT_RIDER_RATE" }, { "FIELD", "Product_Rate_ID" } });
                rate.CREATED_BY = LoginUserName;
                rate.CREATED_ON = DateTime.Now;
               
                if (da_micro_product_rider_rate.Save(rate))
                {
                    Helper.Alert(false, "Saved successfully", lblError);
                    Transaction(3);
                }
                else
                {
                    Helper.Alert(true, "Saved fail", lblError);
                }

            }
            else//update
            {
                rate.PRODUCT_RATE_ID = ProductRateID;
                rate.UPDATED_BY = LoginUserName;
                rate.UPDATED_ON = DateTime.Now;
                rate.UPDATED_REMARKS = "";
                if (da_micro_product_rider_rate.Update(rate))
                {
                    Helper.Alert(false, "Updated successfully", lblError);
                    Transaction(3);
                }
                else
                {
                    Helper.Alert(true, "Updated fail", lblError);
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
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lGender = (Label)r.FindControl("lblGender");
            Label lPayMode = (Label)r.FindControl("lblPayMode");
            Label lAgeMin = (Label)r.FindControl("lblAgeMin");
            Label lAgeMax = (Label)r.FindControl("lblAgeMax");
            Label lSaMin = (Label)r.FindControl("lblSaMin");
            Label lSaMax = (Label)r.FindControl("lblSaMax");
            Label lRatePer = (Label)r.FindControl("lblRatePer");
            Label lRateType = (Label)r.FindControl("lblRateType");
            Label lRate = (Label)r.FindControl("lblRate");
            Label lRemark = (Label)r.FindControl("lblRemarks");

            Helper.SelectedDropDownListIndex("VALUE", ddlProduct, lProductId.Text);
            Helper.SelectedDropDownListIndex("TEXT", ddlGender, lGender.Text);
            Helper.SelectedDropDownListIndex("TEXT", ddlPaymode, lPayMode.Text);
            txtAgeMin.Text = lAgeMin.Text;
            txtAgeMax.Text = lAgeMax.Text;
            txtSaMin.Text = lSaMin.Text;
            txtSaMax.Text = lSaMax.Text;
            txtRatePer.Text = lRatePer.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlRateType, lRateType.Text);
            txtRate.Text = lRate.Text;
            txtRemarks.Text = lRemark.Text;
            ProductRateID = lId.Text;//store for update condition

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
        Transaction(6);
    }
}