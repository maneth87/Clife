using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
public partial class Pages_Policy_calc_policy_lap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnExport.Visible = false;
    }
    protected void ibtnCalc_Click(object sender, ImageClickEventArgs e)
    {
        if (txtPolicyNumber.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Policy Number is required.');", true);
            return;
        }
        if (txtPayDate.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Pay Date is required.');", true);

        }
        else
        { 
            if(Helper.IsDate(txtPayDate.Text.Trim()))
            {
                CalculateLAP();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Pay Date is incorrect format.');", true);

            }
        }
      
    }
    protected void ibtnClear_Click(object sender, ImageClickEventArgs e)
    {
        txtPolicyNumber.Text = "";
        txtPayDate.Text = "";
        txtPolicyStatus.Text = "";
        txtPremium.Text = "";
        txtInterest.Text = "";
        txtTotalAmount.Text = "";
        gv_data.DataSource = null;
        gv_data.DataBind();
    }

    void CalculateLAP()
    {
        string policy_num = txtPolicyNumber.Text.Trim();

        string policy_formate_from = "00000000";

        if (policy_num != "")
        {
            int length_policy_num_from = policy_num.Length;
            /// From Policy Num

            if (policy_num.Length < 8)
            {
                policy_formate_from = policy_formate_from.Remove(0, length_policy_num_from);
                policy_num = policy_formate_from + policy_num;
            }
        }

        DataTable dt_status = da_policy_prem_pay.GetPolicyStatusPayMode(policy_num); 
      
        DataTable dt = new DataTable();

        if (dt_status.Rows.Count > 0)
        {

            dt = da_policy_prem_pay.GetPolicy_Lapsed(policy_num, "", "", Helper.FormatDateTime(txtPayDate.Text.Trim()));

            if (dt.Rows.Count > 0)
            {
                txtPolicyStatus.Text = dt_status.Rows[0]["Policy_Status_Type_ID"].ToString();
            }

        }

        decimal total_prem = 0, total_interest = 0; int duration_days = 0, duration_month = 0;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {

                total_interest = total_interest + decimal.Parse(item["Interest"].ToString());
                total_prem += decimal.Parse(item["Amount"].ToString());

                duration_days += int.Parse(item["duration_day"].ToString());
                duration_month += int.Parse(item["month"].ToString());
                

            }/// End of loop
             /// 
            gv_data.DataSource = null;
            gv_data.DataBind();

            gv_data.DataSource = dt;
            gv_data.DataBind();


            if (total_interest >= 1)
            {
                // > 1 && >=.5 , then round up, else round down
                total_interest = Math.Round(total_interest, 0, MidpointRounding.AwayFromZero);
            }
            else { total_interest = 0; }

            txtPremium.Text = total_prem.ToString();
            txtInterest.Text = total_interest.ToString();
            txtTotalAmount.Text = (total_prem + total_interest).ToString();

            btnExport.Visible = true;
        }
        else {
            gv_data.DataSource = null;
            gv_data.DataBind();
            txtInterest.Text = "";
            txtPremium.Text = "";
            txtTotalAmount.Text = "";
            btnExport.Visible = false;

        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();

        Response.Clear();
        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
        Helper.excel.Sheet = sheet1;
        Helper.excel.HeaderText = new string[] {"No.", "Policy_Number","Full_Name", "Product","Year/Time","PaymentMode", "Sum_Insured","Amount","Due_Date", "Interest", "Days", "Months" };
        Helper.excel.generateHeader();
        //disign rows
        int row_no = 0;
        foreach (GridViewRow row in gv_data.Rows)
        {
            #region //Variable
            Label no = (Label)row.FindControl("lblNo");
            Label pol = (Label)row.FindControl("lblPolicyNumber");
            Label full_name = (Label)row.FindControl("lblName");
            Label product = (Label)row.FindControl("lblProduct");
            Label yeartime = (Label)row.FindControl("lblYearTime");
            Label paymentmode = (Label)row.FindControl("lblPaymentMode");
            Label suminsured = (Label)row.FindControl("lblSumInsured");
            Label amount = (Label)row.FindControl("lblAmount");
            Label duedate = (Label)row.FindControl("lblDueDate");
            Label interest = (Label)row.FindControl("lblInterest");
            Label days = (Label)row.FindControl("lblDays");
            Label months = (Label)row.FindControl("lblMonths");
          
            #endregion
            row_no += 1;
            HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);


            HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(0);
            Cell2.SetCellValue(no.Text);

            HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(1);
            Cell3.SetCellValue(pol.Text);

            HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(2);
            Cell4.SetCellValue(full_name.Text);

            HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(3);
            Cell5.SetCellValue(product.Text);

            HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(4);
            Cell6.SetCellValue(yeartime.Text);

            HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(5);
            Cell7.SetCellValue(paymentmode.Text);

            HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(6);
            Cell8.SetCellValue(suminsured.Text);

            HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(7);
            Cell9.SetCellValue(amount.Text);

            HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(8);
            Cell10.SetCellValue(duedate.Text);

            HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(9);
            Cell11.SetCellValue(interest.Text);

            HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(10);
            Cell12.SetCellValue(days.Text);

            HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(11);
            Cell13.SetCellValue(months.Text);


        }

        string filename = "policy_lap" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        System.IO.MemoryStream file = new System.IO.MemoryStream();
        hssfworkbook.Write(file);

        Response.BinaryWrite(file.GetBuffer());

        Response.End();
    }
}