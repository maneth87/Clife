using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_payment_code
/// </summary>
public class bl_payment_code
{
    #region "Private Variable"

    private int _qb_code_id;
    private string _payment_code;
    private DateTime _quotation_date;
    private string _quotation_number;

    #endregion

    #region "Constructor"
    public bl_payment_code()
	{

    }
    #endregion "Constructor"

    #region "Public Property"

    public int qb_code_id
    {
        get { return _qb_code_id; }
        set { _qb_code_id = value; }
    }

    public string payment_code
    {
        get { return _payment_code; }
        set { _payment_code = value; }
    }

    public DateTime quotation_date
    {
        get { return _quotation_date; }
        set { _quotation_date = value; }
    }

    public string quotation_number
    {
        get { return _quotation_number; }
        set { _quotation_number = value; }
    }
   

    #endregion

}