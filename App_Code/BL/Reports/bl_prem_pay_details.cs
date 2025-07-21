using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_prem_pay_details
/// </summary>
public class bl_prem_pay_details
{
	#region "Private Variable"

        private string _Policy_ID;        
        private double _Sum_Insure;        
        private double _Amount;       
        private int _Prem_Year;
        private int _Prem_Lot;       
    
    #endregion

	#region "Constructor"
        public bl_prem_pay_details()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }


    public double Sum_Insure
    {
        get { return _Sum_Insure; }
        set { _Sum_Insure = value; }
    }

  
    public double Amount
    {
        get { return _Amount; }
        set { _Amount = value; }
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
       
 
	#endregion
}