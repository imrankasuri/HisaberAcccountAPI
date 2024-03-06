using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class DIPAdjustmentBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public int ProductID { get; set; }
        public decimal TotalPhysicalStock { get; set; }
        public decimal TotalSystemStock { get; set; }
        public decimal DifferenceQuantity { get; set; }
        public decimal AdjustmentRate { get; set; }
        public DateTime Dated { get; set; }
        public Boolean IsPosted { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public ProductBE SelectedProduct { get; set; }

        public decimal AdjustmentAmount { get; set; }

        public DIPAdjustmentBE()
        {
            SelectedProduct = new ProductBE();
            AdjustmentAmount = AdjustmentRate * DifferenceQuantity;
        }
    }
}
