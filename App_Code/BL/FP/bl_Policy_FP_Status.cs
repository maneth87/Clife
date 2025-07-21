using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_Policy_FP_Status
/// </summary>
public class bl_Policy_FP_Status
{
    #region 'Local Variables'
    private string policy_id;
    private int policy_status_type_id;
    private string created_by;
    private DateTime created_on;
    private string created_note;
    #endregion
    #region 'Properties'
    public string Created_note
    {
        get { return created_note; }
        set { created_note = value; }
    }


    public DateTime Created_on
    {
        get { return created_on; }
        set { created_on = value; }
    }


    public string Created_by
    {
        get { return created_by; }
        set { created_by = value; }
    }


    public int Policy_status_type_id
    {
        get { return policy_status_type_id; }
        set { policy_status_type_id = value; }
    }


    public string Policy_id
    {
        get { return policy_id; }
        set { policy_id = value; }
    }


    #endregion
    #region 'Constructor'
    public bl_Policy_FP_Status()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_Policy_FP_Status(string policy_id, int policy_status_type_id, string created_by, DateTime created_on, string created_note)
    {
        this.policy_id = policy_id;
        this.policy_status_type_id = policy_status_type_id;
        this.created_on = created_on;
        this.created_by = created_by;
        this.created_note = created_note;
    }

    #endregion
}