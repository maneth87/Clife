using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_sangkat
/// </summary>
public class bl_sangkat
{
	public bl_sangkat()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Khan_ID { get; set; }
    public string Sangkat_ID { get; set; }
    public string Sangkat_Name { get; set; }
    public string Sangkat_Name_Kh { get; set; }
    public string Sangkat_Post_Code { get; set; }
    public string Remarks { get; set; }
    public string Created_By { get; set; }
    public DateTime Created_DateTime { get; set; }
    public string Updated_By { get; set; }
    public DateTime Updated_DateTime { get; set; }

    public bl_sangkat(string khan_id, string sangkat_id, string sangkat_name, string sangkat_name_kh, string sangkat_post_code, string remarks, string created_by, DateTime created_datetime, string updated_by, DateTime updated_datetime)
    {
        this.Khan_ID = khan_id;
        this.Sangkat_ID = sangkat_id;
        this.Sangkat_Name = sangkat_name;
        this.Sangkat_Name_Kh = sangkat_name_kh;
        this.Sangkat_Post_Code = sangkat_post_code;
        this.Remarks = remarks;
        this.Created_By = created_by;
        this.Created_DateTime = created_datetime;
        this.Updated_By = updated_by;
        this.Updated_DateTime = updated_datetime;
    }
}