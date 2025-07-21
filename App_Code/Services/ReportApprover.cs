using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Security;

/// <summary>
/// Summary description for ReportApprover
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class ReportApprover : System.Web.Services.WebService {

    public ReportApprover () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public da_report_approver.bl_report_approver GetApproverInfo(string policy_id)
    {
        da_report_approver.bl_report_approver appro = new da_report_approver.bl_report_approver();
       // string str_info = "";
        appro = da_report_approver.GetAproverInfo(policy_id);
        //str_info = appro.PositionKh + "," + appro.PositionKh;
        return appro;
    }

    [WebMethod]
    public bool SaveApprover(string policy_id, string approver_id )
    {
        bool status = false;
        try
        {
            string user_name = "";
            MembershipUser myUser = Membership.GetUser();
            user_name = myUser.UserName;

            da_report_approver.bl_report_approver_policy app_policy = new da_report_approver.bl_report_approver_policy();
            app_policy.Policy_ID = policy_id;
            app_policy.Approver_ID = Convert.ToInt32(approver_id) ;
            app_policy.Created_By = user_name;
            app_policy.Created_On= DateTime.Now;
            if(da_report_approver.InsertApproverPolicy(app_policy))
            {
                status=true;
            }
            else
            {
               status= false;
            }

        }
        catch (Exception ex)
        {

            status=false;
            throw ex;
            
        }
        return status;
    }
    [WebMethod]

    public  List<da_report_approver.bl_report_approver> GetApproverName(string position_en)
    {
      
        //List<string> str = new List<string>();
        List<da_report_approver.bl_report_approver> list_approv = new List<da_report_approver.bl_report_approver>();
        foreach (da_report_approver.bl_report_approver appro in da_report_approver.GetApproverList())
        {
           if(appro.PositionEn.Trim().ToUpper() == position_en.Trim().ToUpper())
           {
            list_approv.Add(appro);
           }
        }
        return list_approv;
    }



}
