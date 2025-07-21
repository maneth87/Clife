using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_prod_life_cv_item
/// </summary>
public class bl_prod_life_cv_item
{

    #region "Private Variables"

    private string _Prod_Life_CV_Item_ID;
    private string _Product_ID;
    private int _Gender;
    private int _Assure_Year;
    private int _Age;
    private int _Policy_Year;
    private double _Cash_Value;
    private double _CV_Amount;

    #endregion

    #region "Constructor"
    public bl_prod_life_cv_item()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public string Prod_Life_CV_Item_ID
    {
        get { return _Prod_Life_CV_Item_ID; }
        set { _Prod_Life_CV_Item_ID = value; }
    }

    public int Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public int Assure_Year
    {
        get { return _Assure_Year; }
        set { _Assure_Year = value; }
    }
    public int Age
    {
        get { return _Age; }
        set { _Age = value; }
    }
    public int Policy_Year
    {
        get { return _Policy_Year; }
        set { _Policy_Year = value; }
    }
    public double Cash_Value
    {
        get { return _Cash_Value; }
        set { _Cash_Value = value; }
    }
    public double CV_Amount
    {
        get { return _CV_Amount; }
        set { _CV_Amount = value; }
    }

}