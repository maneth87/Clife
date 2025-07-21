using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_master_list
/// </summary>
public class bl_master_list
{
	public bl_master_list()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int Id { get; set; }
    public int OrderNo { get; set; }
    public string MasterListCode { get; set; }
    public string Code { get; set; }
    public string DescEn { get; set; }
    public string DescKh { get; set; }
    public int Status { get; set; }
    public class bl_relation
    {
        public string Id { get; set; }

        public string RelationEn { get; set; }

        public string RelationKh { get; set; }

        public int GenderCode { get; set; }
    }
}