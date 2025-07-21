using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_wing_account
/// </summary>
public class bl_wing_account
{

    #region "Private Variable"

    private string _SK;
    private string _Wing_Number;
    private DateTime _Date_Request;
    private DateTime _Created_On;
    private string _Created_By;
    private int _Status;
   
    #endregion 

	public bl_wing_account()
	{
	}

    #region "Public Variable"

    public string Sk
    {

        get { return _SK; }
        set { _SK = value; }

    }

    public string Wing_Number
    {

        get { return _Wing_Number; }
        set { _Wing_Number = value; }

    }

    public DateTime Date_Request
    {

        get { return _Date_Request; }
        set { _Date_Request = value; }

    }

    public DateTime  Created_On
    {

        get { return _Created_On; }
        set { _Created_On = value; }

    }

    public string Created_By
    {

        get { return _Created_By; }
        set { _Created_By = value; }

    }

    public int Status
    {
        get { return _Status; }
        set { _Status = value; }
    }

    #endregion 
}