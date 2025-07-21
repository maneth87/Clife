using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_commission_report
/// </summary>
public class bl_commission_report
{
	#region "Private Variable"

        private string _Policy_ID;
        private string _Policy_Number;
        private int _Policy_Type; 
        private string _Receipt_No;
        private DateTime _Created_On;
    #endregion

	#region "Constructor"
        public bl_commission_report()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
	}

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
	}

    public int Policy_Type
    {
        get { return _Policy_Type; }
        set { _Policy_Type = value; }
    }
    public string Receipt_No
    {
        get { return _Receipt_No; }
        set { _Receipt_No = value; }
    }

	#endregion
}