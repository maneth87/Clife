using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_policy_status_type
/// </summary>
public class da_policy_status_type
{
    private static da_policy_status_type mytitle = null;
    public da_policy_status_type()
    {
        if (mytitle == null)
        {
            mytitle = new da_policy_status_type();
        }
    }

    #region "Public Functions"

    /// <summary>
    /// Insert into Ct_Policy_Status_Type
    /// </summary>
    public static bool InsertPolicyStatus(bl_policy_status_type policy_status_type)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Status_Type";

            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_status_type.Policy_Status_Type_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Code", policy_status_type.Policy_Status_Code);
            cmd.Parameters.AddWithValue("@Detail", policy_status_type.Detail);
            cmd.Parameters.AddWithValue("@Terminated", policy_status_type.Terminated);
            cmd.Parameters.AddWithValue("@Disabled", policy_status_type.Disabled);
            cmd.Parameters.AddWithValue("@Is_Reserved", policy_status_type.Is_Reserved);
            cmd.Parameters.AddWithValue("@Created_On", policy_status_type.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_status_type.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_status_type.Created_Note);
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
                Log.AddExceptionToLog("Error in function [InsertPolicyStatus] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update in Ct_Policy_Status_Type 
    /// </summary>
    public static bool UpdatePolicyStatus(bl_policy_status_type policy_status_type)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Status_Type";

            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_status_type.Policy_Status_Type_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Code", policy_status_type.Policy_Status_Code);
            cmd.Parameters.AddWithValue("@Detail", policy_status_type.Detail);
            cmd.Parameters.AddWithValue("@Terminated", policy_status_type.Terminated);
            cmd.Parameters.AddWithValue("@Disabled", policy_status_type.Disabled);
            cmd.Parameters.AddWithValue("@Is_Reserved", policy_status_type.Is_Reserved);
            cmd.Parameters.AddWithValue("@Created_On", policy_status_type.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_status_type.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_status_type.Created_Note);
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
                Log.AddExceptionToLog("Error in function [UpdatePolicyStatus] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Dublicate Policy by Policy ID from Ct_Policy_Status_Type
    /// </summary>
    public static bool GetPolicy_By_Policy_ID(bl_policy_status_type policy_status_type, int check_id_code)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_Policy";
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_status_type.Policy_Status_Type_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Code", policy_status_type.Policy_Status_Code);
            cmd.Parameters.AddWithValue("@check_id_code", check_id_code);
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
                Log.AddExceptionToLog("Error in function [GetPolicy_By_Policy_ID] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Delete Policy ID from Ct_Policy_Status_Type
    /// </summary>
    public static bool DeletePolicyStatus(bl_policy_status_type policy_status_type)
    {
        bool result = false, check_policy_is_used = GetPolicy_IsUsed_By_Policy_ID(policy_status_type.Policy_Status_Type_ID);
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_By_Policy_ID";
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_status_type.Policy_Status_Type_ID);
            cmd.Connection = con;
            con.Open();
            try
            {
                if (check_policy_is_used == false)
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicy_By_Policy_ID] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Get Policy ID is being used from Ct_Policy_Status_Txn and Ct_Policy_Status
    /// </summary>
    private static bool GetPolicy_IsUsed_By_Policy_ID(string policy_status_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetPolicy_IsUsed_By_Policy_ID";
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_status_id);
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
                Log.AddExceptionToLog("Error in function [GetPolicy_IsUsed_By_Policy_ID] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string GetPolicy_By_Policy_ID(string Policy_Status_Type_ID, string Policy_Status_Code, int check_id_code)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_Policy";
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", Policy_Status_Type_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Code", Policy_Status_Code);
            cmd.Parameters.AddWithValue("@check_id_code", 0);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();
            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result += "Policy Status ID (" + Policy_Status_Type_ID.ToUpper() + ")";
                }

                result += GetPolicy_By_Policy_Code(Policy_Status_Type_ID, Policy_Status_Code, check_id_code);

                if (result != "") { result += " have already existed"; }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicy_By_Policy_ID] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string GetPolicy_By_Policy_Code(string Policy_Status_Type_ID, string Policy_Status_Code, int check_id_code)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_Policy";
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", Policy_Status_Type_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Code", Policy_Status_Code);
            cmd.Parameters.AddWithValue("@check_id_code", check_id_code);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();
            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (check_id_code == 1)
                    { result += "Policy Status Code (" + Policy_Status_Code.ToUpper() + ")"; }
                    else { result += "Policy Status Code (" + Policy_Status_Code.ToUpper() + ") has already existed"; }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicy_By_Policy_Code] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string GetPolicy_By_Policy_Code_Edit(string Policy_Status_Type_ID, string Policy_Status_Code)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = @"select Policy_Status_Code from Ct_Policy_Status_Type
                                where lower(Policy_Status_Type_ID) <>@Policy_Status_Type_ID and lower(Policy_Status_Code)=@Policy_Status_Code";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", Policy_Status_Type_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Code", Policy_Status_Code.ToLower());
            cmd.Parameters.AddWithValue("@check_id_code", 3);
            cmd.Connection = con;

            con.Open();
            try
            {
                result = cmd.ExecuteScalar().ToString();

                if (result != "")
                {
                    result = "Policy Status Code (" + Policy_Status_Code.ToUpper() + ") has already existed";
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicy_By_Policy_Code_Edit] in class [bl_policy_status_type]. Details: " + ex.Message);
            }
        }
        return result;
    }

    #endregion
}