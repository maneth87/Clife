using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_proposed_insured_item
/// </summary>
public class bl_policy_micro_proposed_insured_item
{
    #region "Constructor"
    public bl_policy_micro_proposed_insured_item()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion


    #region "Private Variable"

    private string _Policy_Micro_Proposed_Insured_Item_ID;
    private string _Policy_Micro_ID;
    private int _Seq_Number;
    private string _Full_Name;
    private int _Age;
    private string _Relationship;
    private string _Address;
    private double _Percentage;
    private string _Relationship_Khmer;
 
    #endregion

    #region "Public Property"

    public string Policy_Micro_Proposed_Insured_Item_ID
    {
        get { return _Policy_Micro_Proposed_Insured_Item_ID; }
        set { _Policy_Micro_Proposed_Insured_Item_ID = value; }
    }

    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
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

    public int Age
    {
        get { return _Age; }
        set { _Age = value; }
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

    
    #endregion


}