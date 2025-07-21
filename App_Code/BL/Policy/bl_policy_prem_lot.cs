using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_prem_lot
/// </summary>
public class bl_policy_prem_lot
{
	public bl_policy_prem_lot()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Policy_ID { get; set; }
    public int Prem_Year { get; set; }
    public int Prem_Lot { get; set; }
    public int Pay_Mod { get; set; }
    public DateTime Due_Date { get; set; }

}