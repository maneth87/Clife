using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_customer
/// </summary>
public class da_micro_group_customer
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";
    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
    public da_micro_group_customer()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool Save(bl_micro_group_customer cusotmer)
    {
        try
        {
            var a = cusotmer;
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_INSERT", new string[,] 
            {
            {"@customer_id", a.ID},
            {"@customer_no",a.CUSTOMER_NUMBER},
            {"@seq", a.SEQ+""},
            {"@first_name_en", a.FIRST_NAME_EN},{"@last_name_en", a.LAST_NAME_EN},
            {"@first_name_kh",a.FIRST_NAME_KH},{"@last_name_kh",a.LAST_NAME_KH},
            {"@full_name_en", a.FULL_NAME_EN},{"@full_name_kh",a.FULL_NAME_KH},
            {"@gender", a.GENDER+""},{"@dob",a.DOB+""},
            {"@id_type", a.ID_TYPE+""},{"@id_number",a.ID_NUMBER},
            {"@nationality", a.NATIONALITY},{"@occupation", a.OCCUPATION},
            {"@place_of_birth", a.PLACE_OF_BIRTH},{"@status", a.STATUS+""},
            {"@marital_status",a.MARITAL_STATUS},
            {"@created_by", a.CREATED_BY},{"@created_on",a.CREATED_ON+""},{"@remarks",a.REMARKS}
            }, "da_micro_group_customer=>Save(bl_micro_group_customer cusotmer)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Save(bl_micro_group_customer cusotmer) in class [da_micro_group_customer], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return _SUCCESS;
    }

    public static bool Update(bl_micro_group_customer cusotmer, string oldCustomerId)
    {
        try
        {
            var a = cusotmer;
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_UPDATE", new string[,] 
            {
            {"@customer_id",oldCustomerId},
            {"@first_name_en", a.FIRST_NAME_EN},
            {"@last_name_en", a.LAST_NAME_EN},
            {"@first_name_kh",a.FIRST_NAME_KH},
            {"@last_name_kh",a.LAST_NAME_KH},
            {"@full_name_en", a.FULL_NAME_EN},
            {"@full_name_kh",a.FULL_NAME_KH},
            {"@gender", a.GENDER+""},
            {"@dob",a.DOB+""},
            {"@id_type", a.ID_TYPE+""},
            {"@id_number",a.ID_NUMBER},
            {"@nationality", a.NATIONALITY},
            {"@occupation", a.OCCUPATION},
            {"@place_of_birth", a.PLACE_OF_BIRTH},
            {"@status", a.STATUS+""},
            {"@marital_status",a.MARITAL_STATUS},
            {"@UPDATED_BY", a.UPDATED_BY},
            {"@UPDATED_ON",a.UPDATED_ON+""},
            {"@remarks",a.REMARKS}
            }, "da_micro_group_customer=>Update(bl_micro_group_customer cusotmer, string oldCustomerId)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Update(bl_micro_group_customer cusotmer, string oldCustomerId) in class [da_micro_group_customer], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return _SUCCESS;
    }

    /// <summary>
    /// Get customer by full name english, id type and id number
    /// </summary>
    /// <param name="fullNameEn"></param>
    /// <param name="idType"></param>
    /// <param name="idNumber"></param>
    /// <returns></returns>
    public static bl_micro_group_customer Get(string fullNameEn, Int32 idType, string idNumber)
    {
        bl_micro_group_customer cuz = new bl_micro_group_customer();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET", new string[,] {
            { "@full_name_en",fullNameEn  } ,{"@id_type",idType+""},{"@id_number",idNumber}
            }, "da_micro_group_customer=>Get(string fullName, Int32 idType, string idNumber) ");
            foreach (DataRow r in tbl.Rows)
            {
                cuz.ID = r["customer_id"].ToString();
                cuz.CUSTOMER_NUMBER = r["customer_no"].ToString();
                cuz.FIRST_NAME_EN = r["first_name_en"].ToString();
                cuz.LAST_NAME_EN = r["last_name_en"].ToString();
                cuz.FIRST_NAME_KH = r["first_name_kh"].ToString();
                cuz.LAST_NAME_KH = r["last_name_kh"].ToString();
                cuz.GENDER = Convert.ToInt32(r["gender"].ToString());
                cuz.DOB = Convert.ToDateTime(r["dob"].ToString());
                cuz.ID_TYPE = Convert.ToInt32(r["id_type"].ToString());
                cuz.ID_NUMBER = r["id_number"].ToString();
            }
        }
        catch (Exception ex)
        {
            cuz = new bl_micro_group_customer();
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Get(string fullName, Int32 idType, string idNumber) in class [da_micro_group_customer], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return cuz;
    }
    /// <summary>
    /// Get customber object by customer number
    /// </summary>
    /// <param name="customerNumber"></param>
    /// <returns></returns>
    public static bl_micro_group_customer Get(string customerNumber)
    {
        bl_micro_group_customer cuz = new bl_micro_group_customer();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET_BY_CUST_NO", new string[,] {
            { "@customer_number",customerNumber  }
            }, "da_micro_group_customer=>Get(string customerNumber)");
            if (db.RowEffect > 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    cuz.ID = r["customer_id"].ToString();
                    cuz.CUSTOMER_NUMBER = r["customer_no"].ToString();
                    cuz.FIRST_NAME_EN = r["first_name_en"].ToString();
                    cuz.LAST_NAME_EN = r["last_name_en"].ToString();
                    cuz.FIRST_NAME_KH = r["first_name_kh"].ToString();
                    cuz.LAST_NAME_KH = r["last_name_kh"].ToString();
                    cuz.GENDER = Convert.ToInt32(r["gender"].ToString());
                    cuz.DOB = Convert.ToDateTime(r["dob"].ToString());
                    cuz.ID_TYPE = Convert.ToInt32(r["id_type"].ToString());
                    cuz.ID_NUMBER = r["id_number"].ToString();
                    cuz.MARITAL_STATUS = r["marital_status"].ToString();
                    cuz.OCCUPATION = r["occupation"].ToString();
                    cuz.NATIONALITY = r["nationality"].ToString();
                }
                _SUCCESS = true;
            }
            else if (db.RowEffect == 0)
            {
                _SUCCESS = true;
                cuz = null;
            }
            else
            {
                _SUCCESS = false;
                cuz = null;
            }
        }
        catch (Exception ex)
        {
            cuz = null;// new bl_micro_group_customer();
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Get(string customerNumber)] in class [da_micro_group_customer], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return cuz;
    }
    /// <summary>
    /// Get existing customer in specific group code
    /// </summary>
    /// <param name="fullNameEn"></param>
    /// <param name="idType"></param>
    /// <param name="idNumber"></param>
    /// <param name="groupCode"></param>
    /// <returns></returns>
    public static bl_micro_group_customer Get(string fullNameEn, Int32 idType, string idNumber, string groupCode)
    {
        bl_micro_group_customer cuz = new bl_micro_group_customer();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET_BY_G", new string[,] {
            { "@full_name_en",fullNameEn  } ,{"@id_type",idType+""},{"@id_number",idNumber},{"@group_code",groupCode}
            }, "da_micro_group_customer=>Get(string fullNameEn, Int32 idType, string idNumber, string groupCode)");
            foreach (DataRow r in tbl.Rows)
            {
                cuz.ID = r["customer_id"].ToString();
                cuz.CUSTOMER_NUMBER = r["customer_no"].ToString();
                cuz.FIRST_NAME_EN = r["first_name_en"].ToString();
                cuz.LAST_NAME_EN = r["last_name_en"].ToString();
                cuz.FIRST_NAME_KH = r["first_name_kh"].ToString();
                cuz.LAST_NAME_KH = r["last_name_kh"].ToString();
                cuz.GENDER = Convert.ToInt32(r["gender"].ToString());
                cuz.DOB = Convert.ToDateTime(r["dob"].ToString());
                cuz.ID_TYPE = Convert.ToInt32(r["id_type"].ToString());
                cuz.ID_NUMBER = r["id_number"].ToString();
            }
        }
        catch (Exception ex)
        {
            cuz = new bl_micro_group_customer();
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Get(string fullNameEn, Int32 idType, string idNumber, string groupCode)] in class [da_micro_group_customer], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return cuz;
    }
    public static bl_micro_group_customer Get(Int32 idType, string idNumber, string groupCode)
    {
        bl_micro_group_customer cuz = new bl_micro_group_customer();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET_BY_ID_TYPE", new string[,] {
            {"@id_type",idType+""},{"@id_number",idNumber},{"@group_code",groupCode}
            }, "da_micro_group_customer=>Get(Int32 idType, string idNumber, string groupCode)");
            foreach (DataRow r in tbl.Rows)
            {
                cuz.ID = r["customer_id"].ToString();
                cuz.CUSTOMER_NUMBER = r["customer_no"].ToString();
                cuz.FIRST_NAME_EN = r["first_name_en"].ToString();
                cuz.LAST_NAME_EN = r["last_name_en"].ToString();
                cuz.FIRST_NAME_KH = r["first_name_kh"].ToString();
                cuz.LAST_NAME_KH = r["last_name_kh"].ToString();
                cuz.GENDER = Convert.ToInt32(r["gender"].ToString());
                cuz.DOB = Convert.ToDateTime(r["dob"].ToString());
                cuz.ID_TYPE = Convert.ToInt32(r["id_type"].ToString());
                cuz.ID_NUMBER = r["id_number"].ToString();
            }
        }
        catch (Exception ex)
        {
            cuz = new bl_micro_group_customer();
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Get(Int32 idType, string idNumber, string groupCode)] in class [da_micro_group_customer], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return cuz;
    }

    public static bl_micro_group_customer.LastSequence GetLastSEQ(string groupCode)
    {
        bl_micro_group_customer.LastSequence returnObj = new bl_micro_group_customer.LastSequence();
        try
        {
            DB db = new DB();
            int seq = 0;
            string[] cus = new string[] { };
            string currentseq = "";
            string custNo = "";
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET_LAST_SEQ", new string[,] { 
        {"@GROUP_CODE", groupCode}
        }, "da_micro_group_customer => GetLastSEQ(string groupCode)");
            if (db.RowEffect == -1)//error
            {
                seq = -1;
                returnObj = new bl_micro_group_customer.LastSequence() { SequenceNumber = -1, Prefix = "" };
            }
            else
            {
                if (tbl.Rows.Count > 0)
                {
                    seq = Convert.ToInt32(tbl.Rows[0]["seq"].ToString());
                    custNo = tbl.Rows[0]["customer_NO"].ToString();
                    cus = custNo.Split('-');
                    if (cus.Length > 0)
                    {
                        currentseq = cus[cus.Length - 1].ToString();
                        string prefix = currentseq.Substring(0, 2);
                       
                        returnObj = new bl_micro_group_customer.LastSequence()
                        {
                            SequenceNumber = seq,
                            Prefix = prefix
                        };
                    }

                }
                else
                {
                    returnObj = new bl_micro_group_customer.LastSequence() { SequenceNumber = 0, Prefix = "" };
                }
            }
        }
        catch (Exception ex)
        {
            returnObj = new bl_micro_group_customer.LastSequence() { SequenceNumber = -1, Prefix = "" };
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [GetLastSEQ(string groupCode)] in class [da_micro_group_customer], Detail:" + ex.Message);

        }
        return returnObj;
    }

    public class Contact
    {
        public static bool Save(bl_micro_group_customer_contact contact)
        {
            try
            {
                var a = contact;
                _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_CONTACT_INSERT", new string[,] 
            {
            {"@contact_id", a.CONTACT_ID},
            {"@customer_id",a.CUSTOMER_ID},
            {"@phone_number1", a.PHONE_NUMBER1},
            {"@phone_number2", a.PHONE_NUMBER2},
            {"@phone_number3", a.PHONE_NUMBER3},
            {"@email1", a.EMAIL1},
            {"@email2", a.EMAIL2},
            {"@email3", a.EMAIL3},
            {"@created_by", a.CREATED_BY},{"@created_on",a.CREATED_ON+""},{"@remarks",a.REMARKS}
            }, "da_micro_group_customer=>Contact=>Save(bl_micro_group_customer_contact contact)");
                if (!_SUCCESS)
                {
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [Save(bl_micro_group_customer_contact contact)] in class [da_micro_group_customer=>Contact], Detail:" + ex.Message);

            }
            return _SUCCESS;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public static bool Update(bl_micro_group_customer_contact contact, string contactId)
        {
            try
            {
                var a = contact;
                _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_CONTACT_UPDATE", new string[,] 
            {
            {"@contact_id", contactId},
            {"@customer_id",a.CUSTOMER_ID},
            {"@phone_number1", a.PHONE_NUMBER1},
            {"@phone_number2", a.PHONE_NUMBER2},
            {"@phone_number3", a.PHONE_NUMBER3},
            {"@email1", a.EMAIL1},
            {"@email2", a.EMAIL2},
            {"@email3", a.EMAIL3},
            {"@updated_by", a.UPDATED_BY},
            {"@updated_on",a.UPDATED_ON+""},
            {"@remarks",a.REMARKS}
            }, "da_micro_group_customer=>Contact=>Update(bl_micro_group_customer_contact contact, string contactId)");
                if (!_SUCCESS)
                {
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [Update(bl_micro_group_customer_contact contact, string contactId)] in class [da_micro_group_customer=>Contact], Detail:" + ex.Message);

            }
            return _SUCCESS;

        }

        public static bl_micro_group_customer_contact Get(string customerNumber)
        {
            bl_micro_group_customer_contact cont = new bl_micro_group_customer_contact();
            try
            {
                DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_CONTACT_GET_BY_CUST_NO", new string[,] { 
                {"@CUSTOMER_NUMBER",customerNumber}
                }, "a_micro_group_customer=>Contact=>Get(string customerNumber)");
                if (db.RowEffect < 0)
                {
                    _SUCCESS = false;
                    _MESSAGE = "Get Customer Contact is getting error.Please contact your system administrator.";
                    cont = null;
                }
                else if (db.RowEffect == 0)
                {
                    cont = null;
                    _SUCCESS = true;
                    _MESSAGE = "No record found.";

                }
                else
                {
                    var r = tbl.Rows[0];
                    cont.CONTACT_ID = r["contact_id"].ToString();
                    cont.CUSTOMER_ID = r["customer_id"].ToString();
                    cont.PHONE_NUMBER1 = r["phone_number1"].ToString();
                    cont.PHONE_NUMBER2 = r["phone_number2"].ToString();
                    cont.PHONE_NUMBER3 = r["phone_number3"].ToString();
                    cont.EMAIL1 = r["email1"].ToString();
                    cont.EMAIL2 = r["email2"].ToString();
                    cont.EMAIL3 = r["email3"].ToString();
                    _SUCCESS = true;
                    _MESSAGE = "Get customer contact successfully.";
                }

            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                cont = null;
                Log.AddExceptionToLog("Error function [Get(string customerNumber)] in class [da_micro_group_customer=>Contact], Detail:" + ex.Message);

            }
            return cont;
        }
    }
    public class Address
    {
        public static bool Save(bl_address address)
        {
            try
            {
                var a = address;
                _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_ADDRESS_INSERT", new string[,] 
            {
            {"@address_id", a.ADDRESS_ID},
            {"@customer_id",a.CUSTOMER_ID},
            {"@address1", a.ADDRESS1},
            {"@address2", a.ADDRESS2},
            {"@address3", a.ADDRESS3},
            {"@created_by", a.CREATED_BY},{"@created_on",a.CREATED_ON+""},{"@remarks",a.REMARKS}
            }, "da_micro_group_customer=>Address=>Save(bl_address address)");
                if (!_SUCCESS)
                {
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [Save(bl_address address) in class [da_micro_group_customer=>Address], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;

        }
        public static bool Update(bl_address address, string addressId)
        {
            try
            {
                var a = address;
                _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_ADDRESS_UPDATE", new string[,] 
            {
            {"@address_id", addressId},
            {"@customer_id",a.CUSTOMER_ID},
            {"@address1", a.ADDRESS1},
            {"@address2", a.ADDRESS2},
            {"@address3", a.ADDRESS3},
            {"@updated_by", a.UPDATED_BY},{"@updated_on",a.UPDATED_ON+""},{"@remarks",a.REMARKS}
            }, "da_micro_group_customer=>Address=> Update(bl_address address, string addressId)");
                if (!_SUCCESS)
                {
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [ Update(bl_address address, string addressId)] in class [da_micro_group_customer=>Address], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;

        }
        public static bl_address Get(string customerNumber)
        {
            bl_address ad = new bl_address();
            try
            {
                DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_ADDRESS_GET_BY_CUST_NO", new string[,] { 
                {"@CUSTOMER_NUMBER",customerNumber}
                }, "a_micro_group_customer=>Address=>Get(string customerNumber)");
                if (db.RowEffect < 0)
                {
                    _SUCCESS = false;
                    _MESSAGE = "Get Customer Address is getting error.Please contact your system administrator.";
                    ad = null;
                }
                else if (db.RowEffect == 0)
                {
                    ad = null;
                    _SUCCESS = true;
                    _MESSAGE = "No record found.";

                }
                else
                {
                    var r = tbl.Rows[0];
                    ad.ADDRESS_ID = r["address_id"].ToString();
                    ad.CUSTOMER_ID = r["customer_id"].ToString();
                    ad.ADDRESS1 = r["address1"].ToString();
                    ad.ADDRESS2 = r["address2"].ToString();
                    ad.ADDRESS3 = r["address3"].ToString();
                    ad.REMARKS = r["remarks"].ToString();
                    _SUCCESS = true;
                    _MESSAGE = "Get customer address successfully.";
                }

            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                ad = null;
                Log.AddExceptionToLog("Error function [Get(string customerNumber)] in class [da_micro_group_customer=>Address], Detail:" + ex.Message);

            }
            return ad;
        }


    }

    public class TempTable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">who is process uploaded</param>
        /// <param name="createdOn">Date of processing upload</param>
        /// <param name="groupMasterCode"></param>
        /// <returns></returns>
        public List<bl_micro_group_customer> GetCustomerList(string userName, DateTime createdOn, string groupMasterCode)
        {
            List<bl_micro_group_customer> listCust = new List<bl_micro_group_customer>();

            //try
            //{
            //    DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET", new string[,] {
            //{ "@full_name_en",fullNameEn  } ,{"@id_type",idType+""},{"@id_number",idNumber}
            //}, "da_micro_group_customer=>Get(string fullName, Int32 idType, string idNumber) ");
            //    foreach (DataRow r in tbl.Rows)
            //    {
            //        cuz.ID = r["customer_id"].ToString();
            //        cuz.CUSTOMER_NUMBER = r["customer_no"].ToString();
            //        cuz.FIRST_NAME_EN = r["first_name_en"].ToString();
            //        cuz.LAST_NAME_EN = r["last_name_en"].ToString();
            //        cuz.FIRST_NAME_KH = r["first_name_kh"].ToString();
            //        cuz.LAST_NAME_KH = r["last_name_kh"].ToString();
            //        cuz.GENDER = Convert.ToInt32(r["gender"].ToString());
            //        cuz.DOB = Convert.ToDateTime(r["dob"].ToString());
            //        cuz.ID_TYPE = Convert.ToInt32(r["id_type"].ToString());
            //        cuz.ID_NUMBER = r["id_number"].ToString();
            //    }
            //}
            //catch (Exception ex)
            //{ 

            //}

            return listCust;
        }
    }
}