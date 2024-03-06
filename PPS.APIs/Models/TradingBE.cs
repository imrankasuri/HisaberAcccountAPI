using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.Models
{
    public class TradingBE
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public decimal Amount { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal? Profit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RemainingDays { get; set; }
        public string Status { get; set; }
        public Boolean ShowEncash { get; set; }
        public decimal DeductionAmount { get; set; }
    }
}