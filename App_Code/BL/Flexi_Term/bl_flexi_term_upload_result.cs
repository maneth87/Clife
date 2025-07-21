using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_upload_result
/// </summary>
public class bl_flexi_term_upload_result
{
	#region "Private Variable"

    private string _Branch;
    private string _Bank_Number;
    private string _First_Name;
    private string _Last_Name;
    private string _ID_Type;
    private string _ID_Card;
    private string _DOB;
 
    private string _Gender;
    private string _Application_Resident;
    private string _Beneficiary_First_Name;
    private string _Beneficiary_Last_Name;
    private string _Beneficiary_ID_Type;
    private string _Beneficiary_ID_Card;
    private string _Beneficiary_Relationship;
    private string _Family_Book;
    private string _Result;
    private string _Reason;  

    #endregion

	#region "Constructor"

    public bl_flexi_term_upload_result()
	{
	}
	#endregion
    
	#region "Public Properties"

    
    public string Branch
    {
        get { return _Branch; }
        set { _Branch = value; }
    }

    public string Bank_Number
    {
        get { return _Bank_Number; }
        set { _Bank_Number = value; }
    }
    
    public string First_Name
    {
        get { return _First_Name; }
        set { _First_Name = value; }
    }

    public string Last_Name
    {
        get { return _Last_Name; }
        set { _Last_Name = value; }
    }

    public string ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }
    }

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }
      
    public string DOB
    {
        get { return _DOB; }
        set { _DOB = value; }
    }

    public string Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public string Application_Resident
    {
        get { return _Application_Resident; }
        set { _Application_Resident = value; }
    }

    public string Beneficiary_First_Name
    {
        get { return _Beneficiary_First_Name; }
        set { _Beneficiary_First_Name = value; }
    }

    public string Beneficiary_Last_Name
    {
        get { return _Beneficiary_Last_Name; }
        set { _Beneficiary_Last_Name = value; }
    }

    public string Beneficiary_ID_Type
    {
        get { return _Beneficiary_ID_Type; }
        set { _Beneficiary_ID_Type = value; }
    }

    public string Beneficiary_ID_Card
    {
        get { return _Beneficiary_ID_Card; }
        set { _Beneficiary_ID_Card = value; }
    }

    public string Beneficiary_Relationship
    {
        get { return _Beneficiary_Relationship; }
        set { _Beneficiary_Relationship = value; }
    }

    public string Family_Book
    {
        get { return _Family_Book; }
        set { _Family_Book = value; }
    }

    public string Result
    {
        get { return _Result; }
        set { _Result = value; }
    }

    public string Reason
    {
        get { return _Reason; }
        set { _Reason = value; }
    }
   

	#endregion
}