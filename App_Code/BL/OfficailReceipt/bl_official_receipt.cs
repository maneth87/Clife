using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public partial class bl_official_receipt
    {
        public string Official_Receipt_ID { get; set; }
        public string Policy_ID { get; set; }
        public string Customer_ID { get; set; }
        public string Receipt_No { get; set; }
        public int Payment_Type_ID { get; set; }
        public int Policy_Type { get; set; }
        public int Method_Payment { get; set; }
        public double Amount { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public string Created_Note { get; set; }
        public double Interest_Amount { get; set; }
        public DateTime Entry_Date { get; set; }

    }

