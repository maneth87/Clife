using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_sale_agent_cancel
/// </summary>
public class da_sale_agent_cancel
{
    private static da_sale_agent_cancel mytitle = null;
    public da_sale_agent_cancel()
	{
	  if (mytitle == null)
        {
            mytitle = new da_sale_agent_cancel();
        }
	}

    /// <summary>
    /// Insert into Ct_Sale_Agent_Cancel
    /// </summary>
    public static bool InsertSale_Agent_Cancel(bl_sale_agent_cancel sale_agent_cancel)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Sale_Agent_Cancel";

            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_cancel.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Created_On", sale_agent_cancel.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", sale_agent_cancel.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", sale_agent_cancel.Created_Note);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertSale_Agent_Cancel] in class [bl_sale_agent_cancel]. Details: " + ex.Message);
            }
        }
        return result;
    }
}