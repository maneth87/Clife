using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_group_micro_certificate
/// </summary>
public class da_group_micro_certificate
{
    private static string className = "da_group_micro_certificate";
    public da_group_micro_certificate()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool Save(bl_group_micro_certificate obj)
    {
        bool result = false;
        try
        {

            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CERTIFICATE_INSERT", new string[,] {
              
            {"@ID", obj.Id},{"@CUSTOMER_NO", obj.CustomerNo},{"@POLICY_NUMBER", obj.PolicyNumber},{"@AGENT_CODE", obj.AgentCode},
        {"@AGENT_NAME_EN", obj.AgentNameEn},{"@AGENT_NAME_KH", obj.AgentNameKh},{"@ID_TYPE", obj.IdType+""},{"@ID_EN", obj.IdEn},
        {"@ID_KH", obj.IdKh},{"@ID_NO", obj.IdNo},{"@FULL_NAME", obj.FullName},{"@GENDER", obj.Gender+""},{"@GENDER_EN", obj.GenderEn},
        {"@GENDER_KH", obj.GenderKh},{"@DATE_OF_BIRTH", obj.DateOfBirth+""},{"@AGE", obj.Age+""},{"@NATIONALITY", obj.Nationality},
        {"@ADDRESS", obj.Address},{"@PROVINCE", obj.Province},{"@PRODUCT_ID", obj.ProductId},{"@PRODUCT_NAME", obj.ProductName}
	    ,{"@PRODUCT_NAME_KH", obj.ProductNameKh},{"@SUM_ASSURE", obj.SumAssure+""},{"@TERM_OF_COVER", obj.TermOfCover+""},{"@PAYMENT_PERIOD", obj.PaymentPeriod+""},
        {"@PAY_MODE", obj.PayMode+""},{"@PAY_MODE_EN", obj.PayModeEn},{"@PAY_MODE_KH", obj.PayModeKh},{"@PREMIUM",obj.Premium+""},
        {"@ANNUAL_PREMIUM", obj.AnnualPremium+""},{"@USER_PREMIUM", obj.UserPremium+""},{"@DISCOUNT_AMOUNT", obj.DiscountAmount+""},{"@TOTAL_AMOUNT", obj.TotalAmount+""}
	    ,{"@RIDER_PRODUCT_ID", obj.RiderProductId},{"@RIDER_PRODUCT_NAME", obj.RiderProductName},{"@RIDER_PRODUCT_NAME_KH", obj.RiderProductNameKh},
        {"@RIDER_SUM_ASSURE", obj.RiderSumAssure+""},{"@RIDER_PREMIUM", obj.RiderPremium+""},{"@RIDER_ANNUAL_PREMIUM", obj.RiderAnnualPremium+""},
        {"@RIDER_DISCOUNT_AMOUNT", obj.RiderDiscountAmount+""},{"@RIDER_TOTAL_AMOUNT", obj.RiderTotalAmount+""},{"@EFFECTIVE_DATE", obj.EffectiveDate+""},
        {"@EXPIRY_DATE", obj.ExpiryDate+""}, {"@NEXT_DUE_DATE", obj.NextDueDate+""},
        {"@COVER_PERIOD_TYPE",obj.CoverPeriodType}, {"@PAY_PERIOD_TYPE",obj.PayPeriodType}
            },

                className + "=>Save(bl_group_micro_certificate obj)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [Save(bl_group_micro_certificate obj)] in call[" + className + "], detail:" + ex.Message);
        }
        return result;
    }

    public static bool UpdateCusotmerInfo(int idType, string idTypeEn, string idTypeKh, string idNo, string fullName, int gender, string genderKh, string genderEn, DateTime dob, string nationality, string oldPolicyNumber)
    {
        bool result = false;
        try
        {

            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CERTIFICATE_UPDATE_CUS_INFO", new string[,] {
            {"@POLICY_NUMBER", oldPolicyNumber}
            ,{"@ID_TYPE", idType+""}
            ,{"@ID_EN", idTypeEn}
            ,{"@ID_KH", idTypeKh}
            ,{"@ID_NO", idNo}
            ,{"@FULL_NAME", fullName}
            ,{"@GENDER", gender+""}
            ,{"@GENDER_EN", genderEn}
            ,{"@GENDER_KH", genderKh}
            ,{"@DATE_OF_BIRTH", dob+""}
          
            ,{"@NATIONALITY", nationality}
            },

                className + "=>UpdateCusotmerInfo(int idType, string idTypeEn, string idTypeKh, string idNo, string fullName, int gender, string genderKh, string genderEn, DateTime dob, string nationality, string oldPolicyNumber)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateCusotmerInfo(int idType, string idTypeEn, string idTypeKh, string idNo, string fullName, int gender, string genderKh, string genderEn, DateTime dob, string nationality, string oldPolicyNumber)] in call[" + className + "], detail:" + ex.Message);
        }
        return result;
    }

    public static bool UpdateCusotmerAddress(string address, string oldPolicyNumber)
    {
        bool result = false;
        try
        {

            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CERTIFICATE_UPDATE_CUS_ADDRESS", new string[,] {
            {"@POLICY_NUMBER", oldPolicyNumber}
            ,{"@address", address}
           
            },
                className + "=>UpdateCusotmerAddress(string address, string oldPolicyNumber)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateCusotmerAddress(string address, string oldPolicyNumber)] in call[" + className + "], detail:" + ex.Message);
        }
        return result;
    }
}