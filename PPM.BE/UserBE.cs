using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class UserBE
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string User_Mobile { get; set; }
        public string User_Email { get; set; }
        public string Password { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public string User_Type { get; set; }
        public string Android_Token { get; set; }
        public Boolean Email_Verified { get; set; }
        public string Verification_Code { get; set; }
        public DateTime Verification_Expiry { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

    }
}
