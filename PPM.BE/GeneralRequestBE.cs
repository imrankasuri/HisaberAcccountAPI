using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class GeneralRequestBE
    {
      
        public string Android_Token { get; set; }
        public string User_Name { get; set; }
        public string User_Type { get; set; }
        public string Password { get; set; }
        public string User_Mobile { get; set; }
        public string FullName { get; set; }
        public string VoucherType { get; set; }
        public int Debit_Account_ID { get; set; }
        public int Credit_Account_ID { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrderBy { get; set; }
        public string AccessKey { get; set; }
        public string UserID { get; set; }
        public string ID { get; set; }
        public string ProductID { get; set; }
        public int AccountID { get; set; }

        public string SalePrice { get; set; }

        public DateTime Dated { get; set; }
        public int Account_Type_ID { get; set; }
        public string BalanceType { get; set; }
        public string Name { get; set; }
    }
}
