using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basement.Framework.Data;
using Basement.Framework.Data.Configuration;


namespace Basement.Framework.Unit
{
    public static class Database
    {
        public static IDatabase User
        {
            get
            {
                return new SqlServer(ConnectionConfig.Connections["User"]);
            }
        }

        public static IDatabase Order
        {
            get
            {
                return new SqlServer(ConnectionConfig.Connections["Order"]);
            }
        }

        public static IDatabase EMall
        {
            get
            {
                return new SqlServer(ConnectionConfig.Connections["EMall"]);
            }
        }

       

        public static IDatabase EMallUni
        {
            get
            {
                return new SqlServer(ConnectionConfig.Connections["EMall"]);
            }
        }
                
        
        public static IDatabase Common
        {
            get
            {
                return new SqlServer(ConnectionConfig.Connections["Uni2uni_Common"]);
            }
        }
    }
}
