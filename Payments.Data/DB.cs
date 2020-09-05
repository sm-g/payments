using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Payments.Data
{
    public static class DB
    {
        private static readonly Lazy<PaymentsDataContext> lazyContext = new Lazy<PaymentsDataContext>(
            () => new PaymentsDataContext(
#if DEBUG
ConfigurationManager.ConnectionStrings["Debug"].ConnectionString
#else
ConfigurationManager.ConnectionStrings["Release"].ConnectionString
#endif
));

        public static PaymentsDataContext Context
        {
            get
            {
                return lazyContext.Value;
            }
        }

    }
}
