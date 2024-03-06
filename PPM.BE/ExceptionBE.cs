using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class ExceptionBE
    {
        public int ID { get; set; }
        public string Module_Name { get; set; }
        public string Page_URL { get; set; }
        public string Event_Name { get; set; }
        public string Message { get; set; }
        public string Exception_Details { get; set; }
        public DateTime Dated { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public string TimeStamp { get; set; }

    }
}
