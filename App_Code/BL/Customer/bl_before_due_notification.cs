using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_before_due_notification
/// </summary>
public class bl_before_due_notification
{
    DateTime __nextDueDate;
    int __numberOfNexeDueDay;
	public bl_before_due_notification()
	{
		//
		// TODO: Add constructor logic here
		//
        __nextDueDate = new DateTime();
        __numberOfNexeDueDay = 0;
    }
    #region //Properties
    public string CustomerID { get; set; }
    public string PolicyNumber { get; set; }
    public string NameEN { get; set; }
    public string NameKH { get; set; }
    public DateTime DOB { get; set; }
    public string Gender { get; set; }
    public string Title { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime EffectiveDate { get; set; }
    public int PayMode { get; set; }
    public DateTime DueDate { get; set; }
    public string ProductID { get; set; }
    public string MessageText { get; set; }
    /// <summary>
    /// CompareDate use to calculate NumberOfNextDueDay
    /// </summary>
    public DateTime CompareDate { get; set; }
    /// <summary>
    /// Next Due Date is computed automaticall when Due date and pay mode are not null
    /// </summary>
    public DateTime NextDueDate
    {
        get {
            return nextDue();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public int NumberOfNextDueDay
    {
        get {
            //return __numberOfNexeDueDay;
            return numberOfNextDueDays();
        }
    }
    public string CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string NotificationStatus { get; set; }
    public string Remarks { get; set; }

    #endregion

    #region //Local Function
    private DateTime nextDue()
    {
        
        if (DueDate != null)
        {
            DateTime myNextDue = new DateTime();
            myNextDue = Helper.GetDueDateList(DueDate, PayMode)[1];// index zero is the first due date
            __nextDueDate = Calculation.GetNext_Due(myNextDue, DueDate, DueDate);

          // __numberOfNexeDueDay = __nextDueDate.Date.Subtract(CompareDate.Date).Days;
         
        }
        return __nextDueDate;
    }
    private int numberOfNextDueDays()
    {
        if (NextDueDate != null)
        {
            return (NextDueDate.Date.Subtract(CompareDate).Days);
        }
        else
        {
            return 0;
        }
    }
   
    #endregion

    //create new class which inherit from  bl_before_due_notification for bl_before_due_notification sub
    public class bl_before_due_notification_sub : bl_before_due_notification
    {
        //add one more property
        public double Premium { get; set; }
        public string App_Number { get; set; }
        public string Policy_Number_Sub { get; set; }
        public string Product_Title { get; set; }
    }
}