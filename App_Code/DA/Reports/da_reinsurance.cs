using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


/// <summary>
/// Summary description for da_reinsurance
/// </summary>
public class da_reinsurance
{
	public da_reinsurance()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Todate"></param>
    /// <param name="RetentionValue">Product Type: [Ordinary, Saving] set value = 50000</param>
    /// <returns></returns>
    public static List<bl_reinsurance> getReinsuranceRecords(DateTime Todate, Decimal RetentionValue)
    { 
        List<bl_reinsurance>ReinsuranceList= new List<bl_reinsurance>();
        bl_reinsurance Reinsurance;
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_ReinsuranceRecords_By_Date", new string[,] { { "@to_date", Todate + "" }, { "@Retention" , RetentionValue+""}  });
            foreach (DataRow row in tbl.Rows)
            {
                Reinsurance = new bl_reinsurance();
                Reinsurance.NO =Convert.ToInt32(row["no"].ToString());
                Reinsurance.CustomerID = row["customer_id"].ToString();
                Reinsurance.PolicyID = row["policy_id"].ToString();
                Reinsurance.PolicyNumber = row["policy_number"].ToString();
                Reinsurance.InsuredNameKH = row["customer_kh"].ToString();
                Reinsurance.InsuredNameEN = row["customer_en"].ToString();
                Reinsurance.BirthDate =Convert.ToDateTime( row["Birth_date"].ToString());
                Reinsurance.Gender = row["gender"].ToString();
                Reinsurance.AgeInsure = Convert.ToInt32(row["age_insure"].ToString());
                Reinsurance.CurrentAge = Convert.ToInt32(row["current_age"].ToString());
                Reinsurance.ProductID = row["product_id"].ToString();
                Reinsurance.ProductName = row["product_name"].ToString();
                Reinsurance.CoveragePeriod = Convert.ToInt32(row["coverageperiod"].ToString());
                Reinsurance.PaymentPeriod = Convert.ToInt32(row["paymentperiod"].ToString());
                Reinsurance.PlanCode = row["plan_code"].ToString();
                Reinsurance.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                Reinsurance.IssuedDate = Convert.ToDateTime(row["issue_date"].ToString());
                Reinsurance.PolicyYear = Convert.ToInt32(row["policyyear"].ToString());
                Reinsurance.TimeOfPayment = Convert.ToInt32(row["policy_year_payment"].ToString());
                Reinsurance.Status=row["status"].ToString();
                Reinsurance.EMPercent = Convert.ToInt32(row["em_percent"].ToString());
                Reinsurance.TotalEMPercent = Convert.ToInt32(row["total_em_percent"].ToString());
                Reinsurance.EMPercentVarian = Convert.ToInt32(row["em_varian"].ToString());
                Reinsurance.SumInsure = Convert.ToDouble(row["sum_insure"].ToString());
                Reinsurance.TotalSumInsure = Convert.ToDouble(row["total_sum_insure"].ToString());
                Reinsurance.Retention = Convert.ToDouble(row["retention"].ToString());
                //auto suminsured
                //max auto = 500000USD this is a fixed value only for product type ordinay
                double auto_suminsured = 0;
                double max_auto = 500000;
                double faculative = 0;
                double new_auto =0;
                int product_type_id = 0;

                auto_suminsured = Convert.ToDouble(row["automatic_sum_insure"].ToString());
                product_type_id = Convert.ToInt32(row["product_type_id"].ToString());

                if (product_type_id == 1 || product_type_id == 3)//type ordinary
                {
                    if (auto_suminsured > max_auto)
                    {
                        new_auto = max_auto;
                        faculative = auto_suminsured - max_auto;
                    }
                    else
                    {
                        new_auto = auto_suminsured;
                        faculative = 0;
                    }
                }
                else
                {
                    new_auto = auto_suminsured;
                    faculative = 0;
                }
                

                //Reinsurance.AutomaticSumInsure = Convert.ToDouble(row["automatic_sum_insure"].ToString());
                //Reinsurance.Faculative = 0;
                Reinsurance.AutomaticSumInsure = new_auto;
                Reinsurance.Faculative = faculative;

                Reinsurance.SumInsureVarian = Convert.ToDouble(row["sum_insure_varian"].ToString());
                Reinsurance.ProductType = row["product_type"].ToString();
                Reinsurance.ProductTypeID = Convert.ToInt32(row["product_type_id"].ToString());
                Reinsurance.PayMode = row["pay_mode"].ToString();
                Reinsurance.Remarks = row["remarks"].ToString();
                Reinsurance.Others = "";
                

                ReinsuranceList.Add(Reinsurance);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getReinsuranceRecords] in class [da_reinsurance], Detail: " + ex.Message);
        }

        return ReinsuranceList;
    }

    public static bool Insert(bl_reinsurance reinsured)
    {
        bool status = false;
        try
        {
            string [,]param = new string[,]{
                {"@Customer_ID", reinsured.CustomerID},
                {"@Policy_ID", reinsured.PolicyID},
                {"@Policy_Number", reinsured.PolicyNumber},
                {"@Customer_KH", reinsured.InsuredNameKH},
                {"@Customer_EN", reinsured.InsuredNameEN},
                {"@Gender", reinsured.Gender},
                {"@Birth_Date", reinsured.BirthDate+""},
                {"@Age", reinsured.AgeInsure+""},
                {"@Current_Age",reinsured.CurrentAge+""},
                {"@Product_ID", reinsured.ProductID},
                {"@Product_Name", reinsured.ProductName},
                {"@CoveragePeriod", reinsured.CoveragePeriod+""},
                {"@PaymentPeriod", reinsured.PaymentPeriod+""},
                {"@Plan_Code", reinsured.PlanCode},
                {"@Effective_Date", reinsured.EffectiveDate+""},
                {"@Issued_Date", reinsured.IssuedDate+""},
                {"@Policy_Year", reinsured.PolicyYear+""},
                {"@Policy_Year_Payment", reinsured.TimeOfPayment+""},
                {"@Status", reinsured.Status},
                {"@Sum_Insured", reinsured.SumInsure+""},
                {"@Total_Sum_Insured", reinsured.TotalEMPercent+""},
                {"@Retention", reinsured.Retention+""},
                {"@Auto_Sum_Insured", reinsured.AutomaticSumInsure+""},
                {"@Faculative_Sum_Insured", reinsured.Faculative+""},
                {"@Sum_Insured_Varian", reinsured.SumInsureVarian+""},
                {"@EM_Percent", reinsured.EMPercent+""},
                {"@Total_EM_Percent", reinsured.TotalEMPercent+""},
                {"@EM_Varian", reinsured.EMPercentVarian+""},
                {"@Product_Type_ID", reinsured.ProductTypeID+""},
                {"@Product_Type", reinsured.ProductType},
                {"@Pay_Mode", reinsured.PayMode},
                {"@Created_By", reinsured.Created_By},
                {"@Created_On", reinsured.Created_On+""},
                {"@Remarks", reinsured.Remarks},
                {"@Others", reinsured.Others}
            
            };

            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_REINSURED", param, "da_reinsurance => Insert");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Insert] in class [da_reinsurance], Detail: " + ex.Message);
            status = false;
        }
        return status;

    }
}