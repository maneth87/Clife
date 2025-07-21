using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Reports_micro_banca_chart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
  lblError.Text = "";
        if (!Page.IsPostBack)
        {
          
            if (!Helper.BindChannel(ddlChannel))
            {
             
                Helper.Alert(true, "Bind channel error", lblError);
            }
            if (ddlChannel.Items.Count > 0)
            {
                ddlChannel.SelectedIndex = 2;
                ddlChannel_SelectedIndexChanged(null, null);
                ddlChannelItem.SelectedIndex = 1;
              
               
            }

          //// all chart types
          //  foreach (int chartType in Enum.GetValues(typeof(SeriesChartType)))
          //  {
          //      ddlChartType.Items.Add(new ListItem(Enum.GetName(typeof(SeriesChartType), chartType), chartType.ToString()));
          //  }
            ddlChartType.Items.Add(new ListItem("Column", "10"));
            ddlChartType.Items.Add(new ListItem("Line", "3"));
        }
       
    }
    protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart1.Series[0].ChartType = (SeriesChartType) Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedValue);
        btnShowChart_Click(null, null);
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    
    protected void btnShowChart_Click(object sender, EventArgs e)
    {
        if (!Helper.IsDate(txtIssuedDateFrom.Text.Trim()))
        {
            Helper.Alert(true, "Issued Date From is invalid format. The expected format is DD-MM-YYYY", lblError);
        }
        else if (!Helper.IsDate(txtIssuedDateTo.Text.Trim()))
        {
            Helper.Alert(true, "Issued Date To is invalid format. The expected format is DD-MM-YYYY", lblError);
        }
        else//is valid display data
        {

            Chart1.Titles.Add("Sale Performance by " +  (ddlChartValue.SelectedIndex == 0 ? "amount" : "policy") + System.Environment.NewLine + "Report Date: " + txtIssuedDateFrom.Text.Trim () + " - " + txtIssuedDateTo.Text.Trim());

            DataTable tbl = da_micro_policy.GetPolicyInsuranceSummaryReport(Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim()), Helper.FormatDateTime(txtIssuedDateTo.Text.Trim()), ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, "");
            DataTable tbl1 = new DataTable();
            tbl1.Columns.Add("Office_Code");
            tbl1.Columns.Add("Total_Policy");
            DataRow r;
            int totalPolicy = tbl.AsEnumerable().Sum(rr => rr.Field<int>("total_policy"));
            decimal totalAmountAfterDis = tbl.AsEnumerable().Sum(rr => rr.Field<decimal>("total_amount_after_discount"));
            Series series = Chart1.Series[0];
            Chart1.ChartAreas[0].AxisX.Title = "Branch Code";
            Chart1.ChartAreas[0].AxisY.Title = ddlChartValue.SelectedIndex == 0 ? "Total Amount = $"+ totalAmountAfterDis.ToString("N") : "Total Policy = "+ totalPolicy;

            Chart1.ChartAreas[0].AxisX.Interval = 1;

            Chart1.Series[0].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedValue);
            //Chart1.ChartAreas[0].Area3DStyle.Enable3D = true;//enable 3d
            //   int i = 0;

            
            foreach (DataRow row in tbl.Rows)
            {
                r = tbl1.NewRow();
                r["office_code"] = row["office_code"];
                //r["total_policy"]=row["total_policy"];
                r["total_policy"] = ddlChartValue.SelectedIndex == 0 ? row["total_amount_after_discount"] : row["total_policy"];
                tbl1.Rows.Add(r);
                //i += 1;
                //if (i == 10)
                //{
                //    break;
                //}

            }
            series.XValueMember = "office_code";
            series.YValueMembers = "total_policy";
            Chart1.DataSource = tbl1;
            Chart1.DataBind();

            // Chart1.DataSource = da_micro_policy.GetPolicyInsuranceSummaryReport(new DateTime(2022, 4, 1), new DateTime(2022, 4, 10), "", "", "");
        }
    }
    protected void ddlChartValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnShowChart_Click(null, null);
    }
}