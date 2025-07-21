using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_user_permission
/// </summary>
public class da_user_permission
{
	private static da_user_permission mytitle = null;
    public da_user_permission()
    {
        if (mytitle == null)
        {
            mytitle = new da_user_permission();
        }

    }

    #region "Public Functions"
    //Function to get user permission by user id and application_id
    public static bl_User_Permission GetUserPermissionByUserID(string application_id, string user_id)
    {
        bl_User_Permission user_permission = new bl_User_Permission();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_User_Permission_By_User_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;
                   
            cmd.Parameters.AddWithValue("@User_ID", user_id);
            cmd.Parameters.AddWithValue("@Application_ID", application_id);
            
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {                
                    user_permission.Read = rdr.GetInt32(rdr.GetOrdinal("Read"));
                    user_permission.Write = rdr.GetInt32(rdr.GetOrdinal("Write"));
                    user_permission.Delete = rdr.GetInt32(rdr.GetOrdinal("Delete"));                 
                }

            }

        }
        return user_permission;
    }

    //Function to check user id by application name
    public static bool CheckUserPermissionForThisApplication(string application_id, string user_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_User_Permission_For_This_Application", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@User_ID", user_id);
            cmd.Parameters.AddWithValue("@Application_ID", application_id);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    result = true;
                }

            }

        }
        return result;
    }

    #endregion

}