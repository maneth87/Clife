using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FP_Helper
/// </summary>
public class FP_Helper
{
	public FP_Helper()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static DateTime newDateFormat(string date)
    {
        DateTime newDate;
        DateTimeFormatInfo  dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        newDate = Convert.ToDateTime(date, dtfi);

        return newDate;
    }
    public static string formatApplicationNumber(int applicationNumber)
    {
        string newAppNo = "";
        int len=applicationNumber.ToString().Length;
        for (int i = len; i < 8; i++)
        { 
            newAppNo = newAppNo + "0";
        }
        newAppNo = "A" + newAppNo + applicationNumber;
        return newAppNo;
    }

   

}