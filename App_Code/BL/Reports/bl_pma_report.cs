using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_pma_report
/// </summary>
public class bl_pma_report
{
    private string _id;
	public bl_pma_report()
	{
		//
		// TODO: Add constructor logic here
		//
        _id = GetId();
        if (ProductCode == null)
            ProductCode = "";
	}

    public string ID { get { return _id; } set { _id = value; } }
    public string PolicyNumber { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime PayDate { get; set; }
    public double SA { get; set; }
    public double Premium { get; set; }
    public double AnnualPremium { get; set; }
    public double DiscountAmount { get; set; }
    public double Amount { get; set; }
    public double EM { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public string PayMode { get; set; }
    public int PayYear { get; set; }
    public int PayLot { get; set; }
    public int NumberPolicy { get; set; }
    public string PolicyRange { get; set; }
    public DateTime ReportDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    private string GetId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_PMA_REPORT" }, { "FIELD", "ID" } });
    }
}