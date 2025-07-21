using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_policy
/// </summary>
public class da_gtli_policy
{
    private static da_gtli_policy mytitle = null;
    public da_gtli_policy()
    {
        if (mytitle == null)
        {
            mytitle = new da_gtli_policy();
	    }
    }
    //Get last policy number
    public static string GetPolicyNumber()
    {
        string policy_number = "GL00000001";
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
                myCommand.CommandText = "SP_Get_Last_GTLI_Policy_Number";

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            policy_number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));

                            int strSplit = Convert.ToInt16(ExtractNumbers(policy_number)) + 1;
                            policy_number = "GL" + strSplit.ToString("D8");

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
            Log.AddExceptionToLog("Error in function [GetPolicyNumber] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return policy_number;
    }

    //Get number from a string
    public static string ExtractNumbers(string expr)
    {
        return string.Join(null, System.Text.RegularExpressions.Regex.Split(expr, "GL"));
    }

    //Get policy Numbe by company_id
    public static string GetPolicyNumberByCompanyID(string gtli_company_id)
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
                myCommand.CommandText = "SP_Get_GTLI_Policy_Number_By_Company_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", gtli_company_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            policy_number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetPolicyNumberByCompanyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return policy_number;
    }
   
    //Insert new policy
    public static bool InsertPolicy(bl_gtli_policy policy)
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
                myCommand.CommandText = "SP_Insert_GTLI_Policy";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", policy.GTLI_Policy_ID);
                myCommand.Parameters.AddWithValue("@Policy_Number", policy.Policy_Number);
                myCommand.Parameters.AddWithValue("@Effective_Date", policy.Effective_Date);
                myCommand.Parameters.AddWithValue("@Expiry_Date", policy.Expiry_Date);           
                myCommand.Parameters.AddWithValue("@Life_Premium", policy.Life_Premium);
                myCommand.Parameters.AddWithValue("@TPD_Premium", policy.TPD_Premium);
                myCommand.Parameters.AddWithValue("@DHC_Premium", policy.DHC_Premium);
                myCommand.Parameters.AddWithValue("@Agreement_date", policy.Agreement_date);
                myCommand.Parameters.AddWithValue("@Issue_Date", policy.Issue_Date);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", policy.GTLI_Company_ID);
                myCommand.Parameters.AddWithValue("@Created_By", policy.Created_By);
                myCommand.Parameters.AddWithValue("@Created_On", policy.Created_On);
                myCommand.Parameters.AddWithValue("@Maturity_Date", policy.Maturity_Date);

                myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", policy.TPD_Premium_Tax_Amount);
                myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", policy.TPD_Premium_Discount);
               
                myCommand.Parameters.AddWithValue("@Original_TPD_Premium", policy.Original_TPD_Premium);
                myCommand.Parameters.AddWithValue("@Original_Life_Premium", policy.Original_Life_Premium);
                myCommand.Parameters.AddWithValue("@Original_DHC_Premium", policy.Original_DHC_Premium);
                myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", policy.Original_Accidental_100Plus_Premium);

                myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", policy.Life_Premium_Tax_Amount);
                myCommand.Parameters.AddWithValue("@Life_Premium_Discount", policy.Life_Premium_Discount);
                myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", policy.DHC_Premium_Tax_Amount);

                myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", policy.DHC_Premium_Discount);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", policy.Accidental_100Plus_Premium_Tax_Amount);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", policy.Accidental_100Plus_Premium);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", policy.Accidental_100Plus_Premium_Discount);

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
            Log.AddExceptionToLog("Error function [InsertPolicy] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        //Return result to the function caller

        return result;
    }

    //Get policy by ID
    public static bl_gtli_policy GetGTLIPolicyByID(string gtli_policy_id)
    {
        //Declare object
        bl_gtli_policy policy = null;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Policy_By_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            policy = new bl_gtli_policy();
                            policy.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            policy.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            policy.Agreement_date = myReader.GetDateTime(myReader.GetOrdinal("Agreement_date"));
                            policy.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            policy.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            policy.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            policy.Issue_Date = myReader.GetDateTime(myReader.GetOrdinal("Issue_Date"));                      
                            policy.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                            policy.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));                         
                            policy.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));

                            policy.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            policy.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                            policy.Accidental_100Plus_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Tax_Amount"));

                            policy.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                            policy.DHC_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Tax_Amount"));
                            policy.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                            policy.Life_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Tax_Amount"));

                            policy.Original_Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Accidental_100Plus_Premium"));
                            policy.Original_DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_DHC_Premium"));
                            policy.Original_Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Life_Premium"));
                            policy.Original_TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_TPD_Premium"));

                           
                            policy.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                            policy.TPD_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Tax_Amount"));

                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetGTLPolicyByID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return policy;
    }

    //Get list of distinct company_id
    public static ArrayList GetListOfCompanyID()
    {
        //Declare object
        string company_id = "";
        ArrayList list_of_company_id = new ArrayList();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
               
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Company_ID_List";              

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            company_id = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));

                            list_of_company_id.Add(company_id);
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetListOfCompanyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return list_of_company_id;
    }

    //Check policy number
    public static bool CheckCompanyId(string company_id)
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
                myCommand.CommandText = "SP_Check_GTLI_Policy_By_Company_ID";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true
                        if (myReader.HasRows)
                        {
                            result = true;

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
            Log.AddExceptionToLog("Error in function [CheckCompanyId] in class [da_gtli_policy]. Details: " + ex.Message);
        }

        return result;
    }

    //Get last gtli policy by company id
    public static bl_gtli_policy GetLastGTLIPolicyBYCompanyID(string company_id)
    {
        //Declare object
        bl_gtli_policy policy = null;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_GTLI_Policy_By_Company_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            policy = new bl_gtli_policy();
                            policy.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            policy.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            policy.Agreement_date = myReader.GetDateTime(myReader.GetOrdinal("Agreement_date"));
                            policy.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            policy.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            policy.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            policy.Issue_Date = myReader.GetDateTime(myReader.GetOrdinal("Issue_Date"));
                            policy.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                            policy.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                            policy.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));

                            policy.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            policy.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                            policy.Accidental_100Plus_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Tax_Amount"));

                            policy.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                            policy.DHC_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Tax_Amount"));
                            policy.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                            policy.Life_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Tax_Amount"));

                            policy.Original_Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Accidental_100Plus_Premium"));
                            policy.Original_DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_DHC_Premium"));
                            policy.Original_Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Life_Premium"));
                            policy.Original_TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_TPD_Premium"));
                                                       
                            policy.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                            policy.TPD_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Tax_Amount"));

                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetLastGTLIPolicyBYCompanyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return policy;
    }

    //Function to delete gtli policy
    public static bool DeleteGTLIPolicy(string gtli_policy_id)
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
                myCommand.CommandText = "SP_Delete_GTLI_Policy";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLIPolicy] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return result;
    }

    //Get policy list by company_id
    public static ArrayList GetMasterListByCompanyID(string company_id)
    {       
        ArrayList gtli_master_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_Policy_By_Company_ID";    
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_master_list gtli_master = new bl_gtli_master_list();

                            gtli_master.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            gtli_master.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            gtli_master.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            gtli_master.Sum_Insured = da_gtli_premium.GetGTLTotalSumInsuredByPolicyID(gtli_master.GTLI_Policy_ID);
                            gtli_master.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            gtli_master.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium")); 
                            gtli_master.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            gtli_master.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            gtli_master.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            gtli_master.GTLI_Company = da_gtli_company.GetCompanyNameByID(company_id);
                            gtli_master.Issue_Date = myReader.GetDateTime(myReader.GetOrdinal("Issue_Date"));

                            gtli_master_list.Add(gtli_master);
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
            Log.AddExceptionToLog("Error in function [GetMasterListByCompanyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return gtli_master_list;
    }

    //Get plan list by policy id
    public static ArrayList GetListOfPlanByPolicyID(string gtli_policy_id)
    {
        //Declare object
        string plan = "";
        ArrayList list_of_plan = new ArrayList();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Plan_ID_List_By_GTLI_Policy_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);
               
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            string plan_id = "0";
                            plan_id = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));

                            bl_gtli_plan myplan = da_gtli_plan.GetPlan(plan_id);

                            string tpd = "";
                            string dhc = "";

                            if (myplan.TPD == 1)
                            {
                                tpd = "Yes";
                            }
                            else
                            {
                                tpd = "No";
                            }

                            if (myplan.DHC_Option_Value == 0)
                            {
                                dhc = "No";
                                plan = myplan.GTLI_Plan + ": $" + myplan.Sum_Insured + ", TPD: " + tpd + ", DHC: " + dhc;
                            }
                            else
                            {
                                dhc = myplan.DHC_Option_Value.ToString();
                                plan = myplan.GTLI_Plan + ": $" + myplan.Sum_Insured + ", TPD: " + tpd + ", DHC: $" + dhc;

                            }

                            list_of_plan.Add(plan);
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetListOfPlanByPolicyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return list_of_plan;
    }

    //Get plan by policy id and certificate id
    public static bl_gtli_plan GetPlanByPolicyIDAndCertificateID(string gtli_policy_id, string certificate_id)
    {
        //Declare object
        bl_gtli_plan plan = new bl_gtli_plan();      
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Plan_By_Policy_ID_And_Certificate_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", certificate_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            plan.GTLI_Plan = myReader.GetString(myReader.GetOrdinal("GTLI_Plan"));
                            plan.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                            plan.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                            plan.TPD = myReader.GetInt32(myReader.GetOrdinal("TPD"));
                            plan.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC"));
                         
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetPlanByPolicyIDAndCertificateID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return plan;
    }

    //Get Added Policy List
    public static ArrayList GetAddedPolicyList(bl_gtli_policy_search policy)
    {
        bl_gtli_policy_search my_policy = default(bl_gtli_policy_search);
        ArrayList added_policy_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_Added_Policy_List";
                myCommand.Parameters.AddWithValue("@From_Date", policy.From_Date);
                myCommand.Parameters.AddWithValue("@To_Date", policy.To_Date);
                myCommand.Parameters.AddWithValue("@Company_Name", policy.Company_Name);
                myCommand.Parameters.AddWithValue("@Date_Type", policy.Date_Type);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            my_policy = new bl_gtli_policy_search();

                            my_policy.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            my_policy.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));
                            my_policy.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
                            my_policy.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            my_policy.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            my_policy.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            my_policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            my_policy.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            my_policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            my_policy.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                            my_policy.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            my_policy.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

                            my_policy.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                            my_policy.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                            my_policy.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                            my_policy.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                           
                            my_policy.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                            my_policy.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                            my_policy.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                            my_policy.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));

                            my_policy.Total_Premium = my_policy.Life_Premium + my_policy.TPD_Premium + my_policy.DHC_Premium;

                            if (my_policy.GTLI_Plan_ID != "")
                            {
                                bl_gtli_plan my_plan = new bl_gtli_plan();
                                my_plan = da_gtli_plan.GetPlan(my_policy.GTLI_Plan_ID);

                                my_policy.GTLI_Plan = my_plan.GTLI_Plan;
                            }

                            if (my_policy.Sale_Agent_ID != "")
                            {
                                if (da_sale_agent.CheckSaleAgentId(my_policy.Sale_Agent_ID))
                                {
                                    my_policy.Employee_Name = da_sale_agent.GetSaleAgentNameByID(my_policy.Sale_Agent_ID);
                                }
                                else
                                {
                                    my_policy.Employee_Name = "";
                                }

                            }

                            added_policy_list.Add(my_policy);
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
            Log.AddExceptionToLog("Error in function [GetAddedPolicyList] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return added_policy_list;
    }

    //Get created policy list
    public static ArrayList GetCreatedPolicyList(bl_gtli_policy_search policy)
    {
        bl_gtli_policy_search my_policy = default(bl_gtli_policy_search);
        ArrayList created_policy_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_Created_Policy_List";
                myCommand.Parameters.AddWithValue("@Company_Name", policy.Company_Name);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            my_policy = new bl_gtli_policy_search();

                            my_policy.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            my_policy.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));
                            my_policy.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
                            my_policy.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            my_policy.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            my_policy.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            my_policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            my_policy.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            my_policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            my_policy.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                            my_policy.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            my_policy.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

                            my_policy.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                            my_policy.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                            my_policy.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                            my_policy.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                          
                            my_policy.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                            my_policy.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                            my_policy.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                            my_policy.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                            my_policy.Total_Premium = my_policy.Life_Premium + my_policy.TPD_Premium + my_policy.DHC_Premium;

                            if (my_policy.GTLI_Plan_ID != "")
                            {
                                bl_gtli_plan my_plan = new bl_gtli_plan();
                                my_plan = da_gtli_plan.GetPlan(my_policy.GTLI_Plan_ID);

                                my_policy.GTLI_Plan = my_plan.GTLI_Plan;
                            }

                            if (my_policy.Sale_Agent_ID != "")
                            {
                                if (da_sale_agent.CheckSaleAgentId(my_policy.Sale_Agent_ID))
                                {
                                    my_policy.Employee_Name = da_sale_agent.GetSaleAgentNameByID(my_policy.Sale_Agent_ID);
                                }
                                else
                                {
                                    my_policy.Employee_Name = "";
                                }

                            }

                            created_policy_list.Add(my_policy);
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
            Log.AddExceptionToLog("Error in function [GetCreatedPolicyList] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return created_policy_list;
    }

    //Get resign member list
    public static ArrayList GetResignMemberList(bl_gtli_policy_search policy)
    {
        bl_gtli_policy_search my_policy = default(bl_gtli_policy_search);
        ArrayList resign_member_policy_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_Resign_Member_Policy_List";
                myCommand.Parameters.AddWithValue("@From_Date", policy.From_Date);
                myCommand.Parameters.AddWithValue("@To_Date", policy.To_Date);
                myCommand.Parameters.AddWithValue("@Company_Name", policy.Company_Name);
                myCommand.Parameters.AddWithValue("@Date_Type", policy.Date_Type);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            my_policy = new bl_gtli_policy_search();

                            my_policy.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            my_policy.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));
                            my_policy.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
                            my_policy.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            my_policy.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            my_policy.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            my_policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            my_policy.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            my_policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            my_policy.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                            my_policy.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            my_policy.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

                            my_policy.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                            my_policy.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                            my_policy.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                            my_policy.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                         
                            my_policy.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                            my_policy.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                            my_policy.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                            my_policy.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));

                            my_policy.Total_Premium = my_policy.Life_Premium + my_policy.TPD_Premium + my_policy.DHC_Premium;

                            if (my_policy.GTLI_Plan_ID != "0")
                            {
                                bl_gtli_plan my_plan = new bl_gtli_plan();
                                my_plan = da_gtli_plan.GetPlan(my_policy.GTLI_Plan_ID);

                                my_policy.GTLI_Plan = my_plan.GTLI_Plan;
                            }

                            if (my_policy.Sale_Agent_ID != "")
                            {
                                if (da_sale_agent.CheckSaleAgentId(my_policy.Sale_Agent_ID))
                                {
                                    my_policy.Employee_Name = da_sale_agent.GetSaleAgentNameByID(my_policy.Sale_Agent_ID);
                                }
                                else
                                {
                                    my_policy.Employee_Name = "";
                                }

                            }


                            resign_member_policy_list.Add(my_policy);
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
            Log.AddExceptionToLog("Error in function [GetResignMemberList] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return resign_member_policy_list;
    }

    //Get policy history by policy number
    public static List<bl_transaction_history> GetPolicyHistoryByPolicyNumber(string policy_number, string sortColum, string sortDir, System.DateTime from_date, System.DateTime to_date)
    {
        //Declare object
        bl_transaction_history mytransaction = null;
        List<bl_transaction_history> list_of_transaction = new List<bl_transaction_history>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Policy_History_By_Policy_Number";
                myCommand.Parameters.AddWithValue("@policy_number", policy_number);
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            mytransaction = new bl_transaction_history();

                            mytransaction.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            string plan_id = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                            mytransaction.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            mytransaction.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            mytransaction.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                            mytransaction.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            mytransaction.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            mytransaction.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            mytransaction.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));

                            mytransaction.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                            mytransaction.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));

                            if (mytransaction.Transaction_Type != 3)
                            {
                                mytransaction.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                                bl_gtli_plan myplan = da_gtli_plan.GetPlan(plan_id);
                                mytransaction.GTLI_Plan = myplan.GTLI_Plan;
                            }

                            string company_id = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));

                            mytransaction.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                            mytransaction.Company_Name = da_gtli_company.GetCompanyNameByID(company_id);

                            mytransaction.Total_Premium = mytransaction.Life_Premium + mytransaction.TPD_Premium + mytransaction.DHC_Premium;
                            list_of_transaction.Add(mytransaction);
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetPolicyHistoryByPolicyNumber] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return list_of_transaction;
    }

    //Update policy in total premium
    public static void UpdatePremium(double life_premium, double tpd_premium, double dhc_premium, double accidental_100plus_premium, double accidental_100plus_premium_discount, double life_premium_discount, double tpd_premium_discount, double dhc_premium_discount, double original_accidental_100plus_premium, double original_life_premium, double original_tpd_premium, double original_dhc_premium, double accidental_100plus_premium_tax_amount, double life_premium_tax_amount, double tpd_premium_tax_amount, double dhc_premium_tax_amount, string gtli_policy_id)
    {
  
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
                myCommand.CommandText = "SP_Update_GTLI_Policy_Premium";

                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);
                myCommand.Parameters.AddWithValue("@Life_Premium", life_premium);
                myCommand.Parameters.AddWithValue("@TPD_Premium", tpd_premium);
                myCommand.Parameters.AddWithValue("@DHC_Premium", dhc_premium);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", accidental_100plus_premium);
                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", accidental_100plus_premium_discount);
                myCommand.Parameters.AddWithValue("@Life_Premium_Discount", life_premium_discount);
                myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", tpd_premium_discount);
                myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", dhc_premium_discount);
                myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", original_accidental_100plus_premium);
                myCommand.Parameters.AddWithValue("@Original_Life_Premium", original_life_premium);
                myCommand.Parameters.AddWithValue("@Original_TPD_Premium", original_tpd_premium);
                myCommand.Parameters.AddWithValue("@Original_DHC_Premium", original_dhc_premium);

                myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", accidental_100plus_premium_tax_amount);
                myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", life_premium_tax_amount);
                myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", tpd_premium_tax_amount);
                myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", dhc_premium_tax_amount);

                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [UpdatePremium] in class [da_gtli_policy]. Details: " + ex.Message);
        }

    }

    //Get gtli policy status by gtli_policy_id
    public static string GetGTLIPolicyStatusBYGTLIPolicyID(string gtli_policy_id)
    {
        //Declare object
        string policy_status = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Policy_Status_By_GTLI_Policy_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            policy_status = myReader.GetString(myReader.GetOrdinal("Policy_Status_Type_ID"));
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetGTLIPolicyStatusBYGTLIPolicyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return policy_status;
    }

    //Check whether this plan_id is exist in last policy -> if true can't add new policy to this plan
    public static bool CheckPlanIdByPlanIDAndPolicyID(string plan_id, string policy_id)
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
                myCommand.CommandText = "SP_Check_GTLI_Plan_ID_By_Plan_ID_And_Policy_ID";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", policy_id);
                myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", plan_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true
                        if (myReader.HasRows)
                        {
                            result = true;

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
            Log.AddExceptionToLog("Error in function [CheckPlanIdByPlanIDAndPolicyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }

        return result;
    }

    //Get policy master list for add member by company_id
    public static ArrayList GetMasterListForAddMemberByCompanyID(string company_id)
    {
       
        ArrayList gtli_master_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_Policy_For_Add_Member_By_Company_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_master_list gtli_master = new bl_gtli_master_list();

                            gtli_master.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            gtli_master.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            gtli_master.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            gtli_master.Sum_Insured = da_gtli_premium.GetGTLTotalSumInsuredByPolicyID(gtli_master.GTLI_Policy_ID);
                            gtli_master.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            gtli_master.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            gtli_master.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            gtli_master.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            gtli_master.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            gtli_master.GTLI_Company = da_gtli_company.GetCompanyNameByID(company_id);

                            gtli_master_list.Add(gtli_master);
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
            Log.AddExceptionToLog("Error in function [GetMasterListForAddMemberByCompanyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return gtli_master_list;
    }

    //Get policy master list for resign member by company_id
    public static ArrayList GetMasterListForResignMemberByCompanyID(string company_id)
    {
       
        ArrayList gtli_master_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_Policy_For_Resign_Member_By_Company_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_master_list gtli_master = new bl_gtli_master_list();

                            gtli_master.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                            gtli_master.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                            gtli_master.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                            gtli_master.Sum_Insured = da_gtli_premium.GetGTLTotalSumInsuredByPolicyID(gtli_master.GTLI_Policy_ID);
                            gtli_master.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                            gtli_master.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                            gtli_master.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                            gtli_master.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                            gtli_master.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                            gtli_master.GTLI_Company = da_gtli_company.GetCompanyNameByID(company_id);

                            gtli_master_list.Add(gtli_master);
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
            Log.AddExceptionToLog("Error in function [GetMasterListForResignMemberByCompanyID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return gtli_master_list;
    }

    //Get policy id by premium id
    public static string GetGTLIPolicyIDByPremiumID(string gtli_premium_id)
    {
        //Declare object
        string policy_id = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Policy_ID_By_Premium_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            policy_id = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                          
                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetGTLIPolicyIDByPremiumID] in class [da_gtli_policy]. Details: " + ex.Message);
        }
        return policy_id;
    }
}