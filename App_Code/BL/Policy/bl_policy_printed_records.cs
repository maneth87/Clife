using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_printed_records
/// </summary>
public class bl_inform_letter_printed_records
{
	public bl_inform_letter_printed_records()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public decimal No { get; set; }
    public string Policy_ID { get; set; }
    public DateTime Printed_Date { get; set; }
    public string Printed_By { get; set; }
    public string Report_Type { get; set; }

    public bl_inform_letter_printed_records(string policy_id, DateTime printed_date, string printed_by, string report_type)
    {
        this.Policy_ID = policy_id;
        this.Printed_Date = printed_date;
        this.Printed_By = printed_by;
        this.Report_Type = report_type;
    }

}