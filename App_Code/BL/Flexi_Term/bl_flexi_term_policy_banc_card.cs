using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_policy_banc_card
/// </summary>
public class bl_flexi_term_policy_banc_card
{
    #region "Private Variable"

    private string _Card_ID;
    private string _Flexi_Term_Policy_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion

    #region "Constructor"
    public bl_flexi_term_policy_banc_card()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Public Property"

    public string Card_ID
    {
        get { return _Card_ID; }
        set { _Card_ID = value; }
    }

    public string Flexi_Term_Policy_ID
    {
        get { return _Flexi_Term_Policy_ID; }
        set { _Flexi_Term_Policy_ID = value; }
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