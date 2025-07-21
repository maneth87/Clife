using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_FP_Customer_sub
/// </summary>
public class bl_FP_Customer_sub
{
    #region 'local variable and properties'
   
    private string app_id;
    private string sub_id;
    private int sub_number;
    private int id_type;
    private string id_number;
    private string surname_kh;
    private string surname_en;
    private string first_name_kh;
    private string first_name_en;
    private int gender;
    private DateTime dob;
    private string created_by;
    private DateTime created_on;
    private string updated_by;
    private DateTime updated_on;
    private string relationship;

    public string Relationship
    {
        get { return relationship; }
        set { relationship = value; }
    }

    #region 'properties'
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


    public int Sub_number
    {
        get { return sub_number; }
        set { sub_number = value; }
    }


    public string App_id
    {
        get { return app_id; }
        set { app_id = value; }
    }


    public string Sub_id
    {
        get { return sub_id; }
        set { sub_id = value; }
    }
    #endregion

    #endregion
    #region'constructor'
    public bl_FP_Customer_sub()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bl_FP_Customer_sub(string app_id, string sub_id, int sub_number, int id_type, string id_number, string surnamekh, string surnameen, string first_name_kh, string first_name_en, int gender, DateTime dob, string relationship, string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.app_id = app_id;
        this.sub_id = sub_id;
        this.sub_number = sub_number;
        this.id_type = id_type;
        this.id_number = id_number;
        this.surname_kh = surnamekh;
        this.surname_en = surnameen;
        this.first_name_kh = first_name_kh;
        this.first_name_en = first_name_en;
        this.gender = gender;
        this.dob = dob;
        this.relationship = relationship;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }

    #endregion
   
}