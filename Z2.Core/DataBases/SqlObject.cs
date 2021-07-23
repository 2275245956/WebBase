using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.Utility;

namespace Z2.Core.DataBases
{
    public class SqlObject
    {
        public IDbConnection Connection { get; set; }

        public static SqlObject GetInstance(string connectionString)
        {
            return new SqlObject(connectionString);
        }

        private SqlObject(string connectionString)
        {
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    Connection = new SqlConnection();
                    break;
                case DatabaseEnum.MySql:
                    Connection = new MySqlConnection();
                    break;
            }

            Connection.ConnectionString = connectionString;
            Connection.Open();
        }

        public static IDbDataAdapter CreateAdapter(IDbCommand command)
        {
            IDbDataAdapter adapter = null;
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    adapter = new SqlDataAdapter((SqlCommand)command);
                    break;
                case DatabaseEnum.MySql:
                    adapter = new MySqlDataAdapter((MySqlCommand)command);
                    break;
                default:
                    break;
            }
            return adapter;
        }
    }

    public class SqlParamWrapper
    {
        public string ParameterName { get; set; }
        public Type ParameterType { get; set; }
        // TODO: SqlParamWrapper IsNullable
        //bool IsNullable { get; set; }
        public object Value { get; set; }

        public SqlParamWrapper(string parameterName, object value)
        {
            ParameterName = parameterName;

            if (value != null)
            {
                ParameterType = value.GetType();
            }

            Value = value;
        }
    }


    public static class DbParamConverter
    {
        public static IDbDataParameter Convert(SqlParamWrapper parameter)
        {
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    if (parameter.Value == null)
                    {
                        return new SqlParameter('@' + parameter.ParameterName, DBNull.Value);
                    }
                    else
                    {
                        return new SqlParameter('@' + parameter.ParameterName, parameter.Value);
                    }
                case DatabaseEnum.MySql:
                    if (parameter.Value == null)
                    {
                        return new MySqlParameter('?' + parameter.ParameterName, DBNull.Value );
                    }
                    else
                    {
                        return new MySqlParameter('?' + parameter.ParameterName, parameter.Value);
                    }
            }

            return null;
        }
    }


    public static class DataReaderConverter
    {
        public static IEnumerable<Dictionary<string, object>> GetList(IDataReader reader)
        {
            var result = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var item = new Dictionary<string, object>();

                for (int lp = 0; lp < reader.FieldCount; lp++)
                {
                    item.Add(reader.GetName(lp), reader.GetValue(lp));
                }

                result.Add(item);
            }

            return result;
        }

        public static void ConvertModel(IDataReader reader, Object model)
        {
            var propertys = model.GetType().GetProperties();
            if (reader.Read())
            {
                for (int lp = 0; lp < reader.FieldCount; lp++)
                {
                    foreach (var item in propertys)
                    {
                        if (item.Name == reader.GetName(lp))
                        {
                            _SetModelValue(model, item, reader.GetValue(lp));
                            break;
                        }
                    }
                }
            }
        }

        public static void ConvertModelList(IDataReader reader, Object lstModel)
        {
            var lstType = lstModel.GetType();
            var type = lstType.GetGenericArguments()[0];
            var propertys = type.GetProperties();

            while (reader.Read())
            {
                var objItem = System.Activator.CreateInstance(type);
                for (int lp = 0; lp < reader.FieldCount; lp++)
                {
                    foreach (var item in propertys)
                    {
                        if (item.Name == reader.GetName(lp))
                        {
                            try
                            {
                                _SetModelValue(objItem, item, reader.GetValue(lp));
                            }
                            catch (Exception e)
                            {
                                //Console.WriteLine(item.Name);
                                throw e;
                            }
                            break;
                        }
                    }
                }
                lstType.GetMethod("Add").Invoke(lstModel, new object[] { objItem });
            }
        }

        private static void _SetModelValue(object model, System.Reflection.PropertyInfo pi, object value)
        {
            if (value == DBNull.Value)
            {
                return;
            }
            if (!pi.CanWrite)
            {
                return;
            }
            //switch (pi.PropertyType.ToString().ToLower())
            //{
            //    case "system.string":
            //        pi.SetValue(model, value.ToString(), null);
            //        break;
            //    case "system.int32":
            //        pi.SetValue(model, Convert.ToInt32(value), null);
            //        break;
            //    case "system.decimal":
            //        pi.SetValue(model, Convert.ToDecimal(value), null);
            //        break;
            //    case "system.datetime":
            //        pi.SetValue(model, Convert.ToDateTime(value), null);
            //        break;
            //    case "system.float":
            //        pi.SetValue(model, Convert.ToDecimal(value), null);
            //        break;
            //    case "system.boolean":
            //        pi.SetValue(model, Convert.ToBoolean(value), null);
            //        break;

            //}

            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "int32")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToInt32(value), null);
                    }
                }

                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "string")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToString(value), null);
                    }
                }

                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "decimal")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        decimal o;
                        if (decimal.TryParse(string.Format("{0}", value), out o))
                        {
                            pi.SetValue(model, o, null);
                        }
                        else
                        {
                            pi.SetValue(model, Converts.ToTryDecimal(value, 0), null);
                        }
                    }
                }

                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "datetime")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToDateTime(value), null);
                    }
                }

                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "float")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToDecimal(value), null);
                    }
                }

                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "boolean")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToBoolean(value), null);
                    }
                }
                if (pi.PropertyType.GetGenericArguments()[0].Name.ToLower() == "int64")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, null, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToInt64(value), null);
                    }
                }
            }
            else
            {
                if (pi.PropertyType.Name.ToLower() == "int32")
                {
                    pi.SetValue(model, Convert.ToInt32(value), null);
                }
                if (pi.PropertyType.Name.ToLower() == "string")
                {
                    pi.SetValue(model, Convert.ToString(value), null);
                }
                if (pi.PropertyType.Name.ToLower() == "decimal")
                {
                    pi.SetValue(model, Converts.ToTryDecimal(value, 0), null);
                }
                if (pi.PropertyType.Name.ToLower() == "datetime")
                {
                    pi.SetValue(model, Convert.ToDateTime(value), null);
                }
                if (pi.PropertyType.Name.ToLower() == "float")
                {
                    pi.SetValue(model, Convert.ToDecimal(value), null);
                }

                if (pi.PropertyType.Name.ToLower() == "boolean")
                {
                    pi.SetValue(model, Convert.ToBoolean(value), null);
                }
                if (pi.PropertyType.Name.ToLower() == "int64")
                {

                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, 0, null);
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ToInt64(value), null);
                    }
                    //pi.SetValue(model, Convert.ToInt64(value), null);
                }
                if (pi.PropertyType.Name.ToLower() == "byte[]")
                {
                    if (value == DBNull.Value)
                    {
                        pi.SetValue(model, 0, null);
                    }
                    else
                    {
                        pi.SetValue(model, (byte[])value, null);
                    }
                }
            }
        }
    }
}
