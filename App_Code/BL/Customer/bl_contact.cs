using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_contact
/// </summary>
public class bl_contact
{
	public bl_contact()
	{
		//
		// TODO: Add constructor logic here
		//
       
	}
    public bl_contact(int id, string cust_id, string name, string mobile, string tel, string fax, string mail, string responsibility, string address, string remarks, string created_by, 
        DateTime created_datetime, string updated_by, DateTime updated_datetime)
    {
        this.ID = id;
        this.Name = name;
        this.Mobile = mobile;
        this.Tel = tel;
        this.Fax = fax;
        this.Mail = mail;
        this.Responsibility = responsibility;
        this.Address = address;
        this.Remarks = remarks;
        this.Created_By = created_by;
        this.Created_DateTime = created_datetime;
        this.Updated_By = updated_by;
        this.Updated_DateTime = updated_datetime;
    }
    public int ID { get; set; }
    public string Cust_ID { get; set; }
    public string Name { get; set; }
    public string Mobile { get; set; }
    public string Tel { get; set; }
    public string Fax { get; set; }
    public string Mail { get; set; }
    public string Responsibility { get; set; }
    public string Address { get; set; }
    public string Remarks { get; set; }
    public string Created_By { get; set; }
    public DateTime Created_DateTime { get; set; }
    public string Updated_By { get; set; }
    public DateTime Updated_DateTime { get; set; }
}