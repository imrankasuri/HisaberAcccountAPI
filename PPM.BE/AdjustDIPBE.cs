using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class AdjustDIPBE
    {
        public int UserID { get; set; }
        public string AccessKey { get; set; }
        public DateTime Date { get; set; }
        public List<ProductDIP> ListofProductDIP { get; set; }

    }

    public class ProductDIP
    {
        public int ProductID { get; set; }
        public decimal AdjustmentRate { get; set; }

        public List<DIPReading> ListofDipReading { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPhysicalStock { get; set; }
        public decimal TotalSystemStock { get; set; }
        public decimal DifferenceQuantity { get; set; }
    }

    public class DIPReading
    {
        public int TankID { get; set; }
        public decimal Reading { get; set; }
        public decimal StockInLtr { get; set; }
        public int TankNo { get; set; }

    }

    public class ListofDates
    {
        public int SerialNo { get; set; }
        public DateTime Date { get; set; }
        public List<ProductDIP> ListofDips { get; set; }
    }

}
