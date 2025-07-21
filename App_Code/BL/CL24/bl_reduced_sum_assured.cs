using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_reduced_sum_assured
/// </summary>
public class bl_reduced_sum_assured
{
	public bl_reduced_sum_assured()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string ProductId { get; set; }
    public int CoverageYear { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public double Rate { get; set; }
}