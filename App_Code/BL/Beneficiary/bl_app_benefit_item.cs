using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_benefit_item
/// </summary>
public class bl_app_benefit_item
{
	#region "Private Variable"

    private string _App_Benefit_Item_ID;
    private string _App_Register_ID;
    private int _Seq_Number;
    private int _ID_Type;
    private string _ID_Card;
    private string _Full_Name;
    private string _Relationship;
    private double _Percentage;
    private string _Relationship_Khmer;
    private string _Remarks;

    #endregion


    #region "Constructor"
    public bl_app_benefit_item()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Benefit_Item_ID
    {
        get { return _App_Benefit_Item_ID; }
        set { _App_Benefit_Item_ID = value; }
    }

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
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
    public string Remarks
    {
        get { return _Remarks; }
        set { _Remarks = value; }
    }
    #endregion
}