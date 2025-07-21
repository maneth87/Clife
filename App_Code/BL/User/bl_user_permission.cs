using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_User_Permission
/// </summary>
public class bl_User_Permission
{
	#region "Private Variable"

        private string _User_Permission_ID;
        private string _User_ID;       
        private string _Application_ID;
        private int _Read;
        private int _Write;
        private int _Delete;
        private int _Status;

    #endregion


    #region "Constructor"
        public bl_User_Permission()
        {

        }
        #endregion

    #region "Public Property"

    public string User_Permission_ID
    {
        get { return _User_Permission_ID; }
        set { _User_Permission_ID = value; }
    }

    public string User_ID
    {
        get { return _User_ID; }
        set { _User_ID = value; }
    }

    public string Application_ID
    {
        get { return _Application_ID; }
        set { _Application_ID = value; }
    }

    public int Read
    {
        get { return _Read; }
        set { _Read = value; }
    }

    public int Write
    {
        get { return _Write; }
        set { _Write = value; }
    }
    
    public int Delete
    {
        get { return _Delete; }
        set { _Delete = value; }
    }

    public int Status
    {
        get { return _Status; }
        set { _Status = value; }
    }

    #endregion
}