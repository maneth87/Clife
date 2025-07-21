using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class policy_schedule : System.Web.UI.Page
{
    string policy_id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        policy_id = Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"]));

        if (!Page.IsPostBack)
        {
            ReportApplicationsByStatus();
        }
    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    protected void ReportApplicationsByStatus()
    {
        dvReportDetail.Style.Clear();

        DataTable report_policy_premium_history = da_report_sale_policy.GetPolicyHistoryPaymentByPolicyID(policy_id, txtFrom_PolicyYear.Text,txtTo_PolicyYear.Text);
        DataTable customer_info = da_report_sale_policy.GetCustomerInfoByPolicyID(policy_id);

        //Draw Header
        #region
       
        lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Policy Schedule</h1></div>";

        if (customer_info.Rows.Count > 0)
        {
            string strTable_cust = "";

            foreach (DataRow item in customer_info.Rows)
            {
                /////Draw Customer Info
                strTable_cust = "<br><table cellpadding=\"0\" cellspacing=\"0\"  border=\"0\">";
                strTable_cust += "<tr border=\"0\">";
                strTable_cust += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width:105px;\">" + "Insurance Plance" + "</td>";
                strTable_cust += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width:300px;\" ><b>" + ": " + item["Product"].ToString() + "</b></td>";
                strTable_cust += "</tr>";

                strTable_cust += "<tr border=\"0\">";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:105px;\" >" + "Policy No." + "</td>";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:300px;\" ><b>" + ": " + item["Policy_Number"].ToString() + "</b></td>";
                strTable_cust += "</tr>";

                strTable_cust += "<tr border=\"0\">";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:105px;\" >" + "Insured Name" + "</td>";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:300px;\" ><b>" + ": " + item["Customer"].ToString() + "</b></td>";
                strTable_cust += "</tr>";

                strTable_cust += "<tr border=\"0\">";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:105px;\" >" + "Sum Insured" + "</td>";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:300px;\" ><b>" + ": " + float.Parse(item["Sum_Insure"].ToString()).ToString("$#,###,###") + "</b></td>";
                strTable_cust += "</tr>";

                strTable_cust += "<tr border=\"0\">";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:105px;\" >" + "Effective Date" + "</td>";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:300px;\" ><b>" + ": " + DateTime.Parse(item["Effective_Date"].ToString()).ToString("dd-MMM-yyyy") + "</b></td>";
                strTable_cust += "</tr>";

                strTable_cust += "<tr border=\"0\">";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:105px;\" >" + "Mode of Pmt." + "</td>";
                strTable_cust += "<td  style=\"text-align: let; padding:5px 5px 5px 5px; width:105px;\" ><b>" + ": " + item["Mode"].ToString() + "</b></td>";
                strTable_cust += "</tr>";

                strTable_cust += "</table>";
                dvReportDetail.Controls.Add(new LiteralControl(strTable_cust));

                lblAgentName.Text ="Agent's name: "+ item["Full_Name"].ToString(); lblAgentPhone.Text ="Tel: " + item["Mobile_Phone1"].ToString();
            }
        }
        
        string strTable = "";

        //Draw Header
        
        strTable = "<br><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
        strTable += "<tr border=\"1\"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Prem. Due Date</th><th styple=\"text-align: center; \">Year/Times</th><th styple=\"text-align: center; \">Stand.Prem</th><th style=\"text-align: center;\">Discount</th><th styple=\"text-align: center; \">Stand.Prem. after Dis.</th><th styple=\"text-align: center; \">Extra Prem.</th><th styple=\"text-align: center; \">Total Prem.</th><th styple=\"text-align: center; \">Accumulated Prem. Paid</th><th styple=\"text-align: center; \">Remarks</th></tr>";

        dvReportDetail.Controls.Add(new LiteralControl(strTable));

        strTable = "";

        #endregion

        //Loop through sale app by status list
        #region
        float get_factor_value = da_report_sale_policy.GetFactorByPolicyID(policy_id);

        int i = 0, check_pay_year=1, get_pay_year = da_report_sale_policy.GetPayYearByPolicyID(policy_id);

        float discount_rate = 0; float accumulated_prem = 0;

        /// If there is processing search by Pay Year
        DateTime paid_last_due_date = DateTime.Now;
        int paid_last_pay_year = 0, paid_last_pay_lot=0;
        /// 

        foreach (DataRow item in report_policy_premium_history.Rows)
        {
            if (txtFrom_PolicyYear.Text != "" && txtTo_PolicyYear.Text != "")
            {
                paid_last_due_date = DateTime.Parse(item["Due_Date"].ToString());
                paid_last_pay_year = int.Parse(item["Prem_Year"].ToString());
                paid_last_pay_lot = int.Parse(item["Prem_Lot"].ToString());
            }

            //Draw Row
            #region

            accumulated_prem += float.Parse(item["Amount"].ToString());

            strTable += "<tr>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + (i + 1) + "</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + DateTime.Parse(item["Due_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Prem_Year"].ToString() + "/" + item["Prem_Lot"].ToString() + "</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + ((float.Parse(item["Amount"].ToString()) + float.Parse(item["Discount_Amount"].ToString())) - float.Parse(item["Prem_EM_Amount"].ToString())).ToString("c2") + "</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + discount_rate + "%" + "</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + (float.Parse(item["Amount"].ToString()) - (float.Parse(item["Prem_EM_Amount"].ToString()))).ToString("c2") + "</td>";

            strTable += "<td  style=\"text-align: center;  padding:5px 5px 5px 5px; width:0px;\"  >" + float.Parse(item["Prem_EM_Amount"].ToString()).ToString("c2") + "</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + float.Parse(item["Amount"].ToString()).ToString("c2") + "</td>";

            strTable += "<td  style=\"text-align: center;  padding:5px 5px 5px 5px; width:0px;\"  >" + accumulated_prem.ToString("c2") + "</td>";

            strTable += "<td  style=\"text-align: left;  padding:5px 5px 5px 5px; width:0px;\"  >" + "Paid" + "</td>";

            strTable += "</tr>";

            dvReportDetail.Controls.Add(new LiteralControl(strTable));

            strTable = "";

            i = i + 1;

            #endregion

        }//End loop payment history list

        // Get Last Prem Year, Prem Lot, Mode, Due_Date
        #region
        string last_prem_year_lot = da_report_sale_policy.GetLastPremYearLotPolicyID(policy_id);
        string[] year_time = last_prem_year_lot.Split('/');
        int last_pay_year =int.Parse(year_time[0].ToString()), last_prem_lot=int.Parse(year_time[1].ToString());

        /// Premium & Extra
        string prem_extra = da_report_sale_policy.GetPremiumAndExtra(policy_id);
        string[] amount_prem_extra = prem_extra.Split('/');
        float premium = float.Parse(amount_prem_extra[0].ToString()), extra = float.Parse(amount_prem_extra[1].ToString());

        // Mode Payment
        int last_pay_mode = da_report_sale_policy.GetLastPayMode(policy_id), basic_lot=da_report_sale_policy.CalculateBasicLotByMode(last_pay_mode);
        
        // Get Due Date
        TimeSpan end_time = new TimeSpan(00, 00, 00);
        DateTime last_due_date = da_report_sale_policy.GetLastDueByPolicy(policy_id);

        check_pay_year = da_report_sale_policy.CalculateNextPremYear(last_pay_mode, last_pay_year, last_prem_lot);

        if (check_pay_year != last_pay_year)
        {
            last_prem_lot = 1; //accumulated_prem = 0;
        }
        else { last_prem_lot =last_prem_lot + 1; }
        #endregion

        /// If Search by Prem Year
        
        if (txtFrom_PolicyYear.Text != "" && txtTo_PolicyYear.Text != "")
        {
            if (report_policy_premium_history.Rows.Count == 0)
            {
                check_pay_year = int.Parse(txtFrom_PolicyYear.Text);
                get_pay_year = int.Parse(txtTo_PolicyYear.Text);
                last_prem_lot = 1;

                ///get last due_date base on pay year
                DateTime effective_date = da_report_sale_policy.GetEffectiveDate(policy_id);
                int add_year = 0;

                if (int.Parse(txtFrom_PolicyYear.Text) <= 1) { add_year = 1; }
                else { add_year = int.Parse(txtFrom_PolicyYear.Text) - 1; }

                last_due_date = effective_date.AddYears(add_year);
                last_due_date = last_due_date.AddMonths(-1);
            }
            else
            {
                last_due_date = paid_last_due_date;

                check_pay_year = da_report_sale_policy.CalculateNextPremYear(last_pay_mode, paid_last_pay_year, paid_last_pay_lot);
                get_pay_year = int.Parse(txtTo_PolicyYear.Text);

                if (check_pay_year != last_pay_year)
                {
                    last_prem_lot = 1; //accumulated_prem = 0;
                }
                else { last_prem_lot = last_prem_lot + 1; }
                
            }
        }
        /// End of checking Prem Year

        /// Loop Pay Year
        while (check_pay_year <= get_pay_year)
        {
            while (last_prem_lot <= basic_lot)
            {
                DateTime next_due = da_report_sale_policy.GetNextDue(last_pay_mode, last_due_date, policy_id).Date + end_time;

                accumulated_prem += premium + extra;

                //Draw Row
                #region

                strTable += "<tr>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + (i + 1) + "</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + next_due.ToString("dd/MM/yyyy") + "</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + check_pay_year + "/" + last_prem_lot + "</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + premium.ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + discount_rate + "%" + "</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + (premium - 0).ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: center;  padding:5px 5px 5px 5px; width:0px;\"  >" + extra.ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + (premium + extra).ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: center;  padding:5px 5px 5px 5px; width:0px;\"  >" + accumulated_prem.ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: left;  padding:5px 5px 5px 5px; width:0px;\"  >" + " " + "</td>";

                strTable += "</tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";

                i = i + 1;

                #endregion

                last_prem_lot = last_prem_lot + 1; last_due_date = next_due;
            }

            //accumulated_prem = 0;

            last_prem_lot = 1;

            check_pay_year = check_pay_year + 1;
        }/// End of Looping Pay Year
       
        #endregion

        dvReportDetail.Style.Clear();

        strTable += "</table>";
        dvReportDetail.Controls.Add(new LiteralControl(strTable));
        strTable = "";
       
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ReportApplicationsByStatus();
    }
}