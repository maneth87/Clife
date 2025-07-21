using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_policy
/// </summary>
public class bl_app_policy
{

    #region "Constructor"
    public bl_app_policy()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variables"
        private string _App_Policy_ID;
        private string _App_Register_ID;
        private string _Policy_ID;
    #endregion

    #region "Public Properties"
        public string App_Policy_ID
        {
            get { return _App_Policy_ID; }
            set { _App_Policy_ID = value; }
        }
        public string App_Register_ID
        {
            get { return _App_Register_ID; }
            set { _App_Register_ID = value; }
        }
        public string Policy_ID
        {
            get { return _Policy_ID; }
            set { _Policy_ID = value; }
        }

    #endregion

}