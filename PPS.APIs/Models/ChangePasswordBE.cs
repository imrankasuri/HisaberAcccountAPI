using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.Models
{
    public class ChangePasswordBE
    {
        public string ID { get; set; }
        public string AccessKey { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
}