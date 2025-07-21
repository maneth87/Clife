using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_plan
/// </summary>
public class da_gtli_plan
{
    private static da_gtli_plan mytitle = null;
    public da_gtli_plan()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_plan();
		}
	}

    //Insert new plan
    public static bool InsertPlan(string gtli_plan_id, string gtli_plan, double sum_insured, int tpd, int dhc, int accidental_100plus, string gtli_company_id, System.DateTime created_on, string created_by)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                
                SqlCommand myCommand = new SqlCommand();

                //Open connection
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Insert_GTLI_Plan";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", gtli_plan_id);
                myCommand.Parameters.AddWithValue("@GTLI_Plan", gtli_plan);
                myCommand.Parameters.AddWithValue("@TPD", tpd);
                myCommand.Parameters.AddWithValue("@Sum_Insured", sum_insured);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", gtli_company_id);
                myCommand.Parameters.AddWithValue("@Created_On", created_on);
                myCommand.Parameters.AddWithValue("@DHC", dhc);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus", accidental_100plus);
                myCommand.Parameters.AddWithValue("@Created_By", created_by);
                //Execute the query
                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();

                //Set result to true to track whether the function is successfully operated
                result = true;
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [InsertPlan] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Get list of plan by company name 
    public static List<bl_gtli_plan> GetPlanListByCompanyName(string company_name)
    {
        List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
               
                SqlCommand myCommand = new SqlCommand();
                //Open connection
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Plan_List_By_Company_Name";
                myCommand.Parameters.AddWithValue("@Company_Name", company_name);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_plan myplan = new bl_gtli_plan();
                            myplan.GTLI_Plan = myReader.GetString(myReader.GetOrdinal("GTLI_Plan"));

                            myplan.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));

                            double sum_issured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                            int tpd = myReader.GetInt32(myReader.GetOrdinal("TPD"));
                            int dhc_option_value = myReader.GetInt32(myReader.GetOrdinal("DHC"));
                            int accidental_100plus = myReader.GetInt32(myReader.GetOrdinal("Accidental_100Plus"));

                            //String TPD
                            string tpd_str = "";
                            if (tpd != 0)
                            {
                                tpd_str = "Yes";
                            }
                            else
                            {
                                tpd_str = "No";
                            }

                            //String Accidental 100Plus
                            string accidental_100plus_str = "";
                            if (accidental_100plus != 0)
                            {
                                accidental_100plus_str = "Yes";
                            }
                            else
                            {
                                accidental_100plus_str = "No";
                            }

                            string dhc_str = "";


                            if (dhc_option_value > 0)
                            {
                                if (sum_issured == 0)
                                {
                                    myplan.GTLI_Plan += ": Dynamic  100Plus: " + accidental_100plus_str + " TPD: " + tpd_str + " DHC: " + dhc_option_value.ToString("C0");
                                }
                                else
                                {
                                    myplan.GTLI_Plan += ": " + sum_issured.ToString("C0") + " 100Plus: " + accidental_100plus_str + " TPD: " + tpd_str + " DHC: " + dhc_option_value.ToString("C0");
                                }
                            }
                            else
                            {
                                dhc_str = "No";
                                if (sum_issured == 0)
                                {
                                    myplan.GTLI_Plan += ": Dynamic 100Plus: " + accidental_100plus_str + " TPD: " + tpd_str + " DHC: " + dhc_str;
                                }
                                else
                                {
                                    myplan.GTLI_Plan += ": " + sum_issured.ToString("C0") + " 100Plus: " + accidental_100plus_str + " TPD: " + tpd_str + " DHC: " + dhc_str;
                                }
                               
                            }

                            plan_list.Add(myplan);
                        }
                    }
                    myReader.Close();
                }
                myConnection.Open();

                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [GetPlanListByCompanyName] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return plan_list;
    }

    //Function to retrieve last plan by company_id
    public static bl_gtli_plan GetLastPlanByCompanyID(string gtli_company_id)
    {

        bl_gtli_plan plan = null;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
              
                //sql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_GTLI_Plan_By_Company_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", gtli_company_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        plan = new bl_gtli_plan();

                        plan.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                        plan.GTLI_Plan = myReader.GetString(myReader.GetOrdinal("GTLI_Plan"));
                        plan.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                        plan.TPD_Option_Value = myReader.GetInt32(myReader.GetOrdinal("TPD"));
                        plan.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC"));
                        plan.Accidental_100Plus_Option_Value = myReader.GetInt32(myReader.GetOrdinal("Accidental_100Plus"));
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
            Log.AddExceptionToLog("Error in function [GetLastPlanByCompanyID] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return plan;
    }

    //Function to check existing plan
    public static bool CheckExistingPlan(string gtli_plan, double sum_insured, int tpd, int dhc, int accidental_100plus, string gtli_company_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
               
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Check_GTLI_Plan";

                myCommand.Parameters.AddWithValue("@GTLI_Plan", gtli_plan);
                myCommand.Parameters.AddWithValue("@Sum_Insured", sum_insured);
                myCommand.Parameters.AddWithValue("@TPD", tpd);
                myCommand.Parameters.AddWithValue("@DHC", dhc);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus", accidental_100plus);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", gtli_company_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        result = true;
                        break; // TODO: might not be correct. Was : Exit While
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
            Log.AddExceptionToLog("Error in function [CheckExistingPlan] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to retrieve plan by plan_id
    public static bl_gtli_plan GetPlan(string gtli_plan_id)
    {

        bl_gtli_plan plan = null;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {               
                //sql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Plan_By_Plan_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", gtli_plan_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        plan = new bl_gtli_plan();

                        plan.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                        plan.GTLI_Plan = myReader.GetString(myReader.GetOrdinal("GTLI_Plan"));
                        plan.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                        plan.TPD = myReader.GetInt32(myReader.GetOrdinal("TPD"));
                        plan.TPD_Option_Value = myReader.GetInt32(myReader.GetOrdinal("TPD"));
                        plan.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC"));
                        plan.Accidental_100Plus = myReader.GetInt32(myReader.GetOrdinal("Accidental_100Plus"));
                        plan.Accidental_100Plus_Option_Value = myReader.GetInt32(myReader.GetOrdinal("Accidental_100Plus"));
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
            Log.AddExceptionToLog("Error in function [GetPlan] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return plan;
    }

    //Get list of object plan by company name 
    public static List<bl_gtli_plan> GetObjectPlanListByCompanyName(string company_name)
    {
        List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                //Open connection
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Plan_List_By_Company_Name";
                myCommand.Parameters.AddWithValue("@Company_Name", company_name);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_plan myplan = new bl_gtli_plan();
                            myplan.GTLI_Plan = myReader.GetString(myReader.GetOrdinal("GTLI_Plan"));
                            myplan.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                            myplan.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                            myplan.TPD = myReader.GetInt32(myReader.GetOrdinal("TPD"));
                            myplan.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC"));
                            myplan.Accidental_100Plus = myReader.GetInt32(myReader.GetOrdinal("Accidental_100Plus"));
                                             
                            plan_list.Add(myplan);
                        }
                    }
                    myReader.Close();
                }
                myConnection.Open();

                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [GetObjectPlanListByCompanyName] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return plan_list;
    }
    //Update plan
    public static bool UpdatePlan(bl_gtli_plan plan)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                //Open connection
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Update_GTLI_Plan";

                myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", plan.GTLI_Plan_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Plan", plan.GTLI_Plan);
                myCommand.Parameters.AddWithValue("@Sum_Insured", plan.Sum_Insured);
                myCommand.Parameters.AddWithValue("@TPD_Option_Value", plan.TPD_Option_Value);
                myCommand.Parameters.AddWithValue("@DHC_Option_Value", plan.DHC_Option_Value);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Option_Value", plan.Accidental_100Plus_Option_Value);
                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
                result = true;
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [UpdatePlan] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return result;

    }

    //Function to delete plan
    public static bool DeletePlan(string plan_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Delete_GTLI_Plan";
                myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", plan_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeletePlan] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return result;
    }


    //Function to retrieve plan name by gtli_certificate_id
    public static string GetPlanNameByCertificateID(string gtli_certificate_id)
    {

        string plan_name = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                //sql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Plan_Name_By_Certificate_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        plan_name = myReader.GetString(myReader.GetOrdinal("GTLI_Plan"));
                      
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
            Log.AddExceptionToLog("Error in function [GetPlanNameByCertificateID] in class [da_gtli_plan]. Details: " + ex.Message);
        }
        return plan_name;
    }
}