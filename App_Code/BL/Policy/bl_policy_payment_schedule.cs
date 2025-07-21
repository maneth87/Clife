using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_payment_schedule
/// </summary>
public class bl_policy_payment_schedule
{

    #region "Constructor"
    public bl_policy_payment_schedule()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variables"
        private string _Policy_Payment_Schedule_ID;
        private string _Policy_ID;
        private double _Premium;
        private double _Sum_Insure;
        private double _Discount;
        private string _Product_ID;
        private int _Pay_Mode;
        private int _Year;
        private int _Time;
        private double _Premium_After_Discount;
        private double _Extra_Premium;
        private double _Total_Premium;
        private DateTime _Due_Date;
        private DateTime _Created_On;
        private string _Created_By;
        private string _Created_Note;
        //maneth
        private double _Original_Amount;
        //End maneth

    #endregion

    #region "Public Properties"
        public string Policy_Payment_Schedule_ID
        {
            get { return _Policy_Payment_Schedule_ID; }
            set { _Policy_Payment_Schedule_ID = value; }
        }
     
        public string Policy_ID
        {
            get { return _Policy_ID; }
            set { _Policy_ID = value; }
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

        public double Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }

        public string Product_ID
        {
            get { return _Product_ID; }
            set { _Product_ID = value; }
        }       

        public int Pay_Mode
        {
            get { return _Pay_Mode; }
            set { _Pay_Mode = value; }
        }

        public int Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        public int Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        public double Premium_After_Discount
        {
            get { return _Premium_After_Discount; }
            set { _Premium_After_Discount = value; }
        }

        public double Extra_Premium
        {
            get { return _Extra_Premium; }
            set { _Extra_Premium = value; }
        }

        public double Total_Premium
        {
            get { return _Total_Premium; }
            set { _Total_Premium = value; }
        }
        //maneth
        public double Original_Amount
        {
            get { return _Original_Amount; }
            set { _Original_Amount = value; }
        }
        //end maneth
        public DateTime Due_Date
        {
            get { return _Due_Date; }
            set { _Due_Date = value; }
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

    #endregion
    

}