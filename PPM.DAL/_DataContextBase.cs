using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HAccounts.DAL
{
    public class _DataContextBase : System.Data.Linq.DataContext
    {
        private static string overrideConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;

        public _DataContextBase()
            : base(overrideConnectionString)
        {
        }

        public _DataContextBase(string connectionString)
            : base(overrideConnectionString)
        {
        }

        public _DataContextBase(string connectionString, System.Data.Linq.Mapping.MappingSource mappingSource)
            : base(overrideConnectionString, mappingSource)
        {
        }

        public _DataContextBase(IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource)
            : base(connection, mappingSource)
        {
        }
    }
}
