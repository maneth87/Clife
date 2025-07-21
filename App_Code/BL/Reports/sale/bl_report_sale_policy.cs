using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_report_sale_app_by_status
/// </summary>
public partial class bl_report_sale_policy
{
    public string Policy_Number { get; set; }
    public string Customer_Name { get; set; }
    public string Customer_Contact { get; set; }
    public string Product_Name { get; set; }
    public double SumInsured { get; set; }
    public double Premium { get; set; }
    public string Mode { get; set; }
    public DateTime Due_Date { get; set; }
    public DateTime Effective_Date { get; set; }
    public DateTime Maturity_Date { get; set; }
    public string Policy_Status { get; set; }
    public string Sale_Agent { get; set; }


}