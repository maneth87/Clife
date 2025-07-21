using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ct_Policy_FP_Customer
/// </summary>
public class bl_FP_Customer
{
    #region 'Local variables declaration and properties'
    private string app_id;
    private string customer_id;
    private int id_type;
    private string id_number;
    private string surname_kh;
    private string surname_en;
    private int gender;
    private DateTime dob;
    private string nationality;
    private string occupation;
    private string created_by;
    private DateTime created_on;
    private string updated_by;
    private DateTime updated_on;
    private string first_name_kh;
    private string first_name_en;

    public string First_name_en
    {
        get { return first_name_en; }
        set { first_name_en = value; }
    }


    public string First_name_kh
    {
        get { return first_name_kh; }
        set { first_name_kh = value; }
    }

    public DateTime Updated_on
    {
        get { return updated_on; }
        set { updated_on = value; }
    }

    public string Updated_by
    {
        get { return updated_by; }
        set { updated_by = value; }
    }


    public DateTime Created_on
    {
        get { return created_on; }
        set { created_on = value; }
    }


    public string Created_by
    {
        get { return created_by; }
        set { created_by = value; }
    }


    public string Occupation
    {
        get { return occupation; }
        set { occupation = value; }
    }


    public string Nationality
    {
        get { return nationality; }
        set { nationality = value; }
    }


    public DateTime Dob
    {
        get { return dob; }
        set { dob = value; }
    }

    public int Gender
    {
        get { return gender; }
        set { gender = value; }
    }

    public string Surname_en
    {
        get { return surname_en; }
        set { surname_en = value; }
    }

    public string Surname_kh
    {
        get { return surname_kh; }
        set { surname_kh = value; }
    }

    public string Id_number
    {
        get { return id_number; }
        set { id_number = value; }
    }

    public int Id_type
    {
        get { return id_type; }
        set { id_type = value; }
    }

    public string Customer_id
    {
        get { return customer_id; }
        set { customer_id = value; }
    }

    public string App_ID
    {
        get { return app_id; }
        set { app_id = value; }
    }
    #endregion
    #region 'constructor'
    public bl_FP_Customer()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_FP_Customer(string app_id, string customer_id, int id_type, string id_number, string surnamekh, string surnameen, string first_name_kh, string first_name_en, int gender, DateTime dob, string nationality, string occupation, string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.app_id = app_id;
        this.customer_id = customer_id;
        this.id_type = id_type;
        this.id_number = id_number;
        this.surname_kh = surnamekh;
        this.surname_en = surnameen;
        this.first_name_kh = first_name_kh;
        this.first_name_en = first_name_en;
        this.gender = gender;
        this.dob = dob;
        this.nationality = nationality;
        this.occupation = occupation;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}