using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class da_official_receipt_prem_pay
    {

        /// <summary>
        /// Insert into Ct_Official_Receipt_Prem_Pay
        /// </summary>
        public static bool Insert_Official_Receipt_Prem_Pay(bl_official_receipt_prem_pay official_receipt_prem_pay)
        {
            bool result = false;

            try
            {
               
                string sql = @"Insert into Ct_Official_Receipt_Prem_Pay (Official_Receipt_Prem_Pay_ID,Official_Receipt_ID,Policy_Prem_Pay_ID,Product_ID,Sum_Insure,Amount,Created_By,Created_On,Payment_Type_ID)
                                Values(@Official_Receipt_Prem_Pay_ID,@Official_Receipt_ID,@Policy_Prem_Pay_ID,@Product_ID,@Sum_Insure,@Amount,@Created_By,@Created_On,@Payment_Type_ID)";


                result = Helper.ExecuteCommand(AppConfiguration.GetConnectionString(), sql, new string[,]{

                {"@Official_Receipt_Prem_Pay_ID", official_receipt_prem_pay.Official_Receipt_Prem_Pay_ID},
                {"@Official_Receipt_ID", official_receipt_prem_pay.Official_Receipt_ID},
                {"@Policy_Prem_Pay_ID", official_receipt_prem_pay.Policy_Prem_Pay_ID},
                {"@Product_ID", official_receipt_prem_pay.Product_ID},
                {"@Sum_Insure", official_receipt_prem_pay.Sum_Insured+""},
                {"@Amount", official_receipt_prem_pay.Amount+""},
                {"@Created_On", official_receipt_prem_pay.Created_On+""},
                {"@Created_By", official_receipt_prem_pay.Created_By},
                {"@Payment_Type_ID", official_receipt_prem_pay.Payment_Type_ID+""}
            }, "function [Insert_Official_Receipt_Prem_Pay] in class [da_official_receipt_prem_pay]");
            }
            catch (Exception ex)
            {
                result = false;
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Insert_Official_Receipt_Prem_Pay] in class [da_official_receipt_prem_pay]. Details: " + ex.Message);
            }

            return result;
        }
    
    }

