using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_number
/// </summary>
public class bl_policy_number
{

    #region "Constructor"
    public bl_policy_number()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variables"

        private string _Policy_ID;
        private string _Policy_Number;

    #endregion

    #region "Public Properties"
        public string Policy_ID
        {
            get { return _Policy_ID; }
            set { _Policy_ID = value; }
        }

        public string Policy_Number
        {
            get { return _Policy_Number; }
            set { _Policy_Number = value; }
        }


    #endregion

}