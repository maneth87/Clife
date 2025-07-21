using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Products_02frmRiderSetup : System.Web.UI.Page
{
    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ProductMicroRiderId { get { return ViewState["_PRODUCT_ID"] + ""; } set { ViewState["_PRODUCT_ID"] = value; } }
    void LoadData()
    {
        try
        {
            string productName = txtProductNameSearch.Text.Trim();

            List<bl_micro_product_rider> proList = new List<bl_micro_product_rider>();
            if (productName != "")
            {
                proList = da_micro_product_rider.GetMicroProductList("%" + productName + "%");
            }
            else
            {
                proList = da_micro_product_rider.GetMicroProductList();
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
        if (type == 1)
        {
            EnableControl(false);
            LoadData();

        }
        else if (type == 2)
        {
            ClearText();
            EnableControl();
            btnDelete.Enabled = false;
            txtProductId.Focus();

        }
        else if (type == 3)
        {
            LoadData();
            ClearText();
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            txtProductId.Enabled = true;
        }
        else if (type == 4)
        {
            ClearText();
            EnableControl(false);
        }
        else if (type == 5)
        {
            ClearText();
            EnableControl();
            btnDelete.Enabled = false;
        }
        else if (type == 6)
        {
            LoadData();
        }
        else if (type == 7)
        {


        }
    }
    void EnableControl(bool yes = true)
    {
        txtProductId.Enabled = yes;
        txtEngTitle.Enabled = yes;
        txtEngAbbre.Enabled = yes;
        txtKhTitle.Enabled = yes;
        txtAgeMin.Enabled = yes;
        txtAgeMax.Enabled = yes;
        txtSaMin.Enabled = yes;
        txtSaMax.Enabled = yes;
        txtRemarks.Enabled = yes;
        btnSave.Enabled = yes;
        btnDelete.Enabled = yes;
        btnCancel.Enabled = yes;
    }
    void ClearText()
    {
        txtProductId.Text = "";
        txtEngTitle.Text = "";
        txtEngAbbre.Text = "";
        txtKhTitle.Text = "";
        txtAgeMin.Text = "";
        txtAgeMax.Text = "";
        txtSaMin.Text = "";
        txtSaMax.Text = "";
        txtRemarks.Text = "";
        ProductMicroRiderId = string.Empty;
    }

    bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtProductId.Text))
        {
            ErrorMessage = "Product ID is required.";
            return false;
        }

        else if (string.IsNullOrWhiteSpace(txtEngTitle.Text))
        {
            ErrorMessage = "Eng Title is required.";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(txtKhTitle.Text))
        {
            ErrorMessage = "Khmer Title is required.";
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
            Transaction(1);
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            string productId = txtProductId.Text;
            string engTitle = txtEngTitle.Text;
            string engAbbre = txtEngAbbre.Text;
            string khTitle = txtKhTitle.Text;
            int ageMin = Convert.ToInt32(txtAgeMin.Text);
            int ageMax = Convert.ToInt32(txtAgeMax.Text);
            int saMin = Convert.ToInt32(txtSaMin.Text);
            int saMax = Convert.ToInt32(txtSaMax.Text);
            string remarks = txtRemarks.Text;
            bool save = false;
            if (string.IsNullOrEmpty(ProductMicroRiderId))
            {
                save = da_micro_product_rider.Save(new bl_micro_product_rider()
                {

                    PRODUCT_MICRO_RIDER_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PRODUCT_RIDER" }, { "FIELD", "PRODUCT_MICRO_RIDER_ID" } }),
                    PRODUCT_ID = productId,
                    EN_TITLE = engTitle,
                    EN_ABBR = engAbbre,
                    KH_TITLE = khTitle,
                    AGE_MIN = ageMin,
                    AGE_MAX = ageMax,
                    SUM_ASSURE_MIN = saMin,
                    SUM_ASSURE_MIX = saMax,
                    REMARKS = remarks,
                    CREATED_BY = LoginUserName,
                    CREATED_ON = DateTime.Now
                });
                if (save)
                {
                    Helper.Alert(false, "Saved successfully.", lblError);
                    Transaction(3);
                }
                else
                {
                    Helper.Alert(false, "Saved fail.", lblError);
                }
            }
            else
            {
                save = da_micro_product_rider.Update(new bl_micro_product_rider()
                   {
                       PRODUCT_MICRO_RIDER_ID=ProductMicroRiderId,
                       PRODUCT_ID = productId,
                       EN_TITLE = engTitle,
                       EN_ABBR = engAbbre,
                       KH_TITLE = khTitle,
                       AGE_MIN = ageMin,
                       AGE_MAX = ageMax,
                       SUM_ASSURE_MIN = saMin,
                       SUM_ASSURE_MIX = saMax,
                       REMARKS = remarks,
                       UPDATED_BY = LoginUserName,
                       UPDATED_ON = DateTime.Now,
                       UPDATED_REMARKS=""
                   });
                if (save)
                {
                    Helper.Alert(false, "Updated successfully.", lblError);
                    Transaction(3);
                }
                else
                {
                    Helper.Alert(false, "Update fail.", lblError);
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Transaction(6);
    }
    protected void gvParam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvParam.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {

        try
        {
            Transaction(5);
            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lId = (Label)r.FindControl("lblId");
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lEngTitle = (Label)r.FindControl("lblEngTitle");
            Label lEngAbbre = (Label)r.FindControl("lblEngAbbr");
            Label lKhTitle = (Label)r.FindControl("lblKhTitle");

            Label lAgeMin = (Label)r.FindControl("lblAgeMin");
            Label lAgeMax = (Label)r.FindControl("lblAgeMax");
            Label lSaMin = (Label)r.FindControl("lblSaMin");
            Label lSaMax = (Label)r.FindControl("lblSaMax");
            Label lRemark = (Label)r.FindControl("lblRemarks");
            txtProductId.Text = lProductId.Text;

            txtEngTitle.Text = lEngTitle.Text;
            txtEngAbbre.Text = lEngAbbre.Text;
            txtKhTitle.Text = lKhTitle.Text;

            txtAgeMin.Text = lAgeMin.Text;
            txtAgeMax.Text = lAgeMax.Text;
            txtSaMin.Text = lSaMin.Text;
            txtSaMax.Text = lSaMax.Text;
            txtRemarks.Text = lRemark.Text;
            ProductMicroRiderId = lId.Text;//store for update condition

        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        Transaction(2);
    }
}