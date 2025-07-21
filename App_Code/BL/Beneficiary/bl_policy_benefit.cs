using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_benefit
/// </summary>
public class bl_policy_benefit
{
	
    #region "Constructor"
        public bl_policy_benefit()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
        }
    #endregion

    #region "Private Variables"

        private string _Policy_ID;
        private string _Benefit_Note;       
        private DateTime _Created_On;
        private string _Created_By;
        private string _Created_Note;

    #endregion

    #region "Public Properties"

        public string Policy_ID
        {
            get { return _Policy_ID; }
            set { _Policy_ID = value; }
        }

        public string Benefit_Note
        {
            get { return _Benefit_Note; }
            set { _Benefit_Note = value; }
        }
        public string Created_Note
        {
            get { return _Created_Note; }
            set { _Created_Note = value; }
        }

        public string Created_By
        {
            get { return _Created_By; }
            set { _Created_By = value; }
        }

        public DateTime Created_On
        {
            get { return _Created_On; }
            set { _Created_On = value; }
        } 

    #endregion


}