using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_customer_lead_app_temp
/// </summary>
public class bl_customer_lead_app_temp:bl_customer_lead
{
	public bl_customer_lead_app_temp()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string CHANNEL_ITEM_ID { get; set; }
    public string CHANNEL_LOCATION_ID { get; set; }
    public string SALE_AGENT_ID { get; set; }
    public string SALE_AGENT_NAME { get; set; }
}