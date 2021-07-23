using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.Security;

namespace Z2.Core.Configs
{
    public class DBConnection
    {
        public static bool Encrypt { get; set; }
        public DBConnection(bool encrypt)
        {
            Encrypt = encrypt;
        }
        public static string connectionString
        {
            get
            {
                string connection = System.Configuration.ConfigurationManager.ConnectionStrings["Z2DbContext"].ConnectionString;
                if (Encrypt == true)
                {
                    return DESEncryptHelper.Decrypt(connection);
                }
                else
                {
                    return connection;
                }
            }
        }
    }
}
