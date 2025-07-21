using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Security;
public partial class Reports_report_view : System.Web.UI.Page
{

    DataTable temp_sub_table = new DataTable("Premium_Detail");
     string app_register_id = "";
     string action = "";
     ReportViewer my_report_view;
     string user_id = "";
     string user_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
       // app_register_id = "3DF5D2EC-DABC-4DEA-9A9F-43FC96B28956";
        app_register_id = Request.Params["app_register_id"];
        action = Request.Params["action"];
        if(!Page.IsPostBack)
        {

            MembershipUser myUser = Membership.GetUser();
             user_id = myUser.ProviderUserKey.ToString();
             user_name = myUser.UserName;

            //ViewState["userId"] = user_id;
            //ViewState["userName"] = user_name;

            //if (app_register_id != null && app_register_id != "null" && app_register_id != "")
            if (app_register_id != null && app_register_id != "null")
            {
                string str = "";
                str=show_report();
                if (str!="")
                {
                   // Response.Write(str);
                    div_message.InnerText = str;
                  
                }

            }
            else
            {
                div_message.InnerText = "Nothing to dispay.";

            }
            

        }
        
    }

   private string  show_report()
    {
        string status = "";
        try
        {
            my_report_view = new ReportViewer();
            //parameters
            string maturity_date, effective_date, mode_of_payment, premium_due_date, plan_name;
            int policy_owner_age = 0;
            //reset report
            my_report_view.Reset();

            //datasource
            DataTable my_data = new DataTable("V_Data_Check_List_Personal_Detail");
            DataTable tbl_beneficiaries = new DataTable("Beneficiaries");
            DataSet myDataSet = new DataSet();
            bool isCreditLife = false;

            if (app_register_id.Trim() != "")
            {
                #region Check credit life
                bl_underwriting uw = da_underwriting.GetUnderwritingObject(app_register_id);
                if (uw.Product_ID != null)
                {
                    List<bl_group_master_product> mProuct = da_group_master_product.GetGroupMasterProductList(uw.Product_ID, "");
                    if (mProuct.Count > 0)
                    {
                        isCreditLife = true;
                        myDataSet = DataSetGenerator.GetDataSet("SP_Get_Data_Check_List_Credit_Life_By_App_Register_ID", new string[,] { { "@APP_REGISTER_ID", app_register_id } });
                    }
                }
                #endregion
                if (action != null && action != "")
                {
                    //add rider
                    my_data = DataSetGenerator.Get_Data_Soure("SP_Get_Data_Check_List_Add_Rider_By_App_Register_ID", new string[,] { { "@App_Register_ID", app_register_id } });

                }
                else
                {
                    if (isCreditLife)
                    {
                        my_data = myDataSet.Tables[0];
                    }
                    else
                    {
                        my_data = DataSetGenerator.Get_Data_Soure("SP_Get_Data_Check_List_By_App_Register_ID", new string[,] { { "@App_Register_ID", app_register_id } });
                        
                    }
                }

                
                if (isCreditLife)
                {
                    tbl_beneficiaries = myDataSet.Tables[1];

                }
                else
                {
                  
                        tbl_beneficiaries = DataSetGenerator.Get_Data_Soure("SP_Get_Data_Checking_List_Beneficiary_By_App_ID", new string[,] { { "@App_Register_ID", app_register_id } });
                   
                }
                if (my_data.Rows.Count == 0)
                {
                    //Flate rate product
                    myDataSet = DataSetGenerator.GetDataSet("SP_GENERATE_POLICY_FLAT_RATE_REPORT", new string[,] { { "@POLICY_ID", app_register_id }, { "@POLICY_STATUS_ID", "1" } });
                    my_data = myDataSet.Tables[0];
                    tbl_beneficiaries = myDataSet.Tables[1];
                }

            }

            maturity_date = "";
            effective_date = "";
            mode_of_payment = "";
            premium_due_date = "";
            plan_name = "";
            string app_number = "";

            if (my_data.Rows.Count > 0)
            {
                app_number = my_data.Rows[0]["App_Number"].ToString();
                int day, month;
                DataRow row = my_data.Rows[0];
                int pay_mode = -1;
                DateTime effective;
                int assure_year = 0;
                DateTime policy_owner_birth_date;
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "MM/dd/yyyy";
                dtfi.DateSeparator = "/";
              
                effective = Convert.ToDateTime(row["effective_date"].ToString(), dtfi);
                
                policy_owner_birth_date = Convert.ToDateTime(row["birth_date1"].ToString());
                pay_mode = Convert.ToInt32(row["pay_mode"].ToString().Trim());
                mode_of_payment = Helper.GetPaymentModeInKhmer(pay_mode);
                assure_year = Convert.ToInt32(row["assure_year"].ToString());
               
                //policy owner age

                effective_date = effective.ToShortDateString();

                if (action != null && action != "")
                {
                    //effective and assure year 
                    bl_underwriting bl = da_underwriting.GetUnderwritingObject(app_register_id);

                    policy_owner_age = Calculation.Culculate_Customer_Age(policy_owner_birth_date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), bl.Effective_Date);

                    maturity_date = bl.Effective_Date.AddYears(bl.Assure_Year).ToShortDateString();

                }
                else
                {
                    policy_owner_age = Calculation.Culculate_Customer_Age(policy_owner_birth_date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), effective);

                    maturity_date = effective.AddYears(assure_year).ToShortDateString(); 
                   

                }

                //Response.Write(effective + "   " + DateTime.Now.ToShortDateString());
                //return;

                day = effective.Day;
                month = effective.Month;


                //year = effective_date.Year;
                //maturity date = effective date + assured year
                if (pay_mode == 0)//single
                {
                    //premium_due_date = "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month);
                    premium_due_date = "-";
                    //pay_mode = "បង់តែម្តង";
                }
                else if (pay_mode == 1)// annually
                {
                    premium_due_date = "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month);
                    //pay_mode = "ប្រចាំឆ្នាំ";
                }
                else if (pay_mode == 2)//semi
                {
                    premium_due_date = "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 6);
                    //pay_mode = "ប្រចាំឆមាស";
                }
                else if (pay_mode == 3)//quarterly
                {
                    //premium_due_date = "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 3) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 6) + ", " + "ថ្ងៃ " + day + " " + da_poli1cy.GetMonthName(month + 9) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 12);
                    premium_due_date = "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 3) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 6) + ", " + "ថ្ងៃ " + day + " " + da_policy.GetMonthName(month + 9);
                    //pay_mode = "ប្រចាំត្រីមាស";
                }
                else if (pay_mode == 4)//monthly
                {
                    premium_due_date = "ថ្ងៃ " + day + " នៃខែនីមួយៗ";
                    //pay_mode = "ប្រចាំខែ";
                }

            }
            else
            {
                status = "Record Not Found.";
                return status;
            }


            #region parameters for subreport

            double system_sum_insure = 0.0;
            double rounded_amount = 0.0;
            double em_premium = 0.0;
            double system_premium = 0.0;
            double em_amount = 0.0;
            double user_premium = 0.0;
            double user_premium_discount = 0.0;
            int life_assure_year = 0;
            int life_pay_year = 0;

            string[,] para_sub = new string[,] { { "", "" } };
            DataTable tbl_sub_report = new DataTable();

            if (action != "" && action != null)
            {
                tbl_sub_report = DataSetGenerator.Get_Data_Soure("SP_Get_Data_Check_List_Premium_Detail_Add_Rider_By_App_Register_ID", new string[,] { { "@App_Register_ID", app_register_id } });
                             
            }
            else
            {
                if (isCreditLife)
                {
                    tbl_sub_report = myDataSet.Tables[2];
                }
                else
                {
                    tbl_sub_report = DataSetGenerator.Get_Data_Soure("SP_Get_Data_Check_List_Premium_Detail_By_App_Register_ID", new string[,] { { "@App_Register_ID", app_register_id } });
                    if (tbl_sub_report.Rows.Count == 0)
                    {
                        //Flate rate product
                        tbl_sub_report = myDataSet.Tables[2];
                    }
                }
            }

            temp_sub_table = tbl_sub_report.Clone();

            //loop to get record if rider_type is not basic
            foreach (DataRow row in tbl_sub_report.Rows)
            {
                string rider_type = "";
                rider_type = row["rider_type"].ToString();
                if (tbl_sub_report.Rows.Count == 1)
                {//has only basic life

                    //basic premium
                    system_sum_insure = Convert.ToDouble(row["system_sum_insure"].ToString());
                    rounded_amount = Convert.ToDouble(row["rounded_amount"].ToString());
                    em_premium = Convert.ToDouble(row["em_premium"].ToString());
                    system_premium = Convert.ToDouble(row["system_premium"].ToString());
                    em_amount = Convert.ToDouble(row["em_amount"].ToString());
                    user_premium = Convert.ToDouble(row["user_premium"].ToString());
                    user_premium_discount = Convert.ToDouble(row["user_premium_discount"].ToString());
                    life_pay_year = Convert.ToInt32(row["pay_year"].ToString());
                    life_assure_year = Convert.ToInt32(row["assure_year"].ToString());

                    //this case i reset row to zero
                    //this record i will be removed in report
                    temp_sub_table.ImportRow(row);
                    temp_sub_table.Rows[0]["system_sum_insure"] = 0;
                    temp_sub_table.Rows[0]["system_premium"] = 0;
                    temp_sub_table.Rows[0]["rounded_amount"] = 0;
                    temp_sub_table.Rows[0]["em_premium"] = 0;
                    temp_sub_table.Rows[0]["em_amount"] = 0;
                    temp_sub_table.Rows[0]["user_premium_discount"] = 0;
                    temp_sub_table.Rows[0]["assure_year"] = 0;
                    temp_sub_table.Rows[0]["pay_year"] = 0;


                }
                else
                {//has basic life and rider
                    if (rider_type != "Basic")
                    {

                        temp_sub_table.ImportRow(row);

                    }
                    else
                    {
                        //basic premium
                        system_sum_insure = Convert.ToDouble(row["system_sum_insure"].ToString());
                        rounded_amount = Convert.ToDouble(row["rounded_amount"].ToString());
                        em_premium = Convert.ToDouble(row["em_premium"].ToString());
                        system_premium = Convert.ToDouble(row["system_premium"].ToString());
                        em_amount = Convert.ToDouble(row["em_amount"].ToString());
                        user_premium = Convert.ToDouble(row["user_premium"].ToString());
                        user_premium_discount = Convert.ToDouble(row["user_premium_discount"].ToString());
                        life_pay_year = Convert.ToInt32(row["pay_year"].ToString());
                        life_assure_year = Convert.ToInt32(row["assure_year"].ToString());

                    }
                }

            }


            #endregion


            //report title
            //plan_name = "គំរោងសុវត្ថិភាពគ្រួសារខ្ញុំ";
            bl_product product = da_product.GetProductByProductID(tbl_sub_report.Rows[0]["product_id"].ToString());
            plan_name = product.Kh_Title;

            string str_report_title = "";
            if (action != "" && action != null)
            {
                str_report_title = "DATA CHECKING LIST (ADD RIDER)";
            }
            else
            {
                str_report_title = "DATA CHECKING LIST";
            }

            #region Maturity Value <by: Maneth, date: 07072017>
            double maturityValue = 0;
            bl_product_type proType = new bl_product_type();
            proType = da_product.GetProductTypeByProductID(product.Product_ID);

            if (proType.Product_Type.Trim().ToUpper() == "SAVINGS")
            {
                // percent_val base on coverage year
                //10 years = 110%
                //12 years = 112%
                //15 years = 115%
                double percent_val = 0.0;
                int assure_y = 0;
                assure_y = life_assure_year;
                if (assure_y == 10)
                {
                    percent_val = 1.1;//110%
                }
                else if (assure_y == 12)
                {
                    percent_val = 1.12;//112%
                }
                else if (assure_y == 15)
                {
                    percent_val = 1.15;//115%
                }
                maturityValue = rounded_amount * percent_val * assure_y; //rounded_amount = Math.Ceiling(original_amount)
                maturityValue = Math.Round(maturityValue, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                maturityValue = 0;
            }
            #endregion

            ReportDataSource ds_beneficiaries = new ReportDataSource("Beneficiaries", tbl_beneficiaries);

            ReportDataSource report_source = new ReportDataSource("Personal_Detail", my_data);
            my_report_view.LocalReport.DataSources.Clear();
            my_report_view.LocalReport.DataSources.Add(report_source);

            my_report_view.LocalReport.DataSources.Add(ds_beneficiaries);

            //report path
            //my_report_view.LocalReport.ReportPath = "Reports/Report.rdlc";
            my_report_view.LocalReport.ReportPath = Server.MapPath("data_check_list.rdlc");

            ReportParameter[] paras = new ReportParameter[] { 
        
            new ReportParameter("Report_Title", str_report_title),
            new ReportParameter("Plan_Name",plan_name),
            new ReportParameter("Effective_Date", effective_date),
            new ReportParameter("Maturity_Date",maturity_date),
            new ReportParameter("Premium_Due_Date",premium_due_date),
            new ReportParameter("Payment_Mode", mode_of_payment),
            //parameters for sub report
            new ReportParameter("system_sum_insure", system_sum_insure+""),
            new ReportParameter("rounded_amount",rounded_amount+""),
            new ReportParameter("em_premium",em_premium+""),
            new ReportParameter("system_premium",system_premium+""),
            new ReportParameter("em_amount",em_amount+""),
            new ReportParameter("user_premium",user_premium+""),
            new ReportParameter("user_premium_discount",user_premium_discount+""),
            //printed by and date
            //new ReportParameter("Printed_By", ViewState["userName"]+""),
             new ReportParameter("Printed_By", user_name),
            new ReportParameter("Printed_Date",DateTime.Now.ToShortDateString()),
            //policyowner age
            new ReportParameter("Policyowner_Age", policy_owner_age+""),
            //coverage and pay year
             new ReportParameter("assure_year", life_assure_year+""),
           new ReportParameter("pay_year", life_pay_year+""),
           new ReportParameter("App_Register_ID", app_register_id),
           new ReportParameter("App_Number", app_number),
           new ReportParameter("Maturity_Value",maturityValue+"")
        };

            my_report_view.LocalReport.SetParameters(paras);
            //paper size

            // Class subreport
            my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_Premium_Detail);


            //refresh

            my_report_view.LocalReport.Refresh();

            //export to pdf

            Report_Generator.ExportToPDF(this.Context, my_report_view, "Data_checking_list" + DateTime.Now.ToString("yyyyMMddhhmmss"), true);

       
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [show_report] in class [da_check_life_RP_new.aspx.cs], Dettail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
            status = "Report load fail. :(";
        }
        return status;
    }

    //sub report
    void Data_Check_List_Premium_Detail(object sender, SubreportProcessingEventArgs e)
    {
      //DataTable  tbl_sub_report = DataSetGenerator.Get_Data_Soure("SP_Get_Data_Check_List_Premium_Detail_By_App_Register_ID", new string[,] { { "@App_Register_ID", app_register_id } });
      ReportDataSource ds = new ReportDataSource("Premium_Detail", temp_sub_table);
        e.DataSources.Add(ds);

    }
}