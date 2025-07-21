using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_Policy_FP
/// </summary>
public class bl_Policy_FP
{
    #region 'Local Variables'
    private string policy_id;
    private string customer_id;
    private string fp_plan_id;
    private double sum_insured1;
    private double sum_insured2;
    private double sum_insured3;
    private double sum_insured4;
    private int coverage_peroid;
    private int payment_peroid;
    private int age;
    private DateTime effective_date;
    private DateTime maturity_date;
    private DateTime created_on;
    private string created_by;
    private DateTime updated_on;
    private string updated_by;
    #endregion
    #region 'Properties'
    public string Updated_by
    {
        get { return updated_by; }
        set { updated_by = value; }
    }


    public DateTime Updated_on
    {
        get { return updated_on; }
        set { updated_on = value; }
    }

    public string Created_by
    {
        get { return created_by; }
        set { created_by = value; }
    }


    public DateTime Created_on
    {
        get { return created_on; }
        set { created_on = value; }
    }

    public DateTime Maturity_date
    {
        get { return maturity_date; }
        set { maturity_date = value; }
    }

    public DateTime Effective_date
    {
        get { return effective_date; }
        set { effective_date = value; }
    }

    public int Age
    {
        get { return age; }
        set { age = value; }
    }

    public int Payment_peroid
    {
        get { return payment_peroid; }
        set { payment_peroid = value; }
    }


    public int Coverage_peroid
    {
        get { return coverage_peroid; }
        set { coverage_peroid = value; }
    }


    public double Sum_insured4
    {
        get { return sum_insured4; }
        set { sum_insured4 = value; }
    }
    public double Sum_insured3
    {
        get { return sum_insured3; }
        set { sum_insured3 = value; }
    }
    public double Sum_insured2
    {
        get { return sum_insured2; }
        set { sum_insured2 = value; }
    }
    public double Sum_insured1
    {
        get { return sum_insured1; }
        set { sum_insured1 = value; }
    }


    public string Fp_plan_id
    {
        get { return fp_plan_id; }
        set { fp_plan_id = value; }
    }


    public string Customer_id
    {
        get { return customer_id; }
        set { customer_id = value; }
    }


    public string Policy_id
    {
        get { return policy_id; }
        set { policy_id = value; }
    }
    
    #endregion
    #region 'Constructor'
    public bl_Policy_FP()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_Policy_FP(string policy_id, string customer_id, string fp_plan_id, 
        double sum_insured1, double sum_insured2, double sum_insured3, double sum_insured4, 
        int coverage_peroid, int payment_peroid, int age, DateTime effective_date, 
        DateTime maturity_date, DateTime created_on, string created_by,
        DateTime updated_on, string updated_by)
    {
        this.policy_id = policy_id;
        this.customer_id = customer_id;
        this.fp_plan_id = fp_plan_id;
        this.sum_insured1 = sum_insured1;
        this.sum_insured2 = sum_insured2;
        this.sum_insured3 = sum_insured3;
        this.sum_insured4 = sum_insured4;
        this.coverage_peroid = coverage_peroid;
        this.payment_peroid = payment_peroid;
        this.effective_date = effective_date;
        this.maturity_date = maturity_date;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}