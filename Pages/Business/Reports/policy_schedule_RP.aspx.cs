using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;

using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Globalization;

public partial class Pages_Business_Reports_policy_schedule_RP : System.Web.UI.Page
{
    ReportDocument report = new ReportDocument();
    string policy_id = "";

    protected void Page_Load(object sender, EventArgs e)
    
    {
           
        try
        {
            policy_id = Request.QueryString["policy_id"];
            if (policy_id != null && policy_id != "null" && policy_id != "")
            {
                //WriteHtml(policy_id);
                //return;

                #region Load report
                string report_path = "";
                string pay_mode = "";
                string due_date = "";

                DataTable tbl = new DataTable();
                DataSet ds = new DataSet();
                ds = DataSetGenerator.GetDataSet("SP_GetPolicyScheduleByPolicyID", policy_id);

                //Check has record or not
                if (ds.Tables[0].Rows.Count == 0)
                {
                    message.InnerHtml = "Record(s) not found.";
                    //imgPrint.Visible = false;
                    return;
                }
                else 
                {
                   // imgPrint.Visible = true;
                    message.Style.Clear();
                    
                }

                //main report parth
                report_path = Server.MapPath("policy_nfp.rpt");
                //load report
                report.Load(report_path);

                //due_date​ and payment mode
                int int_pay_mode=-1;
                int_pay_mode = Convert.ToInt32(ds.Tables[0].Rows[0]["pay_mode"].ToString());
                int day = 0;
                int month = 0;
               
                //int year = 0;
                DateTime effective_date;

                effective_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["effective_date"].ToString());
                day = effective_date.Day;
                month = effective_date.Month;
                
                //year = effective_date.Year;

                if (int_pay_mode == 0)//single
                {
                    due_date = day + " " + da_policy.GetMonthName(month) ;
                    pay_mode = "បង់តែម្តង";
                }
                else if (int_pay_mode == 1)// annually
                {
                    due_date = day + " " + da_policy.GetMonthName(month);
                    pay_mode = "ប្រចាំឆ្នាំ";
                }
                else if (int_pay_mode == 2)//semi
                {
                    due_date = day + " " + da_policy.GetMonthName(month) + ", " + day + " " + da_policy.GetMonthName(month + 6);
                    pay_mode = "ប្រចាំឆមាស";
                }
                else if (int_pay_mode == 3)//quarterly
                {
                    due_date = day + " " + da_policy.GetMonthName(month) + ", " + day + " " + da_policy.GetMonthName(month + 3) + ", " + day + " " + da_policy.GetMonthName(month + 6) + ", " + day + " " + da_policy.GetMonthName(month + 9) + ", " + day + " " + da_policy.GetMonthName(month + 12);
                    pay_mode = "ប្រចាំត្រីមាស";
                }
                else if (int_pay_mode == 4)//monthly
                {
                    due_date = "ថ្ងៃទី " + day + " នៃខែនីមួយៗ";
                    pay_mode = "ប្រចាំខែ";
                }

               
               

                //sub report 
                DataSet sub_ds = new DataSet();
                sub_ds = DataSetGenerator.GetDataSet("SP_GetPolicyScheduleByPolicyID_Sub_Report_Beneficiary", policy_id);
                //if beneficiary more than 4 do not show in report but show in other attach list
                if (sub_ds.Tables[0].Rows.Count > 4)
                {
                    sub_ds.Tables[0].Rows.Clear();
                    DataRow newRow;
                    newRow = sub_ds.Tables[0].NewRow();
                    newRow["Full_Name"] = "Please see attach list";
                    //newRow["Relationship"] = "-";
                    //newRow["Percentage"] = "-";
                    //newRow["Benefit_Note"] = "-";
                    sub_ds.Tables[0].Rows.Add(newRow);

                }
                //for (int i = 1; i < 4; i++)
                //{
                //    DataRow newRow = sub_ds.Tables[0].NewRow();
                //    newRow["Full_Name"] = "Camlife insurance " + i;
                //    //newRow["Relationship"] = "-";
                //    //newRow["Percentage"] = "-";
                //    //newRow["Benefit_Note"] = "-";
                //    sub_ds.Tables[0].Rows.Add(newRow);
                //}

                report.OpenSubreport("policy_nfp_sub_benefit.rpt").SetDataSource(sub_ds.Tables[0]);
                //report.Subreports["policy_nfp_sub_benefit.rpt"].SetDataSource(sub_ds.Tables["Beneficiary"]);

                //sub report benefit and premium
                sub_ds = new DataSet();
                sub_ds = DataSetGenerator.GetDataSet("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", policy_id);

                //add black riders
                DataTable tblrider = sub_ds.Tables[0];
                int intkid = 0;
                int intspouse = 0;
                double total_premium = 0.0;
                double total_premium_rider = 0.0;
                double grand_total = 0.0;
                double basic_premium = 0.0;
                foreach (DataRow row in tblrider.Rows)
                {
                    int level = 0;
                    level = Convert.ToInt32(row["Level"].ToString().Trim());
                    basic_premium = Convert.ToDouble(row["Premium"].ToString()) + Convert.ToDouble(row["EM_Amount"].ToString()) - Convert.ToDouble(row["Discount_Amount"].ToString());
                    if (level == 1)
                    {
                        //premium for life insured
                        total_premium = total_premium + Convert.ToDouble(row["Premium_Rider"].ToString()) + Convert.ToDouble(row["EM_Amount_Rider"].ToString()) - Convert.ToDouble(row["Discount_Amount_Rider"].ToString());
                    }
                    if (level == 2)
                    {//spouse
                        intspouse = intspouse + 1;
                        //sum premium for rider
                        total_premium_rider = Convert.ToDouble(row["Premium_Rider"].ToString()) + Convert.ToDouble(row["EM_Amount_Rider"].ToString()) - Convert.ToDouble(row["Discount_Amount_Rider"].ToString());
                    }
                    else if (level > 2)
                    {
                        intkid = intkid + 1;
                        //sum premium for rider
                        total_premium_rider = total_premium_rider + Convert.ToDouble(row["Premium_Rider"].ToString()) + Convert.ToDouble(row["EM_Amount_Rider"].ToString()) - Convert.ToDouble(row["Discount_Amount_Rider"].ToString());
                    }

                }
                //grand total 
                grand_total = basic_premium + total_premium + total_premium_rider;

                //add blank spouse
                if (intspouse == 0)
                {
                    DataRow rowSpouse = sub_ds.Tables[0].NewRow();
                    rowSpouse["Rider_Type"] = "Spouse";
                    sub_ds.Tables[0].Rows.Add(rowSpouse);
                }

                //add blank kids
                //int kidno=0;
                //kidno = 4 - intkid;
                if (intkid < 4)
                {
                    intkid = intkid + 1;
                    for (int i = intkid; i <= 4; i++)
                    {
                        DataRow rowKid = sub_ds.Tables[0].NewRow();
                        rowKid["Rider_Type"] = "Kid " + i;
                        rowKid["Sum_Insure"] = 0;
                        rowKid["Rider_Sum_Insured"] = 0;
                        rowKid["Premium_Rider"] = 0;
                        rowKid["EM_Amount"] = 0;
                        rowKid["EM_Amount_Rider"] = 0;
                        rowKid["Level"] = i;
                        rowKid["Discount_Amount"] = 0;
                        rowKid["Discount_Amount_Rider"] = 0;

                        sub_ds.Tables[0].Rows.Add(rowKid);
                    }
                }

                report.OpenSubreport("policy_nfp_sub_benefit_premium.rpt").SetDataSource(sub_ds.Tables[0]);
               

                //assign dataset into report
                report.SetDataSource(ds.Tables[0]);
                //assign parameters into report
                report.SetParameterValue("Pay_Mode", pay_mode);

                report.SetParameterValue("Due_Date", due_date);
                //Get issue date
                string issue_date = "";
                issue_date = da_policy.GetIssueDate(policy_id).ToShortDateString() + "";
                string[] str;
                char a = '/';
                str = issue_date.Split(a);
                if (str.Length > 0)
                {
                    issue_date = str[1] + "-" + str[0] + "-" + str[2];
                }

                report.SetParameterValue("Issued_Date", "Issued Date: " + issue_date);

                //assign parameter into sub report
                report.SetParameterValue("Grand_Total", grand_total, "policy_nfp_sub_benefit_premium.rpt");
                
                CrystalReportViewer1.ReportSource = report;
                CrystalReportViewer1.Visible = true;


#region printing
               // PrinterSettings setting = new PrinterSettings();
                //PrintDialog dialog = new PrintDialog();
                //if (dialog.ShowDialog() == DialogResult.OK)
                //{
                //    setting = dialog.PrinterSettings;
                //}
                //setting.PrinterName = "Microsoft XPS Document Writer";
                //setting.PrinterName = "\\" + "\\192.168.1.81\\EPSON T60 Series";
               
                //report.PrintToPrinter(setting, new PageSettings() { }, false);

                ////view report in pdf
                //BinaryReader reader = new BinaryReader(report.ExportToStream(ExportFormatType.PortableDocFormat));
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.ContentType = "application/pdf";

                //Response.BinaryWrite(reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length)));
                //Response.Flush();
                //Response.Close();

               // WriteHtml(policy_id);
#endregion

                #endregion
            }
            else
            {
                message.InnerHtml = "Record(S) Not Found.";
                imgPrint.Visible = false;
            }
            

        }
        catch (Exception ex)
        {
            message.InnerHtml = ex.Message;
            Log.AddExceptionToLog("Error in page [policy_schedule_RP.aspx.cs], Detail: " + ex.Message);
        }
    }
       
   
    protected void imgPrint_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //printing
           // PrinterSettings setting = new PrinterSettings();

            //PrintDialog dialog = new PrintDialog();

            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    setting = dialog.PrinterSettings;
            //    report.PrintToPrinter(setting, new PageSettings() { }, false);
            //}

            //setting.PrinterName = "Microsoft XPS Document Writer";
            //setting.PrinterName = "\\" + "\\192.168.1.81\\EPSON T60 Series";
            //report.PrintToPrinter(setting, new PageSettings() { }, false);


            ////view report in pdf
            //BinaryReader reader = new BinaryReader(report.ExportToStream(ExportFormatType.PortableDocFormat));
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/pdf";

            //Response.BinaryWrite(reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length)));
            //Response.Flush();
            //Response.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Print Error in page [policy_shedule_RP.aspx.cs] class [imgPrint_Click], Detail: " + ex.Message);
        }
       
        
    }
    void WriteHtml(string policy_id)
    {
        string display = "";
        DataTable tbl = new DataTable();
        DataSet ds = new DataSet();
        DataSet ds_premium = new DataSet();
        DataSet ds_benefit = new DataSet();

        double basic_sum_insure = 0.0;
        double basic_premium = 0.0;
        double basic_extra_premium = 0.0;
        double basic_total_premium = 0.0;
        double grand_total_premium = 0.0;

        string issue_date = "";

        ds = DataSetGenerator.GetDataSet("SP_GetPolicyScheduleByPolicyID", policy_id);
        ds_premium = DataSetGenerator.GetDataSet("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", policy_id);
        ds_benefit = DataSetGenerator.GetDataSet("SP_GetPolicyScheduleByPolicyID_Sub_Report_Beneficiary", policy_id);

       
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            //get issue date
            issue_date = GetNewDateFormat(da_policy.GetIssueDate(policy_id));

            //Write Header
            display = display + "<table style='font-family:'Khmer OS Content'; font-size:8px; font-weight:bold; width:100%; border:1px solid;' border=1> " +
                                         "<tr>" +

                                             "<td class='RowDetail'>" +
                                                 "លេខបណ្ណសន្យារ៉ាប់រង Policy Number" +

                                             "</td>" +

                                             "<td class='RowDetail'>" +
                                                 row["policy_number"].ToString() +
                                             "</td>" +
                                             "<td rowspan='2' style='text-align:right;'><img src='../../../App_Themes/images/Logo.png' style='width:200px; height:100px;' /></td>" +
                                             
                                         "</tr>" +
                                         "<tr><td>តារាងបណ្ណសន្យារ៉ាប់រង Policy Schedule</td><td></td></tr>";
            //body personal detail
            //<tr><td></td><td></td><td></td></tr>
            display = display + "<tr><td  colspan='3' class='ColHeader'>ព័ត៌មានលំអិតអំពីម្ចាស់បណ្ណសន្យារ៉ាប់រង និង អ្នកត្រូវបានធានាអាយុជីវិត Policyowner and Life Assured Personal Details</td></tr>";
            display = display + "<tr><td></td><td class='ColHeaderSub'>ម្ចាស់បណ្ណសន្យារ៉ាប់រង Policyowner</td><td class='ColHeaderSub'>អ្នកត្រូវបានធានាអាយុជីវិត Life Assured</td></tr>";
            //Full Name
            display = display + "<tr><td class='RowDetail'>ឈ្មោះពេញ Full Name</td><td class='RowDetail'>" + row["Owner_Khmer_First_Name"].ToString() + " " + row["Owner_Khmer_Last_Name"].ToString() + "</td><td class='RowDetail'>" + row["Khmer_First_Name"].ToString() + " " + row["Khmer_Last_Name"].ToString() + "</td></tr>";
            //ID Card
            display = display + "<tr><td class='RowDetail'>លេខអត្តសញ្ញាណបណ្ណ/លិខិតឆ្លងដែន/ <br />សំបុត្រកំណើត ID./Passport/Birth Certificate No.</td><td class='RowDetail'>" + row["Owner_ID_Card"].ToString() + "</td><td class='RowDetail'>" + row["ID_Card"].ToString() + "</td></tr>";
            //Birth Date
            display = display + "<tr><td class='RowDetail'>ថ្ងៃ ខែ ឆ្នាំកំណើត Date of Birth</td><td class='RowDetail'>" + GetNewDateFormat(Convert.ToDateTime( row["Birth_Date1"].ToString())) + "</td><td class='RowDetail'>" + GetNewDateFormat(Convert.ToDateTime( row["Birth_Date"].ToString())) + "</td></tr>";
            //Age
            display = display + "<tr><td class='RowDetail'>អាយុ Age</td><td class='RowDetail'></td><td class='RowDetail'>" + row["Age_Insure"].ToString() + "</td></tr>";
            //Gender
            display = display + "<tr><td class='RowDetail'>ភេទ Sex</td><td class='RowDetail'>" + row["Owner_Gender"].ToString() + "</td><td class='RowDetail'>" + row["Gender"].ToString() + "</td></tr>";
            //Customer NO
            display = display + "<tr><td class='RowDetail'>លេខសម្គាល់អតិថិជន Customer No.</td><td class='RowDetail'></td><td class='RowDetail'>" + row["Customer_ID"].ToString() + "</td></tr>";
                   
            //body Policy detail
            display = display + "<tr><td colspan='3' class='ColHeader'>ព័ត៌មានលម្អិតអំពីម្ចាស់បណ្ណសន្យារ៉ាប់រងPolicy Details</td></tr>";

            //merch 3 columns and create new table with two columns
            display = display + "<tr><td colspan='3'>";
            display = display + "<table border=1 style='width:100%;'><tr><td class='RowDetail'>លេខបណ្ណសន្យារ៉ាប់រង Policy Number : " + row["policy_number"].ToString() + "</td><td class='RowDetail'>គម្រោងធានារ៉ាប់រង Insurance Plan : គម្រោងសុវត្ថិភាពគ្រួសារខ្ញុំ</td></tr>";
            display = display + "<tr><td class='RowDetail'>កាលបរិច្ឆេទមានសុពលភាព Effective : " + GetNewDateFormat(Convert.ToDateTime( row["effective_date"].ToString())) + "</td><td class='RowDetail'>កាលបរិច្ឆេទដល់កំណត់ Maturity Date : " + GetNewDateFormat(Convert.ToDateTime( row["maturity_date"].ToString())) + "</td></tr>";
            display = display + "<tr><td class='RowDetail'>រយៈពេលធានារ៉ាប់រង Coverage : " + row["assure_year"].ToString() +"</td><td class='RowDetail'>រយៈពេលបង់បុព្វលាភរ៉ាប់រង Premium Payment Period : " + row["pay_year"].ToString() + "</td></tr>";
            display = display + "<tr><td class='RowDetail'>របៀបបង់បុព្វលាភរ៉ាប់រង Mode of Payment : " +​​​ GetPaymentModeInKhmer(Convert.ToInt32( row["pay_mode"].ToString())) + "</td><td class='RowDetail'>កាលបរិច្ឆេទដល់កំណត់បង់បុព្វលាភរ៉ាប់រង Premium Due Date</td></tr>";
            //end of new table
            display = display + "</table>";
            //end merch column
            display = display + "</td></tr>";

        }

        //body Premium Detail
        display = display + "<tr><td colspan='3' class='ColHeader'>ព័តមានលម្អិតអំពីអត្ថប្រយោជន៍ និងបុព្វលាភរ៉ាប់រង Benefits and Premium Details</td>";
        //merch 3 columns and create new table with 5 columns <table><tr><td></td><td></td><td></td><td></td><td></td></tr></table>
        display = display + "<tr><td colspan='3'><table border=1 style='width:100%;'><tr><td rowspan='2'></td><td rowspan='2' class='ColHeaderSub'>ទឹកប្រាក់ធានារ៉ាប់រងសរុប/អត្ថប្រយោជន៍ (ដុល្លា) Sum Assured/Benefits (USD)</td><td colspan='3' class='ColHeaderSub'>បុព្វលាភរ៉ាប់រង (ដុល្លា) Premium (USD)</td></tr>";

        display = display + "<tr><td class='ColHeaderSub'>បុព្វលាភរ៉ាប់រងស្តង់ដា Standard Premium</td><td class='ColHeaderSub'>បុព្វលាភរ៉ាប់រងស្តង់បន្ថែម Extra Premium</td><td class='ColHeaderSub'>បុព្វលាភរ៉ាប់រងសរុប Total Premium</td></tr>";
        if (ds_premium.Tables[0].Rows.Count > 0)
        {
            //basic life
            basic_sum_insure = Convert.ToDouble(ds_premium.Tables[0].Rows[0]["sum_insure"].ToString());
            basic_premium = Convert.ToDouble(ds_premium.Tables[0].Rows[0]["premium"].ToString());
            basic_extra_premium = Convert.ToDouble(ds_premium.Tables[0].Rows[0]["em_amount"].ToString());
            basic_total_premium = basic_premium + basic_extra_premium;
             
            //sum total premium
            grand_total_premium = grand_total_premium + basic_total_premium;

            display = display + "<tr><td class='RowDetail'>បណ្ណសន្យារ៉ាប់រងអាយុជីវិតជាមូលដ្ឋាន (ដុល្លា) Basic Life Insurance Policy (USD)</td><td class='ColHeaderSub'>" + basic_sum_insure + "</td><td class='ColHeaderSub'>" + basic_premium + "</td><td class='ColHeaderSub'>" + basic_extra_premium + "</td><td class='ColHeaderSub'>" + basic_total_premium + "</td></tr>";
            
            foreach (DataRow row in ds_premium.Tables[0].Rows)
            {
               //rider detail
                string rider_type = "";
                string rider_type_desc = "";
                double rider_sum_insure = 0.0;
                double rider_premium = 0.0;
                double rider_extra_premium = 0.0;
                double rider_total_premium = 0.0;

                rider_sum_insure = Convert.ToDouble(row["rider_sum_insured"].ToString());
                rider_premium = Convert.ToDouble(row["premium_rider"].ToString());
                rider_extra_premium = Convert.ToDouble(row["em_amount_rider"].ToString());
                rider_total_premium = rider_premium + rider_extra_premium;

                grand_total_premium = grand_total_premium + rider_total_premium;

                rider_type = row["rider_type"].ToString().Trim();
                if (rider_type == "ADB") 
                {
                    rider_type_desc = "គ្រោះថ្នាក់ចៃដន្យ AD Rider";
                }
                else if (rider_type == "TPD")
                {
                    rider_type_desc = "ពិការភាពទាំងស្រុង និង អចិន្ត្រៃយ៍ TPD Rider";
                }
                else if (rider_type == "Spouse")
                {
                    rider_type_desc = "អត្ថប្រយោជន៍សម្រាប់ស្វាមី-ភរិយា Spouse Rider";
                }
                else if (rider_type == "Kid 1")
                {
                    rider_type_desc = "អត្ថប្រយោជន៍សម្រាប់កូនៗ (1) Kid Rider";
                }
                else if (rider_type == "Kid 2")
                {
                    rider_type_desc = "អត្ថប្រយោជន៍សម្រាប់កូនៗ (2) Kid Rider";
                }
                else if (rider_type == "Kid 3")
                {
                    rider_type_desc = "អត្ថប្រយោជន៍សម្រាប់កូនៗ (3) Kid Rider";
                }
                else if (rider_type == "Kid 4")
                {
                    rider_type_desc = "អត្ថប្រយោជន៍សម្រាប់កូនៗ (4) Kid Rider";
                }
                else 
                {
                    rider_type_desc = "-";
                }

                display = display + "<tr><td class='RowDetail'>" + rider_type_desc + "</td><td class='ColHeaderSub'>" + rider_sum_insure + "</td><td class='ColHeaderSub'>" + rider_premium + "</td><td class='ColHeaderSub'>" + rider_extra_premium + "</td><td class='ColHeaderSub'>" + rider_total_premium + "</td></tr>";
            
            }
            //display total premium on page
            display = display + "<tr><td class='RowDetail'>បុព្វលាភរ៉ាប់រងសរុប (ដុល្លា) Total Premium (USD)</td><td colspan='3'></td><td class='ColHeaderSub'>" + grand_total_premium + "</td></tr>";

            display = display + "<tr><td class='RowDetail'>អត្ថប្រយោជន៍ដល់កំណត់ (ដុល្លា) Maturity Benefit (USD)</td><td colspan='4' class='ColHeaderSub'>-</td></tr>";

        
        }

        //End merch 3 columns and create new table with 5 columns
        display = display + "</table></tr></td>";

        //body Beneficiary
        display = display + "<tr><td colspan='3' class='ColHeader'>អ្នកទទួលផល Beneficiary</td></tr>";
        //Merch 3 columns and create new beneficiary table with 4 columns <table><tr><td></td><td></td><td></td<td></td></tr></table>
        display = display + "<tr><td colspan='3'><table border=1 style='width:100%;'><tr><td class='ColHeaderSub'>ឈ្មោះពេញ Full Name</td><td class='ColHeaderSub'>ទំនាក់ទំនង Relation</td><td class='ColHeaderSub'>ភាគរយនៃអត្ថប្រយោជន៍(%) Share of Benefitit </td><td class='ColHeaderSub'>កំណត់សម្គាល់ Remarks</td></tr>";
        if (ds_benefit.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds_benefit.Tables[0].Rows)
            {
                //beneficiary detail
                string full_name = "";
                string relation = "";
                string share_benefit = "";
                string remarks = "";

                full_name = row["full_name"].ToString();
                relation = row["relationship"].ToString();
                share_benefit = row["percentage"].ToString();
                remarks = row["benefit_note"].ToString();

                display = display + "<tr><td class='ColHeaderSub'>" + full_name + "</td><td class='ColHeaderSub'>"+ relation +"</td><td class='ColHeaderSub'>" + share_benefit + "</td><td class='ColHeaderSub'>" + remarks + "</td></tr>";

            }
        }
       
        display = display + "</table></td></tr>";
        //End merch 3 columns and create new beneficiary table with 4 columns 

        //page footer
        display = display + "<tr><td colspan='3' class='RowDetail'>តារាងបណ្ណសន្យារ៉ាប់រងនេះត្រូវបានចេញជូនម្ចាស់បណ្ណសន្យារ៉ាប់រងស្ថិតនៅក្រោមបទប្បញ្ញត្តិទូទៅ និង លក្ខខណ្ឌដែលបានភ្ជាប់ជូន ។</td></tr>";
        display = display + "<tr><td colspan='3' class='RowDetail'>This Policy Schedule is issued to Policyowner under the General Provisions and Insurance Terms and Conditions attached hereto.</td></tr>";
        display = display + "<tr><td colspan='3' class='RowDetail'  style='text-align:right;'>កាលបរិចេទ្ឆចេញបណ្ណសន្យារ៉ាប់រង Issued Date: " + issue_date + "</td></tr>";

        //close main table tag
        display = display + "</table>";

        //Display on page
        message.Style.Clear();
        print.InnerHtml = display;
    }
    private string GetPaymentModeInKhmer(int pay_mode)
    {
        string pay_mode_khmer = "";
        try
        {
            if (pay_mode == 0)//single
            {
                pay_mode_khmer = "បង់តែម្តង";
               
            }
            else if (pay_mode == 1)//annually
            {
                pay_mode_khmer = "ប្រចាំឆ្នាំ";
                
            }
            else if (pay_mode == 2)//semi-annul
            {
                pay_mode_khmer = "ប្រចាំឆមាស";
               
            }
            else if (pay_mode == 3)//quarterly
            {
                pay_mode_khmer = "ប្រចាំត្រីមាស";
               
            }
            else if (pay_mode == 4)//monthly
            {
                pay_mode_khmer = "ប្រចាំខែ";
                
            }
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPaymentModeInKhmer] in page [policy_schedule_RP.aspx.cs], Detail: " + ex.Message);
        }
        return pay_mode_khmer;
    }
    private string GetNewDateFormat(DateTime date)
    {
        //format date to dd-mm-yyyy
        string date_formated = "";
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";
        date = Convert.ToDateTime(date, dtfi);

        try
        {
            string[] str;
            char a = '/';
            str = date.ToShortDateString().Split(a);
            if (str.Length > 0)
            {
                date_formated = str[1] + "-" + str[0] + "-" + str[2];
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetNewDateFormat] in page [policy_schedule_RP.aspx.cs], Datail: " + ex.Message);
        }
        return date_formated;
    }
}