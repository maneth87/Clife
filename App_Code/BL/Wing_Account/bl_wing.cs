using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_wing_policy
/// </summary>
public class bl_wing
{
	public bl_wing()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Wing Policy
    public class policy : bl_ci.Policy /*inherit all properties from class bl_ci.Policy*/
    { 
        /*new properties*/
        public string Categories { get; set; }
        public string ConsentNumber { get; set; }
        public string FactoryName { get; set; }
        /// <summary>
        /// file location of consent form
        /// </summary>
        public string ConsentForm { get; set; }
        public string Remarks { get; set; }

        public class PolicyInfo : policy
        {
            #region Properties
            public string CusotmerID { get; set; }
            public string FirstNameEN { get; set; }
            public string LastNameEn { get; set; }
            public string FirstNameKH { get; set; }
            public string LastnameKH { get; set; }
            public string FullNameEn {

                get { 
                    return (LastNameEn + " " + FirstNameEN);
                }
            
            }
            public string FullNameKh {
                get
                { 
                    return (LastnameKH + " " + FirstNameKH);
                }
            }
            public string Gender { get; set; }
            public DateTime DOB { get; set; }
            public string IDType { get; set; }
            public string IDNumber { get; set; }
            public string PolicyDetailID { get; set; }
            public string PaymentMode { get; set; }
            public int SumAssured { get; set; }
            public double Premium { get; set; }
            public double UserPremium { get; set; }
            public double Originalpremium { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string PaymentCode { get; set; }
            public string PaymentBy { get; set; }
            public string PolicyStatus { get; set; }
            public DateTime EffectiveDate { get; set; }
            #endregion properties
        }
        

    }

    #endregion

    #region Wing Policy Detail
    public class PolicyDetail : bl_ci.PolicyDetail/*inherit all properties from class  bl_ci.PolicyDetail*/
    { 
        /*new properties*/
        public string TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Sequence { get; set; }
    }
    #endregion

    #region Wing Data Upload
    public class DataUpload : bl_ci_load_data /*inherit all properties from class bl_ci_load_data*/
    { 
        /*new properties*/
        /// <summary>
        /// This wing account number will be used as policy number in camlife
        /// </summary>
        public string WingAccNumber { get; set; }
        public string ConsentNumber { get; set; }
        public string FactoryName { get; set; }
        /// <summary>
        /// Remarks status while uploading data in to system
        /// </summary>
        public string Remarks { get; set; }
    }
    #endregion 

#region Save wing log
    /// <summary>
    /// 
    /// </summary>
    /// <param name="log_file">Log file name</param>
    /// <param name="message_log">log error message</param>
    public static void SaveLog(string log_file, string message_log)
    {
        Log.CreateLog(log_file, message_log);
    }
    #endregion
}