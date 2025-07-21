using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_report_sale
/// </summary>
public class da_report_sale
{
    private static da_report_sale mytitle = null;
    public da_report_sale()
	{
        if (mytitle == null)
        {
            mytitle = new da_report_sale();
		}
	}   
       

    //Get applications count by sale agent id and bewteen dates 
    public static int GetApplicationCountBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        int application_count = 0;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_App_Count_By_Sale_Agent_ID_And_Between_Dates";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        application_count = myReader.GetInt32(myReader.GetOrdinal("Application_Count"));
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationCountBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return application_count;
    }

    //Get applications submit First Year Premium (SBMT FYP) by sale agent id and between dates 
    public static double GetSBMTFYPBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        double sbmt_fyp = 0;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_SBMT_FYP_By_Sale_Agent_ID_And_Between_Dates";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        double pay_amount = myReader.GetDouble(myReader.GetOrdinal("Total_Amount"));
                        int pay_mode = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode"));
                        string product_id = myReader.GetString(myReader.GetOrdinal("Product_ID"));
                        sbmt_fyp += pay_amount;
                      
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetSBMTFYPBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return sbmt_fyp;
    }
  
    //Get application os (underwriting process) count by sale agent id and between dates
    public static int GetApplicationOSCountBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        int app_os_count = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                string sql = @"select Count(DISTINCT App_Register_ID) As OS_Count from Cv_Underwriting where Cv_Underwriting.Sale_Agent_ID = @Sale_Agent_ID ";
                sql += @" AND (Cv_Underwriting.Status_Code not in ('IF','DC', 'CC', 'NT', 'PP') or Cv_Underwriting.Status_Code is null) ";
                sql += @" AND (CONVERT(nvarchar(10),Cv_Underwriting.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) ";
                sql += @" and CONVERT(nvarchar(10),@to_date,111)) and (App_Register_ID not in (select App_Register_ID from Ct_App_Register_Cancel)) ";
                                
                SqlCommand cmd = new SqlCommand(sql, myConnection);

                //Parameters
                cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

                myConnection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        app_os_count = rdr.GetInt32(rdr.GetOrdinal("OS_Count"));

                    }
                }/// End loop
            }//End using 
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationOSCountBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return app_os_count;
    }
    
    //Get application decline count by sale agent id and between dates
    public static int GetApplicationDeclineCountBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        int app_decline_count = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                string sql = @"select Count(DISTINCT App_Register_ID) As Decline_Count from Cv_Underwriting where Cv_Underwriting.Sale_Agent_ID = @Sale_Agent_ID AND Cv_Underwriting.Status_Code in ('DC', 'NT') ";
                sql += @" AND (CONVERT(nvarchar(10),Cv_Underwriting.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) ";

                SqlCommand cmd = new SqlCommand(sql, myConnection);

                //Parameters
                cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

                myConnection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        app_decline_count = rdr.GetInt32(rdr.GetOrdinal("Decline_Count"));
              
                    }
                }/// End loop
            }//End using 
        }
        catch (Exception ex)
        {            
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationDeclineCountBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return app_decline_count;
    }

    //Get application cancel count by sale agent id and between dates
    public static int GetApplicationCancelCountBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        int app_cancel_count = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                string sql = @"select Count(App_Register_ID) As Cancel_Count from Cv_Underwriting where Cv_Underwriting.Sale_Agent_ID = @Sale_Agent_ID AND Cv_Underwriting.Status_Code = 'CC' ";
                sql += @" AND (CONVERT(nvarchar(10),Cv_Underwriting.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) ";

                SqlCommand cmd = new SqlCommand(sql, myConnection);

                //Parameters
                cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

                myConnection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        app_cancel_count = rdr.GetInt32(rdr.GetOrdinal("Cancel_Count"));

                    }
                }/// End loop
            }//End using 
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationCancelCountBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return app_cancel_count;
    }

    //Get application postpone count by sale agent id and between dates
    public static int GetApplicationPostponeCountBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        int app_postpone_count = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                string sql = @"select Count(App_Register_ID) As Postpone_Count from Cv_Underwriting where Cv_Underwriting.Sale_Agent_ID = @Sale_Agent_ID AND Cv_Underwriting.Status_Code = 'PP' ";
                sql += @" AND (CONVERT(nvarchar(10),Cv_Underwriting.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) ";

                SqlCommand cmd = new SqlCommand(sql, myConnection);

                //Parameters
                cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

                myConnection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        app_postpone_count = rdr.GetInt32(rdr.GetOrdinal("Postpone_Count"));

                    }
                }/// End loop
            }//End using 
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationPostponeCountBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return app_postpone_count;
    }

    //Get policy issue count by sale agent id and between dates include only individual policies
    public static int GetPolicyIssueCountBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        int policy_issue_count = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Issue_Count_By_Sale_Agent_ID_And_Between_Dates";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        policy_issue_count = myReader.GetInt32(myReader.GetOrdinal("Issue_Count"));
                        
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }//End using 
        }
        catch (Exception ex)
        {            
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicyIssueCountBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return policy_issue_count;
    }

    //Get policy api by sale agent id and between dates
    public static double GetPolicyAPIBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        double api = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_API_By_Sale_Agent_ID_And_Between_Dates";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        double premium = myReader.GetDouble(myReader.GetOrdinal("Premium_Paid"));
                        int pay_mode = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode"));
                        string product_id = myReader.GetString(myReader.GetOrdinal("Product_ID"));

                        switch (pay_mode)
                        {
                            case 0: //Single
                            case 1: //Annually
                                api += premium;
                                break;
                            case 2: //Semi-annually
                                api += (premium * 2);
                                break;
                            case 3: //Quarterly
                                api += (premium * 4);
                                break;
                            case 4: //Monthly
                                api += (premium * 12);
                                break;
                        }                                

                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();

          
            }//End using 
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetAPIBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return api;
    }

    //Get issue fyp by sale agent id and between dates include only individual policies
    public static double GetIssueFYPBySaleAgentIDAndBetweenDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        double issue_fyp = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_ISSUE_FYP_By_Sale_Agent_ID_And_Between_Dates";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        issue_fyp += myReader.GetDouble(myReader.GetOrdinal("FYP"));

                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();                             
               
            }//End using 
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetIssueFYPBySaleAgentIDAndBetweenDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return issue_fyp;
    }

    //Get applications bewteen dates 
    public static List<bl_report_sale_app_by_status> GetApplicationByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_report_sale_app_by_status> sale_app_by_status_list = new List<bl_report_sale_app_by_status>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Report_App_By_Dates";
    
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_report_sale_app_by_status sale_app_by_status = new bl_report_sale_app_by_status();

                        sale_app_by_status.App_Register_ID = myReader.GetString(myReader.GetOrdinal("App_Register_ID"));
                        sale_app_by_status.Agent_Name = myReader.GetString(myReader.GetOrdinal("Full_Name"));
                        sale_app_by_status.App_Date = myReader.GetDateTime(myReader.GetOrdinal("App_Date"));
                        sale_app_by_status.App_No = myReader.GetString(myReader.GetOrdinal("Original_App_Number"));

                        sale_app_by_status.Last_Status = GetApplicationLastStatus(sale_app_by_status.App_Register_ID);

                        sale_app_by_status.Policy_No = GetPolicyNumber(sale_app_by_status.App_Register_ID);

                        sale_app_by_status_list.Add(sale_app_by_status);
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationByDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return sale_app_by_status_list;
    }

    //Get applications bewteen dates and channel
    public static List<bl_report_sale_app_by_status> GetApplicationByDatesAndChannel(DateTime from_date, DateTime to_date, string channel_id)
    {
        List<bl_report_sale_app_by_status> sale_app_by_status_list = new List<bl_report_sale_app_by_status>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Report_App_By_Dates_And_Channel";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);
                myCommand.Parameters.AddWithValue("@Channel_ID", channel_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_report_sale_app_by_status sale_app_by_status = new bl_report_sale_app_by_status();

                        sale_app_by_status.App_Register_ID = myReader.GetString(myReader.GetOrdinal("App_Register_ID"));
                        sale_app_by_status.Agent_Name = myReader.GetString(myReader.GetOrdinal("Full_Name"));
                        sale_app_by_status.App_Date = myReader.GetDateTime(myReader.GetOrdinal("App_Date"));
                        sale_app_by_status.App_No = myReader.GetString(myReader.GetOrdinal("Original_App_Number"));

                        sale_app_by_status.Last_Status = GetApplicationLastStatus(sale_app_by_status.App_Register_ID);

                        sale_app_by_status.Policy_No = GetPolicyNumber(sale_app_by_status.App_Register_ID);

                        sale_app_by_status_list.Add(sale_app_by_status);
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationByDatesAndChannel] in class [da_report_sale]. Details: " + ex.Message);

        }
        return sale_app_by_status_list;
    }

    //Get applications by sale agent id and bewteen dates 
    public static List<bl_report_sale_app_by_status> GetApplicationBySaleAgentIDAndDates(string sale_agent_id, DateTime from_date, DateTime to_date)
    {
        List<bl_report_sale_app_by_status> sale_app_by_status_list = new List<bl_report_sale_app_by_status>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Report_App_By_Sale_Agent_And_Dates";

                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_report_sale_app_by_status sale_app_by_status = new bl_report_sale_app_by_status();

                        sale_app_by_status.App_Register_ID = myReader.GetString(myReader.GetOrdinal("App_Register_ID"));
                        sale_app_by_status.Agent_Name = myReader.GetString(myReader.GetOrdinal("Full_Name"));
                        sale_app_by_status.App_Date = myReader.GetDateTime(myReader.GetOrdinal("App_Date"));
                        sale_app_by_status.App_No = myReader.GetString(myReader.GetOrdinal("Original_App_Number"));
                        sale_app_by_status.Premium = myReader.GetDouble(myReader.GetOrdinal("User_Premium"));
                        sale_app_by_status.Last_Status = GetApplicationLastStatus(sale_app_by_status.App_Register_ID);

                        sale_app_by_status.Policy_No = GetPolicyNumber(sale_app_by_status.App_Register_ID);

                        sale_app_by_status_list.Add(sale_app_by_status);
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationBySaleAgentIDAndDates] in class [da_report_sale]. Details: " + ex.Message);

        }
        return sale_app_by_status_list;
    }

    //Get applications bewteen dates and team
    public static List<bl_report_sale_app_by_status> GetApplicationByDatesAndTeam(DateTime from_date, DateTime to_date, string sale_agent_team_id)
    {
        List<bl_report_sale_app_by_status> sale_app_by_status_list = new List<bl_report_sale_app_by_status>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Report_App_By_Dates_And_Team";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);
                myCommand.Parameters.AddWithValue("@Sale_Agent_Team_ID", sale_agent_team_id);
                

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_report_sale_app_by_status sale_app_by_status = new bl_report_sale_app_by_status();

                        sale_app_by_status.App_Register_ID = myReader.GetString(myReader.GetOrdinal("App_Register_ID"));
                        sale_app_by_status.Agent_Name = myReader.GetString(myReader.GetOrdinal("Full_Name"));
                        sale_app_by_status.App_Date = myReader.GetDateTime(myReader.GetOrdinal("App_Date"));
                        sale_app_by_status.App_No = myReader.GetString(myReader.GetOrdinal("Original_App_Number"));

                        sale_app_by_status.Last_Status = GetApplicationLastStatus(sale_app_by_status.App_Register_ID);

                        sale_app_by_status.Policy_No = GetPolicyNumber(sale_app_by_status.App_Register_ID);

                        sale_app_by_status_list.Add(sale_app_by_status);
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationByDatesAndTeam] in class [da_report_sale]. Details: " + ex.Message);

        }
        return sale_app_by_status_list;
    }

    //Get applications last status 
    public static string GetApplicationLastStatus(string app_register_id)
    {
        string last_status = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_App_Status";

                myCommand.Parameters.AddWithValue("@App_Register_ID", app_register_id);
               
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        last_status = myReader.GetString(myReader.GetOrdinal("Last_Status"));

                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetApplicationLastStatus] in class [da_report_sale]. Details: " + ex.Message);

        }
        return last_status;
    }

    //Get policy number of this application
    public static string GetPolicyNumber(string app_register_id)
    {
        string policy_number = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Number_By_App_Register_ID";

                myCommand.Parameters.AddWithValue("@App_Register_ID", app_register_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        policy_number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));

                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicyNumber] in class [da_report_sale]. Details: " + ex.Message);

        }
        return policy_number;
    }
}