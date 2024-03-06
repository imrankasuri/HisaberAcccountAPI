using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class AccountTypeBE
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Boolean Is_Deleted { get; set; }
        public Boolean Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        //properties for display purpose only.

        public decimal Balance { get; set; }
        public string BalanceType { get; set; }

    }
}
