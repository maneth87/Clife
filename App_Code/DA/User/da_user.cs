using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

using System.Collections.Specialized;
using System.Configuration;
/// <summary>
/// Summary description for da_user
/// </summary>
public class da_user
{
	private static da_user mytitle = null;
    public da_user()
    {
        if (mytitle == null)
        {
            mytitle = new da_user();
        }

    }

    #region "Public Functions"

    //Function to get user id by user name
    public static string GetUserIDByUserName(string user_name)
    {
        string user_id = "";

        string connString = AppConfiguration.GetAccountConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_User_ID_By_User_Name", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@User_Name", user_name);
           
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    user_id = rdr.GetGuid(rdr.GetOrdinal("UserId")).ToString();
                         
                }

            }

        }
        return user_id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user_name">User name can be blank</param>
    /// <returns></returns>
    public static List<bl_user> GetUser(string user_name, string application_name)
    {
        List<bl_user> user_list = new List<bl_user>();
        try
        {
            DataTable tbl = new DB().GetData(AppConfiguration.GetAccountConnectionString(), "SP_Get_All_User_By_Application_Name", new string[,] { { "@username", user_name }, { "@user_type", "0" }, { "@status", "-1" }, { "@application_name", application_name } }, "da_user => GetUser(string user_name)");
            foreach (DataRow row in tbl.Rows)
            {
                user_list.Add(new bl_user() { UserId = row["userid"].ToString().Trim(), UserName = row["username"].ToString().Trim() });
            }
        }
        catch (Exception ex)
        {
            user_list = new List<bl_user>();
            Log.AddExceptionToLog("Error function [GetUser(string user_name)] in class [da_user], detail:" + ex.Message);
        }
        return user_list;
    }
	  //Function to get all user 
    public static List<bl_user> GetAllUserList(string username, string user_type,int status)
    {
        List<bl_user> user_list = new List<bl_user>();

        string connString = AppConfiguration.GetConnectionString_User();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_All_User";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@user_type", user_type);
                cmd.Parameters.AddWithValue("@status", status);
               
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        bl_user user = new bl_user();

                        user.UserId =rdr.GetGuid(rdr.GetOrdinal("UserId")).ToString();
                        user.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                        user.RoleId = rdr.GetGuid(rdr.GetOrdinal("RoleId")).ToString();
                        user.RoleName = rdr.GetString(rdr.GetOrdinal("RoleName"));
                        user.Email = rdr.GetString(rdr.GetOrdinal("Email"));
                        user.Approved = rdr.GetSqlBoolean(rdr.GetOrdinal("IsApproved")).IsTrue;

                        if (user.Approved == true)
                        {
                            user.IsApproved = "1";
                        }
                        else {

                            user.IsApproved = "0" ;
                        }

                        user.LockedOut = rdr.GetSqlBoolean(rdr.GetOrdinal("IsLockedOut")).IsTrue;

                        if (user.LockedOut == true)
                        {
                            user.IsLockedOut = "1";
                        }
                        else
                        {

                            user.IsLockedOut = "0";
                        }

                        user.CreateDate = rdr.GetDateTime(rdr.GetOrdinal("CreateDate"));
                        user.LastActivityDate = rdr.GetDateTime(rdr.GetOrdinal("LastActivityDate"));
                        user.LastLoginDate = rdr.GetDateTime(rdr.GetOrdinal("LastLoginDate"));

                        user_list.Add(user);

                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetAllUserList] in class [da_user]. Details: " + ex.Message);
        }

        return user_list;
    }

    /// <summary>
    /// Update in user account [userMemberShip]
    /// </summary>
    public static bool UpdateUserAccountMembership(bl_user  edit_user)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString_User();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_User_Membership";
            
            cmd.Parameters.AddWithValue("@user_id", edit_user.UserId);        
            cmd.Parameters.AddWithValue("@user_email", edit_user.Email);
            cmd.Parameters.AddWithValue("@user_approved",edit_user.IsApproved);
            cmd.Parameters.AddWithValue("@user_locked", edit_user.IsLockedOut);
            
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
                Log.AddExceptionToLog("Error in function [UpdateUserAccountMembership] in class [da_user]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update in user account [UserInRole]
    /// </summary>
    public static bool UpdateUserAccountUserInRole(string role_id,string user_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString_User();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_UserIn_Role";

            cmd.Parameters.AddWithValue("@user_id",user_id);
            cmd.Parameters.AddWithValue("@user_role", role_id);

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
                Log.AddExceptionToLog("Error in function [UpdateUserAccountUserInRole] in class [da_user]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static bool AddRole(bl_role role)
    {
      
       
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetAccountConnectionString(), "SP_ROLES_INSERT", new string[,] {
            {"@application_id", role.ApplicationID},
            {"@role_name", role.RoleName},
            {"@loweredrolename", role.LoweredRoleName},
            {"@description", role.Description}
            }, "da_user => AddRole(bl_role role)");
         
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error in function [AddRole(bl_role role)] in class [da_user]. Detail: " + ex.Message);

        }
        return result;
    }

    public static bool UpdateRole(bl_role role)
    {


        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetAccountConnectionString(), "SP_ROLES_UPDATE", new string[,] {
          {"@role_id", role.RoleID},
            {"@role_name", role.RoleName},
            {"@lowered_role_name", role.LoweredRoleName},
            {"@description", role.Description}
            }, "da_user => UpdateRole(bl_role role)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error in function [UpdateRole(bl_role role)] in class [da_user]. Detail: " + ex.Message);

        }
        return result;
    }
    public static List<bl_role> GetRole(string application_name)
    {
        List<bl_role> role_list = new List<bl_role>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetAccountConnectionString(), "SP_Get_Role_By_Application_Name", new string[,] { 
            {"@application_name", application_name}}, "da_user => GetRole(string application_name)");

            foreach (DataRow row in tbl.Rows)
            {
                role_list.Add(new bl_role()
                {
                    ApplicationID = row["applicationId"].ToString().Trim(),
                    Description = row["description"].ToString().Trim(),
                    RoleID = row["roleid"].ToString().Trim(),
                    RoleName = row["roleName"].ToString().Trim()
                });
            }
        }
        catch (Exception ex)
        {
            role_list = new List<bl_role>();
            Log.AddExceptionToLog("Error in function [GetRole(string application_name)] in class [da_user]. Detail: " + ex.Message);
        }
        return role_list;
    }
    public static string GetApplicationID(string application_name)
    {
        string app_id = "";
        try
        {
            DataTable tbl =  DataSetGenerator.Get_Data_Soure(AppConfiguration.GetAccountConnectionString(), "select ApplicationId from aspnet_Applications where ApplicationName='" + application_name + "'; ");
            foreach (DataRow row in tbl.Rows)
            {
                app_id = row["applicationid"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            app_id = "";
            Log.AddExceptionToLog("Error in function [GetApplicationID(string application_name)] in class [da_user]. Detail: " + ex.Message);
        }
        return app_id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="user_type"></param>
    /// <param name="status"></param>
    /// <param name="application_name"></param>
    /// <returns></returns>
    public static List<bl_user> GetAllUserList(string username, string user_type, int status, string application_name)
    {
        List<bl_user> user_list = new List<bl_user>();

        string connString = AppConfiguration.GetAccountConnectionString();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_All_User_By_Application_Name";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@user_type", user_type);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@application_name", application_name);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        bl_user user = new bl_user();

                        user.UserId = rdr.GetGuid(rdr.GetOrdinal("UserId")).ToString();
                        user.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                        user.RoleId = rdr.GetGuid(rdr.GetOrdinal("RoleId")).ToString();
                        user.RoleName = rdr.GetString(rdr.GetOrdinal("RoleName"));
                        user.Email = rdr.GetString(rdr.GetOrdinal("Email"));
                        user.Approved = rdr.GetSqlBoolean(rdr.GetOrdinal("IsApproved")).IsTrue;

                        if (user.Approved == true)
                        {
                            user.IsApproved = "1";
                        }
                        else
                        {

                            user.IsApproved = "0";
                        }

                        user.LockedOut = rdr.GetSqlBoolean(rdr.GetOrdinal("IsLockedOut")).IsTrue;

                        if (user.LockedOut == true)
                        {
                            user.IsLockedOut = "1";
                        }
                        else
                        {

                            user.IsLockedOut = "0";
                        }

                        user.CreateDate = rdr.GetDateTime(rdr.GetOrdinal("CreateDate"));
                        user.LastActivityDate = rdr.GetDateTime(rdr.GetOrdinal("LastActivityDate"));
                        user.LastLoginDate = rdr.GetDateTime(rdr.GetOrdinal("LastLoginDate"));

                        user_list.Add(user);

                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetAllUserList(string username, string user_type, int status, string application_name)] in class [da_user]. Details: " + ex.Message);
        }

        return user_list;
    }
    #endregion

    #region Password policy
    /// <summary>
    /// 
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="ApplicationName"></param>
    /// <returns></returns>
    public static bl_user GetUserByUser(string UserName, string ApplicationName)
    {
        int age = 0;
        bl_user user = new bl_user();
        try
        {
            DataTable tbl=new DataTable();
            DB db = new DB();
            tbl = db.GetData(AppConfiguration.GetAccountConnectionString(), "SP_GET_USER_BY_USER_NAME_APP_NAME", new string[,] {
            {"@user_name", UserName},{"@APPLICATION_NAME",ApplicationName}
            }, "da_user => GetUser(string UserName, string ApplicationName)");

            if (db.RowEffect > 0)
            {
                var r = tbl.Rows[0];
                user.UserId = r["UserId"].ToString();
                user.UserName = r["UserName"].ToString();
                user.RoleId = r["RoleId"].ToString();
                user.RoleName =r["RoleName"].ToString();
                user.Email = r["Email"].ToString();
                user.Approved = Convert.ToBoolean( r["IsApproved"].ToString() );
                user.IsApproved = user.Approved == true ? "1" : "0";
                user.LockedOut = Convert.ToBoolean(r["IsLockedOut"].ToString());
                user.IsLockedOut = user.LockedOut == true ? "1" : "0";
                user.CreateDate = Convert.ToDateTime(r["CreateDate"].ToString());
                user.LastActivityDate = Convert.ToDateTime(r["LastActivityDate"].ToString());
                user.LastLoginDate = Convert.ToDateTime(r["LastLoginDate"].ToString());
                user.LastPasswordChangedDate = Convert.ToDateTime(r["LastPasswordChangedDate"].ToString());
                user.FailedPasswordAttemptCount = Convert.ToInt32(r["FailedPasswordAttemptCount"].ToString());
            }

          
        }
        catch (Exception ex)
        {
            user = new bl_user();
            Log.AddExceptionToLog("Error function [] in class [da_user], detail: " + ex.Message + " => " + ex.StackTrace);
        }
        return user;
    }
    #endregion Password Policy

    
}