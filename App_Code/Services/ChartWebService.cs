using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for ChartWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ChartWebService : System.Web.Services.WebService {

    public ChartWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //Get Policy Pie or Line Chart Data
    [WebMethod]
    public List<object> GetPolicyChartData(string check_form_load_or_search, string policy_status, string order_by, string from_date, string to_date, string product, string policy_number, string chart_data)
    {
        List<object> chartData = new List<object>();

        if(chart_data == "0"){
            chartData.Add(new object[] { "Product", "Total Sum Insure" });
        }
        else if (chart_data == "1")
        {
            chartData.Add(new object[] { "Product", "Total Premium" });
        }
        else if (chart_data == "2")
        {
            chartData.Add(new object[] { "Product", "Total Policy" });
        }
        List<bl_policy_report_chart> list_report_policy_chart = new List<bl_policy_report_chart>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";

        dtfi.DateSeparator = "/";

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(from_date, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(to_date, dtfi).Date + end_time;

        if (check_form_load_or_search != "1")
        {
            list_report_policy_chart = da_report_policy_chart.GetPolicyReportChartByConditions(from_date_time, to_date_time, policy_status, policy_number, product);
        }
          
        //Loop policy report list
        for (int i = 0; i < list_report_policy_chart.Count; i++)
        {
            bl_policy_report_chart policy_report_chart = new bl_policy_report_chart();
            policy_report_chart = list_report_policy_chart[i];

            switch (chart_data)
            {
                case "0"://Sum Insure
                    chartData.Add(new object[] { policy_report_chart.Product, policy_report_chart.Total_Sum_Insure });
                    break;
                case "1"://Premium                   
                    chartData.Add(new object[] { policy_report_chart.Product, policy_report_chart.Total_Premium });
                    break;
                case "2"://Policy
                    chartData.Add(new object[] { policy_report_chart.Product, policy_report_chart.Total_Policy });
                    break;

            }

        }
        
        return chartData;

    }

    //Get Policy Combo Chart Data
    [WebMethod]
    public List<ArrayList> GetPolicyComboChartData(string check_form_load_or_search, string policy_status, string order_by, string from_date, string to_date, string product, string policy_number, string chart_data)
    {
        List<ArrayList> chartData = new List<ArrayList>();
               
        List<bl_policy_report_chart> list_report_policy_chart = new List<bl_policy_report_chart>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";

        dtfi.DateSeparator = "/";

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(from_date, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(to_date, dtfi).Date + end_time;

        if (check_form_load_or_search != "1")
        {
            list_report_policy_chart = da_report_policy_chart.GetPolicyReportComboChartByConditions(from_date_time, to_date_time, policy_status, policy_number, product);
        }
     
        List<string> product_list = new List<string>();
        List<string> month_list = new List<string>();   

        //Get Distinct Product
        if (check_form_load_or_search != "1")
        {
            product_list = da_report_policy_chart.GetDistinctProductByConditions(from_date_time, to_date_time, policy_status, policy_number, product);          
            month_list = da_report_policy_chart.GetDistinctMonthByConditions(from_date_time, to_date_time, policy_status, policy_number, product);
        }

        ArrayList chart_title = new ArrayList();

        chart_title.Add("Month");

        for (int p = 0; p < product_list.Count; p++)
        {
            chart_title.Add(product_list[p]);
        }

        //Add x & y title to chart data
        chartData.Add(chart_title);


        //Add data to chart by month & product     
        for (int m = 0; m < month_list.Count; m++)
        {
            string month = month_list[m];

            ArrayList chart_value = new ArrayList();

            //Add month to x chart data
            chart_value.Add(month);
                       
            for (int p2 = 0; p2 < product_list.Count; p2++)
            {
                string data_product = product_list[p2];
                double value = 0;
                //Loop policy report list
                for (int i = 0; i < list_report_policy_chart.Count; i++)
                {
                    bl_policy_report_chart policy_report_chart = new bl_policy_report_chart();
                    policy_report_chart = list_report_policy_chart[i];

                    if ((policy_report_chart.Year + "/" + policy_report_chart.Month).ToString() == month && policy_report_chart.Product == data_product)
                    {
                        switch (chart_data)
                        {
                            case "0"://Sum Insure
                                value = policy_report_chart.Total_Sum_Insure;                                
                              
                                break;
                            case "1"://Premium                   
                                value = policy_report_chart.Total_Premium;
                               
                                break;
                            case "2"://Policy
                                value = policy_report_chart.Total_Policy;
                               
                                break;

                        }
                       
                    }
                }
             
                    chart_value.Add(value);
                
            }

           
            chartData.Add(chart_value);
                       
        }        
       
        return chartData;

    }


    //Get Application Pie or Line Chart Data
    [WebMethod]
    public List<object> GetApplicationChartData(string check_form_load_or_search, string policy_status, string order_by, string from_date, string to_date, string chart_data)
    {
        List<object> chartData = new List<object>();

        if (chart_data == "0")
        {
            chartData.Add(new object[] { "Product", "Total Sum Insure" });
        }
        else if (chart_data == "1")
        {
            chartData.Add(new object[] { "Product", "Total Premium" });
        }
        else if (chart_data == "2")
        {
            chartData.Add(new object[] { "Product", "Total Application" });
        }
        List<bl_application_report_chart> list_report_application_chart = new List<bl_application_report_chart>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";

        dtfi.DateSeparator = "/";

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(from_date, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(to_date, dtfi).Date + end_time;

        if (check_form_load_or_search != "1")
        {
            list_report_application_chart = da_report_application_chart.GetApplicationReportChartByConditions(from_date_time, to_date_time, policy_status);
        }

        //Loop application report list
        for (int i = 0; i < list_report_application_chart.Count; i++)
        {
            bl_application_report_chart application_report_chart = new bl_application_report_chart();
            application_report_chart = list_report_application_chart[i];

            switch (chart_data)
            {
                case "0"://Sum Insure
                    chartData.Add(new object[] { application_report_chart.Product, application_report_chart.Total_Sum_Insure });
                    break;
                case "1"://Premium                   
                    chartData.Add(new object[] { application_report_chart.Product, application_report_chart.Total_Premium });
                    break;
                case "2"://Application
                    chartData.Add(new object[] { application_report_chart.Product, application_report_chart.Total_Application });
                    break;

            }

        }

        return chartData;

    }

    //Get Application Combo Chart Data
    [WebMethod]
    public List<ArrayList> GetApplicationComboChartData(string check_form_load_or_search, string policy_status, string order_by, string from_date, string to_date, string chart_data)
    {
        List<ArrayList> chartData = new List<ArrayList>();

        List<bl_application_report_chart> list_report_application_chart = new List<bl_application_report_chart>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";

        dtfi.DateSeparator = "/";

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(from_date, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(to_date, dtfi).Date + end_time;

        if (check_form_load_or_search != "1")
        {
            list_report_application_chart = da_report_application_chart.GetApplicationReportComboChartByConditions(from_date_time, to_date_time, policy_status);
        }

        List<string> product_list = new List<string>();
        List<string> month_list = new List<string>();

        //Get Distinct Product
        if (check_form_load_or_search != "1")
        {
            product_list = da_report_application_chart.GetDistinctProductByConditions(from_date_time, to_date_time, policy_status);
            month_list = da_report_application_chart.GetDistinctMonthByConditions(from_date_time, to_date_time, policy_status);
        }

        ArrayList chart_title = new ArrayList();

        chart_title.Add("Month");

        for (int p = 0; p < product_list.Count; p++)
        {
            chart_title.Add(product_list[p]);
        }

        //Add x & y title to chart data
        chartData.Add(chart_title);


        //Add data to chart by month & product     
        for (int m = 0; m < month_list.Count; m++)
        {
            string month = month_list[m];

            ArrayList chart_value = new ArrayList();

            //Add month to x chart data
            chart_value.Add(month);

            for (int p2 = 0; p2 < product_list.Count; p2++)
            {
                string data_product = product_list[p2];
                double value = 0;
               
                //Loop application report list
                for (int i = 0; i < list_report_application_chart.Count; i++)
                {
                    bl_application_report_chart application_report_chart = new bl_application_report_chart();
                    application_report_chart = list_report_application_chart[i];

                    if ((application_report_chart.Year + "/" + application_report_chart.Month).ToString() == month && application_report_chart.Product == data_product)
                    {
                        switch (chart_data)
                        {
                            case "0"://Sum Insure
                                value = application_report_chart.Total_Sum_Insure;

                                break;
                            case "1"://Premium                   
                                value = application_report_chart.Total_Premium;

                                break;
                            case "2"://Application
                                value = application_report_chart.Total_Application;

                                break;

                        }

                    }
                }

                chart_value.Add(value);

            }


            chartData.Add(chart_value);

        }

        return chartData;

    }


    //Get Policy Chart Data Total Amount
    [WebMethod]
    public double GetPolicyChartDataTotal(string check_form_load_or_search, string policy_status, string order_by, string from_date, string to_date, string product, string policy_number, string chart_data)
    {
        double total_amount = 0;

        List<bl_policy_report_chart> list_report_policy_chart = new List<bl_policy_report_chart>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";

        dtfi.DateSeparator = "/";

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(from_date, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(to_date, dtfi).Date + end_time;

        if (check_form_load_or_search != "1")
        {
            list_report_policy_chart = da_report_policy_chart.GetPolicyReportComboChartByConditions(from_date_time, to_date_time, policy_status, policy_number, product);
        }

        //Loop policy report list
        for (int i = 0; i < list_report_policy_chart.Count; i++)
        {
            bl_policy_report_chart policy_report_chart = new bl_policy_report_chart();
            policy_report_chart = list_report_policy_chart[i];

           
                switch (chart_data)
                {
                    case "0"://Sum Insure
                       total_amount += policy_report_chart.Total_Sum_Insure;

                        break;
                    case "1"://Premium                   
                        total_amount += policy_report_chart.Total_Premium;

                        break;
                    case "2"://Policy
                        total_amount += policy_report_chart.Total_Policy;

                        break;

                }

            
        }

        return total_amount;

    }

    //Get Application Chart Data Total Amount
    [WebMethod]
    public double GetApplicationChartDataTotal(string check_form_load_or_search, string policy_status, string order_by, string from_date, string to_date, string chart_data)
    {
        double total_amount = 0;

        List<bl_application_report_chart> list_report_application_chart = new List<bl_application_report_chart>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";

        dtfi.DateSeparator = "/";

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(from_date, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(to_date, dtfi).Date + end_time;

        if (check_form_load_or_search != "1")
        {
            list_report_application_chart = da_report_application_chart.GetApplicationReportComboChartByConditions(from_date_time, to_date_time, policy_status);
        }

        //Loop application report list
        for (int i = 0; i < list_report_application_chart.Count; i++)
        {
            bl_application_report_chart application_report_chart = new bl_application_report_chart();
            application_report_chart = list_report_application_chart[i];

         
                switch (chart_data)
                {
                    case "0"://Sum Insure
                        total_amount += application_report_chart.Total_Sum_Insure;

                        break;
                    case "1"://Premium                   
                        total_amount += application_report_chart.Total_Premium;

                        break;
                    case "2"://Application
                        total_amount += application_report_chart.Total_Application;

                        break;

                }

            
        }

        return total_amount;

    }
}
