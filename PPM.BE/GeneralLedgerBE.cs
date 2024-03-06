using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class GeneralLedgerBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public int Account_ID { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Description { get; set; }
        public int Reference_No { get; set; }
        public string Reference_Type { get; set; }
        public string Vehicle_No { get; set; }
        public string Receipt_No { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string BalanceType { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public AccountBE SelectedAccount { get; set; }

        public GeneralLedgerBE()
        {
            SelectedAccount = new AccountBE();
        }
    }
}
