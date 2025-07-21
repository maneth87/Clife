using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_group_policy_payment_bunch
/// </summary>
public class bl_micro_group_policy_payment_bunch
{
	public bl_micro_group_policy_payment_bunch()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public class summary
    {
        private string _id;
        private int _bunchNumber = 0;
        public summary()
        {

            _id = GetId();
            _bunchNumber = GetLastBunchNo();


            if (Remarks == null)
                Remarks = "";

        }

        public string BunchId { get { return _id; } set { _id = value; } }
        public Int32 BunchNumber { get { return _bunchNumber; } set { _bunchNumber = value; } }
        public string GroupMasterCode { get; set; }
        public double Amount { get; set; }
        public double DisountAmount { get; set; }
        public double TotalAmount { get; set; }
        public double ReturnAmount { get; set; }
        public Int32 NumberPolicy { get; set; }
        public DateTime ReportDate { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string PaymentType { get; set; }
        public string CreatedBy{get;set;}
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }


        private string GetId()
        {
            return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_SUMMARY" }, { "FIELD", "BUNCH_ID" } });
        }
        private Int32 GetLastBunchNo()
        { 
            
         Int32 no = 0;
            DB db =new DB();
            System.Data.DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_SUMMARY_GET_LAST_BUNCH_NUMBER", new string[,] {  }, "bl_micro_group_policy_payment_bunch => summary");
            if (db.RowEffect > 0)
            {
                no = Convert.ToInt32(tbl.Rows[0][0].ToString()) +1;
            }
            else if (db.RowEffect == 0)
            {
                no = 1;
            }
            else
            {
                no = 0; //error while geting last number
            }
            return no;
        }
    }


    public class detail
    {
        private string _id;

        public detail()
        {
            _id = GetId();
        }
        public string BunchDetailId { get { return _id; } set { _id = value; } }
        public string BunchId { get; set; }
        public string PolicyPaymentId { get; set; }

        private string GetId()
        {
            return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_DETAIL" }, { "FIELD", "BUNCH_DETAIL_ID" } });
        }
    }


    public class TranDetail : detail
    {
        public TranDetail() { }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TranType { get; set; }
    }
    public class TranSummary : summary
    {
        public TranSummary() {
            if (UpdatedBy == null)
                UpdatedBy = "";
            if (UpdatedOn == null)
                UpdatedOn = new DateTime(1900, 1, 1);
            if (Remarks == null)
                Remarks = "";
        }
        public string TranBy { get; set; }
        public DateTime TranDate { get; set; }
        public string TranType { get; set; }
    }
    public class SummaryReport
    {
        public SummaryReport()
        {
    
        }
        public string ChannelId { get; set; }
        public string ChannelItemId { get; set; }
        public string ChannelLocationId { get; set; }
        public string ChannelName { get; set; }
        public string GroupMasterCode { get; set; }
        public string BunchId { get; set; }
        public Int32 BunchNumber { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public double ReturnAmount { get; set; }
        DateTime ReportDate { get; set; }
        public Int32 Status { get; set; }
        public string Remarks { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
    public class DetailReport
    {
        public DetailReport()
        {

        }


    }
}