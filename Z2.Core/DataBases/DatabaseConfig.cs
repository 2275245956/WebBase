using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.Configs;

namespace Z2.Core.DataBases
{
    static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                return DBConnection.connectionString;
            }
        }

        public static DatabaseEnum DatabaseType
        {
            get
            {
                switch (ConfigWrap.GetValue("DatabaseType"))
                {
                    case "SqlServer":
                        return DatabaseEnum.SqlServer;
                    case "MySql":
                        return DatabaseEnum.MySql;
                    default:
                        throw new Exception("アプリケーション構成ファイルのAppSettings.DatabaseTypeに誤りがあります。");
                }
            }
        }
    }

    public enum DatabaseEnum
    {
        SqlServer,
        MySql
    }
}
