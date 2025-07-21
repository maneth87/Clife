using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_premium
/// </summary>
public class bl_policy_premium
{	
    #region "Constructor"
    public bl_policy_premium()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion

    #region "Private Variable"
        private string _Policy_ID;
        private double _Sum_Insure;
        private double _Premium;
        private double _EM_Percent;
        private double _EM_Premium;
        
        private double _EF_Rate;
        private double _EF_Premium;
        private int _Total_EF_Year;
        private DateTime _Created_On;
        private string _Created_By;
        private string _Created_Note;

        private double _Original_Amount;
        private double _EM_Amount;
        private double _Discount_Amount;

        //NEW PROPS
        private double _Old_Sum_Insure;
        private double _Old_Premium;
        private double _Old_Original_Amount;
        private double _Old_EM_Premium;
        private double _Old_EM_Amount;
        private double _Old_Discount_Amount;
        

    #endregion


    #region "Public Properties"

        public double Discount_Amount
        {
            get { return _Discount_Amount; }
            set { _Discount_Amount = value; }
        }   


        public double Original_Amount
        {
            get { return _Original_Amount; }
            set { _Original_Amount = value; }
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
        public double Premium
        {
            get { return _Premium; }
            set { _Premium = value; }
        }
        public double Sum_Insure
        {
            get { return _Sum_Insure; }
            set { _Sum_Insure = value; }
        }
        public string Policy_ID
        {
            get { return _Policy_ID; }
            set { _Policy_ID = value; }
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

        //New Props
        public double Old_Sum_Insure
        {
            get { return _Old_Sum_Insure; }
            set { _Old_Sum_Insure = value; }
        }
        public double Old_Premium
        {
            get { return _Old_Premium; }
            set { _Old_Premium = value; }
        }
        public double Old_Original_Amount
        {
            get { return _Old_Original_Amount; }
            set { _Old_Original_Amount = value; }
        }
        public double Old_EM_Premium
        {
            get { return _Old_EM_Premium; }
            set { _Old_EM_Premium = value; }
        }
        public double Old_EM_Amount
        {
            get { return _Old_EM_Amount; }
            set { _Old_EM_Amount = value; }
        }
        public double Old_Discount_Amount
        {
            get { return _Old_Discount_Amount; }
            set { _Old_Discount_Amount = value; }
        }


    #endregion

}