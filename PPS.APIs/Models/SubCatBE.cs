using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.Models
{
    public class SubCatBE
    {
        public int ID { get; set; }
        public int MainCatID { get; set; }
        public string AccessKey { get; set; }
    }
}