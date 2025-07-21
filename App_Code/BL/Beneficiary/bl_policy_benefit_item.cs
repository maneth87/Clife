using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_benefit_item
/// </summary>
public class bl_policy_benefit_item
{

    #region "Constructor"
    public bl_policy_benefit_item()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion
    
    #region "Private Variable"

    private string _Policy_Benefit_Item_ID;
    private string _Policy_ID;
    private int _Seq_Number;
    private int _ID_Type;
    private string _ID_Card;
    private string _Full_Name;
    private string _Relationship;
    private string _Relationship_Khmer;
    private double _Percentage;

    #endregion

    #region "Public Property"

    public string Policy_Benefit_Item_ID
    {
        get { return _Policy_Benefit_Item_ID; }
        set { _Policy_Benefit_Item_ID = value; }
    }

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }

    public int Seq_Number
    {
        get { return _Seq_Number; }
        set { _Seq_Number = value; }
    }

    public int ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }

    }

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }

    public string Full_Name
    {
        get { return _Full_Name; }
        set { _Full_Name = value; }
    }

    public string Relationship
    {
        get { return _Relationship; }
        set { _Relationship = value; }
    }

    public string Relationship_Khmer
    {
        get { return _Relationship_Khmer; }
        set { _Relationship_Khmer = value; }
    }

    public double Percentage
    {
        get { return _Percentage; }
        set { _Percentage = value; }
    }

    #endregion

}