using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_cl24_renewal_premium
/// </summary>
public class bl_cl24_renewal_premium
{
    #region "Constructor"
	public bl_cl24_renewal_premium()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion

    #region "Private Variable"

    private string _Policy_ID;
    private string _Policy_Number;
    private string _Customer_ID;
    private string _Insured_Name;
    private DateTime _Birth_Date;
    private string _Gender;
    private string _Phone_Number;
    private DateTime _Start_Date;
    private DateTime _End_Date;
    private float _Sum_Insure;
    private DateTime _Effective_Date;
    private DateTime _Issued_Date;
    private DateTime _Due_Date;
    private DateTime _Next_Due_Date;
    private string _Product_ID;
    private string _Product_Name;
    private float _Premium;
    private float _Total_Premium;
    private float _EM_Amount;
    private int _Pay_Year;
    private int _Pay_Lot;
    private string _Agent_Code;
    private string _Agent_Name;
    private string _Policy_Year;
    private int _Pay_Mode_ID;
    private string _Paymode_Mode;
    private int _Factor;
    private string _Created_By;
    private DateTime _Report_Date;
    private string _Status_Remark;
    private string _Remark;
    private DateTime _Paid_Off_Date;

    #endregion

    #region "Public Property"
    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }
    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }
    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public string Insured_Name
    {
        get { return _Insured_Name; }
        set { _Insured_Name = value; }
    }
    public DateTime Birth_Date
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
    }
    public string Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }
    public string Phone_Number
    {
        get { return _Phone_Number; }
        set { _Phone_Number = value; }
    }
    public DateTime Start_Date
    {
        get { return _Start_Date; }
        set { _Start_Date = value; }
    }
    public DateTime End_Date
    {
        get { return _End_Date; }
        set { _End_Date = value; }
    }
    public float Sum_Insure
    {
        get { return _Sum_Insure; }
        set { _Sum_Insure = value; }
    }
    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public DateTime Issued_Date
    {
        get { return _Issued_Date; }
        set { _Issued_Date = value; }
    }
    public DateTime Due_Date
    {
        get { return _Due_Date; }
        set { _Due_Date = value; }
    }
    public DateTime Next_Due_Date
    {
        get { return _Next_Due_Date; }
        set { _Next_Due_Date = value; }
    }
    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }
    public string Product_Name
    {
        get { return _Product_Name; }
        set { _Product_Name = value; }
    }
    public float Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }
    public float Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }
    public float EM_Amount
    {
        get { return _EM_Amount; }
        set { _EM_Amount = value; }
    }
    public int Pay_Year
    {
        get { return _Pay_Year; }
        set { _Pay_Year = value; }
    }
    public int Pay_Lot
    {
        get { return _Pay_Lot; }
        set { _Pay_Lot = value; }
    }
    public string Agent_Code
    {
        get { return _Agent_Code; }
        set { _Agent_Code = value; }
    }
    public string Agent_Name
    {
        get { return _Agent_Name; }
        set { _Agent_Name = value; }
    }
    public string Policy_Year
    {
        get { return _Policy_Year; }
        set { _Policy_Year = value; }
    }
    public int Pay_Mode_ID
    {
        get { return _Pay_Mode_ID; }
        set { _Pay_Mode_ID = value; }
    }
    public string Paymode_Mode
    {
        get { return _Paymode_Mode; }
        set { _Paymode_Mode = value; }
    }
    public int Factor
    {
        get { return _Factor; }
        set { _Factor = value; }
    }
    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }
    public DateTime Report_Date
    {
        get { return _Report_Date; }
        set { _Report_Date = value; }
    }

    public string Remark
    {
        get { return _Remark; }
        set { _Remark = value; }
    }

    public string Status_Remark
    {
        get { return _Status_Remark; }
        set { _Status_Remark = value; }
    }

    public DateTime Paid_Off_Date
    {
        get { return _Paid_Off_Date; }
        set { _Paid_Off_Date = value; }
    }

    


    #endregion

}