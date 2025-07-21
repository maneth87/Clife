using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_page
/// </summary>
public class bl_page
{
    public bl_page()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string ApplicatoinID { get; set; }
    public string PageName { get; set; }
    public string PageID { get; set; }
    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public string UpdatedOn { get; set; }
}