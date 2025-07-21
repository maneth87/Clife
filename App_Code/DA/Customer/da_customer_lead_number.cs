using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_customer_lead_number
/// </summary>
public class da_customer_lead_number
{
     private static bool _SUCCESS = false;
    private static string _MESSAGE = "";
    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
	public da_customer_lead_number()
	{
		//
		// TODO: Add constructor logic here
		//
	}
   

      public static bl_customer_lead_prefix.bl_customer_lead_number GetLastCustomerLeadNumber()
      {
          bl_customer_lead_prefix.bl_customer_lead_number lead = new bl_customer_lead_prefix.bl_customer_lead_number();
          DB db = new DB();
          try
          {

              DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_CUSTOMER_LEAD_NUMBER_GET_LAST_NUMBER", new string[,]{
            
        }, "da_customer_lead_number => GetLastCustomerLeadNumber()");
              if (db.RowEffect == -1)//error get data
              {
                  _SUCCESS = false;
                  _MESSAGE = db.Message;
              }
              else
              {
                  if (tbl.Rows.Count > 0)
                  {
                      var row = tbl.Rows[0];
                      lead = new bl_customer_lead_prefix.bl_customer_lead_number()
                      {
                          LeadNumberId = row["LEAD_NUMBER_ID"].ToString(),
                          LeadNumber = Convert.ToInt32( row["LEAD_NUMBER"].ToString()),
                          LeadNumberVar = row["LEAD_NUMBER_VAR"].ToString()
                      };

                      _SUCCESS = true;
                      _MESSAGE = "Success";
                  }
                  else
                  {
                      _SUCCESS = true;
                      _MESSAGE = "No records found.";
                  }
              }
          }
          catch (Exception ex)
          {
              _MESSAGE = ex.Message;

              lead = new bl_customer_lead_prefix.bl_customer_lead_number();
              Log.AddExceptionToLog("Error function [bl_customer_lead_prefix.bl_customer_lead_number GetLastCustomerLeadNumber()] in class [da_customer_lead_number], detail: " + ex.Message);

          }
          return lead;
      }

      public static bool Save(bl_customer_lead_prefix.bl_customer_lead_number leadNumber)
      {
      
          try
          {
              DB db = new DB();
              _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_CUSTOMER_LEAD_NUMBER_INSERT", new string[,] {
            {"@LEAD_NUMBER_ID",leadNumber.LeadNumberId},
            {"@LEAD_NUMBER", leadNumber.LeadNumber+""},
              {"@LEAD_NUMBER_VAR",leadNumber.LeadNumberVar},
                  {"@CREATED_BY",leadNumber.CreatedBy},
                  {"@CREATED_ON",leadNumber.CreatedOn+""},
                  {"@REMARKS",leadNumber.Remarks}
            }, "da_customer_lead_number ==> Save(bl_customer_lead_prefix.bl_customer_lead_number leadNumber)");

              _MESSAGE = db.Message;
          }
          catch (Exception ex)
          {
              _MESSAGE = ex.Message;
              _SUCCESS = false;
              Log.AddExceptionToLog("Error function [Save(bl_customer_lead_prefix.bl_customer_lead_number leadNumber)] in class [da_customer_lead_number], detail: " + ex.Message );
          }
       
          return _SUCCESS;
      }
      public static bool Delete(string id)
      {

          try
          {
              DB db = new DB();
              _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_CUSTOMER_LEAD_NUMBER_DELETE", new string[,] {
            {"@LEAD_NUMBER_ID",id}
            }, "da_customer_lead_number ==> Delete(string id)");

              _MESSAGE = db.Message;
          }
          catch (Exception ex)
          {
              _MESSAGE = ex.Message;
              _SUCCESS = false;
              Log.AddExceptionToLog("Error function [Delete(string id)] in class [da_customer_lead_number], detail: " + ex.Message );
          }

          return _SUCCESS;
      }
}