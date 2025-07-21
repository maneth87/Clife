using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_underwrite
/// </summary>
public class bl_underwrite_code
{
	 #region "Private Variable"

    private string _Status_Code;
    private string _Detail;
    private bool _Is_Inforce;
    private bool _Is_Reserved;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private string _Parent_Status_Code;


    #endregion

    #region "Constructor"
    public bl_underwrite_code()
    {

    }
    #endregion

    #region "Public Property"

    public string Status_Code
    {
        get { return _Status_Code; }
        set { _Status_Code = value; }
    }

    public string Detail
    {
        get { return _Detail; }
        set { _Detail = value; }
    }

    public bool Is_Inforce
    {
        get { return _Is_Inforce; }
        set { _Is_Inforce = value; }
    }

    public bool Is_Reserved
    {
        get { return _Is_Reserved; }
        set { _Is_Reserved = value; }
    }

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
    }

    public string Parent_Status_Code
    {
        get { return _Parent_Status_Code; }
        set { _Parent_Status_Code = value; }
    }

   
    #endregion
}