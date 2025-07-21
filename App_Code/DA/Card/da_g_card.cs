using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_g_card
/// </summary>
public class da_g_card
{
    private static MySqlDB db = new MySqlDB();
    public da_g_card()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private static bool _SaveStatus;
    private static String _Exception;
    private static int _CountData;
    /// <summary>
    /// Return Save Status [true/false]
    /// </summary>
    public static bool TransactionStatus { get { return _SaveStatus; } }
    public static bool GetDataStatus { get { return _SaveStatus; } }
    /// <summary>
    /// Return number of data count after generate data, return -1 when generate data error.
    /// </summary>
    public static int CountData { get { return _CountData; } }
    /// <summary>
    /// Return Error Execption
    /// </summary>
    public static string Exception { get { return _Exception; } }

    public static void SaveCardTemp(bl_g_card card)
    {
        try
        {
            db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ProcedureName = "SP_TBL_G_CERT_TEMP_INSERT";
            db.Parameters = new string[,] { 
        {"V_CUSTOMER_NAME_KH", card.CustomerNameKh},
        {"V_CUSTOMER_NAME_EN", card.CustomerNameEn },
        {"D_CUSTOMER_DOB", card.DOB.Date.ToString("yyyy-MM-dd") } , 
        {"V_CUSTOMER_GENDER", card.Gender},
        {"V_CERTIFICATE_NUMBER", card.CertificateNumber},
        {"D_EFFECTIVE_DATE", card.EffectiveDate.ToString("yyyy-MM-dd") },
        {"D_EXPIRY_DATE", card.ExpiryDate.Date.ToString("yyyy-MM-dd") },
        {"D_MATURITY_DATE", card.MaturityDate.Date.ToString("yyyy-MM-dd") },
        {"I_STATUS", card.Status+""} ,
        {"V_CREATED_BY", card.CreatedBy},
        {"D_CREATED_ON",card.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")},
        {"V_OWNER", card.Owner}
        };
            db.Execute();

            if (db.ExecuteStatus == true)
            {
                _SaveStatus = true;
            }
            else
            {
                _SaveStatus = false;
                _Exception = db.Exception;
            }
        }
        catch (Exception ex)
        {
            _SaveStatus = false;
            _Exception = ex.Message;
            Log.AddExceptionToLog("Error function [SaveCardTemp(bl_g_card card)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
    }
    public static void SaveCard(bl_g_card card)
    {
        try
        {
            db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ProcedureName = "SP_TBL_G_CERT_INSERT";
            db.Parameters = new string[,] { 
        {"V_CUSTOMER_NAME_KH", card.CustomerNameKh},
        {"V_CUSTOMER_NAME_EN", card.CustomerNameEn },
        {"D_CUSTOMER_DOB", card.DOB.Date.ToString("yyyy-MM-dd") } , 
        {"V_CUSTOMER_GENDER", card.Gender},
        {"V_CERTIFICATE_NUMBER", card.CertificateNumber},
        {"D_EFFECTIVE_DATE", card.EffectiveDate.ToString("yyyy-MM-dd") },
        {"D_EXPIRY_DATE", card.ExpiryDate.Date.ToString("yyyy-MM-dd") },
        {"D_MATURITY_DATE", card.MaturityDate.Date.ToString("yyyy-MM-dd") },
        {"I_STATUS", card.Status+""} ,
        {"V_CREATED_BY", card.CreatedBy},
        {"D_CREATED_ON",card.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")},
        {"V_OWNER", card.Owner}
        };
            db.Execute();

            if (db.ExecuteStatus == true)
            {
                _SaveStatus = true;
            }
            else
            {
                _SaveStatus = false;
                _Exception = db.Exception;
            }
        }
        catch (Exception ex)
        {
            _SaveStatus = false;
            _Exception = ex.Message;
            Log.AddExceptionToLog("Error function [SaveCard(bl_g_card card)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Status"></param>
    /// <param name="UpdatedBy"></param>
    /// <param name="UpdatedOn"></param>
    /// <param name="UpdatedRemarks"></param>
    /// <param name="CertificateNumber">Is condition Field</param>
    /// <param name="Owner">Is condition Field</param>
    public static void UpdateCard(int Status, string UpdatedBy, DateTime UpdatedOn, string UpdatedRemarks, string CertificateNumber, string Owner)
    {
        try
        {
            db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ProcedureName = "SP_TBL_G_CERT_UPDATE_STATUS";
            db.Parameters = new string[,] { 
      
        {"V_CERTIFICATE_NUMBER", CertificateNumber},
        {"I_STATUS", Status+""} ,
        {"V_UPDATED_BY", UpdatedBy},
        {"D_UPDATED_ON",UpdatedOn.ToString("yyyy-MM-dd HH:mm:ss")},
        {"V_OWNER", Owner},
        {"V_UPDATED_REMARKS", UpdatedRemarks}
        };
            db.Execute();

            if (db.ExecuteStatus == true)
            {
                _SaveStatus = true;
            }
            else
            {
                _SaveStatus = false;
                _Exception = db.Exception;
            }
        }
        catch (Exception ex)
        {
            _SaveStatus = false;
            _Exception = ex.Message;
            Log.AddExceptionToLog("Error function [UpdateCard(int Status, string UpdatedBy, DateTime UpdatedOn, string UpdatedRemarks, string CertificateNumber, string Owner)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
    }
    public static void DeleteCardTemp(string CreatedBy, DateTime CreatedOn)
    {
        try
        {
            db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ProcedureName = "SP_TBL_G_CERT_TEMP_DELETE";
            db.Parameters = new string[,] { 
            {"V_CREATED_BY", CreatedBy}, {"D_CREATED_ON",CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")}
        };
            db.Execute();

            if (db.ExecuteStatus == true)
            {
                _SaveStatus = true;
            }
            else
            {
                _SaveStatus = false;
                _Exception = db.Exception;
            }
        }
        catch (Exception ex)
        {
            _SaveStatus = false;
            _Exception = ex.Message;
            Log.AddExceptionToLog("Error function [ DeleteCardTemp(string CreatedBy, DateTime CreatedOn)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
    }
    /// <summary>
    /// Get card by owner
    /// </summary>
    /// <param name="owner"> is the log in user name</param>
    /// <returns></returns>
    public static bl_g_card Getcard(string owner)
    {
        bl_g_card card = new bl_g_card();

        try
        {
            MySqlDB db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "VAR_OWNER", owner } };
            db.ProcedureName = "SP_TBL_G_CERT_GET";
            db.GenerateData();
            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {
                    var row = db.Data.Rows[0];
                    card = new bl_g_card()
                    {
                        CustomerNameKh = row["customer_name_kh"].ToString(),
                        CustomerNameEn = row["customer_name_en"].ToString(),
                        Gender = row["customer_gender"].ToString(),
                        DOB = Convert.ToDateTime(row["customer_dob"].ToString()),
                        CertificateNumber = row["certificate_number"].ToString(),
                        EffectiveDate = Convert.ToDateTime(row["effecive_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                        MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()),
                        Status = Convert.ToInt32(row["status"].ToString()),
                        Owner = row["owner"].ToString()
                    };
                }
                else // no error and data found
                {
                    card = new bl_g_card();
                }
                _CountData = db.DataCount;
                // _CountData = 0;
                _SaveStatus = true;
            }
            else
            {
                _CountData = -1;
                _Exception = db.Exception;
                _SaveStatus = false;
            }
        }
        catch (Exception ex)
        {
            card = new bl_g_card();
            _Exception = ex.Message;
            _SaveStatus = false;
            Log.AddExceptionToLog("Error function [Getcard(string owner)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
        return card;
    }
    /// <summary>
    /// Get data which is able to insert or update from temp table tbl_g_cert_temp
    /// </summary>
    /// <param name="CreatedBy"></param>
    /// <param name="CreatedOn"></param>
    /// <returns></returns>
    public static List<bl_g_card> GetCardListTempForInsertUpdate(string CreatedBy, DateTime CreatedOn)
    {
        // bl_g_card card = new bl_g_card();
        List<bl_g_card> CardList = new List<bl_g_card>();
        try
        {
            MySqlDB db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "V_CREATED_BY", CreatedBy }, { "D_CREATED_ON", CreatedOn.ToString("yyyy-MM-dd") } };
            db.ProcedureName = "SP_TBL_G_CERT_TEMP_GET_FOR_INSERT_UPDATE";
            db.GenerateData();
            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {
                    //var row = db.Data.Rows[0];
                    foreach (DataRow row in db.Data.Rows)
                    {
                        CardList.Add(new bl_g_card()
                        {
                            CustomerNameKh = row["customer_name_kh"].ToString(),
                            CustomerNameEn = row["customer_name_en"].ToString(),
                            Gender = row["customer_gender"].ToString(),
                            DOB = Convert.ToDateTime(row["customer_dob"].ToString()),
                            CertificateNumber = row["certificate_number"].ToString(),
                            EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                            ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                            MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()),
                            Status = Convert.ToInt32(row["status"].ToString()),
                            Owner = row["owner"].ToString(),
                            CreatedBy = row["created_by"].ToString(),
                            CreatedOn = Convert.ToDateTime(row["created_on"].ToString())
                        });

                    }
                }
                else // no error and data found
                {
                    CardList = new List<bl_g_card>();
                }
                _CountData = db.DataCount;
                _SaveStatus = true;
            }
            else
            {
                _CountData = -1;
                _Exception = db.Exception;
                _SaveStatus = false;
            }
        }
        catch (Exception ex)
        {

            CardList = new List<bl_g_card>();
            _Exception = ex.Message;
            _SaveStatus = false;
            _CountData = -1;
            Log.AddExceptionToLog("Error function [GetCardListTempForInsertUpdate(string CreatedBy, DateTime CreatedOn)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
        return CardList;
    }
    /// <summary>
    /// Get data which not allow to insert or update 
    /// </summary>
    /// <param name="CreatedBy">Is the login user name who are uploading data</param>
    /// <param name="CreatedOn">Is the date of uploading data</param>
    /// <returns></returns>
    public static List<bl_g_card> GetCardListTempNotAllowInsertUpdate(string CreatedBy, DateTime CreatedOn)
    {
        // bl_g_card card = new bl_g_card();
        List<bl_g_card> CardList = new List<bl_g_card>();
        try
        {
            MySqlDB db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "V_CREATED_BY", CreatedBy }, { "D_CREATED_ON", CreatedOn.ToString("yyyy-MM-dd") } };
            db.ProcedureName = "SP_TBL_G_CERT_TEMP_GET_NOT_ALLOW_INSERT_UPDATE";
            db.GenerateData();
            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {
                    //  var row = db.Data.Rows[0];
                    foreach (DataRow row in db.Data.Rows)
                    {
                        CardList.Add(new bl_g_card()
                        {
                            CustomerNameKh = row["customer_name_kh"].ToString(),
                            CustomerNameEn = row["customer_name_en"].ToString(),
                            Gender = row["customer_gender"].ToString(),
                            DOB = Convert.ToDateTime(row["customer_dob"].ToString()),
                            CertificateNumber = row["certificate_number"].ToString(),
                            EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                            ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                            MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()),
                            Status = Convert.ToInt32(row["status"].ToString()),
                            Owner = row["owner"].ToString(),
                            CreatedBy = row["created_by"].ToString(),
                            CreatedOn = Convert.ToDateTime(row["created_on"].ToString()),
                            Remarks = row["remarks"].ToString()
                        });
                    }

                }
                else // no error and data found
                {
                    CardList = new List<bl_g_card>();
                }
                _CountData = db.DataCount;
                _SaveStatus = true;
            }
            else
            {
                _CountData = -1;
                _Exception = db.Exception;
                _SaveStatus = false;
            }
        }
        catch (Exception ex)
        {
            CardList = new List<bl_g_card>();
            _Exception = ex.Message;
            _SaveStatus = false;
            _CountData = -1;
            Log.AddExceptionToLog("Error function [GetCardListTempNotAllowInsertUpdate(string CreatedBy, DateTime CreatedOn)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
        return CardList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Status">0 is not approved, 1 is approved</param>
    /// <param name="Username"></param>
    public static void UpdateMembershipApproval(int Status, string Username)
    {
        try
        {
            db = new MySqlDB();
            db.ConnectionString = AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.ProcedureName = "SP_my_aspnet_membership_UPDATE_APPROVED";
            db.Parameters = new string[,] { 
      
        {"I_APPROVED", Status+""},
       
        {"V_USER", Username}
        };
            db.Execute();

            if (db.ExecuteStatus == true)
            {
                _SaveStatus = true;
            }
            else
            {
                _SaveStatus = false;
                _Exception = db.Exception;
            }
        }
        catch (Exception ex)
        {
            _SaveStatus = false;
            _Exception = ex.Message;
            Log.AddExceptionToLog("Error function [UpdateMembershipApproval(int Status, string Username)] in class [da_g_card], detail: " + ex.StackTrace + " => " + ex.Message);
        }
    }

}