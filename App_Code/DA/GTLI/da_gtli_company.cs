using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Collections;
/// <summary>
/// Summary description for da_gtli_company
/// </summary>
public class da_gtli_company
{
	#region "Constructor"
    private static da_gtli_company mytitle = null;
    public da_gtli_company()
        {
            if (mytitle == null)
            {
                mytitle = new da_gtli_company();
	        }
        }
    #endregion

    #region "Public Functions"
        //Insert new company
        public static bool InsertCompany(bl_gtli_company company)
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
                    myCommand.CommandText = "SP_Insert_GTLI_Company";

                    //Bind parameter to value received from the function caller
                    myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company.GTLI_Company_ID);
                    myCommand.Parameters.AddWithValue("@Company_Name", company.Company_Name);
                    myCommand.Parameters.AddWithValue("@Type_Of_Business", company.Type_Of_Business);
                    myCommand.Parameters.AddWithValue("@Company_Email", company.Company_Email);
                    myCommand.Parameters.AddWithValue("@Created_On", company.Created_On);
                    myCommand.Parameters.AddWithValue("@Company_Address", company.Company_Address);
                    myCommand.Parameters.AddWithValue("@Created_By", company.Created_By);
                    //Execute the query                   
                    myCommand.ExecuteNonQuery();
                    //Close connection
                    myConnection.Close();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                //Add error to log for analysis
                Log.AddExceptionToLog("Error function [InsertCompany] in class [da_gtli_company]. Details: " + ex.Message);
            }
            //Return result to the function caller
            //If <true> means the function has insertd new article successfully
            return result;
        }

        //Function to retrieve list of company by company (for search company)
        public static List<bl_gtli_company> GetListOfCompanyByCompanyName(string company_name)
        {
            List<bl_gtli_company> list_of_company = new List<bl_gtli_company>();
               
            string connString = AppConfiguration.GetConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {

                    //Mysql command
                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "SP_Get_GTLI_Company_List_By_Company_Name";
                    myCommand.Parameters.AddWithValue("@Company_Name", company_name);

                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {

                        while (myReader.Read())
                        {
                            bl_gtli_company company = new bl_gtli_company();
                            company.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                            company.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
                            company.Company_Email = myReader.GetString(myReader.GetOrdinal("Company_Email"));
                            company.Company_Address = myReader.GetString(myReader.GetOrdinal("Company_Address"));
                            company.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));                         

                            list_of_company.Add(company);
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
                Log.AddExceptionToLog("Error in function [GetListOfCompanyByCompanyName] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return list_of_company;
        }

        //Function to retrieve list of all company
        public static List<bl_gtli_company> GetListOfCompany()
        {
            List<bl_gtli_company> list_of_company = new List<bl_gtli_company>();

            string connString = AppConfiguration.GetConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {

                    //Mysql command
                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "SP_Get_GTLI_Company_List";                  

                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {

                        while (myReader.Read())
                        {
                            bl_gtli_company company = new bl_gtli_company();
                            company.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                            company.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
                            company.Company_Email = myReader.GetString(myReader.GetOrdinal("Company_Email"));
                            company.Company_Address = myReader.GetString(myReader.GetOrdinal("Company_Address"));
                            company.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));
                            company.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                            company.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

                            list_of_company.Add(company);
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
                Log.AddExceptionToLog("Error in function [GetListOfCompany] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return list_of_company;
        }

        //Function to retrieve list of all company name
        public static ArrayList GetListOfCompanyName()
        {
            ArrayList list_of_company_name = new ArrayList();

            string connString = AppConfiguration.GetConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {

                    //Mysql command
                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "SP_Get_GTlI_Company_Name_List";

                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {

                        while (myReader.Read())
                        {
                            string company_name = "";

                            company_name = myReader.GetString(myReader.GetOrdinal("Company_Name"));

                            list_of_company_name.Add(company_name);
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
                Log.AddExceptionToLog("Error in function [GetListOfCompanyName] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return list_of_company_name;
        }

        //Update Company
        public static bool UpdateCompany(bl_gtli_company company)
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
                    myCommand.CommandText = "SP_Update_GTLI_Company";

                    myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company.GTLI_Company_ID);
                    myCommand.Parameters.AddWithValue("@Company_Name", company.Company_Name);
                    myCommand.Parameters.AddWithValue("@Company_Email", company.Company_Email);
                    myCommand.Parameters.AddWithValue("@Company_Address", company.Company_Address);
                    myCommand.Parameters.AddWithValue("@Type_Of_Business", company.Type_Of_Business);

                    myCommand.ExecuteNonQuery();

                    //Close connection
                    myConnection.Close();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                //Add error to log for analysis
                Log.AddExceptionToLog("Error in function [UpdateCompany] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return result;

        }

        //Check company
        public static bool CheckCompany(string company_name)
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
                    myCommand.CommandText = "SP_Check_GTLI_Company";

                    //Bind parameter to value received from the function caller
                    myCommand.Parameters.AddWithValue("@Company_Name", company_name);

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
                Log.AddExceptionToLog("Error in function [CheckCompany] in class [da_gtli_company]. Details: " + ex.Message);
            }

            return result;
        }

        //Function to delete company
        public static bool DeleteCompany(string company_id)
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
                    myCommand.CommandText = "SP_Delete_GTLI_Company";
                    myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [DeleteCompany] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return result;
        }

        //Get company ID by company name
        public static string GetCompanyIDByCompanyName(string company_name)
        {
            string company_id = "0";
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
                    myCommand.CommandText = "SP_Get_GTLI_Company_ID_By_Company_Name";

                    myCommand.Parameters.AddWithValue("@Company_Name", company_name);

                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (myReader.Read())
                        {
                            //If found row, return true & do the statement
                            if (myReader.HasRows)
                            {
                                company_id = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
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
                Log.AddExceptionToLog("Error in function [GetCompanyIDByCompanyName] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return company_id;
        }

        //Get company object by company ID
        public static bl_gtli_company GetObjCompanyByID(string company_id)
        {
            //Declare object
            bl_gtli_company myCompany = null;

            string connString = AppConfiguration.GetConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {
                    
                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "SP_Get_GTLI_Company_By_ID";
                    myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);

                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (myReader.Read())
                        {
                            //If found row, return true & do the statement
                            if (myReader.HasRows)
                            {
                                myCompany = new bl_gtli_company();
                                myCompany.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                                myCompany.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
                                myCompany.Company_Email = myReader.GetString(myReader.GetOrdinal("Company_Email"));
                                myCompany.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                                myCompany.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));
                                myCompany.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                                myCompany.Company_Address = myReader.GetString(myReader.GetOrdinal("Company_Address"));

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
                Log.AddExceptionToLog("Error function [GetObjCompanyByID] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return myCompany;
        }

        //Get company name by id
        public static string GetCompanyNameByID(string company_id)
        {
            string company_name = "";
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
                    myCommand.CommandText = "SP_Get_GTLI_Company_Name_By_ID";

                    myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);

                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (myReader.Read())
                        {
                            //If found row, return true & do the statement
                            if (myReader.HasRows)
                            {
                                company_name = myReader.GetString(myReader.GetOrdinal("Company_Name"));
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
                Log.AddExceptionToLog("Error in function [GetCompanyNameByID] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return company_name;
        }

        //Get list of all company Sorting
        public static List<bl_gtli_company> GetCompanyListSorting(bl_gtli_company blcompany)
        {
            List<bl_gtli_company> company_list = new List<bl_gtli_company>();
            bl_gtli_company myCompany = default(bl_gtli_company);
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
                    myCommand.CommandText = "SP_Get_GTLI_Company_List_Sorting";
                    myCommand.Parameters.AddWithValue("@Company_Name", blcompany.Company_Name);
                    myCommand.Parameters.AddWithValue("@Type_Of_Business", blcompany.Type_Of_Business);
                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (myReader.Read())
                        {
                            //If found row, return true & do the statement
                            if (myReader.HasRows)
                            {
                                myCompany = new bl_gtli_company();

                                myCompany.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                                myCompany.Company_Name = myReader.GetString(myReader.GetOrdinal("Company_Name"));

                                myCompany.Company_Email = myReader.GetString(myReader.GetOrdinal("Company_Email"));
                                myCompany.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                                myCompany.Type_Of_Business = myReader.GetString(myReader.GetOrdinal("Type_Of_Business"));
                                myCompany.Company_Address = myReader.GetString(myReader.GetOrdinal("Company_Address"));
                                myCompany.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

                                myCompany.Latest_Contact_Email = myReader.GetString(myReader.GetOrdinal("Company_Email"));
                                myCompany.Latest_Contact_Name = myReader.GetString(myReader.GetOrdinal("Contact_Name"));
                                myCompany.Latest_Contact_Phone = myReader.GetString(myReader.GetOrdinal("Contact_Phone"));


                                company_list.Add(myCompany);
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
                Log.AddExceptionToLog("Error in function [GetCompanyListSorting] in class [da_gtli_company]. Details: " + ex.Message);
            }
            return company_list;
        }
    #endregion
}