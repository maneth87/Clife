using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_payment_mode
/// </summary>
public class da_payment_mode
{
	private static da_payment_mode mytitle = null;
    public da_payment_mode()
    {
        if (mytitle == null)
        {
            mytitle = new da_payment_mode();
        }

    }

    #region "Public Functions"
    //Function to get payment mode by product_id
    public static bl_payment_mode GetPaymentModeByPayModeID(int pay_mode_ID)
    {
        bl_payment_mode payment_mode = new bl_payment_mode();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Payment_Mode_By_Pay_Mode_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@PayModeID";
            paramName.Value = pay_mode_ID;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    payment_mode.Pay_Mode_ID = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode_ID"));
                    payment_mode.Mode = rdr.GetString(rdr.GetOrdinal("Mode"));
                    payment_mode.Factor = rdr.GetDouble(rdr.GetOrdinal("Factor"));
                  

                }

            }

        }
        return payment_mode;
    }
    #endregion

    #region  Added by maneth @02-Feb-24
    public static List< bl_payment_mode> GetPaymentModeList()
    {
        List< bl_payment_mode> modList = new List<bl_payment_mode>();

        try
        {
            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_PAYMENT_MODE_GET_LIST", new string[,] {
           
            }, "da_payment_mode=>GetPaymentModeList()");
            foreach (DataRow row in tbl.Rows)
            {
                modList.Add(new bl_payment_mode() {
                 Pay_Mode_ID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                 Mode=row["mode"].ToString(),
                 Factor= Convert.ToDouble(row["factor"].ToString())
                });
            }
        }
        catch (Exception ex)
        {
            modList = new List<bl_payment_mode>();
            Log.AddExceptionToLog("Error function [GetPaymentModeList()] in class [da_payment_mode], detail: " + ex.Message + "=>" + ex.StackTrace);
        }
        return modList;
        
    }
    #endregion
}