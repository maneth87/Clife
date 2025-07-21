using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_loan
/// </summary>
public class bl_app_loan
{
	#region "Private Variable"

    private string _App_Register_ID;
    private int _Loan_Type;
    private double _Interest_Rate;
    private int _Term_Year;
    private DateTime _Loan_Effiective_Date;
    private double _Out_Std_Loan;
          
    public enum LoanType {Home=0, Business=1, Others=2};
    #endregion
    

    #region "Constructor"
    public bl_app_loan()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public int Loan_Type
    {
        get { return _Loan_Type; }
        set { _Loan_Type = value; }
    }

    public double Interest_Rate
    {
        get { return _Interest_Rate; }
        set { _Interest_Rate = value; }

    }

    public int Term_Year
    {
        get { return _Term_Year; }
        set { _Term_Year = value; }
    }

    public DateTime Loan_Effiective_Date
    {
        get { return _Loan_Effiective_Date; }
        set { _Loan_Effiective_Date = value; }
    }

    public double Out_Std_Loan
    {
        get { return _Out_Std_Loan; }
        set { _Out_Std_Loan = value; }
    }
       
    #endregion
}