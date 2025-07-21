using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_policy_info_person
/// </summary>
public class bl_flexi_term_policy_info_person
{

    #region "Constructor"
    public bl_flexi_term_policy_info_person()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variables"
        private string _Flexi_Term_Policy_ID;
        private string _ID_Card;
        private int _ID_Type;
        private string _First_Name;
        private string _Last_Name;
        private string _Khmer_First_Name;
        private string _Khmer_Last_Name;
        private int _Gender;
        private DateTime _Birth_Date;
        private int _Marital_Status;
        private int _Resident;
        private string _Branch;
        private string _Bank_Number;
       
    #endregion

    #region "Public Properties"

        public string Flexi_Term_Policy_ID
        {
            get { return _Flexi_Term_Policy_ID; }
            set { _Flexi_Term_Policy_ID = value; }
        }

        public string ID_Card
        {
            get { return _ID_Card; }
            set { _ID_Card = value; }
        }

        public int ID_Type
        {
            get { return _ID_Type; }
            set { _ID_Type = value; }
        }

        public string First_Name
        {
            get { return _First_Name; }
            set { _First_Name = value; }
        }

        public string Last_Name
        {
            get { return _Last_Name; }
            set { _Last_Name = value; }
        }

        public string Khmer_First_Name
        {
            get { return _Khmer_First_Name; }
            set { _Khmer_First_Name = value; }
        }

        public string Khmer_Last_Name
        {
            get { return _Khmer_Last_Name; }
            set { _Khmer_Last_Name = value; }
        }

        public int Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        public DateTime Birth_Date
        {
            get { return _Birth_Date; }
            set { _Birth_Date = value; }
        }

        public int Marital_Status
        {
            get { return _Marital_Status; }
            set { _Marital_Status = value; }
        }

        public int Resident
        {
            get { return _Resident; }
            set { _Resident = value; }
        }

        public string Branch
        {
            get { return _Branch; }
            set { _Branch = value; }
        }

        public string Bank_Number
        {
            get { return _Bank_Number; }
            set { _Bank_Number = value; }
        }

    #endregion

}