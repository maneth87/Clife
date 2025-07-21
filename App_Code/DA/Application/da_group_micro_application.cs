using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_group_micro_application
/// </summary>
public class da_group_micro_application
{
    private static string className = "da_group_micro_application";
    public da_group_micro_application()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool Save(bl_group_micro_application obj)
    {
        bool result = false;
        try
        {
            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_INSERT", new string[,] { 
            {"@ID",obj.Id}
      ,{"@APPLICATION_NUMBER", obj.ApplicationNumber}
      ,{"@APPLICATION_DATE", obj.ApplicationDate+""}
      ,{"@AGENT_CODE", obj.AgentCode}
      ,{"@AGENT_NAME_EN", obj.AgentNameEn}
      ,{"@AGENT_NAME_KH", obj.AgentNameKh}
      ,{"@ID_TYPE", obj.IdType+""}
      ,{"@ID_EN", obj.IdEn}
      ,{"@ID_KH", obj.IdKh}
      ,{"@ID_NO", obj.IdNo}
      ,{"@FIRST_NAME_IN_ENGLISH",obj.FirstNameInEnglish}
      ,{"@LAST_NAME_IN_ENGLISH",obj.LastNameInEnglish}
      ,{"@FIRST_NAME_IN_KHMER", obj.FirstNameInKhmer}
      ,{"@LAST_NAME_IN_KHMER",obj.LastNameInKhmer}
      ,{"@GENDER", obj.Gender+""}
      ,{"@GENDER_EN", obj.GenderEn}
      ,{"@GENDER_KH",obj.GenderKh}
      ,{"@DATE_OF_BIRTH", obj.BirthOfDate+""}
      ,{"@NATIONALITY", obj.Nationality}
      ,{"@MARITAL_STATUS", obj.MaritalStatus}
      ,{"@MARITAL_STATUS_KH", obj.MaritalStatusKh}
      ,{"@OCCUPATION", obj.Occupation}
      ,{"@OCCUPATION_KH", obj.OccupationKh}
      ,{"@PHONE_NUMBER", obj.PhoneNumber}
      ,{"@EMAIL",obj.Email}
      ,{"@ADDRESS", obj.Address}
      ,{"@ADDRESS_KH", obj.AddressKh}
      ,{"@PROVINCE",obj.Province}
      ,{"@PROVINCE_KH", obj.ProvinceKh}
      ,{"@PRODUCT_ID", obj.ProductId}
      ,{"@PRODUCT_NAME", obj.ProductName}
      ,{"@PRODUCT_NAME_KH", obj.ProductNameKh}
      ,{"@SUM_ASSURE", obj.SumAssure+""}
      ,{"@COVER_PERIOD_TYPE", obj.CoverPeriodType}
      ,{"@PAY_PERIOD_TYPE", obj.PayPeriodType}
      ,{"@TERM_OF_COVER", obj.TermOfCover+""}
      ,{"@PAYMENT_PERIOD",obj.PaymentPeriod+""}
      ,{"@PAY_MODE", obj.PayMode+""}
      ,{"@PAY_MODE_EN", obj.PayModeEn}
      ,{"@PAY_MODE_KH", obj.PayModeKh}
      ,{"@PREMIUM", obj.Premium+""}
      ,{"@ANNUAL_PREMIUM", obj.AnnualPremium+""}
      ,{"@USER_PREMIUM", obj.UserPremium+""}
      ,{"@DISCOUNT_AMOUNT", obj.DiscountAmount+""}
      ,{"@TOTAL_AMOUNT", obj.TotalAmount+""}
      ,{"@RIDER_PRODUCT_ID", obj.RiderProductId}
      ,{"@RIDER_PRODUCT_NAME", obj.RiderProductName}
      ,{"@RIDER_PRODUCT_NAME_KH", obj.RiderProductNameKh}
      ,{"@RIDER_SUM_ASSURE", obj.RiderSumAssure+""}
      ,{"@RIDER_PREMIUM", obj.RiderPremium+""}
      ,{"@RIDER_ANNUAL_PREMIUM", obj.RiderAnnualPremium+""}
      ,{"@RIDER_DISCOUNT_AMOUNT", obj.RiderDiscountAmount+""}
      ,{"@RIDER_TOTAL_AMOUNT", obj.RiderTotalAmount+""}
      ,{"@BEN_FULL_NAME", obj.BenFullName}
      ,{"@BEN_AGE", obj.BenAge}
      ,{"@BEN_ADDRESS", obj.BenAddress}
      ,{"@PERCENTAGE_OF_SHARE", obj.PercentageShared+""}
      ,{"@RELATION", obj.Relation}
      ,{"@RELATION_KH", obj.RelationKh}
      ,{"@QUESTION_ID", obj.QuestionId}
      ,{"@ANSWER", obj.Answer+""}
      ,{"@ANSWER_REMARKS", obj.AnswerRemarks}
      ,{"@PAYMENT_CODE", obj.PaymentCode}
      ,{"@REFERRER_ID", obj.ReferrerId}
      ,{"@REFERRER", obj.Referrer}
            }, className + "=>Save(bl_group_micro_application obj)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [Save(bl_group_micro_application obj)] in class[" + className + "], detail:" + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationNumber">Applicatioin number which is updated</param>
    /// <param name="idType"></param>
    /// <param name="IdTypeEn"></param>
    /// <param name="IdTypeKh"></param>
    /// <param name="idNo"></param>
    /// <param name="firstNameEn"></param>
    /// <param name="lastNameEn"></param>
    /// <param name="firstNameKh"></param>
    /// <param name="lastNameKh"></param>
    /// <param name="gender"></param>
    /// <param name="genderEn"></param>
    /// <param name="genderKh"></param>
    /// <param name="dob"></param>
    /// <param name="nationality"></param>
    /// <param name="maritalStatus"></param>
    /// <param name="maritalStatusKh"></param>
    /// <param name="occupation"></param>
    /// <param name="occupationKh"></param>
    /// <returns></returns>
    public static bool UpdateCustomerInfo(string applicationid, int idType, string IdTypeEn, string IdTypeKh, string idNo, string firstNameEn, string lastNameEn, string firstNameKh, string lastNameKh, int gender, string genderEn, string genderKh, DateTime dob, string nationality, string maritalStatus, string maritalStatusKh, string occupation, string occupationKh)
    {
        bool result = false;
        try
        {
            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_UPDATE_CUS_INFO", new string[,] { 
         
      {"@APPLICATION_ID",applicationid}
      ,{"@ID_TYPE", idType+""}
      ,{"@ID_EN", IdTypeEn}
      ,{"@ID_KH", IdTypeKh}
      ,{"@ID_NO", idNo}
      ,{"@FIRST_NAME_IN_ENGLISH",firstNameEn}
      ,{"@LAST_NAME_IN_ENGLISH",lastNameEn}
      ,{"@FIRST_NAME_IN_KHMER", firstNameKh}
      ,{"@LAST_NAME_IN_KHMER",lastNameKh}
      ,{"@GENDER", gender+""}
      ,{"@GENDER_EN", genderEn}
      ,{"@GENDER_KH",genderKh}
      ,{"@DATE_OF_BIRTH", dob+""}
      ,{"@NATIONALITY", nationality}
      ,{"@MARITAL_STATUS", maritalStatus}
      ,{"@MARITAL_STATUS_KH", maritalStatusKh}
      ,{"@OCCUPATION", occupation}
      ,{"@OCCUPATION_KH", occupationKh}
     
            }, className + "=> UpdateCustomerInfo(string applicationNumber, int idType, string IdTypeEn, string IdTypeKh, string idNo, string firstNameEn, string lastNameEn, string firstNameKh, string lastNameKh, int gender, string genderEn, string genderKh, DateTime dob, string nationality, string maritalStatus, string maritalStatusKh, string occupation, string occupationKh)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [ UpdateCustomerInfo(string applicationNumber, int idType, string IdTypeEn, string IdTypeKh, string idNo, string firstNameEn, string lastNameEn, string firstNameKh, string lastNameKh, int gender, string genderEn, string genderKh, DateTime dob, string nationality, string maritalStatus, string maritalStatusKh, string occupation, string occupationKh)] in class[" + className + "], detail:" + ex.Message);
        }
        return result;
    }

    public static bool UpdateCustomerContact(string applicationId, string phoneNumber, string email)
    {
        bool result = false;
        try
        {
            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_UPDATE_CUS_CONCATE", new string[,] { 
         
      {"@APPLICATION_ID",applicationId}
      ,{"@phone_number", phoneNumber}
      ,{"@email", email}
       }, className + "=> UpdateCustomerContact(string applicationId, string phoneNumber, string email)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateCustomerContact(string applicationId, string phoneNumber, string email)] in class[" + className + "], detail:" + ex.Message);
        }
        return result;
    }

    public static bool UpdateCustomerAddress(string applicationId, string address, string addressKh)
    {
        bool result = false;
        try
        {
            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_UPDATE_CUS_ADDRESS", new string[,] { 
         
      {"@application_id",applicationId}
      ,{"@address", address}
      ,{"@address_kh", addressKh}
       }, className + "=>UpdateCustomerAddress(string applicationId, string address, string addressKh)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateCustomerAddress(string applicationId, string address, string addressKh)] in class[" + className + "], detail:" + ex.Message);
        }
        return result;
    }

    public static bool UpdateCustomerBeneficiary(string applicationId, string benFullName, string benAge, string benAddress, float percentage, string benRelation, string benRelationKh)
    {
        bool result = false;
        try
        {
            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_UPDATE_CUS_BEN", new string[,] { 
         
      {"@application_id",applicationId}
      ,{"@ben_full_name", benFullName}
      ,{"@ben_age", benAge}
       ,{"@ben_address", benAddress}
        ,{"@PERCENTAGE_OF_SHARE", percentage+""}
        ,{"@relation", benRelation+""}
        ,{"@relation_kh", benRelationKh+""}
       }, className + "=>UpdateCustomerBeneficiary(string applicationId, string benFullName, string benAge, string benAddress, float percentage, string benRelation, string benRelationKh)");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateCustomerBeneficiary(string applicationId, string benFullName, string benAge, string benAddress, float percentage, string benRelation, string benRelationKh)] in class[" + className + "], detail:" + ex.Message);
        }
        return result;
    }

    public static DataTable GetApplicationDetail(string id)
    {
        DataTable tbl = new DataTable();
        try
        {
            DB db = new DB();
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_GET_BY_APP_ID", new string[,] { 
            {"@ID",id}
            }, className + "=>GetApplicationDetail(string id)");

        }
        catch (Exception ex)
        {
            tbl = new DataTable();
            Log.AddExceptionToLog("Error function [GetApplicationDetail(string id)] in class[" + className + "], detail:" + ex.Message);
        }
        return tbl;
    }
}