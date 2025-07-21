using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_before_due_notificaton
/// </summary>
public class da_before_due_notificaton
{
    public da_before_due_notificaton()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Filter only next 7 & 15 due days only 
    /// </summary>
    /// <param name="process_on_date">This date use for calculate number of next due days</param>
    /// <returns></returns>
    public static bool ProcessBeforDueNotification(DateTime process_on_date)
    {
        bool status = false;
        try
        {
            System.Data.DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CUSTOMER_BEFORE_DUE_NOTIFICATION]", new string[,] { },

                "da_before_due_notification=>ProcessBeforDueNotification(DateTime process_on_date)");
            List<bl_before_due_notification> listBeforeDue = new List<bl_before_due_notification>();
            bl_before_due_notification obj_before_due;
            string[] arrPhone;

            if (tbl.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in tbl.Rows)//.Select(" en_name='SANN VISAL'"))
                {
                    arrPhone = row["phone"].ToString().Replace(" ", "").Replace("​", "").Replace(" ", "").Split('/'); // the second and third replacement is replace khmer unicode space
                    foreach (string phone in arrPhone)
                    {
                        if (phone.Trim() != "")
                        {
                            obj_before_due = new bl_before_due_notification();

                            obj_before_due.DueDate = Convert.ToDateTime(row["due_date"].ToString());
                            obj_before_due.CompareDate = process_on_date;
                            obj_before_due.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                            obj_before_due.PayMode = Convert.ToInt32(row["pay_mode"].ToString());

                            if (obj_before_due.NumberOfNextDueDay == 7 || obj_before_due.NumberOfNextDueDay == 15)
                            {
                                if (IsExist(row["policy_number"].ToString().Trim(), phone.Trim(), DateTime.Now.Date, obj_before_due.NextDueDate, obj_before_due.NumberOfNextDueDay) == false)
                                {

                                    obj_before_due.NameEN = row["en_name"].ToString();
                                    obj_before_due.Email = row["email"].ToString();
                                    obj_before_due.PolicyNumber = row["policy_number"].ToString();
                                    obj_before_due.CustomerID = row["customer_id"].ToString();

                                    obj_before_due.Gender = row["GENDER"].ToString();
                                    obj_before_due.NameKH = row["kh_name"].ToString();
                                    obj_before_due.DOB = Convert.ToDateTime(row["birth_date"].ToString());
                                    obj_before_due.Title = row["title"].ToString();
                                    obj_before_due.PhoneNumber = phone;// row["phone"].ToString();
                                    obj_before_due.CreatedBy = "WEB_SERVICES";
                                    obj_before_due.CreatedDateTime = DateTime.Now;
                                    obj_before_due.NotificationStatus = "";
                                    obj_before_due.ProductID = row["product_id"].ToString();
                                    obj_before_due.Remarks = "";
                                    obj_before_due.MessageText = "";

                                    if (obj_before_due.NumberOfNextDueDay == 7)
                                    {
                                        obj_before_due.Remarks = "SEND MESSAGE";
                                        obj_before_due.MessageText = "គោរពជូន" + obj_before_due.Title + ' ' + obj_before_due.NameKH + " សូមមេត្តាបង់បុព្វលាភមុនថ្ងៃទី " + obj_before_due.NextDueDate.Date.ToString("dd/MM/yyyy") + "។ ប្រសិនបើលោកអ្នកបានបង់រួចហើយសូមរំលងសារនេះ។ព័ត៌មានបន្ថែម សូមទាក់ទង ខេម ឡៃហ្វ 061431111";
                                        status = Save(obj_before_due);
                                    }
                                    else if (obj_before_due.NumberOfNextDueDay == 15)
                                    {
                                        obj_before_due.Remarks = "SEND E-MAIL";
                                        obj_before_due.MessageText = "Dear " + (obj_before_due.Gender.Trim().ToUpper() == "M" ? "Mr." : obj_before_due.Gender.Trim().ToUpper() == "F" ? "Ms." : "") + row["en_name"].ToString() + ","
                                    + "<br/><br/> Trust this email will find you and your family in good health."
                                    + "<br/><br/> Please kindly be informed that your policy will be due on " + obj_before_due.NextDueDate.Date.ToString("dd/MM/yyyy") + ". Please make the payment before the due date through our channels as in the attachment file. "
                                    + "If you have already paid, please ignore this email."
                                    + "<br/><br/> Should you have any questions, please contact our friendly policyholder service unit at 023/061 431 111. Thank you for choosing Camlife as your life insurance partner."
                                    + "<br/><br/> Best Regards, "
                                    + "<br/> Policyholder Service Unit "
                                            //+ "<br/> CAMBODIA LIFE INSURANCE PLC. | Building No. 30, Street 432, Group 34, Sangkat Tuol Tompoung 1, Khan Chamkar Morn, Phnom Penh 12310.";
                                    + "<br/> CAMBODIA LIFE MICRO INSURANCE “CAMLIFE” PLC. | Building No. 30, Street 432, Group 34, Sangkat Tuol Tompoung 1, Khan Chamkar Morn, Phnom Penh 12310.";

                                        status = Save(obj_before_due);
                                        break; //save on mail only one time
                                    }

                                    //if (obj_before_due.NumberOfNextDueDay == 7)
                                    //{
                                    //    status = Save(obj_before_due);
                                    //}
                                    //else if (obj_before_due.NumberOfNextDueDay == 15)
                                    //{
                                    //    status = Save(obj_before_due);
                                    //    break; //save on mail only one time
                                    //}
                                }

                            }
                        }
                    }


                }
            }
            else
            {
                Log.AddExceptionToLog("Information: There is no record of 7-days before due.");
            }
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function[ProcessBeforDueNotification(DateTime process_on_date)] in class [da_before_due_notification], detail:" + ex.Message, "WEB_SERVICES");
        }
        return status;
    }
    public static bool Save(bl_before_due_notification obj_before_due)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CUSTOMER_UPLOAD_NOTIFICATION", new string[,] { 
            {"@CUSTOMER_ID",obj_before_due.CustomerID},
            {"@POLICY_NUMBER", obj_before_due.PolicyNumber},
            {"@EN_NAME", obj_before_due.NameEN},
            {"@KH_NAME", obj_before_due.NameKH},
            {"@GENDER", obj_before_due.Gender},
            {"@EMAIL", obj_before_due.Email},
            {"@PHONE", obj_before_due.PhoneNumber},
            {"@CREATED_BY", obj_before_due.CreatedBy},
            {"@CREATED_ON", obj_before_due.CreatedDateTime+""},
            {"@TITLE", obj_before_due.Title},
            {"@DOB", obj_before_due.DOB+""},
            {"@EFFECTIVE_DATE", obj_before_due.EffectiveDate+""},
            {"@DUE_DATE", obj_before_due.DueDate+""},
            {"@NEXT_DUE", obj_before_due.NextDueDate+""},
            {"@NEXT_DUE_DAY", obj_before_due.NumberOfNextDueDay+""},
            {"@PRODUCT_ID", obj_before_due.ProductID+""},
            {"@REMARKS", obj_before_due.Remarks},
            {"@NOTIFICATION_STATUS", obj_before_due.NotificationStatus},
            {"@MESSAGE_TEXT", obj_before_due.MessageText}
            }, "function [Save(bl_before_due_notification obj_before_due)], class [da_before_due_notification]");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [Save(bl_before_due_notification obj_before_due)] in class [da_before_due_notification], detail:" + ex.Message, obj_before_due.CreatedBy);
        }
        return status;
    }
    /// <summary>
    /// Delete record in table Ct_Customer_Upload_Notification by record id
    /// </summary>
    /// <param name="record_id"></param>
    /// <returns></returns>
    public static bool Delete(Int32 record_id, string user_name)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_CUSTOMER_UPLOAD_NOTIFICATION_BY_ID", new string[,] { 
            {"@RECORD_ID", record_id+""}
            }, "Function [Delete(string record_id)], class [da_before_due_notification]");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [Delete(string record_id)] in class [da_before_due_notification], detail:" + ex.Message, user_name);
        }
        return status;
    }
    /// <summary>
    /// Update before due by record id
    /// </summary>
    /// <param name="record_id">Record will be updated base on this record_id</param>
    /// <param name="remarks">New value</param>
    /// <param name="notification_status">New value</param>
    /// <returns></returns>
    public static bool UpdateNotificationStatus(Int32 record_id, string updated_by, DateTime updated_on, string notification_status)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CUSTOMER_UPLOAD_NOTIFICATION_STATUS", new string[,] { 
            {"@RECORD_ID", record_id+""},
            {"@notification_status", notification_status},
            {"@UPDATED_BY", updated_by},
            {"@UPDATED_ON", updated_on+""}
            }, "[UpdateNotificationStatus(string record_id, string updated_by, DateTime updated_on, string notification_status)] in class [da_before_due_notification]");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [UpdateNotificationStatus(string remarks, string notification_status) in class [da_before_due_notification], detail:" + ex.Message, updated_by);
        }
        return status;
    }

    public static bool CheckDuplicatSendingEmail(string email, DateTime next_due)
    {
        bool result = true;
        try
        {

            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_Check_Duplicate_Sending_Email_By_Email_Next_Due", new string[,]{
               {"@Email", email},
               {"@Next_Due", next_due+""}
               }, "class [da_before_due_notification] ==> CheckDuplicatSendingEmail(string email, DateTime next_due)");

            if (tbl.Rows.Count > 0)
            {

                if (tbl.Rows[0][0].ToString().Trim() != "0")
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [CheckDuplicatSendingEmail(string email, DateTime next_due)] in class [da_before_due_notification], detail:" + ex.Message, "");
            result = false;
        }
        return result;
    }

    public static DataTable GetCustomerNotification(DateTime created_on_from, DateTime created_on_to)
    {
        DataTable tbl_result = new DataTable();
        try
        {
            tbl_result = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CUSTOMER_UPLOAD_NOTIFICATION", new string[,] {
            {"@CREATED_ON_FROM",created_on_from+""},
            {"@CREATED_ON_TO",created_on_to+""}
            }, "[da_before_due_notification => GetCustomerNotification(DateTime created_on_from, DateTime created_on_to)]");

        }
        catch (Exception ex)
        {
            tbl_result = new DataTable();
            Log.AddExceptionToLog("Error funcion [GetCustomerNotification(DateTime created_on_from, DateTime created_on_to)] in class [da_before_due_notification], detail:" + ex.Message, "");
        }
        return tbl_result;
    }

    public static void SendCustomerNotificationEmail(DateTime send_date, string send_by)
    {
        int rowId = 0;
        try
        {
            DataTable tbl = GetCustomerNotification(send_date, send_date);
            EmailSender mail;
            int index = 0;
            int send_index = 0;

            string str_summary_email = "Customers and policies list which send remind 15 days before due on " + send_date.Date.ToString("dd/MM/yyyy") + " </br>";
            str_summary_email += "<table border='1'><th>No.</th><th>Cusotmer Name</th><th>Gender</th><th>Phone Number</th><th>Customer No.</th><th>Policy No.</th><th>Product</th><th>Due Date</th><th>Mail Address</th>";
            
            if (tbl.Rows.Count > 0)
            {
                saveLog(2, "<<STARTED @" + DateTime.Now);
                foreach (DataRow row in tbl.Select("next_due_day='15'"))//select only next_due_day =15
                {
                    index += 1;
                    rowId = Convert.ToInt32(row["id"].ToString().Trim());
                    if (row["email"].ToString().Trim() != "")
                    {
                        if (!CheckDuplicatSendingEmail(row["email"].ToString().Trim(), Convert.ToDateTime(row["next_due"].ToString())))
                        {

                            mail = new EmailSender();
                            mail.From = "policyholder@camlife.com.kh";
                            mail.To = row["email"].ToString().Trim();
                            mail.Subject = "Email Premium Due Notice";
                            mail.Message = row["message_text"].ToString().Trim();
                            mail.Host = "mail.camlife.com.kh";

                            mail.Port = 587;
                            mail.Password = "policyholder";
                            mail.Attachment = System.Web.HttpContext.Current.Server.MapPath(@"~\Bank 1.jpg");
                            if (mail.SendMail(mail))
                            {
                                send_index += 1;
                                InsertCustomerSendingEmail((row["gender"].ToString().Trim().ToUpper() == "M" ? "Mr." : row["gender"].ToString().Trim().ToUpper() == "F" ? "Ms." : "") + row["en_name"].ToString(),
                               row["email"].ToString().Trim(),
                               Convert.ToDateTime(row["next_due"].ToString()),
                               send_by,
                               System.DateTime.Now);

                                /* Update Notification Status to Success before delete the record */
                                UpdateNotificationStatus(Convert.ToInt32(row["id"].ToString().Trim()), send_by, send_date, "SUCCESS");

                                /*delete record*/
                                if (Delete(Convert.ToInt32(row["id"].ToString().Trim()), send_by))
                                {
                                    /*save log*/
                                    saveLog(2, index + ". [SUCCESS]\t PHONE:[" + row["phone"].ToString() + "]\t" + "\t MESSAGE TEXT:[" + row["message_text"].ToString() + "]" + System.Environment.NewLine + "==> [DELETE Ct_Customer_Upload_Notification SUCCESS] [RECORD ID: " + row["ID"].ToString() + "]");
                                }
                                else
                                {
                                    /*save log*/
                                    saveLog(2, index + ". [SUCCESS]\t PHONE:[" + row["phone"].ToString() + "]\t" + "\t MESSAGE TEXT:[" + row["message_text"].ToString() + "]" + System.Environment.NewLine + "==> [DELETE Ct_Customer_Upload_Notification FAIL] [RECORD ID: " + row["ID"].ToString() + "]");
                                }

                                /*Make up summary email*/
                                str_summary_email += "<tr><td>" + send_index + "</td><td>" + row["en_name"].ToString().Trim() + "</td><td>" + row["gender"].ToString().Trim() + "</td><td>" + row["phone"].ToString().Trim() + "</td><td>" + row["customer_id"].ToString().Trim() + "</td><td>" + row["policy_number"].ToString().Trim() + "</td><td>" + row["product_id"].ToString().Trim() + "</td><td>" + Convert.ToDateTime(row["next_due"].ToString().Trim()).Date.ToString("dd/MM/yyyy") + "</td><td>" + row["email"].ToString().Trim() + "</td></tr>";
                            }
                            else
                            {

                                /*update status*/
                                if (UpdateNotificationStatus(Convert.ToInt32(row["id"].ToString().Trim()), send_by, send_date, "FAIL"))
                                {
                                    /*save log*/
                                    saveLog(2, index + ". [FAIL]\t PHONE:[" + row["phone"].ToString() + "]\t" + "\t MESSAGE TEXT:[" + row["message_text"].ToString() + "]" + System.Environment.NewLine + "==> [UPDATE NOTIFICATION_STATUS to fail in Ct_Customer_Upload_Notification SUCCESS] [RECORD ID: " + row["ID"].ToString() + "]");
                                }
                                else
                                {
                                    /*save log*/
                                    saveLog(2, index + ". [FAIL]\t PHONE:[" + row["phone"].ToString() + "]\t" + "\t MESSAGE TEXT:[" + row["message_text"].ToString() + "]" + System.Environment.NewLine + "==> [UPDATE NOTIFICATION_STATUS to fail in Ct_Customer_Upload_Notification FAIL] [RECORD ID: " + row["ID"].ToString() + "]");
                                }

                            }

                        }
                        else
                        {

                            /*delete record*/
                            if (Delete(Convert.ToInt32(row["id"].ToString().Trim()), send_by))
                            {
                                /*save log*/
                                saveLog(2, index + "[INFO]\t[CANNOT SEND DUPLICATE E-MAIL]\t MESSAGE TEXT:[" + row["message_text"].ToString() + "]" + System.Environment.NewLine + "==> [DELETE Ct_Customer_Upload_Notification SUCCESS] [RECORD ID: " + row["ID"].ToString() + "]");
                            }
                            else
                            {
                                /*save log*/
                                saveLog(2, index + "[INFO]\t[CANNOT SEND DUPLICATE E-MAIL]\t MESSAGE TEXT:[" + row["message_text"].ToString() + "]" + System.Environment.NewLine + "==> [DELETE Ct_Customer_Upload_Notification FAIL] [RECORD ID: " + row["ID"].ToString() + "]");
                            }
                        }
                    }
                    else
                    {
                        UpdateNotificationStatus(rowId, send_by, send_date, "NO E-MAIL ADDRESS");
                       bool del= Delete(Convert.ToInt32(row["id"].ToString().Trim()), send_by);
                        saveLog(2, index + ". [INFO]\t[CUSTOMER NUMBER:" + row["CUSTOMER_ID"].ToString() + "]\t[NO HAVE EMAIL ADDRESS TO SEND], RECORD IS REMOVED FROM THE LIST " + (del== true ? "SUCCESSFULLY":"FAIL"));
                    }
                }

                str_summary_email += "</table>";
                if (send_index > 0)
                {
                    /*send summary email*/
                    mail = new EmailSender();
                    mail.From = "";
                    mail.From = "policyholder@camlife.com.kh";
                    mail.To = "policyholderservice@camlife.com.kh";
                    // mail.To = "maneth.som@camlife.com.kh";
                    mail.BCC = "maneth.som@camlife.com.kh";
                    mail.Subject = "Email Premium Due Notice";
                    mail.Message = str_summary_email;
                    mail.Host = "mail.camlife.com.kh";

                    mail.Port = 587;
                    mail.Password = "policyholder";

                    if (mail.SendMail(mail))
                    {
                        saveLog(2, "[INFO]\t Summary email was been send to " + mail.To + " successfully.");
                    }
                    else
                    {
                        saveLog(2, "[INFO]\t Summary email was been send to " + mail.To + " fail.");
                    }
                }
                saveLog(2, "ENDED @" + DateTime.Now + ">>");
            }
        }
        catch (Exception ex)
        {
            UpdateNotificationStatus(rowId, send_by, send_date, "FAIL");
            Log.AddExceptionToLog("Error function [SendCustomerNotificationEmail(DateTime send_date, string send_by)] in class [da_before_due_notification], detail:" + ex.Message, send_by);

        }
    }

    public static void SendCustomerNotificationSMS(DateTime send_date, string send_by)
    {
        int index = 0;
        string message = "";
        string messageCate = "";
        string[] phone = new string[] { };
        //string strResponse = "";
        string send_to_number = "";
        int send_index = 0;
        int rowID = 0;
        string phoneNumber = "";
        string str_summary_email = "Customer and policy list which send remind 7 days before due on " + send_date.Date.ToString("dd/MM/yyyy") + " </br>";
        str_summary_email += "<table border='1'><th>No.</th><th>Cusotmer Name</th><th>Gender</th><th>Phone Number</th><th>Customer No.</th><th>Policy No.</th><th>Product</th><th>Due Date</th><th>Mail Address</th><th>Status</th>";
        try
        {
            DataTable tbl = GetCustomerNotification(send_date, send_date);
            saveLog(1, "<<STARTED @" + DateTime.Now);

            if (tbl.Rows.Count > 0)
            {
                
                foreach (DataRow row in tbl.Select("next_due_day='7'"))//select only next_due_day =7
                {
                    try
                    {
                        rowID = Convert.ToInt32(row["id"].ToString().Trim());
                        if (row["phone"].ToString().Trim() != "")
                        {

                            phone = row["phone"].ToString().Trim().Replace(" ", "").Split('/');

                            if (phone.Length > 0) // PHONE NUMBER HAS MORE THAN ONE
                            {
                                foreach (string str_phone in phone)
                                {
                                    phoneNumber = str_phone;
                                    if (str_phone.Trim() != "")
                                    {
                                       
                                        SENDSMS sms;
                                        #region //Send message

                                        index += 1;
                                        message = row["message_text"].ToString().Trim();
                                        messageCate = "7DAY_BEFORE_DUE";
                                        sms = new SENDSMS();
                                        sms.Message = message;
                                        sms.MessageCate = messageCate;
                                        sms.PhoneNumber = str_phone;

                                        send_to_number = str_phone;
                                        try
                                        {
                                            if (sms.Send()) // (strResponse.Trim().ToLower() == "true")
                                            {
                                                send_index += 1;

                                                /* Update Notification Status to Success before delete the record */
                                                UpdateNotificationStatus(rowID, send_by, DateTime.Now, "SUCCESS");
                                                /*delete record*/
                                                if (Delete(Convert.ToInt32(row["id"].ToString().Trim()), send_by))
                                                {
                                                    /*save log*/
                                                    saveLog(1, index + ". [SUCCESS]\t PHONE:[" + str_phone + "]\t MESSAGE CATE.:[" + messageCate + "]\t MESSAGE TEXT:[" + message + "]" + System.Environment.NewLine + "==> [DELETE Ct_Customer_Upload_Notification SUCCESS] [RECORD ID: " + row["ID"].ToString() + "]");
                                                }
                                                else
                                                {
                                                    /*save log*/
                                                    saveLog(1, index + ". [SUCCESS]\t PHONE:[" + str_phone + "]\t MESSAGE CATE.:[" + messageCate + "]\t MESSAGE TEXT:[" + message + "]" + System.Environment.NewLine + "==> [DELETE Ct_Customer_Upload_Notification FAIL] [RECORD ID: " + row["ID"].ToString() + "]");
                                                }
                                                /*Make up summary email*/
                                                str_summary_email += "<tr><td>" + send_index + "</td><td>" + row["en_name"].ToString().Trim() + "</td><td>" + row["gender"].ToString().Trim() + "</td><td>" + row["phone"].ToString().Trim() + "</td><td>" + row["customer_id"].ToString().Trim() + "</td><td>" + row["policy_number"].ToString().Trim() + "</td><td>" + row["product_id"].ToString().Trim() + "</td><td>" + Convert.ToDateTime(row["next_due"].ToString().Trim()).Date.ToString("dd/MM/yyyy") + "</td><td>" + row["email"].ToString().Trim() + "</td><td>SUCCESS</td></tr>";
                                            }
                                            else
                                            {
                                                str_summary_email += "<tr><td>" + send_index + "</td><td>" + row["en_name"].ToString().Trim() + "</td><td>" + row["gender"].ToString().Trim() + "</td><td>" + row["phone"].ToString().Trim() + "</td><td>" + row["customer_id"].ToString().Trim() + "</td><td>" + row["policy_number"].ToString().Trim() + "</td><td>" + row["product_id"].ToString().Trim() + "</td><td>" + Convert.ToDateTime(row["next_due"].ToString().Trim()).Date.ToString("dd/MM/yyyy") + "</td><td>" + row["email"].ToString().Trim() + "</td><td>FAIL</td></tr>";

                                                /*update status*/
                                                if (UpdateNotificationStatus(Convert.ToInt32(row["id"].ToString().Trim()), send_by, send_date, "FAIL"))
                                                {
                                                    /*save log*/
                                                    saveLog(1, index + ". [FAIL]\t PHONE:[" + str_phone + "]\t MESSAGE CATE.:[" + messageCate + "]\t MESSAGE TEXT:[" + message + "]" + System.Environment.NewLine + "==> [UPDATE NOTIFICATION_STATUS to fail in Ct_Customer_Upload_Notification SUCCESS] [RECORD ID: " + row["ID"].ToString() + "]");
                                                }
                                                else
                                                {
                                                    /*save log*/
                                                    saveLog(1, index + ". [FAIL]\t PHONE:[" + str_phone + "]\t MESSAGE CATE.:[" + messageCate + "]\t MESSAGE TEXT:[" + message + "]" + System.Environment.NewLine + "==> [UPDATE NOTIFICATION_STATUS to fail in Ct_Customer_Upload_Notification FAIL] [RECORD ID: " + row["ID"].ToString() + "]");
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            str_summary_email += "<tr><td>" + send_index + "</td><td>" + row["en_name"].ToString().Trim() + "</td><td>" + row["gender"].ToString().Trim() + "</td><td>" + row["phone"].ToString().Trim() + "</td><td>" + row["customer_id"].ToString().Trim() + "</td><td>" + row["policy_number"].ToString().Trim() + "</td><td>" + row["product_id"].ToString().Trim() + "</td><td>" + Convert.ToDateTime(row["next_due"].ToString().Trim()).Date.ToString("dd/MM/yyyy") + "</td><td>" + row["email"].ToString().Trim() + "</td><td>SUCCESS</td></tr>";

                                            saveLog(1, index + ". [FAIL]\t PHONE:[" + str_phone + "]\t Error:[" + ex.Message + "]");
                                        }

                                        #endregion
                                    }


                                }
                                
                            }

                        }
                        else
                        {
                            UpdateNotificationStatus(rowID, send_by, send_date, "FAIL");
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateNotificationStatus(rowID, send_by, send_date, "FAIL");
                        saveLog(1, index + ". [FAIL]\t PHONE:[" + phoneNumber + "]\t Error:[" + ex.Message + "]");
                    }
                }

                str_summary_email += "</table>";
                if (send_index > 0)
                {
                    /*send summary email*/
                    EmailSender mail = new EmailSender();
                    mail.From = "";
                    mail.From = "policyholder@camlife.com.kh";
                    mail.To = "policyholderservice@camlife.com.kh";
                    //mail.To = "maneth.som@camlife.com.kh";
                    mail.BCC = "maneth.som@camlife.com.kh";
                    mail.Subject = "SMS Premium Due Notice";
                    mail.Message = str_summary_email;
                    mail.Host = "mail.camlife.com.kh";

                    mail.Port = 587;
                    mail.Password = "policyholder";

                    if (mail.SendMail(mail))
                    {
                        saveLog(2, "[INFO]\t Summary email was been send to " + mail.To + " successfully.");
                    }
                    else
                    {
                        saveLog(2, "[INFO]\t Summary email was been send to " + mail.To + " fail.");
                    }
                }

            }
            saveLog(1, "ENDED @" + DateTime.Now + ">>");
        }
        catch (Exception ex)
        {

            /*send fail*/
            saveLog(1, index + ". [FAIL]\t PHONE:[" + send_to_number + "]\t MESSAGE CATE.:[" + messageCate + "]\t MESSAGE TEXT:[" + message + "]");

            Log.AddExceptionToLog("Error function [SendCustomerNotificationSMS(DateTime send_date, string send_by)] in class [da_before_due_notification], detail:" + ex.Message, send_by);
        }
  
    }

    public static bool InsertCustomerSendingEmail(string en_name, string email, DateTime next_due, string created_by, DateTime created_on)
    {
        bool result = false;

        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Customer_Send_Email", new string[,]{
        {"@En_Name", en_name},
        {"@Email",email},
        {"@Next_Due",next_due+"" },
        {"@Created_By", created_by},
        {"@Created_On", created_on+""}
        }, "[da_email_notification] ==> [InsertCustomerSendingEmail(string en_name , string email , DateTime nex_due, string created_by , DateTime created_on, string customer_id)]");


        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [InsertCustomerSendingEmail(string en_name , string email , DateTime nex_due, string created_by , DateTime created_on, string customer_id) ] in class [da_email_notification], details: " + ex.Message, created_by);
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="log_file">[1=SaveLogSMS, 2=SaveLogEmail]</param>
    /// <param name="log"></param>
    private static void saveLog(int log_file, string log)
    {
        if (log_file == 1)
        {
            Log.CreateLog("SendCustomerNotificationSMS", log);
        }
        else if (log_file == 2)
        {
            Log.CreateLog("SendCustomerNotificationEmail", log);
        }
    }

    /// <summary>
    /// Check existing record
    /// </summary>
    /// <param name="policy_number"></param>
    /// <param name="created_on"></param>
    /// <param name="next_due"></param>
    /// <param name="next_due_day"></param>
    /// <returns></returns>
    public static bool IsExist(string policy_number, string phone_number, DateTime created_on, DateTime next_due, int next_due_day)
    {
        bool result = false;
        try
        {
            DataTable tbl = GetCustomerNotification(created_on, created_on);
            foreach (DataRow row in tbl.Select("next_due='" + next_due.Date + "' and next_due_day=" + next_due_day + " and policy_number='" + policy_number + "' and phone='" + phone_number + "'"))
            {
                if (row["id"].ToString().Trim() != "")
                {
                    result = true;
                    break;
                }
                else
                {
                    result = false;
                }
            }
        }
        catch (Exception ex)
        {
            result = true;//
            Log.AddExceptionToLog("Error function [IsExist(DateTime next_due, int next_due_day)] in class [da_before_due_notification], detail:" + ex.Message, "");
        }
        return result;
    }

}