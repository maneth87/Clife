using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_user
/// </summary>
public class bl_user
{
    #region "Private Variable"

    private string _UserId;
    private string _UserName;
    private string _RoleId;
    private string _RoleName;
    private string _Email;
    private bool _Approved;
    private string _IsApproved;
    private DateTime _CreateDate;
    private DateTime _LastActivityDate;
    private DateTime _LastLoginDate;
    private bool _LockedOut;
    private string _IsLockedOut;
    #endregion 

    #region "Constructor"
    public bl_user()
	{
    }
    #endregion 

    #region "Public Variable"

    public string UserId {

        get { return _UserId; }
        set { _UserId = value; }
    
    }

    public string UserName
    {

        get { return _UserName; }
        set { _UserName = value; }

    }

    public string RoleId
    {

        get { return _RoleId; }
        set { _RoleId = value; }

    }

    public string RoleName
    {

        get { return _RoleName; }
        set { _RoleName = value; }

    }

    public string Email
    {

        get { return _Email; }
        set { _Email = value; }

    }

    public bool Approved 
    {
        get { return _Approved; }
        set { _Approved = value; }
    }

    public string IsApproved
    {
        get { return _IsApproved; }
        set { _IsApproved = value; }
    }
    public DateTime CreateDate 
    {

        get { return _CreateDate; }
        set { _CreateDate = value; }
    }

    public DateTime LastActivityDate
    {

        get { return _LastActivityDate; }
        set { _LastActivityDate = value; }
    }

    public DateTime LastLoginDate
    {

        get { return _LastLoginDate; }
        set { _LastLoginDate = value; }
    }

    public bool LockedOut
    {
        get { return _LockedOut; }
        set { _LockedOut = value; }
    }
    public string IsLockedOut
    {
        get { return _IsLockedOut; }
        set { _IsLockedOut = value; }
    }
    #endregion 

    public DateTime LastPasswordChangedDate    { get; set; }
    public int FailedPasswordAttemptCount{get;set;}
    /// <summary>
    /// Return password age as days
    /// </summary>
    public int PasswordAge
    {
        get { return CalcPasswordAge(); }
    }
    private int CalcPasswordAge()
    {
        int age = 0;

        age = LastPasswordChangedDate.Date.Subtract(DateTime.Now.Date).Days;
        age = age < 0 ? age * -1 : age;
        return age;
    }
}