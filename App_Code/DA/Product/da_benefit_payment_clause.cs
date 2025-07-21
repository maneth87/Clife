using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_benefit_payment_clause
/// </summary>
public class da_benefit_payment_clause
{
    private static da_benefit_payment_clause mytitle = null;
    public da_benefit_payment_clause()
    {
        if (mytitle == null)
        {
            mytitle = new da_benefit_payment_clause();
        }

    }

    #region "Public Functions"

    //Insert Ct_Benefit_Payment_Clause
    public static string InsertBenefitPaymentClause(bl_benefit_payment_clause benefit_payment_clause)
    {
        string id = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Benefit_Payment_Clause";

            cmd.Parameters.AddWithValue("@Benefit_Payment_Clause_ID", benefit_payment_clause.Benefit_Payment_Clause_ID);
            cmd.Parameters.AddWithValue("@Product_ID", benefit_payment_clause.Product_ID);
            cmd.Parameters.AddWithValue("@Benefit_Clause", benefit_payment_clause.Benefit_Clasue);         

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                id = benefit_payment_clause.Benefit_Payment_Clause_ID;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [InsertBenefitPaymentClause] in class [da_benefit_payment_clause]. Details: " + ex.Message);
            }
        }
        return id;
    }

    //Function to get Ct_Benefit_Payment_Clause by Product_ID
    public static bl_benefit_payment_clause GetBenefitPaymentClauseByProductID(string product_id)
    {

        bl_benefit_payment_clause benefit_payment_clause = new bl_benefit_payment_clause();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Benefit_Payment_Clause", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@Product_ID";
            paramName.Value = product_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    benefit_payment_clause.Benefit_Payment_Clause_ID = rdr.GetString(rdr.GetOrdinal("Benefit_Payment_Clause_ID"));
                    benefit_payment_clause.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    benefit_payment_clause.Benefit_Clasue = rdr.GetString(rdr.GetOrdinal("Benefit_Clause"));
                }

            }
            con.Close();
        }
        return benefit_payment_clause;
    }
    //Update Ct_Benefit_Payment_Clause
    public static bool UpdateBenefitPaymentClause(bl_benefit_payment_clause benefit_payment_clause)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Benefit_Payment_Clause";

            cmd.Parameters.AddWithValue("@Benefit_Payment_Clause_ID", benefit_payment_clause.Benefit_Payment_Clause_ID);
            cmd.Parameters.AddWithValue("@Product_ID", benefit_payment_clause.Product_ID);
            cmd.Parameters.AddWithValue("@Benefit_Clause", benefit_payment_clause.Benefit_Clasue);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateBenefitPaymentClause] in class [da_benefit_payment_clause]. Details: " + ex.Message);
            }
        }
        return result;
    }

  
    #endregion
}