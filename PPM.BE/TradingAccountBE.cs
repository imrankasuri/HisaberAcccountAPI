using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class TradingAccountBE
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal TotalSales { get; set; }
        public decimal ProfitEarned { get; set; }
        public decimal LossSuffered { get; set; }
        public decimal DebitTotal { get; set; }
        public decimal CreditTotal { get; set; }
    }
}
