using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Micro_policy_return : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack){

            txtPolicyNumber.Text = "";

        }

    }
    protected void btnCalculateReturn_Click(object sender, EventArgs e)
    {

        DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
        dtfi2.ShortDatePattern = "dd/MM/yyyy";
        dtfi2.DateSeparator = "/";

        string policy_id = da_policy_micro.GetPolicyIDByNumber(txtPolicyNumber.Text.Trim());

        if(policy_id ==""){

            lblMessage.Text = "No micro policy number found.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;

        }else{

            lblMessage.Text = "";
        //Get policy micro

        bl_policy_micro_single_row_data policy_micro = new bl_policy_micro_single_row_data();

        policy_micro = da_policy_micro.GetPolicySingleRowData(policy_id);


        int use_days = Calculation.CalculateUsedDays(policy_micro.Effective_Date, Convert.ToDateTime(txtResignDate.Text.Trim(), dtfi2));

        double premium_return_Death = policy_micro.User_Premium - Math.Round(Convert.ToDouble((policy_micro.User_Premium * use_days) / 365 ) , 0 , MidpointRounding.AwayFromZero);
 
        //lblMessage.Text  = "Premium Return is $" + premium_return_Death.ToString();


        string row = "";
        string table = "<table border='1'  width='100%'>";

        dvContent.Controls.Add(new LiteralControl(table));

        row = "<tr role='row'>";

        row += "<th  style=' text-align: center;' >No</th>";
        row += "<th  style=' text-align: center;' >Barcode</th>";
        row += "<th  style=' text-align:center;' >Card no.</th>";
        row += "<th  style=' text-align:center;' >Policy no.</th>";
        row += "<th  style=' text-align:center;' >Insured's name.</th>";
        row += "<th  style=' text-align:center;' >Product</th>";
        row += "<th  style=' text-align:center;' >Effective Date</th>";
        row += "<th  style=' text-align:center;' >Expirty Date</th>";
        row += "<th  style=' text-align:center;' >Sum Insured (USD) </th>";
        row += "<th  style=' text-align:center;' >Premium paid(USD)</th>";
        row += "<th  style=' text-align:center;' >Cancellation Date</th>";
        row += "<th  style=' text-align:center;' >Days not Cover</th>";
        row += "<th  style=' text-align:center;' >Premium Return (USD)</th>";

        dvContent.Controls.Add(new LiteralControl(row));
        row += "</tr>";

        DateTime expire_date = policy_micro.Effective_Date.AddYears(1);
        expire_date = expire_date.AddDays(-1);

        row = "<tr role='row'>";
        row += "<td  style=' text-align: center;' >1</td>";
        row += "<td  style=' text-align: center;' >" + policy_micro.Barcode + "</td>";
        row += "<td  style=' text-align:center;' >" + policy_micro.Card_Number + "</td>";
        row += "<td  style=' text-align:center;' >" + policy_micro.Policy_Number + " </td>";
        row += "<td  style=' text-align:left;' >" + (policy_micro.Last_Name + " " + policy_micro.First_Name) + " </td>";
        row += "<td  style=' text-align:center;' >" + policy_micro.Product_ID + "</th>";
        row += "<td  style=' text-align:center;' >" + policy_micro.Effective_Date.ToString("dd/MM/yyyy") + "</td>";
        row += "<td  style=' text-align:center;' >" + expire_date.ToString("dd/MM/yyyy") + "</td>";
        row += "<td  style=' text-align:center;' >" + policy_micro.User_Sum_Insure + "</td>";
        row += "<td  style=' text-align:center;' >" + policy_micro.User_Premium.ToString() + "</td>";
        row += "<td  style=' text-align:center;' >"+ txtResignDate.Text +"</td>";
        row += "<td  style=' text-align:center;' >" + (365 - use_days) + "</td>";
        row += "<td  style=' text-align:center;' >"+ premium_return_Death +"</td>";

        dvContent.Controls.Add(new LiteralControl(row));

        row += "</tr>";

        dvContent.Controls.Add(new LiteralControl("</table>"));

        }


    }




}