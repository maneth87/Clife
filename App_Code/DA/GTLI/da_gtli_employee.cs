using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_employee
/// </summary>
public class da_gtli_employee
{
    private static da_gtli_employee mytitle = null;
    public da_gtli_employee()
	 {
        if (mytitle == null)
        {
            mytitle = new da_gtli_employee();
		}
	 }

     //Get last certificate no by company id
     public static int GetLastCertificateNoByCompanyID(string gtli_company_id)
     {
         int certificate_number = 0;
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
                 myCommand.CommandText = "SP_Get_GTLI_Last_Certificate_Number_By_Company";

                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", gtli_company_id);
               
                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             certificate_number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
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
             Log.AddExceptionToLog("Error in function [GetLastCertificateNoByCompanyID] in class [da_gtli_employee]. Details: " + ex.Message);
         }
         return certificate_number;
     }

     //Check employee by company_id, employee_name, gender, dob
     public static bool CheckGTLIEmployee(string employee_name, string gender, System.DateTime dob, string company_id)
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
                 myCommand.CommandText = "SP_Check_GTLI_Employee";

                 //Bind parameter to value received from the function caller

                 myCommand.Parameters.AddWithValue("@DOB", dob);
                 myCommand.Parameters.AddWithValue("@Employee_Name", employee_name);
                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                 myCommand.Parameters.AddWithValue("@Gender", gender);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true
                         if (myReader.HasRows)
                         {
                             result = true;
                             break; // TODO: might not be correct. Was : Exit While
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
             Log.AddExceptionToLog("Error in function [CheckGTLIEmployee] in class [da_gtli_employee]. Details: " + ex.Message);
         }

         return result;
     }

     //Check employee by company_id, employee_name, gender, dob, plan_id, policy_id
     public static bool CheckGTLIEmployeeForResign(string employee_name, string gender, System.DateTime dob, string company_id, string plan_id, string policy_id)
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
                 myCommand.CommandText = "SP_Check_GTLI_Employee_For_Resign";

                 //Bind parameter to value received from the function caller

                 myCommand.Parameters.AddWithValue("@DOB", dob);
                 myCommand.Parameters.AddWithValue("@Employee_Name", employee_name.ToLower());
                 myCommand.Parameters.AddWithValue("@Gender", gender);
                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                 myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", plan_id);
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true
                         if (myReader.HasRows)
                         {
                             result = true;
                             break; // TODO: might not be correct. Was : Exit While
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
             Log.AddExceptionToLog("Error in function [CheckGTLIEmployeeForResign] in class [da_gtli_employee]. Details: " + ex.Message);
         }

         return result;
     }

     //Check customer employee_name, gender, dob in Ct_Customer
     public static bool CheckGTLIEmployeeInCtCustomer(string first_name, string last_name, string gender, System.DateTime dob)
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
                 myCommand.CommandText = "SP_Check_GTLI_Employee_In_Ct_Customer";

                 //Bind parameter to value received from the function caller

                 myCommand.Parameters.AddWithValue("@DOB", dob);
                 myCommand.Parameters.AddWithValue("@First_Name", first_name.ToLower());
                 myCommand.Parameters.AddWithValue("@Last_Name", last_name.ToLower());

                 if (gender == "M")
                 {
                     myCommand.Parameters.AddWithValue("@Gender", 1);
                 }
                 else
                 {
                     myCommand.Parameters.AddWithValue("@Gender", 0);
                 }                

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true
                         if (myReader.HasRows)
                         {
                             result = true;
                             break; // TODO: might not be correct. Was : Exit While
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
             Log.AddExceptionToLog("Error in function [CheckGTLIEmployeeInCtCustomer] in class [da_gtli_employee]. Details: " + ex.Message);
         }

         return result;
     }

     //get ct customer id employee_name, gender, dob
     public static string GetCtCustomerID(string first_name, string last_name, string gender, System.DateTime dob)
     {
         string ct_customer_id = "";
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
                 myCommand.CommandText = "SP_Get_Ct_Customer_ID_By_Name_DOB_Gender";

                 //Bind parameter to value received from the function caller

                 myCommand.Parameters.AddWithValue("@DOB", dob);
                 myCommand.Parameters.AddWithValue("@First_Name", first_name.ToLower());
                 myCommand.Parameters.AddWithValue("@Last_Name", last_name.ToLower());
                 if (gender == "M")
                 {
                     myCommand.Parameters.AddWithValue("@Gender", 1);
                 }
                 else
                 {
                     myCommand.Parameters.AddWithValue("@Gender", 0);
                 }
                

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true
                         if (myReader.HasRows)
                         {
                             ct_customer_id = myReader.GetString(myReader.GetOrdinal("Customer_ID"));
                            
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
             Log.AddExceptionToLog("Error in function [GetCtCustomerID] in class [da_gtli_employee]. Details: " + ex.Message);
         }

         return ct_customer_id;
     }

     //Add new gtli employee
     public static bool InsertGTLIEmployee(bl_gtli_employee employee)
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
                 myCommand.CommandText = "SP_Insert_GTLI_Employee";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", employee.GTLI_Certificate_ID);
                 myCommand.Parameters.AddWithValue("@Employee_ID", employee.Employee_ID);      
                 myCommand.Parameters.AddWithValue("@Employee_Name", employee.Employee_Name);
                 myCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                 myCommand.Parameters.AddWithValue("@DOB", employee.DOB);
                 myCommand.Parameters.AddWithValue("@Position", employee.Position);
                 myCommand.Parameters.AddWithValue("@Customer_Status", employee.Customer_Status);
         
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
             Log.AddExceptionToLog("Error function [InsertGTLIEmployee] in class [da_gtli_employee]. Details: " + ex.Message);
         }
         //Return result to the function caller
         //If <true> means the function has insertd new article successfully
         return result;
     }

     //get list of employee by gtli premium id
    public static ArrayList GetListOfEmployeeByGTLIPremiumID(string gtli_premium_id)
     {
         bl_gtli_employee gtli_employee = null;
         ArrayList gtli_employee_list = new ArrayList();

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {
                 //Get premium rate by applicant age, policy year, and product ID

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Employee_List_By_GTLI_Premium_ID";

                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         gtli_employee = new bl_gtli_employee();
                         gtli_employee.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                         gtli_employee.Certificate_Number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                         gtli_employee.Employee_ID = myReader.GetString(myReader.GetOrdinal("Employee_ID"));
                         gtli_employee.Employee_Name = myReader.GetString(myReader.GetOrdinal("Employee_Name"));
                         gtli_employee.Gender = myReader.GetString(myReader.GetOrdinal("Gender"));
                         gtli_employee.DOB = myReader.GetDateTime(myReader.GetOrdinal("DOB"));
                         gtli_employee.Position = myReader.GetString(myReader.GetOrdinal("Position"));
                         gtli_employee.Customer_Status = myReader.GetInt16(myReader.GetOrdinal("Customer_Status"));  
                       
                         gtli_employee_list.Add(gtli_employee);
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
             Log.AddExceptionToLog("Error in function [GetListOfEmployeeByGTLIPremiumID], class [da_gtli_employee]. Details: " + ex.Message);
         }

         return gtli_employee_list;
     }
   
     //get employee by company_id, plan_id, employee_name, gender, dob
     public static bl_gtli_employee GetGTLIEmployeeForResign(string employee_name, string gender, System.DateTime dob, string company_id, string plan_id, string gtli_policy_id)
     {
         bl_gtli_employee mygtliemployee = new bl_gtli_employee();
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
                 myCommand.CommandText = "SP_Get_GTLI_Employee_For_Resign";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@DOB", dob);
                 myCommand.Parameters.AddWithValue("@Employee_Name", employee_name.ToLower());
                 myCommand.Parameters.AddWithValue("@Gender", gender);
                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);             
                 myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", plan_id);
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true
                         if (myReader.HasRows)
                         {

                             mygtliemployee.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                             mygtliemployee.Employee_ID = myReader.GetString(myReader.GetOrdinal("Employee_ID"));
                             mygtliemployee.Employee_Name = myReader.GetString(myReader.GetOrdinal("Employee_Name"));
                             mygtliemployee.Gender = myReader.GetString(myReader.GetOrdinal("Gender"));
                             mygtliemployee.DOB = myReader.GetDateTime(myReader.GetOrdinal("DOB"));
                             mygtliemployee.Position = myReader.GetString(myReader.GetOrdinal("Position"));
                             mygtliemployee.Customer_Status = myReader.GetInt16(myReader.GetOrdinal("Customer_Status"));
                             mygtliemployee.Certificate_Number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                        
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
             Log.AddExceptionToLog("Error in function [GetGTLIEmployeeForResign] in class [da_gtli_employee]. Details: " + ex.Message);
         }

         return mygtliemployee;
     }

     //Get list of active employee
     public static ArrayList GetListOfActiveEmployee(string gtli_policy_id)
     {
         bl_gtli_employee gtli_employee = null;
         ArrayList myArraylist = new ArrayList();

         string connString = AppConfiguration.GetConnectionString();

         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {
                                
                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_List_Of_Active_Employee";
                
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         gtli_employee = new bl_gtli_employee();
                         gtli_employee.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                         gtli_employee.Employee_ID = myReader.GetString(myReader.GetOrdinal("Employee_ID"));
                         gtli_employee.Employee_Name = myReader.GetString(myReader.GetOrdinal("Employee_Name"));
                         gtli_employee.Gender = myReader.GetString(myReader.GetOrdinal("Gender"));
                         gtli_employee.DOB = myReader.GetDateTime(myReader.GetOrdinal("DOB"));
                         gtli_employee.Position = myReader.GetString(myReader.GetOrdinal("Position"));
                         gtli_employee.Customer_Status = myReader.GetInt16(myReader.GetOrdinal("Customer_Status"));
                         gtli_employee.Certificate_Number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                         gtli_employee.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                         gtli_employee.Sum_Insured = myReader.GetString(myReader.GetOrdinal("Sum_Insured"));

                         myArraylist.Add(gtli_employee);
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
             Log.AddExceptionToLog("Error in function [GetListOfActiveEmployee], class [da_gtli_employee]. Details: " + ex.Message);
         }

         return myArraylist;
     }

     //get employee by id
     public static bl_gtli_employee GetEmployeeByID(string gtli_certificate_id)
     {
         bl_gtli_employee gtli_employee = new bl_gtli_employee();
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
                 myCommand.CommandText = "SP_Get_GTLI_Employee_By_ID";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true
                         if (myReader.HasRows)
                         {

                             gtli_employee.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                             gtli_employee.Employee_ID = myReader.GetString(myReader.GetOrdinal("Employee_ID"));
                             gtli_employee.Employee_Name = myReader.GetString(myReader.GetOrdinal("Employee_Name"));
                             gtli_employee.Gender = myReader.GetString(myReader.GetOrdinal("Gender"));
                             gtli_employee.DOB = myReader.GetDateTime(myReader.GetOrdinal("DOB"));
                             gtli_employee.Position = myReader.GetString(myReader.GetOrdinal("Position"));
                             gtli_employee.Customer_Status = myReader.GetInt16(myReader.GetOrdinal("Customer_Status"));
                             gtli_employee.Certificate_Number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                                                       
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
             Log.AddExceptionToLog("Error in function [GetEmployeeByID] in class [da_gtli_employee]. Details: " + ex.Message);
         }

         return gtli_employee;
     }

     //Update employee status
     public static void UpdateStatus(string gtli_certificate_id, Int16 status)
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
                 myCommand.CommandText = "SP_Update_GTLI_Employee_Status";

                 myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);

                 myCommand.Parameters.AddWithValue("@Customer_Status", status);
                 myCommand.ExecuteNonQuery();

                 //Close connection
                 myConnection.Close();
             }

         }
         catch (Exception ex)
         {
             //Add error to log for analysis
             Log.AddExceptionToLog("Error in function [UpdateStatus] in class [da_gtli_employee]. Details: " + ex.Message);
         }

     }

     //Function to delete gtli employee by premium id
     public static bool DeleteGTLIEmployeeByGTLIPremiumID(string gtli_premium_id)
     {
         bool bolresult = false;
         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Delete_GTLI_Employee_By_GTLI_Premium_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                 myCommand.ExecuteNonQuery();
                 myConnection.Close();
                 bolresult = true;
             }
         }
         catch (Exception ex)
         {
             //Add error to log 
             Log.AddExceptionToLog("Error in function [DeleteGTLIEmployeeByGTLIPremiumID] in class [da_gtli_employee]. Details: " + ex.Message);
         }
         return bolresult;
     }

     //Function to delete ct_customer_gtli_employee by premium id
     public static bool DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(string gtli_premium_id)
     {
         bool bolresult = false;
         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Delete_Customer_GTLI_Employee_By_GTLI_Premium_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                 myCommand.ExecuteNonQuery();
                 myConnection.Close();
                 bolresult = true;
             }
         }
         catch (Exception ex)
         {
             //Add error to log 
             Log.AddExceptionToLog("Error in function [DeleteCtCustomerGTLIEmployeeByGTLIPremiumID] in class [da_gtli_employee]. Details: " + ex.Message);
         }
         return bolresult;
     }


     //Get last certificate no by policy id
     public static int GetLastCertificateNoByPolicyID(string gtli_policy_id)
     {
         int certificate_number = 0;
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
                 myCommand.CommandText = "SP_Get_GTLI_Last_Certificate_Number_By_GTLI_Policy_ID";

                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             certificate_number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
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
             Log.AddExceptionToLog("Error in function [GetLastCertificateNoByPolicyID] in class [da_gtli_employee]. Details: " + ex.Message);
         }
         return certificate_number;
     }
}