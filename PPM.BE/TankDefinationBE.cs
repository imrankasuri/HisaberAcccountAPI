using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class TankDefinationBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public int TankNo { get; set; }
        public int ProductID { get; set; }
        public decimal TankFullCapacity { get; set; }
        public decimal UseableCapacity { get; set; }
        public string TankSizeDetails { get; set; }
        public string TankShape { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public ProductBE SelectedProduct { get; set; }
        public string AccessKey { get; set; }
        public int UserID { get; set; }
        public TankDefinationBE()
        {
            SelectedProduct = new ProductBE();
        }
    }
}
