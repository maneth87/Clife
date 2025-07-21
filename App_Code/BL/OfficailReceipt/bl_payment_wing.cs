using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public  class bl_payment_wing
    {
        public string Payment_Wing_ID { get; set; }
        public string Policy_ID { get; set; }
        public string Bill_No { get; set; }
        public string Transaction_ID { get; set; }
        public double Received_Amount { get; set; }
        public DateTime Received_Date { get; set; }
        public DateTime Created_On { get; set; }
        public string Created_By { get; set; }
        public int Status_Paid { get; set; }
        public int Check_Status { get; set; }
        public int Transaction_Type { get; set; }
        public string Created_Note { get; set; }
    }

