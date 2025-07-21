using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_Policy_FP_Premium
/// </summary>
public class bl_Policy_FP_Premium
{
    #region 'Local Variables'
    private string premium_id;
    private string policy_id;
    private int pay_mode_id;
    private double premium; //round up
    private double original_premium;
    private string created_by;
    private DateTime created_on;
    private string updated_by;
    private DateTime updated_on;
    #endregion
    #region 'Properties'

    public DateTime Updated_on
    {
        get { return updated_on; }
        set { updated_on = value; }
    }

    public string Updated_by
    {
        get { return updated_by; }
        set { updated_by = value; }
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

    public double Original_premium
    {
        get { return original_premium; }
        set { original_premium = value; }
    }


    public double Premium
    {
        get { return premium; }
        set { premium = value; }
    }
    

    public int Pay_mode_id
    {
        get { return pay_mode_id; }
        set { pay_mode_id = value; }
    }


    public string Policy_id
    {
        get { return policy_id; }
        set { policy_id = value; }
    }


    public string Premium_id
    {
        get { return premium_id; }
        set { premium_id = value; }
    }


    #endregion
    #region 'Constructor'
    public bl_Policy_FP_Premium()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_Policy_FP_Premium(string premium_id, string policy_id, int pay_mode_id, double premium, double original_premium,
        string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.premium_id = premium_id;
        this.policy_id = policy_id;
        this.pay_mode_id = pay_mode_id;
        this.premium = premium;
        this.original_premium = original_premium;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}