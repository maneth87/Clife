using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for ProductWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ProductWebService : System.Web.Services.WebService {

    public ProductWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<bl_product> GetInsurancePlans(string product_type) {
        List<bl_product> product_list = new List<bl_product>();

        product_list = da_product.GetProductsByProductType(product_type);

        return product_list;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<bl_product> GetInsurancePlansEdit(string product_type, string product_id)
    {
        List<bl_product> product_list = new List<bl_product>();

        product_list = da_product.GetEditProductsByProductType(product_type, product_id);

        return product_list;
    }

    [WebMethod]   
    public int GetPayYear(string product_id)
    {
       int pay_year = 0;

       pay_year = da_product.GetPayYearByProductID(product_id);

       return pay_year;
    }

    [WebMethod]
    public int GetAssureYear(string product_id, string customer_age)
    {
        int assure_year = 0;

        assure_year = da_product.GetAssureYearByProductID(product_id);

        //Get Product information
        bl_product product = new bl_product();
        product = da_product.GetProductByProductID(product_id);

        switch (product.Plan_Block)
        {          
           
            case "001"://New Whole Life
            case "A": //Old Whole Life
                assure_year = assure_year - Convert.ToInt32(customer_age); // 90 - Age_Insure
               
                break;

            case "M" : //MRTA
            case "006" :
            case "007" :
            case "008" :
                assure_year = 0;

                break;

            case "T":
                assure_year = assure_year + 0;
                break;

        }

        return assure_year;
    }


    //Get benefit payment clause from db by passing product_id param
    [WebMethod]
    public bl_benefit_payment_clause GetBenefitClause(string product_id)
    {
        bl_benefit_payment_clause benefit_payment_clause = new bl_benefit_payment_clause();

        benefit_payment_clause = da_benefit_payment_clause.GetBenefitPaymentClauseByProductID(product_id);

        return benefit_payment_clause;
    }

    //Micro product
    [WebMethod]
    public bl_product GetProductMicro(string card)
    {
        switch (card)
        {
            case "1":
                card = "T1011";
                break;
        }

        bl_product product = new bl_product();
        product = da_product.GetProductByProductID(card);

        return product;
    }

    //Flexi Term product
    [WebMethod]
    public bl_product GetProductFlexiTerm(string card)
    {
        switch (card)
        {
            case "1":
                card = "FT013";
                break;
        }

        bl_product product = new bl_product();
        product = da_product.GetProductByProductID(card);

        return product;
    }

    [WebMethod]
    public List<bl_product> GetProductCMK(string card)
    {
        switch (card)
        {
            case "6":
                card = "CLC";
                break;
        }

        List<bl_product> product = new List<bl_product>();
        product.Add(da_product.GetProductByProductID(card));

        return product;
    }
}
