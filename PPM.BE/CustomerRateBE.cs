using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class CustomerRateBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public string PumpCode { get; set; }
        public int Customer_ID { get; set; }
        public int? Product_ID { get; set; }
        public decimal? Selling_Rate { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public AccountBE Selected_Account { get; set; }
        public ProductBE Selected_Product { get; set; }
    }
}
