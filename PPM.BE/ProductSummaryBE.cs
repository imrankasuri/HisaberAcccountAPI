using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class ProductSummaryBE
    {
        public int SerialNo { get; set; }
        public DateTime Dated { get; set; }
        public string ProductName { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal OpeningStockValue { get; set; }
        public decimal Purchase { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal Sales { get; set; }
        public decimal SalesValue { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal ClosingStockValue { get; set; }
    }
}
