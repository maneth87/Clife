using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_user_access
/// </summary>
public class bl_user_access
{
	public bl_user_access()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string ID { get; set; }
    public string UserId { get; set; }
    public int Access { get; set; }
    public string PageId { get; set; }
    public string PageName { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; }
}