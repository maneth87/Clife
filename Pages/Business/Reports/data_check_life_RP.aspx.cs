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
using System.Resources;
using System.Globalization;
using System.Net;
public partial class Pages_Business_Reports_data_check_life_RP : System.Web.UI.Page
{
    string app_register_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        app_register_id = Request.Params["app_register_id"];
        if (app_register_id != null && app_register_id != "null" && app_register_id != "")
        {
            Report_Properties report_property = new Report_Properties();
            DataTable main_table = new DataTable();
            ReportDocument rpt = new ReportDocument();
            string maturity_date, effective_date, mode_of_payment, premium_due_date;

            //load main report
            rpt.Load(Server.MapPath("data_check_list.rpt"));

            report_property.Report_Class = rpt;
            string[,] para1 = new[,] { { "@App_Register_ID", app_register_id } };

            //data source for main report
            main_table = report_property.Get_Data_Soure("SP_Get_Data_Check_List_By_App_Register_ID", para1);

            maturity_date = "";
            effective_date = "";
            mode_of_payment = "";
            premium_due_date = "";

            if (main_table.Rows.Count > 0)
            {
                int day, month;
                DataRow row = main_table.Rows[0];
                int pay_mode = -1;
                DateTime effective;
                int assure_year = 0;

                pay_mode = Convert.ToInt32(row["pay_mode"].ToString().Trim());
                mode_of_payment = Helper.GetPaymentModeInKhmer(pay_mode);
                assure_year = Convert.ToInt32(row["assure_year"].ToString());

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "MM/dd/yyyy";
                dtfi.DateSeparator = "/";

                effective = Convert.ToDateTime(row["effective_date"].ToString(), dtfi);

                effective_date = effective.ToShortDateString();
                maturity_date = effective.AddYears(assure_year).ToShortDateString();

                //Response.Write(effective + "   " + DateTime.Now.ToShortDateString());
                //return;

                day = effective.Day;
                month = effective.Month;


                //year = effective_date.Year;
                //maturity date = effective date + assured year
                if (pay_mode == 0)//single
                {
                    premium_due_date = day + " " + da_policy.GetMonthName(month);
                    //pay_mode = "បង់តែម្តង";
                }
                else if (pay_mode == 1)// annually
                {
                    premium_due_date = day + " " + da_policy.GetMonthName(month);
                    //pay_mode = "ប្រចាំឆ្នាំ";
                }
                else if (pay_mode == 2)//semi
                {
                    premium_due_date = day + " " + da_policy.GetMonthName(month) + ", " + day + " " + da_policy.GetMonthName(month + 6);
                    //pay_mode = "ប្រចាំឆមាស";
                }
                else if (pay_mode == 3)//quarterly
                {
                    premium_due_date = day + " " + da_policy.GetMonthName(month) + ", " + day + " " + da_policy.GetMonthName(month + 3) + ", " + day + " " + da_policy.GetMonthName(month + 6) + ", " + day + " " + da_policy.GetMonthName(month + 9) + ", " + day + " " + da_policy.GetMonthName(month + 12);
                    //pay_mode = "ប្រចាំត្រីមាស";
                }
                else if (pay_mode == 4)//monthly
                {
                    premium_due_date = day + " នៃខែនីមួយៗ";
                    //pay_mode = "ប្រចាំខែ";
                }

            }
            else
            {
                message.Style.Add("display", "block");
                message.InnerHtml = "Report not found.";
                return;
            }

            //Report.Report_Name = Server.MapPath("data_check_list.rpt");
            String[,] para = new string[,] { { "Report_Title", "DATA CHECK LIST" },
                                            {"Plan_Name","គំរោងសុវត្តិភាពគ្រួសារខ្ញុំ"}, 
                                            {"Effective_Date", effective_date},
                                            {"Maturity_Date",maturity_date},
                                            {"Mode_Of_Payment",mode_of_payment},
                                            {"Premium_Due_Date",premium_due_date}};



            DataTable temp_sub_table = new DataTable();
            DataTable sub_table = new DataTable();
            //data source for sub report premium detail
            sub_table = report_property.Get_Data_Soure("SP_Get_Data_Check_List_Premium_Detail_By_App_Register_ID", new string[,] { { "@App_Register_ID", app_register_id } });
            temp_sub_table = sub_table.Clone();

            string[,] para_sub = new string[,] { { "", "" } };

            //loop to get record if rider_type is not basic
            foreach (DataRow row in sub_table.Rows)
            {
                string rider_type = "";
                rider_type = row["rider_type"].ToString();
                if (rider_type != "Basic")
                {

                    temp_sub_table.ImportRow(row);

                }
                else
                {
                    //para_sub = new string[,] { { "system_sum_insure", row["system_sum_insure"].ToString() } };
                    para_sub = new string[,] { { "system_sum_insure", row["system_sum_insure"].ToString()},
                                            {"rounded_amount", row["rounded_amount"].ToString()},
                                            {"em_premium",row["em_premium"].ToString()},
                                            {"system_premium", row["system_premium"].ToString()},
                                            {"em_amount",row["em_amount"].ToString()},
                                            {"user_premium",row["user_premium"].ToString()},
                                            {"user_premium_discount",row["user_premium_discount"].ToString()}};
                }
            }



            report_property.Report_Class.SetDataSource(main_table);
            report_property.Report_Class.OpenSubreport("data_check_list_premium_detail.rpt").SetDataSource(temp_sub_table);

            report_property.InitialParameters(para);
            report_property.InitialParameters(para_sub, "data_check_list_premium_detail.rpt");

            ////view report in pdf
            //System.IO.BinaryReader reader = new System.IO.BinaryReader(report_property.Report_Class.ExportToStream(ExportFormatType.PortableDocFormat));
           
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/pdf";

            //Response.BinaryWrite(reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length)));
            //Response.Flush();
            //Response.Close();

        //    string strTime = DateTime.Now.ToString("yyyyMMddhhmmss");
        //    String path = "D:\\" + strTime ;
        //    ExportFormatType ftype;

        //    ftype = ExportFormatType.PortableDocFormat;
        //    switch (ftype)
        //    { 
        //        case ExportFormatType.PortableDocFormat:
        //            Response.ContentType = "application/pdf";
                   
        //            Response.Charset = System.Text.Encoding.UTF8.WebName;
    
        //            Response.ContentEncoding = System.Text.Encoding.UTF8;
        //            path = path + "report.pdf";
        //            break;
                    
        //        case ExportFormatType.Excel:
        //             Response.ContentType= "application/vnd.ms-excel";
        //             path = path + "report.xls";
        //            break;
                    
        //        case ExportFormatType.WordForWindows:

        //            Response.ContentType = "application/msword";
        //            path = path + "report.doc";
        //            break;

        //    }

         
        //report_property.Report_Class.ExportToDisk(ftype,path);
          

        //    WebClient client = new WebClient();
        //    byte[] buffer = client.DownloadData(path);
        //    string str = buffer.Length.ToString();


        //    Response.Charset = "UTF-8";
        //    Response.ContentEncoding.GetBytes(str);
          
        //Response.AddHeader("", str);

        //Response.BinaryWrite(buffer);
        

            CrystalReportViewer1.ReportSource = report_property.Report_Class;
        }
        else
        {
            message.Style.Add("display", "block");
            message.InnerHtml = "Report not found.";
        }
        

    }
   
}