using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class SMSSentBE
    {
        public long ID { get; set; }
        public string SMS_Mask { get; set; }
        public string SMS_Text { get; set; }
        public string SMS_TO { get; set; }
        public string Sender_Department { get; set; }
        public string Sender_Name { get; set; }
        public int? Sender_ID { get; set; }
        public string Reference_No { get; set; }
        public Boolean Is_Approved { get; set; }
        public Boolean Is_Delivered { get; set; }
        public string Error_Code { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public string Response_Details { get; set; }
        public string Service_Provider { get; set; }
    }
}
