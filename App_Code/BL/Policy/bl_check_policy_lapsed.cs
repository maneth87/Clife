using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_check_policy_lapsed
/// </summary>
public partial class bl_check_policy_lapsed
{
    public string policy_num { get; set; }
    public string customer { get; set; }
    public string gender { get; set; }
    public int year_time { get; set; }
    public string mode { get; set; }
    public decimal sum_insure { get; set; }
    public decimal premium { get; set; }
    public DateTime due_date { get; set; }
    public decimal interest { get; set; }
    public decimal prem_interest { get; set; }
    public int dur_days { get; set; }
    public int dur_months { get; set; }
    public DateTime next_due { get; set; }
    public string product { get; set; }

}