using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_payment_mode
/// </summary>
public class bl_payment_mode
{
    #region "Private Variable"
    private int _Pay_Mode_ID;
    private string _Mode;
    private double _Factor;

    #endregion
   
    #region "Constructor"
    public bl_payment_mode()
        {

        }
    #endregion

    #region "Public Property"


    public int Pay_Mode_ID
    {
        get { return _Pay_Mode_ID; }
        set { _Pay_Mode_ID = value; }
    }

    public string Mode
    {
        get { return _Mode; }
        set { _Mode = value; }
    }

    public double Factor
    {
        get { return _Factor; }
        set { _Factor = value; }
    }
    #endregion
    /// <summary>
    /// Return property name
    /// </summary>
    public class NAME
    {
        public static string Pay_Mode_ID { get { return "Pay_Mode_ID"; } }
        public static string Mode { get { return "Mode"; } }
        public static string Factor { get { return "Factor"; } }
    }
}