using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_underwriting_co
/// </summary>
public class bl_underwriting_co
{
    #region "Constructor"


    public bl_underwriting_co()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #endregion

    #region "Private Variable"
        private string _UW_Life_Product_ID;
        private string _CO_Doc_Number;
        private DateTime _Requested_On;
        private int _Doc_Schedule;
        private double _System_Sum_Insure;
        private double _System_Premium;
        private double _EM_Percent;
        private double _EM_Premium;
        private double _EM_Amount;
        private double _EF_Rate;
        private double _EF_Premium;
        private int _Total_EF_Year;     
        private DateTime _Created_On;
        private string _Created_By;
        private string _Created_Note;
        private int _Pay_Mode;
        private double _EM_Rate;
        private double _Original_Amount;
        private string _Benefit_Note;
        private string _Round_Status_ID;
        
    #endregion



    #region "Public Property"

        public string Benefit_Note
        {
            get { return _Benefit_Note; }
            set { _Benefit_Note = value; }
        }
        public double Original_Amount
        {
            get { return _Original_Amount; }
            set { _Original_Amount = value; }
        }
        public double EM_Rate
        {
            get { return _EM_Rate; }
            set { _EM_Rate = value; }
        }
        public int Pay_Mode
        {
            get { return _Pay_Mode; }
            set { _Pay_Mode = value; }
        }
        public DateTime Created_On
        {
            get { return _Created_On; }
            set { _Created_On = value; }               
        }

        public string Created_By
        {
            get { return _Created_By; }
            set { _Created_By = value; }
        }

        public string Created_Note
        {
            get { return _Created_Note; }
            set { _Created_Note = value; }
        }

        public int Total_EF_Year
        {
            get { return _Total_EF_Year; }
            set { _Total_EF_Year = value; }
        }    
    
    
        public double EF_Premium
        {
            get { return _EF_Premium; }
            set { _EF_Premium = value; }
        }    
    
        public double EF_Rate
        {
            get { return _EF_Rate; }
            set { _EF_Rate = value; }
        } 
    
        public double EM_Amount
        {
            get { return _EM_Amount; }
            set { _EM_Amount = value; }
        } 

        public double EM_Premium
        {
            get { return _EM_Premium; }
            set { _EM_Premium = value; }
        } 
        public double EM_Percent
        {
            get { return _EM_Percent; }
            set { _EM_Percent = value; }
        } 
        public double System_Premium
        {
            get { return _System_Premium; }
            set { _System_Premium = value; }
        }   
        public double System_Sum_Insure
        {
            get { return _System_Sum_Insure; }
            set { _System_Sum_Insure = value; }
        }    

        public int Doc_Schedule
        {
            get { return _Doc_Schedule; }
            set { _Doc_Schedule = value; }
        }    
        public string UW_Life_Product_ID
        {
            get { return _UW_Life_Product_ID; }
            set { _UW_Life_Product_ID = value; }
        }

        public string CO_Doc_Number
        {
            get { return _CO_Doc_Number; }
            set { _CO_Doc_Number = value; }
        }

        public string Round_Status_ID
        {
            get { return _Round_Status_ID; }
            set { _Round_Status_ID = value; }
        }

        public DateTime Requested_On
        {
            get { return _Requested_On; }
            set { _Requested_On = value; }
        }

    #endregion


}