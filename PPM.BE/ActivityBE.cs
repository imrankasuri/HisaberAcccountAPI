using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAccounts.BE
{
    public class ActivityBE
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime  EndDate{ get; set; }
        public TimeSpan TimeDifference { get; set; }

        public ActivityBE(int id, string name, DateTime startDate, DateTime endDate)
        {
            ID = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            TimeDifference = EndDate - StartDate;
        }
    }
}
