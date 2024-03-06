using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class PumpReadingBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public int PumpMachineID { get; set; }
        public int Pump_No { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Last_Reading { get; set; }
        public decimal Current_Reading { get; set; }
        public decimal Returned { get; set; }
        public DateTime Dated { get; set; }
        public int Invoice_ID { get; set; }
        public Boolean Is_Active { get; set; }
        public Boolean Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        //property for display purpose only

        public decimal UsedQuantity { get; set; }

        public PumpReadingBE()
        {
            UsedQuantity = (Current_Reading - Last_Reading) - Returned;
        }
    }
}
