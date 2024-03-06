using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class ProductBE
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public int PumpID { get; set; }
        public string Name { get; set; }
        public int MeasureUnitID { get; set; }
        public string Description { get; set; }
        public decimal Sale_Price { get; set; }
        public decimal Last_Purchase_Price { get; set; }
        public Boolean Is_Default { get; set; }
        public Boolean Is_Deleted { get; set; }
        public Boolean Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public MeasureUnitBE Measure_Unit_BE { get; set; }

        //properties for display purpose only

        public decimal Profit { get; set; }
        public int Serial_No { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public int UserID { get; set; }
        public string BalanceType { get; set; }
        public string AccessKey { get; set; }
        public List<DipReadingBE> ListofReadings{ get; set; }
        public ProductBE()
        {
            Measure_Unit_BE = new MeasureUnitBE();
            ListofReadings = new List<DipReadingBE>();
        }
    }
}
