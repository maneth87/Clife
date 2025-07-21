using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_sale_agent_cancel
/// </summary>
public class bl_sale_agent_cancel
{
	#region "Private Variable"

    private string _Sale_Agent_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
        
     
    #endregion


    #region "Constructor"
        public bl_sale_agent_cancel()
        {

        }
        #endregion

    #region "Public Property"

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
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