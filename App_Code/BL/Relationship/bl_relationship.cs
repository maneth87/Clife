using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_relationship
/// </summary>
public class bl_relationship
{
	#region "Private Variable"

    private string _Relationship;
    private bool _Is_Clean_Case;
    private bool _Is_Reserved;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private string _Relationship_Khmer;

    #endregion

    #region "Constructor"
    public bl_relationship()
    {

    }
    #endregion

    #region "Public Property"

    public string Relationship
    {
        get { return _Relationship; }
        set { _Relationship = value; }
    }

    public bool Is_Clean_Case
    {
        get { return _Is_Clean_Case; }
        set { _Is_Clean_Case = value; }
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

    public string Relationship_Khmer
    {
        get { return _Relationship_Khmer; }
        set { _Relationship_Khmer = value; }
    }


    #endregion
}