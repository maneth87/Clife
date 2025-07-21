using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Text;
/// <summary>
/// This class use for check permission to access the page of each user
/// </summary>
public class authentication
{
    string  __userName ="";
    string __userID = "";
    string __requestPage = "";
   
	public authentication()
	{
		//
		// TODO: Add constructor logic here
		//
        __userID = Membership.GetUser().ProviderUserKey.ToString();
        __userName = Membership.GetUser().UserName;
       
    }
    #region private function
    //private bool GetAuthorize()
    //{
    //    bool result = false;
    //    StringBuilder response = new StringBuilder();
    //    try
    //    {
    //        da_user_access user_acc = new da_user_access();
    //        if (user_acc.GetActiveUserAccessPage(RequestPage, __userID).UserId != __userID)
    //        {
    //            result = false;
               
    //        }
    //        else
    //        {
    //            result = true;
               
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        result = false;
    //        Log.AddExceptionToLog("Error " + Log.GenerateLog(ex));

    //    }
    //    return result;
    //}
    #endregion private function
    #region Properties
    public string UserName {
        get {
            return __userName;
        }
    }
    public string UserID
    {
        get
        {
            return __userID;
        }
    }
    ///// <summary>
    ///// This page name which is checked permission
    ///// </summary>
    //public string RequestPage { get; set; }
    //public bool Authorize {
    //    get {
    //       return GetAuthorize();
    //    }
    //}
   
    #endregion Properties
}