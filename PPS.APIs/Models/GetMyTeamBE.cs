using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.Models
{
    public class GetMyTeamBE
    {
        public int ID { get; set; }
        public string AccessKey { get; set; }
        public int PageNo { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int ChildID { get; set; }
        public int LevelID { get; set; }

    }
}