using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.Configs;

namespace Z2.Core.DataBases
{
    public class DbUtility : IDisposable
    {
        private readonly IDbConnection _sqlConnection;
        private IDbTransaction _sqlTransaction;

        public List<SqlParamWrapper> SqlParameters { get; set; }

        public static DbUtility GetInstance(string connectionString)
        {
            return new DbUtility(connectionString);
        }

        public static DbUtility GetInstance()
        {
            return new DbUtility(DatabaseConfig.ConnectionString);
        }

        public static DbUtility GetInstanceByNoConnect()
        {
            return new DbUtility();
        }

        private DbUtility()
        {
        }

        private DbUtility(string connectionString)
        {
            _sqlConnection = SqlObject.GetInstance(connectionString).Connection;
            SqlParameters = new List<SqlParamWrapper>();
        }

        public void Dispose()
        {
            if (_sqlTransaction != null)
            {
                _sqlTransaction.Rollback();
                _sqlTransaction.Dispose();
                _sqlTransaction = null;
            }

            if (_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }

        public bool ConnectionTest(string connectionString)
        {
            IDbConnection conn = null;
            bool result;

            try
            {
                conn = SqlObject.GetInstance(connectionString).Connection;
                conn.Close();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return result;
        }

        public void BeginTransaction()
        {
            _sqlTransaction = _sqlConnection.BeginTransaction();
        }

        public void Rollback()
        {
            _sqlTransaction.Rollback();
            _sqlTransaction.Dispose();
            _sqlTransaction = null;
        }

        public void Commit()
        {
            _sqlTransaction.Commit();
            _sqlTransaction.Dispose();
            _sqlTransaction = null;
        }

        public int GetLastInsertIntId()
        {
            return GetLastInsertId<int>();
        }
        public long GetLastInsertLongId()
        {
            return GetLastInsertId<long>();
        }

        public T GetLastInsertId<T>() where T : struct, IComparable, IConvertible
        {
            T newId = default(T);
            var sql = "";
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    sql = "select IDENT_CURRENT";
                    break;
                case DatabaseEnum.MySql:
                    sql = "select last_insert_id();";
                    break;
            }

            var lastObj = ExecuteScalar(sql);
            if (lastObj != null)
            {
                newId = (T)Convert.ChangeType(lastObj, typeof(T));
            }
            return newId;
        }

        public int ExecuteNonQuery(string sql)
        {
            var command = _sqlConnection.CreateCommand();

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }
            command.CommandTimeout = 60 * 1000 * 60;

            _setSqlParameters(command);

            command.CommandText = sql;
            var effRows = command.ExecuteNonQuery();
            command.Dispose();

            SqlParameters = new List<SqlParamWrapper>();
            return effRows;
        }

        public int ExecuteNonQuery(string sql, object model)
        {
            var effRows = _sqlConnection.Execute(sql, model, _sqlTransaction);
            return effRows;
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string sql)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }
            
            var reader = command.ExecuteReader();
            command.Dispose();
            try
            {
                var result = DataReaderConverter.GetList(reader);
                return result;
            }
            finally
            {
                reader.Close();
                SqlParameters = new List<SqlParamWrapper>();
            }
        }

        public object ExecuteScalar(string sql)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }

            var obj = command.ExecuteScalar();
            command.Dispose();
            try
            {
                return obj;
            }
            finally
            {
                SqlParameters = new List<SqlParamWrapper>();
            }
        }

        public DataSet GetDataSet(string sql)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }

            var adapter = SqlObject.CreateAdapter(command);
            DataSet dtSet = new DataSet();
            adapter.Fill(dtSet);
            SqlParameters = new List<SqlParamWrapper>();
            return dtSet;
        }

        public void ExecuteReaderModel(string sql, Object model)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }
            var reader = command.ExecuteReader();
            command.Dispose();
            try
            {
                DataReaderConverter.ConvertModel(reader, model);
            }
            finally
            {
                reader.Close();
                SqlParameters = new List<SqlParamWrapper>();
            }
        }

        public void ExecuteReaderModelList(string sql, Object list)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }
            var reader = command.ExecuteReader();
            command.Dispose();
            try
            {
                DataReaderConverter.ConvertModelList(reader, list);
            }
            finally
            {
                reader.Close();
                SqlParameters = new List<SqlParamWrapper>();
            }
        }
   

        public IEnumerable<T> ReaderModelList<T>(string sql)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }
            var reader = command.ExecuteReader();
            command.Dispose();
            try
            {
                var list = new List<T>();
                DataReaderConverter.ConvertModelList(reader,list);
                return list;
            }
            finally
            {
                reader.Close();
                SqlParameters = new List<SqlParamWrapper>();
            }
        }

        public T ReaderModel<T>(string sql, object parameters)
        {
            try
            {
                var r = _sqlConnection.QuerySingleOrDefault<T>(sql, parameters, _sqlTransaction);
                return r;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public IEnumerable<T> ReaderModelList<T>(string sql,object parameters)
        {
            try
            {
                var r = _sqlConnection.Query<T>(sql, parameters, _sqlTransaction);
                return r;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        public void AddParameter(string parameterName, object value)
        {
            SqlParameters.Add(new SqlParamWrapper(parameterName.Trim(), value));
        }

        private void _setSqlParameters(IDbCommand command)
        {
            command.Parameters.Clear();

            if (SqlParameters != null)
            {
                foreach (var item in SqlParameters)
                {
                    command.Parameters.Add(DbParamConverter.Convert(item));
                }
            }
        }
    }
}
