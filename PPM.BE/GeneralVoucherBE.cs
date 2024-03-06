using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class GeneralVoucherBE
    {
        public int ID { get; set; }
        public int VoucherNo { get; set; }
        public int PumpID { get; set; }
        public DateTime Dated { get; set; }
        public string Description { get; set; }
        public int Debit_Account_ID { get; set; }
        public int Credit_Account_ID { get; set; }
        public decimal Amount { get; set; }
        public int AddedByUserID { get; set; }
        public string AddedByUser { get; set; }
        public string UpdatedByUser { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public string TimeStamp { get; set; }

        public AccountBE Debit_Account { get; set; }
        public AccountBE Credit_Account { get; set; }

        //properties for display purpose only.

        public string AccessKey { get; set; }
        public string UserID { get; set; }

        public int SerialNo { get; set; }

        public GeneralVoucherBE()
        {
            Debit_Account = new AccountBE();
            Credit_Account = new AccountBE();
        }
    }
}
