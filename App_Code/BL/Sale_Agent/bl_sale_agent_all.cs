using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_sale_agent_all
/// </summary>
public class bl_sale_agent_all:bl_sale_agent_ordinary
{
	public bl_sale_agent_all()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int Sale_Agent_Type { get; set; }
    public string Sale_Agent_Type_Name { get; set; }
    public string Full_Name_KH { get; set; }
    public string Full_Name_EN{get;set;}
    public string Mobile_Phone { get; set; }
    public string Home_Phone { get; set; }
    public string Email { get; set; }
    public string Created_Note { get; set; }
    public string Resigned { get; set; }
}