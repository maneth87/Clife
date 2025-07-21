using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_prem_pay
/// </summary>
public class bl_policy_prem_pay
{
    #region "Constructor"
    public bl_policy_prem_pay()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variables"
        private string _Policy_Prem_Pay_ID;
        private string _Policy_ID;
        private DateTime _Due_Date;
        private DateTime _Pay_Date;
        private int _Prem_Year;
        private int _Prem_Lot;
        private double _Amount;
        private string _Sale_Agent_ID;
        private string _Office_ID;
        private DateTime _Created_On;
        private string _Created_By;
        private string _Created_Note;
        private int _Pay_Mode_ID;

    #endregion


    #region "Public Properties"

        public string Policy_Prem_Pay_ID
        {
            get { return _Policy_Prem_Pay_ID; }
            set { _Policy_Prem_Pay_ID = value; }
        }
        public string Policy_ID
        {
            get { return _Policy_ID; }
            set { _Policy_ID = value; }
        }
        public DateTime Due_Date
        {
            get { return _Due_Date; }
            set { _Due_Date = value; }
        }
        public DateTime Pay_Date
        {
            get { return _Pay_Date; }
            set { _Pay_Date = value; }
        }
        public int Prem_Year
        {
            get { return _Prem_Year; }
            set { _Prem_Year = value; }
        }
        public int Prem_Lot
        {
            get { return _Prem_Lot; }
            set { _Prem_Lot = value; }
        }
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }
        public string Sale_Agent_ID
        {
            get { return _Sale_Agent_ID; }
            set { _Sale_Agent_ID = value; }
        }
        public string Office_ID
        {
            get { return _Office_ID; }
            set { _Office_ID = value; }
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

        public int Pay_Mode_ID
        {
            get { return _Pay_Mode_ID; }
            set { _Pay_Mode_ID = value; }
        }   


    #endregion

}