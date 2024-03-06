using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.Models
{
    public class User
    {
        public int user_id { get; set; }
        public int role_id { get; set; }
        public string user_name { get; set; }
        public string user_city_code { get; set; }
    }
}