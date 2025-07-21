using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Globalization;
/// <summary>
/// Summary description for GTLIWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class GTLIWebService : System.Web.Services.WebService {

    public GTLIWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string CheckCompany(string company_name)
    {
        if (da_gtli_company.CheckCompany(company_name))
        {
            return "1"; //Found Company
        }
        else
        {
            return "0"; //Note Found Company
        }       
    }

    [WebMethod]
    public string GetPlan(string company_name)
    {
        List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();
        plan_list = da_gtli_plan.GetPlanListByCompanyName(company_name);

        string company_id = da_gtli_company.GetCompanyIDByCompanyName(company_name);

        if (plan_list.Count > 0)
        {
            //Get last plan by company id
            bl_gtli_plan last_plan = new bl_gtli_plan();
            last_plan = da_gtli_plan.GetLastPlanByCompanyID(company_id);
           
            //Add char value
            char last_plan_char = Convert.ToChar(last_plan.GTLI_Plan);
            
            char new_plan_char = Convert.ToChar(last_plan_char + 1);
            return new_plan_char.ToString();
        }
        else
        {
            return "A";
        }
    }

    [WebMethod]
    public List<bl_gtli_plan> GetPlans(string company_name)
    {
        List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();
        plan_list = da_gtli_plan.GetPlanListByCompanyName(company_name);

        return plan_list;
    }
    [WebMethod]
    public string getEffectiveDate(string companyName)
    {
        string companyID = "";
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();

        DateTime effectiveDate = DateTime.Now;

        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        companyID = da_gtli_company.GetCompanyIDByCompanyName(companyName.Trim());
        bl_gtli_policy last_policy = new bl_gtli_policy();
        last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(companyID);

        effectiveDate = last_policy.Effective_Date;
        effectiveDate = effectiveDate.AddYears(1);
       // effectiveDate = Convert.ToDateTime(effectiveDate, dtfi);

        return effectiveDate.ToString("dd/MM/yyyy");
    }
}
