using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class ProductPumpBE
    {
        public int ID { get; set; }
        public int PumpID { get; set; }
        public int Pump_No { get; set; }
        public int Product_ID { get; set; }
        public Boolean Is_Deleted { get; set; }
        public Boolean Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string TimeStamp { get; set; }
        public ProductBE Selected_Product { get; set; }

        public ProductPumpBE()
        {
            Selected_Product = new ProductBE();
        }


        //properties for display purpose only

        public int UserID { get; set; }
        public string AccessKey { get; set; }
    }
}
