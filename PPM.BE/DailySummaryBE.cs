using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class DailySummaryBE
    {
        public int SerialNo { get; set; }
        public DateTime Dated { get; set; }
        public decimal Opening_Cash { get; set; }
        public decimal Cash_Sales { get; set; }
        public decimal Credit_Sales { get; set; }
        public decimal Cash_Purchase { get; set; }
        public decimal Credit_Purchase { get; set; }
        public decimal Total_Receipts { get; set; }
        public decimal Total_Payments { get; set; }
        public decimal Total_Expenses { get; set; }
        public decimal Closing_Cash { get; set; }
    }
}
