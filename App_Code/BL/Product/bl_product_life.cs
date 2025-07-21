using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_product_life
/// </summary>
public class bl_product_life
{
	 #region "Private Variable"
        private string _Product_ID;
        private int _Pay_Year;
        private int _Pay_Up_To_Age;
        private int _Assure_Year;
        private int _Assure_Up_To_Age;
        private int _Yearly_Allow;
        private int _Half_Yearly_Allow;
        private int _Quartyly_Allow;
        private int _Monthly_Allow;
        private int _Single_Premium_Allow;
        private double _Int_Calc_Premium;      
          
    #endregion


    #region "Constructor"
        public bl_product_life()
        {

        }
        #endregion

    #region "Public Property"

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
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

    public int Yearly_Allow
    {
        get { return _Yearly_Allow; }
        set { _Yearly_Allow = value; }
    }

    public int Half_Yearly_Allow
    {
        get { return _Half_Yearly_Allow; }
        set { _Half_Yearly_Allow = value; }
    }

    public int Quartyly_Allow
    {
        get { return _Quartyly_Allow; }
        set { _Quartyly_Allow = value; }
    }

    public int Monthly_Allow
    {
        get { return _Monthly_Allow; }
        set { _Monthly_Allow = value; }
    }

    public int Single_Premium_Allow
    {
        get { return _Single_Premium_Allow; }
        set { _Single_Premium_Allow = value; }
    }

    public double Int_Calc_Premium
    {
        get { return _Int_Calc_Premium; }
        set { _Int_Calc_Premium = value; }
    }

   
    #endregion
}