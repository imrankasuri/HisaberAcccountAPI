using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class SaleInvoiceDetailBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public string PumpCode { get; set; }
        public int Invoice_ID { get; set; }
        public int Product_ID { get; set; }
        public int Account_ID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public Boolean Is_Cash { get; set; }
        public string Vehicle_No { get; set; }
        public string Receipt_No { get; set; }
        public decimal Purchase_Price { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public ProductBE Product_BE { get; set; }
        public AccountBE Account_BE { get; set; }
        public SaleInvoiceHeadBE Sale_Invoice_Head_BE { get; set; }
        public decimal Amount 
        { 
            get
            {
                return  Price * Quantity;
            } 
        }
        public int Serial_No { get; set; }
        public decimal Profit { get; set; }
        public SaleInvoiceDetailBE()
        {
            Product_BE = new ProductBE();
            Account_BE = new AccountBE();
            Sale_Invoice_Head_BE = new SaleInvoiceHeadBE();
        }
    }
}
