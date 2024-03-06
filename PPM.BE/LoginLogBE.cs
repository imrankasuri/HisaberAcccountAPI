using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class LoginLogBE
    {
        public long ID { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public Boolean Is_Success { get; set; }
        public string IP_Address { get; set; }
        public string User_Type { get; set; }
        public string Login_Source { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
    }
}
