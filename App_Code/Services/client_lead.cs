using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for client_lead
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class client_lead : System.Web.Services.WebService {

    public client_lead () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

  
    [WebMethod]
    public void SendLead(string BranchCode, string BranchName, string ApplicationId, string ReferralStaffId, string ReferralStaffName, string ReferralStaffPosition, string ClientType, string ClientCIF, string ClientNameENG, string ClientNameKHM, string ClientGender, string ClientNationality, string ClientDoB, string ClientVillage, string ClientCommune, string ClientDistrict, string ClientProvince, string DocumentType, string DocumentId, string ClientPhoneNumber, string Interest, string ReferredDate, string CreatedBy)
    {
        string LEAD_ID = "";
        MyResponse response = new MyResponse();

        Validate validate = new Validate();
        List<Validate> vaList = new List<Validate>();

        #region Validate
        if (BranchCode.Trim() == "")
        {
            validate = new Validate();
            validate.field = "BranchCode";
            validate.message = "Banch code cannot be blank.";

            vaList.Add(validate);
        }
        if (BranchName.Trim() == "")
        {
            validate = new Validate();
            validate.field = "BranchName";
            validate.message = "Banch name cannot be blank.";

            vaList.Add(validate);
        }
        if (ApplicationId.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ApplicationId";
            validate.message = "Application id cannot be blank.";

            vaList.Add(validate);
        }
        if (ReferralStaffId.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ReferralStaffId";
            validate.message = "Referral staff id cannot be blank.";

            vaList.Add(validate);
        }
        if (ReferralStaffName.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ReferralStaffName";
            validate.message = "Referral staff name cannot be blank.";

            vaList.Add(validate);
        }
        if (ReferralStaffPosition.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ReferralStaffPosition";
            validate.message = "Referral staff position cannot be blank.";

            vaList.Add(validate);
        }
        if (ClientCIF.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ClientCIF";
            validate.message = "Client CIF cannot be blank.";

            vaList.Add(validate);
        }
        else
        {
            if (ClientType.ToUpper().Trim() != "FAMILY" && ClientType == "")
            {
                validate = new Validate();
                validate.field = "ClientCIF";
                validate.message = "Client cif cannot be blank.";

                vaList.Add(validate);
            }
        }
        if (ClientNameENG.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ClientNameENG";
            validate.message = "Client name in english cannot be blank.";

            vaList.Add(validate);
        }
        if (ClientNameKHM.Trim() == "")
        {
            validate = new Validate();
            validate.field = "ClientNameKHM";
            validate.message = "Client name in khmer cannot be blank.";

            vaList.Add(validate);
        }
        if (ClientGender.Trim() == "")
        {
            vaList.Add(new Validate() { field = "ClientGender", message = "Gender cannot be blank." });
        }
        else
        {
            //if (GENDER.Trim().Length > 6)
            //{
            //    vaList.Add(new Validate() { FIELD = "GENDER", MESSAGE = "Size must be between 1 and 6" });

            //}
            //else
            //{
            //    if ( GENDER.Trim().ToUpper() != "MALE" || GENDER.Trim().ToUpper() != "FEMALE")
            //    {
            //        vaList.Add(new Validate() { FIELD = "GENDER", MESSAGE = "Gender must be Male or Female" });
            //    }
            //}

            if (ClientGender.Trim().ToUpper() != "MALE" && ClientGender.Trim().ToUpper() != "FEMALE")
            {
                vaList.Add(new Validate() { field = "GENDER", message = "Gender must be Male or Female" });
            }
        }
        if (ClientNationality.Trim().ToUpper() =="")
        {
            vaList.Add(new Validate() { field = "ClientNationality", message = "Nationality cannot be blank." });
        }
        if (ClientDoB.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ClientDoB", message = "Date of birth cannot be blank." });
        }
        else
        {
            if (!Helper.IsDate(ClientDoB))
            {
                vaList.Add(new Validate() { field = "ClientDoB", message = "Date of birth must be in format dd-MM-yyyy." });
            }
        }
        if (ClientVillage.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ClientVillage", message = "Village cannot be blank." });
        }
        if (ClientCommune.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ClientCommune", message = "Commune cannot be blank." });
        }
        if (ClientDistrict.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ClientDistrict", message = "District cannot be blank." });
        }
        if (ClientProvince.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ClientProvince", message = "Province cannot be blank." });
        }
        if (DocumentType.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "DocumentType", message = "Document type cannot be blank." });
        }
        if (DocumentId.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "DocumentType", message = "Document id number cannot be blank." });
        }
        if (ClientPhoneNumber.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ClientPhoneNumber", message = "Phone number cannot be blank." });
        }
        
        else
        {
            int num;
            var isNumeric = int.TryParse(ClientPhoneNumber, out num);

            if (!isNumeric)
            {
                vaList.Add(new Validate() { field = "ClientPhoneNumber", message = "Phone number cannot be number only." });
            }
        }
        if (Interest.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "Interest", message = "Interest cannot be blank." });
        }
        if (ReferredDate.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "ReferredDate", message = "Referred date cannot be blank." });
        }
        else
        {
            if (!Helper.IsDate(ReferredDate))
            {
                vaList.Add(new Validate() { field = "ReferredDate", message = "Referred date must be in format dd-MM-yyyy." });
            }
        }
        if (CreatedBy.Trim().ToUpper() == "")
        {
            vaList.Add(new Validate() { field = "CreatedBy", message = "Created by cannot be blank." });
        }
        #endregion

        //Valid Data
        if (vaList.Count == 0)
        {
            try
            {
              
                bl_customer_lead obj_lead;
              
                //CHECK EXISING RECORD BY APPLICATION ID (HTB APPLICATION ID)
                bl_customer_lead exist_obj = new bl_customer_lead();
                exist_obj = da_customer_lead.GetCustomerLeadByApplicationID(ApplicationId);

                if (exist_obj.ApplicationID != null)
                {
                    //Upate
                    obj_lead = new bl_customer_lead() { BranchCode = BranchCode, BranchName = BranchName, ApplicationID = ApplicationId, ReferralStaffId = ReferralStaffId, ReferralStaffName = ReferralStaffName, ReferralStaffPosition = ReferralStaffPosition, ClientType = ClientType, ClientCIF = ClientCIF, ClientNameENG = ClientNameENG, ClientNameKHM = ClientNameKHM, ClientGender = ClientGender, ClientNationality = ClientNationality, ClientDoB = Helper.FormatDateTime(ClientDoB), ClientVillage = ClientVillage, ClientCommune = ClientCommune, ClientDistrict = ClientDistrict, ClientProvince = ClientProvince, DocumentType = DocumentType, DocumentId = DocumentId, ClientPhoneNumber = ClientPhoneNumber, Interest = Interest, ReferredDate = Helper.FormatDateTime(ReferredDate), CreatedBy = CreatedBy, CreatedOn     = DateTime.Now, Status = "", Remarks = "", StatusRemarks = "", UpdatedBy = CreatedBy, UpdatedOn = DateTime.Now, ID = exist_obj.ID };

                    LEAD_ID = da_customer_lead.UpdateCustomerLead(obj_lead);
                }
                else
                { 
                    //save
                    obj_lead = new bl_customer_lead() { BranchCode = BranchCode, BranchName = BranchName, ApplicationID = ApplicationId, ReferralStaffId = ReferralStaffId, ReferralStaffName = ReferralStaffName, ReferralStaffPosition = ReferralStaffPosition, ClientType = ClientType, ClientCIF = ClientCIF, ClientNameENG = ClientNameENG, ClientNameKHM = ClientNameKHM, ClientGender = ClientGender, ClientNationality = ClientNationality, ClientDoB = Helper.FormatDateTime(ClientDoB), ClientVillage = ClientVillage, ClientCommune = ClientCommune, ClientDistrict = ClientDistrict, ClientProvince = ClientProvince, DocumentType = DocumentType, DocumentId = DocumentId, ClientPhoneNumber = ClientPhoneNumber, Interest = Interest, ReferredDate = Helper.FormatDateTime(ReferredDate), CreatedBy = CreatedBy, CreatedOn = DateTime.Now, Status = "", Remarks = "", StatusRemarks = "" };

                    LEAD_ID = da_customer_lead.InsertCustomerLead(obj_lead);
                }
                
              
                string m = da_customer_lead.MESSAGE;

                if (LEAD_ID != "")
                {
                    //response.STATUS = "200";
                   // response.CODE = "1000";
                    response.message     = "Success";
                    response.detail = new MyResponse1() { ApplicationId = obj_lead.ApplicationID };
                    var jsonString = JsonConvert.SerializeObject(response);

                   // ResponseJson(jsonString);
                   // ResponseDailyInsurance(jsonString);

                    //HttpContext.Current.Response.Clear();
                    //HttpContext.Current.Response.ContentType = "application/json";
                    ////HttpContext.Current.Response.AddHeader("content-length", jsonString.Length.ToString());
                    //HttpContext.Current.Response.Write(jsonString);
                    //HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.End();

                    Context.Response.Write(jsonString);
                  
                }
                else
                {
                    //Error err = new Error() { STATUS = "400", CODE = "1007", MESSAGE = m };
                    Error err = new Error() {  message = m };
                   // ResponseJson(JsonConvert.SerializeObject(err));
                 Context.Response.Write (JsonConvert.SerializeObject(err));
                }

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [SaveLead(string BRANCH_CODE, string BRANCH_NAME, string APPLICATION_ID, string REFERRAL_STAFF_ID, string REFERRAL_STAFF_NAME, string REFERRAL_STAFF_POSITION, string CLIENT_TYPE, string CIF, string CLIENT_NAME_IN_ENGLISH, string CLIENT_NAME_IN_KHMER, string GENDER, string NATIONALITY, string DATE_OF_BIRTH, string VILLAGE, string COMMUNE, string DISTRICT, string PROVINCE, string ID_TYPE, string ID_NUMBER, string PHONE_NUMBER, string INTEREST, string REFERRED_DATE, string CREATED_BY)] in call [client_lead], detail: " + ex.Message + " ==> " + ex.StackTrace);
                //Error err = new Error() { STATUS = "400", CODE = "1007", MESSAGE = ex.Message };
                Error err = new Error() {  message = ex.Message };
                //ResponseJson(JsonConvert.SerializeObject(err));
                Context.Response.Write(JsonConvert.SerializeObject(err));
            }
            
        }
        else
        {
            //response validate
            MyResponseValidate response1 = new MyResponseValidate();
            //response1.STATUS = "500";
           // response1.CODE = "1005";
            response1.message = "Validation error, The request has " + vaList.Count + " errors.";
            response1.detail=vaList;
           // ResponseJson(JsonConvert.SerializeObject(response1));

            Context.Response.Write(JsonConvert.SerializeObject(response1));
        }
    }

    public class MyResponse 
    {
        //public string STATUS{get;set;}
        //public string CODE { get; set; }
        public string status { get { return "200"; } }
        public string code { get { return "1000"; } }
        public string message { get; set; }

        //public MyResponse1 DETAIL { get; set; }
        public object detail { get; set; }
    }

    public class MyResponseValidate
    {
        
        public string status {
            get {
                return "500";
            }
        }
        public string code { get { return "1005"; } }
        public string message { get; set; }

        public List<Validate> detail { get; set; }
    }
    public class Validate
    {
        public string field { get; set; }
        public string message { get; set; }
    }
    public class MyResponse1
    {
        public string ApplicationId { get; set; }
    }
   
    public class Error
    {
        
        //public string STATUS { get; set; }
        //public string CODE { get; set; }
        public string status { get{return "400";} }
        public string code { get{return "1007";} }

        public string message { get; set; }
    }

    [WebMethod]
    public void GetLead()
    {
        List<bl_customer_lead> list_obj = new List<bl_customer_lead>();

        list_obj = da_customer_lead.GetCustomerLead();
        ResponseJson(JsonConvert.SerializeObject(list_obj));
    }

    [WebMethod]
    public void UpdateLeadStatus(string lead_id, string new_status, string status_remarks, string updated_by)
    {
        bool result = da_customer_lead.UpdateCustomerLeadStatus(new_status, status_remarks, lead_id, updated_by, DateTime.Now);
        string message = da_customer_lead.MESSAGE;
        if (result)
        {
         ResponseJson(JsonConvert.SerializeObject(new MyResponse() {  message = "Success", detail = new MyResponse1() { ApplicationId = lead_id } }));
        }
        else
        {
            ResponseJson(JsonConvert.SerializeObject(new Error { message = message }));

        }
    
    }

    [WebMethod]
    public void GetDailyInsuranceBookingHTB(string StartDate, string ToDate)
    {
        MyResponseValidate resValidate= new MyResponseValidate();
       
        List<Validate> listValidate = new List<Validate>();
        if (StartDate.Trim() == "")
        {
            listValidate.Add(new Validate() { field = "StartDate", message = "Start date cannot be blank." });

        }
        else
        {
            if (!Helper.IsDate(StartDate))
            {
                listValidate.Add(new Validate() { field = "StartDate", message = "Start date must be in format dd-MM-yyyy." });

            }
        }
        if (ToDate.Trim() == "")
        {
            listValidate.Add(new Validate() { field = "ToDate", message = "To date cannot be blank." });

        }
        else
        {
            if (!Helper.IsDate(ToDate))
            {
                listValidate.Add(new Validate() { field = "ToDate", message = "To date must be in format dd-MM-yyyy." });

            }
        }


        if (listValidate.Count == 0)
        {
            DateTime s_date = Helper.FormatDateTime(StartDate);
            DateTime t_date = Helper.FormatDateTime(ToDate);
            List<bl_daily_insurance_booking_htb> objList = new List<bl_daily_insurance_booking_htb>();
            objList=da_banca.GetDailyInsuranceBookingHTB(s_date, t_date);
            
            if (da_banca.SUCCESS)
            {
                if (objList.Count > 0)
                {
                   //ResponseJson(JsonConvert.SerializeObject(new MyResponse() { message = da_banca.MESSAGE, detail = objList }));
                    Context.Response.Write (JsonConvert.SerializeObject(new MyResponse() { message = da_banca.MESSAGE, detail = objList }));
                }
                else
                {
                    objList = new List<bl_daily_insurance_booking_htb>();
                    //ResponseJson(JsonConvert.SerializeObject(new MyResponse() { message = da_banca.MESSAGE, detail = objList }));
                    Context.Response.Write(JsonConvert.SerializeObject(new MyResponse() { message = da_banca.MESSAGE, detail = objList }));
                }
            }
            else
            {
                //ResponseJson(JsonConvert.SerializeObject(new Error() { message = da_banca.MESSAGE }));
                Context.Response.Write(JsonConvert.SerializeObject(new Error() { message = da_banca.MESSAGE }));
            }
            
        }
        else
        {
            MyResponseValidate response1 = new MyResponseValidate();
            
            response1.message = "Validation error, The request has " + listValidate.Count + " errors.";
            response1.detail = listValidate;
            //ResponseJson(JsonConvert.SerializeObject(response1));
            Context.Response.Write(JsonConvert.SerializeObject(response1));
        }
    }
    [WebMethod]
    public void GetApplicationConsumer(string InsuranceApplicationId)
    {
        string jSon = "";
        List<Validate> vaList = new List<Validate>();
        if (InsuranceApplicationId.Trim() == "")
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id cannot be blank." });
        }
        else if (InsuranceApplicationId.Trim().Length != 10)
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Size must be 10 degits." });

        }
        else if (InsuranceApplicationId.Trim().Substring(0, 3) != "APP")
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id must be in format APP{number 7 degits}." });
        }
        else if (!Helper.IsNumeric(InsuranceApplicationId.Trim().Substring(3, 7)))
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id must be in format APP{number 7 degits}." });
        }
        if (vaList.Count == 0)
        {
            da_banca.ApplicationConsumer obj = new da_banca.ApplicationConsumer();
            obj = da_banca.ApplicationConsumer.GetApplicationConsumer(InsuranceApplicationId);
           //obj = new da_banca.ApplicationConsumer();
            List<da_banca.ApplicationConsumer> objList = new List<da_banca.ApplicationConsumer>();
            objList.Add(obj);
           // objList = new List<da_banca.ApplicationConsumer>();
            if (da_banca.SUCCESS)
            {
                if ( obj.InsuranceApplicationId ==null)
                {
                    objList = new List<da_banca.ApplicationConsumer>();
                }
                
               jSon = JsonConvert.SerializeObject(new MyResponse() { message = da_banca.MESSAGE, detail = objList });
                
              // ResponseJson(jSon);

               Context.Response.Write(jSon);
               
            }
            else
            {
               // Context.Response.Write(JsonConvert.SerializeObject(new Error() { MESSAGE = da_banca.MESSAGE }));
                jSon = JsonConvert.SerializeObject(new Error() { message = da_banca.MESSAGE });
               // ResponseJson(jSon);
                Context.Response.Write(jSon);
            }

        }
        else
        {
            //Context.Response.Write(JsonConvert.SerializeObject(new MyResponseValidate() { MESSAGE = "Validation error, The request has " + vaList.Count + " errors.", DETAIL = vaList }));
            jSon = JsonConvert.SerializeObject(new MyResponseValidate() { message = "Validation error, The request has " + vaList.Count + " errors.", detail = vaList });
           //ResponseJson(jSon);
            Context.Response.Write(jSon);
        }
      
        


       // Context.Response.Write(jSon);
       
    }
    [WebMethod]
    public void SendPaymentList1(string BranchCode, string BranchName, string PaymentReferenceNo, string TransactionType, string InsuranceApplicationId, string ClientNameENG, string Currency, string Premium, string PaymentDate)
    {
        List<Validate> vaList = new List<Validate>();
        if (BranchCode.Trim() == "")
        {

            vaList.Add(new Validate() { field = "BranchCode", message = "Banch code cannot be blank."});
        }
        if (BranchName.Trim() == "")
        {
            vaList.Add(new Validate() { field = "BranchName", message = "Banch name cannot be blank." });
        }
        if (PaymentReferenceNo.Trim() == "")
        {
            vaList.Add(new Validate() { field = "PaymentReferenceNo", message = "Payment reference no cannot be blank." });
        }
        if (TransactionType.Trim() == "")
        {
            vaList.Add(new Validate() { field = "TransactionType", message = "Transaction cannot be blank." });
        }
        if (InsuranceApplicationId.Trim() == "")
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id cannot be blank." });
        }

        else if (InsuranceApplicationId.Trim().Length != 10)
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Size must be 10 degits." });

        }
        else if (InsuranceApplicationId.Trim().Substring(0, 3) != "APP")
        {
            vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id must be in format APP{number 7 degits}." });
        }
        if (ClientNameENG.Trim() == "")
        {
            vaList.Add(new Validate() { field = "ClientNameENG", message = "Client name cannot be blank." });
        }
        if (Currency.Trim() == "")
        {
            vaList.Add(new Validate() { field = "Currency", message = "Currency cannot be blank." });
        }
        if (Premium.Trim() == "")
        {
            vaList.Add(new Validate() { field = "Premium", message = "Premium cannot be blank." });
        }else if (!Helper.IsNumeric(Premium.Trim()))
        {
            vaList.Add(new Validate() { field = "Premium", message = "Premium must be IsNumeric." });
        }
        if (PaymentDate.Trim() == "")
        {
            vaList.Add(new Validate() { field = "PaymentDate", message = "Payment date cannot be blank." });
        }else if(!Helper.IsDate(PaymentDate.Trim()))
        {
            vaList.Add(new Validate() { field = "PaymentDate", message = "Payment date must be in format dd-MM-yyyy." });
        }

        if (vaList.Count == 0)
        {
            try
            {
                string created_by = "HTB";
                DateTime created_on = DateTime.Now;
                double premium_ = 0;
                string remarks = "";
                premium_ = Convert.ToDouble(Premium);
                DateTime payment_date_ = Helper.FormatDateTime(PaymentDate);
                da_banca.PaymentHTB.SavePaymentHTB(new da_banca.PaymentHTB()
                {
                     BranchCode = BranchCode, BranchName=BranchName, PaymentReferenceNo=PaymentReferenceNo, TransactionType = TransactionType, InsuranceApplicationId= InsuranceApplicationId,ClientNameENG = ClientNameENG, Premium = premium_, CreatedBy=created_by, CreatedOn = created_on, Remarks=remarks, Currency= Currency , PaymentDate=payment_date_

                });
                if (da_banca.SUCCESS)
                {
                    var jSon=(JsonConvert.SerializeObject(new MyResponse() { message="Success", detail = new string[]{} }));
                    //HttpContext.Current.Response.Clear();
                    //HttpContext.Current.Response.ContentType = "application/json";
                    //HttpContext.Current.Response.AddHeader("content-length", "1000000");
                    //HttpContext.Current.Response.Write(jSon);
                    //HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.End();
                    ResponseJson(jSon);
                }
                else
                {
                    ResponseJson(JsonConvert.SerializeObject(new Error() { message= da_banca.MESSAGE }));
                }
            }
            catch (Exception ex)
            {
                ResponseJson(JsonConvert.SerializeObject(new Error() { message = ex.Message }));
            }
        }
        else
        {
            ResponseJson(JsonConvert.SerializeObject(new MyResponseValidate() { message = "Validation error, The request has " + vaList.Count + " errors.", detail = vaList }));
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendPaymentList(List<da_banca.PaymentHTBObjectString> PaymentList)
    {
        List<da_banca.PaymentHTB> result = new List<da_banca.PaymentHTB>();
         List<Error> listError = new List<Error>();
         List<MyResponse> listSuccess = new List<MyResponse>();
         #region Validate
         List<Validate> vaList = new List<Validate>();
         int index = 0;
         foreach (da_banca.PaymentHTBObjectString obj in PaymentList)
         {
             
             
             if (obj.BranchCode.Trim() == "")
             {

                 vaList.Add(new Validate() { field = "BranchCode", message = "Banch code cannot be blank." + " [Row Index: " + index + "]." });
             }
             if (obj.BranchName.Trim() == "")
             {
                 vaList.Add(new Validate() { field   = "BranchName", message = "Banch name cannot be blank." + " [Row Index: " + index + "]." });
             }
             if (obj.PaymentReferenceNo.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "PaymentReferenceNo", message = "Payment reference no cannot be blank." + " [Row Index: " + index + "]." });
             }
             if (obj.TransactionType.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "TransactionType", message = "Transaction cannot be blank." + " [Row Index: " + index + "]." });
             }
             if (obj.InsuranceApplicationId.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id cannot be blank." + " [Row Index: " + index + "]." });
             }

             else if (obj.InsuranceApplicationId.Trim().Length != 10)
             {
                 vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Size must be 10 degits." + " [Row Index: " + index + "]." });

             }
             else if (obj.InsuranceApplicationId.Trim().Substring(0, 3) != "APP")
             {
                 vaList.Add(new Validate() { field = "InsuranceApplicationId", message = "Insurance application id must be in format APP{number 7 degits}." + " [Row Index: " + index + "]." });
             }
             if (obj.ClientNameENG.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "ClientNameENG", message = "Client name cannot be blank." + " [Row Index: " + index + "]." });
             }
             if (obj.Currency.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "Currency", message = "Currency cannot be blank." + " [Row Index: " + index + "]." });
             }
             if (obj.Premium.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "Premium", message = "Premium cannot be blank." + " [Row Index: " + index + "]." });
             }
             else if (!Helper.IsNumeric(obj.Premium.Trim()))
             {
                 vaList.Add(new Validate() { field = "Premium", message = "Premium must be IsNumeric." + " [Row Index: " + index + "]." });
             }
             if (obj.PaymentDate.Trim() == "")
             {
                 vaList.Add(new Validate() { field = "PaymentDate", message = "Payment date cannot be blank." + " [Row Index: " + index + "]." });
             }
             else if (!Helper.IsDate(obj.PaymentDate.Trim()))
             {
                 vaList.Add(new Validate() { field = "PaymentDate", message = "Payment date must be in format dd-MM-yyyy." + " [Row Index: " + index + "]." });
             }
             index += 1;
         }
#endregion 
        //reset index
         int fail = 0; int success = 0;
         int total_record = PaymentList.Count;
         List<PaymentList> errList = new List<PaymentList>();

         if (vaList.Count == 0)
         {
             foreach (da_banca.PaymentHTBObjectString obj in PaymentList)
             {
                 try
                 {
                     DateTime payment_date_ = Helper.FormatDateTime( obj.PaymentDate);
                     DateTime created_on = DateTime.Now;
                     string created_by = "HTB";
                     double premium = Convert.ToDouble(obj.Premium);
                     da_banca.PaymentHTB.SavePaymentHTB(new da_banca.PaymentHTB()
                     {
                         BranchCode = obj.BranchCode,
                         BranchName = obj.BranchName,
                         PaymentReferenceNo = obj.PaymentReferenceNo,
                         TransactionType = obj.TransactionType,
                         InsuranceApplicationId = obj.InsuranceApplicationId,
                         ClientNameENG = obj.ClientNameENG,
                         Premium = premium,
                         CreatedBy = created_by,
                         CreatedOn = created_on,
                         Remarks = "",
                         Currency = obj.Currency,
                         PaymentDate = payment_date_

                     });
                     if (da_banca.SUCCESS)
                     {
                         success += 1;
                        
                        // listSuccess.Add(new MyResponse() { MESSAGE = "Success", DETAIL = "PAYMENT_REFERENCE_NO:" + obj.PAYMENT_REFERENCE_NO });
                     }
                     else
                     {
                         fail += 1;
                         //listError.Add(new Error() { MESSAGE = da_banca.MESSAGE });
                         errList.Add(new PaymentList()
                         {
                             BranchCode = obj.BranchCode,
                             BranchName = obj.BranchName,
                             PaymentReferenceNo = obj.PaymentReferenceNo,
                             TransactionType = obj.TransactionType,
                             InsuranceApplicationId = obj.InsuranceApplicationId,
                             ClientNameENG = obj.ClientNameENG,
                             Currency = obj.Currency,
                             PaymentDate = obj.PaymentDate,
                             Premium = obj.Premium,
                             ErrorMessage = da_banca.MESSAGE
                         });
                     }
                 }
                 catch (Exception ex)
                 {
                     fail += 1;
                     //listError.Add(new Error() { MESSAGE = ex.Message });
                     errList.Add(new PaymentList()
                     {
                         BranchCode = obj.BranchCode,
                         BranchName = obj.BranchName,
                         PaymentReferenceNo = obj.PaymentReferenceNo,
                         TransactionType = obj.TransactionType,
                         InsuranceApplicationId = obj.InsuranceApplicationId,
                         ClientNameENG = obj.ClientNameENG,
                         Currency = obj.Currency,
                         PaymentDate = obj.PaymentDate,
                         Premium = obj.Premium,
                         ErrorMessage = ex.Message
                     });


                 }
                 
             }
             string json_string="";
            
             if (success > 0 && fail == 0)//success all
             {
                 ResponsePaymentListSuccessAll objSucc = new ResponsePaymentListSuccessAll() {
                     status = "200",
                     code = "1000",
                     message = "Success",
                     TotalRecords = total_record,
                     TotalSuccessRecords=success,
                     TotalfailRecords=fail
                 };
                 json_string = JsonConvert.SerializeObject(objSucc);
             }
             else if (fail > 0 ) //fail all
             {
                 ResponsePaymentListError objErr = new ResponsePaymentListError() {
                     status = "400",
                     code = "1007",
                     message = "Fail",
                     TotalRecords = total_record,
                     TotalSuccessRecords=success,
                     TotalfailRecords=fail,
                     detail = errList
                 };
                 json_string = JsonConvert.SerializeObject(objErr);
             }
             //else if (success > 0 & fail > 0)// succes and fail
             //{
             //    ResponsePaymentListSuccess obj1 = new ResponsePaymentListSuccess();
             //    obj1.STATUS = "200";
             //    obj1.CODE = "1000";
             //    obj1.MESSAGE = "Success";
             //    obj1.TOTAL_RECORDS = success;
             //    obj1.ERROR = new ResponsePaymentListError()
             //    {
             //        STATUS = "400",
             //        CODE = "1007",
             //        MESSAGE = "Success",
             //        TOTAL_RECORDS = fail,
             //        DETAIL = errList

             //    };
             //    json_string = JsonConvert.SerializeObject(obj1);
             //}
             //ResponseJson(JsonConvert.SerializeObject(listSuccess));
             ResponseJson(json_string);
         }
         else//validate error
         {
             ResponseJson(JsonConvert.SerializeObject(new MyResponseValidate() { message = "Validation error, The request has " + vaList.Count + " errors.", detail = vaList }));
         
         }
        
    }

    #region response paymentlist

   
    public class ResponsePaymentListSuccessAll
    {
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public int TotalRecords { get; set; }
        public int TotalSuccessRecords { get; set; }
        public int TotalfailRecords { get; set; }
    }
    public class ResponsePaymentListError 
    {
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public int TotalRecords { get; set; }
        public int TotalSuccessRecords { get; set; }
        public int TotalfailRecords { get; set; }
        public List<PaymentList> detail { get; set; }

    }
  
    public class PaymentList
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string PaymentReferenceNo { get; set; }
        public string TransactionType { get; set; }
        public string InsuranceApplicationId { get; set; }
        public string ClientNameENG { get; set; }
        public string Currency { get; set; }
        public string Premium { get; set; }
        public string PaymentDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion

    void ResponseJson(string jSon)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json";
        //HttpContext.Current.Response.AddHeader("content-length", "8888888");
        HttpContext.Current.Response.Write(jSon);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
        
    }
    void ResponseDailyInsurance(string jSon)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json";
        //HttpContext.Current.Response.AddHeader("content-length", "8888888");
        HttpContext.Current.Response.Write(jSon);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
}
