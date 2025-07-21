using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_province
/// </summary>
public class bl_province
{
	public bl_province()
	{
		//
		// TODO: Add constructor logic here
		//
        CreatedDateTime = DateTime.Now;
        UpdatedDatetime = DateTime.Now;
	}
    public string ProID { get; set; }
    public string ProNAME { get; set; }
    public string ProNameKH { get; set; }
    public string Remarks { get; set; }
    public string ProPostCode { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDatetime { get; set; }

    public bl_province(string pro_id, string pro_name, string pro_name_kh, string remarks, string pro_post_code, string created_by, DateTime created_datetime, string updated_by, DateTime updated_datetime)
    {
        this.ProID = pro_id;
        this.ProNAME = pro_name;
        this.ProNameKH = pro_name_kh;
        this.ProPostCode = pro_post_code;
        this.Remarks = remarks;
        this.CreatedBy = created_by;
        this.UpdatedBy = updated_by;
        this.UpdatedDatetime = updated_datetime;
        this.CreatedDateTime = created_datetime;
    }
}
