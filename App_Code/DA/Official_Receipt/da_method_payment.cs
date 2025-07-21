using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


    public static class da_method_payment
    {
        /// <summary>
        /// Insert into Ct_Method_Payment
        /// </summary>
        public static bool Insert_Method_Payment(bl_method_payment method_payment)
        {
            bool result = false;

            try
            {

                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_METHOD_PAYMENT", new string[,] { 
               {"@Method_ID", method_payment.Method_ID+""},
               {"@Method_Name", method_payment.Method_Name},
               {"@Official_Receipt_ID", method_payment.Official_Receipt_ID},
               {"@Transaction_ID", method_payment.Transaction_ID},
               {"@Created_By", method_payment.Created_By},
               {"@Created_On", method_payment.Created_On+""},
               {"@Created_Note", method_payment.Created_Note}
                }, "");


            }
            catch (Exception ex)
            {
                result = false;
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Insert_Method_Payment(bl_method_payment method_payment)] in class [da_method_payment]. Details: " + ex.Message);
            }

            return result;
        }

    }

