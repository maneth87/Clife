using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da
/// </summary>
public class da_report_certificate_policy
{
    private static da_report_certificate_policy mytitle = null;
	public da_report_certificate_policy()
	{
		if (mytitle == null)
        {
            mytitle = new da_report_certificate_policy();
        }
	}

    public static DataTable Get_Benefitciary()
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = @"select Full_Name,Relationship, 
                            cast(Percentage as nvarchar(10)) +'%' Percentage from [dbo].[Ct_App_Benefit_Item]
                            where App_Register_ID='0109A6ED-D750-47F0-8CC0-873DCA911FCF' ";

            cmd.CommandText = sql;
            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Get_Benefitciary] in class []. Details: " + ex.Message);
            }
        }
        return dt;
    }
    

}