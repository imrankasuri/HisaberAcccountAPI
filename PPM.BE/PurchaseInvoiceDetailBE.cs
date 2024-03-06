using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class PurchaseInvoiceDetailBE
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
        public string Updatedby { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public ProductBE Product_BE { get; set; }
        public AccountBE Account_BE { get; set; }
        public PurchaseInvoiceHeadBE Purchase_InvoiceHead_BE { get; set; }
        public decimal Amount 
        { 
            get 
                { 
                   return  Price * Quantity;
                }
            set
            {
                Amount = value;
            }
        }
        public int Serial_No { get; set; }
        public PurchaseInvoiceDetailBE()
        {
            Product_BE = new ProductBE();
            Account_BE = new AccountBE();
            Purchase_InvoiceHead_BE = new PurchaseInvoiceHeadBE();
        }
    }
}
