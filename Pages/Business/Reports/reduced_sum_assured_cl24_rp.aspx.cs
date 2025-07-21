using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
public partial class Pages_Business_Reports_reduced_sum_assured_cl24_rp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DataTable tblReducedSumAssured = new DataTable("tbl_reduced_sum_assured_cl24");
            ReportViewer rpt;
            ReportDataSource rptDatasource;
            string productName = "";
            int coverageYear = 0;
            string productId = "";
            double reducedSumAssured = 0;
            double sumAssured = 0;
            double rate = 0;
          
            List<bl_reduced_sum_assured> rateList = new List<bl_reduced_sum_assured>();
            string policy_id = Request.QueryString["policy_id"];
            string policyNumber = "";

            var col = tblReducedSumAssured.Columns;
            col.Add("year");
            col.Add("month");
            col.Add("reduced_sum_assured");
            col.Add("assured_year");
            col.Add("pay_year");
            col.Add("policy_number");
            col.Add("product_id");
            
          List<bl_group_master_policy> gMasterList=  da_group_master_policy.GetGroupMasterPolicyList(policy_id, "");
          if (gMasterList.Count > 0)//product is credit life 24
          {
              bl_policy_detail policyDetail = da_policy.GetPolicyDetailSplit(policy_id);
              sumAssured = policyDetail.System_Sum_Insure;
              productId = policyDetail.Product_ID;
              coverageYear = policyDetail.Assure_Year;

              //format group policy number [group code + sequence number]
              string groupCode = "";
              //get group code
              foreach (bl_group_master_product obj in da_group_master_product.GetGroupMasterProductList(productId,"").Where(A => A.GroupMasterID == gMasterList[0].GroupMasterID))
              {
                  groupCode = obj.GroupCode;
                  break;
              }
              policyNumber = groupCode + "-" + gMasterList[0].SeqNumber;

              rateList = da_reduced_sum_assured.GetTableRate(productId, coverageYear);
              
              //loop assured year
              DataRow row ;
              for (int i = 1; i <= coverageYear; i++)
              {
                  //loop month
                  for (int month = 1; month <= 12; month++)
                  {
                      //get rate from list using linq filter by month and year
                      foreach (bl_reduced_sum_assured obj in rateList.Where(__ => __.Month == month && __.Year == i))
                      {
                          rate = obj.Rate;
                          break;
                      }
                      reducedSumAssured = (sumAssured * rate) / 1000;
                      row = tblReducedSumAssured.NewRow();
                      row["year"] = i;
                      row["month"] = month;
                      row["reduced_sum_assured"] = reducedSumAssured.ToString("#,0;(#,0)");
                      row["assured_year"] = coverageYear;
                      row["pay_year"] = policyDetail.Pay_Year;
                      row["policy_number"] = policyNumber;
                      row["product_id"] = da_product.GetProductByProductID(productId).En_Title;//get product name

                      tblReducedSumAssured.Rows.Add(row);
                  }
              }

              int rowCount = tblReducedSumAssured.Rows.Count;
              if (rowCount > 0)
              {
                  //add blank rows
                  int newAssuredYear = policyDetail.Assure_Year + 1;
                  for (int year = newAssuredYear; year <= 20; year++)
                  {
                      //loop months
                      for (int i = 1; i <= 12; i++)
                      {
                          row = tblReducedSumAssured.NewRow();
                          row["year"] = year;
                          row["month"] = i;
                          row["reduced_sum_assured"] = "-";
                          row["assured_year"] = coverageYear;
                          row["pay_year"] = policyDetail.Pay_Year;
                          row["policy_number"] = policyNumber;
                          row["product_id"] = da_product.GetProductByProductID(productId).En_Title;//get product name

                          tblReducedSumAssured.Rows.Add(row);
                      }
                  }
                  
                  rptDatasource = new ReportDataSource("ds_reduced_sum_assured_cl24", tblReducedSumAssured);

                  productName = da_product.GetProductByProductID(productId).En_Title;
                  //combind product name with assured and pay year to dispay in report header
                  //productName=productName + " (" + policyDetail.Assure_Year + "/" + policyDetail.Pay_Year + ")";
                  //set fix assure and pay year requested by Mr.Prim somoy @04May2021
                  productName = productName + " (1/1)";
                  //CL24
                  ReportParameter[] paras = new ReportParameter[] {
                    new ReportParameter("ProductName", productName ),
                    new ReportParameter("cover_period", "Coverage Period 1 Year and Auto Renewal" )
                  };
                  rpt= new ReportViewer();
                  rpt.LocalReport.DataSources.Clear();
                  rpt.LocalReport.DataSources.Add(rptDatasource);
                  rpt.LocalReport.ReportPath = Server.MapPath("reduced_sum_assured_cl24_rr2.rdlc");
                  rpt.LocalReport.SetParameters(paras);
                  rpt.LocalReport.Refresh();
                  //generate report in pdf
                  Report_Generator.ExportToPDF(this.Context, rpt, "reduced_sum_assured_cl24" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
              }
          }
          else
          {
              message.InnerText = "ProductID [" + productId +"] is not support." ;
          }
        }
        catch (Exception ex)
        {
            message.InnerText = "Load report error.";

            Log.AddExceptionToLog("Error function [page load] in page [], Detail: " + ex.InnerException + "==>"+ ex.Message + "==>" + ex.StackTrace);
        }
    }
}