using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_Policy_FP_Plan
/// </summary>
public class bl_Policy_FP_Plan
{
    #region 'Local Variables'
    private string fp_plan_id;
    private string fp_Plan_name;
    private double sum_insured1;
    private double sum_insured2;
    private double sum_insured3;
    private double sum_insured4;
    private int coverage_peroid;
    private int payment_peroid;
    private DateTime created_on;
    private string created_by;
    private DateTime updated_on;
    private string updated_by;
    #endregion
    #region 'Properties'
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
    public string Fp_Plan_name
    {
        get { return fp_Plan_name; }
        set { fp_Plan_name = value; }
    }


    public string Fp_plan_id
    {
        get { return fp_plan_id; }
        set { fp_plan_id = value; }
    }


    #endregion
    #region 'Constructor'
    public bl_Policy_FP_Plan()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_Policy_FP_Plan(string fp_plan_id, string fp_plan_name, double sum_insured1, double sum_insured2, double sum_insured3, double sum_insured4,
        int coverage_peroid, int payment_peroid, DateTime created_on, string created_by, DateTime updated_on, string updated_by)
    {
        this.fp_plan_id = fp_plan_id;
        this.fp_Plan_name = fp_plan_name;
        this.sum_insured1 = sum_insured1;
        this.sum_insured2 = sum_insured2;
        this.sum_insured3 = sum_insured3;
        this.sum_insured4 = sum_insured4;
        this.coverage_peroid = coverage_peroid;
        this.payment_peroid = payment_peroid;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}