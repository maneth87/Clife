using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_company_gtli
/// </summary>
public class bl_gtli_company
{
    #region "Private Variable"
    private string _GTLI_Company_ID;
    private string _Company_Name;
    private string _Type_Of_Business;
    private string _Company_Email;
    private string _Company_Address;
    private System.DateTime _Created_On;
    private string _Created_By;
    
    //Extra
    private string _Latest_Contact_Name;
    private string _Latest_Contact_Phone;

    private string _Latest_Contact_Email;
    private string _SortColum;   
    private string _SortDir;

    #endregion

    #region "Constructor"
    public bl_gtli_company()
    {
    }
    #endregion


    #region "Public Properties"

    public string GTLI_Company_ID
    {
        get { return _GTLI_Company_ID; }
        set { _GTLI_Company_ID = value; }
    }

    public string Company_Name
    {
        get { return _Company_Name; }
        set { _Company_Name = value; }
    }

    public string Type_Of_Business
    {
        get { return _Type_Of_Business; }
        set { _Type_Of_Business = value; }
    }

    public string Company_Address
    {
        get { return _Company_Address; }
        set { _Company_Address = value; }
    }

    public string Company_Email
    {
        get { return _Company_Email; }
        set { _Company_Email = value; }
    }  

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public System.DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    //Extra
    public string Latest_Contact_Name
    {
        get { return _Latest_Contact_Name; }
        set { _Latest_Contact_Name = value; }
    }

    public string Latest_Contact_Phone
    {
        get { return _Latest_Contact_Phone; }
        set { _Latest_Contact_Phone = value; }
    }
    public string Latest_Contact_Email
    {
        get { return _Latest_Contact_Email; }
        set { _Latest_Contact_Email = value; }
    }

    public string SortColum
    {
        get { return _SortColum; }
        set { _SortColum = value; }
    }

    public string SortDir
    {
        get { return _SortDir; }
        set { _SortDir = value; }
    }
    #endregion

}