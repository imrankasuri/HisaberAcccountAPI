using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class FuelProviderBE
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Logo_Login { get; set; }
        public string Logo_Title { get; set; }
        public string Logo_Reports { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public Boolean IsSelected { get; set; }
    }
}
