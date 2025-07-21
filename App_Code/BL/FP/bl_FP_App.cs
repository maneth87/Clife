using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_PIP_App
/// </summary>
public class bl_FP_App
{
    #region'local variable declaration and properties'

    private string app_id;

    public string App_id
    {
        get { return app_id; }
        set { app_id = value; }
    }
    private string app_number;

    public string App_number
    {
        get { return app_number; }
        set { app_number = value; }
    }
    private int payment_mode;

    public int Payment_mode
    {
        get { return payment_mode; }
        set { payment_mode = value; }
    }
    private string policy_number;

    public string Policy_number
    {
        get { return policy_number; }
        set { policy_number = value; }
    }
    private int sale_agent_id;

    public int Sale_agent_id
    {
        get { return sale_agent_id; }
        set { sale_agent_id = value; }
    }
    private DateTime entry_date;

    public DateTime Entry_date
    {
        get { return entry_date; }
        set { entry_date = value; }
    }
    private string created_note;

    public string Created_note
    {
        get { return created_note; }
        set { created_note = value; }
    }
    private string created_by;

    public string Created_by
    {
        get { return created_by; }
        set { created_by = value; }
    }
    private DateTime created_on;

    public DateTime Created_on
    {
        get { return created_on; }
        set { created_on = value; }
    }
    private string updated_by;

    public string Updated_by
    {
        get { return updated_by; }
        set { updated_by = value; }
    }
    private DateTime updated_on;

    public DateTime Updated_on
    {
        get { return updated_on; }
        set { updated_on = value; }
    }
   
    #endregion

    #region 'constructor'
    public bl_FP_App()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bl_FP_App(string app_id, string app_number, int payment_mode, string policy_number, string created_note, int sale_agent_id, DateTime entry_date, string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.app_id = app_id;
        this.app_number = app_number;
        this.payment_mode = payment_mode;
        this.policy_number = policy_number;
        this.created_note = created_note;
        this.sale_agent_id = sale_agent_id;
        this.entry_date = entry_date;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}