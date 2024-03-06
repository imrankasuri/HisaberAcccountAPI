using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.Models
{
    public class GetProductBE
    {
        public int ID { get; set; }
        public int NoofProducts { get; set; }
        public string AccessKey { get; set; }
        public int MainCategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int BrandID { get; set; }
        public int ProductID { get; set; }
    }
}