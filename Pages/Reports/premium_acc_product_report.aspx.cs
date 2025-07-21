using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Data;


public partial class Pages_Reports_acc_product_premium_report : System.Web.UI.Page
{
    //First Page Load Event    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Default Table Detail
            dvReportDetail.Style.Clear();
            dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));
            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }
    }       

    //Generate Product Premium Report
    protected void ReportProductPremium()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        DateTime from_date = DateTime.Parse(txtFrom_date.Text, dtfi);
        DateTime to_date = DateTime.Parse(txtTo_date.Text, dtfi3);

        TimeSpan start_time = new TimeSpan(00, 00, 00); TimeSpan end_time = new TimeSpan(23, 59, 59);
        from_date = from_date.Date + start_time; to_date = to_date.Date + end_time;
              

        //Get list of product premium details
        #region
            //Premium List from Individual Prem Pay
            List<bl_product_premium_report> product_premium_list = new List<bl_product_premium_report>();

            product_premium_list = da_product_premium_report.GetACCProductPremiumDetailsByDates(from_date, to_date);

            //Premium List from Group Prem Pay
            List<bl_product_premium_report> group_prem_pay_list = new List<bl_product_premium_report>();

            group_prem_pay_list = da_product_premium_report.GetACCGTLIPremiumDetailsFromPremPayByDates(from_date, to_date);

            //Premium List from Group Prem Return
            List<bl_product_premium_report> group_prem_return_list = new List<bl_product_premium_report>();

            group_prem_return_list = da_product_premium_report.GetACCGTLIPremiumDetailsFromPremReturnByDates(from_date, to_date);

        #endregion

        //Declare total premium by group of products variables
        #region

            double total_amount_paid = 0;
        
            double total_term_life = 0; // FT013, T10, T10002, T1011, T3, T3002, T5, T5002, 
            double total_endowment = 0; // PP15/10, PP200
            double total_whole_life = 0;// W10, W15, W20, W9010, W9015, W9020
            double total_mrta = 0; // MRTA, MRTA12, MRTA24, MRTA36
            double total_gtli = 0; //GTLI (Group Term Life)

            double total_term_life_policy = 0;
            double total_endowment_policy = 0;
            double total_whole_life_policy = 0;           
            double total_mrta_policy = 0;
            double total_gtli_policy = 0;

            double total_term_life_fyp = 0;
            double total_endowment_fyp = 0;
            double total_whole_life_fyp = 0;
            double total_mrta_fyp = 0;
            double total_gtli_fyp = 0;

            double total_term_life_ryp = 0;
            double total_endowment_ryp = 0;
            double total_whole_life_ryp = 0;
            double total_mrta_ryp = 0;
            double total_gtli_ryp = 0;

            double total_term_life_sum_insure = 0;
            double total_endowment_sum_insure = 0;
            double total_whole_life_sum_insure = 0;
            double total_mrta_sum_insure = 0;
            double total_gtli_sum_insure = 0;

        #endregion

        //Draw Header
        #region

            if (product_premium_list.Count > 0)
            {
                lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Premium Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

                lblfrom1.Text = txtFrom_date.Text;
                lblto1.Text = txtTo_date.Text;
            }
            else
            {
                lblfrom.Text = "";
                lblfrom1.Text = "";
                lblto1.Text = "";
            }

            string strTable = "";

            //Draw Header
            if (product_premium_list.Count > 0)
            {

                strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
                strTable += "<tr border=\"1\"><th styple=\"text-align: center; width:90px; \">No</th><th styple=\"text-align: center; \">Receipt Number</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">Effective Date</th><th style=\"text-align: center;\">Pay Date</th><th styple=\"text-align: center; \">Product</th><th style=\"text-align: center;\">Mode</th><th styple=\"text-align: center; \">Year/Time</th><th styple=\"text-align: center; \">SI</th><th style=\"text-align: center;\">Amount Paid</th></tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";
            }
            else
            {
                strTable = "";
            }


        #endregion

            int row_no = 0;

        //Loop through product premium list (Individual)
        #region
            for (int i = 0; i < product_premium_list.Count; i++)
            {
                //Get obj product premium of this index[i]
                bl_product_premium_report product_premium = new bl_product_premium_report();
                product_premium = product_premium_list[i];

                string year_time = "";               
                double prem_pay_sum_insure = 0;

                //Get total from individual policies
                #region

                    total_amount_paid += product_premium.Amount_Paid;

                    //Get Prem Pay Details by Official Receipt Number
                    List<bl_prem_pay_details> prem_pay_list = new List<bl_prem_pay_details>();

                    prem_pay_list = da_product_premium_report.GetACCProductPremiumPayDetailsByDates(product_premium.OR_No, product_premium.Policy_Number);

                     
                        year_time = da_product_premium_report.GetPremYearAndPremLot(product_premium.Policy_ID, product_premium.OR_No);

                        if (year_time =="") { year_time = "1st Year"; }         
                            
                    switch(product_premium.Product_ID)
                    { 
                        case "FT013": //Flexi Term
                        case "T10": //Term 10 Old
                        case "T10002": //Term 10 New
                        case "T1011": //Micro
                        case "T3": //Term 3 Old
                        case "T3002": //Term 3 New
                        case "T5": //Term 5 Old
                        case "T5002": //Term 5 New

                            total_term_life += product_premium.Amount_Paid;

                            if (prem_pay_list.Count > 1)
                            {
                                for (int pp = 0; pp < prem_pay_list.Count; pp++)
                                {
                                    bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                    prem_pay_details = prem_pay_list[pp];

                                    prem_pay_sum_insure = prem_pay_details.Sum_Insure;                                                                        
                                                                       
                                    //Total Term Life policy
                                    if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                    {
                                        total_term_life_policy += 1;
                                        total_term_life_sum_insure += prem_pay_details.Sum_Insure;
                                    }

                                    //Total Term Life FYP
                                    if (prem_pay_details.Prem_Year == 1)
                                    {
                                        total_term_life_fyp += prem_pay_details.Amount;
                                    }
                                    else //Total Term Life RYP
                                    {
                                        total_term_life_ryp += prem_pay_details.Amount;
                                    }
                                }

                                product_premium.Sum_Insure = prem_pay_sum_insure;
                            }
                            else if(prem_pay_list.Count ==1)//case count == 1
                            {
                                bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                prem_pay_details = prem_pay_list[0];
                               
                                product_premium.Sum_Insure = prem_pay_details.Sum_Insure;                                                             

                                //Total Term Life policy
                                if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                {
                                    total_term_life_policy += 1;
                                    total_term_life_sum_insure += prem_pay_details.Sum_Insure;
                                }

                                //Total Term Life FYP
                                if (prem_pay_details.Prem_Year == 1)
                                {
                                    total_term_life_fyp += prem_pay_details.Amount;
                                }
                                else //Total Term Life RYP
                                {
                                    total_term_life_ryp += prem_pay_details.Amount;
                                }
                            }
                                                       
                          
                            break;
                        case "PP15/10": 
                        case "PP200":
                            total_endowment += product_premium.Amount_Paid;

                            if (prem_pay_list.Count > 1)
                            {
                                for (int pp = 0; pp < prem_pay_list.Count; pp++)
                                {
                                    bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                    prem_pay_details = prem_pay_list[pp];

                                    prem_pay_sum_insure = prem_pay_details.Sum_Insure;

                                    //Total Endowment policy
                                    if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                    {
                                        total_endowment_policy += 1;
                                        total_endowment_sum_insure += prem_pay_details.Sum_Insure;
                                    }

                                    //Total Endowment FYP
                                    if (prem_pay_details.Prem_Year == 1)
                                    {
                                        total_endowment_fyp += prem_pay_details.Amount;
                                    }
                                    else //Total Endowment RYP
                                    {
                                        total_endowment_ryp += prem_pay_details.Amount;
                                    }
                                }

                                product_premium.Sum_Insure = prem_pay_sum_insure;

                            }
                            else if(prem_pay_list.Count ==1) //case count == 1
                            {
                                bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                prem_pay_details = prem_pay_list[0];

                                product_premium.Sum_Insure = prem_pay_details.Sum_Insure;

                                //Total Endowment policy
                                if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                {
                                    total_endowment_policy += 1;
                                    total_endowment_sum_insure += prem_pay_details.Sum_Insure;
                                }

                                //Total Endowment FYP
                                if (prem_pay_details.Prem_Year == 1)
                                {
                                    total_endowment_fyp += prem_pay_details.Amount;
                                }
                                else //Total Endowment RYP
                                {
                                    total_endowment_ryp += prem_pay_details.Amount;
                                }
                            }                                                  

                            break;
                        case "W10": //Whole Life 10 Old
                        case "W15": //Whole Life 15 Old 
                        case "W20": //Whole Life 20 Old
                        case "W9010": //Whole Life 10 New
                        case "W9015": //Whole Life 15 New
                        case "W9020": //Whole Life 20 New
                            total_whole_life += product_premium.Amount_Paid;

                            if (prem_pay_list.Count > 1)
                            {
                                for (int pp = 0; pp < prem_pay_list.Count; pp++)
                                {
                                    bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                    prem_pay_details = prem_pay_list[pp];

                                    prem_pay_sum_insure = prem_pay_details.Sum_Insure;

                                    //Total Whole Life policy
                                    if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                    {
                                        total_whole_life_policy += 1;
                                        total_whole_life_sum_insure += prem_pay_details.Sum_Insure;
                                    }

                                    //Total Whole Life FYP
                                    if (prem_pay_details.Prem_Year == 1)
                                    {
                                        total_whole_life_fyp += prem_pay_details.Amount;
                                    }
                                    else //Total Whole Life RYP
                                    {
                                        total_whole_life_ryp += prem_pay_details.Amount;
                                    }
                                }

                                product_premium.Sum_Insure = prem_pay_sum_insure;
                            }
                            else if(prem_pay_list.Count ==1) //case count == 1
                            {
                                bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                prem_pay_details = prem_pay_list[0];

                                product_premium.Sum_Insure = prem_pay_details.Sum_Insure;

                                //Total Whole Life policy
                                if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                {
                                    total_whole_life_policy += 1;
                                    total_whole_life_sum_insure += prem_pay_details.Sum_Insure;
                                }

                                //Total Whole Life FYP
                                if (prem_pay_details.Prem_Year == 1)
                                {
                                    total_whole_life_fyp += prem_pay_details.Amount;
                                }
                                else //Total Whole Life RYP
                                {
                                    total_whole_life_ryp += prem_pay_details.Amount;
                                }

                            }
                         

                            break;
                        case "MRTA": // MRTA Old
                        case "MRTA12": //MRTA12
                        case "MRTA24": //MRTA24
                        case "MRTA36": //MRTA36
                            total_mrta += product_premium.Amount_Paid;

                            if (prem_pay_list.Count > 1)
                            {
                                for (int pp = 0; pp < prem_pay_list.Count; pp++)
                                {
                                    bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                    prem_pay_details = prem_pay_list[pp];

                                    prem_pay_sum_insure = prem_pay_details.Sum_Insure;

                                    //Total MRTA policy
                                    if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                    {
                                        total_mrta_policy += 1;
                                        total_mrta_sum_insure += prem_pay_details.Sum_Insure;
                                    }

                                    //Total MRTA FYP
                                    if (product_premium.Prem_Year == 1)
                                    {
                                        total_mrta_fyp += prem_pay_details.Amount;
                                    }
                                    else //Total MRTA RYP
                                    {
                                        total_mrta_ryp += prem_pay_details.Amount;
                                    }

                                }

                                product_premium.Sum_Insure = prem_pay_sum_insure;

                            }
                            else if(prem_pay_list.Count ==1) //case count == 1
                            {
                                bl_prem_pay_details prem_pay_details = new bl_prem_pay_details();
                                prem_pay_details = prem_pay_list[0];

                                product_premium.Sum_Insure = prem_pay_details.Sum_Insure;

                                //Total MRTA policy
                                if (prem_pay_details.Prem_Year == 1 && prem_pay_details.Prem_Lot == 1)
                                {
                                    total_mrta_policy += 1;
                                    total_mrta_sum_insure += prem_pay_details.Sum_Insure;
                                }

                                //Total MRTA FYP
                                if (product_premium.Prem_Year == 1)
                                {
                                    total_mrta_fyp += prem_pay_details.Amount;
                                }
                                else //Total MRTA RYP
                                {
                                    total_mrta_ryp += prem_pay_details.Amount;
                                }
                            }
                          
                            break;
                    }                 
                                      

                    #endregion

                //Draw Row Indiviudal
                #region

                    if (product_premium_list.Count > 0)
                    {
                        strTable += "<tr>";
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (i + 1) + "</td>";

                        //Official Receipt Number Column
                        strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.OR_No + "</td>";
                                             
                        //Policy Number Column
                        strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Policy_Number + "</td>";

                        //Effective Date Column
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                        //Pay Date Column
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Pay_Date.ToString("d-MMM-yyyy") + "</td>";


                        //Product Column
                        strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Product + "</td>";
                        

                        //Pay Mode Column
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Pay_Mode + "</td>";

                        //Year/Time Column
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + year_time + "</td>";
                                                
                        //Sum Insure Column
                        if (product_premium.Sum_Insure == 0)
                        {
                            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                        }
                        else
                        {
                            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Sum_Insure.ToString("C0") + "</td>";
                        }

                        //Amount Paid Column
                        if(product_premium.Amount_Paid == 0)
                        {
                            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                        }
                        else
                        {
                            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >" + product_premium.Amount_Paid.ToString("C0") + "</td>";
                        }
                                              
                        
                        strTable += "</tr>";

                        dvReportDetail.Controls.Add(new LiteralControl(strTable));

                        strTable = "";
                    }

                #endregion

                row_no = i + 1; 
            }//End loop product premium list
        #endregion

        //Loop through group prem pay list (GTLI)
        #region
            for (int j = 0; j < group_prem_pay_list.Count; j++)
            {
                row_no += 1;

                //Get obj product prem pay of this index[j]
                bl_product_premium_report group_prem_pay = new bl_product_premium_report();
                group_prem_pay = group_prem_pay_list[j];

                string official_receipt_number_from_prem_pay = da_product_premium_report.GetOfficialReceiptNumberByGTLIPolicyPremPayID(group_prem_pay.Prem_Pay_ID);

                //Get total from group prem pay policies
                #region

                    total_amount_paid += group_prem_pay.Amount_Paid;
                   
                    total_gtli_sum_insure += group_prem_pay.Sum_Insure;

                    if(group_prem_pay.Transaction_Type == 1){
                        total_gtli_policy += 1;
                    }

                    total_gtli += group_prem_pay.Amount_Paid;

                    total_gtli_fyp += group_prem_pay.Amount_Paid;                

                #endregion

                //Draw Row GTLI Prem Pay
                #region
                     
                    if (group_prem_pay_list.Count > 0)
                    {
                        string strTableGroupPremPay = "<tr>";
                        strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                        //Official Receipt Number Column
                        strTableGroupPremPay += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + official_receipt_number_from_prem_pay + "</td>";

                        //Policy Number Column
                        strTableGroupPremPay += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Policy_Number + "</td>";

                        //Effective Date Column
                        strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                        //Pay Date Column
                        strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Pay_Date.ToString("d-MMM-yyyy") + "</td>";
                        
                        //Product Column
                        strTableGroupPremPay += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Product + "</td>";
                        
                        //Pay Mode Column
                        strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Pay_Mode + "</td>";

                        //Year/Time Column
                        strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + "Year " + group_prem_pay.Prem_Year + "/1" + "</td>";
                                               
                        //Sum Insure Column
                        if (group_prem_pay.Sum_Insure == 0)
                        {
                            strTableGroupPremPay += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                        }
                        else
                        {
                            strTableGroupPremPay += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Sum_Insure.ToString("C0") + "</td>";
                        }

                        //Amount Paid Column
                        if (group_prem_pay.Amount_Paid == 0)
                        {
                            strTableGroupPremPay += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                        }
                        else
                        {
                            strTableGroupPremPay += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >" + group_prem_pay.Amount_Paid.ToString("C0") + "</td>";
                        }


                        strTableGroupPremPay += "</tr>";

                        dvReportDetail.Controls.Add(new LiteralControl(strTableGroupPremPay));

                        strTableGroupPremPay = "";
                    }

                #endregion

            }//End loop group prem pay list
        #endregion

        //Loop through group prem return list (GTLI)
        #region
            for (int k = 0; k < group_prem_return_list.Count; k++)
            {
                row_no += 1;

                //Get obj product prem return of this index[k]
                bl_product_premium_report group_prem_return = new bl_product_premium_report();
                group_prem_return = group_prem_return_list[k];

                string official_receipt_number_from_prem_return = da_product_premium_report.GetOfficialReceiptNumberByGTLIPolicyPremReturnID(group_prem_return.Prem_Return_ID);

                //Get total from group prem return policies
                #region

                    total_amount_paid -= group_prem_return.Amount_Paid;
                 
                    total_gtli_sum_insure -= group_prem_return.Sum_Insure;

                    total_gtli -= group_prem_return.Amount_Paid;

                    total_gtli_fyp -= group_prem_return.Amount_Paid;

                #endregion

                //Draw Row GTLI Prem Return
                #region

                if (group_prem_return_list.Count > 0)
                {
                    string strTableGroupPremReturn = "<tr>";
                    strTableGroupPremReturn += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                    //Official Receipt Number Column
                    strTableGroupPremReturn += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + official_receipt_number_from_prem_return + "</td>";

                    //Policy Number Column
                    strTableGroupPremReturn += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Policy_Number + "</td>";

                    //Effective Date Column
                    strTableGroupPremReturn += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                    //Pay Date Column
                    strTableGroupPremReturn += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Pay_Date.ToString("d-MMM-yyyy") + "</td>";

                    //Product Column
                    strTableGroupPremReturn += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Product + "</td>";

                    //Pay Mode Column
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Pay_Mode + "</td>";

                    //Year/Time Column
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + "Year " + group_prem_return.Prem_Year + "</td>";
                                        
                    //Sum Insure Column
                    if (group_prem_return.Sum_Insure == 0)
                    {
                        strTableGroupPremReturn += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                    }
                    else
                    {
                        strTableGroupPremReturn += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + group_prem_return.Sum_Insure.ToString("C0") + ")</td>";
                    }

                    //Amount Paid Column
                    if (group_prem_return.Amount_Paid == 0)
                    {
                        strTableGroupPremReturn += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTableGroupPremReturn += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >(" + group_prem_return.Amount_Paid.ToString("C0") + ")</td>";
                    }


                    strTableGroupPremReturn += "</tr>";

                    dvReportDetail.Controls.Add(new LiteralControl(strTableGroupPremReturn));

                    strTableGroupPremReturn = "";
                }

                #endregion

            }//End loop group prem return list
        #endregion

        //Draw Total
        #region

            if (product_premium_list.Count > 0)
            {
                strTable += "<tr>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\" >&nbsp;</td>";
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
       
                strTable += "<td  style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >Total:</td>";


                if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure) == 0)
                {
                    
                    strTable += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                       
                }
                else
                {
                    //Negative
                    if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure) < 0)
                    {
                        strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure).ToString("C0") + ")</td>";
                    }
                    else //Positive
                    {
                        strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure).ToString("C0") + "</td>";
                    }
                }

                if (total_amount_paid == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    //Negative
                    if (total_amount_paid < 0)
                    {
                        strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_amount_paid.ToString("C0") + ")</td>";
                    }
                    else //Positive
                    {
                        strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_amount_paid.ToString("C0") + "</td>";
                    }
                }

                
                strTable += "</tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";
            }

        #endregion
               
        //Draw Summary Table Product Premium (Term Life, Endowment, Whole Life, MRTA, Group Term Life)
        #region
            string strTableSummray = "";

            //Draw Header Summary
            #region
                if (product_premium_list.Count > 0)
                {

                    strTableSummray = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
                    strTableSummray += "<tr border=\"1\"><th styple=\"text-align: center; width:90px; \">No</th><th styple=\"text-align: center; \">Product</th><th styple=\"text-align: center; \">No of Policy</th><th styple=\"text-align: center; \">SI</th><th styple=\"text-align: center; \">FYP</th><th styple=\"text-align: center; \">RYP</th><th styple=\"text-align: center; \">Total Amount</th></tr>";

                    dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));

                    strTableSummray = "";
                }
                else
                {
                    strTableSummray = "";
                }
            #endregion

            //Draw Row Summary
            #region
                for(int j = 1; j <=5; j++){
                    if (product_premium_list.Count > 0)
                    {
                        strTableSummray += "<tr>";
                        strTableSummray += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (j) + "</td>";

                        switch (j)
                        {
                            case 1: //Term Life
                                strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Term Life </td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_term_life_policy + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_term_life_sum_insure.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_term_life_fyp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_term_life_ryp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_term_life.ToString("C0") + "</td>";
                                break;
                            case 2: //Endowment
                                strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Endowment </td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_endowment_policy + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_endowment_sum_insure.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" +  total_endowment_fyp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" +  total_endowment_ryp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" +  total_endowment.ToString("C0") + "</td>";
                                break;
                            case 3: //Whole Life
                                strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Whole Life </td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_policy+ "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_sum_insure.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_fyp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" +  total_whole_life_ryp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" +  total_whole_life.ToString("C0") + "</td>";
                                break;
                            case 4: //MRTA
                                strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > MRTA </td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_policy + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_sum_insure.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_fyp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_ryp.ToString("C0") + "</td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta.ToString("C0") + "</td>";
                                break;
                            case 5: //Group Term Life (GTLI)
                                strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Group </td>";
                                strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_policy + "</td>";

                                //Negative
                                if (total_gtli_sum_insure < 0)
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli_sum_insure.ToString("C0") + ")</td>";
                                }
                                else //Positive
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_sum_insure.ToString("C0") + "</td>";
                                }

                                //Negative 
                                if (total_gtli_fyp < 0)
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli_fyp.ToString("C0") + ")</td>";
                                }
                                else //Positive
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_fyp.ToString("C0") + "</td>";
                                }

                                //Negative
                                if (total_gtli_ryp < 0)
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli_ryp.ToString("C0") + ")</td>";
                                }
                                else //Positive
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_ryp.ToString("C0") + "</td>";
                                }

                                //Negative
                                if (total_gtli < 0)
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli.ToString("C0") + ")</td>";
                                }
                                else //Positive
                                {
                                    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli.ToString("C0") + "</td>";
                                }                                
                                break;
                        }

                        strTableSummray += "</tr>";

                        dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));

                        strTableSummray = "";

                    }
                }
            
            #endregion

            //Draw Total
            #region

            if (product_premium_list.Count > 0)
            {
                strTableSummray += "<tr>";

                strTableSummray += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\" >&nbsp;</td>";

                strTableSummray += "<td  style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >Total:</td>";

                if ((total_mrta_policy + total_term_life_policy + total_whole_life_policy + total_endowment_policy + total_gtli_policy) == 0)
                {
                    strTableSummray += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_policy + total_term_life_policy + total_whole_life_policy + total_endowment_policy) + "</td>";
                }

                if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure) == 0)
                {
                    strTableSummray += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    //Negative
                    if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure) < 0)
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure).ToString("C0") + ")</td>";
                    }
                    else //Positive
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure).ToString("C0") + "</td>";
                    }
                }

                if ((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp) == 0)
                {
                    strTableSummray += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    //Negative
                    if ((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp) < 0)
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp).ToString("C0") + ")</td>";
                    }
                    else //Positive
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp).ToString("C0") + "</td>";
                    }
                    
                }

                if ((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp) == 0)
                {
                    strTableSummray += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    //Negative 
                    if ((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp) < 0)
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp).ToString("C0") + ")</td>";
                    }
                    else //Positive
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp).ToString("C0") + "</td>";
                    }
                    
                }

                if ((total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli) == 0)
                {
                    strTableSummray += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    //Negative
                    if ((total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli) < 0)
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli).ToString("C0") + ")</td>";
                    }
                    else //Positive
                    {
                        strTableSummray += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli).ToString("C0") + "</td>";
                    }
                    
                }

                strTableSummray += "</tr>";

                dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));

                strTableSummray = "";
            }

            #endregion
           
        #endregion

        dvReportDetail.Style.Clear();
        dvReportSummary.Style.Clear();

        if (product_premium_list.Count > 0)
        {
            //Table Summary End
            strTableSummray += "</table>";
            dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));
            strTable = "";

            //Table Detail End
            strTable += "</table>";
            dvReportDetail.Controls.Add(new LiteralControl(strTable));
            strTable = "";
        }
        else
        {
           
            //Default Text in Table Detail
            dvReportDetail.Controls.Add(new LiteralControl("No data found. Please filter your search...."));

            strTable = "";

            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }

    }
         
    //Button Search Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        ReportProductPremium();
    }
}