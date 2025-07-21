using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_report_chart
/// </summary>
public class bl_policy_report_chart
{
    #region "Private Variable"
   
    private int _Total_Policy;
    private double _Total_Sum_Insure;
    private double _Total_Premium;
    private double _Total_EM_Premium; 
    private string _Product_ID;
    private string _Product;
    private int _Month;
    private int _Year;
    #endregion

    #region "Constructor"
    public bl_policy_report_chart()
    {

    }
    #endregion

    #region "Public Property"

    public double Total_Sum_Insure
    {
        get { return _Total_Sum_Insure; }
        set { _Total_Sum_Insure = value; }
    }

    public int Total_Policy
    {
        get { return _Total_Policy; }
        set { _Total_Policy = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }

    public double Total_EM_Premium
    {
        get { return _Total_EM_Premium; }
        set { _Total_EM_Premium = value; }
    }

    public string Product
    {
        get { return _Product; }
        set { _Product = value; }
    }

    public int Month
    {
        get { return _Month; }
        set { _Month = value; }
    }

    public int Year
    {
        get { return _Year; }
        set { _Year = value; }
    }

    #endregion
}