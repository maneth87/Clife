using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_group_invioce
/// </summary>
public class bl_micro_group_invoice
{
    public bl_micro_group_invoice()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Return Invioce Number with formation [CLI-INV{5-digits}/{MM}/{YY}
    /// </summary>
    /// <param name="invNo"></param>
    /// <param name="invDate"></param>
    /// <returns></returns>
    public static string GenerateInvoiceNumber(int invNo, DateTime invDate)
    {
        //CLI-INV{000001/{MM/YY}}  
        string str = string.Format("CLI-INV{0}/{1}/{2}", invNo.ToString("00000"), invDate.ToString("MM"), invDate.ToString("yy"));
        return str;
    }

    public class summary
    {
        string _id;
        Int32 _invoiceNo = 0;
        public summary()
        {
            _id = GetId();
           // _invoiceNo = GetLastInvoiceNo();

            if (Remarks == null)
                Remarks = "";

        }
        public string InvoiceId { get { return _id; } set { _id = value; } }
        //public Int32 InvoiceNumber { get { return _invoiceNo; } set { _invoiceNo = value; } }
        public string InvoiceNumber { get; set; }
        /// <summary>
        /// Official receipt will be updated after issued official receipt
        /// </summary>
        public string OfficialRecieptId { get; set; }
        public string GroupMasterCode { get; set; }
        public double Amount { get; set; }
        public double DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public Int32 NumberPolicy { get; set; }
        public DateTime InvoiceDate { get; set;}
        public double ExchangeRateTax { get; set; }
        public double TotalAmountKh { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        private string GetId()
        {
            return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_INVOICE_SUMMARY" }, { "FIELD", "INVOICE_ID" } });
        }

        private Int32 GetLastInvoiceNo()
        {
            Int32 no = 0;
            DB db =new DB();
            System.Data.DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_SUMMARY_GET_LAST_NUMBER", new string[,] {  }, "bl_micro_group_invoice => summary");
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
        string _id;
        public detail()
        {
            _id = GetId();
          
        }
        public string InvoiceDetailId { get { return _id; } set { _id = value; } }
        public string InvoiceId { get; set; }
        public string BunchDetailId { get; set; }
        public string BunchId { get; set; }
      
        private string GetId()
        {
            return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_INVOICE_DETAIL" }, { "FIELD", "INVOICE_DETAIL_ID" } });
        }
    }
}