using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_underwrite
/// </summary>
public class da_underwrite_code
{
	 private static da_underwrite_code mytitle = null;
     public da_underwrite_code()
    {
        if (mytitle == null)
        {
            mytitle = new da_underwrite_code();
        }
    }

     /// <summary>
     /// Insert into Ct_Underwrite_Table
     /// </summary>
     public static bool InsertUnderwrite_Code(bl_underwrite_code underwrite_code)
     {
         bool result = false;
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_Insert_Underwrite_Code";

             cmd.Parameters.AddWithValue("@Status_Code", underwrite_code.Status_Code);
             cmd.Parameters.AddWithValue("@Detail", underwrite_code.Detail);

             if (underwrite_code.Is_Inforce == true)
             { cmd.Parameters.AddWithValue("@Is_Inforce", 1); }
             else { cmd.Parameters.AddWithValue("@Is_Inforce", 0); }

             if (underwrite_code.Is_Reserved == true)
             { cmd.Parameters.AddWithValue("@Is_Reserved", 1); }
             else { cmd.Parameters.AddWithValue("@Is_Reserved", 0); }
             
             cmd.Parameters.AddWithValue("@Created_On", underwrite_code.Created_On);
             cmd.Parameters.AddWithValue("@Created_By", underwrite_code.Created_By);
             cmd.Parameters.AddWithValue("@Created_Note", underwrite_code.Created_Note);
             cmd.Connection = con;
             con.Open();
             try
             {
                 cmd.ExecuteNonQuery();
                 result = true;
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [InsertUnderwrite_Code] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }

     /// <summary>
     /// Update Ct_Underwrite_Table
     /// </summary>
     public static bool UpdateUnderwrite_Code(bl_underwrite_code underwrite_code)
     {
         bool result = false;
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_Update_Underwrite_Code";

             cmd.Parameters.AddWithValue("@Status_Code", underwrite_code.Status_Code);
             cmd.Parameters.AddWithValue("@Detail", underwrite_code.Detail);

             if (underwrite_code.Is_Inforce == true)
             { cmd.Parameters.AddWithValue("@Is_Inforce", 1); }
             else { cmd.Parameters.AddWithValue("@Is_Inforce", 0); }

             if (underwrite_code.Is_Reserved == true)
             { cmd.Parameters.AddWithValue("@Is_Reserved", 1); }
             else { cmd.Parameters.AddWithValue("@Is_Reserved", 0); }

             cmd.Parameters.AddWithValue("@Created_On", underwrite_code.Created_On);
             cmd.Parameters.AddWithValue("@Created_By", underwrite_code.Created_By);
             cmd.Parameters.AddWithValue("@Created_Note", underwrite_code.Created_Note);
             cmd.Connection = con;
             con.Open();
             try
             {
                 cmd.ExecuteNonQuery();
                 result = true;
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [UpdateUnderwrite_Code] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }

     /// <summary>
     /// Check Duplicate Status Code from Ct_Underwrite_Table
     /// </summary>
     public static bool GetUnderwrite_By_Status_Code(bl_underwrite_code underwrite_code, int check_id_detail)
     {
         bool result = false;
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_Check_Duplicate_Status_Code";
             cmd.Parameters.AddWithValue("@Status_Code", underwrite_code.Status_Code);
             cmd.Parameters.AddWithValue("@Detail",underwrite_code.Detail);
             cmd.Parameters.AddWithValue("@check_id_detail", check_id_detail);
             cmd.Connection = con;
             DataTable dt = new DataTable();
             SqlDataAdapter dap = new SqlDataAdapter(cmd);

             con.Open();
             try
             {
                 dap.Fill(dt);
                 if (dt.Rows.Count > 0)
                 {
                     result = true;
                 }
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [GetUnderwrite_By_Status_Code] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }

     /// <summary>
     /// Delete from Ct_Underwrite_Table
     /// </summary>
     public static bool DeleteUnderwrite_Code(bl_underwrite_code underwrite_code)
     {
         bool result = false, check_status_code_is_used = GetStatus_Coude_By_Status_Code(underwrite_code.Status_Code); 
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_Delete_Underwrite_By_Status_Code";
             cmd.Parameters.AddWithValue("@Status_Code", underwrite_code.Status_Code);
             cmd.Connection = con;
             con.Open();
             try
             {
                 if (check_status_code_is_used == false)
                 {
                     cmd.ExecuteNonQuery();
                     result = true;
                 }
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [DeleteUnderwrite_Code] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }


     /// <summary>
     /// Get Status Code is being used from Ct_Underwrite_Table
     /// </summary>
     public static bool GetStatus_Coude_By_Status_Code(string status_code)
     {
         bool result = false; 
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_GetUnderwrite_IsUsed_By_Status_Code";
             cmd.Parameters.AddWithValue("@Status_Code", status_code);
             cmd.Connection = con;
             DataTable dt = new DataTable();
             SqlDataAdapter dap = new SqlDataAdapter(cmd);

             con.Open();
             try
             {
                 dap.Fill(dt);
                 if (dt.Rows.Count > 0)
                 {
                     result = true;
                 }
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [GetUnderwrite_By_Status_Code] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }

     public static string GetUnderwrite_By_Status_Code(string Status_Code, string Detail, int check_id_detail)
     {
         string result = "";
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_Check_Duplicate_Status_Code";
             cmd.Parameters.AddWithValue("@Status_Code", Status_Code.ToLower());
             cmd.Parameters.AddWithValue("@Detail", Detail.ToLower());
             cmd.Parameters.AddWithValue("@check_id_detail", 0);
             cmd.Connection = con;
             DataTable dt = new DataTable();
             SqlDataAdapter dap = new SqlDataAdapter(cmd);

             con.Open();
             try
             {
                 dap.Fill(dt);
                 if (dt.Rows.Count > 0)
                 {
                     result = "The status code (" + Status_Code.ToUpper() + ")";
                 }

                 result += GetUnderwrite_By_Status_Code_Detail(Status_Code.ToLower(), Detail.ToLower(), check_id_detail);

                 if (result != "") { result += " have already existed"; }
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [GetUnderwrite_By_Status_Code] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }

     public static string GetUnderwrite_By_Status_Code_Detail(string Status_Code, string Detail, int check_id_detail)
     {
         string result = "";
         string connString = AppConfiguration.GetConnectionString();
         using (SqlConnection con = new SqlConnection(connString))
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_Check_Duplicate_Status_Code";
             cmd.Parameters.AddWithValue("@Status_Code", Status_Code.ToLower());
             cmd.Parameters.AddWithValue("@Detail", Detail.ToLower());
             cmd.Parameters.AddWithValue("@check_id_detail", check_id_detail);
             cmd.Connection = con;
             DataTable dt = new DataTable();
             SqlDataAdapter dap = new SqlDataAdapter(cmd);

             con.Open();
             try
             {
                 dap.Fill(dt);
                 if (dt.Rows.Count > 0)
                 {
                     if (check_id_detail == 1)
                     { result = "The Detail (" + Detail.ToUpper() + ")"; }
                     else { result = "The Detail (" + Detail.ToUpper() + ") has already existed"; }
                 }
             }
             catch (Exception ex)
             {
                 //Add error to log 
                 Log.AddExceptionToLog("Error in function [GetUnderwrite_By_Status_Code_Detail] in class [bl_underwrite_code]. Details: " + ex.Message);
             }
         }
         return result;
     }



}