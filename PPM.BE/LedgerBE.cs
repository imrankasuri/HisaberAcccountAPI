using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class LedgerBE
    {
        public int Serial_No { get; set; }
        public DateTime Dated { get; set; }
        public string Description { get; set; }
        public string Vehicle_No { get; set; }
        public string Receipt_No { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string Reference_Type { get; set; }
        public int Reference_ID { get; set; }

    }
}
