using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WingWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WingWebService : System.Web.Services.WebService {

    public WingWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string CheckWingAccount(string wing_sk, string wing_num)
    {
        string wing_account = "";

        string sk = da_wing_account.GetWingSkAccount(wing_sk);
        string num = da_wing_account.GetWingNumAccount(wing_num);

        if (sk != wing_sk && num !=wing_num)
        {
    
            wing_account = "update";      
               
        }

        else if (num != wing_num)
        {         

            wing_account = "update";
         
        }
        else if (sk != wing_sk)
        {
            
                wing_account = "update";
            
        }
        else {

            if (sk == wing_sk || num == wing_num)
            {

                wing_account = "Wing Sk " + wing_sk + " and " + "Wing Num " + wing_num + " already created !";
                     
            }
        }

        return wing_account;
    }


    //Get wing account object
    [WebMethod]
    public bl_policy_wing GetWINGObject(string policy_wing_id)
    {
        bl_policy_wing my_policy_wing = da_policy.GetPolicyWINGByID(policy_wing_id);    
        return my_policy_wing;
    }
    
}
