using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_policy_benefit_item
/// </summary>
public class bl_flexi_term_policy_benefit_item
{
    #region "Constructor"
    public bl_flexi_term_policy_benefit_item()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion


    #region "Private Variable"

    private string _Flexi_Term_Policy_Benefit_Item_ID;
    private string _Flexi_Term_Policy_ID;
    private int _Seq_Number;
    private string _Full_Name;
    private string _First_Name;
    private string _Last_Name;
    private DateTime _Birth_Date;
    private string _Relationship;
    private string _Address;
    private double _Percentage;
    private string _Relationship_Khmer;
    private int _Family_Book;
    private string _ID_Card;
    private int _ID_Type;

    #endregion

    #region "Public Property"

    public string Flexi_Term_Policy_Benefit_Item_ID
    {
        get { return _Flexi_Term_Policy_Benefit_Item_ID; }
        set { _Flexi_Term_Policy_Benefit_Item_ID = value; }
    }

    public string Flexi_Term_Policy_ID
    {
        get { return _Flexi_Term_Policy_ID; }
        set { _Flexi_Term_Policy_ID = value; }
    }

    public int Seq_Number
    {
        get { return _Seq_Number; }
        set { _Seq_Number = value; }
    }

    public string Full_Name
    {
        get { return _Full_Name; }
        set { _Full_Name = value; }
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
    public DateTime Birth_Date
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
    }

    public string Relationship
    {
        get { return _Relationship; }
        set { _Relationship = value; }
    }

    public string Address
    {
        get { return _Address; }
        set { _Address = value; }
    }


    public double Percentage
    {
        get { return _Percentage; }
        set { _Percentage = value; }
    }


    public string Relationship_Khmer
    {
        get { return _Relationship_Khmer; }
        set { _Relationship_Khmer = value; }
    }

    public int Family_Book
    {
        get { return _Family_Book; }
        set { _Family_Book = value; }
    }

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }

    public int ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }
    }


    #endregion


}