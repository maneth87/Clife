using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Globalization;

public partial class Pages_Chart_policy_complete_chart : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Get values from source page public property & Set the value to hidden fields
            hdfToDate.Value = PreviousPage.To_Date.ToString();
            hdfFromDate.Value = PreviousPage.From_Date.ToString();
            hdfCheckFormLoadOrSearch.Value = PreviousPage.Check_Form_Load_Or_Search;
            hdfProduct.Value = PreviousPage.Product;
            hdfPolicyNumber.Value = PreviousPage.Policy_Number;
            hdfPolicyStatus.Value = PreviousPage.Status_Code;
            hdfOrderBy.Value = PreviousPage.Order_By;
            hdfChartType.Value = PreviousPage.Chart_Type;
            hdfChartData.Value = PreviousPage.Chart_Data;
        }
    }    

}