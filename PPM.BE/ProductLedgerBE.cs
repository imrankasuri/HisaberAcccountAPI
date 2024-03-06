using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class ProductLedgerBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public int Product_ID { get; set; }
        public DateTime Transaction_Date { get; set; }
        public int Reference_ID { get; set; }
        public string Reference_Type { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string BalanceType { get; set; }
        public string Description { get; set; }
        public string Vehicle_No { get; set; }
        public string Receipt_No { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public ProductBE Selected_Product { get; set; }
        public ProductLedgerBE()
        {
            Selected_Product = new ProductBE();
        }
    }
}
