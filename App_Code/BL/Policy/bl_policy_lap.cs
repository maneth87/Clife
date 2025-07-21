using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_lap
/// </summary>
public class bl_policy_lap
{
    DateTime __NextDueDate;
    int __Period;
	public bl_policy_lap()
	{
		//
		// TODO: Add constructor logic here
		//
        __NextDueDate = new DateTime();
        __Period = 0;
	}
    #region Property
    public string PolicyID { get; set; }
    public string PolicyNumber { get; set; }
    public string CustomerID { get; set; }
    public string CustomerNameEn { get; set; }
    public string CustomerNameKh { get; set; }
    public string CustomerGender { get; set; }
    public DateTime CustomerDOB { get; set; }
    public string CustomerPhoneNumber { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime NextDueDate { get { return nextDueDate(); } }
    public int GracePeriod { get { return 30; }} //grace period 30 days after next due date
    public string ProductID { get; set; }
    public int PayMode { get; set; }
    public int CalculatedGracePeriod
    {
        get
        {
            return period();
        }
    }
    /// <summary>
    /// Current date use for calculating period between current date and next due date
    /// </summary>
    public DateTime CurrentDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string Remarks { get; set; }
    public string PolicyStatus { get; set; }
    #endregion Property
    #region Private function
    private DateTime nextDueDate()
    {
        if (DueDate != null)
        {
            DateTime myNextDue = new DateTime();
            myNextDue = Helper.GetDueDateList(DueDate, PayMode)[1];// index zero is the first due date
            __NextDueDate = Calculation.GetNext_Due(myNextDue, DueDate, DueDate);

        }
        return __NextDueDate;
    }
    private int period()
    {
        int policyPeriod = 0;
        if (NextDueDate != null)
        {
            policyPeriod = CurrentDate.Subtract(NextDueDate).Days;

        }
        return policyPeriod;

    }
    #endregion Private function

}