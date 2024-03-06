using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class DefaultProductBE
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int MeasureUnitID { get; set; }
        public string Description { get; set; }
        public decimal Sale_Price { get; set; }
        public decimal Last_Purchase_Price { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }

        public MeasureUnitBE Measure_Unit_BE { get; set; }
    }
}
