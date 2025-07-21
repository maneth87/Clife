using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_channel_sale_agent
/// </summary>
public class bl_channel_sale_agent
{
	public bl_channel_sale_agent()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string ID { get; set; }
    public string USER_NAME { get; set; }
    public string SALE_AGENT_ID { get; set; }
    public string CHANNEL_LOCATION_ID { get; set; }
    public DateTime CREATED_ON { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime UPDATED_ON { get; set; }
    public string UPDATED_BY { get; set; }
    public string REMARKS { get; set; }
}