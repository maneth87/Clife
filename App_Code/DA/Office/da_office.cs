using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

/// <summary>
/// Summary description for da_office_table
/// </summary>
public class da_office
{
    private static da_office mytitle = null;
    public da_office()
    {
        if (mytitle == null)
        {
            mytitle = new da_office();
        }
    }

    #region "Public Functions"
    
    /// <summary>
    /// Insert into Ct_Office
    /// </summary>
    public static bool InsertOffice(bl_office office)
    {        
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Office";

            cmd.Parameters.AddWithValue("@Office_ID", office.Office_ID);
            cmd.Parameters.AddWithValue("@Detail", office.Detail);
            cmd.Parameters.AddWithValue("@Status", office.Status);
            cmd.Parameters.AddWithValue("@Created_On", office.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", office.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", office.Created_Note);
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
                Log.AddExceptionToLog("Error in function [InsertOffice] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update in Ct_Office
    /// </summary>
    public static bool UpdateOfficeTable(bl_office office_table)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Office";
            //cmd.Parameters.AddWithValue("@Old_Office_ID", office_table.Old_Office_ID);
            cmd.Parameters.AddWithValue("@Office_ID", office_table.Office_ID);
            cmd.Parameters.AddWithValue("@Detail", office_table.Detail);
            cmd.Parameters.AddWithValue("@Status", office_table.Status);
            cmd.Parameters.AddWithValue("@Created_On", office_table.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", office_table.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", office_table.Created_Note);
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
                Log.AddExceptionToLog("Error in function [UpdateOfficeTable] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Delete from Ct_Office
    /// </summary>
    public static bool DeleteOffice_Record(bl_office office_table)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Office_By_Office_ID";
            cmd.Parameters.AddWithValue("@Office_ID", office_table.Office_ID);
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
                Log.AddExceptionToLog("Error in function [DeleteOffice_Record] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Check Office ID that is being used from Ct_Office
    /// </summary>
    public static bool GetOffice_ID_IsUsed_By_Office_ID(bl_office office_table)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_OfficeID_IsUsed_By_Office_ID";
            cmd.Parameters.AddWithValue("@Office_ID", office_table.Office_ID);
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
                Log.AddExceptionToLog("Error in function [GetOffice_ID_IsUsed_By_Office_ID] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Check Duplicate Office ID from Ct_Office
    /// </summary>
    public static bool GetOffice_By_Office_ID(bl_office office_table, int check_id_detail)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_OfficeCode_Detail";
            cmd.Parameters.AddWithValue("@Office_ID", office_table.Office_ID);
            cmd.Parameters.AddWithValue("@Detail", office_table.Detail);
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
                Log.AddExceptionToLog("Error in function [GetOffice_By_Office_ID] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string GetOffice_By_Office_ID(string Office_ID, string Detail, int check_id_detail)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_OfficeCode_Detail";
            cmd.Parameters.AddWithValue("@Office_ID", Office_ID);
            cmd.Parameters.AddWithValue("@Detail", Detail);
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
                    result = "Office Code (" + Office_ID.ToUpper() + ")";
                }

                result += GetOffice_By_Office_Detail(Office_ID, Detail, check_id_detail);

                if (result != "")
                {
                    result += " have already existed";
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetOffice_By_Office_ID] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string GetOffice_By_Office_Detail(string Office_ID, string Detail, int check_id_detail)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_OfficeCode_Detail";
            cmd.Parameters.AddWithValue("@Office_ID", Office_ID);
            cmd.Parameters.AddWithValue("@Detail", Detail);
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
                    { result = "Office Detail (" + Detail.ToUpper() + ")"; }
                    else { result = "Office Detail (" + Detail.ToUpper() + ") has already existed"; }
                    
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetOffice_By_Office_Detail] in class [bl_office]. Details: " + ex.Message);
            }
        }
        return result;
    }

  

    #endregion

}