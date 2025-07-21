using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
/// <summary>
/// Summary description for Notification
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Notification : System.Web.Services.WebService {

    public Notification () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
       
    }
   [WebMethod]
    public void Process()
    {
        da_before_due_notificaton.ProcessBeforDueNotification(DateTime.Now.Date);
       // da_before_due_notificaton.ProcessBeforDueNotification(new DateTime(2019,3,12));
        //da_before_due_notificaton.ProcessBeforDueNotification(new DateTime(2019, 3, 20));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="date">[Date format: DD/MM/YYYY]</param>
   [WebMethod]
   public void ProcessByDate(string date)
   {
       da_before_due_notificaton.ProcessBeforDueNotification(Helper.FormatDateTime(date));
       // da_before_due_notificaton.ProcessBeforDueNotification(new DateTime(2019,3,12));
       //da_before_due_notificaton.ProcessBeforDueNotification(new DateTime(2019, 3, 20));
   }
   [WebMethod]
   public void SendMailNotification()
   {
       da_before_due_notificaton.SendCustomerNotificationEmail(DateTime.Now.Date, "WEB_SERVICE");
   }
   [WebMethod]
   public void SendMailNotificationByDate(string date)
   {
       da_before_due_notificaton.SendCustomerNotificationEmail(Helper.FormatDateTime(date), "WEB_SERVICE");
   }
   [WebMethod]
   public void SendSMSNotification()
   {
       da_before_due_notificaton.SendCustomerNotificationSMS(DateTime.Now.Date, "WEB_SERVICE");
     
   }
   [WebMethod]
   public void SendSMSNotificationByDate(string date)
   {
       da_before_due_notificaton.SendCustomerNotificationSMS(Helper.FormatDateTime(date), "WEB_SERVICE");

   }


    #region policy lap
   [WebMethod]
   public string ProcessPolicyLap()
   {
       string date = DateTime.Now.ToString("dd/MM/yyyy");
       #region
       
       //string result = "";
       //string str_log = "";
      
       //int no = 1;

       //HSSFWorkbook hssfworkbook = new HSSFWorkbook();
       //HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
       //Helper.excel.Sheet = sheet1;
       ////design row header
       //Helper.excel.HeaderText = new string[] { "No.","Policy Number","Customer ID","Customer Name (EN)","Customer Name (KH)","DOB",
       //    "Gender","Phone","Email","Pay Mode","Product ID","Effective Date","Due Date","Next Due Date","Grace Period","Over Grace Period" };
       //Helper.excel.generateHeader();

       //List<bl_policy_lap> lapList=da_policy_lap.UpdatePolicyToLap(Helper.FormatDateTime(date),"Webservice",DateTime.Now,"System update policy status to LAP");

       //foreach (bl_policy_lap lap in lapList)
       //{

       //    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(no);

       //    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
       //    Cell1.SetCellValue(no);

       //    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
       //    Cell2.SetCellValue(lap.PolicyNumber);

       //    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
       //    Cell3.SetCellValue(lap.CustomerID);


       //    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
       //    Cell4.SetCellValue(lap.CustomerNameEn);

       //    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
       //    Cell5.SetCellValue(lap.CustomerNameKh);

       //    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
       //    Cell6.SetCellValue(lap.CustomerDOB.ToString("dd/MM/yyyy"));
       //    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
       //    Cell7.SetCellValue(lap.CustomerGender);
       //    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
       //    Cell8.SetCellValue(lap.CustomerPhoneNumber);
       //    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
       //    Cell9.SetCellValue(lap.CustomerEmail);
       //    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
       //    Cell10.SetCellValue(Helper.GetPaymentModeEnglish( lap.PayMode));
       //    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
       //    Cell11.SetCellValue(lap.ProductID);
       //    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
       //    Cell12.SetCellValue(lap.EffectiveDate.ToString("dd/MM/yyyy"));
       //    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
       //    Cell13.SetCellValue(lap.DueDate.ToString("dd/MM/yyyy"));
       //    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
       //    Cell14.SetCellValue(lap.NextDueDate.ToString("dd/MM/yyyy"));
       //    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
       //    Cell15.SetCellValue(lap.GracePeriod);
       //    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
       //    Cell16.SetCellValue(lap.CalculatedGracePeriod - lap.GracePeriod);
       //    no += 1;
       //}


       //string filename = Server.MapPath("Temp") + "\\UpdatedPolicyLap" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
      
       //MemoryStream file = new MemoryStream();
       //hssfworkbook.Write(file);

       ////write to file
       //FileStream fileS = new FileStream(filename, FileMode.Create, FileAccess.Write);
       //file.WriteTo(fileS);
       //fileS.Close();
       //file.Close();

       //try
       //{
       //    /*send summary email*/
       //    EmailSender mail = new EmailSender();
       //    mail.From = "camlifesys@camlife.com.kh";
       //    mail.To = "policyholderservice@camlife.com.kh";
       //    //mail.To = "maneth.som@camlife.com.kh";
       //    mail.BCC = "maneth.som@camlife.com.kh";
       //    mail.Subject = "List of policies lap on "+Helper.FormatDateTime( date).ToString("dd/MMM/yyyy");
       //    mail.Message = "Dear Team, please kindly find the attached file for list of policy lap on " + Helper.FormatDateTime( date).ToString("dd/MMM/yyyy");
       //    mail.Host = "mail.camlife.com.kh";
       //    mail.Port = 587;
       //    mail.Password = "admin1!2cdef";
       //    mail.Attachment = filename;

       //    if (mail.SendMail(mail))
       //    {
       //        str_log = "[List of Updated Policies Lap Status.] was sent to " + mail.To + " successfully.";
       //        result = "System had sent email to policyholderservice successfully.";
       //    }
       //    else
       //    {
       //        str_log = "[List of Updated Policies Lap Status.] was sent to " + mail.To + " fail.";
       //        result = "System had sent email to policyholderservice success.";
       //    }
       //    try
       //    {
       //        File.Delete(filename);
       //    }
       //    catch (Exception)
       //    { }
       //}
       //catch (Exception ex)
       //{
       //    Log.AddExceptionToLog("Error function [ProcessPolicyLap(string date)] in class [Notification], detail:" + ex.Message);
       //    result = "System had sent email to policyholderservice fail.";
       //    str_log = "";
       //    try
       //    {
       //        File.Delete(filename);
       //    }
       //    catch (Exception)
       //    { }
       //}
       //if (str_log != "")//save log email
       //{
       //    Log.CreateLog("Email_Log", str_log);
       //}

       //return result;

       #endregion

       return ProcessPolicyLapByDate(date);
   }
   [WebMethod]
   public string ProcessPolicyLapByDate(string date)
   {
       string result = "";
       string str_log = "";

       int no = 1;

       HSSFWorkbook hssfworkbook = new HSSFWorkbook();
       HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
       Helper.excel.Sheet = sheet1;
       //design row header
       Helper.excel.HeaderText = new string[] { "No.","Policy Number","Customer ID","Customer Name (EN)","Customer Name (KH)","DOB",
           "Gender","Phone","Email","Pay Mode","Product ID","Effective Date","Due Date","Next Due Date","Grace Period","Over Grace Period" };
       Helper.excel.generateHeader();

       List<bl_policy_lap> lapList = da_policy_lap.UpdatePolicyToLap(Helper.FormatDateTime(date), "Webservice", DateTime.Now, "System update policy status to LAP");

       foreach (bl_policy_lap lap in lapList)
       {

           HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(no);

           HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
           Cell1.SetCellValue(no);

           HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
           Cell2.SetCellValue(lap.PolicyNumber);

           HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
           Cell3.SetCellValue(lap.CustomerID);


           HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
           Cell4.SetCellValue(lap.CustomerNameEn);

           HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
           Cell5.SetCellValue(lap.CustomerNameKh);

           HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
           Cell6.SetCellValue(lap.CustomerDOB.ToString("dd/MM/yyyy"));
           HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
           Cell7.SetCellValue(lap.CustomerGender);
           HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
           Cell8.SetCellValue(lap.CustomerPhoneNumber);
           HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
           Cell9.SetCellValue(lap.CustomerEmail);
           HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
           Cell10.SetCellValue(Helper.GetPaymentModeEnglish(lap.PayMode));
           HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
           Cell11.SetCellValue(lap.ProductID);
           HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
           Cell12.SetCellValue(lap.EffectiveDate.ToString("dd/MM/yyyy"));
           HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
           Cell13.SetCellValue(lap.DueDate.ToString("dd/MM/yyyy"));
           HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
           Cell14.SetCellValue(lap.NextDueDate.ToString("dd/MM/yyyy"));
           HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
           Cell15.SetCellValue(lap.GracePeriod);
           HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
           Cell16.SetCellValue(lap.CalculatedGracePeriod - lap.GracePeriod);
           no += 1;
       }


       string filename = Server.MapPath("Temp") + "\\UpdatedPolicyLap" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";

       MemoryStream file = new MemoryStream();
       hssfworkbook.Write(file);

       //write to file
       FileStream fileS = new FileStream(filename, FileMode.Create, FileAccess.Write);
       file.WriteTo(fileS);
       fileS.Close();
       file.Close();

       try
       {
           /*send summary email*/
           EmailSender mail = new EmailSender();
           mail.From = "camlifesys@camlife.com.kh";
           mail.To = "policyholderservice@camlife.com.kh";
           //mail.To = "maneth.som@camlife.com.kh";
           mail.BCC = "maneth.som@camlife.com.kh";
           mail.Subject = "List of policies lap on " + Helper.FormatDateTime(date).ToString("dd/MMM/yyyy");
           mail.Message = "Dear Team, please kindly find the attached file for list of policy lap on " + Helper.FormatDateTime(date).ToString("dd/MMM/yyyy");
           mail.Host = "mail.camlife.com.kh";
           mail.Port = 587;
           mail.Password = "admin1!2cdef";
           mail.Attachment = filename;

           if (mail.SendMail(mail))
           {
               str_log = "[List of Updated Policies Lap Status.] was sent to " + mail.To + " successfully.";
               result = "System had sent email to policyholderservice successfully.";
           }
           else
           {
               str_log = "[List of Updated Policies Lap Status.] was sent to " + mail.To + " fail.";
               result = "System had sent email to policyholderservice success.";
           }
           try
           {
               File.Delete(filename);
           }
           catch (Exception)
           { }
       }
       catch (Exception ex)
       {
           Log.AddExceptionToLog("Error function [ProcessPolicyLap(string date)] in class [Notification], detail:" + ex.Message);
           result = "System had sent email to policyholderservice fail.";
           str_log = "";
           try
           {
               File.Delete(filename);
           }
           catch (Exception)
           { }
       }
       if (str_log != "")//save log email
       {
           Log.CreateLog("Email_Log", str_log);
       }
       return result;
   }

    #endregion policy lap


   #region policy cl24
   [WebMethod]
   public string ProcessPolicy30DayBeforeDue()
   {
       string date = DateTime.Now.ToString("yyyy/M/dd "); 
       string result = "";
       string str_log = "";

       int no = 1;

       HSSFWorkbook hssfworkbook = new HSSFWorkbook();
       HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
       Helper.excel.Sheet = sheet1;
       //design row header
       Helper.excel.HeaderText = new string[] { "No.","Customer ID","Policy Number","Customer Name (EN)","Customer Name (KH)","DOB",
           "Gender","Phone","Effective Date","Due Date","Next Due Date","Pay Mode","Premium", "Product ID", "Product Title" };
       Helper.excel.generateHeader();

       List<bl_before_due_notification> policyList = da_policy_cl24.GetPolicyCL24BeforeDue(Helper.FormatDateTime(date), "Webservice", DateTime.Now, "System update policy status to 7 Days Before Due");
       if (policyList.Count > 0)
       {
           foreach (bl_before_due_notification.bl_before_due_notification_sub policy in policyList)
           {

               HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(no);

               HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
               Cell1.SetCellValue(no);

               HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
               Cell2.SetCellValue(policy.CustomerID);

               HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
               Cell3.SetCellValue(policy.PolicyNumber + policy.Policy_Number_Sub);

               HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
               Cell4.SetCellValue(policy.NameEN);

               HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
               Cell5.SetCellValue(policy.NameKH);

               HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
               Cell6.SetCellValue(policy.DOB.ToString("dd/MM/yyyy"));

               HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
               Cell7.SetCellValue(policy.Gender);

               HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
               Cell8.SetCellValue(policy.PhoneNumber);

               HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
               Cell9.SetCellValue(policy.EffectiveDate.ToString("dd/MM/yyyy"));

               HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
               Cell10.SetCellValue(policy.DueDate.ToString("dd/MM/yyyy"));

               HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
               Cell11.SetCellValue(policy.NextDueDate.ToString("dd/MM/yyyy"));

               HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
               Cell12.SetCellValue(Helper.GetPaymentModeEnglish(policy.PayMode));

               HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
               Cell13.SetCellValue(policy.Premium);

               HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
               Cell14.SetCellValue(policy.ProductID);

               HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
               Cell15.SetCellValue(policy.Product_Title);

               no += 1;
           }


           string filename = Server.MapPath("Temp") + "\\PolicyCreditLife30DayBeforeDue" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";

           MemoryStream file = new MemoryStream();
           hssfworkbook.Write(file);

           //write to file
           FileStream fileS = new FileStream(filename, FileMode.Create, FileAccess.Write);
           file.WriteTo(fileS);
           fileS.Close();
           file.Close();

           try
           {
               /*send summary email*/
               EmailSender mail = new EmailSender();
               mail.From = "camlifesys@camlife.com.kh";
               mail.To = "policyholderservice@camlife.com.kh";
               //mail.To = "sun.meas@camlife.com.kh";
               mail.BCC = "sun.meas@camlife.com.kh";
               //mail.BCC = "maneth.som@camlife.com.kh";
               mail.Subject = "List of policies 30 Days Before Due on " + Helper.FormatDateTime(date).ToString("dd/MMM/yyyy");
               mail.Message = "Dear Team, please kindly find the attached file for list of policy 30 Days Before Due on " + Helper.FormatDateTime(date).ToString("dd/MMM/yyyy");
               mail.Host = "mail.camlife.com.kh";
               mail.Port = 587;
               mail.Password = "admin1!2cdef";
               mail.Attachment = filename;

               if (mail.SendMail(mail))
               {
                   str_log = "[List of Updated Policies 30 Days Before Due Status.] was sent to " + mail.To + " successfully.";
                   result = "System had sent email to policyholderservice successfully.";
               }
               else
               {
                   str_log = "[List of Updated Policies 30 Days Before Due Status.] was sent to " + mail.To + " fail.";
                   result = "System had sent email to policyholderservice success.";
               }
               try
               {
                   File.Delete(filename);
               }
               catch (Exception)
               { }
           }
           catch (Exception ex)
           {
               Log.AddExceptionToLog("Error function [ProcessPolicy30DayBeforeDue()] in class [Notification], detail:" + ex.Message);
               result = "System had sent email to policyholderservice fail.";
               str_log = "";
               try
               {
                   File.Delete(filename);
               }
               catch (Exception)
               { }
           }
           if (str_log != "")//save log email
           {
               Log.CreateLog("Email_Log", str_log);
           }
       }
       
       return result;
   }
   #endregion

    #region PREMIUM REPORT - ALL PRODUCT 
   [WebMethod]
   public void RenewalPremiumReportDailyService()
   
   {
       da_product_premium_report.GetRenewalPremiumReportByAllProduct(DateTime.Now.Date, "WEB_SERVICE");
   }
    #endregion


    #region send micro policy renewal notification 
   [WebMethod]
   public void SendMailNotificationMicroPolicyRenewal()
   {
       //banca policy from hattha
       //da_micro_policy_expiring.SendSummaryNotification("791D3296-82D0-4F07-AC62-B5C358742E2B", "", new DateTime(2023, 3, 31), "WEBSERVICE");
       da_micro_policy_expiring.SendSummaryNotification("791D3296-82D0-4F07-AC62-B5C358742E2B", "", DateTime.Now.Date, "WEBSERVICE");
   }
    #endregion send micro policy renewal notification
}
