using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for da_relationship
/// </summary>
public class da_relationship
{
	private static da_relationship mytitle = null;
    public da_relationship()
	{
	  if (mytitle == null)
        {
            mytitle = new da_relationship();
        }
	}

    public static List<bl_relationship> GetRelationshipList()
    {
        List<bl_relationship> relationship_list = new List<bl_relationship>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Relationship_List", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_relationship relationship = new bl_relationship();
                    relationship.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                    relationship.Relationship_Khmer = rdr.GetString(rdr.GetOrdinal("Relationship_Khmer"));
                    //MessageBox.Show((rdr.GetOrdinal("Is_Clean_Case")).ToString());
                    //relationship.Is_Clean_Case = rdr.GetBoolean(rdr.GetOrdinal("Is_Clean_Case"));
                    //relationship.Is_Reserved = rdr.GetBoolean(rdr.GetOrdinal("Is_Reserved"));
                    relationship.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                    relationship.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                    relationship.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                    relationship_list.Add(relationship);
                }
            }
        }

        return relationship_list;
    }

    /// <summary>
    /// Insert into Ct_Relationship
    /// </summary>
    public static bool InsertRelationship(bl_relationship relationship)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Relationship";

            cmd.Parameters.AddWithValue("@Relationship", relationship.Relationship);
            if (relationship.Is_Clean_Case == true)
            { cmd.Parameters.AddWithValue("@Is_Clean_Case", 1); }
            else { cmd.Parameters.AddWithValue("@Is_Clean_Case", 0); }

            if (relationship.Is_Reserved == true)
            { cmd.Parameters.AddWithValue("@Is_Reserved", 1); }
            else { cmd.Parameters.AddWithValue("@Is_Reserved", 0); }
            
            cmd.Parameters.AddWithValue("@Created_On", relationship.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", relationship.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", relationship.Created_Note);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", relationship.Relationship_Khmer);

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
                Log.AddExceptionToLog("Error in function [InsertRelationship] in class [bl_relationship]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update Ct_Relationship
    /// </summary>
    public static bool UpdateRelationship(bl_relationship relationship)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Relationship";

            cmd.Parameters.AddWithValue("@Relationship", relationship.Relationship);
            if (relationship.Is_Clean_Case == true)
            { cmd.Parameters.AddWithValue("@Is_Clean_Case", 1); }
            else { cmd.Parameters.AddWithValue("@Is_Clean_Case", 0); }

            if (relationship.Is_Reserved == true)
            { cmd.Parameters.AddWithValue("@Is_Reserved", 1); }
            else { cmd.Parameters.AddWithValue("@Is_Reserved", 0); }

            cmd.Parameters.AddWithValue("@Created_On", relationship.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", relationship.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", relationship.Created_Note);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", relationship.Relationship_Khmer);

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
                Log.AddExceptionToLog("Error in function [UpdateRelationship] in class [bl_relationship]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Check Duplicate Relationship in Ct_Relationship
    /// </summary>
    public static bool GetRelationship_By_Relationship(bl_relationship relationship)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_Relationship";
            cmd.Parameters.AddWithValue("@Relationship", relationship.Relationship);
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
                Log.AddExceptionToLog("Error in function [GetRelationship_By_Relationship] in class [bl_relationship]. Details: " + ex.Message);
            }
        }
        return result;
    }


    /// <summary>
    /// Delete from Ct_Relationship
    /// </summary>
    public static bool DeleteRelationship(bl_relationship relationship)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Relationship";
            cmd.Parameters.AddWithValue("@Relationship", relationship.Relationship);
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
                Log.AddExceptionToLog("Error in function [DeleteRelationship] in class [bl_relationship]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Get relationship khmer by relationship param
    public static string GetRelationshipKhmer(string relationship)
    {
        string relationship_khmer = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Relationship_Khmer";

            cmd.Parameters.AddWithValue("@Relationship", relationship);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        relationship_khmer = rdr.GetString(rdr.GetOrdinal("Relationship_Khmer"));
                     
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetRelationshipKhmer] in class [da_relationship]. Details: " + ex.Message);
            }

        }
        return relationship_khmer;
    }


    /// <summary>
    /// Check Duplicate Relationship in Ct_Relationship
    /// </summary>
    public static string GetRelationship_By_Relationship(string Relationship, string Relationship_Kh)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_Relationship";
            cmd.Parameters.AddWithValue("@Relationship", Relationship);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();
            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = "Relationship En (" + Relationship + ")";
                }

                // result += GetRelationship_By_Relationship_Kh(Relationship,Relationship_Kh);

                if (result != "") { result += " have already existed"; }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetRelationship_By_Relationship] in class [bl_relationship]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string GetRelationship_By_Relationship_Kh(string Relationship, string Relationship_Kh)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = @"select Relationship_Kh from Ct_Relationship where lower(Relationship) <>@Relationship
                                    and  lower(Relationship_Kh) like N'" + Relationship_Kh + "'";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@Relationship", Relationship.ToLower());
            con.Open();
            try
            {
                result = cmd.ExecuteScalar().ToString();
                if (result != "")
                {
                    result = " Relationship Kh (" + Relationship_Kh + ")";
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetRelationship_By_Relationship_Kh] in class [bl_relationship]. Details: " + ex.Message);
            }
        }
        return result;
    }


   
}