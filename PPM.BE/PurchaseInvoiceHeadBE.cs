using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class PurchaseInvoiceHeadBE
    {
        public int ID { get; set; }
        public int InvoiceNo { get; set; }
        public int PumpID { get; set; }
        public string PumpCode { get; set; }
        public DateTime Dated { get; set; }
        public decimal Cash_Total { get; set; }
        public decimal Credit_Total { get; set; }
        public string Description { get; set; }
        public string Reference_No { get; set; }
        public string AddedBy { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        //properties for display purpose and API only

        public int UserID { get; set; }
        public string AccessKey { get; set; }
        public decimal TotalPurchase { get; set; }
        public List<PurchaseInvoiceDetailBE> ListofPurchaseDetails { get; set; }

        public PurchaseInvoiceHeadBE()
        {
            ListofPurchaseDetails = new List<PurchaseInvoiceDetailBE>();
        }

    }
}
