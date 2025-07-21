using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_pay_mode
/// </summary>
public class bl_policy_pay_mode
{
	
    #region "Constructor"
        public bl_policy_pay_mode()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
        }
    #endregion

    #region "Private Variables"
        
        private string _Policy_ID;
        private int _Pay_Mode;
        private int _First_Due_Day;
        private int _First_Due_Month;       
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
        public int Pay_Mode
        {
            get { return _Pay_Mode; }
            set { _Pay_Mode = value; }
        }
        public int First_Due_Day
        {
            get { return _First_Due_Day; }
            set { _First_Due_Day = value; }
        }
        public int First_Due_Month
        {
            get { return _First_Due_Month; }
            set { _First_Due_Month = value; }
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