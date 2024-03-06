using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class DipReadingBE
    {
        public int ID { get; set; }
        public int AdjustmentID { get; set; }
        public int TankID { get; set; }
        public int PumpID { get; set; }
        public int TankNo { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal DIP { get; set; }
        public decimal StockLtr { get; set; }
        public DateTime Dated { get; set; }
        public Boolean IsPosted { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public List<ProductBE> ListofProducts { get; set; }

        public DipReadingBE()
        {
            ListofProducts = new List<ProductBE>();
        }
    }
}
