using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_left_over_amount
/// </summary>
public class bl_left_over_amount
{
	public bl_left_over_amount()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Left_Over_Amount_ID { get; set; }
    public string Policy_ID { get; set; }
    public double Received_Amount { get; set; }
    public double Prem_Amount { get; set; }
    public double Prem_Amount_Paid { get; set; }
    public double Prem_Amount_Left_Over { get; set; }
    public double Left_Over_Substract { get; set; }
    public DateTime Received_Date { get; set; }
    public string Created_By { get; set; }
    public DateTime Created_On { get; set; }
    public int Status_Used { get; set; }
    public string Official_Receipt_ID { get; set; }
}