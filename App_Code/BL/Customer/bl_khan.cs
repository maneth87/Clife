using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_khan
/// </summary>
public class bl_khan
{
	public bl_khan()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pro_id">Province ID</param>
    /// <param name="khan_id"></param>
    /// <param name="khan_name"></param>
    /// <param name="khan_name_kh"></param>
    /// <param name="khan_post_code"></param>
    /// <param name="remarks"></param>
    /// <param name="created_by"></param>
    /// <param name="created_datetime"></param>
    /// <param name="updated_by"></param>
    /// <param name="updated_datetime"></param>
    public bl_khan(string pro_id, string khan_id, string khan_name, string khan_name_kh, string khan_post_code, string remarks, string created_by, DateTime created_datetime, string updated_by, DateTime updated_datetime) 
    {
        this.Pro_ID = pro_id;
        this.Khan_ID = khan_id;
        this.Khan_Name = khan_name;
        this.Khan_Name_Kh = khan_name_kh;
        this.Khan_Post_Code = khan_post_code;
        this.Remarks = remarks;
        this.Created_By = created_by;
        this.Created_DateTime = created_datetime;
        this.Updated_By = updated_by;
        this.Updated_DateTime = updated_datetime;
    }
    public string Pro_ID { get; set; }
    public string Khan_ID { get; set; }
    public string Khan_Name { get; set; }
    public string Khan_Name_Kh { get; set; }
    public string Khan_Post_Code { get; set; }
    public string Remarks { get; set; }
    public string Created_By { get; set; }
    public DateTime Created_DateTime { get; set; }
    public string Updated_By { get; set; }
    public DateTime Updated_DateTime { get; set; }
}