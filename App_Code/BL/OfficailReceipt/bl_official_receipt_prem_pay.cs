using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public partial class bl_official_receipt_prem_pay
    {
        public string Official_Receipt_Prem_Pay_ID { get; set; }
        public string Official_Receipt_ID { get; set; }
        public string Policy_Prem_Pay_ID { get; set; }
        public string Product_ID { get; set; }
        public double Sum_Insured { get; set; }
        public double Amount { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public int Payment_Type_ID { get; set; }
    }

