using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;

/// <summary>
/// Summary description for CustomerWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class CustomerWebService : System.Web.Services.WebService {

    public CustomerWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //[Meas sun Modified On 2019-11-20]
    [WebMethod]
    public string SaveCustomer(string customer_info)
    {
        string cust_id = "";
        try
        {
            string[] str_arr = customer_info.Split(';');
            int row_count = str_arr.Length - 1;
            bl_customer my_customer = new bl_customer();
            my_customer.Customer_Number = str_arr[0].ToString();
            my_customer.First_Name = str_arr[1].ToString();
            my_customer.Last_Name = str_arr[2].ToString();
            my_customer.Khmer_Last_Name = str_arr[3].ToString();
            my_customer.Khmer_First_Name = str_arr[4].ToString();
            if (str_arr[5].ToString() == "")
            {
                my_customer.Gender = -1;
            }
            else
            {
                my_customer.Gender = Convert.ToInt32(str_arr[5].ToString());
            }
            if (str_arr[6].ToString() == "")
            {
                my_customer.ID_Type = -1;
            }
            else
            {
                my_customer.ID_Type = Convert.ToInt32(str_arr[6].ToString());
            }
            my_customer.ID_Card = str_arr[7].ToString();
            if (str_arr[8].ToString() == "")
            {
                my_customer.Birth_Date = DateTime.Now;
            }
            else
            {
                my_customer.Birth_Date = Helper.FormatDateTime(str_arr[8].ToString());
            }
            my_customer.Nationality = str_arr[9].ToString();
            my_customer.Mother_First_Name = str_arr[10].ToString();
            my_customer.Mother_Last_Name = str_arr[11].ToString();
            my_customer.Mother_First_Name_KH = str_arr[12].ToString();
            my_customer.Mother_Last_Name_KH = str_arr[13].ToString();
            my_customer.Father_First_Name = str_arr[14].ToString();
            my_customer.Father_Last_Name = str_arr[15].ToString();
            my_customer.Father_First_Name_KH = str_arr[16].ToString();
            my_customer.Father_Last_Name_KH = str_arr[17].ToString();
            my_customer.Created_By = Membership.GetUser().UserName;
            my_customer.Created_On = DateTime.Now;

            //save records
            if (da_customer.SaveCustomer(my_customer))
            {
                cust_id = str_arr[0].ToString(); 
            }
           
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveCustomer] in class [CustomerWebService], Detail: " + ex.Message);
            cust_id = "";
        }
        return cust_id;
    }

    [WebMethod]
    public bool UpdateCustomer(string customer_info)
    {
        bool status = false;
        try
        {
            string[] str_arr = customer_info.Split(';');
            int row_count = str_arr.Length - 1;
            bl_customer my_customer = new bl_customer();
            my_customer.Customer_Number = str_arr[0].ToString();
            my_customer.First_Name = str_arr[1].ToString();
            my_customer.Last_Name = str_arr[2].ToString();
            my_customer.Khmer_Last_Name = str_arr[3].ToString();
            my_customer.Khmer_First_Name = str_arr[4].ToString();
            if (str_arr[5].ToString() == "")
            {
                my_customer.Gender = -1;
            }
            else
            {
                my_customer.Gender = Convert.ToInt32(str_arr[5].ToString());
            }
            if (str_arr[6].ToString() == "")
            {
                my_customer.ID_Type = -1;
            }
            else
            {
                my_customer.ID_Type = Convert.ToInt32(str_arr[6].ToString());
            }
            my_customer.ID_Card = str_arr[7].ToString();
            if (str_arr[8].ToString() == "")
            {
                my_customer.Birth_Date = DateTime.Now;
            }
            else
            {
                my_customer.Birth_Date = Helper.FormatDateTime(str_arr[8].ToString());
            }
            my_customer.Nationality = str_arr[9].ToString();
            my_customer.Mother_First_Name = str_arr[10].ToString();
            my_customer.Mother_Last_Name = str_arr[11].ToString();
            my_customer.Mother_First_Name_KH = str_arr[12].ToString();
            my_customer.Mother_Last_Name_KH = str_arr[13].ToString();
            my_customer.Father_First_Name = str_arr[14].ToString();
            my_customer.Father_Last_Name = str_arr[15].ToString();
            my_customer.Father_First_Name_KH = str_arr[16].ToString();
            my_customer.Father_Last_Name_KH = str_arr[17].ToString();
            my_customer.Created_By = Membership.GetUser().UserName;
            my_customer.Created_On = DateTime.Now;

            //save records
            status = da_customer.UpdateCustomer(my_customer);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateCustomer] in class [CustomerWebService], Detail: " + ex.Message);
        }
        return status;
    }

    [WebMethod]
    public string GetExistCustomer(string first_name_en, string last_name_en, string gender, string id_card, string birth_date)
    {
        string cust_id = "";
        try
        {
            cust_id = da_customer.GetExistingCustomer(first_name_en, last_name_en, Convert.ToInt32( gender), id_card, Helper.FormatDateTime( birth_date));
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function [GetExistCustomer] in class [CustomerWebService], Detail: " + ex.Message);
        }
        return cust_id;
    }
    [WebMethod]
    public List<bl_customer> GetCustomerList(string customer_name, string customer_type_id, string id_card, string customer_id, string customer_number)
    {
        return da_customer.GetCustomerList(customer_name, customer_type_id, id_card, customer_id, customer_number);
    }
    [WebMethod]
    public List<bl_customer> GetCustomerByCustID(string customer_number)
    {
        return da_customer.GetCustomerList("", "", "", "", customer_number);
    }
    [WebMethod]
    public string GenerateNewCustomerNumber()
    {
        return da_customer.GetCustomerID();
    }
    [WebMethod]
    public bool DeleteCustomer(string customer_id)
    {
        return da_customer.DeleteCustomer(customer_id);

    }

    //[Meas sun Modified On 2019-12-03]
    [WebMethod]
    public bool SaveContact(List<string> contact)
    {
        bool status = false;
        try
        {
            bl_app_info_contact obj = new bl_app_info_contact();
            if (contact.Count > 0)
            {
                #region Save Contact
                obj.PolicyID = contact[0].ToString();;
                obj.Mobile_Phone1 = contact[1].ToString();
                obj.Mobile_Phone2 = contact[2].ToString();
                obj.Fax1 = contact[3].ToString();
                obj.EMail = contact[4].ToString();

                status = da_policy.InsertPolicyContact(obj);
                #endregion

            }
            else
            {
                return status;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveContact] in class [CustomerWebService], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }

    //[Meas sun Modified On 2020-04-14]
    [WebMethod]
    public string SaveAddress(string country_id, string province, string zip_code, string address1, string address2)
    {
        string address_id = "";
        try
        {
            bl_policy_address addr = new bl_policy_address();
            addr.Address_ID = Helper.GetNewGuid("SP_Check_Policy_Address_ID", "@Address_ID").ToString();
            addr.Country_ID = country_id;
            addr.Province = province;
            addr.Zip_Code = zip_code;
            addr.Address1 = address1;
            addr.Address2 = address2;
            addr.Address3 = "";

            if (da_policy.InsertPolicyAddress(addr))
            {
                address_id = addr.Address_ID;
            }
            else
            {
                address_id = "";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveAddress] in class [CustomerWebService], Detail: " + ex.Message);
            address_id = "";
        }
        return address_id;
    }

    public bool DeleteContact(string id)
    { 
        bool status =false;
        try
        {
            string[] id_arr = id.Split(';');
            foreach (string delete_id in id_arr)
            {
                status = da_customer.DeleteContact(Convert.ToInt32(delete_id));
            }
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteContact] in class [CustomerWebService], Detail: " + ex.Message);
        }

        return status;
    }
    [WebMethod]
    public List<bl_contact> GetContactList(string cust_id)
    {
        List<bl_contact> contact_list = new List<bl_contact>();
        try
        {
            contact_list = da_customer.GetContactList(cust_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetContactList] in class [CustomerWebService], Detail: " + ex.Message);
        }
        return contact_list;
    }
    [WebMethod]
    public List<bl_khan> GetKhanListByProID(string pro_id)
    {
        List<bl_khan> khan_list = new List<bl_khan>();
        try 
        {
            khan_list = da_customer.GetKhanListByProID(pro_id);
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Errro function [GetKhankListByProID] in class [da_customer], Detail: " + ex.Message);
        }
        return khan_list;
    }
    [WebMethod]
    public List<bl_sangkat> GetSangkatListByKhanID(string khan_id)
    {
        List<bl_sangkat> sangkat_list = new List<bl_sangkat>();
        try
        {
            sangkat_list = da_customer.GetSangkatListByKhanID(khan_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Errro function [GetSangkatListByProID] in class [da_customer], Detail: " + ex.Message);
        }
        return sangkat_list;
    }
    [WebMethod]
    public List<string> GetProvince(string pro_id) {
        List<string> province_name = new List<string>();
        foreach (bl_province pro in da_province.GetProvinceList(pro_id))
        {
            province_name.Add(pro.ProNAME);
        }
        return province_name;
    }

    [WebMethod]
    public void CustomerBeforeDue()
    {
        System.Data.DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(),"[SP_GET_CUSTOMER_BEFORE_DUE_NOTIFICATION]", new string[,] { },

            "Function [CustomerBeforeDue()] class [CustomerWebService]");
        List<bl_before_due_notification> listBeforeDue = new List<bl_before_due_notification>();
        bl_before_due_notification obj_before_due;
        if (tbl.Rows.Count > 0)
        {
            foreach (System.Data.DataRow row in tbl.Rows)
            {
                obj_before_due = new bl_before_due_notification();
                obj_before_due.NameEN = row["en_name"].ToString();
                obj_before_due.Email = row["email"].ToString();
                obj_before_due.PolicyNumber = row["policy_number"].ToString();
                obj_before_due.CustomerID = row["customer_id"].ToString();
                obj_before_due.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                obj_before_due.PayMode = Convert.ToInt32(row["pay_mode"].ToString());
                
                obj_before_due.NameKH = row["kh_name"].ToString();
                obj_before_due.DOB = Convert.ToDateTime(row["birth_date"].ToString());
                obj_before_due.Title = row["title"].ToString();
                obj_before_due.PhoneNumber = row["phone"].ToString();
                obj_before_due.DueDate = Convert.ToDateTime(row["due_date"].ToString());
                obj_before_due.CompareDate = DateTime.Now.Date;

               //DateTime nextdue = new DateTime();
               // nextdue = Helper.GetDueDateList(obj_before_due.DueDate, obj_before_due.PayMode)[1]; // index zero is the first due date

                if (obj_before_due.NumberOfNextDueDay == 7 || obj_before_due.NumberOfNextDueDay==15)
                {

                    listBeforeDue.Add(obj_before_due);
                }
            }

            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(listBeforeDue));
        }
    }

    //[Meas sun Created On 2019-12-09]
    [WebMethod]
    public bool RollBack(string customer_id)
    {
        bool status = false;
        try
        {
            status = da_customer.RollBack(customer_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [RollBack] in class [CustomerWebService], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
    //[Meas sun Created On 2019-12-09]
    [WebMethod]
    public bool RollBackAddress(string address_id)
    {
        bool status = false;
        try
        {
            status = da_customer.RollBackAddress(address_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [RollBackAddress] in class [CustomerWebService], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
}
