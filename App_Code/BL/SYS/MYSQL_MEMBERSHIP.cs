using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
/// <summary>
/// Summary description for MYSQL_MEMBERSHIP
/// </summary>
public class MYSQL_MEMBERSHIP
{
    public MYSQL_MEMBERSHIP()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string ApplicationName { get { return _appName(); } }
    public static int ApplicationId { get { return _appId(); } }
    //public static MembershipProvider Provider { get { return Membership.Providers["MySqlMembershipProvider"]; } }
    //public static RoleProvider RoleProvider { get { return Roles.Providers["MySqlRoleProvider"]; } }

    /*camlife portal*/
    public static MembershipProvider Provider { get { return Membership.Providers["CAMLIFEPORTAL"]; } }
    public static RoleProvider RoleProvider { get { return Roles.Providers["CAMLIFEPORTAL"]; } }

    private static string _appName()
    {
        return Provider.ApplicationName;

    }
    private static int _appId()
    {
        try
        {
            //MySqlDB db = new MySqlDB(AppConfiguration.GetCamlifeMidleWareConnectionString());
            MySqlDB db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.Parameters = new string[,] { { "v_app_name", _appName() } };
            db.ProcedureName = "my_aspnet_applications_get";
            db.GenerateData();
            if (db.GenerateDataStatus)
            {
                if (db.DataCount > 0)
                {
                    return Convert.ToInt32(db.Data.Rows[0]["id"].ToString());
                }
                else { return -1; }
            }
            else { return -1; }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [_appId()] in class [MYSQL_MEMBERSHIP], detail: " + ex.Message);
            return -1;
        }
    }

    public static List<bl_user> GetUser()
    {
        List<bl_user> uList = new List<bl_user>();
        try
        {
            MySqlDB db = new MySqlDB();
            //db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ConnectionString = AppConfiguration.GetCamlifePortalConnectionString();
            db.Parameters = new string[,] { { "v_app_name", _appName() } };
            db.ProcedureName = "my_aspnet_users_get_userinfo";
            db.GenerateData();

            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {

                    foreach (DataRow r in db.Data.Rows)
                    {
                        uList.Add(new bl_user()
                        {
                            UserId = r["userid"].ToString(),
                            UserName = r["username"].ToString(),
                            Email = r["email"].ToString(),
                            CreateDate = Convert.ToDateTime(r["creationdate"].ToString()),
                            IsApproved = r["isapproved"].ToString(),
                            IsLockedOut = r["islockedout"].ToString(),
                            Approved = r["isapproved"].ToString() == "1" ? true : false,
                            LockedOut = r["islockedout"].ToString() == "1" ? true : false,
                            LastLoginDate = Convert.ToDateTime(r["lastlogindate"].ToString()),
                            LastActivityDate = Convert.ToDateTime(r["lastactivitydate"].ToString()),
                            LastPasswordChangedDate = Convert.ToDateTime(r["lastpasswordchangeddate"].ToString()),
                            FailedPasswordAttemptCount = Convert.ToInt32(r["failedPasswordAttemptCount"].ToString()),
                            RoleId = r["roleid"].ToString(),
                            RoleName = r["rolename"].ToString()
                        });


                    }
                }
                else
                {
                    uList = new List<bl_user>();
                }

            }

        }
        catch (Exception ex)
        {
            uList = new List<bl_user>();
            Log.AddExceptionToLog("Error function [GetUser()] in class [MYSQL_MEMBERSHIP], detail: " + ex.Message);
        }
        return uList;

    }

    public static List<bl_user> GetUser(string userName, int roleId, int isActive)
    {
        List<bl_user> filterList = new List<bl_user>();
        List<bl_user> filterUser = new List<bl_user>();
        List<bl_user> filterRole = new List<bl_user>();
        List<bl_user> filterActive = new List<bl_user>();
        List<bl_user> lUser = GetUser();
        if (userName != "")
        {
            foreach (bl_user u in lUser.Where(_ => _.UserName.Contains(userName)))
            {
                filterUser.Add(u);
            }
            filterList = filterUser;
        }
        else
        {
            filterList = lUser;
        }

        if (roleId > 0)
        {
            foreach (bl_user u in filterList.Where(_ => _.RoleId == roleId + ""))
            {
                filterRole.Add(u);
            }
            filterList = filterRole;
        }
        if (isActive >= 0)
        {
            foreach (bl_user u in filterList.Where(_ => _.IsApproved == (isActive == 1 ? "True" : "False")))
            {
                filterActive.Add(u);
            }
            filterList = filterActive;
        }
        return filterList;
    }

    public static bool UnlockedUser(int userId, string comment)
    {

        try
        {
            MySqlDB db = new MySqlDB();
            //db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ConnectionString = AppConfiguration.GetCamlifePortalConnectionString();
            db.Parameters = new string[,] { { "v_comment", comment }, { "i_userid", userId + "" } };
            db.ProcedureName = "my_aspnet_users_unlocked";
            db.Execute();
            return db.ExecuteStatus;

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error function [UnlockedUser(int userId, string comment)] in class [MYSQL_MEMBERSHIP], detail: " + ex.Message);
            return false;
        }

    }

    public static bool CreateUser(string userName, string password, string email, string roleName)
    {
        System.Guid proKey;
        System.Web.Security.MembershipCreateStatus status;
        proKey = System.Guid.NewGuid();
        Provider.CreateUser(userName, password, email, "", "", true, proKey, out status);
        if (status == System.Web.Security.MembershipCreateStatus.Success)
        {
            RoleProvider.AddUsersToRoles(new string[] { userName }, new string[] { roleName });
           
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId">Old role id </param>
    /// <param name="email"></param>
    /// <param name="isApproved"></param>
    /// <param name="isLocked"></param>
    /// <param name="newRoleId"></param>
    /// <returns></returns>
    public static bool UpdateUserRole(int userId, int roleId, string email, int isApproved, int isLocked, int newRoleId)
    {
        try
        {
            MySqlDB db = new MySqlDB();
            //db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            string conString = AppConfiguration.GetCamlifePortalConnectionString();
            db.ConnectionString = conString;// AppConfiguration.GetCamlifePortalConnectionString();
            db.Parameters = new string[,] { { "i_user_id", userId+"" }, { "i_role_id", roleId+"" } , { "i_new_role_id", newRoleId + "" }};
            db.ProcedureName = "my_aspnet_usersinroles_update";
            db.Execute();

            db.ConnectionString = conString;// AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "i_user_id", userId+"" }, { "i_isApproved", isApproved+"" } , { "i_isLocked", isLocked + "" }, { "v_email", email  }};
            db.ProcedureName = "my_aspnet_membership_update";
            db.Execute();

            return db.ExecuteStatus;

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error function [UpdateUserRole(int userId, int roleId, string email, int isApproved, int isLocked, int newRoleId)] in class [MYSQL_MEMBERSHIP], detail: " + ex.Message);
            return false;
        }
      
    }

    public static bool ResetPassword(string userName, string newPassword)
    {
        MembershipUser u = Provider.GetUser(userName, false);
        string genPwd = u.ResetPassword();
        return u.ChangePassword(genPwd, newPassword);
    }
}