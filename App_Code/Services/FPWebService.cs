using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for FPWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.ComponentModel.ToolboxItem(false)] 
[System.Web.Script.Services.ScriptService]
public class FPWebService : System.Web.Services.WebService {

    public FPWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

   

    [WebMethod]
    public void getPlanList(string planId)
    {
        List<bl_Policy_FP_Plan> myPlanList = new List<bl_Policy_FP_Plan>();
        myPlanList = da_Policy_FP_Plan.getPlanListByPlanID(planId);
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(myPlanList));
    }

    [WebMethod]
    public string getPremiumLife1(string product_id, string customer_age, string gender, string paymentMode)
    {
        double premium = 0;
       
        int mycustomer_age = Convert.ToInt32(customer_age);
        int mygender = Convert.ToInt32(gender);
        int paymentid = Convert.ToInt32(paymentMode);
        string result = "0,0";
        double originalPremium = 0.0;

        premium = da_FP_Premium.getPremium(product_id, mygender, mycustomer_age, paymentid);
        originalPremium = da_FP_Premium.getOriginalPremium(product_id, mygender, mycustomer_age);

        result = premium + "," + originalPremium;
        return result;
        //JavaScriptSerializer js = new JavaScriptSerializer();
        //Context.Response.Write(js.Serialize(result));
    }

    [WebMethod]
    public string  getPremiumLife2(string product_id, string customer_age, string gender, string paymentMode)
    {
        
        int mycustomer_age = Convert.ToInt32(customer_age);
        int mygender = Convert.ToInt32(gender);
        int paymentid = Convert.ToInt32(paymentMode);
        double originalPremium = 0.0;
        double premium = 0;
        string result = "0,0";

        premium = da_FP_Premium.getLifeInsuredPremium(product_id, mygender, mycustomer_age, paymentid);
        originalPremium = da_FP_Premium.getLifeInsuredOriginalPremium(product_id, mygender, mycustomer_age);

        result = premium + "," + originalPremium;
     
       //JavaScriptSerializer js = new JavaScriptSerializer();
       //Context.Response.Write(js.Serialize(result));
        return result;
    }
    
}
