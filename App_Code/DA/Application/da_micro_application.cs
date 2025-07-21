
using System;
using System.Collections.Generic;
using System.Data;

public class da_micro_application
{
  private static bool _SUCCESS = false;
  private static string _MESSAGE = "";
  private static DB db = new DB();

  public static bool SUCCESS {get{return da_micro_application._SUCCESS;}}

  public static string MESSAGE {get{return da_micro_application._MESSAGE;}}

  public static bool SaveApplication(bl_micro_application APPLICATION)
  {
    bool flag = false;
    try
    {
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_INSERT", new string[,]
      {
        {
          "@APPLICATION_ID",
          APPLICATION.APPLICATION_ID
        },
        {
          "@SEQ",
          string.Concat((object) APPLICATION.SEQ)
        },
        {
          "@APPLICATION_NUMBER",
          APPLICATION.APPLICATION_NUMBER
        },
        {
          "@APPLICATION_DATE",
          string.Concat((object) APPLICATION.APPLICATION_DATE)
        },
        {
          "@CHANNEL_ID",
          APPLICATION.CHANNEL_ID
        },
        {
          "@CHANNEL_ITEM_ID",
          APPLICATION.CHANNEL_ITEM_ID
        },
        {
          "@CHANNEL_LOCATION_ID",
          APPLICATION.CHANNEL_LOCATION_ID
        },
        {
          "@SALE_AGENT_ID",
          APPLICATION.SALE_AGENT_ID
        },
        {
          "@APPLICATION_CUSTOMER_ID",
          APPLICATION.APPLICATION_CUSTOMER_ID
        },
        {
          "@CREATED_BY",
          APPLICATION.CREATED_BY
        },
        {
          "@CREATED_ON",
          string.Concat((object) APPLICATION.CREATED_ON)
        },
        {
          "@REMARKS",
          APPLICATION.REMARKS
        },
        {
          "@REFERRER_ID",
          APPLICATION.REFERRER_ID
        },
        {
          "@RENEW_FROM_POLICY",
          APPLICATION.RENEW_FROM_POLICY
        },
        {
          "@CLIENT_TYPE",
          APPLICATION.CLIENT_TYPE
        },
        {
          "@CLIENT_TYPE_REMARKS",
          APPLICATION.CLIENT_TYPE_REMARKS
        },
        {
          "@CLIENT_TYPE_RELATION",
          APPLICATION.CLIENT_TYPE_RELATION
        },
        {
          "@MAIN_APPLICATION",
          APPLICATION.MainApplicationNumber
        },
        {
          "@NUMBERS_OF_PURCHASING_YEAR",
          string.Concat((object) APPLICATION.NumbersOfPurchasingYear)
        },
        {
          "@NUMBERS_OF_APPLICATION_FIRST_YEAR",
          string.Concat((object) APPLICATION.NumbersOfApplicationFirstYear)
        },
        {
          "@LOAN_NUMBER",
          APPLICATION.LoanNumber
        },
        {
          "@POLICYHOLDER_NAME",
          APPLICATION.PolicyholderName == null ? "" : APPLICATION.PolicyholderName
        },
        {
          "@POLICYHOLDER_GENDER",
          string.Concat((object) APPLICATION.PolicyholderGender)
        },
        {
          "@POLICYHOLDER_DOB",
          string.Concat((object) APPLICATION.PolicyholderDOB)
        },
        {
          "@POLICYHOLDER_ID_TYPE",
          string.Concat((object) APPLICATION.PolicyholderIDType)
        },
        {
          "@POLICYHOLDER_ID_NO",
          APPLICATION.PolicyholderIDNo == null ? "" : APPLICATION.PolicyholderIDNo
        },
        {
          "@POLICYHOLDER_PHONE_NUMBER",
          APPLICATION.PolicyholderPhoneNumber == null ? "" : APPLICATION.PolicyholderPhoneNumber
        },
        {
          "@POLICYHOLDER_PHONE_NUMBER2",
          APPLICATION.PolicyholderPhoneNumber2 == null ? "" : APPLICATION.PolicyholderPhoneNumber2
        },
        {
          "@POLICYHOLDER_EMAIL",
          APPLICATION.PolicyholderEmail == null ? "" : APPLICATION.PolicyholderEmail
        },
        {
          "@POLICYHOLDER_ADDRESS",
          APPLICATION.PolicyholderAddress == null ? "" : APPLICATION.PolicyholderAddress
        }
      }, "da_micro_application => SaveApplication(bl_micro_application APPLICATION)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._MESSAGE = da_micro_application.db.Message;
        da_micro_application._SUCCESS = false;
      }
      else
      {
        da_micro_application._MESSAGE = "Sucess";
        da_micro_application._SUCCESS = true;
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [SaveApplication(bl_micro_application APPLICATION)] in class [da_micro_application], detail: "+ex.Message+" =>"+ ex.StackTrace);
    }
    return flag;
  }

  public static bool UpdateApplication(bl_micro_application APPLICATION)
  {
    bool flag = false;
    try
    {
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_UPDATE", new string[,]
      {
        {
          "@APPLICATION_ID",
          APPLICATION.APPLICATION_ID
        },
        {
          "@SEQ",
          string.Concat((object) APPLICATION.SEQ)
        },
        {
          "@APPLICATION_NUMBER",
          APPLICATION.APPLICATION_NUMBER
        },
        {
          "@APPLICATION_DATE",
          string.Concat((object) APPLICATION.APPLICATION_DATE)
        },
        {
          "@CHANNEL_ID",
          APPLICATION.CHANNEL_ID
        },
        {
          "@CHANNEL_ITEM_ID",
          APPLICATION.CHANNEL_ITEM_ID
        },
        {
          "@CHANNEL_LOCATION_ID",
          APPLICATION.CHANNEL_LOCATION_ID
        },
        {
          "@SALE_AGENT_ID",
          APPLICATION.SALE_AGENT_ID
        },
        {
          "@APPLICATION_CUSTOMER_ID",
          APPLICATION.APPLICATION_CUSTOMER_ID
        },
        {
          "@UPDATED_BY",
          APPLICATION.UPDATED_BY
        },
        {
          "@UPDATED_ON",
          string.Concat((object) APPLICATION.UPDATED_ON)
        },
        {
          "@REMARKS",
          APPLICATION.REMARKS
        },
        {
          "@REFERRER_ID",
          APPLICATION.REFERRER_ID
        },
        {
          "@RENEW_FROM_POLICY",
          APPLICATION.RENEW_FROM_POLICY
        },
        {
          "@CLIENT_TYPE",
          APPLICATION.CLIENT_TYPE
        },
        {
          "@CLIENT_TYPE_REMARKS",
          APPLICATION.CLIENT_TYPE_REMARKS
        },
        {
          "@CLIENT_TYPE_RELATION",
          APPLICATION.CLIENT_TYPE_RELATION
        },
        {
          "@LOAN_NUMBER",
          APPLICATION.LoanNumber
        },
        {
          "@POLICYHOLDER_NAME",
          APPLICATION.PolicyholderName == null ? "" : APPLICATION.PolicyholderName
        },
        {
          "@POLICYHOLDER_GENDER",
          string.Concat((object) APPLICATION.PolicyholderGender)
        },
        {
          "@POLICYHOLDER_DOB",
          string.Concat((object) APPLICATION.PolicyholderDOB)
        },
        {
          "@POLICYHOLDER_ID_TYPE",
          string.Concat((object) APPLICATION.PolicyholderIDType)
        },
        {
          "@POLICYHOLDER_ID_NO",
          APPLICATION.PolicyholderIDNo == null ? "" : APPLICATION.PolicyholderIDNo
        },
        {
          "@POLICYHOLDER_PHONE_NUMBER",
          APPLICATION.PolicyholderPhoneNumber == null ? "" : APPLICATION.PolicyholderPhoneNumber
        },
        {
          "@POLICYHOLDER_PHONE_NUMBER2",
          APPLICATION.PolicyholderPhoneNumber2 == null ? "" : APPLICATION.PolicyholderPhoneNumber2
        },
        {
          "@POLICYHOLDER_EMAIL",
          APPLICATION.PolicyholderEmail == null ? "" : APPLICATION.PolicyholderEmail
        },
        {
          "@POLICYHOLDER_ADDRESS",
          APPLICATION.PolicyholderAddress == null ? "" : APPLICATION.PolicyholderAddress
        }
      }, "da_micro_application => UpdateApplication(bl_micro_application APPLICATION)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._MESSAGE = da_micro_application.db.Message;
        da_micro_application._SUCCESS = false;
      }
      else
      {
        da_micro_application._MESSAGE = "Sucess";
        da_micro_application._SUCCESS = true;
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [UpdateApplication(bl_micro_application APPLICATION)] in class [da_micro_application], detail: "+ex.Message+" => "+ex.StackTrace);
    }
    return flag;
  }

  public static bool DeleteApplication(string APPLICATION_NUMBER)
  {
    bool flag;
    try
    {
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_DELETE", new string[,]
      {
        {
          "@APPLICATION_NUMBER",
          APPLICATION_NUMBER
        }
      }, "da_micro_application=>DeleteApplication(string APPLICATION_NUMBER)");
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      flag = false;
      Log.AddExceptionToLog("Error function [DeleteApplication(string APPLICATION_NUMBER)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return flag;
  }

  public static bl_micro_application GetApplication(string APPLICATION_NUMBER)
  {
    bl_micro_application application = new bl_micro_application();
    try
    {
      DataTable data = da_micro_application.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_GET_BY_APPLICATION_NUMBER", new string[1, 2]
      {
        {
          "@APPLICATION_NUMBER",
          APPLICATION_NUMBER
        }
      }, "da_micro_application=>GetApplication(string APPLICATION_NUMBER)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._SUCCESS = false;
        da_micro_application._MESSAGE = da_micro_application.db.Message;
      }
      else
      {
        if (data.Rows.Count > 0)
        {
          DataRow row = data.Rows[0];
          application = new bl_micro_application()
          {
            APPLICATION_ID = row["application_id"].ToString(),
            APPLICATION_NUMBER = row["application_number"].ToString(),
            APPLICATION_DATE = Convert.ToDateTime(row["application_date"].ToString()),
            SEQ = Convert.ToInt32(row["seq"].ToString()),
            APPLICATION_CUSTOMER_ID = row["application_customer_id"].ToString(),
            CHANNEL_ID = row["channel_id"].ToString(),
            CHANNEL_ITEM_ID = row["channel_item_id"].ToString(),
            CHANNEL_LOCATION_ID = row["channel_location_id"].ToString(),
            SALE_AGENT_ID = row["sale_agent_id"].ToString(),
            CREATED_BY = row["created_by"].ToString(),
            CREATED_ON = Convert.ToDateTime(row["created_on"].ToString()),
            UPDATED_BY = row["updated_by"].ToString(),
            UPDATED_ON = Convert.ToDateTime(row["updated_on"].ToString()),
            REMARKS = row["remarks"].ToString(),
            REFERRER_ID = row["referrer_id"].ToString(),
            REFERRER = row["referrer"].ToString(),
            RENEW_FROM_POLICY = row["renew_from_policy"].ToString(),
            CLIENT_TYPE = row["CLIENT_TYPE"].ToString(),
            CLIENT_TYPE_RELATION = row["CLIENT_TYPE_RELATION"].ToString(),
            CLIENT_TYPE_REMARKS = row["CLIENT_TYPE_REMARKS"].ToString(),
            LoanNumber = row["LOAN_NUMBER"].ToString(),
            PolicyholderName = row["policyholder_name"].ToString(),
            PolicyholderGender = Convert.ToInt32(row["policyholder_gender"].ToString()),
            PolicyholderDOB = Convert.ToDateTime(row["policyholder_dob"].ToString()),
            PolicyholderIDType = Convert.ToInt32(row["policyholder_id_type"].ToString()),
            PolicyholderIDNo = row["policyholder_IdNo"].ToString(),
            PolicyholderPhoneNumber = row["policyholder_phone_number"].ToString(),
            PolicyholderPhoneNumber2 = row["policyholder_phone_number2"].ToString(),
            PolicyholderEmail = row["policyholder_email"].ToString(),
            PolicyholderAddress = row["policyholder_address"].ToString()
          };
        }
        da_micro_application._SUCCESS = true;
        da_micro_application._MESSAGE = "Success";
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      application = new bl_micro_application();
      Log.AddExceptionToLog("Error function [GetApplication(string APPLICATION_NUMBER)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return application;
  }

  public static bl_micro_application GetApplicationByApplicationID(string APPLICATION_ID)
  {
    bl_micro_application applicationByApplicationId = new bl_micro_application();
    try
    {
      DataTable data = da_micro_application.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_GET_BY_APPLICATION_ID", new string[,]
      {
        {
          "@APPLICATION_ID",
          APPLICATION_ID
        }
      }, "da_micro_application=>GetApplicationByApplicationID(string APPLICATION_ID)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._SUCCESS = false;
        da_micro_application._MESSAGE = da_micro_application.db.Message;
      }
      else
      {
        if (data.Rows.Count > 0)
        {
          DataRow row = data.Rows[0];
          applicationByApplicationId = new bl_micro_application()
          {
            APPLICATION_ID = row["application_id"].ToString(),
            APPLICATION_NUMBER = row["application_number"].ToString(),
            APPLICATION_DATE = Convert.ToDateTime(row["application_date"].ToString()),
            SEQ = Convert.ToInt32(row["seq"].ToString()),
            APPLICATION_CUSTOMER_ID = row["application_customer_id"].ToString(),
            CHANNEL_ID = row["channel_id"].ToString(),
            CHANNEL_ITEM_ID = row["channel_item_id"].ToString(),
            CHANNEL_LOCATION_ID = row["channel_location_id"].ToString(),
            SALE_AGENT_ID = row["sale_agent_id"].ToString(),
            CREATED_BY = row["created_by"].ToString(),
            CREATED_ON = Convert.ToDateTime(row["created_on"].ToString()),
            UPDATED_BY = row["updated_by"].ToString(),
            UPDATED_ON = Convert.ToDateTime(row["updated_on"].ToString()),
            REMARKS = row["remarks"].ToString(),
            REFERRER_ID = row["referrer_id"].ToString(),
            REFERRER = row["referrer"].ToString(),
            RENEW_FROM_POLICY = row["renew_from_policy"].ToString(),
            CLIENT_TYPE = row["CLIENT_TYPE"].ToString(),
            CLIENT_TYPE_RELATION = row["CLIENT_TYPE_RELATION"].ToString(),
            CLIENT_TYPE_REMARKS = row["CLIENT_TYPE_REMARKS"].ToString(),
            LoanNumber = row["LOAN_NUMBER"].ToString(),
            PolicyholderName = row["policyholder_name"].ToString(),
            PolicyholderGender = Convert.ToInt32(row["policyholder_gender"].ToString()),
            PolicyholderDOB = Convert.ToDateTime(row["policyholder_dob"].ToString()),
            PolicyholderIDType = Convert.ToInt32(row["policyholder_id_type"].ToString()),
            PolicyholderIDNo = row["policyholder_Id_No"].ToString(),
            PolicyholderPhoneNumber = row["policyholder_phone_number"].ToString(),
            PolicyholderPhoneNumber2 = row["policyholder_phone_number2"].ToString(),
            PolicyholderEmail = row["policyholder_email"].ToString(),
            PolicyholderAddress = row["policyholder_address"].ToString()
          };
        }
        da_micro_application._SUCCESS = true;
        da_micro_application._MESSAGE = "Success";
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      applicationByApplicationId = new bl_micro_application();
      Log.AddExceptionToLog("Error function [GetApplicationByApplicationID(string APPLICATION_ID)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return applicationByApplicationId;
  }

  public static DataTable GetApplicationDetailByApplicationID(string APPLICATION_ID)
  {
    DataTable dataTable = new DataTable();
    DataTable detailByApplicationId;
    try
    {
      detailByApplicationId = da_micro_application.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_GET_APPLICATION_DETAIL", new string[1, 2]
      {
        {
          "@APPLICATION_ID",
          APPLICATION_ID
        }
      }, "da_micro_application=>GetApplicationDetailByApplicationID(string APPLICATION_ID)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._SUCCESS = false;
        da_micro_application._MESSAGE = da_micro_application.db.Message;
      }
      else
      {
        da_micro_application._SUCCESS = true;
        da_micro_application._MESSAGE = "Success";
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      detailByApplicationId = new DataTable();
      Log.AddExceptionToLog("Error function [GetApplicationDetailByApplicationID(string APPLICATION_ID)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return detailByApplicationId;
  }

  public static DataSet GetApplicationFormByApplicationID( string applicationId, da_micro_application.ApplicationTypeOption applicationType)
  {
    DataSet dataSet = new DataSet();
    DataSet formByApplicationId;
    try
    {
      formByApplicationId = da_micro_application.db.GetDataSet(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_FORM", new string[,]
      {
        {
          "@APPLICATION_ID",
          applicationId
        },
        {
          "application_type",
          applicationType.ToString()
        }
      });
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._SUCCESS = false;
        da_micro_application._MESSAGE = da_micro_application.db.Message;
      }
      else
      {
        da_micro_application._SUCCESS = true;
        da_micro_application._MESSAGE = "Success";
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      formByApplicationId = new DataSet();
      Log.AddExceptionToLog("Error function [GetApplicationFormByApplicationID(string APPLICATION_ID)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return formByApplicationId;
  }

  public static DataSet GetAppFormDataSet(    string appId,    da_micro_application.ApplicationTypeOption appType)
  {
    DataSet appFormDataSet = new DataSet();
    DataSet formByApplicationId = da_micro_application.GetApplicationFormByApplicationID(appId, appType);
    DataTable dataTable1 = new DataTable();
    DataTable dataTable2 = new DataTable();
    DataTable table1 = formByApplicationId.Tables[0];
    DataTable table2 = formByApplicationId.Tables[1];
    if (table1.Rows.Count > 0 && appType.ToString() == da_micro_application.ApplicationTypeOption.IND.ToString())
    {
      table1.Columns.Add("Address");
      DataRow row = table1.Rows[0];
      if (row["province_en"].ToString().Trim().ToUpper() == "PHNOM PENH")
        row["Address"] = (row["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + row["house_no_kh"].ToString().Trim())+" "+ (row["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + row["street_no_kh"].ToString().Trim())+" "+(row["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + row["village_kh"].ToString().Trim())+" "+(row["commune_kh"].ToString().Trim() == "" ? "" : "សង្កាត់" + row["commune_kh"].ToString().Trim())+ " " +(row["district_kh"].ToString().Trim() == "" ? "" : "ខណ្ឌ" + row["district_kh"].ToString().Trim());
      else
        row["Address"] = (row["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + row["house_no_kh"].ToString().Trim())+ " "+(row["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + row["street_no_kh"].ToString().Trim())+" "+(row["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + row["village_kh"].ToString().Trim())+" "+(row["commune_kh"].ToString().Trim() == "" ? "" : "ឃុំ" + row["commune_kh"].ToString().Trim())+" "+(row["district_kh"].ToString().Trim() == "" ? "" : "ស្រុក" + row["district_kh"].ToString().Trim());
    }
    DataTable dataTable3 = new DataTable("tbl_micro_application_form");
    DataTable table3 = table1.Copy();
    DataTable dataTable4 = new DataTable("tbl_micro_application_form_ben");
    DataTable table4 = table2.Copy();
    appFormDataSet.Tables.Add(table3);
    appFormDataSet.Tables.Add(table4);
    return appFormDataSet;
  }

  public static List<bl_micro_application_search> SearchApplication(
    string applicationNumber,
    string customerName,
    DateTime customerDOB,
    int customerGender,
    string agentCode,
    string agentName)
  {
    List<bl_micro_application_search> applicationSearchList = new List<bl_micro_application_search>();
    try
    {
      int num = 0;
      foreach (DataRow row in (InternalDataCollectionBase) da_micro_application.db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_SEARCH", new string[,]
      {
        {
          "@application_number",
          applicationNumber
        },
        {
          "@customer_name",
          customerName
        },
        {
          "@customer_gender",
          string.Concat((object) customerGender)
        },
        {
          "@customer_dob",
          string.Concat((object) customerDOB)
        },
        {
          "@agent_code",
          agentCode
        },
        {
          "@agent_name",
          agentName
        }
      }, "da_micro_application=>SearchApplication(string applicationNumber, string customerName, DateTime customerDOB, int customerGender, string agentCode, string agentName)").Rows)
      {
        ++num;
        applicationSearchList.Add(new bl_micro_application_search()
        {
          No = num,
          ApplicationID = row["application_id"].ToString(),
          ApplicationNumber = row["application_number"].ToString(),
          CustomerFirstName = row["first_name_in_english"].ToString(),
          CustomerLastName = row["last_name_in_english"].ToString(),
          CustomberGender = row["gender"].ToString(),
          CustomerDOB = Convert.ToDateTime(row["date_of_birth"].ToString()),
          AgentCode = row["sale_agent_id"].ToString(),
          AgentName = row["full_name"].ToString()
        });
      }
    }
    catch (Exception ex)
    {
      applicationSearchList = new List<bl_micro_application_search>();
      Log.AddExceptionToLog("Error function [SearchApplication(string applicationNumber, string customerName, DateTime customerDOB, int customerGender, string agentCode, string agentName)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return applicationSearchList;
  }

  public static bool BackupApplication(
    string applicationID,
    string tranType,
    string tranBy,
    DateTime tranDate)
  {
    bool flag = false;
    try
    {
      da_micro_application.db = new DB();
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_BACKUP", new string[,]
      {
        {
          "@APPLICATION_ID",
          applicationID
        },
        {
          "@TRAN_TYPE",
          tranType
        },
        {
          "@TRAN_DATE",
          string.Concat((object) tranDate)
        },
        {
          "@TRAN_BY",
          tranBy
        }
      }, "da_micro_application => BackupApplication(string applicationID, string tranType, string tranBy, DateTime tranDate)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._MESSAGE = da_micro_application.db.Message;
        da_micro_application._SUCCESS = false;
      }
      else
      {
        da_micro_application._MESSAGE = "Sucess";
        da_micro_application._SUCCESS = true;
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [BackupApplication(string applicationID, string tranType, string tranBy, DateTime tranDate)] in class [da_micro_application], detail: "+ex.Message+" => " +ex.StackTrace);
    }
    return flag;
  }

  public static bool RestoreApplication(string applicationNumber, string tranBy, DateTime tranDate)
  {
    bool flag = false;
    try
    {
      da_micro_application.db = new DB();
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_RESTORE", new string[,]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        },
        {
          "@TRAN_DATE",
          string.Concat((object) tranDate)
        },
        {
          "@TRAN_BY",
          tranBy
        }
      }, "da_micro_application => RestoreApplication(string applicationNumber, string tranBy, DateTime tranDate)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._MESSAGE = da_micro_application.db.Message;
        da_micro_application._SUCCESS = false;
      }
      else
      {
        da_micro_application._MESSAGE = "Sucess";
        da_micro_application._SUCCESS = true;
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [RestoreApplication(string applicationNumber, string tranBy, DateTime tranDate)] in class [da_micro_application], detail: "+ex.Message+" => "+ex.StackTrace);
    }
    return flag;
  }

  public static bool DeleteBackupApplication(
    string applicationNumber,
    string tranBy,
    DateTime tranDate)
  {
    bool flag = false;
    try
    {
      da_micro_application.db = new DB();
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_BACKUP_DELETE", new string[3, 2]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        },
        {
          "@TRAN_DATE",
          string.Concat((object) tranDate)
        },
        {
          "@TRAN_BY",
          tranBy
        }
      }, "da_micro_application => DeleteBackupApplication(string applicationNumber, string tranBy, DateTime tranDate)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._MESSAGE = da_micro_application.db.Message;
        da_micro_application._SUCCESS = false;
      }
      else
      {
        da_micro_application._MESSAGE = "Sucess";
        da_micro_application._SUCCESS = true;
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [DeleteBackupApplication(string applicationNumber, string tranBy, DateTime tranDate)] in class [da_micro_application], detail: "+ex.Message+ "=> "+ex.StackTrace);
    }
    return flag;
  }

  public static List<bl_micro_application> GetApplicationBatchByApplicationID(string APPLICATION_ID)
  {
    List<bl_micro_application> batchByApplicationId = new List<bl_micro_application>();
    try
    {
      DataTable data = da_micro_application.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_BATCH_GET_BY_APPLICATION_ID", new string[,]
      {
        {
          "@APPLICATION_ID",
          APPLICATION_ID
        }
      }, "da_micro_application=>GetApplicationBatchByApplicationID(string APPLICATION_ID)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._SUCCESS = false;
        da_micro_application._MESSAGE = da_micro_application.db.Message;
      }
      else
      {
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
          batchByApplicationId.Add(new bl_micro_application()
          {
            APPLICATION_ID = row["application_id"].ToString(),
            APPLICATION_NUMBER = row["application_number"].ToString(),
            APPLICATION_DATE = Convert.ToDateTime(row["application_date"].ToString()),
            SEQ = Convert.ToInt32(row["seq"].ToString()),
            APPLICATION_CUSTOMER_ID = row["application_customer_id"].ToString(),
            CHANNEL_ITEM_ID = row["channel_item_id"].ToString(),
            CHANNEL_LOCATION_ID = row["channel_location_id"].ToString(),
            SALE_AGENT_ID = row["sale_agent_id"].ToString(),
            CREATED_BY = row["created_by"].ToString(),
            CREATED_ON = Convert.ToDateTime(row["created_on"].ToString()),
            UPDATED_BY = row["updated_by"].ToString(),
            UPDATED_ON = Convert.ToDateTime(row["updated_on"].ToString()),
            REMARKS = row["remarks"].ToString(),
            CLIENT_TYPE = row["CLIENT_TYPE"].ToString(),
            CLIENT_TYPE_RELATION = row["CLIENT_TYPE_RELATION"].ToString(),
            CLIENT_TYPE_REMARKS = row["CLIENT_TYPE_REMARKS"].ToString(),
            NumbersOfApplicationFirstYear = Convert.ToInt32(row["NUMBERS_OF_APPLICATION_FIRST_YEAR"].ToString()),
            NumbersOfPurchasingYear = Convert.ToInt32(row["NUMBERS_OF_PURCHASING_YEAR"].ToString())
          });
        da_micro_application._SUCCESS = true;
        da_micro_application._MESSAGE = "Success";
      }
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      batchByApplicationId = new List<bl_micro_application>();
      Log.AddExceptionToLog("Error function [GetApplicationBatchByApplicationID(string APPLICATION_ID)] in class [da_micro_application], detail: " +ex.Message+"==>"+ex.StackTrace);
    }
    return batchByApplicationId;
  }

  public static bool UpdateApplicationStatus(
    string applicationNumber,
    string status,
    string updatedBy,
    DateTime updatedOn,
    string updatedRemarks)
  {
    bool flag;
    try
    {
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_UPDATE_STATUS", new string[,]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        },
        {
          "@STATUS",
          status
        },
        {
          "@UPDATED_BY",
          updatedBy
        },
        {
          "@UPDATED_ON",
          string.Concat((object) updatedOn)
        },
        {
          "@UPDATED_REMARKS",
          updatedRemarks
        }
      }, "da_micro_application=>UpdateApplicationStatus(string applicationNumber,string status, string updatedBy, DateTime updatedOn, string updatedRemarks)");
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      flag = false;
      Log.AddExceptionToLog("Error function [UpdateApplicationStatus(string applicationNumber,string status, string updatedBy, DateTime updatedOn, string updatedRemarks)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return flag;
  }

  public static bool UpdateApplicationTotalNumbers(
    string applicationNumber,
    int numbersOfApplicationFirstYear,
    int numbersOfPurchasingYear,
    string updatedBy,
    DateTime updatedOn,
    string updatedRemarks)
  {
    bool flag;
    try
    {
      flag = da_micro_application.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_UPDATE_NUMBERS_OF_APP", new string[,]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        },
        {
          "@NUMBERS_OF_APPLICATION_FIRST_YEAR",
          string.Concat((object) numbersOfApplicationFirstYear)
        },
        {
          "@NUMBERS_OF_PURCHASING_YEAR",
          string.Concat((object) numbersOfPurchasingYear)
        },
        {
          "@UPDATED_BY",
          updatedBy
        },
        {
          "@UPDATED_ON",
          string.Concat((object) updatedOn)
        },
        {
          "@UPDATED_REMARKS",
          updatedRemarks
        }
      }, "da_micro_application=>UpdateApplicationTotalNumbers(string applicationNumber, int numbersOfApplicationFirstYear, int numbersOfPurchasingYear, string updatedBy, DateTime updatedOn, string updatedRemarks)");
    }
    catch (Exception ex)
    {
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      flag = false;
      Log.AddExceptionToLog("Error function [UpdateApplicationTotalNumbers(string applicationNumber, int numbersOfApplicationFirstYear, int numbersOfPurchasingYear, string updatedBy, DateTime updatedOn, string updatedRemarks)] in class [da_micro_application], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return flag;
  }

  public static bl_micro_application_details GetApplicationDetail(string applicationNumber)
  {
    bl_micro_application_details applicationDetail = new bl_micro_application_details();
    try
    {
      DataSet dataSet = new DB().GetDataSet(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_DETAIL_BY_APPLICATION_NUMBER", new string[1, 2]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        }
      });
      bl_micro_application_customer customer = applicationDetail.Customer;
      bl_micro_application application = applicationDetail.Application;
      bl_micro_application_insurance insurance = applicationDetail.Insurance;
      bl_micro_application_insurance_rider rider = applicationDetail.Rider;
      List<bl_micro_application_beneficiary> applicationBeneficiaryList = new List<bl_micro_application_beneficiary>();
      bl_micro_application_questionaire questionaire = applicationDetail.Questionaire;
      if (dataSet.Tables.Count > 0)
      {
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[2];
        DataTable table4 = dataSet.Tables[3];
        DataRow row1 = table1.Rows[0];
        bl_micro_application_customer applicationCustomer = new bl_micro_application_customer()
        {
          CUSTOMER_ID = row1["APPLICATION_CUSTOMER_ID"].ToString(),
          ID_TYPE = row1["id_type"].ToString(),
          ID_NUMBER = row1["id_number"].ToString(),
          FIRST_NAME_IN_ENGLISH = row1["first_name_in_english"].ToString(),
          LAST_NAME_IN_ENGLISH = row1["last_name_in_english"].ToString(),
          FIRST_NAME_IN_KHMER = row1["first_name_in_khmer"].ToString(),
          LAST_NAME_IN_KHMER = row1["last_name_in_khmer"].ToString(),
          GENDER = row1["gender"].ToString(),
          DATE_OF_BIRTH = Convert.ToDateTime(row1["date_of_birth"].ToString()),
          NATIONALITY = row1["nationality"].ToString(),
          MARITAL_STATUS = row1["marital_status"].ToString(),
          OCCUPATION = row1["occupation"].ToString(),
          PHONE_NUMBER1 = row1["phone_number"].ToString(),
          EMAIL1 = row1["email"].ToString(),
          HOUSE_NO_EN = row1["house_no_en"].ToString(),
          STREET_NO_EN = row1["street_no_en"].ToString(),
          VILLAGE_EN = row1["village_en"].ToString(),
          VILLAGE_KH = row1["village_kh"].ToString(),
          COMMUNE_EN = row1["commune_en"].ToString(),
          COMMUNE_KH = row1["commune_kh"].ToString(),
          DISTRICT_EN = row1["district_en"].ToString(),
          DISTRICT_KH = row1["district_kh"].ToString(),
          PROVINCE_EN = row1["province_en"].ToString(),
          PROVINCE_KH = row1["province_kh"].ToString(),
          VILLAGE_CODE = row1["village_code"].ToString(),
          COMMUNE_CODE = row1["commune_code"].ToString(),
          DISTRICT_CODE = row1["district_code"].ToString(),
          PROVINCE_CODE = row1["province_code"].ToString()
        };
        bl_micro_application microApplication = new bl_micro_application()
        {
          APPLICATION_ID = row1["application_id"].ToString(),
          APPLICATION_NUMBER = row1["application_number"].ToString(),
          APPLICATION_DATE = Convert.ToDateTime(row1["application_date"].ToString()),
          REFERRER = row1["referrer"].ToString(),
          REFERRER_ID = row1["referrer_id"].ToString(),
          CLIENT_TYPE = row1["client_type"].ToString(),
          CLIENT_TYPE_REMARKS = row1["client_type_remarks"].ToString(),
          CLIENT_TYPE_RELATION = row1["client_type_relation"].ToString(),
          RENEW_FROM_POLICY = row1["renew_from_policy"].ToString(),
          CREATED_BY = row1["created_by"].ToString(),
          CREATED_ON = Convert.ToDateTime(row1["created_on"].ToString()),
          MainApplicationNumber = row1["Main_application_number"].ToString(),
          NumbersOfPurchasingYear = Convert.ToInt32(row1["NUMBERS_OF_PURCHASING_YEAR"].ToString()),
          NumbersOfApplicationFirstYear = Convert.ToInt32(row1["NUMBERS_OF_APPLICATION_FIRST_YEAR"].ToString()),
          LoanNumber = row1["Loan_Number"].ToString(),
          PolicyholderName = row1["policyholder_name"].ToString(),
          PolicyholderGender = Convert.ToInt32(row1["policyholder_gender"].ToString()),
          PolicyholderDOB = Convert.ToDateTime(row1["policyholder_dob"].ToString()),
          PolicyholderIDType = Convert.ToInt32(row1["policyholder_id_type"].ToString()),
          PolicyholderIDNo = row1["policyholder_Id_No"].ToString(),
          PolicyholderPhoneNumber = row1["policyholder_phone_number"].ToString(),
          PolicyholderPhoneNumber2 = row1["policyholder_phone_number2"].ToString(),
          PolicyholderEmail = row1["policyholder_email"].ToString(),
          PolicyholderAddress = row1["policyholder_address"].ToString(),
          CHANNEL_ID = row1["channel_id"].ToString(),
          CHANNEL_ITEM_ID = row1["channel_item_id"].ToString(),
          CHANNEL_LOCATION_ID = row1["channel_location_id"].ToString(),
          SALE_AGENT_ID = row1["sale_agent_id"].ToString()
        };
        bl_micro_application_insurance applicationInsurance = new bl_micro_application_insurance()
        {
          PRODUCT_ID = row1["product_id"].ToString(),
          PACKAGE = row1["package"].ToString(),
          PAYMENT_PERIOD = Convert.ToInt32(row1["payment_period"].ToString()),
          TERME_OF_COVER = Convert.ToInt32(row1["term_of_cover"].ToString()),
          SUM_ASSURE = Convert.ToDouble(row1["sum_assure"].ToString()),
          PAY_MODE = Convert.ToInt32(row1["pay_mode"].ToString()),
          PREMIUM = Convert.ToDouble(row1["premium"].ToString()),
          ANNUAL_PREMIUM = Convert.ToDouble(row1["annual_premium"].ToString()),
          DISCOUNT_AMOUNT = Convert.ToDouble(row1["discount_amount"].ToString()),
          TOTAL_AMOUNT = Convert.ToDouble(row1["total_amount"].ToString()),
          USER_PREMIUM = Convert.ToDouble(row1["user_premium"].ToString()),
          PAYMENT_CODE = row1["payment_code"].ToString(),
          COVER_TYPE = (bl_micro_product_config.PERIOD_TYPE)Enum.Parse(typeof(bl_micro_product_config.PERIOD_TYPE), row1["TERM_OF_COVER_TYPE"].ToString())
        };
        bl_micro_application_insurance_rider applicationInsuranceRider;
        if (row1["rider_product_id"].ToString() != "")
        {
          da_micro_product_rider.GetMicroProductByProductID(row1["rider_product_id"].ToString());
          applicationInsuranceRider = new bl_micro_application_insurance_rider()
          {
            PRODUCT_ID = row1["rider_product_id"].ToString(),
            SUM_ASSURE = Convert.ToDouble(row1["rider_sum_assure"].ToString()),
            PREMIUM = Convert.ToDouble(row1["rider_premium"].ToString()),
            ANNUAL_PREMIUM = Convert.ToDouble(row1["rider_annual_premium"].ToString()),
            DISCOUNT_AMOUNT = Convert.ToDouble(row1["rider_discount_amount"].ToString()),
            TOTAL_AMOUNT = Convert.ToDouble(row1["rider_total_amount"].ToString())
          };
        }
        else
          applicationInsuranceRider = (bl_micro_application_insurance_rider) null;
        foreach (DataRow row2 in (InternalDataCollectionBase) table2.Rows)
          applicationBeneficiaryList.Add(new bl_micro_application_beneficiary()
          {
            ID = row2["id"].ToString(),
            FULL_NAME = row2["full_name"].ToString(),
            AGE = row2["age"].ToString(),
            RELATION = row2["relation"].ToString(),
            PERCENTAGE_OF_SHARE = Convert.ToDouble(row2["percentage_of_share"].ToString()),
            ADDRESS = row2["address"].ToString(),
            DOB = Convert.ToDateTime(row2["DOB"].ToString()),
            Gender = Convert.ToInt32(row2["Gender"].ToString()),
            IdType = Convert.ToInt32(row2["Id_Type"].ToString()),
            IdNo = row2["id_no"].ToString()
          });
        bl_micro_application_beneficiary.PrimaryBeneciary primaryBeneciary = (bl_micro_application_beneficiary.PrimaryBeneciary) null;
        if (table4.Rows.Count > 0)
        {
          DataRow row3 = table4.Rows[0];
          primaryBeneciary = new bl_micro_application_beneficiary.PrimaryBeneciary()
          {
            Id = row3["id"].ToString(),
            FullName = row3["full_name"].ToString(),
            LoanNumber = row3["Loan_number"].ToString(),
            Address = row3["Address"].ToString()
          };
        }
        bl_micro_application_questionaire applicationQuestionaire = new bl_micro_application_questionaire()
        {
          QUESTION_ID = row1["question_id"].ToString(),
          ANSWER = Convert.ToInt32(row1["answer"].ToString()),
          ANSWER_REMARKS = row1["answer_remarks"].ToString()
        };
        applicationDetail.PolicyId = row1["policy_id"].ToString();
        applicationDetail.PolicyNumber = row1["policy_number"].ToString();
        applicationDetail.PolicyStatus = row1["policy_status"].ToString();
        applicationDetail.Age = Calculation.Culculate_Customer_Age(applicationCustomer.DATE_OF_BIRTH.ToString("dd/MM/yyyy"), microApplication.APPLICATION_DATE);
        applicationDetail.IssueDate = Convert.ToDateTime(row1["issued_date"].ToString());
        applicationDetail.EffectiveDate = Convert.ToDateTime(row1["EFFECTIVE_DATE"].ToString());
        applicationDetail.CollectedPremium = Convert.ToDouble(row1["collected_premium"].ToString());
        applicationDetail.PaymentReferenceNo = row1["transaction_referrence_no"].ToString();
        applicationDetail.OfficeCode = row1["office_code"].ToString();
        applicationDetail.OfficeName = row1["office_name"].ToString();
        applicationDetail.Customer = applicationCustomer;
        applicationDetail.Application = microApplication;
        applicationDetail.Insurance = applicationInsurance;
        applicationDetail.Rider = applicationInsuranceRider;
        applicationDetail.Beneficiaries = applicationBeneficiaryList;
        applicationDetail.Questionaire = applicationQuestionaire;
        applicationDetail.PrimaryBeneficiary = primaryBeneciary;
        List<bl_micro_application_details.SubApplication> subApplicationList = new List<bl_micro_application_details.SubApplication>();
        foreach (DataRow row4 in (InternalDataCollectionBase) table3.Rows)
          subApplicationList.Add(new bl_micro_application_details.SubApplication()
          {
            ApplicationId = row4["application_id"].ToString().Trim(),
            ApplicationNumber = row4["application_number"].ToString().Trim(),
            BasicAmount = Convert.ToDouble(row4["Total_basic_amount"].ToString()),
            RiderAmount = Convert.ToDouble(row4["total_rider_amount"].ToString()),
            TotalAmount = Convert.ToDouble(row4["total_amount"].ToString()),
            ClientType = row4["client_type"].ToString(),
            SumAssure = Convert.ToDouble(row4["sum_assure"].ToString())
          });
        applicationDetail.SubApplications = subApplicationList;
      }
    }
    catch (Exception ex)
    {
      applicationDetail = (bl_micro_application_details) null;
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [GetApplicationDetail(string applicationNumber)] in class [da_micro_application], detail: " +ex.Message+"=>"+ex.StackTrace);
    }
    return applicationDetail;
  }

  public static List<bl_micro_application.ApplicationFilter> GetApplicationNumberMainSub(
    string applicationNumber)
  {
    List<bl_micro_application.ApplicationFilter> applicationNumberMainSub = new List<bl_micro_application.ApplicationFilter>();
    try
    {
      da_micro_application.db = new DB();
      DataTable data = da_micro_application.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_GET_APPLICATION_NO_MAIN_SUB", new string[,]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        }
      }, "da_micro_application =>GetApplicationNumberMainSub(string applicationNumber)");
      if (da_micro_application.db.RowEffect == -1)
      {
        da_micro_application._MESSAGE = da_micro_application.db.Message;
        da_micro_application._SUCCESS = false;
      }
      else
      {
        da_micro_application._MESSAGE = "Sucess";
        da_micro_application._SUCCESS = true;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
          applicationNumberMainSub.Add(new bl_micro_application.ApplicationFilter()
          {
            ApplicationId = row["application_id"].ToString(),
            ApplicationNumber = row["application_number"].ToString(),
            MainApplicationNumber = row["main_application_number"].ToString(),
            ApplicationType = row["application_type"].ToString() == "N" ? Helper.ApplicationType.N : Helper.ApplicationType.R,
            NumbersApplication = Convert.ToInt32(row["NUMBERS_OF_APPLICATION_FIRST_YEAR"].ToString()),
            NumbersPurchasingYear = Convert.ToInt32(row["NUMBERS_OF_PURCHASING_YEAR"].ToString())
          });
      }
    }
    catch (Exception ex)
    {
      applicationNumberMainSub = (List<bl_micro_application.ApplicationFilter>) null;
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [List<bl_micro_application. ApplicationFilter> GetApplicationNumberMainSub(string applicationNumber)] in class [da_micro_application], detail: "+ex.Message+"=>"+ex.StackTrace);
    }
    return applicationNumberMainSub;
  }

  public static bl_micro_application.bl_application_for_issue GetApplicationForIssuePolicy(
    string applicationNumber)
  {
    bl_micro_application.bl_application_for_issue applicationForIssuePolicy = new bl_micro_application.bl_application_for_issue();
    try
    {
      DataSet dataSet = new DB().GetDataSet(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_FOR_ISSUE_POLICY", new string[,]
      {
        {
          "@APPLICATION_NUMBER",
          applicationNumber
        }
      });
      bl_micro_application_customer customer = applicationForIssuePolicy.Customer;
      bl_micro_application application = applicationForIssuePolicy.Application;
      bl_micro_application_insurance insurance = applicationForIssuePolicy.Insurance;
      bl_micro_application_insurance_rider rider = applicationForIssuePolicy.Rider;
      List<bl_micro_application_beneficiary> applicationBeneficiaryList = new List<bl_micro_application_beneficiary>();
      bl_micro_application_questionaire questionaire = applicationForIssuePolicy.Questionaire;
      bl_micro_application_beneficiary.PrimaryBeneciary primaryBeneciary = new bl_micro_application_beneficiary.PrimaryBeneciary();
      if (dataSet.Tables.Count > 0)
      {
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[2];
        DataRow row1 = table1.Rows[0];
        bl_micro_application_customer applicationCustomer = new bl_micro_application_customer()
        {
          CUSTOMER_ID = row1["application_customer_id"].ToString(),
          ID_TYPE = row1["id_type"].ToString(),
          ID_NUMBER = row1["id_number"].ToString(),
          FIRST_NAME_IN_ENGLISH = row1["first_name_in_english"].ToString(),
          LAST_NAME_IN_ENGLISH = row1["last_name_in_english"].ToString(),
          FIRST_NAME_IN_KHMER = row1["first_name_in_khmer"].ToString(),
          LAST_NAME_IN_KHMER = row1["last_name_in_khmer"].ToString(),
          GENDER = row1["gender"].ToString(),
          DATE_OF_BIRTH = Convert.ToDateTime(row1["date_of_birth"].ToString()),
          NATIONALITY = row1["nationality"].ToString(),
          MARITAL_STATUS = row1["marital_status"].ToString(),
          OCCUPATION = row1["occupation"].ToString(),
          PHONE_NUMBER1 = row1["phone_number"].ToString(),
          EMAIL1 = row1["email"].ToString(),
          VILLAGE_EN = row1["village_en"].ToString(),
          COMMUNE_EN = row1["commune_en"].ToString(),
          DISTRICT_EN = row1["district_en"].ToString(),
          PROVINCE_EN = row1["province_en"].ToString(),
          HOUSE_NO_EN = row1["house_no_en"].ToString(),
          STREET_NO_EN = row1["street_no_en"].ToString()
        };
        bl_micro_application microApplication = new bl_micro_application()
        {
          APPLICATION_NUMBER = row1["application_number"].ToString(),
          APPLICATION_CUSTOMER_ID = row1["application_customer_id"].ToString(),
          CHANNEL_ID = row1["channel_id"].ToString(),
          CHANNEL_ITEM_ID = row1["channel_item_id"].ToString(),
          CHANNEL_LOCATION_ID = row1["channel_location_id"].ToString(),
          APPLICATION_DATE = Convert.ToDateTime(row1["application_date"].ToString()),
          APPLICATION_ID = row1["Application_id"].ToString(),
          SALE_AGENT_ID = row1["sale_agent_id"].ToString(),
          REFERRER = row1["referrer"].ToString(),
          REFERRER_ID = row1["referrer_id"].ToString(),
          CLIENT_TYPE = row1["client_type"].ToString(),
          CLIENT_TYPE_REMARKS = row1["client_type_remarks"].ToString(),
          CLIENT_TYPE_RELATION = row1["client_type_relation"].ToString(),
          RENEW_FROM_POLICY = row1["renew_from_policy"].ToString()
        };
        bl_micro_application_insurance applicationInsurance = new bl_micro_application_insurance()
        {
          APPLICATION_NUMBER = microApplication.APPLICATION_NUMBER,
          PRODUCT_ID = row1["product_id"].ToString(),
          PACKAGE = row1["package"].ToString(),
          PAYMENT_PERIOD = Convert.ToInt32(row1["payment_period"].ToString()),
          TERME_OF_COVER = Convert.ToInt32(row1["term_of_cover"].ToString()),
          SUM_ASSURE = Convert.ToDouble(row1["sum_assure"].ToString()),
          PAY_MODE = Convert.ToInt32(row1["pay_mode"].ToString()),
          PREMIUM = Convert.ToDouble(row1["premium"].ToString()),
          ANNUAL_PREMIUM = Convert.ToDouble(row1["annual_premium"].ToString()),
          DISCOUNT_AMOUNT = Convert.ToDouble(row1["discount_amount"].ToString()),
          TOTAL_AMOUNT = Convert.ToDouble(row1["total_amount"].ToString()),
          USER_PREMIUM = Convert.ToDouble(row1["user_premium"].ToString()),
          PAYMENT_CODE = row1["payment_code"].ToString(),
          COVER_TYPE = (bl_micro_product_config.PERIOD_TYPE) Enum.Parse(typeof (bl_micro_product_config.PERIOD_TYPE), row1["TERM_OF_COVER_TYPE"].ToString())
        };
        bl_micro_application_insurance_rider applicationInsuranceRider = new bl_micro_application_insurance_rider()
        {
          APPLICATION_NUMBER = microApplication.APPLICATION_NUMBER,
          PRODUCT_ID = row1["rider_product_id"].ToString(),
          SUM_ASSURE = Convert.ToDouble(row1["rider_sum_assure"].ToString()),
          PREMIUM = Convert.ToDouble(row1["rider_premium"].ToString()),
          ANNUAL_PREMIUM = Convert.ToDouble(row1["rider_annual_premium"].ToString()),
          DISCOUNT_AMOUNT = Convert.ToDouble(row1["rider_discount_amount"].ToString()),
          TOTAL_AMOUNT = Convert.ToDouble(row1["rider_total_amount"].ToString())
        };
        foreach (DataRow row2 in (InternalDataCollectionBase) table2.Rows)
          applicationBeneficiaryList.Add(new bl_micro_application_beneficiary()
          {
            APPLICATION_NUMBER = row2["application_number"].ToString(),
            ID = row2["id"].ToString(),
            FULL_NAME = row2["full_name"].ToString(),
            AGE = row2["age"].ToString(),
            RELATION = row2["relation"].ToString(),
            PERCENTAGE_OF_SHARE = Convert.ToDouble(row2["percentage_of_share"].ToString()),
            ADDRESS = row2["address"].ToString(),
            DOB = Convert.ToDateTime(row2["dob"].ToString()),
            IdType = Convert.ToInt32(row2["id_type"].ToString()),
            IdNo = row2["id_no"].ToString(),
            Gender = Convert.ToInt32(row2["gender"].ToString())
          });
        bl_micro_application_questionaire applicationQuestionaire = new bl_micro_application_questionaire()
        {
          APPLICATION_NUMBER = microApplication.APPLICATION_NUMBER,
          QUESTION_ID = row1["question_id"].ToString(),
          ANSWER = Convert.ToInt32(row1["answer"].ToString()),
          ANSWER_REMARKS = row1["answer_remarks"].ToString()
        };
        if (table3.Rows.Count > 0)
        {
          DataRow row3 = table3.Rows[0];
          primaryBeneciary = new bl_micro_application_beneficiary.PrimaryBeneciary()
          {
            Id = row3["ID"].ToString(),
            ApplicationNumber = row3["application_number"].ToString(),
            FullName = row3["full_name"].ToString(),
            LoanNumber = row3["loan_number"].ToString(),
            Address = row3["address"].ToString()
          };
        }
        applicationForIssuePolicy.PolicyNumber = row1["policy_number"].ToString();
        applicationForIssuePolicy.Customer = applicationCustomer;
        applicationForIssuePolicy.Application = microApplication;
        applicationForIssuePolicy.Insurance = applicationInsurance;
        applicationForIssuePolicy.Rider = applicationInsuranceRider;
        applicationForIssuePolicy.Beneficiaries = applicationBeneficiaryList;
        applicationForIssuePolicy.Questionaire = applicationQuestionaire;
        applicationForIssuePolicy.PrimaryBeneciary = primaryBeneciary;
      }
    }
    catch (Exception ex)
    {
      applicationForIssuePolicy = (bl_micro_application.bl_application_for_issue) null;
      da_micro_application._SUCCESS = false;
      da_micro_application._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [ bl_micro_application. bl_application_for_issue GetApplicationForIssuePolicy(string applicationNumber)] in class [da_micro_application], detail: "+ex.Message+"=>"+ex.StackTrace);
    }
    return applicationForIssuePolicy;
  }

  public enum ApplicationTypeOption
  {
    IND,
    BDL,
  }
}
