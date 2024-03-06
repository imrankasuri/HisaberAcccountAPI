using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class CompanyInfoBE
    {
        public int ID { get; set; }
        public string PumpCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Logo_Login { get; set; }
        public string Logo_Title { get; set; }
        public string Logo_Reports { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public Boolean Show_Pumps { get; set; }
        public int  FuelProviderID { get; set; }

        public string PackageName { get; set; }
        public DateTime PackageExpiry { get; set; }
        //for display purpose only

        public int UserID { get; set; }

        public string AccessKey { get; set; }

        public string Password { get; set; }
    }
}
