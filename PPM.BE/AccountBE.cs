using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class AccountBE
    {
        public int ID { get; set; }
        public int Account_Type_ID { get; set; }
        public int PumpID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Mobile_No { get; set; }
        public string Email_Address { get; set; }
        public string Phone_No { get; set; }
        public Boolean Is_Deleted { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Default { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public AccountTypeBE Account_Type_BE { get; set; }

        //properties for Printing purpose only

        public int Serial_No { get; set; }
        public Decimal Balance { get; set; }
        public string BalanceType { get; set; }

        public int UserID { get; set; }
        public string AccessKey { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public string OpeningDate { get; set; }
        public AccountBE()
        {
            Account_Type_BE = new AccountTypeBE();
        }
    }
}
