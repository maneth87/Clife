
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

public class da_micro_policy_beneficiary
{
  private static bool _SUCCESS = false;
  private static string _MESSAGE = "";
  private static DB db = new DB();

  public static bool SUCCESS {get{return da_micro_policy_beneficiary._SUCCESS;}}

  public static string MESSAGE {get{return da_micro_policy_beneficiary._MESSAGE;}}

  public static bool SaveBeneficiary(bl_micro_policy_beneficiary BENEFICIARY)
  {
    try
    {
      da_micro_policy_beneficiary._SUCCESS = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_INSERT", new string[,]
      {
        {
          "@ID",
          BENEFICIARY.ID
        },
        {
          "@POLICY_ID",
          BENEFICIARY.POLICY_ID
        },
        {
          "@FULL_NAME",
          BENEFICIARY.FULL_NAME
        },
        {
          "@AGE",
          BENEFICIARY.AGE
        },
        {
          "@RELATION",
          BENEFICIARY.RELATION
        },
        {
          "@PERCENTAGE_OF_SHARE",
          string.Concat((object) BENEFICIARY.PERCENTAGE_OF_SHARE)
        },
        {
          "@ADDRESS",
          BENEFICIARY.ADDRESS
        },
        {
          "@CREATED_BY",
          BENEFICIARY.CREATED_BY
        },
        {
          "@CREATED_ON",
          string.Concat((object) BENEFICIARY.CREATED_ON)
        },
        {
          "@REMARKS",
          BENEFICIARY.REMARKS
        },
        {
          "@DOB",
          string.Concat((object) BENEFICIARY.BirthDate)
        },
        {
          "@GENDER",
          BENEFICIARY.Gender ?? ""
        },
        {
          "@ID_TYPE",
          string.Concat((object) BENEFICIARY.IdType)
        },
        {
          "@ID_NO",
          BENEFICIARY.IdNo
        }
      }, "da_micro_policy_benificiary=>SaveBeneficiary(bl_micro_policy_beneficiary BENEFICIARY)");
      da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.RowEffect != -1 ? "Success" : da_micro_policy_beneficiary.db.Message;
    }
    catch (Exception ex)
    {
      da_micro_policy_beneficiary._SUCCESS = false;
      da_micro_policy_beneficiary._MESSAGE = ex.Message;
      Log.AddExceptionToLog("Error function [SaveBeneficiary(bl_micro_policy_beneficiary BENEFICIARY)] in class [da_micro_policy_beneficiary], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return da_micro_policy_beneficiary._SUCCESS;
  }

  public static bl_micro_policy_beneficiary GetBeneficiary(string policyId, string userName = "")
  {
    bl_micro_policy_beneficiary beneficiary = new bl_micro_policy_beneficiary();
    try
    {
      DataTable data = da_micro_policy_beneficiary.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_GET_BY_POLICY_ID", new string[1, 2]
      {
        {
          "@POLICY_ID",
          policyId
        }
      }, "da_micro_policy_benificiary=>GetBeneficiary(string policyId, string userName)");
      if (da_micro_policy_beneficiary.db.RowEffect == -1)
      {
        da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.Message;
      }
      else
      {
        da_micro_policy_beneficiary._MESSAGE = "Success";
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
          beneficiary = new bl_micro_policy_beneficiary()
          {
            ID = row["id"].ToString(),
            POLICY_ID = row["policy_id"].ToString(),
            FULL_NAME = row["full_name"].ToString(),
            AGE = row["age"].ToString(),
            RELATION = row["relation"].ToString(),
            PERCENTAGE_OF_SHARE = Convert.ToDouble(row["percentage_of_share"].ToString()),
            ADDRESS = row["address"].ToString(),
            REMARKS = row["remarks"].ToString()
          };
      }
    }
    catch (Exception ex)
    {
      da_micro_policy_beneficiary._SUCCESS = false;
      da_micro_policy_beneficiary._MESSAGE = ex.Message;
      Log.SaveLog(new bl_log()
      {
        LogDate = DateTime.Now,
        Class = typeof(da_micro_policy_beneficiary).Name,
        FunctionName = MethodBase.GetCurrentMethod().Name,
        LogType = "ERROR",
        ErrorLine = Log.GetLineNumber(ex),
        Message = ex.Message+"=>"+ex.StackTrace,
        UserName = userName
      });
    }
    return beneficiary;
  }

  public static List<bl_micro_policy_beneficiary> GetBeneficiaryList(
    string policyId,
    string userName = "")
  {
    List<bl_micro_policy_beneficiary> beneficiaryList = new List<bl_micro_policy_beneficiary>();
    try
    {
      DataTable data = da_micro_policy_beneficiary.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_GET_BY_POLICY_ID", new string[,]
      {
        {
          "@POLICY_ID",
          policyId
        }
      }, "da_micro_policy_benificiary=>GetBeneficiaryList(string policyId, string userName)");
      if (da_micro_policy_beneficiary.db.RowEffect == -1)
      {
        da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.Message;
      }
      else
      {
        da_micro_policy_beneficiary._MESSAGE = "Success";
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          bl_micro_policy_beneficiary policyBeneficiary = new bl_micro_policy_beneficiary()
          {
            ID = row["id"].ToString(),
            POLICY_ID = row["policy_id"].ToString(),
            FULL_NAME = row["full_name"].ToString(),
            AGE = row["age"].ToString(),
            RELATION = row["relation"].ToString(),
            PERCENTAGE_OF_SHARE = Convert.ToDouble(row["percentage_of_share"].ToString()),
            ADDRESS = row["address"].ToString(),
            REMARKS = row["remarks"].ToString()
          };
          beneficiaryList.Add(policyBeneficiary);
        }
      }
    }
    catch (Exception ex)
    {
      da_micro_policy_beneficiary._SUCCESS = false;
      da_micro_policy_beneficiary._MESSAGE = ex.Message;
      Log.SaveLog(new bl_log()
      {
        LogDate = DateTime.Now,
        Class = typeof (da_micro_policy_beneficiary).Name,
        FunctionName = MethodBase.GetCurrentMethod().Name,
        LogType = "ERROR",
        ErrorLine = Log.GetLineNumber(ex),
        Message =ex.Message+"=>"+ex.StackTrace,
        UserName = userName
      });
    }
    return beneficiaryList;
  }

  public static bool UpdateBeneficiary(bl_micro_policy_beneficiary BENEFICIARY)
  {
    bool flag;
    try
    {
      flag = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_UPDATE", new string[,]
      {
        {
          "@POLICY_ID",
          BENEFICIARY.POLICY_ID
        },
        {
          "@FULL_NAME",
          BENEFICIARY.FULL_NAME
        },
        {
          "@AGE",
          BENEFICIARY.AGE
        },
        {
          "@RELATION",
          BENEFICIARY.RELATION
        },
        {
          "@PERCENTAGE_OF_SHARE",
          string.Concat((object) BENEFICIARY.PERCENTAGE_OF_SHARE)
        },
        {
          "@ADDRESS",
          BENEFICIARY.ADDRESS
        },
        {
          "@UPDATED_BY",
          BENEFICIARY.UPDATED_BY
        },
        {
          "@UPDATED_ON",
          string.Concat((object) BENEFICIARY.UPDATED_ON)
        },
        {
          "@REMARKS",
          BENEFICIARY.REMARKS
        }
      }, "da_micro_policy_benificiary=>UpdateBeneficiary(bl_micro_policy_beneficiary BENEFICIARY)");
      da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.RowEffect != -1 ? "Success" : da_micro_policy_beneficiary.db.Message;
      da_micro_policy_beneficiary._SUCCESS = flag;
    }
    catch (Exception ex)
    {
      da_micro_policy_beneficiary._SUCCESS = false;
      da_micro_policy_beneficiary._MESSAGE = ex.Message;
      flag = false;
      Log.AddExceptionToLog("Error function [UpdateBeneficiary(bl_micro_policy_beneficiary BENEFICIARY)] in class [da_micro_policy_beneficiary], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return flag;
  }

  public static bool DeleteBeneficiary(string ID)
  {
    bool flag;
    try
    {
      flag = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_DELETE", new string[,]
      {
        {
          "@ID",
          ID
        }
      }, "da_micro_policy_beneficiary=>DeleteBeneficiary(string ID)");
    }
    catch (Exception ex)
    {
      da_micro_policy_beneficiary._SUCCESS = false;
      da_micro_policy_beneficiary._MESSAGE = ex.Message;
      flag = false;
      Log.AddExceptionToLog("Error function [DeleteBeneficiary(string ID)] in class [da_micro_policy_beneficiary], detail: "+ex.Message+"==>"+ex.StackTrace);
    }
    return flag;
  }

  public class beneficiary_primary
  {
    public static bool Delete(
      da_micro_policy_beneficiary.beneficiary_primary.DeleteOption deleteOption,
      string value)
    {
      bool flag = false;
      try
      {
        switch (deleteOption)
        {
          case da_micro_policy_beneficiary.beneficiary_primary.DeleteOption.BY_ID:
            flag = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_PRIMARY_DELETE_BY_ID", new string[,]
            {
              {
                "@ID",
                value
              }
            }, "da_micro_policy_beneficiary.beneficiary_primary=>Delete(DeleteOption deleteOption, string value)");
            break;
          case da_micro_policy_beneficiary.beneficiary_primary.DeleteOption.BY_POLICY_ID:
            flag = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_PRIMARY_DELETE", new string[1, 2]
            {
              {
                "@POLICY_ID",
                value
              }
            }, "da_micro_policy_beneficiary.beneficiary_primary=>Delete(DeleteOption deleteOption, string value)");
            break;
        }
      }
      catch (Exception ex)
      {
        da_micro_policy_beneficiary._SUCCESS = false;
        da_micro_policy_beneficiary._MESSAGE = ex.Message;
        flag = false;
        Log.AddExceptionToLog("Error function [Delete(DeleteOption deleteOption, string value)] in class [da_micro_policy_beneficiary.beneficiary_primary], detail: "+ex.Message+"==>"+ex.StackTrace);
      }
      return flag;
    }

    public static bl_micro_policy_beneficiary.beneficiary_primary GetBeneficiaryPrimary(string policyId)
    {
      bl_micro_policy_beneficiary.beneficiary_primary beneficiaryPrimary = new bl_micro_policy_beneficiary.beneficiary_primary();
      try
      {
        DataTable data = da_micro_policy_beneficiary.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_PRIMARY_GET", new string[,]
        {
          {
            "@POLICY_ID",
            policyId
          }
        }, "da_micro_policy_benificiary.beneficary_primary=>bl_micro_policy_beneficiary. beneficiary_primary GetBeneficiaryPrimary(string policyId)");
        if (da_micro_policy_beneficiary.db.RowEffect == -1)
        {
          da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.Message;
        }
        else
        {
          da_micro_policy_beneficiary._MESSAGE = "Success";
          foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
            beneficiaryPrimary = new bl_micro_policy_beneficiary.beneficiary_primary()
            {
              Id = row["id"].ToString(),
              FullName = row["full_name"].ToString(),
              LoanNumber = row["loan_number"].ToString(),
              PolicyId = row["policy_id"].ToString(),
              Address = row["address"].ToString(),
              CreatedBy = row["created_by"].ToString(),
              CreatedOn = Convert.ToDateTime(row["created_on"].ToString()),
              CreatedRemarks = row["created_remarks"].ToString(),
              UpdatedBy = row["updated_by"].ToString(),
              UpdatedOn = Convert.ToDateTime(row["updated_on"].ToString()),
              UpdatedRemarks = row["updated_remarks"].ToString()
            };
        }
      }
      catch (Exception ex)
      {
        da_micro_policy_beneficiary._SUCCESS = false;
        da_micro_policy_beneficiary._MESSAGE = ex.Message;
        Log.AddExceptionToLog("Error function [GetBeneficiaryPrimary(string policyId)] in class [da_micro_policy_beneficiary.beneficiary_primary], detail: "+ex.Message+"==>"+ex.StackTrace);
      }
      return beneficiaryPrimary;
    }

    public static bool Update(
      bl_micro_policy_beneficiary.beneficiary_primary beneficiary)
    {
      bool flag;
      try
      {
        bl_micro_policy_beneficiary.beneficiary_primary beneficiaryPrimary = beneficiary;
        flag = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_PRIMARY_UPDATE", new string[,]
        {
          {
            "@POLICY_ID",
            beneficiaryPrimary.PolicyId
          },
          {
            "@FULL_NAME",
            beneficiaryPrimary.FullName
          },
          {
            "@LOAN_NUMBER",
            beneficiaryPrimary.LoanNumber
          },
          {
            "@ADDRESS",
            beneficiaryPrimary.Address
          },
          {
            "@UPDATED_BY",
            beneficiaryPrimary.UpdatedBy
          },
          {
            "@UPDATED_ON",
            string.Concat((object) beneficiaryPrimary.UpdatedOn)
          },
          {
            "@UPDATED_REMARKS",
            beneficiaryPrimary.UpdatedRemarks
          }
        }, "da_micro_policy_benificiary.beneficiary_primary=>Update(bl_micro_policy_beneficiary.beneficiary_primary beneficiary))");
        da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.RowEffect != -1 ? "Success" : da_micro_policy_beneficiary.db.Message;
        da_micro_policy_beneficiary._SUCCESS = flag;
      }
      catch (Exception ex)
      {
        da_micro_policy_beneficiary._SUCCESS = false;
        da_micro_policy_beneficiary._MESSAGE = ex.Message;
        flag = false;
        Log.AddExceptionToLog("Error function [Update(bl_micro_policy_beneficiary.beneficiary_primary beneficiary)] in class [da_micro_policy_beneficiary.beneficiary_primary], detail: "+ex.Message+"==>"+ex.StackTrace);
      }
      return flag;
    }

    public static bool Save(
      bl_micro_policy_beneficiary.beneficiary_primary beneficiary)
    {
      bool flag;
      try
      {
        bl_micro_policy_beneficiary.beneficiary_primary beneficiaryPrimary = beneficiary;
        flag = da_micro_policy_beneficiary.db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_BENEFICIARY_PRIMARY_INSERT", new string[,]
        {
          {
            "@ID",
            beneficiaryPrimary.Id
          },
          {
            "@POLICY_ID",
            beneficiaryPrimary.PolicyId
          },
          {
            "@FULL_NAME",
            beneficiaryPrimary.FullName
          },
          {
            "@LOAN_NUMBER",
            beneficiaryPrimary.LoanNumber
          },
          {
            "@ADDRESS",
            beneficiaryPrimary.Address
          },
          {
            "@CREATED_BY",
            beneficiaryPrimary.CreatedBy
          },
          {
            "@CREATED_ON",
            string.Concat((object) beneficiaryPrimary.CreatedOn)
          },
          {
            "@CREATED_REMARKS",
            beneficiaryPrimary.CreatedRemarks
          }
        }, "da_micro_policy_benificiary.beneficiary_primary=>Save(bl_micro_policy_beneficiary.beneficiary_primary beneficiary))");
        da_micro_policy_beneficiary._MESSAGE = da_micro_policy_beneficiary.db.RowEffect != -1 ? "Success" : da_micro_policy_beneficiary.db.Message;
        da_micro_policy_beneficiary._SUCCESS = flag;
      }
      catch (Exception ex)
      {
        da_micro_policy_beneficiary._SUCCESS = false;
        da_micro_policy_beneficiary._MESSAGE = ex.Message;
        flag = false;
        Log.AddExceptionToLog("Error function [Save(bl_micro_policy_beneficiary.beneficiary_primary beneficiary)] in class [da_micro_policy_beneficiary.beneficiary_primary], detail: "+ex.Message+"==>"+ex.StackTrace);
      }
      return flag;
    }

    public enum DeleteOption
    {
      BY_ID,
      BY_POLICY_ID,
    }
  }
}
