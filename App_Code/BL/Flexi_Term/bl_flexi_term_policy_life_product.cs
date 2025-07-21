using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_policy_life_product
/// </summary>
public class bl_flexi_term_policy_life_product
{
    #region "Private Variable"

    private string _Flexi_Term_Policy_ID;
    private string _Product_ID;
    private int _Age_Insure;
    private int _Pay_Year;
    private int _Pay_Up_To_Age;
    private int _Assure_Year;
    private int _Assure_Up_To_Age;
    private double _User_Sum_Insure;
    private double _System_Sum_Insure;
    private double _User_Premium;
    private double _System_Premium;
    private double _System_Premium_Discount;
    private int _Pay_Mode;

    #endregion

    #region "Constructor"
    public bl_flexi_term_policy_life_product()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Public Property"

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public string Flexi_Term_Policy_ID
    {
        get { return _Flexi_Term_Policy_ID; }
        set { _Flexi_Term_Policy_ID = value; }
    }

    public int Age_Insure
    {
        get { return _Age_Insure; }
        set { _Age_Insure = value; }

    }

    public int Pay_Year
    {
        get { return _Pay_Year; }
        set { _Pay_Year = value; }
    }

    public int Pay_Up_To_Age
    {
        get { return _Pay_Up_To_Age; }
        set { _Pay_Up_To_Age = value; }
    }

    public int Assure_Year
    {
        get { return _Assure_Year; }
        set { _Assure_Year = value; }
    }
    public int Assure_Up_To_Age
    {
        get { return _Assure_Up_To_Age; }
        set { _Assure_Up_To_Age = value; }
    }

    public double User_Sum_Insure
    {
        get { return _User_Sum_Insure; }
        set { _User_Sum_Insure = value; }
    }

    public double System_Sum_Insure
    {
        get { return _System_Sum_Insure; }
        set { _System_Sum_Insure = value; }
    }

    public double User_Premium
    {
        get { return _User_Premium; }
        set { _User_Premium = value; }
    }

    public double System_Premium
    {
        get { return _System_Premium; }
        set { _System_Premium = value; }
    }

    public double System_Premium_Discount
    {
        get { return _System_Premium_Discount; }
        set { _System_Premium_Discount = value; }
    }

    public int Pay_Mode
    {
        get { return _Pay_Mode; }
        set { _Pay_Mode = value; }
    }
    #endregion


}