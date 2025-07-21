using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_reinsurance
/// </summary>
public class bl_reinsurance
{
	public bl_reinsurance()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties
    public int NO { get; set; }
    public string CustomerID { get; set; }
    public string PolicyNumber { get; set; }
    public string PolicyID { get; set; }
    public string InsuredNameKH { get; set; }
    public string InsuredNameEN { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeInsure { get; set; }
    public int CurrentAge { get; set; }
    public string Gender { get; set; }
    public string ProductID { get; set; }
    public string ProductName { get; set; }
    public int CoveragePeriod { get; set; }
    public int PaymentPeriod { get; set; }
    public string PlanCode { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime IssuedDate { get; set; }
    public int PolicyYear { get; set; }
    public int TimeOfPayment { get; set; }
    public int EMPercent { get; set; }
    public int TotalEMPercent { get; set; }
    public int EMPercentVarian { get; set; }
    public string Status { get; set; }
    public double SumInsure { get; set; }
    public double TotalSumInsure { get; set; }
    public double Retention { get; set; }
    public double AutomaticSumInsure { get; set; }
    public double Faculative { get; set; }
    public double SumInsureVarian { get; set; }
    public string ProductType { get; set; }
    public int ProductTypeID { get; set; }
    public string PayMode { get; set; }
    public string Remarks { get; set; }
    public string Others { get; set; }
    public string Created_By { get; set; }
    public DateTime Created_On { get; set; }

    #endregion
}