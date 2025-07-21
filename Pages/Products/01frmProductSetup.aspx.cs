using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Products_01frmProductSetup : System.Web.UI.Page
{

    private string ErrorMessage { get { return ViewState["_ERROR_MESSAGE"] + ""; } set { ViewState["_ERROR_MESSAGE"] = value; } }
    private string LoginUserName { get { return System.Web.Security.Membership.GetUser().UserName; } }
    private string ProductID { get { return ViewState["_PRODUCT_ID"] + ""; } set { ViewState["_PRODUCT_ID"] = value; } }
    void LoadData()
    {
        try
        {
            string productName = txtProductNameSearch.Text.Trim();
   
            List<bl_product> proList = new List<bl_product>();
         if (productName != "")
         {
             proList = da_product.GetProductList("%"+productName+"%");
         }
         else {
             proList = da_product.GetProductList();
         }

         DataTable tbl = Convertor.ToDataTable(proList);
         gvParam.DataSource = tbl;
         gvParam.DataBind();
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Load data error, please contact your system administrator.", lblError);
            Log.AddExceptionToLog("Error function [LoadData()] in page [frmProductSetup], detail: " + ex.Message );
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
            txtProductId.Enabled = false;
            btnDelete.Enabled = false;
        }
        else if(type==6)
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
        ddlProductType.Enabled = yes;
        txtEngTitle.Enabled = yes;
        txtEngAbbre.Enabled = yes;
        txtKhTitle.Enabled = yes;
        txtKhAbbre.Enabled = yes;
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
        ddlProductType.SelectedIndex = 0;
        txtEngTitle.Text = "";
        txtEngAbbre.Text = "";
        txtKhTitle.Text = "";
        txtKhAbbre.Text = "";
        txtAgeMin.Text = "";
        txtAgeMax.Text = "";
        txtSaMin.Text = "";
        txtSaMax.Text = "";
        txtRemarks.Text = "";
        ProductID = string.Empty;
    }

    bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtProductId.Text))
        {
            ErrorMessage = "Product ID is required.";
            return false;
        }
        else if (ddlProductType.SelectedIndex == 0)
        {
            ErrorMessage = "Product Type is required.";
            return false;
        }
        else if(string.IsNullOrWhiteSpace(txtEngTitle.Text))
        {
            ErrorMessage="Eng Title is required.";
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
            List<bl_product_type> proList = da_product.getAllProductType();
            Options.Bind(ddlProductType, proList, "Product_Type", "Product_Type_ID", 0, "--- SELECT ---");
            Transaction(1);
           
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
      

        if (ValidateForm())
        {
            string productId = txtProductId.Text;
            int productType = Convert.ToInt32(ddlProductType.SelectedValue);
            string planType = ddlProductType.SelectedItem.Text;
            string engTitle = txtEngTitle.Text;
            string engAbbre = txtEngAbbre.Text;
            string khTitle = txtKhTitle.Text;
            string khAbbre = txtKhAbbre.Text;
            int ageMin = Convert.ToInt32(txtAgeMin.Text);
            int ageMax = Convert.ToInt32(txtAgeMax.Text);
            double saMin = Convert.ToDouble(txtSaMin.Text);
            double saMax = Convert.ToDouble(txtSaMax.Text);
            string remarks = txtRemarks.Text;
            if (string.IsNullOrEmpty(ProductID))
            {
                bool save = da_product.Save(productId, planType, engTitle, engAbbre, khTitle, khAbbre, ageMin, ageMax, saMin, saMax, productType, remarks);
                if (save)
                {
                    Helper.Alert(false, "Saved new product successfully.", lblError);
                    Transaction(3);
                }
                else
                {
                    Helper.Alert(true, "Saved new product fail.", lblError);
                }
            }
            else
            {
                bool save = da_product.Update(productId, planType, engTitle, engAbbre, khTitle, khAbbre, ageMin, ageMax, saMin, saMax, productType, remarks,LoginUserName,DateTime.Now,"");
                if (save)
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
        else {
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
    protected void gvParam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Transaction(5);
            GridViewRow r = gvParam.Rows[e.NewEditIndex];
            Label lProductId = (Label)r.FindControl("lblProductId");
            Label lPlanType = (Label)r.FindControl("lblPlanType");
            Label lEngTitle = (Label)r.FindControl("lblEngTitle");
            Label lEngAbbre = (Label)r.FindControl("lblEngAbbre");
            Label lKhTitle = (Label)r.FindControl("lblKhTitle");
            Label lKhAbbre = (Label)r.FindControl("lblKhAbbre");
            Label lAgeMin = (Label)r.FindControl("lblAgeMin");
            Label lAgeMax = (Label)r.FindControl("lblAgeMax");
            Label lSaMin = (Label)r.FindControl("lblSaMin");
            Label lSaMax = (Label)r.FindControl("lblSaMax");
            Label lRemark = (Label)r.FindControl("lblRemarks");
            txtProductId.Text = lProductId.Text;
            Helper.SelectedDropDownListIndex("TEXT", ddlProductType, lPlanType.Text);
            txtEngTitle.Text = lEngTitle.Text;
            txtEngAbbre.Text = lEngAbbre.Text;
            txtKhTitle.Text = lKhTitle.Text;
            txtKhAbbre.Text = lKhAbbre.Text;
            txtAgeMin.Text = lAgeMin.Text;
            txtAgeMax.Text = lAgeMax.Text;
            txtSaMin.Text = lSaMin.Text;
            txtSaMax.Text = lSaMax.Text;
            txtRemarks.Text = lRemark.Text;
            ProductID = lProductId.Text;//store for update condition
           
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