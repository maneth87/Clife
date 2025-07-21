using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for report_application
/// </summary>
public class da_report_application
{
    private static da_report_application mytitle = null;
    public da_report_application()
	{
	  if (mytitle == null)
        {
            mytitle = new da_report_application();
        }
	}

    public static List<bl_report_application> GetAppReportList(DateTime from_date, DateTime to_date, string check_status_code, int order_by)
    {
        List<bl_report_application> application_report_list = new List<bl_report_application>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            string sql = "", Order_by = "";

            if (check_status_code == "")
            {//convert(varchar(10),dbo.Ct_App_Info.App_Date,103)
                sql = @"SELECT        dbo.Ct_App_Register.App_Number AS [App.no.], 
                         CASE WHEN Ct_App_Info_Person.Gender = 0 THEN 'Ms. ' + Ct_App_Info_Person.Last_Name + ' ' + Ct_App_Info_Person.First_Name ELSE 'Mr. ' + Ct_App_Info_Person.Last_Name
                          + ' '+ Ct_App_Info_Person.First_Name END AS [Applicant name],
						   CONVERT(varchar(10), dbo.Ct_App_Info.App_Date, 103) AS [Reg.date], 
                         dbo.Ct_App_Life_Product.User_Sum_Insure AS [S.A.], dbo.Ct_App_Life_Product.User_Premium AS Premium, dbo.Ct_Product.En_Abbr AS [Plan], 
                         dbo.Ct_Underwrite_Table.Status_Code AS Status, CONVERT(varchar(10), dbo.Ct_Underwriting.Updated_On, 103) AS [Status date], 
                         CASE WHEN Ct_Sale_Agent_Ordinary.Gender = 0 THEN 'Ms. ' + Ct_Sale_Agent.Full_Name ELSE 'Mr. ' + Ct_Sale_Agent.Full_Name END AS Agent
                            FROM            dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"SELECT        dbo.Ct_App_Register.App_Number AS [App.no.], 
                         CASE WHEN Ct_App_Info_Person.Gender = 0 THEN 'Ms. ' + Ct_App_Info_Person.Last_Name + ' ' + Ct_App_Info_Person.First_Name ELSE 'Mr. ' + Ct_App_Info_Person.Last_Name
                          + ' '+ Ct_App_Info_Person.First_Name END AS [Applicant name],
						   CONVERT(varchar(10), dbo.Ct_App_Info.App_Date, 103) AS [Reg.date], 
                         dbo.Ct_App_Life_Product.User_Sum_Insure AS [S.A.], dbo.Ct_App_Life_Product.User_Premium AS Premium, dbo.Ct_Product.En_Abbr AS [Plan], 
                         dbo.Ct_Underwrite_Table.Status_Code AS Status, CONVERT(varchar(10), dbo.Ct_Underwriting.Updated_On, 103) AS [Status date], 
                         CASE WHEN Ct_Sale_Agent_Ordinary.Gender = 0 THEN 'Ms.' + Ct_Sale_Agent.Full_Name ELSE 'Mr.' + Ct_Sale_Agent.Full_Name END AS Agent
                         FROM            dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (dbo.Ct_Underwrite_Table.Status_Code in (" + check_status_code + "))";

            }

            if (order_by == 1) // App number
            {
                Order_by = " order by [App.no.] asc";
            }
            else  // Register date
            {
                Order_by = " order by [Reg.date] asc";
            }

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandText = sql + Order_by;
            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    DateTime next_due = DateTime.Now;
                    bl_report_application report_app = new bl_report_application();

                    report_app.App_Number = rdr.GetString(rdr.GetOrdinal("App.no."));
                    report_app.customer_name = rdr.GetString(rdr.GetOrdinal("Applicant name"));
                    report_app.App_Date = rdr.GetString(rdr.GetOrdinal("Reg.date"));
                    report_app.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("S.A."));
                    report_app.User_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));
                    report_app.En_Abbr = rdr.GetString(rdr.GetOrdinal("Plan"));
                    report_app.Status_Code = rdr.GetString(rdr.GetOrdinal("Status"));
                    report_app.Status_date = rdr.GetString(rdr.GetOrdinal("Status date"));
                    report_app.agent_name = rdr.GetString(rdr.GetOrdinal("Agent"));

                    application_report_list.Add(report_app);
                }
            }
        }
        return application_report_list;
    }

    public static DataTable Get_Dt_AppReportList(DateTime from_date, DateTime to_date, string check_status_code, int order_by)
    {
        DataTable dt = new DataTable();
        
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = "", Order_by = "";

//            if (check_status_code == "")
//            {
//                sql = @"select * from V_report_application where 
//                                (CONVERT(nvarchar(10),V_report_application.[Reg.date],103) between CONVERT(nvarchar(10),@from_date,111) 
//                                 and CONVERT(nvarchar(10),@to_date,111))";
//            }
//            else
//            {
//                sql = @"select * from V_report_application where 
//                                (CONVERT(nvarchar(10),V_report_application.[Reg.date],103) between CONVERT(nvarchar(10),@from_date,111) 
//                                 and CONVERT(nvarchar(10),@to_date,111)) and (V_report_application.Status in (" + check_status_code + "))";

//            }

            if (check_status_code == "")
            {
                sql = @"SELECT        dbo.Ct_App_Register.App_Number AS [App.no.], 
                         CASE WHEN Ct_App_Info_Person.Gender = 0 THEN 'Ms. ' + Ct_App_Info_Person.Last_Name + ' ' + Ct_App_Info_Person.Father_First_Name ELSE 'Mr. ' + Ct_App_Info_Person.Last_Name
                          + ' ' + Ct_App_Info_Person.Father_First_Name END AS [Applicant name],
						   CONVERT(varchar(10), dbo.Ct_App_Info.App_Date, 103) AS [Reg.date], 
                         dbo.Ct_App_Life_Product.User_Sum_Insure AS [S.A.], dbo.Ct_App_Life_Product.User_Premium AS Premium, dbo.Ct_Product.En_Abbr AS [Plan], 
                         dbo.Ct_Underwrite_Table.Status_Code AS Status, CONVERT(varchar(10), dbo.Ct_Underwriting.Updated_On, 103) AS [Status date], 
                         CASE WHEN Ct_Sale_Agent_Ordinary.Gender = 0 THEN 'Ms. ' + Ct_Sale_Agent.Full_Name ELSE 'Mr. ' + Ct_Sale_Agent.Full_Name END AS Agent
                            FROM            dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"SELECT        dbo.Ct_App_Register.App_Number AS [App.no.], 
                         CASE WHEN Ct_App_Info_Person.Gender = 0 THEN 'Ms. ' + Ct_App_Info_Person.Last_Name + ' ' + Ct_App_Info_Person.Father_First_Name ELSE 'Mr. ' + Ct_App_Info_Person.Last_Name
                          + ' ' + Ct_App_Info_Person.Father_First_Name END AS [Applicant name],
						   CONVERT(varchar(10), dbo.Ct_App_Info.App_Date, 103) AS [Reg.date], 
                         dbo.Ct_App_Life_Product.User_Sum_Insure AS [S.A.], dbo.Ct_App_Life_Product.User_Premium AS Premium, dbo.Ct_Product.En_Abbr AS [Plan], 
                         dbo.Ct_Underwrite_Table.Status_Code AS Status, CONVERT(varchar(10), dbo.Ct_Underwriting.Updated_On, 103) AS [Status date], 
                         CASE WHEN Ct_Sale_Agent_Ordinary.Gender = 0 THEN 'Ms. ' + Ct_Sale_Agent.Full_Name ELSE 'Mr. ' + Ct_Sale_Agent.Full_Name END AS Agent
                         FROM            dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (dbo.Ct_Underwrite_Table.Status_Code in (" + check_status_code + "))";

            }

            if (order_by == 1) // App number
            {
                Order_by = " order by [App.no.] asc";
            }
            else  // Register date
            {
                Order_by = " order by [Reg.date] asc";
            }

            cmd.CommandText = sql + Order_by;
            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetAppReportList] in class [bl_report_application]. Details: " + ex.Message);
            }
        }
        return dt;
    }
}