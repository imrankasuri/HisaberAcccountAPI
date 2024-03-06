using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class InvestmentSummaryBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public string PumpCode { get; set; }
        public DateTime Dated { get; set; }
        public decimal Stock_Value { get; set; }
        public decimal Credits { get; set; }
        public decimal Cash { get; set; }
        public decimal Income { get; set; }
        public decimal ExtraIncome { get; set; }
        public decimal Adjustments { get; set; }
        public decimal Gross_Investment { get; set; }
        public decimal Amount_Payable { get; set; }
        public decimal Net_Investment { get; set; }
        public decimal Expenses { get; set; }
        public decimal Net_Income { get; set; }
        public decimal Investment_Difference { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
    }
}
