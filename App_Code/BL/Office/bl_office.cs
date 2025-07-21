using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_office_table
/// </summary>
public class bl_office
{
    #region "Private Variable"

    private string _Office_ID;
    private string _Detail;   
    private int _Status;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    /// <summary>
    /// Add more parameter
    /// </summary>
    private string _Old_Office_ID;
   
    #endregion


    #region "Constructor"
    public bl_office()
    {

    }
    #endregion

    #region "Public Property"

    public string Old_Office_ID
    {
        get { return _Old_Office_ID; }
        set { _Old_Office_ID = value; }
    }

    public string Office_ID
    {
        get { return _Office_ID; }
        set { _Office_ID = value; }
    }

    public string Detail
    {
        get { return _Detail; }
        set { _Detail = value; }
    }      

    public int Status
    {
        get { return _Status; }
        set { _Status = value; }
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

   
    #endregion
}