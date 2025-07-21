using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_Policy_FP_Premium_Pay
/// </summary>
public class bl_Policy_FP_Premium_Pay
{
    #region 'Local Variable'
    private string policy_premium_pay_id;
    private string policy_id;
    private DateTime due_date;
    private DateTime pay_date;
    private double amount;
    private int pay_mode_id;
    private string sale_agent_id;
    private int policy_year;
    private int premium_lot;
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


    public int Premium_lot
    {
        get { return premium_lot; }
        set { premium_lot = value; }
    }


    public int Policy_year
    {
        get { return policy_year; }
        set { policy_year = value; }
    }


    public string Sale_agent_id
    {
        get { return sale_agent_id; }
        set { sale_agent_id = value; }
    }


    public int Pay_mode_id
    {
        get { return pay_mode_id; }
        set { pay_mode_id = value; }
    }


    public double Amount
    {
        get { return amount; }
        set { amount = value; }
    }


    public DateTime Pay_date
    {
        get { return pay_date; }
        set { pay_date = value; }
    }


    public DateTime Due_date
    {
        get { return due_date; }
        set { due_date = value; }
    }


    public string Policy_id
    {
        get { return policy_id; }
        set { policy_id = value; }
    }


    public string Policy_premium_pay_id
    {
        get { return policy_premium_pay_id; }
        set { policy_premium_pay_id = value; }
    }


    #endregion
    #region 'Constructor'
    public bl_Policy_FP_Premium_Pay()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_Policy_FP_Premium_Pay(string policy_premium_pay_id, string policy_id, DateTime due_date, DateTime pay_date, double amount, int pay_mode_id, string sale_agent_id,
        int policy_year, int premium_lot, string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.policy_premium_pay_id = policy_premium_pay_id;
        this.policy_id = policy_id;
        this.due_date = due_date;
        this.pay_date = pay_date;
        this.amount = amount;
        this.pay_mode_id = pay_mode_id;
        this.sale_agent_id = sale_agent_id;
        this.policy_year = policy_year;
        this.premium_lot = premium_lot;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}