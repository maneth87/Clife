using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_customer_lead_app_temp
/// </summary>
public class da_customer_lead_app_temp
{
	public da_customer_lead_app_temp()
	{
		//
		// TODO: Add constructor logic here
		//
        SUCCESS = false;
        MESSAGE = "";
	}
    public static string MESSAGE;
    public static bool SUCCESS;
    public static bl_customer_lead_app_temp GetCustomerLeadAppTempByID(string ID)
    {
        bl_customer_lead_app_temp obj = new bl_customer_lead_app_temp();
        try
        {
            DB db = new DB();
            DataTable tbl = new DataTable();
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_CUSTOMER_LEAD_APP_TEMP_BY_ID", new string[,] {
            
            {"@ID", ID}
            }, "da_customer_lead_app_temp ==> GetCustomerLeadAppTempByID(string ID)");
            foreach (DataRow row in tbl.Rows)
            {
                obj = (new bl_customer_lead_app_temp()
                {
                    ID = row["ID"].ToString(),
                    BranchCode = row["BRANCH_CODE"].ToString(),
                    BranchName = row["BRANCH_NAME"].ToString(),
                    ApplicationID = row["APPLICATION_ID"].ToString(),
                    ReferralStaffId = row["REFERRAL_STAFF_ID"].ToString(),
                    ReferralStaffName = row["REFERRAL_STAFF_NAME"].ToString(),
                    ReferralStaffPosition = row["REFERRAL_STAFF_POSITION"].ToString(),
                    ClientType = row["CLIENT_TYPE"].ToString(),
                    ClientCIF = row["CIF"].ToString(),
                    ClientNameENG = row["CLIENT_NAME_IN_ENGLISH"].ToString(),
                    ClientNameKHM = row["CLIENT_NAME_IN_KHMER"].ToString(),
                    ClientGender = row["GENDER"].ToString(),
                    ClientNationality = row["NATIONALITY"].ToString(),
                    ClientDoB = Convert.ToDateTime(row["DATE_OF_BIRTH"].ToString()),
                    ClientVillage = row["VILLAGE"].ToString(),
                    ClientCommune = row["COMMUNE"].ToString(),
                    ClientDistrict = row["DISTRICT"].ToString(),
                    ClientProvince = row["PROVINCE"].ToString(),
                    DocumentType = row["ID_TYPE"].ToString(),
                    DocumentId = row["ID_NUMBER"].ToString(),
                    ClientPhoneNumber = row["PHONE_NUMBER"].ToString(),
                    Interest = row["INTEREST"].ToString(),
                    ReferredDate = Convert.ToDateTime(row["REFERRED_DATE"].ToString()),
                    Status = row["STATUS"].ToString(),
                    StatusRemarks = row["STATUS_REMARKS"].ToString(),
                    Remarks = row["REMARKS"].ToString(),
                    CreatedBy = row["CREATED_BY"].ToString(),
                    CreatedOn = Convert.ToDateTime(row["CREATED_ON"].ToString()),
                    UpdatedBy = row["UPDATED_BY"].ToString(),
                    UpdatedOn = Convert.ToDateTime(row["UPDATED_ON"].ToString()),
                    CHANNEL_ITEM_ID = row["CHANNEL_ITEM_ID"].ToString(),
                    CHANNEL_LOCATION_ID=row["CHANNEL_LOCATION_ID"].ToString(),
                    SALE_AGENT_ID=row["SALE_AGENT_ID"].ToString(),
                    SALE_AGENT_NAME=row["SALE_AGENT_NAME"].ToString()
                });
            }
            MESSAGE = "Success";
            SUCCESS = true;

        }
        catch (Exception ex)
        {
            MESSAGE = ex.Message;
            SUCCESS = false;
            obj = new bl_customer_lead_app_temp();
            Log.AddExceptionToLog("Error function [GetCustomerLeadAppTempByID(string ID)] in class [da_customer_lead_app_temp], detail: " + ex.Message + " ==> " + ex.StackTrace);

        }
        return obj;
    }
  
    public static bool DeleteCustomerLeadAppTempByID(string ID)
    {
        bool result = false;
        try
        {
            DB db = new DB();

            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_CUSTOMER_LEAD_APP_TEMP_DELET_BY_ID", new string[,] { 
            {"@ID", ID}
            }, "da_customer_lead_app_temp => DeleteCustomerLeadAppTempByID(string ID)");

            if (result)
            {
                if (db.RowEffect == 0)
                {
                    MESSAGE = "No row deleted.";
                }
            }
            else
            {
                MESSAGE = db.Message;
            }
            SUCCESS = result;

        }
        catch (Exception ex)
        {
            MESSAGE = ex.Message;
            SUCCESS = false;
            Log.AddExceptionToLog("Error function [DeleteCustomerLeadAppTempByID(string ID)] in class [da_customer_lead_app_temp], detail: " + ex.Message + " ==> " + ex.StackTrace);

        }
        return result;
    }
}