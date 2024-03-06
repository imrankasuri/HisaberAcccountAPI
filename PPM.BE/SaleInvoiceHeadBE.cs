using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class SaleInvoiceHeadBE
    {
        public int ID { get; set; }
        public int InvoiceNo { get; set; }
        public int PumpID { get; set; }
        public string PumpCode { get; set; }
        public DateTime Dated { get; set; }
        public decimal Cash_Total { get; set; }
        public decimal Credit_Total { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public string TimeStamp { get; set; }


        //properties for display purpose only
        public decimal TotalSales { get; set; }
        public int UserID { get; set; }
        public string AccessKey { get; set; }

        public List<SaleInvoiceDetailBE> ListofSaleDetails { get; set; }
        public List<PumpReadingBE> ListofReadings { get; set; }
        public SaleInvoiceHeadBE()
        {
            ListofSaleDetails = new List<SaleInvoiceDetailBE>();
            ListofReadings = new List<PumpReadingBE>();
        }
    }
}
